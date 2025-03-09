using ExcelDataReader;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ServiceModel;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.ManageOfflineTask;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Pharmacy.UI
{
    public partial class frmWaiting : Form
    {
        UnitOfWork unitOfWork;
        public frmWaiting()
        {
            InitializeComponent();
        }

        private void frmWaiting_Load(object sender, EventArgs e)
        {
        }

        private void bgDataLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Comp_Timer = new System.Timers.Timer();
            //Comp_Timer.Enabled = true;
            //Comp_Timer.Interval = 3000;
            //Comp_Timer.Elapsed += new System.Timers.ElapsedEventHandler(Comp_Timer_Elapsed);
            //Comp_Timer.Start();
            //Thread.Sleep(3000);

            //pbLoading.Visible = false;
            //lblStatus.Text = "Done!";
            //pbCompleted.Visible = true;
            //pbCompleted.BringToFront();
            //pnlDisplay.Invalidate();
            //panel1.Invalidate();

            this.Invoke(new Action(() =>
             {
                 pbLoading.Visible = false;
                 lblStatus.Text = "Done!";
                 pbCompleted.Visible = true;
                 pbCompleted.BringToFront();
                 pnlDisplay.Invalidate();
                 this.Invalidate();
                 lblStatus.Refresh();
                 pnlDisplay.Refresh();
                 Thread.Sleep(3000);
                 this.Close();
             }));
        }

        private void Comp_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    pbLoading.Visible = false;
                    lblStatus.Text = "Done!";
                    pbCompleted.Visible = true;
                    pbCompleted.BringToFront();
                    pnlDisplay.Invalidate();
                    this.Close();
                }));

            }
            else
            {
                pbLoading.Visible = false;
                lblStatus.Text = "Done!";
                pbCompleted.Visible = true;
                pbCompleted.BringToFront();
                pnlDisplay.Invalidate();
                this.Close();
            }
        }

        private void bgDataLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            //Thread.Sleep(5000);
            //return;
            try
            {
                bool proceedToLoadData = false;
                using (unitOfWork = new UnitOfWork())
                {
                    if (unitOfWork.SupplierRepository.SuppliersExist() && unitOfWork.ItemRspository.ItemsCount() > 1)
                    {
                        MessageBox.Show("Data already loaded", "Data exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (!unitOfWork.SupplierRepository.SuppliersExist() || unitOfWork.ItemRspository.ItemsCount() == 1)
                    {
                        proceedToLoadData = true;
                    }
                }
                if (proceedToLoadData)
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string path = "";
                    int pracId = this.GetPriaceId();
                    if (pracId > 0 || pracId == -1) // -1 is for testing pcs:
                    {
                        if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable && false)
                        {
                            if (!SharedVariables.AdminPractiseSetting.IsFranchiseMapped)
                            {
                                path = Path.Combine(baseDir, "Data", "pharmacy_Def_2.xls");
                                DataTable dt1 = ReadExcel(path, ".xls");
                                loadItemsData_FromExcelSheet3(dt1);
                            }

                            //now compare the mapped file
                            if (pracId == -1 || pracId >= 0)
                            {
                                path = Path.Combine(baseDir, "Data", "Khizar Hospital.xls");
                                DataTable dt = ReadExcel(path, ".xls");
                                loadItemsData_FromExcelSheet4(dt);
                            }

                            //else if (pracId == -1) //-1 is for local testing pcs: put practice id of all pharmacies using same data sheet for default items.
                            //{
                            //    path = Path.Combine(baseDir, "Data", "pharmacy_Def_2.xls");
                            //    DataTable dt = ReadExcel(path, ".xls");
                            //    loadItemsData_FromExcelSheet3(dt);
                            //}
                            //else
                            //{
                            //    path = Path.Combine(baseDir, "Data", "ItemsSheet_1.xls");
                            //    DataTable dt = ReadExcel(path, ".xls");
                            //    LoadCustomStock(dt);
                            //}
                            //else if (pracId == 2) // khizar hospital
                            //{
                            //    path = Path.Combine(baseDir, "Data", "Khizar Hospital.xls");
                            //    DataTable dt = ReadExcel(path, ".xls");
                            //    loadItemsData_KhizarPharmacy(dt);
                            //}
                            //else if (pracId == 3)
                            //{
                            //    //path = Path.Combine(baseDir, "Data", "EverCarePharmacy.xls");
                            //    path = Path.Combine(baseDir, "Data", "EverCarePharmacy.xls");
                            //    DataTable dt = ReadExcel(path, ".xls");
                            //    loadItemsData_EverCarePharmacy(dt);
                            //}
                            //else if (pracId == 9) // hameed hospital pharmacy
                            //{
                            //    path = Path.Combine(baseDir, "Data", "Hameed_Hospital_Pharmacy_updaeted.xls");
                            //    DataTable dt = ReadExcel(path, ".xls");
                            //    loadItemsData_HameedHospitalPharmacy(dt);
                            //}

                            //update practiceID
                            int m = updateDB("Update AdminPractiseSettings set IsFranchiseMapped =" + 1 + ",IsUniqueIdMapped=" + 1);
                        }
                        else
                        {
                            /*if (pracId == -1) //-1 is for local testing pcs: put practice id of all pharmacies using same data sheet for default items.
                            {
                                path = Path.Combine(baseDir, "Data", "pharmacy_Def_2.xls");
                                DataTable dt = ReadExcel(path, ".xls");
                                loadItemsData_FromExcelSheet3(dt);
                            }
                            else
                            {
                                path = Path.Combine(baseDir, "Data", "ItemsSheet_1.xls");
                                DataTable dt = ReadExcel(path, ".xls");
                                LoadCustomStock(dt);
                            }*/
                            //else if (pracId == 2) // khizar hospital
                            //{
                            //    path = Path.Combine(baseDir, "Data", "Khizar Hospital.xls");
                            //    DataTable dt = ReadExcel(path, ".xls");
                            //    loadItemsData_KhizarPharmacy(dt);
                            //}
                            //else if (pracId == 3)
                            //{
                            //    //path = Path.Combine(baseDir, "Data", "EverCarePharmacy.xls");
                            //    path = Path.Combine(baseDir, "Data", "EverCarePharmacy.xls");
                            //    DataTable dt = ReadExcel(path, ".xls");
                            //    loadItemsData_EverCarePharmacy(dt);
                            //}
                            //else if (pracId == 9) // hameed hospital pharmacy
                            //{
                            //    path = Path.Combine(baseDir, "Data", "Hameed_Hospital_Pharmacy_updaeted.xls");
                            //    DataTable dt = ReadExcel(path, ".xls");
                            //    loadItemsData_HameedHospitalPharmacy(dt);
                            //}
                        }
                    }
                    else
                    {
                        MessageBox.Show("Could not proceed to load practice data because practice id not found. please specify practice id to load items.", "Missing Practice Id", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private int GetPriaceId()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    return unitOfWork.AdminPractiseSettingRepository.GetPracticeId();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private int updateDB(string qry)
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand com = new SqlCommand(qry, con);
            int i = com.ExecuteNonQuery();
            con.Close();
            return i;
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
        private void loadItemsData_FromExcelSheet(DataTable dtSource)
        {
            string suppName = "";
            using (unitOfWork = new UnitOfWork())
            {

                if (unitOfWork.SupplierRepository.SuppliersExist())
                {
                    goto LoadItems;
                }
                List<Supplier> sl = new List<Supplier>();
                foreach (DataRow r in dtSource.Rows)
                {
                    suppName = "";
                    suppName = r["Supplier"].ToString().Trim();
                    if (suppName == "")
                    {
                        continue;
                    }
                    Supplier objSupplier = sl.Where(sp => sp.Name.ToLower().Equals(suppName.ToLower())).FirstOrDefault();
                    if (objSupplier == null)
                    {
                        objSupplier =
                        new Supplier
                        {
                            Name = suppName,
                            Phone = "",
                            Address = "",
                            PrimaryContactPersonName = "",
                            PrimaryContactPersonPhone = "",
                            OpeningBalance = 0,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            IsActive = true,
                            IsNew = true,
                            IsUpdate = false,
                            IsSynced = false
                        };
                        sl.Add(objSupplier);
                    }
                    else
                    {

                    }
                }
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.SupplierRepository.InsertRange(sl);
                    unitOfWork.Save();
                }
            }

        LoadItems:
            List<Item> il = new List<Item>();
            using (unitOfWork = new UnitOfWork())
            {
                foreach (DataRow r in dtSource.Rows)
                {
                    Item objItem = new Item();
                    objItem.ItemName = r["Product Name"].ToString().Trim();
                    if (objItem.ItemName == "")
                    {
                        continue;
                    }
                    objItem.Barcode = r["Product Code"].ToString();
                    objItem.RetailPrice = Convert.ToDouble(r["Retail Price"]);
                    suppName = "";
                    suppName = r["Supplier"].ToString().Trim();
                    Supplier objSupplier = unitOfWork.SupplierRepository.GetSupplierByName(suppName.ToLower());
                    objItem.RackNo = "";
                    objItem.Unit = "Pack";
                    objItem.ConversionUnit = 1;
                    objItem.ReOrderingLevel = 0;
                    objItem.OpeningStock = 0;
                    objItem.ChecmicalName = "";
                    objItem.UnitCostPrice = 0;
                    objItem.IsNarcotic = false;
                    objItem.IsActive = true;
                    objItem.IsNew = true;
                    objItem.IsUpdate = false;
                    objItem.IsSynced = false;
                    objItem.CreateDate = DateTime.Now;
                    objItem.UpdateDate = DateTime.Now;
                    objItem.IsDefault = false;
                    objItem.Suppliers = new List<Supplier>();
                    objItem.Suppliers.Add(objSupplier);
                    il.Add(objItem);
                }
                unitOfWork.ItemRspository.InsertRange(il);
                unitOfWork.Save();
            }
            //MessageBox.Show("Data loaded successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void loadItemsData_FromExcelSheet2(DataTable dtSource)
        {
            //string suppName = "";
            string ManufName = "";
            using (unitOfWork = new UnitOfWork())
            {

                //    if (unitOfWork.SupplierRepository.SuppliersExist())
                //    {
                //        goto LoadItems;
                //    }
                //    List<Supplier> sl = new List<Supplier>();
                //    foreach (DataRow r in dtSource.Rows)
                //    {
                //        suppName = "";
                //        suppName = r["Supplier"].ToString().Trim();
                //        if (suppName == "")
                //        {
                //            continue;
                //        }
                //        Supplier objSupplier = sl.Where(sp => sp.Name.ToLower().Equals(suppName.ToLower())).FirstOrDefault();
                //        if (objSupplier == null)
                //        {
                //            objSupplier =
                //            new Supplier
                //            {
                //                Name = suppName,
                //                Phone = "",
                //                Address = "",
                //                PrimaryContactPersonName = "",
                //                PrimaryContactPersonPhone = "",
                //                OpeningBalance = 0,
                //                CreatedAt = DateTime.Now,
                //                UpdatedAt = DateTime.Now,
                //                IsActive = true,
                //                IsNew = true,
                //                IsUpdate = false,
                //                IsSynced = false
                //            };
                //            sl.Add(objSupplier);
                //        }
                //        else
                //        {

                //        }
                //    }
                //    using (unitOfWork = new UnitOfWork())
                //    {
                //        unitOfWork.SupplierRepository.InsertRange(sl);
                //        unitOfWork.Save();
                //    }
                //}

                // load manufacturers
                /*if (unitOfWork.ManufacturerRepository.ManufacturerExist())
                {
                    goto LoadItems;
                }
                List<Manufacturer> sl = new List<Manufacturer>();
                foreach (DataRow r in dtSource.Rows)
                {
                    ManufName = "";
                    ManufName = r["Manufacturer"].ToString().Trim();
                    if (ManufName == "")
                    {
                        continue;
                    }
                    Manufacturer objManuf = sl.Where(sp => sp.Name.ToLower().Equals(ManufName.ToLower())).FirstOrDefault();
                    if (objManuf == null)
                    {
                        objManuf =
                        new Manufacturer
                        {
                            Name = ManufName,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            IsActive = true,
                            IsNew = true, // nosyncing will be performed for default items
                            IsUpdate = false,
                            IsSynced = false,
                            IsSyncable = false
                        };
                        sl.Add(objManuf);
                    }
                }
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.ManufacturerRepository.InsertRange(sl);
                    unitOfWork.Save();
                }*/
            }
            int convUnit = 1;
            double unitCost = 0;
        LoadItems:
            List<Item> il = new List<Item>();
            using (unitOfWork = new UnitOfWork())
            {
                foreach (DataRow r in dtSource.Rows)
                {
                    convUnit = 1;

                    Item objItem = new Item();
                    objItem.ItemName = r["Product Name"].ToString().Trim();
                    if (objItem.ItemName == "")
                    {
                        continue;
                    }
                    Item itemFound = il.Where(i => i.ItemName.ToLower().Equals(objItem.ItemName.ToLower())).FirstOrDefault();
                    //objItem.Barcode = r["Product Code"].ToString();
                    //objItem.RetailPrice = Convert.ToDouble(r["Retail Price"]);
                    if (itemFound != null)
                    {
                        continue;
                    }
                    ManufName = "";
                    ManufName = r["Manufacturer"].ToString().Trim();
                    Manufacturer objManufac = unitOfWork.ManufacturerRepository.GetManufByName(ManufName.ToLower());
                    objItem.RackNo = "";
                    objItem.Unit = "Pack";
                    int.TryParse(r["Purchase Conversion Factor"].ToString(), out convUnit);
                    objItem.ConversionUnit = convUnit;
                    objItem.ReOrderingLevel = 0;
                    objItem.OpeningStock = 0;
                    objItem.ChecmicalName = r["Generic"].ToString();
                    double.TryParse(r["Cost Price"].ToString(), out unitCost);
                    objItem.UnitCostPrice = unitCost;
                    objItem.IsNarcotic = false;
                    objItem.IsActive = true;
                    objItem.IsNew = true;
                    objItem.IsUpdate = false;
                    objItem.IsSynced = false;
                    objItem.IsSyncable = false;
                    objItem.CreateDate = DateTime.Now;
                    objItem.UpdateDate = DateTime.Now;
                    objItem.Manufacturer = objManufac;
                    il.Add(objItem);
                }
                unitOfWork.ItemRspository.InsertRange(il);
                unitOfWork.Save();
            }
            //MessageBox.Show("Data loaded successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void loadItemsData_FromExcelSheet3(DataTable dtSource)
        {
            if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    label1.Text = "starting Data Mapping .... ";
                }));
                string ManufName = "";
                string catName = "";
                string suppName = "";
                int convUnit = 1;
                double unitCost = 0;
                double retailPrice = 0;
                int strips = 0;
                int uniqueItem_id = 0;

                try
                {
                    #region Suppliers
                    using (unitOfWork = new UnitOfWork())
                    {
                        if (unitOfWork.SupplierRepository.SuppliersExist())
                        {
                            goto LoadCategories;
                        }
                        List<Supplier> sl = new List<Supplier>();
                        foreach (DataRow r in dtSource.Rows)
                        {
                            suppName = "";
                            suppName = r["Supplier Name"].ToString().Trim();
                            if (suppName == "")
                            {
                                continue;
                            }
                            Supplier objSupplier = sl.Where(sp => sp.Name.ToLower().Equals(suppName.ToLower())).FirstOrDefault();
                            if (objSupplier == null)
                            {
                                objSupplier =
                                new Supplier
                                {
                                    Name = suppName,
                                    Phone = "",
                                    Address = "",
                                    PrimaryContactPersonName = "",
                                    PrimaryContactPersonPhone = "",
                                    OpeningBalance = 0,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    IsActive = true,
                                    IsNew = true,
                                    IsUpdate = false,
                                    IsSynced = false,
                                    User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId)
                                };
                                sl.Add(objSupplier);
                            }
                        }
                        unitOfWork.SupplierRepository.InsertRange(sl);
                        unitOfWork.Save();
                        if (sl != null && sl.Count() > 0)
                        {
                            var suppliersBatches = SharedFunctions.ChunkBy(sl, SharedVariables.ModuleBatch);
                            foreach (var batch in suppliersBatches)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, batch, ModuleName.BulkSuppliers, ModuleAction.Create, 0);
                            }
                        }
                    }
                    #endregion
                    LoadCategories:
                    #region Categories
                    using (unitOfWork = new UnitOfWork())
                    {
                        if (unitOfWork.CategoryRepository.Any())
                        {
                            goto LoadManufacs;
                        }
                        List<Category> categories = new List<Category>();
                        foreach (DataRow r in dtSource.Rows)
                        {
                            catName = "";
                            catName = r["Category"].ToString().ToLower();
                            if (catName == "")
                            {
                                continue;
                            }
                            Category objCat = categories.Where(c => c.Name.ToLower() == catName).FirstOrDefault();
                            if (objCat == null)
                            {
                                objCat =
                                new Category
                                {
                                    Name = catName,
                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                    IsActive = true,
                                    IsNew = true, // nosyncing will be performed for default items
                                IsUpdate = false,
                                    IsSynced = false,
                                    User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId)
                                };
                                categories.Add(objCat);
                            }
                        }
                        unitOfWork.CategoryRepository.InsertRange(categories);
                        unitOfWork.Save();
                        if (categories != null && categories.Count() > 0)
                        {
                            var categoriesBatches = SharedFunctions.ChunkBy(categories, SharedVariables.ModuleBatch);
                            foreach (var batch in categoriesBatches)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, batch, ModuleName.BulkCategories, ModuleAction.Create, 0);
                            }
                        }
                    }
                    #endregion
                    LoadManufacs:
                    #region Manufactures
                    using (unitOfWork = new UnitOfWork())
                    {
                        if (unitOfWork.ManufacturerRepository.ManufacturerExist())
                        {
                            goto LoadItems;
                        }
                        List<Manufacturer> ml = new List<Manufacturer>();
                        foreach (DataRow r in dtSource.Rows)
                        {
                            ManufName = "";
                            ManufName = r["Manufacturer"].ToString().Trim();
                            if (ManufName == "")
                            {
                                continue;
                            }
                            Manufacturer objManuf = ml.Where(sp => sp.Name.ToLower().Equals(ManufName.ToLower())).FirstOrDefault();
                            if (objManuf == null)
                            {
                                objManuf =
                                new Manufacturer
                                {
                                    Name = ManufName,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    IsActive = true,
                                    IsNew = true, // nosyncing will be performed for default items
                                IsUpdate = false,
                                    IsSynced = false,
                                    IsSyncable = false
                                };
                                ml.Add(objManuf);
                            }
                        }
                        unitOfWork.ManufacturerRepository.InsertRange(ml);
                        unitOfWork.Save();
                        if (ml != null && ml.Count() > 0)
                        {
                            var manufactureBatches = SharedFunctions.ChunkBy(ml, SharedVariables.ModuleBatch);
                            foreach (var batch in manufactureBatches)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, batch, ModuleName.BulkManufactures, ModuleAction.Create, 0);
                            }
                        }
                    }
                    #endregion
                    LoadItems:
                    #region Items
                    using (unitOfWork = new UnitOfWork())
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            progressBar1.Maximum = dtSource.Rows.Count;
                            progressBar1.Value = 0;
                            label2.Text = progressBar1.Value + " out of " + progressBar1.Maximum;
                        }));

                        List<Item> il = new List<Item>();
                        if (SharedVariables.AdminPractiseSetting.IsFranchiseMapped)
                        {
                            if (unitOfWork.ItemRspository.ExistsAny())
                            {
                                return;
                            }
                        }
                        foreach (DataRow r in dtSource.Rows)
                        {
                            convUnit = 1;
                            string name = r["Product Name"].ToString();
                            int uniqid = Convert.ToInt32(r["Unique ItemId"].ToString());
                            //string itemname = "";
                            //if (name.Contains("'"))
                            //{
                            //    foreach (var rr in name)
                            //    {
                            //        if(rr.ToString() == "'")
                            //        {
                            //            itemname = itemname + rr + "'";
                            //        }
                            //        else
                            //        {
                            //            itemname = itemname + rr;
                            //        }
                            //    }
                            //    name = itemname;
                            //}
                            //if (name != null)
                            //{
                            //    DataTable dt = getItemName("select * from Items where ItemName='" + name + "'");
                            //    if (dt.Rows.Count != 0)
                            //    {
                            //        continue;
                            //    }
                            //}
                            this.Invoke(new MethodInvoker(delegate ()
                            {
                                label1.Text = "Mapping Item " + name;
                                label2.Text = progressBar1.Value + " out of " + progressBar1.Maximum;
                                progressBar1.Value += 1;
                            }));
                            Item objItem = new Item();
                            objItem.ItemName = r["Product Name"].ToString().Trim();
                            if (objItem.ItemName == "")
                            {
                                continue;
                            }


                            objItem.Suppliers = new List<Supplier>();
                            objItem.PCTCode = GetExcelStr(dtSource, "PCTCode", r);
                            Item itemFound = il.Where(i => i.ItemName.ToLower().Equals(objItem.ItemName.ToLower())).FirstOrDefault();
                            //objItem.Barcode = r["Product Code"].ToString();
                            //objItem.RetailPrice = Convert.ToDouble(r["Retail Price"]);
                            if (itemFound != null)
                            {
                                continue;
                            }
                            ManufName = "";
                            suppName = "";
                            catName = "";

                            suppName = r["Supplier Name"].ToString().Trim();
                            catName = r["Category"].ToString().ToLower();
                            ManufName = r["Manufacturer"].ToString().Trim();
                            suppName = r["Supplier Name"].ToString().Trim();
                            double.TryParse(r["Product Price"].ToString(), out retailPrice);

                            int.TryParse(r["Unique ItemId"].ToString(), out uniqueItem_id); //unique item Id for Franchise                        
                            int.TryParse(r["No of Strips"].ToString(), out strips);
                            double.TryParse(r["Cost Price"].ToString(), out unitCost);
                            int.TryParse(r["Purchase Conversion Factor"].ToString(), out convUnit);

                            Manufacturer objManufac = unitOfWork.ManufacturerRepository.GetManufByName(ManufName.ToLower());
                            Supplier objSupp = unitOfWork.SupplierRepository.GetSupplierByName(suppName.ToLower());
                            objItem.RackNo = "";
                            objItem.Unit = "Pack";
                            objItem.ConversionUnit = convUnit;
                            objItem.ReOrderingLevel = 0;
                            objItem.OpeningStock = 0;
                            objItem.ChecmicalName = r["Generic"].ToString();
                            objItem.UnitCostPrice = Math.Round(unitCost, 3);
                            objItem.RetailPrice = Math.Round(retailPrice, 3);
                            objItem.NoOfStrips = strips;
                            objItem.Unique_itemId = uniqueItem_id;
                            objItem.IsApproved = 0;
                            objItem.IsNarcotic = false;
                            objItem.IsActive = true;
                            objItem.IsNew = true;
                            objItem.IsUpdate = false;
                            objItem.IsSynced = false;
                            objItem.IsSyncable = false;
                            objItem.CreateDate = DateTime.Now;
                            objItem.UpdateDate = DateTime.Now;
                            objItem.Manufacturer = objManufac;
                            objItem.IsFileItem = true;
                            objItem.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                            if (objSupp != null)
                            {
                                objItem.Suppliers.Add(objSupp);
                            }

                            objItem.Category = unitOfWork.CategoryRepository.GetCategoryIdByName(catName);
                            il.Add(objItem);
                        }
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            label1.Text = "Saving Mapped Data";
                            label2.Text = "Analyzing Data...";
                            progressBar1.Value = progressBar1.Maximum;
                        }));
                        unitOfWork.ItemRspository.InsertRange(il);
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            label1.Text = "Saving Mapped Data";
                            label2.Text = "Saving .......";
                            progressBar1.Value = progressBar1.Maximum;
                            progressBar1.Style = ProgressBarStyle.Marquee;
                        }));
                        unitOfWork.Save();
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            label1.Text = "Default Data Set Loaded on Database";
                            label2.Text = "Completed...";
                            progressBar1.Style = ProgressBarStyle.Blocks;
                            progressBar1.Value = progressBar1.Maximum;

                        }));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion
            }
            else
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    label1.Text = "Starting Data Manipulation .... ";
                }));
                string ManufName = "";
                string catName = "";
                string suppName = "";
                int convUnit = 1;
                double unitCost = 0;
                double retailPrice = 0;

                
                using (unitOfWork = new UnitOfWork())
                {
/*                    if (unitOfWork.SupplierRepository.SuppliersExist())
                    {
                        goto LoadCategories;
                    }
                    List<Supplier> sl = new List<Supplier>();
                    foreach (DataRow r in dtSource.Rows)
                    {
                        suppName = "";
                        suppName = r["Supplier Name"].ToString().Trim();
                        if (suppName == "")
                        {
                            continue;
                        }
                        Supplier objSupplier = sl.Where(sp => sp.Name.ToLower().Equals(suppName.ToLower())).FirstOrDefault();
                        if (objSupplier == null)
                        {
                            objSupplier =
                            new Supplier
                            {
                                Name = suppName,
                                Phone = "",
                                Address = "",
                                PrimaryContactPersonName = "",
                                PrimaryContactPersonPhone = "",
                                OpeningBalance = 0,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                                IsActive = true,
                                IsNew = true,
                                IsUpdate = false,
                                IsSynced = false,
                                User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId)
                            };
                            sl.Add(objSupplier);
                        }
                    }
                    unitOfWork.SupplierRepository.InsertRange(sl);
                    unitOfWork.Save();*/
                   /* if (sl != null && sl.Count() > 0)
                    {
                        var suppliersBatches = SharedFunctions.ChunkBy(sl, SharedVariables.ModuleBatch);
                        foreach (var batch in suppliersBatches)
                        {
                            OfflineJsonTask.SaveOfflineTask(unitOfWork, batch, ModuleName.BulkSuppliers, ModuleAction.Create, 0);
                        }
                    }*/
                }
/*                #endregion
                LoadCategories:
                #region Categories
                using (unitOfWork = new UnitOfWork())
                {
                    if (unitOfWork.CategoryRepository.Any())
                    {
                        goto LoadManufacs;
                    }
                    List<Category> categories = new List<Category>();
                    foreach (DataRow r in dtSource.Rows)
                    {
                        catName = "";
                        catName = r["Category"].ToString().ToLower();
                        if (catName == "")
                        {
                            continue;
                        }
                        Category objCat = categories.Where(c => c.Name.ToLower() == catName).FirstOrDefault();
                        if (objCat == null)
                        {
                            objCat =
                            new Category
                            {
                                Name = catName,
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                                IsActive = true,
                                IsNew = true, // nosyncing will be performed for default items
                            IsUpdate = false,
                                IsSynced = false,
                                User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId)
                            };
                            categories.Add(objCat);
                        }
                    }
                    unitOfWork.CategoryRepository.InsertRange(categories);
                    unitOfWork.Save();
                    if (categories != null && categories.Count() > 0)
                    {
                        var categoriesBatches = SharedFunctions.ChunkBy(categories, SharedVariables.ModuleBatch);
                        foreach (var batch in categoriesBatches)
                        {
                            OfflineJsonTask.SaveOfflineTask(unitOfWork, batch, ModuleName.BulkCategories, ModuleAction.Create, 0);
                        }
                    }
                }
                #endregion
                LoadManufacs:
                #region Manufactures
                using (unitOfWork = new UnitOfWork())
                {
                    if (unitOfWork.ManufacturerRepository.ManufacturerExist())
                    {
                        goto LoadItems;
                    }
                    List<Manufacturer> ml = new List<Manufacturer>();
                    foreach (DataRow r in dtSource.Rows)
                    {
                        ManufName = "";
                        ManufName = r["Manufacturer"].ToString().Trim();
                        if (ManufName == "")
                        {
                            continue;
                        }
                        Manufacturer objManuf = ml.Where(sp => sp.Name.ToLower().Equals(ManufName.ToLower())).FirstOrDefault();
                        if (objManuf == null)
                        {
                            objManuf =
                            new Manufacturer
                            {
                                Name = ManufName,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                                IsActive = true,
                                IsNew = true, // nosyncing will be performed for default items
                            IsUpdate = false,
                                IsSynced = false,
                                IsSyncable = false
                            };
                            ml.Add(objManuf);
                        }
                    }
                    unitOfWork.ManufacturerRepository.InsertRange(ml);
                    unitOfWork.Save();
                    if (ml != null && ml.Count() > 0)
                    {
                        var manufactureBatches = SharedFunctions.ChunkBy(ml, SharedVariables.ModuleBatch);
                        foreach (var batch in manufactureBatches)
                        {
                            OfflineJsonTask.SaveOfflineTask(unitOfWork, batch, ModuleName.BulkManufactures, ModuleAction.Create, 0);
                        }
                    }
                }
                #endregion
                LoadItems:
                #region Items
                using (unitOfWork = new UnitOfWork())
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        progressBar1.Maximum = dtSource.Rows.Count;
                        progressBar1.Value = 0;
                        label2.Text = progressBar1.Value + "/" + progressBar1.Maximum;
                    }));
                    List<Item> il = new List<Item>();
                    if (unitOfWork.ItemRspository.ExistsAny())
                    {
                        return;
                    }
                    foreach (DataRow r in dtSource.Rows)
                    {
                        convUnit = 1;
                        Item objItem = new Item();
                        objItem.ItemName = r["Product Name"].ToString().Trim();
                        if (objItem.ItemName == "")
                        {
                            continue;
                        }
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                        //progressBar1.Maximum = dtSource.Rows.Count;
                        label1.Text = "Loading Item " + objItem.ItemName;
                            progressBar1.Value += 1;
                            label2.Text = progressBar1.Value + "/" + progressBar1.Maximum;
                        }));
                        objItem.Suppliers = new List<Supplier>();
                        objItem.PCTCode = GetExcelStr(dtSource, "PCTCode", r);
                        Item itemFound = il.Where(i => i.ItemName.ToLower().Equals(objItem.ItemName.ToLower())).FirstOrDefault();
                        //objItem.Barcode = r["Product Code"].ToString();
                        //objItem.RetailPrice = Convert.ToDouble(r["Retail Price"]);
                        if (itemFound != null)
                        {
                            continue;
                        }
                        ManufName = "";
                        suppName = "";
                        catName = "";

                        suppName = r["Supplier Name"].ToString().Trim();
                        catName = r["Category"].ToString().ToLower();
                        ManufName = r["Manufacturer"].ToString().Trim();
                        suppName = r["Supplier Name"].ToString().Trim();

                        int.TryParse(r["Purchase Conversion Factor"].ToString(), out convUnit);
                        double.TryParse(r["Cost Price"].ToString(), out unitCost);
                        double.TryParse(r["Product Price"].ToString(), out retailPrice);


                        Manufacturer objManufac = unitOfWork.ManufacturerRepository.GetManufByName(ManufName.ToLower());
                        Supplier objSupp = unitOfWork.SupplierRepository.GetSupplierByName(suppName.ToLower());
                        objItem.RackNo = "";
                        objItem.Unit = "Pack";
                        objItem.ConversionUnit = convUnit;
                        objItem.ReOrderingLevel = 0;
                        objItem.OpeningStock = 0;
                        objItem.ChecmicalName = r["Generic"].ToString();
                        objItem.UnitCostPrice = Math.Round(unitCost, 3);
                        objItem.RetailPrice = Math.Round(retailPrice, 3);
                        objItem.IsNarcotic = false;
                        objItem.IsActive = true;
                        objItem.IsNew = true;
                        objItem.IsUpdate = false;
                        objItem.IsSynced = false;
                        objItem.IsSyncable = false;
                        objItem.CreateDate = DateTime.Now;
                        objItem.UpdateDate = DateTime.Now;
                        objItem.Manufacturer = objManufac;
                        objItem.IsFileItem = true;
                        objItem.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        if (objSupp != null)
                        {
                            objItem.Suppliers.Add(objSupp);
                        }

                        objItem.Category = unitOfWork.CategoryRepository.GetCategoryIdByName(catName);
                        il.Add(objItem);
                    }
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                    //progressBar1.Maximum = dtSource.Rows.Count;
                    label1.Text = "Saving Default Dataset....";
                        progressBar1.Style = ProgressBarStyle.Continuous;
                        label2.Text = "Analyzing Data ....";
                    }));
                    unitOfWork.ItemRspository.InsertRange(il);
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                    //progressBar1.Maximum = dtSource.Rows.Count;
                    label1.Text = "Saving Default Dataset....";
                        progressBar1.Style = ProgressBarStyle.Marquee;
                        label2.Text = "Saving Data ....";
                    }));
                    unitOfWork.Save();
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                    //progressBar1.Maximum = dtSource.Rows.Count;
                    label1.Text = "Saving Default Dataset....";
                        progressBar1.Style = ProgressBarStyle.Blocks;
                        progressBar1.Value = progressBar1.Maximum;
                        label2.Text = "Default Dataset Loaded Successfully ....";
                    }));
                }
                #endregion*/

            }
        }
        #region franchiseMapping
        private void loadItemsData_FromExcelSheet4(DataTable dtSource)
        {
            int count = 0;
            int ItemId = 0;
            string desktopName = "";
            string defaultName = "";
            int unique_id = 0;
            int desktop_Id = 0;

            List<Item> il = new List<Item>();
            foreach (DataRow r in dtSource.Rows)
            {
                Item objItem = new Item();

                ItemId = Convert.ToInt32(r["Item_Id"]);
                //desktopName = r["Franchise_Item"].ToString().ToLower();
                defaultName = r["Default_name"].ToString().Trim();
                unique_id = Convert.ToInt32(r["Unique_ItemId"]);

                objItem.ItemID = ItemId;
                //objItem.ItemName = desktopName;

                if (unique_id == 0)
                {
                    if (ItemId == 0)
                    {
                    }
                    else
                    {
                        int DfaultItemId = getDefaultItemID(defaultName);
                        objItem.DefaultItemId = DfaultItemId;

                        if (DfaultItemId == 0)
                        {
                        }
                        else
                        {
                            UpdateItemsKey(objItem);
                        }
                    }
                }
                else
                {
                    //count++;
                    this.InActiveItems();

                    //item that are new
                    this.loadItemsData_FromExcelSheet5(unique_id, ItemId);
                }
            }
        }

        private int getFranchiseItemId(string francName)
        {
            int ItemId = 0;
            DataTable Items = getItemName("select * from Items where LOWER(ItemName)=LOWER(@ItemName) and Unique_ItemId =" + 0, francName);
            if (Items.Rows.Count > 0)
            {
                ItemId = Convert.ToInt32(Items.Rows[0]["ItemID"]);
            }
            else { ItemId = 0; }

            return ItemId;
        }

        public int getDefaultItemID(string DefaultName)
        {
            int prevItemId = 0;
            DataTable Items = getItemName("select * from Items where LOWER(ItemName)=LOWER(@ItemName) and Unique_ItemId != " + 0, DefaultName);
            if (Items.Rows.Count > 0)
            {
                prevItemId = Convert.ToInt32(Items.Rows[0]["ItemID"]);
            }
            else { prevItemId = 0; }

            return prevItemId;
        }

        private DataTable getItemName(string qry, string ItemName)
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            con.Open();
            SqlCommand com = new SqlCommand(qry, con);
            com.Parameters.AddWithValue("@ItemName", ItemName.TrimStart());
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = com;
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        private DataTable getItemName(string qry)
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            con.Open();
            SqlCommand com = new SqlCommand(qry, con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = com;
            sda.Fill(dt);
            con.Close();
            return dt;
        }


        public void UpdateItemsKey(Item obj)
        {
            try
            {
                //Update StockItems
                int stock = updateDB("Update StockItems set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);

                //Update InvoiceItems
                int invoice = updateDB("Update InvoiceItems set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);

                //Update StockReturnItems
                int stockReturn = updateDB("Update StockReturnItems set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);

                //Update PurchaseOrderItems
                int prchaseOrder = updateDB("Update PurchaseOrderItems set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);

                //Update RackItems
                int rackItem = updateDB("Update RackItems set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);

                //Update SalesReturnItems
                int saleReturn = updateDB("Update SalesReturnItems set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);

                //Update StockAuditDetail
                int stockDetail = updateDB("Update StockAuditDetails set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);

                //Update StockConsumptionitems
                int stockCons = updateDB("Update StockConsumptionitems set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);

                //Update AdjustmentItems
                int adjustItem = updateDB("Update AdjustmentItems set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);

                //Update TemplateItems
                int templtItem = updateDB("Update TemplateItems set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);

                //Update SupplierItems
                int supplier = updateDB("Update SupplierItems set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);

                //Update DiscountItems
                int discountItem = updateDB("Update DiscountItems set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);

                //Update Missed Sales
                int misedSale = updateDB("Update MissedSales set Item_ItemID =" + obj.DefaultItemId + " where Item_ItemID= " + obj.ItemID);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void InActiveItems()
        {
            int n = updateDB("Update Items set IsActive =" + 0 + " where Unique_itemId= " + 0);
        }
        private void loadItemsData_FromExcelSheet5(int uniqueID, int ItemID)
        {
            int ItemId = 0;
            int uniqueId = uniqueID;

            int set = updateDB("update Items set IsActive =" + 1 + ",Unique_itemId=" + Convert.ToInt32(uniqueId) + ",IsApproved=" + 1 + " where ItemId=" + ItemID);

            //if Update No ofStrips
            //int strips = updateDB("update Items set NoOfStrips =" + 1 + " where ItemId=" + ItemID);
        }

        private int getNotMappedItem(string francName)
        {
            int ItemId = 0;
            DataTable Items = getItemName("select * from Items where LOWER(ItemName)=LOWER(@ItemName) and Unique_ItemId =" + 0 + " and IsActive =" + 0, francName);
            if (Items.Rows.Count > 0)
            {
                ItemId = Convert.ToInt32(Items.Rows[0]["ItemID"]);
            }
            else { ItemId = 0; }

            return ItemId;
        }

        private int loadItemsData_FromExcelSheet6(DataTable dtSource)
        {
            int ItemId = 0;
            int count = 0;
            string desktopName = "";
            string defaultName = "";
            int desktop_Id = 0;

            List<Item> il = new List<Item>();
            foreach (DataRow r in dtSource.Rows)
            {
                Item objItem = new Item();

                desktopName = r["Franchise_Item"].ToString().ToLower();
                defaultName = r["Default_name"].ToString().Trim();
                ItemId = getFranchiseItemId(desktopName);

                objItem.ItemID = ItemId;
                objItem.ItemName = desktopName;

                int check = getFranchiseItemId(desktopName);
                if (ItemId == 0)
                { }
                else
                {
                    count++;
                }
            }
            return count;
        }
        #endregion
        private void loadItemsData_KhizarPharmacy(DataTable dtSource)
        {
            int convUnit = 1;
            double retailPrice = 0;
            int OpeningStock = 0;
            List<Item> il = new List<Item>();
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.ItemRspository.ExistsAny())
                {
                    return;
                }
                foreach (DataRow r in dtSource.Rows)
                {
                    OpeningStock = 0;
                    convUnit = 1;

                    Item objItem = new Item();
                    objItem.ItemName = r[0].ToString().Trim();
                    if (objItem.ItemName == "")
                    {
                        continue;
                    }
                    //objItem.Suppliers = new List<Supplier>();
                    Item itemFound = il.Where(i => i.ItemName.ToLower().Equals(objItem.ItemName.ToLower())).FirstOrDefault();


                    int.TryParse(r["Quantity"].ToString(), out OpeningStock);
                    double.TryParse(r["Total Stock Price"].ToString(), out retailPrice);

                    if (itemFound != null)
                    {
                        itemFound.RetailPrice = itemFound.RetailPrice * itemFound.OpeningStock;
                        itemFound.RetailPrice += retailPrice;
                        itemFound.OpeningStock += OpeningStock;
                        itemFound.RetailPrice = itemFound.RetailPrice / (itemFound.OpeningStock > 0 ? itemFound.OpeningStock : 1);
                    }
                    else
                    {
                        if (retailPrice > 0)
                        {
                            retailPrice = retailPrice / (OpeningStock > 0 ? OpeningStock : 1);
                        }
                        objItem.RackNo = "";
                        objItem.Unit = "Pack";
                        objItem.ConversionUnit = convUnit;
                        objItem.ReOrderingLevel = 0;
                        objItem.OpeningStock = OpeningStock;
                        objItem.ChecmicalName = "";
                        objItem.UnitCostPrice = 0;
                        objItem.RetailPrice = retailPrice;
                        objItem.IsNarcotic = false;
                        objItem.IsActive = true;
                        objItem.IsNew = true;
                        objItem.IsUpdate = false;
                        objItem.IsSynced = false;
                        objItem.IsSyncable = false;
                        objItem.CreateDate = DateTime.Now;
                        objItem.UpdateDate = DateTime.Now;
                        il.Add(objItem);
                    }
                }
                foreach (var i in il)
                {
                    if (i.OpeningStock > 0)
                    {
                        MakeOpeningStockEntry(i, 0);
                    }
                }
                unitOfWork.ItemRspository.InsertRange(il);
                unitOfWork.Save();
            }
            //MessageBox.Show("Data loaded successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void loadItemsData_EverCarePharmacy(DataTable dtSource)
        {
            int convUnit = 1;
            double retailPrice = 0;
            double costPrice = 0;
            int OpeningStock = 0;
            string catName = "";
            List<Item> il = new List<Item>();

            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.CategoryRepository.Any())
                {
                    goto LoadItemData;
                }

                List<Category> categories = new List<Category>();
                foreach (DataRow r in dtSource.Rows)
                {
                    catName = "";
                    catName = r["Category"].ToString().ToLower();
                    if (catName == "")
                    {
                        continue;
                    }
                    Category objCat = categories.Where(c => c.Name.ToLower() == catName).FirstOrDefault();
                    if (objCat == null)
                    {
                        objCat =
                        new Category
                        {
                            Name = catName,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            IsActive = true,
                            IsNew = true, // nosyncing will be performed for default items
                            IsUpdate = false,
                            IsSynced = false,
                            User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId)
                        };
                        categories.Add(objCat);
                    }
                }
                unitOfWork.CategoryRepository.InsertRange(categories);
                unitOfWork.Save();
            }


        LoadItemData:
            string barcode = "";
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.ItemRspository.ExistsAny())
                {
                    return;
                }
                foreach (DataRow r in dtSource.Rows)
                {
                    barcode = "";
                    OpeningStock = 0;
                    convUnit = 1;

                    Item objItem = new Item();
                    objItem.ItemName = r[0].ToString().Trim();
                    if (objItem.ItemName == "")
                    {
                        continue;
                    }
                    //objItem.Suppliers = new List<Supplier>();
                    Item itemFound = il.Where(i => i.ItemName.ToLower().Equals(objItem.ItemName.ToLower())).FirstOrDefault();


                    int.TryParse(r["Quantity"].ToString(), out OpeningStock);
                    double.TryParse(r["Unit Cost"].ToString(), out costPrice);
                    double.TryParse(r["Retail value"].ToString(), out retailPrice);
                    barcode = r["Barcode"].ToString();

                    if (itemFound == null)
                    {
                        objItem.RackNo = "";
                        objItem.Barcode = barcode;
                        objItem.Unit = "Pack";
                        objItem.ConversionUnit = convUnit;
                        objItem.ReOrderingLevel = 0;
                        objItem.OpeningStock = OpeningStock;
                        objItem.ChecmicalName = "";
                        objItem.UnitCostPrice = costPrice;
                        objItem.RetailPrice = retailPrice;
                        objItem.IsNarcotic = false;
                        objItem.IsActive = true;
                        objItem.IsNew = true;
                        objItem.IsUpdate = false;
                        objItem.IsSynced = false;
                        objItem.IsSyncable = false;
                        objItem.CreateDate = DateTime.Now;
                        objItem.UpdateDate = DateTime.Now;
                        il.Add(objItem);
                    }
                }
                int docNo = 100001;
                foreach (var i in il)
                {
                    if (i.OpeningStock > 0)
                    {
                        MakeOpeningStockEntry(i, docNo);
                        docNo += 1;
                    }
                }
                unitOfWork.ItemRspository.InsertRange(il);
                unitOfWork.Save();
            }
            //MessageBox.Show("Data loaded successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void loadItemsData_HameedHospitalPharmacy(DataTable dtSource)
        {
            string suppName = "";
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.SupplierRepository.SuppliersExist())
                {
                    goto LoadItems;
                }

                List<Supplier> sl = new List<Supplier>();
                foreach (DataRow r in dtSource.Rows)
                {
                    suppName = "";
                    suppName = r["Supplier"].ToString().Trim();
                    if (suppName == "")
                    {
                        continue;
                    }
                    Supplier objSupplier = sl.Where(sp => sp.Name.ToLower().Equals(suppName.ToLower())).FirstOrDefault();
                    if (objSupplier == null)
                    {
                        objSupplier =
                        new Supplier
                        {
                            Name = suppName,
                            Phone = "",
                            Address = "",
                            PrimaryContactPersonName = "",
                            PrimaryContactPersonPhone = "",
                            OpeningBalance = 0,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            IsActive = true,
                            IsNew = true,
                            IsUpdate = false,
                            IsSynced = false,
                            User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId)
                        };
                        sl.Add(objSupplier);
                    }
                }

                unitOfWork.SupplierRepository.InsertRange(sl);
                unitOfWork.Save();
            }



        LoadItems:

            int convUnit = 1;
            double retailPrice = 0;
            double costPrice = 0;
            int Quantity = 0;
            List<Item> il = new List<Item>();
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.ItemRspository.ExistsAny())
                {
                    return;
                }
                foreach (DataRow r in dtSource.Rows)
                {
                    suppName = "";
                    Quantity = 0;
                    convUnit = 1;

                    Item objItem = new Item();
                    objItem.ItemName = r["Name"].ToString().Trim();
                    if (objItem.ItemName == "")
                    {
                        continue;
                    }
                    Item itemFound = il.Where(i => i.ItemName.ToLower().Equals(objItem.ItemName.ToLower())).FirstOrDefault();
                    suppName = r["Supplier"].ToString().Trim();
                    int.TryParse(r["Quantity"].ToString(), out Quantity);
                    double.TryParse(r["Unit Cost"].ToString(), out costPrice);
                    double.TryParse(r["Retail Price"].ToString(), out retailPrice);

                    if (itemFound == null)
                    {
                        objItem.RackNo = "";
                        objItem.Barcode = "";
                        objItem.Unit = "Pack";
                        objItem.ConversionUnit = convUnit;
                        objItem.ReOrderingLevel = 0;
                        objItem.OpeningStock = Quantity;
                        objItem.ChecmicalName = "";
                        objItem.UnitCostPrice = costPrice;
                        objItem.RetailPrice = retailPrice;
                        objItem.IsNarcotic = false;
                        objItem.IsActive = true;
                        objItem.IsNew = true;
                        objItem.IsUpdate = false;
                        objItem.IsSynced = false;
                        objItem.IsSyncable = true;
                        objItem.CreateDate = DateTime.Now;
                        objItem.UpdateDate = DateTime.Now;
                        objItem.Suppliers = new List<Supplier>();
                        Supplier objSupp = unitOfWork.SupplierRepository.GetSupplierByName(suppName.ToLower());
                        if (objSupp != null)
                        {
                            objItem.Suppliers.Add(objSupp);
                        }

                        objItem.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        il.Add(objItem);
                    }
                }
                int docNo = 100001;
                foreach (var i in il)
                {
                    if (i.OpeningStock > 0)
                    {
                        MakeOpeningStockEntry(i, docNo);
                        docNo += 1;
                    }
                }
                unitOfWork.ItemRspository.InsertRange(il);
                unitOfWork.Save();
            }
            //MessageBox.Show("Data loaded successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                #region IfSuppliersExist
                if (dtSource.Columns.Contains("Supplier"))
                {
                    IsSuppliersExists = true;
                    using (unitOfWork = new UnitOfWork())
                    {
                        if (unitOfWork.SupplierRepository.SuppliersExist())
                        {
                            goto LoadCategories;
                        }
                        List<Supplier> sl = new List<Supplier>();
                        foreach (DataRow r in dtSource.Rows)
                        {
                            suppName = "";
                            suppName = r["Supplier"].ToString().Trim();
                            if (suppName == "")
                            {
                                continue;
                            }
                            Supplier objSupplier = sl.Where(sp => sp.Name.ToLower().Equals(suppName.ToLower())).FirstOrDefault();
                            if (objSupplier == null)
                            {
                                objSupplier =
                                new Supplier
                                {
                                    Name = suppName,
                                    Phone = "",
                                    Address = "",
                                    PrimaryContactPersonName = "",
                                    PrimaryContactPersonPhone = "",
                                    OpeningBalance = 0,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    IsActive = true,
                                    IsNew = true,
                                    IsUpdate = false,
                                    IsSynced = false,
                                    User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId)
                                };
                                sl.Add(objSupplier);
                            }
                        }
                        unitOfWork.SupplierRepository.InsertRange(sl);
                        unitOfWork.Save();
                        if (sl != null && sl.Count() > 0)
                        {
                            var supplierBatches = SharedFunctions.ChunkBy(sl, SharedVariables.ModuleBatch);
                            foreach (var batch in supplierBatches)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, batch, ModuleName.BulkSuppliers, ModuleAction.Create, 0);
                            }
                        }
                    }
                }
                #endregion
            LoadCategories:
                #region IfCategoriesExist
                if (dtSource.Columns.Contains("Category"))
                {
                    IsCategoryExists = true;
                    using (unitOfWork = new UnitOfWork())
                    {
                        if (unitOfWork.CategoryRepository.CategoriesExist())
                        {
                            goto LoadManufacs;
                        }
                        List<Category> mn = new List<Category>();
                        foreach (DataRow r in dtSource.Rows)
                        {
                            categoryName = "";
                            categoryName = r["Category"].ToString().Trim();
                            if (categoryName == "")
                            {
                                continue;
                            }
                            Category objCategory = mn.Where(sp => sp.Name.ToLower().Equals(categoryName.ToLower())).FirstOrDefault();
                            if (objCategory == null)
                            {
                                objCategory =
                                new Category
                                {
                                    Name = categoryName,
                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                    IsActive = true,
                                    IsNew = true,
                                    IsUpdate = false,
                                    IsSynced = false,
                                    User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId)
                                };
                                mn.Add(objCategory);
                            }
                        }
                        unitOfWork.CategoryRepository.InsertRange(mn);
                        unitOfWork.Save();
                        if (mn != null && mn.Count() > 0)
                        {
                            var categoryBatches = SharedFunctions.ChunkBy(mn, SharedVariables.ModuleBatch);
                            foreach (var item in categoryBatches)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkCategories, ModuleAction.Create, 0);
                            }
                        }
                    }
                }
                #endregion
            LoadManufacs:
                #region IfManufacturersExist
                if (dtSource.Columns.Contains("Manufacturer"))
                {
                    IsManufacturerExists = true;
                    using (unitOfWork = new UnitOfWork())
                    {
                        if (unitOfWork.ManufacturerRepository.ManufacturersExist())
                        {
                            goto LoadItems;
                        }
                        List<Manufacturer> mn = new List<Manufacturer>();
                        foreach (DataRow r in dtSource.Rows)
                        {
                            manufactureName = "";
                            manufactureName = r["Manufacturer"].ToString().Trim();
                            if (manufactureName == "")
                            {
                                continue;
                            }
                            Manufacturer objManufacturer = mn.Where(sp => sp.Name.ToLower().Equals(manufactureName.ToLower())).FirstOrDefault();
                            if (objManufacturer == null)
                            {
                                objManufacturer =
                                new Manufacturer
                                {
                                    Name = manufactureName,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    IsActive = true,
                                    IsNew = true,
                                    IsUpdate = false,
                                    IsSynced = false,
                                };
                                mn.Add(objManufacturer);
                            }
                        }
                        unitOfWork.ManufacturerRepository.InsertRange(mn);
                        unitOfWork.Save();
                        if (mn != null && mn.Count() > 0)
                        {
                            var manufacturerBatches = SharedFunctions.ChunkBy(mn, SharedVariables.ModuleBatch);
                            foreach (var item in manufacturerBatches)
                            {
                                OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.BulkManufactures, ModuleAction.Create, 0);
                            }
                        }
                    }
                }
                #endregion
            LoadItems:
                #region LoadAndSaveItems
                double retailPrice = 0;
                double costPrice = 0;
                int Quantity = 0;
                List<Item> il = new List<Item>();
                using (unitOfWork = new UnitOfWork())
                {
                    userModel = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    if (unitOfWork.ItemRspository.ExistsAny())
                    {
                        return;
                    }

                    //List<ItemExcelVM> items = SharedFunctions.DataTableToModel<ItemExcelVM>(dtSource);

                    foreach (DataRow r in dtSource.Rows)
                    {
                        suppName = "";
                        manufactureName = "";
                        categoryName = "";
                        Quantity = 0;
                        Item objItem = new Item();
                        objItem.ItemName = r["ItemName"].ToString().Trim();
                        objItem.Barcode = GetExcelStr(dtSource, "Barcode", r);
                        if (objItem.ItemName == "")
                        {
                            continue;
                        }
                        Item itemFound = il.Where(i => i.ItemName.ToLower().Equals(objItem.ItemName.ToLower()) || i.Barcode.ToLower().Equals(objItem.Barcode.ToLower())).FirstOrDefault();
                        //-------------------- For Supplier  ----------------//
                        if (IsSuppliersExists)
                            suppName = r["Supplier"].ToString().Trim();
                        //-------------------- For Manufacturer ----------------//
                        if (IsManufacturerExists)
                            manufactureName = r["Manufacturer"].ToString().Trim();
                        //-------------------- For Category ----------------//
                        if (IsCategoryExists)
                            categoryName = r["Category"].ToString().Trim();
                        int.TryParse(r["Quantity"].ToString(), out Quantity);
                        double.TryParse(r["CostPrice"].ToString(), out costPrice);
                        double.TryParse(r["RetailPrice"].ToString(), out retailPrice);
                        if (itemFound == null)
                        {
                            objItem.RackNo = ""; 
                            objItem.ChecmicalName = GetExcelStr(dtSource, "GenericName", r);
                            objItem.PCTCode = GetExcelStr(dtSource, "PCTCode", r);
                            int ConUnit = GetExcelInt(dtSource, "ConUnit", r);
                            objItem.ConversionUnit = ConUnit > 0 ? ConUnit : 1;
                            objItem.Unit = "Pack";
                            objItem.ReOrderingLevel = 0;
                            objItem.OpeningStock = Quantity;
                            objItem.UnitCostPrice = Math.Round(costPrice,3);
                            objItem.RetailPrice = Math.Round(retailPrice,3);
                            objItem.IsNarcotic = GetExcelBool(dtSource, "Narcotic", r);
                            objItem.IsActive = true;
                            objItem.IsNew = true;
                            objItem.IsUpdate = false;
                            objItem.IsSynced = false;
                            objItem.IsSyncable = true;
                            objItem.CreateDate = DateTime.Now;
                            objItem.UpdateDate = DateTime.Now;
                            objItem.IsFileItem = true;
                            //-------------------- For Category ----------------//
                            if (IsCategoryExists)
                            {
                                objItem.Category = new Category();
                                objItem.Category = unitOfWork.CategoryRepository.GetCategoryByName(categoryName.ToLower());
                            }
                            //-------------------- For Manufacturer ----------------//
                            if (IsManufacturerExists)
                            {
                                objItem.Manufacturer = new Manufacturer();
                                objItem.Manufacturer = unitOfWork.ManufacturerRepository.GetManufByName(manufactureName.ToLower());
                            }
                            //-------------------- For Supplier ----------------//
                            if (IsSuppliersExists)
                            {
                                objItem.Suppliers = new List<Supplier>();
                                Supplier objSupp = unitOfWork.SupplierRepository.GetSupplierByName(suppName.ToLower());
                                if (objSupp != null)
                                {
                                    objItem.Suppliers.Add(objSupp);
                                }
                            }
                            objItem.User = userModel;
                            il.Add(objItem);
                        }
                    }
                    if (il != null && il.Count() > 0)
                    {
                        unitOfWork.ItemRspository.InsertRange(il);
                        unitOfWork.Save();
                    }
                }
                #endregion

                #region SaveStock
                List<Item> stockItemList = il.Where(x => x.OpeningStock > 0).ToList();
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
                //if (ts != null)
                //    ts.Dispose();
                throw ex;
            }

        }
        private void SetTransactionManagerField(string fieldName, object value)
        {
            typeof(TransactionManager).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, value);
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

        public int GetExcelInt(DataTable dtSource, string colName, DataRow r)
        {
            int result = 0;
            if (dtSource.Columns.Contains(colName))
                int.TryParse(r[colName].ToString(), out result);
            return result;
        }

        public string GetExcelStr(DataTable dtSource, string colName, DataRow r)
        {
            string result = "";
            if (dtSource.Columns.Contains(colName))
                result = r[colName].ToString().Trim();
            //result = result != null && result != string.Empty ? result : "";
            return result;
        }

        private void MakeOpeningStockEntry(Item objItem, int DocNo)
        {
            Stock objStock = new Stock();
            objStock.DocumentNo = DocNo;
            objStock.Supplier = null;
            objStock.SupplierInvoiceNo = "";
            objStock.SupplierIvoiceDate = DateTime.Now;
            objStock.IsAutoInsertedStock = false; // if need to sync stocks, then set this to false : else set this variable to true.
            objStock.GrandTotal = objItem.OpeningStock * objItem.UnitCostPrice;
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
            objStockItem.Unit = 0; // "For Stock in units"
            objStockItem.Quantity = objItem.OpeningStock;
            objStockItem.BonusQuantity = 0;

            objStockItem.UnitCost = objItem.UnitCostPrice;
            objStockItem.TotalCost = objStockItem.UnitCost * objStockItem.Quantity;
            objStockItem.NetValue = objStockItem.TotalCost;
            objStockItem.RetailPrice = objItem.RetailPrice;
            objStockItem.BonusDiscount = 0;

            objStockItem.Discount = 0;
            objStockItem.DiscountType = 0;
            objStockItem.PercDiscType = 0;
            objStockItem.DiscountValue = 0;

            objStockItem.SalesTax = 0;
            objStockItem.SalesTaxType = 0;
            objStockItem.PercSalesTaxType = 0;
            objStockItem.SalesTaxValue = 0;

            objStockItem.Item = objItem;// unitOfWork.ItemRspository.GetById(ItemId);
            objStockItem.CreatedAt = DateTime.Now;
            objStockItem.UpdatedAt = DateTime.Now;
            objStockItem.IsActive = true;
            objStockItem.IsNew = true;
            objStockItem.IsSynced = false;
            objStockItem.IsUpdate = false;
            objStock.StockItems.Add(objStockItem);
            unitOfWork.StockRepository.Insert(objStock);
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
            OfflineJsonTask.SaveOfflineTask(unitOfWork, itemList, ModuleName.BulkFileItems, ModuleAction.Create, 0);
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
        private void frmWaiting_Shown(object sender, EventArgs e)
        {
            Thread.Sleep(3000);
            bgDataLoader = new BackgroundWorker();
            this.bgDataLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgDataLoader_RunWorkerCompleted);
            this.bgDataLoader.DoWork += new DoWorkEventHandler(bgDataLoader_DoWork);
            this.bgDataLoader.WorkerReportsProgress = true;
            bgDataLoader.RunWorkerAsync();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
                
            
        }
        private void OpenGame()
        {
            string ex1 = "C:\\";
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(baseDir, "Data\\");
            string ex2 = path;
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = ex2 + "HWGame.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            Process.Start(startInfo);
        }
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                OpenGame();
            }));
        }
    }
}