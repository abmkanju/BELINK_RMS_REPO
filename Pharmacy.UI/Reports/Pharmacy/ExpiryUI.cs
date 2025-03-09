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
    public partial class ExpiryUI : Form
    {
        UnitOfWork unitOfWork;
        IPagedList<ExpiryVM> ExpiryItems;
        int PageNo = 1;
        public ExpiryUI()
        {
            InitializeComponent();
        }

        private void ToggleEvents(bool register)
        {
            if (register)
            {
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                dtpTo.ValueChanged += dtpTo_ValueChanged;
            }
            else
            {
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpTo.ValueChanged -= dtpTo_ValueChanged;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                using (new CursorWait())
                {
                    ToggleEvents(false);
                    dtpFrom.Value = DateTime.Now;
                    dtpTo.Value = DateTime.Now.Date.AddDays(30);
                    LoadExpiry();
                    ToggleEvents(true);
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ExpiryUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint, btnRefreshForm });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                ToggleEvents(false);
                dtpTo.Value = DateTime.Now.Date.AddDays(30);
                LoadExpiry();
                ToggleEvents(true);
                SharedFunctions.SetGridStyle(grdData);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }
        private void LoadExpiry()
        {
            try
            {
                grdData.Rows.Clear();
                using (unitOfWork = new UnitOfWork())
                {
                    ExpiryItems = unitOfWork.ItemRspository.GetItemsExpiry(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                }

                
                foreach (ExpiryVM e in ExpiryItems)
                {
                    ItemDetailVM vm = new ItemDetailVM();
                    //ItemsVM item = new ItemsVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        //item = unitOfWork.ItemRspository.GetItemWithAvQtywithoutexpiry(e.ItemId);
                        vm = unitOfWork.ItemRspository.GetBatchStockByItemId_1(e.ItemId);
                    }
                    foreach (BatchStockVM v in vm.BatchStockList)
                    {
                        
                        if ((v.ConsumedStock < v.ExpiredStock && v.BatchName == e.BatchName) || (e.ExpiryDate >= DateTime.Now && v.BatchName == e.BatchName))
                        {
                            e.TotalCost = v.CostPrice * (e.Quantity - v.ConsumedStock);
                            e.TotalRetailValue = v.RetailPrice * (e.Quantity - v.ConsumedStock);
                            grdData.Rows.Add(e.ItemId, e.ItemName, e.BatchName, e.Quantity-v.ConsumedStock, e.ExpiryDate, e.StockAdditionDate, e.TotalCost, e.TotalRetailValue);
                        }
                    }
                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, ExpiryItems.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = ExpiryItems.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = ExpiryItems.HasNextPage;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadExpiry();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadExpiry();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadExpiry();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = ExpiryItems.PageCount;
            LoadExpiry();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadExpiry();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadExpiry();
        }

        private void cmbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadExpiry();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            //    if (d.IsPrint)
            //    {
            ExpiryViewer v = new ExpiryViewer(0, this.dtpFrom.Value, this.dtpTo.Value);
            v.Show();
            //    }
            //    else if (d.IsSaveAsPdf)
            //    {
            //        SavePdf(d.Orientation);
            //    }
            //}
        }

        private void SavePdf(int Orientation)
        {
            try
            {
                List<ExpiryVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    Result = unitOfWork.ItemRspository.GetItemsExpiry_Rpt(this.dtpFrom.Value, this.dtpTo.Value);
                }
                if (Orientation == 0)
                {
                    Reports.Pharmacy.ExpiryRpt_Portraitrpt r = new ExpiryRpt_Portraitrpt();
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
                        MessageBox.Show("Items Expiry Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    ExpiryRpt_Landscape rpt = new ExpiryRpt_Landscape();
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
                        MessageBox.Show("Items Expiry Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<ExpiryVM> Result;
                List<ExpiryVM> Result1 = new List<ExpiryVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    Result = unitOfWork.ItemRspository.GetItemsExpiry_Rpt(this.dtpFrom.Value, this.dtpTo.Value);
                }
                foreach (ExpiryVM ee in Result)
                {
                    ItemDetailVM vm = new ItemDetailVM();
                    //ItemsVM item = new ItemsVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        //item = unitOfWork.ItemRspository.GetItemWithAvQtywithoutexpiry(e.ItemId);
                        vm = unitOfWork.ItemRspository.GetBatchStockByItemId_1(ee.ItemId);
                    }
                    foreach (BatchStockVM v in vm.BatchStockList)
                    {
                        if ((v.ConsumedStock < v.ExpiredStock && v.BatchName == ee.BatchName) || (ee.ExpiryDate >= DateTime.Now && v.BatchName == ee.BatchName))
                        {
                            ee.Quantity = ee.Quantity - v.ConsumedStock;
                            ee.TotalCost = v.CostPrice * ee.Quantity;
                            ee.TotalRetailValue = v.RetailPrice * ee.Quantity;
                            Result1.Add(ee);
                        }
                    }
                }
                ExpiryRpt_Landscape rpt = new ExpiryRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Result1);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
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
                    MessageBox.Show("Items Expiry Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    if (_pageNo <= 0 || _pageNo > ExpiryItems.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadExpiry();
                }
            }
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            using (new CursorWait())
            {
                PageNo = 1;
                ToggleEvents(false);
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now.Date.AddDays(30);
                LoadExpiry();
                ToggleEvents(true);
            }
        }
    }
}