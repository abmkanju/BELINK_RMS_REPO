using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using PagedList;
using System.IO;
using Pharmacy.UI.Reports.Product;
using Pharmacy.UI.MenuBar.Pharmacy;
namespace Pharmacy.UI.Reports.Products
{
    public partial class ProductPriceUI : Form
    {
        enum FilterType { Default = 1, Date = 2, Category = 3, Item = 4 }
        FilterType filter = FilterType.Default;
        private UnitOfWork unitOfWork;
        IPagedList<ProductPriceVM> ppList;
        int PageNo = 1;
        private int SelectedItemId = 0;
        public ProductPriceUI()
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;

            if (this.SelectedItemId > 0)
            {
                filter = FilterType.Item;
            }
            else
            {
                filter = FilterType.Default;
            }
            LoadProductPrices();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpTo.ValueChanged -= dtpTo_ValueChanged;
                this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                uC_SearchItems1.SetText = "";
                filter = FilterType.Default;
                LoadCategories();
                LoadProductPrices();
                this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                dtpTo.ValueChanged += dtpTo_ValueChanged;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LowStockUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                SharedFunctions.SetGridStyle(grdData);
                LoadCategories();
                LoadProductPrices();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }

        private void LoadCategories()
        {
            cmbCat.SelectedIndexChanged -= cmbCat_SelectedIndexChanged;
            List<Category> cats = new List<Category>();
            using (unitOfWork = new UnitOfWork())
            {
                cats = unitOfWork.CategoryRepository.GetAllActiveCategories();
            }
            Category c = new Category
            {
                CategoryId = 0,
                Name = "Select Category"
            };
            cats.Insert(0, c);
            cmbCat.DataSource = cats;
            cmbCat.ValueMember = "CategoryId";
            cmbCat.DisplayMember = "Name";
            cmbCat.SelectedIndexChanged += cmbCat_SelectedIndexChanged;
        }

        private void LoadProductPrices()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    if (filter == FilterType.Date)
                    {
                        ppList = unitOfWork.ItemRspository.GetProductPrices(DateFrom: dtpFrom.Value, DateTo: dtpTo.Value, pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                    }
                    else if (filter == FilterType.Item)
                    {
                        ppList = unitOfWork.ItemRspository.GetProductPrices_Item(ItemId: this.SelectedItemId, pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                    }
                    else if (filter == FilterType.Category)
                    {
                        int cId = Convert.ToInt32(cmbCat.SelectedValue);
                        if (cId > 0)
                        {
                            ppList = unitOfWork.ItemRspository.GetProductPrices_cat(categoryId: cId, pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                        }
                        else
                        {
                            ppList = unitOfWork.ItemRspository.GetProductPrices(pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                        }
                    }
                    else
                    {
                        ppList = unitOfWork.ItemRspository.GetProductPrices(pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                    }
                }

                grdData.Rows.Clear();
                foreach (var i in ppList)
                {                   
                    grdData.Rows.Add(i.ItemId, i.ItemName, i.Category, Math.Round(i.CostPrice, 2), Math.Round(i.RetailPrice, 2));
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = ppList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = ppList.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, ppList.PageCount);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<ProductPriceVM> ppList = GetReportData();
                //dlgPrintOptions d = new dlgPrintOptions();
                //d.ShowDialog();
                //if (!d.IsCancelled)
                //{
                //    if (d.IsPrint)
                //{
                ProductPriceViewer v = new ProductPriceViewer(0, ppList);
                v.Show();
                //}
                //else if (d.IsSaveAsPdf)
                //{
                //    SavePdf(d.Orientation);
                //}
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading report data, please try loading again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<ProductPriceVM> GetReportData()
        {
            try
            {
                List<ProductPriceVM> list = new List<ProductPriceVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    if (filter == FilterType.Date)
                    {
                        list = unitOfWork.ItemRspository.GetProductPrices_Rpt(DateFrom: dtpFrom.Value, DateTo: dtpTo.Value);
                    }
                    else if (filter == FilterType.Item)
                    {
                        list = unitOfWork.ItemRspository.GetProductPrices_Item_Rpt(ItemId: this.SelectedItemId);
                    }
                    else if (filter == FilterType.Category)
                    {
                        int cId = Convert.ToInt32(cmbCat.SelectedValue);
                        if (cId > 0)
                        {
                            list = unitOfWork.ItemRspository.GetProductPrices_cat_Rpt(categoryId: cId);
                        }
                        else
                        {
                            list = unitOfWork.ItemRspository.GetProductPrices_Rpt();
                        }
                    }
                    else
                    {
                        list = unitOfWork.ItemRspository.GetProductPrices_Rpt();
                    }
                }
                //foreach (var i in list)
                //{
                //    if (i.DiscountType == 0) // %
                //    {
                //        i.DiscTypeString = "%";
                //        i.DiscountedPrice = i.RetailPrice - ((i.RetailPrice / 100) * i.Discount);
                //    }
                //    else
                //    {
                //        i.DiscTypeString = "Value";
                //        i.DiscountedPrice = i.RetailPrice - i.Discount;
                //    }
                //}
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetReportsData()
        {
            //long SId = Convert.ToInt64(cmbSuppliers.SelectedValue);
            //if (SId > 0)
            //{
            //    ppList = unitOfWork.ItemRspository.GetLowStockItems_BySupplier(SId, PageNo, SharedVariables.PageSize);
            //}
            //else
            //{
            //    ppList = unitOfWork.ItemRspository.GetLowStockItems(PageNo, SharedVariables.PageSize);
            //}
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (SharedVariables.AdminPractiseSetting == null)
                {
                    throw new Exception("Failed to load report headers, please try after login again");
                }
                List<ProductPriceVM> Result = GetReportData();
                ProductPriceRpt_Portrait rpt = new ProductPriceRpt_Portrait();
                rpt.Database.Tables[0].SetDataSource(Result);
                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                //rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Product Price Report Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        public string GetLowStockExcelSheet()
        {
            return "";
            //try
            //{
            //    if (SharedVariables.AdminPractiseSetting == null)
            //    {
            //        throw new Exception("Failed to load report headers, please try after login again");
            //    }
            //    List<LowStockVM> Result = GetReportData();
            //    LowStocksRpt_Landscape rpt = new LowStocksRpt_Landscape();
            //    rpt.Database.Tables[0].SetDataSource(Result);
            //    if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
            //    {
            //        rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
            //    }
            //    rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
            //    rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
            //    rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
            //    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
            //    try
            //    {
            //        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");
            //        System.IO.Directory.CreateDirectory(filePath);
            //        filePath = Path.Combine(filePath, "LowStocksEmail" + SharedFunctions.GetTimestamp(DateTime.Now) + ".xls");
            //        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, filePath);
            //        return filePath;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception("Unable to extract data to excel sheet");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            //    return null;
            //}
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > ppList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadProductPrices();
                }
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            this.PageNo = 1;
            LoadProductPrices();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            this.PageNo = 1;
            LoadProductPrices();
        }

        private void cmbCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter = FilterType.Category;
            this.PageNo = 1;
            LoadProductPrices();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadProductPrices();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = ppList.PageCount;
            LoadProductPrices();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadProductPrices();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadProductPrices();
        }

        private void btnSearchItems_Click(object sender, EventArgs e)
        {
            frmSearchItem f = new frmSearchItem();
            f.OnItemSelected += frmSearchItem_OnItemSelected;
            f.ShowDialog();
        }


        private void frmSearchItem_OnItemSelected(int ItemId, int Quantity)
        {
            this.PageNo = 1;
            filter = FilterType.Item;
            this.SelectedItemId = ItemId;
            LoadProductPrices();
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadProductPrices();
        }
    }
}