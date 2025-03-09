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
    public partial class ItemWisePurchaseUI : Form
    {
        UnitOfWork unitOfWork;
        IPagedList<ItemWisePurchaseVM> ItemPurchases;
        int PageNo = 1;
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        List<Item> Items = new List<Item>();
        string prevstockUnit = "";
        string intialstockunit = ""; 
        public ItemWisePurchaseUI()
        {
            InitializeComponent();
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
                    prevstockUnit = "";
                    cmbStockUnit.SelectedIndex = 0;
                    LoadItems();
                    LoadItemsData();
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
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint, btnRefreshForm });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                LoadItems();
                LoadItemsData();
                SharedFunctions.SetGridStyle(grdData);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }
        private void bgWItemsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                cmbItems.Enabled = false;
                lblLoadingItems.Visible = true;
            });

            //if (SharedVariables.POSItems == null)
            //{
            using (unitOfWork = new UnitOfWork())
            {
                Items = unitOfWork.ItemRspository.GetActiveItems().ToList();
            }
            //}
        }

        private void bgWItemsLoader_RunWokrerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmbItems.SelectedIndexChanged -= cmbItems_SelectedIndexChanged;
            Item select = new Item { ItemID = 0, ItemName = "Select Item" };
            Items.Insert(0, select);
            cmbItems.DataSource = Items;
            cmbItems.ValueMember = "ItemId";
            cmbItems.DisplayMember = "ItemName";
            cmbItems.SelectedIndexChanged += cmbItems_SelectedIndexChanged;
            cmbItems.SelectedIndex = 0;
            cmbItems.Enabled = true;
            lblLoadingItems.Visible = false;
        }
        private void LoadItems()
        {
            if (!this.bgWItemsLoader.IsBusy)
            {
                this.bgWItemsLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWItemsLoader_DoWork);
                this.bgWItemsLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWItemsLoader_RunWokrerCompleted);
                this.bgWItemsLoader.RunWorkerAsync();
            }
        }

        private void LoadItemsData()
        {
            try
            {
                grdData.Rows.Clear();
                ItemWisePurchaseMasterVM Result = new ItemWisePurchaseMasterVM();
                using (unitOfWork = new UnitOfWork())
                {
                    if (cmbItems.SelectedIndex > 0)
                    {
                        Result = unitOfWork.ItemRspository.GetItemWisePurchases(dtpFrom.Value, dtpTo.Value, Convert.ToInt32(cmbItems.SelectedValue));
                    }
                    else
                    {
                        Result = unitOfWork.ItemRspository.GetItemWisePurchases(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                    }
                }
                ItemPurchases = Result.ItemWisePurchases;
                if (intialstockunit == "")
                {
                    cmbStockUnit.SelectedIndex = 0;
                    intialstockunit = "initialized";
                }
                var StockUnit = cmbStockUnit.SelectedItem.ToString();
                var totalSale = 0;
                if (StockUnit.Equals("Pack", StringComparison.OrdinalIgnoreCase) && !prevstockUnit.Equals("Pack", StringComparison.OrdinalIgnoreCase))
                {
                    StockUnitChanged();
                }
                else if (StockUnit.Equals("Unit", StringComparison.OrdinalIgnoreCase) && !prevstockUnit.Equals("Unit", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (ItemWisePurchaseVM i in ItemPurchases)
                    {
                        grdData.Rows.Add(i.ItemName, i.AvailableStock, i.PurchaseCount);
                        totalSale += i.PurchaseCount;
                    }
                    lblTotalSales.Text = totalSale.ToString();
                    chartItemSales.DataSource = ItemPurchases;
                    chartItemSales.Series["PurchaseItems"].XValueMember = "ItemName";
                    chartItemSales.Series["PurchaseItems"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                    chartItemSales.Series["PurchaseItems"].YValueMembers = "PurchaseCount";
                    chartItemSales.Series["PurchaseItems"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;

                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, ItemPurchases.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = ItemPurchases.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = ItemPurchases.HasNextPage;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            cmbItems.SelectedIndex = 0;
            prevstockUnit = "";
            LoadItemsData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            cmbItems.SelectedIndex = 0;
            prevstockUnit = "";
            LoadItemsData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            prevstockUnit = "";
            LoadItemsData();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = ItemPurchases.PageCount;
            prevstockUnit = "";
            LoadItemsData();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            prevstockUnit = "";
            LoadItemsData();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            prevstockUnit = "";
            LoadItemsData();
        }

        private void cmbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            prevstockUnit = "";
            LoadItemsData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            //    if (d.IsPrint)
            //    {
            //        if (cmbItems.SelectedIndex > 0)
            //        {
            ItemWisePurchaseViewer v = new ItemWisePurchaseViewer(0, Convert.ToInt32(cmbItems.SelectedValue), this.dtpFrom.Value, this.dtpTo.Value);
            v.Show();
            //    }
            //    else
            //    {
            //        ItemSalesViewer v = new ItemSalesViewer(d.Orientation, this.dtpFrom.Value, this.dtpTo.Value);
            //        v.Show();
            //    }
            //}
            //else if (d.IsSaveAsPdf)
            //{
            //    SavePdf(d.Orientation);
            //}
            //}
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
                        Result = unitOfWork.ItemRspository.GetItemWiseSales_Rpt(this.dtpFrom.Value, this.dtpTo.Value, Convert.ToInt32(cmbItems.SelectedValue));
                    }
                    else
                    {
                        Result = unitOfWork.ItemRspository.GetItemWiseSales_Rpt(this.dtpFrom.Value, this.dtpTo.Value);
                    }
                }
                if (Orientation == 0)
                {
                    Reports.Pharmacy.ItemSalesRpt_Portrait r = new ItemSalesRpt_Portrait();
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
                        MessageBox.Show("ItemWise Sales Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    ItemSalesRpt_Landscape rpt = new ItemSalesRpt_Landscape();
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
                        MessageBox.Show("ItemWise Sales Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                List<ItemWisePurchaseVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    if (cmbItems.SelectedIndex > 0)
                    {
                        Result = unitOfWork.ItemRspository.GetItemWisePurchase_Rpt(this.dtpFrom.Value, this.dtpTo.Value, Convert.ToInt32(cmbItems.SelectedValue));
                    }
                    else
                    {
                        Result = unitOfWork.ItemRspository.GetItemWisePurchase_Rpt(this.dtpFrom.Value, this.dtpTo.Value);
                    }
                }
                ItemPurchasesReport_Landscape rpt = new ItemPurchasesReport_Landscape();
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
                    MessageBox.Show("ItemWise Purchases Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > ItemPurchases.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    prevstockUnit = "";
                    LoadItemsData();
                }
            }
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            using (new CursorWait())
            {
                PageNo = 1;
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpTo.ValueChanged -= dtpTo_ValueChanged;
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                cmbStockUnit.SelectedIndex = 0;
                prevstockUnit = "";
                LoadItems();
                LoadItemsData();
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                dtpTo.ValueChanged += dtpTo_ValueChanged;
            }
        }
        private void StockUnitChanged()
        {
            PageNo = 1;

            var stockunit = cmbStockUnit.Text;

            if (!string.Equals(stockunit, prevstockUnit, StringComparison.OrdinalIgnoreCase))
            {
                grdData.Rows.Clear();
                prevstockUnit = stockunit;
                var totalSale = 0;
                if (stockunit.Equals("Pack", StringComparison.OrdinalIgnoreCase))
                {
                    List<ItemWisePurchaseVM> vmlist = new List<ItemWisePurchaseVM>();
                    foreach (var itemPurchedet in ItemPurchases)
                    {
                        ItemWisePurchaseVM vm = new ItemWisePurchaseVM();
                        var item = Items?.Find(i => i.ItemID == itemPurchedet.ItemId);
                        var conversionUnit = item?.ConversionUnit ?? 1;
                        vm.ItemName = itemPurchedet.ItemName;
                        vm.PurchaseCount = itemPurchedet.PurchaseCount / conversionUnit;
                        grdData.Rows.Add(itemPurchedet.ItemName, itemPurchedet.AvailableStock / conversionUnit, vm.PurchaseCount);
                        totalSale += vm.PurchaseCount;
                        vmlist.Add(vm);
                    }
                    lblTotalSales.Text = totalSale.ToString();
                    chartItemSales.DataSource = vmlist;
                    chartItemSales.Series["PurchaseItems"].XValueMember = "ItemName";
                    chartItemSales.Series["PurchaseItems"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                    chartItemSales.Series["PurchaseItems"].YValueMembers = "PurchaseCount";
                    chartItemSales.Series["PurchaseItems"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                }
                else
                {
                    foreach (var i in ItemPurchases)
                    {
                        grdData.Rows.Add(i.ItemName, i.AvailableStock, i.PurchaseCount);
                        totalSale += i.PurchaseCount;
                    }
                    lblTotalSales.Text = totalSale.ToString();
                    chartItemSales.DataSource = ItemPurchases;
                    chartItemSales.Series["PurchaseItems"].XValueMember = "ItemName";
                    chartItemSales.Series["PurchaseItems"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                    chartItemSales.Series["PurchaseItems"].YValueMembers = "PurchaseCount";
                    chartItemSales.Series["PurchaseItems"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;

                }
            }
        }

        private void cmbStockUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var stockunit = cmbStockUnit.SelectedItem.ToString();
            if(stockunit != prevstockUnit)
            {
                StockUnitChanged();
            }
        }
    }
}