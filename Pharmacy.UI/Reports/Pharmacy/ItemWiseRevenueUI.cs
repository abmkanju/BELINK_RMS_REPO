using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using PagedList;
using Pharmacy.UI.MenuBar.Pharmacy;

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class ItemWiseRevenueUI : Form
    {
        enum FilterType { Default = 0, Category = 1, Item = 2, Date = 3 }
        UnitOfWork unitOfWork;
        ItemWiseRevenueMasterVM Revenue = new ItemWiseRevenueMasterVM();
        int PageNo = 1;
        public int PageCount { get; set; }
        List<BulkItemsVM> Items = new List<BulkItemsVM>();
        FilterType appliedFilter = FilterType.Default;
        List<Item> items = new List<Item>();
        string prevstockUnit = "";
        string intialstockunit = "";

        public ItemWiseRevenueUI()
        {
            InitializeComponent();
        }

        private void LoadCategories(UnitOfWork _unitOfWork)
        {
            List<Category> cats = _unitOfWork.CategoryRepository.GetAllActiveCategories();
            Category cSelect = new Category { CategoryId = 0, Name = "--Select Category--" };
            cats.Insert(0, cSelect);
            cmbCategory.DataSource = cats;
            cmbCategory.ValueMember = "CategoryId";
            cmbCategory.DisplayMember = "Name";
            cmbCategory.SelectedIndex = 0;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                using (new CursorWait())
                {
                    dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                    dtpTo.ValueChanged -= dtpTo_ValueChanged;
                    dtpFrom.Value = DateTime.Now;
                    dtpTo.Value = DateTime.Now;
                    appliedFilter = FilterType.Default;
                    toggleEvents(false);
                    prevstockUnit = "";
                    cmbStockUnit.SelectedIndex = 0;
                    using (unitOfWork = new UnitOfWork())
                    {
                        LoadCategories(unitOfWork);
                        LoadItemsData(unitOfWork, appliedFilter);
                    }
                    LoadItems();
                    toggleEvents(true);
                    dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                    dtpTo.ValueChanged += dtpTo_ValueChanged;
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ItemWiseSaleUI_Load(object sender, EventArgs e)
        {
            try
            {
                appliedFilter = FilterType.Default;
                toggleEvents(false);
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint, btnRefreshForm });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                //LoadItems();
                prevstockUnit = "";
                using (unitOfWork = new UnitOfWork())
                {
                    LoadCategories(unitOfWork);
                    LoadItemsData(unitOfWork, appliedFilter);
                }
                LoadItems();
                SharedFunctions.SetGridStyle(grdData);
                toggleEvents(true);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }

        private void toggleEvents(bool Register)
        {
            if (Register)
            {
                cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
                cmbItems.SelectedIndexChanged += cmbItems_SelectedIndexChanged;
            }
            else
            {
                cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
                cmbItems.SelectedIndexChanged -= cmbItems_SelectedIndexChanged;
            }
        }
        private void LoadItems()
        {
            using(UnitOfWork un = new UnitOfWork())
            {
                items = un.ItemRspository.GetActiveItems();
            }
        }
        private void LoadItemsData(UnitOfWork _unitOfWork, FilterType filter)
        {
            try
            {
                grdData.Rows.Clear();
                if (filter == FilterType.Category)
                {
                    Revenue = _unitOfWork.ItemRspository.GetItemWiseRevenue_Category(PageNo, SharedVariables.PageSize, Convert.ToInt32(cmbCategory.SelectedValue) , dtpFrom.Value, dtpTo.Value);
                }
                else if (filter == FilterType.Item)
                {
                    Revenue = _unitOfWork.ItemRspository.GetItemWiseRevenue_Item(PageNo, SharedVariables.PageSize, Convert.ToInt32(cmbItems.SelectedValue) , dtpFrom.Value, dtpTo.Value);
                }
                else // filter.Default will be applied here
                {
                    Revenue = _unitOfWork.ItemRspository.GetItemWiseRevenue_Date(PageNo, SharedVariables.PageSize, dtpFrom.Value, dtpTo.Value);
                    //Revenue = _unitOfWork.ItemRspository.GetItemWiseRevenue_All(PageNo, SharedVariables.PageSize);
                }
                
                if (Revenue == null)
                {
                    return;
                }
                ItemsVM vm = new ItemsVM();

                if (intialstockunit == "")
                {
                    cmbStockUnit.SelectedIndex = 0;
                    intialstockunit = "intialized";
                }
                var StockUnit = cmbStockUnit.SelectedItem.ToString();
                if (StockUnit.Equals("Pack", StringComparison.OrdinalIgnoreCase) && !prevstockUnit.Equals("Pack", StringComparison.OrdinalIgnoreCase))
                {
                    StockUnitChanged();
                }
                else if (StockUnit.Equals("Unit", StringComparison.OrdinalIgnoreCase) && !prevstockUnit.Equals("Unit", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (ItemWiseRevenueVM i in Revenue.ItemWiseRevenue)
                    {
                        using (unitOfWork = new UnitOfWork())
                        {
                            vm = unitOfWork.ItemRspository.GetItemWithAvQtywithouthold(i.ItemId);
                        }
                        grdData.Rows.Add(i.ItemName, i.AvailableStock, vm.ConsumedStock, Math.Round(i.Revenue, 2));
                    }
                }
               
                
                //ItemSales = Result.ItemWiseSales;
                lblRetailValue.Text = Math.Round(Revenue.ItemWiseRevenue.Sum(i => i.Revenue), 2).ToString();//.GrandSaleRevenue.ToString();
                chartItemRevenue.DataSource = Revenue.ItemWiseRevenue;
                chartItemRevenue.Series["SaleItems"].XValueMember = "ItemName";
                chartItemRevenue.Series["SaleItems"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                chartItemRevenue.Series["SaleItems"].YValueMembers = "Revenue";
                chartItemRevenue.Series["SaleItems"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                //double val = (double)((double)RevenueList.Count / (double)SharedVariables.PageSize);
                //this.PageCount = (int)Math.Ceiling(val);
                //LoadCustomPagination();
                SharedFunctions.ShowPageNo(lblPageNo, this.PageNo, Revenue.PageCount);
                btnNext.Enabled = btnLastPage.Enabled = Revenue.HasNextPage;
                btnPrevious.Enabled = btnFirstPage.Enabled = Revenue.HasPreviousPage;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            prevstockUnit = "";
            using (unitOfWork = new UnitOfWork())
            {
                LoadItemsData(unitOfWork, appliedFilter);
            }
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            PageNo = 1;
            //cmbItems.SelectedIndex = 0;
            prevstockUnit = "";
            using (unitOfWork = new UnitOfWork())
            {
                LoadItemsData(unitOfWork, appliedFilter);
            }
            dtpTo.ValueChanged += dtpTo_ValueChanged;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            //LoadCustomPagination();
            prevstockUnit = "";
            using (unitOfWork = new UnitOfWork())
            {
                LoadItemsData(unitOfWork, appliedFilter); // this time it will pick last applied filter
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = this.Revenue.PageCount;
            prevstockUnit = "";
            using (unitOfWork = new UnitOfWork())
            {
                LoadItemsData(unitOfWork, appliedFilter); // this time it will pick last applied filter
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            prevstockUnit = "";
            using (unitOfWork = new UnitOfWork())
            {
                LoadItemsData(unitOfWork, appliedFilter); // this time it will pick last applied filter
            }
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadCustomPagination();
        }

        private void LoadCustomPagination()
        {
            //List<InvoiceItemVM> items = RevenueList.Skip((PageNo - 1) * SharedVariables.PageSize).Take(SharedVariables.PageSize).ToList();
            //grdData.Rows.Clear();
            //foreach (InvoiceItemVM i in items)
            //{
            //    grdData.Rows.Add(i.ItemName, Math.Round(i.Revenue, 2));
            //}
            //SharedFunctions.ShowPageNo(lblPageNo, PageNo, PageCount);
            //btnFirstPage.Enabled = btnPrevious.Enabled = PageNo > 1;
            //btnLastPage.Enabled = btnNext.Enabled = PageNo < PageCount;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            List<ItemWiseRevenueVM> RevenueList = GetReportData();
            //if (d.IsPrint)
            //{
            //ItemWiseRevenueViewer v = new ItemWiseRevenueViewer(d.Orientation, this.dtpFrom.Value, this.dtpTo.Value, RevenueList);
            ItemWiseRevenueViewer v = new ItemWiseRevenueViewer(0, this.dtpFrom.Value, this.dtpTo.Value, RevenueList);
            v.Show();
            //}
            //else if (d.IsSaveAsPdf)
            //{
            //    SavePdf(d.Orientation);
            //}
            //}
        }

        private List<ItemWiseRevenueVM> GetReportData()
        {
            List<ItemWiseRevenueVM> Revenue = new List<ItemWiseRevenueVM>();
            using (unitOfWork = new UnitOfWork())
            {
                if (appliedFilter == FilterType.Category)
                {
                    Revenue = unitOfWork.ItemRspository.GetItemWiseRevenue_Category_Rpt(Convert.ToInt32(cmbCategory.SelectedValue));
                }
                else if (appliedFilter == FilterType.Item)
                {
                    Revenue = unitOfWork.ItemRspository.GetItemWiseRevenue_Item_Rpt( Convert.ToInt32(cmbItems.SelectedValue));
                }
                else // filter.Default will be applied here
                {
                    Revenue = unitOfWork.ItemRspository.GetItemWiseRevenue_Date_Rpt(dtpFrom.Value, dtpTo.Value);
                }
            }
            return Revenue;
            //List<InvoiceItemVM> Revenues = new List<InvoiceItemVM>();
            //using (unitOfWork = new UnitOfWork())
            //{
            //    if (cmbItems.SelectedIndex > 0)
            //    {
            //        //Result = unitOfWork.ItemRspository.GetItemWiseRevenue(Convert.ToInt32(cmbItems.SelectedValue));
            //        Revenues = unitOfWork.ItemRspository.GetItemWiseRevenue(dtpFrom.Value, dtpTo.Value, Convert.ToInt32(cmbItems.SelectedValue));
            //    }
            //    else
            //    {
            //        Revenues = unitOfWork.ItemRspository.GetItemWiseRevenue(dtpFrom.Value, dtpTo.Value);
            //    }
            //}
            //return Revenues;
        }

        private void SavePdf(int Orientation)
        {
            try
            {
                List<ItemWiseSaleVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    if (cmbItems.SelectedIndex > 0)
                    {
                        Result = unitOfWork.ItemRspository.GetItemWiseRevenue_Rpt(Convert.ToInt32(cmbItems.SelectedValue));
                    }
                    else
                    {
                        Result = unitOfWork.ItemRspository.GetItemWiseRevenue_Rpt(this.dtpFrom.Value, this.dtpTo.Value);
                    }
                }
                if (Orientation == 0)
                {
                    Reports.Pharmacy.ItemWiswRevenueRpt_Portrait r = new ItemWiswRevenueRpt_Portrait();
                    r.SetDataSource(Result);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    r.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    r.SetParameterValue("DateTo", this.dtpTo.Value);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        r.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("ItemWise Revenue Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    ItemWiseRevenueRpt_Landscape rpt = new ItemWiseRevenueRpt_Landscape();
                    rpt.SetDataSource(Result);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("ItemWise Revenue Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (SharedVariables.AdminPractiseSetting == null)
                {
                    throw new Exception("Failed to load report headers, please try after login again");
                }
                List<ItemWiseRevenueVM> Result = this.GetReportData();
                //using (unitOfWork = new UnitOfWork())
                //{
                //    if (cmbItems.SelectedIndex > 0)
                //    {
                //        Result = unitOfWork.ItemRspository.GetItemWiseRevenue_Rpt(Convert.ToInt32(cmbItems.SelectedValue));
                //    }
                //    else
                //    {
                //        Result = unitOfWork.ItemRspository.GetItemWiseRevenue_Rpt(this.dtpFrom.Value, this.dtpTo.Value);
                //    }
                //}

                ItemWiseRevenueRpt_Landscape rpt = new ItemWiseRevenueRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Result);
                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("ItemWise Revenue Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }
        private void LoadItemsByCategory(int catId, UnitOfWork _unitOfWork)
        {
            CategoryVM cat = new CategoryVM();
            List<BulkItemsVM> items = new List<BulkItemsVM>();

            if (catId > 0) // select items by category
            {
                cat = unitOfWork.CategoryRepository.GetCategoryActiveItems(catId);
                ItemsVM i = new ItemsVM { ItemId = 0, ItemName = "---Select Item---" };
                cat.Items.Insert(0, i);
                cmbItems.DataSource = cat.Items;
            }
            else // select all items
            {
                items = SharedVariables.BulkLoadedItemsList;
                BulkItemsVM i = new BulkItemsVM { ItemId = 0, ItemName = "---Select Item---" };
                Items.Insert(0, i);
                cmbItems.DataSource = items;
            }
            cmbItems.ValueMember = "ItemId";
            cmbItems.DisplayMember = "ItemName";
            cmbItems.SelectedIndex = 0;
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > this.Revenue.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    //this.PageNo = _pageNo;
                    //LoadCustomPagination();
                    prevstockUnit = "";
                    using (unitOfWork = new UnitOfWork())
                    {
                        LoadItemsData(unitOfWork, appliedFilter);
                    }
                }
            }
        }

        private void cmbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbItems.SelectedIndex <= 0)
            {
                return;
            }
            appliedFilter = FilterType.Item;
            PageNo = 1;
            prevstockUnit = "";
            using (unitOfWork = new UnitOfWork())
            {

                LoadItemsData(unitOfWork, appliedFilter);
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (new CursorWait())
                {
                    this.appliedFilter = FilterType.Default;
                    int selectedValue = Convert.ToInt32(cmbCategory.SelectedValue);

                    prevstockUnit = "";
                    using (unitOfWork = new UnitOfWork())
                    {
                        if (selectedValue > 0)
                        {
                            LoadItemsByCategory(Convert.ToInt32(cmbCategory.SelectedValue), unitOfWork);
                            appliedFilter = FilterType.Category;
                        }
                        LoadItemsData(unitOfWork, appliedFilter);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            appliedFilter = FilterType.Default;
            cmbItems.SelectedIndex = -1;
            prevstockUnit = "";
            cmbStockUnit.SelectedIndex = 0;
            toggleEvents(false);
            using (unitOfWork = new UnitOfWork())
            {
                LoadCategories(unitOfWork);
                LoadItemsData(unitOfWork, appliedFilter);
            }
            LoadItems();
            toggleEvents(true);
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            
        }
        private void StockUnitChanged()
        {
            PageNo = 1;

            var stockunit = cmbStockUnit.Text;

            if (!string.Equals(stockunit, prevstockUnit, StringComparison.OrdinalIgnoreCase))
            {
                ItemsVM vm = new ItemsVM();
                grdData.Rows.Clear();
                prevstockUnit = stockunit;
                if (stockunit.Equals("Pack", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var itemRevdet in Revenue.ItemWiseRevenue)
                    {
                        var item = items.Find(i => i.ItemID == itemRevdet.ItemId);
                        var conversionUnit = item?.ConversionUnit ?? 1;
                        using (unitOfWork = new UnitOfWork())
                        {
                            vm = unitOfWork.ItemRspository.GetItemWithAvQtywithouthold(itemRevdet.ItemId);
                        }
                        grdData.Rows.Add(itemRevdet.ItemName, itemRevdet.AvailableStock / conversionUnit, vm.ConsumedStock/conversionUnit, Math.Round(itemRevdet.Revenue, 2));
                    }
                }
                else
                {
                    foreach (var i in Revenue.ItemWiseRevenue)
                    {
                        using (unitOfWork = new UnitOfWork())
                        {
                            vm = unitOfWork.ItemRspository.GetItemWithAvQtywithouthold(i.ItemId);
                        }
                        grdData.Rows.Add(i.ItemName, i.AvailableStock, vm.ConsumedStock, Math.Round(i.Revenue,2));
                    }
                }
            }
        }

        private void cmbStockUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var stockunit = cmbStockUnit.SelectedItem.ToString();
            if (stockunit != prevstockUnit)
            {
                StockUnitChanged();
            }
        }

    }
}