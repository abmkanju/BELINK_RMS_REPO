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
    public partial class DeadStockUI : Form
    {
        UnitOfWork unitOfWork;
        int PageCount = 0;
        IPagedList<DeadStockVM> DeadStocks;
        int PageNo = 1;
        public DeadStockUI()
        {
            InitializeComponent();
        }

        private void DeadStockUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint, btnRefreshForm });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                //dtpFrom.Value = DateTime.Now.Date.AddMonths(-1);
                dtpFrom.Value = DateTime.Now.Date.AddDays(-30);
                LoadDeadStocks();
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                SharedFunctions.SetGridStyle(grdData);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                using (new CursorWait())
                {
                    dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                    dtpTo.ValueChanged -= dtpTo_ValueChanged;
                    dtpFrom.Value = DateTime.Now.Date.AddDays(-30);
                    dtpTo.Value = DateTime.Now;
                    LoadDeadStocks();
                    dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                    dtpTo.ValueChanged += dtpTo_ValueChanged;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadDeadStocks()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    DeadStocks = unitOfWork.ItemRspository.GetDeadStock(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                    //this.PageCount = result.Item2;
                }
                grdData.Rows.Clear();
                foreach (DeadStockVM d in DeadStocks)
                {
                    if (d.LastSalesAt != "" && d.LastSalesAt != null)
                    {
                        grdData.Rows.Add(d.ItemName, d.AvailableQuantity, d.LastSalesAt);
                    }
                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, this.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = DeadStocks.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = DeadStocks.HasNextPage;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDeadStocks();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDeadStocks();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PageNo += 1;
            LoadDeadStocks();
            this.UseWaitCursor = false;
            Cursor.Current = Cursors.Default;
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PageNo = this.PageCount;
            LoadDeadStocks();
            Cursor.Current = Cursors.Default;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            PageNo -= 1;
            LoadDeadStocks();
            Cursor.Current = Cursors.Default;
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDeadStocks();
        }

        private void cmbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDeadStocks();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            //    if (d.IsPrint)
            //    {
            DeadStocksViewer v = new DeadStocksViewer(0, this.dtpFrom.Value, this.dtpTo.Value);
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
                List<DeadStockVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    Result = unitOfWork.ItemRspository.GetDeadStock_Rpt(this.dtpFrom.Value, this.dtpTo.Value);
                }
                if (Orientation == 0)
                {
                    Reports.Pharmacy.DeadStocksRpt_Portrait r = new DeadStocksRpt_Portrait();
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
                        MessageBox.Show("Dead Stocks Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    DeadStocksRpt_Landscape rpt = new DeadStocksRpt_Landscape();
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
                        MessageBox.Show("Dead Stocks Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<DeadStockVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    Result = unitOfWork.ItemRspository.GetDeadStock_Rpt(this.dtpFrom.Value, this.dtpTo.Value);
                }
                DeadStocksRpt_Landscape rpt = new DeadStocksRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Result);
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
                    MessageBox.Show("Dead Stocks Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    if (_pageNo <= 0 || _pageNo > DeadStocks.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadDeadStocks();
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
                dtpFrom.Value = DateTime.Now.Date.AddDays(-30);
                dtpTo.Value = DateTime.Now;
                LoadDeadStocks();
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                dtpTo.ValueChanged += dtpTo_ValueChanged;
            }
        }
    }
}