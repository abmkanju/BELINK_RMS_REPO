using ExcelDataReader;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ServiceModel;
using Pharmacy.Infrastructure;
using Pharmacy.UI.ManageOfflineTask;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Pharmacy.UI.Franchise_Deployement
{
    public partial class frmMappingProgress : Form
    {
        UnitOfWork unitOfWork;
        public string _username;
        public string _password;
        public long _practiseid;
        public string _excelfilelocation;
        public frmMappingProgress()
        {
            InitializeComponent();
        }
        public frmMappingProgress(string Username ,  string Password , long PractiseId , string ExcelFileLocation)
        {
            InitializeComponent();
            this._username = Username;
            this._password = Password;
            this._practiseid = PractiseId;
            this._excelfilelocation = ExcelFileLocation;
            Task.Run(() => StartMapping());
        }
        public frmMappingProgress(string Username, string Password, long PractiseId, string ExcelFileLocation , bool NewDeployement)
        {
            InitializeComponent();
            this._username = Username;
            this._password = Password;
            this._practiseid = PractiseId;
            this._excelfilelocation = ExcelFileLocation;
            string query = "DROP DATABASE" + Shared.GetConnectionBuilder().InitialCatalog;
            Process(query);
            new frmWaiting().ShowDialog();
            ItemsStockEntry(ExcelFileLocation);
        }
        public DataTable ReadExcel(string filePath, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            try
            {
                FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                ExcelDataReader.IExcelDataReader excelReader = ExcelDataReader.ExcelReaderFactory.CreateBinaryReader(stream);
                excelReader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
                var value = excelReader.RowCount;
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };
                var dataSet = excelReader.AsDataSet(conf);
                dtexcel = dataSet.Tables[0];
                excelReader.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            //if (fileExt.CompareTo(".xls") == 0)//compare the extension of the file
            //{
            //    //conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=2';";//for below excel 2007
            //    conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filePath + "';Extended Properties= Excel 8.0;";//for below excel 2007
            //}

            //else
            //    conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1';";//for above excel 2007
            //using (OleDbConnection con = new OleDbConnection(conn))
            //{
            //    try
            //    {
            //        //OleDbDataAdapter oleAdpt = new OleDbDataAdapter(@"select * from [CourseList$]", con);//here we read data from sheet1
            //        OleDbDataAdapter oleAdpt = new OleDbDataAdapter(@"select * from [Sheet1$]", con);//here we read data from sheet1

            //        //oleAdpt.TableMappings.Add("Table", "TestTable");
            //        //con.Open();
            //        //DataTable schemaTable = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null });
            //        //foreach (DataRow dataRow in schemaTable.Rows)
            //        //{
            //        //    string tableName = dataRow["TABLE_NAME"].ToString();	// gets the sheet name from each schema entry
            //        //    if (!tableName.EndsWith("_"))
            //        //    {
            //        //        // some code to display the name
            //        //    }
            //        //}
            //        oleAdpt.Fill(dtexcel);//fill excel data into dataTable
            //        con.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message.ToString());
            //    }
            //}
            return dtexcel;
        }
        private void ItemsStockEntry(string path)
        {
            
            DataTable dt = ReadExcel(path, ".xls");
            LoadCustomStock(dt);
        }
        public string GetExcelStr(DataTable dtSource, string colName, DataRow r)
        {
            string result = "";
            if (dtSource.Columns.Contains(colName))
                result = r[colName].ToString().Trim();
            //result = result != null && result != string.Empty ? result : "";
            return result;
        }
        public int GetExcelInt(DataTable dtSource, string colName, DataRow r)
        {
            int result = 0;
            if (dtSource.Columns.Contains(colName))
                int.TryParse(r[colName].ToString(), out result);
            return result;
        }
        public bool GetExcelBool(DataTable dtSource, string colName, DataRow r)
        {
            bool result = false;
            if (dtSource.Columns.Contains(colName))
            {
                if (!string.IsNullOrEmpty(r[colName].ToString()))
                {
                    result = r[colName].ToString().ToLower() == "true" ? true : false;
                }
            }
            return result;
        }
        public TransactionScope CreateTransactionScope(TimeSpan timeout)
        {
            // or for netcore / .net5+ use these names instead:
            //    s_cachedMaxTimeout
            //    s_maximumTimeout
            SetTransactionManagerField("_cachedMaxTimeout", true);
            SetTransactionManagerField("_maximumTimeout", timeout);
            return new TransactionScope(TransactionScopeOption.RequiresNew, timeout);
        }
        private void SetTransactionManagerField(string fieldName, object value)
        {
            typeof(TransactionManager).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, value);
        }
        private void LoadCustomStock(DataTable dtSource)
        {
            try
            {
                User userModel;
                string suppName = "";
                string manufactureName = "";
                string categoryName = "";
                bool IsSuppliersExists = false;
                bool IsManufacturerExists = false;
                bool IsCategoryExists = false;
                #region LoadItems
                double retailPrice = 0;
                double costPrice = 0;
                int Quantity = 0;
                List<Item> il = new List<Item>();
                using (unitOfWork = new UnitOfWork())
                {
                    userModel = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    

                    //List<ItemExcelVM> items = SharedFunctions.DataTableToModel<ItemExcelVM>(dtSource);

                    foreach (DataRow r in dtSource.Rows)
                    {
                        suppName = "";
                        manufactureName = "";
                        categoryName = "";
                        Quantity = 0;
                        int Unique_Id = 0;
                        Item objItem = new Item();
                        objItem.ItemName = r["ItemName"].ToString().Trim();
                        objItem.Unique_itemId = Convert.ToInt32(r["Unique_Id"].ToString());
                        objItem.Barcode = GetExcelStr(dtSource, "Barcode", r);
                        if (objItem.Unique_itemId == 0)
                        {
                            continue;
                        }
                        Item itemFound = il.Where(i => i.Unique_itemId.Equals(Unique_Id)).FirstOrDefault();
                        int.TryParse(r["Quantity"].ToString(), out Quantity);
                        double.TryParse(r["CostPrice"].ToString(), out costPrice);
                        double.TryParse(r["RetailPrice"].ToString(), out retailPrice);
                        if (itemFound == null)
                        {
                            il.Add(itemFound);
                        }
                    }
                    
                }
                #endregion

                #region SaveStock
                List<Item> stockItemList = il;
                if (stockItemList != null && stockItemList.Count() > 0)
                {
                    var stockBatchItems = SharedFunctions.ChunkBy(stockItemList, SharedVariables.ItemBatch);
                    int docNo = 100001;
                    foreach (var i in stockBatchItems)
                    {
                        using (var ts = CreateTransactionScope(TimeSpan.FromMinutes(60)))
                        {
                            using (unitOfWork = new UnitOfWork())
                            {
                                MakeOpeningStockEntry(i, docNo, userModel);
                                ts.Complete();
                                docNo += 1;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void MakeOpeningStockEntry(List<Item> objItem, int DocNo, User usermodel)
        {
            Stock objStock = new Stock();
            objStock.DocumentNo = DocNo;
            objStock.Supplier = null;
            objStock.SupplierInvoiceNo = "";
            objStock.SupplierIvoiceDate = DateTime.Now;
            objStock.IsAutoInsertedStock = false; // if need to sync stocks, then set this to false : else set this variable to true.
            objStock.GrandTotal = objItem.Sum(x => x.OpeningStock) * objItem.Sum(x => x.UnitCostPrice);
            objStock.TotalPaid = 0;
            objStock.SupplierInvoiceNo = "";
            objStock.ImagePath = null;
            objStock.StockDate = DateTime.Now;
            objStock.CreateDate = DateTime.Now;
            objStock.UpdateDate = DateTime.Now;
            objStock.IsActive = true;
            objStock.IsNew = true;
            objStock.IsUpdate = false;
            objStock.IsSynced = false;
            objStock.DocumentNo = DocNo;
            objStock.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
            objStock.StockItems = new List<StockItem>();
            foreach (var item in objItem)
            {
                StockItem objStockItem = new StockItem();
                objStockItem.Batch = new Batch();
                objStockItem.Batch.BatchName = "Opening Stock";
                objStockItem.Batch.Expiry = null;
                objStockItem.Batch.CreatedAt = DateTime.Now;
                objStockItem.Batch.UpdatedAt = DateTime.Now;
                objStockItem.Batch.IsNew = true;
                objStockItem.Batch.IsActive = true;
                objStockItem.Batch.IsSynced = false;
                objStockItem.Batch.IsUpdate = false;
                objStockItem.Unit = 1; // "For Stock in units"
                objStockItem.Quantity = item.OpeningStock;
                objStockItem.BonusQuantity = 0;
                objStockItem.UnitCost = item.UnitCostPrice;
                objStockItem.TotalCost = objStockItem.UnitCost * objStockItem.Quantity;
                objStockItem.NetValue = objStockItem.TotalCost;
                objStockItem.RetailPrice = item.RetailPrice;
                objStockItem.BonusDiscount = 0;
                objStockItem.Discount = 0;
                objStockItem.DiscountType = 0;
                objStockItem.PercDiscType = 0;
                objStockItem.DiscountValue = 0;
                objStockItem.SalesTax = 0;
                objStockItem.SalesTaxType = 0;
                objStockItem.PercSalesTaxType = 0;
                objStockItem.SalesTaxValue = 0;
                objStockItem.Item = unitOfWork.ItemRspository.GetById(item.ItemID);// item;// unitOfWork.ItemRspository.GetById(ItemId);
                objStockItem.CreatedAt = DateTime.Now;
                objStockItem.UpdatedAt = DateTime.Now;
                objStockItem.IsActive = true;
                objStockItem.IsNew = true;
                objStockItem.IsSynced = false;
                objStockItem.IsUpdate = false;
                objStock.StockItems.Add(objStockItem);
            }
            unitOfWork.StockRepository.Insert(objStock);
            unitOfWork.Save();
            //SOT
            //--------------Map Items
            List<m_Item> itemList = objItem.Select(i => new m_Item()
            {
                id = i.ItemID,
                desktop_id = i.ItemID,
                item_name = i.ItemName,
                barcode = i.Barcode,
                //rack = i.Rack.Name,
                stocking_unit = i.Unit,
                re_ordering_level = i.ReOrderingLevel,
                conversion_unit = i.ConversionUnit,
                retail_price = (float)i.RetailPrice,
                generic_name = i.ChecmicalName,
                unit_cost = (float)i.UnitCostPrice,
                narcotic = i.IsNarcotic,
                item_classification_id = i.Category != null ? (int)i.Category.CategoryId : 0,
                manufacturer_id = i.Manufacturer != null ? i.Manufacturer.ManufacturerId : 0,
                user_id = null, //i.User.UserId,
                created_at = i.CreateDate,
                updated_at = i.UpdateDate,
                deleted_at = i.DeletedAt,
                practice_id = 0,
                inventory_supplier_ids = i.Suppliers != null ? i.Suppliers.Select(s => (int)s.SupplierID).ToList() : new List<int> { }
            }).OrderBy(i => i.desktop_id).ToList();
            //OfflineJsonTask.SaveOfflineTask(unitOfWork, itemList, ModuleName.BulkFileItems, ModuleAction.Create, 0);
            //--------------Map Batches
            List<m_Batch> batchList = objStock.StockItems.Select(x => x.Batch).ToList().Select(b => new m_Batch()
            {
                id = b.BatchId,
                desktop_id = b.BatchId,
                name = b.BatchName,
                expiry = b.Expiry,
                created_at = b.CreatedAt,
                updated_at = b.UpdatedAt,
                practice_id = 0,
                is_new = b.IsNew,
                is_update = b.IsUpdate,
                _destroy = !b.IsActive,
            }).ToList();
            OfflineJsonTask.SaveOfflineTask(unitOfWork, batchList, ModuleName.BulkFileBatches, ModuleAction.Create, 0);
            //--------------Map Stock
            m_Stock stock = new m_Stock()
            {
                id = (int)objStock.StockId,
                desktop_id = (int)objStock.StockId,
                document_number = (int)objStock.DocumentNo,
                invoice_number = objStock.SupplierInvoiceNo,
                supplier_invoice_date = objStock.SupplierIvoiceDate,
                grand_total = (float)objStock.GrandTotal,
                paid_total = (float)objStock.TotalPaid,
                stock_date = objStock.StockDate,
                deleted_at = objStock.DeletedAt,
                order_id = (objStock.PurchaseOrder == null ? null : (int?)objStock.PurchaseOrder.PurchaseOrderId),
                user_id = objStock.User.UserId,
                practice_id = 0,
                inventory_supplier_id = objStock.Supplier == null ? null : (int?)objStock.Supplier.SupplierID,
                which_type = "RMS",
                created_at = objStock.CreateDate,
                updated_at = objStock.UpdateDate,
                stock_items_attributes = (objStock.StockItems.Select(si => new m_StockItem()
                {
                    id = (int)si.StockItemId,
                    desktop_id = (int)si.StockItemId,
                    item_id = si.Item.ItemID,
                    quantity = si.Quantity + si.BonusQuantity,
                    per_unit_cost = (float)si.UnitCost,
                    total_value = (float)si.TotalCost,
                    unit_sales_value = (float)si.RetailPrice,
                    batch_id = si.Batch.BatchId,
                    discount = (float)si.Discount,
                    discount_value = 0,
                    sale_value = 0,
                    unit = si.Unit,
                    store_id = null,
                    stock_discount_type = si.DiscountType,
                    sales_tax = (float)si.SalesTax,
                    sales_tax_type = si.SalesTaxType,
                    net_value = (float)si.NetValue,
                    created_at = si.CreatedAt,
                    updated_at = si.UpdatedAt,
                    deleted_at = si.DeletedAt,
                    _destroy = si.IsActive ? "false" : "1"
                })).ToList()
            };
            OfflineJsonTask.SaveOfflineTask(unitOfWork, stock, ModuleName.FileStock, ModuleAction.Create, 0);
        }
        public async Task Config()
        {
            string Query = "update AdminPharmacySettings set OnlineWebUsername = '" + _username + "' , OnlineWebPassword = '" + _password + "' ,  PracticeId = " + _practiseid + " , IsOnlineIntegEnable =  1;";
            await Process(Query);
        }
        private void deactivateItems(DataTable dt)
        {
            string Ids = "";
            foreach(DataRow row in dt.Rows)
            {
                long Id = Convert.ToInt64(row["ItemID"].ToString());
                Ids = Ids + "," + Id;
            }
            Ids = Ids.Substring(1, Ids.Length);
            string Query = "update Items set IsActive=0 where ItemID not in ("+ Ids +");";
            Process(Query);
        }
        public async Task StartMapping()
        {
            Action();
            this.Invoke(new Action(() =>
            {
                if (progressBar1.Value <= progressBar1.Maximum)
                {
                    label1.Text = "Configuring Franchise Credentials ....";
                    progressBar1.Style = ProgressBarStyle.Marquee;
                }
            }));
            await Task.Run(() => Config());
            this.Invoke(new Action(() =>
            {
                if (progressBar1.Value <= progressBar1.Maximum)
                {
                    label1.Text = "Completed";
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Value = progressBar1.Maximum;
                    MessageBox.Show("Franchise Configured Successfully...");
                    this.Close();
                }
            }));
        }

        public DataTable ReadExcel(string filePath)
        {
            this.Invoke(new Action(() =>
            {
                if (progressBar1.Value <= progressBar1.Maximum)
                {
                    label1.Text = "Processing Excel File....";
                    progressBar1.Style = ProgressBarStyle.Marquee;
                }
            }));

            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            try
            {
                FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                ExcelDataReader.IExcelDataReader excelReader = ExcelDataReader.ExcelReaderFactory.CreateBinaryReader(stream);
                excelReader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
                var value = excelReader.RowCount;
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };
                var dataSet = excelReader.AsDataSet(conf);
                dtexcel = dataSet.Tables[0];
                excelReader.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            return dtexcel;
        }

        private async Task Action()
        {
            DataTable dt = ReadExcel(_excelfilelocation);
            
            this.Invoke(new Action(() =>
            {
                if (progressBar1.Value <= progressBar1.Maximum)
                {
                    label1.Text = "Mapping Data...";
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = dt.Rows.Count + 1;
                    progressBar1.Value = 0;
                }
            }));

            var tasks = dt.Rows.Cast<DataRow>().Select(row =>
            {
                return Task.Run(async () =>
                {
                    long Unique_Id = Convert.ToInt64(row["Unique_itemId"].ToString());
                    long Id = Convert.ToInt64(row["ItemID"].ToString());
                    int ConvUnit = Convert.ToInt32(row["ConversionUnit"].ToString());
                    int NoOfStrip = Convert.ToInt32(row["NoOfStrips"].ToString());
                    string newname = row["DefaultName"].ToString();
                    string name = newname;
                    if (Id == null)
                    {
                        if (newname.Contains("\'"))
                        {
                            name = "";
                            foreach (char i in newname)
                            {
                                if (i == '\'')
                                {
                                    name = name + i + "\'";
                                }
                                else
                                {
                                    name = name + i;
                                }
                            }
                        }
                        string Query = "insert into Items" +
                        "      (Unique_itemId, ItemName, ConversionUnit, NoOfStrips , RetailPrice , OpeningStock , UnitCostPrice , IsNarcotic, MappedCategoryId , MappedProductId , CreateDate , UpdateDate , IsDefault , IsDefaultLoaded , IsSyncable , IsFileItem , IsActive ,IsNew , IsUpdate , IsSynced , IsApproved , User_UserId) " +
                        "values("+Unique_Id+",'"+ name+"',"+ ConvUnit+","+NoOfStrip+",0,0,0,0,0,0,'"+DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "',0,0,1,0,1,1,0,0,0,"+ SharedVariables.LoggedInUser.UserId+")";
                        await Process(Query);
                    }
                    else
                    {
                        if (newname.Contains("\'"))
                        {
                            name = "";
                            foreach (char i in newname)
                            {
                                if (i == '\'')
                                {
                                    name = name + i + "\'";
                                }
                                else
                                {
                                    name = name + i;
                                }
                            }
                        }
                        string Query = "update Items set Unique_itemId= " + Unique_Id + ", ItemName='" + name + "', ConversionUnit=" + ConvUnit + ", NoOfStrips=" + NoOfStrip + "  where ItemID =" + Id;
                        await Process(Query);
                    }
                    
                });
            });

            await Task.WhenAll(tasks);
            deactivateItems(dt);
        }

        private async Task Process(string query)
        {
            try
            {
                string Query = query;
                using (SqlConnection con = new SqlConnection(Shared.GetConnectionString()))
                {
                    using (SqlCommand com = new SqlCommand(Query, con))
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        int i = await com.ExecuteNonQueryAsync();
                        if (i > 0)
                        {
                            this.Invoke(new Action(() =>
                            {
                                if (progressBar1.Value < progressBar1.Maximum)
                                {
                                    progressBar1.Value += 1;
                                }
                            }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void frmMappingProgress_Shown(object sender, EventArgs e)
        {
            
        }

        private void frmMappingProgress_Load(object sender, EventArgs e)
        {
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void frmMappingProgress_VisibleChanged(object sender, EventArgs e)
        {
           
        }
    }
}
