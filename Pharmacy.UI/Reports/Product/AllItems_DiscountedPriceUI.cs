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
    public partial class AllItems_DiscountedPriceUI : Form
    {
        enum FilterType { Default = 1, Category = 3, Item = 4 }
        FilterType filter = FilterType.Default;
        private UnitOfWork unitOfWork;
        IPagedList<DiscountedItemVM> pmList;
        int PageNo = 1;
        private int SelectedItemId = 0;
        private int DiscountId = 0;
        bool IsAllItems = false;
        public AllItems_DiscountedPriceUI(int DiscountId, bool IsAllItems)
        {
            InitializeComponent();
            this.DiscountId = DiscountId;
            this.IsAllItems = IsAllItems;
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        public AllItems_DiscountedPriceUI()
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
            LoadDiscountedItems();
        }

        private void LowStockUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                SharedFunctions.SetGridStyle(grdData);
                LoadCategories();
                LoadDiscountedItems();
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

        private void LoadDiscountedItems()
        {
            try
            {
                if (this.IsAllItems)
                {
                    this.ShowAllItemsDiscounts();
                }
                else
                {
                    this.ShowSelectedItemsDiscounts();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void ShowAllItemsDiscounts()
        {
            using (unitOfWork = new UnitOfWork())
            {
                if (filter == FilterType.Item)
                {
                    pmList = unitOfWork.ItemRspository.GetDiscountedItems_Item_all(this.DiscountId, ItemId: this.SelectedItemId, pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                }
                else if (filter == FilterType.Category)
                {
                    int cId = Convert.ToInt32(cmbCat.SelectedValue);
                    if (cId > 0)
                    {
                        pmList = unitOfWork.ItemRspository.GetDiscountedItems_cat_all(this.DiscountId, categoryId: cId, pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                    }
                    else
                    {
                        pmList = unitOfWork.ItemRspository.GetDiscountedItems_all(this.DiscountId, pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                    }
                }
                else
                {
                    pmList = unitOfWork.ItemRspository.GetDiscountedItems_all(this.DiscountId, pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                }
            }

            grdData.Rows.Clear();
            foreach (var i in pmList)
            {
                if (i.DiscountData.DiscountType == 0) // %
                {
                    i.DiscountData.DiscountedPrice = i.RetailPrice - ((i.RetailPrice / 100) * i.DiscountData.Discount);
                }
                else
                {
                    i.DiscountData.DiscountedPrice = i.RetailPrice - i.DiscountData.Discount;
                }
                grdData.Rows.Add(i.ItemId, i.ItemName, i.CategoryName, Math.Round(i.CostPrice, 2), Math.Round(i.RetailPrice, 2), Math.Round(i.DiscountData.Discount, 2), i.DiscountData.DiscountType == 0 ? "%" : "Value", i.DiscountData.Name, Math.Round(i.DiscountData.DiscountedPrice, 2));
            }
            btnFirstPage.Enabled = btnPrevious.Enabled = pmList.HasPreviousPage;
            btnLastPage.Enabled = btnNext.Enabled = pmList.HasNextPage;
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, pmList.PageCount);
        }

        private void ShowSelectedItemsDiscounts()
        {
            using (unitOfWork = new UnitOfWork())
            {
                if (filter == FilterType.Item)
                {
                    pmList = unitOfWork.ItemRspository.GetDiscountedItems_Item(this.DiscountId, ItemId: this.SelectedItemId, pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                }
                else if (filter == FilterType.Category)
                {
                    int cId = Convert.ToInt32(cmbCat.SelectedValue);
                    if (cId > 0)
                    {
                        pmList = unitOfWork.ItemRspository.GetDiscountedItems_cat(this.DiscountId, categoryId: cId, pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                    }
                    else
                    {
                        pmList = unitOfWork.ItemRspository.GetDiscountedItems(this.DiscountId, pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                    }
                }
                else
                {
                    pmList = unitOfWork.ItemRspository.GetDiscountedItems(this.DiscountId, pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                }
            }

            grdData.Rows.Clear();

            if (this.IsAllItems)
            {
                foreach (var i in pmList)
                {
                    if (i.DiscountData.DiscountType == 0) // %
                    {
                        i.DiscountData.DiscountedPrice = i.RetailPrice - ((i.RetailPrice / 100) * i.DiscountData.Discount);
                    }
                    else
                    {
                        i.DiscountData.DiscountedPrice = i.RetailPrice - i.DiscountData.Discount;
                    }
                    grdData.Rows.Add(i.ItemId, i.ItemName, i.CategoryName, Math.Round(i.CostPrice, 2), Math.Round(i.RetailPrice, 2), Math.Round(i.DiscountData.Discount, 2), i.DiscountData.DiscountType == 0 ? "%" : "Value", i.DiscountData.Name, Math.Round(i.DiscountData.DiscountedPrice, 2));
                }
            }
            else
            {
                foreach (var i in pmList)
                {
                    if (i.DiscountType == 0) // %
                    {
                        i.DiscountedPrice = i.RetailPrice - ((i.RetailPrice / 100) * i.Discount);
                    }
                    else
                    {
                        i.DiscountedPrice = i.RetailPrice - i.Discount;
                    }
                    grdData.Rows.Add(i.ItemId, i.ItemName, i.CategoryName, Math.Round(i.CostPrice, 2), Math.Round(i.RetailPrice, 2), Math.Round(i.Discount, 2), i.DiscountType == 0 ? "%" : "Value", i.DiscountName, Math.Round(i.DiscountedPrice, 2));
                }
            }
            btnFirstPage.Enabled = btnPrevious.Enabled = pmList.HasPreviousPage;
            btnLastPage.Enabled = btnNext.Enabled = pmList.HasNextPage;
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, pmList.PageCount);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<DiscountedItemVM> pmList = GetReportData();
                //dlgPrintOptions d = new dlgPrintOptions();
                //d.ShowDialog();
                //if (!d.IsCancelled)
                //{
                //    if (d.IsPrint)
                //{
                DiscountedItemViewer v = new DiscountedItemViewer(0, pmList);
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

        private List<DiscountedItemVM> GetReportData()
        {
            try
            {
                List<DiscountedItemVM> vm = new List<DiscountedItemVM>();
                List<DiscountedItemVM> list = new List<DiscountedItemVM>();

                if (IsAllItems)
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        if (filter == FilterType.Item)
                        {
                            list = unitOfWork.ItemRspository.GetDiscountedItems_Item_Rpt_all(this.DiscountId, ItemId: this.SelectedItemId);
                        }
                        else if (filter == FilterType.Category)
                        {
                            int cId = Convert.ToInt32(cmbCat.SelectedValue);
                            if (cId > 0)
                            {
                                list = unitOfWork.ItemRspository.GetDiscountedItems_cat_Rpt_all(this.DiscountId, categoryId: cId);
                            }
                            else
                            {
                                list = unitOfWork.ItemRspository.GetDiscountedItems_Rpt_all(this.DiscountId);
                            }
                        }
                        else
                        {
                            list = unitOfWork.ItemRspository.GetDiscountedItems_Rpt_all(this.DiscountId);
                        }
                    }
                }
                else
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        if (filter == FilterType.Item)
                        {
                            list = unitOfWork.ItemRspository.GetDiscountedItems_Item_Rpt(this.DiscountId, ItemId: this.SelectedItemId);
                        }
                        else if (filter == FilterType.Category)
                        {
                            int cId = Convert.ToInt32(cmbCat.SelectedValue);
                            if (cId > 0)
                            {
                                list = unitOfWork.ItemRspository.GetDiscountedItems_cat_Rpt(this.DiscountId, categoryId: cId);
                            }
                            else
                            {
                                list = unitOfWork.ItemRspository.GetDiscountedItems_Rpt(this.DiscountId);
                            }
                        }
                        else
                        {
                            list = unitOfWork.ItemRspository.GetDiscountedItems_Rpt(this.DiscountId);
                        }
                    }
                }
                if (this.IsAllItems)
                {
                    foreach (var i in list)
                    {
                        DiscountedItemVM v = new DiscountedItemVM();
                        v.DiscountName = i.DiscountData.Name;
                        if (i.DiscountData.DiscountType == 0) // %
                        {
                            v.DiscTypeString = "%";
                            v.DiscountedPrice = i.RetailPrice - ((i.RetailPrice / 100) * i.DiscountData.Discount);
                        }
                        else
                        {
                            v.DiscTypeString = "Value";
                            v.DiscountedPrice = i.RetailPrice - i.DiscountData.Discount;
                        }
                        v.RetailPrice = i.RetailPrice;
                        v.CostPrice = i.CostPrice;
                        v.ItemName = i.ItemName;
                        v.Discount = i.DiscountData.Discount;
                        vm.Add(v);
                    }
                }
                else
                {
                    foreach (var i in list)
                    {
                        DiscountedItemVM v = new DiscountedItemVM();
                        v.DiscountName = i.DiscountName;
                        if (i.DiscountType == 0) // %
                        {
                            v.DiscTypeString = "%";
                            v.DiscountedPrice = i.RetailPrice - ((i.RetailPrice / 100) * i.Discount);
                        }
                        else
                        {
                            v.DiscTypeString = "Value";
                            v.DiscountedPrice = i.RetailPrice - i.Discount;
                        }
                        v.RetailPrice = i.RetailPrice;
                        v.CostPrice = i.CostPrice;
                        v.ItemName = i.ItemName;
                        v.Discount = i.Discount;
                        vm.Add(v);
                    }
                }
                return vm;
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
            //    pmList = unitOfWork.ItemRspository.GetLowStockItems_BySupplier(SId, PageNo, SharedVariables.PageSize);
            //}
            //else
            //{
            //    pmList = unitOfWork.ItemRspository.GetLowStockItems(PageNo, SharedVariables.PageSize);
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
                List<DiscountedItemVM> Result = GetReportData();
                DiscountedItemRpt_Portrait rpt = new DiscountedItemRpt_Portrait();
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
                    MessageBox.Show("Discounted Price Report Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    if (_pageNo <= 0 || _pageNo > pmList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadDiscountedItems();
                }
            }
        }


        private void cmbCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter = FilterType.Category;
            this.PageNo = 1;
            LoadDiscountedItems();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadDiscountedItems();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = pmList.PageCount;
            LoadDiscountedItems();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadDiscountedItems();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDiscountedItems();
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
            LoadDiscountedItems();
        }

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}