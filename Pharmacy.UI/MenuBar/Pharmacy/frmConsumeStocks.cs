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
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using PagedList;
using Pharmacy.UI.Reports;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmConsumeStocks : Form
    {
        UnitOfWork unitOfWork;
        IPagedList<StockConsumptionItemVM> ConsumptionsList;

        int PageNo = 1;
        public frmConsumeStocks()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }

        private void btnConsumeStock_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmConsumeStock(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(frmConsumeStock_Closed);
        }

        private void frmConsumeStock_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmConsumeStocks_Load(object sender, EventArgs e)
        {
            try
            {
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                LoadStockConsumptions();
                LoadBatches();
                SharedFunctions.SetGridStyle(grdItems);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void LoadBatches()
        {
            cmbBatches.UseWaitCursor = true;
            List<string> Batches = unitOfWork.BatchRepository.GetAllUniqueBatches();
            //List<Batch> Batches = unitOfWork.BatchRepository.GetAll().ToList();
            //Batch b = new Batch();
            //b.BatchId = 0;
            string defaultOp = "Select";
            Batches.Insert(0, defaultOp);
            cmbBatches.DataSource = Batches;
            //cmbBatches.DisplayMember = "BatchName";
            //cmbBatches.ValueMember = "BatchId";
            cmbBatches.UseWaitCursor = false;
        }
        private void LoadStockConsumptions()
        {
            grdItems.Rows.Clear();
            string BatchSearch = cmbBatches.GetItemText(cmbBatches.SelectedItem).ToLower();
            if(BatchSearch == "select")
            {
                BatchSearch = "";
            }
            ConsumptionsList = null;// unitOfWork.StockConsumptionRespository.GetStockConsumptions(dtpFrom.Value, dtpTo.Value, txtSearchByName.Text.Trim().ToLower(), BatchSearch, PageNo, SharedVariables.PageSize);
            foreach (var c in ConsumptionsList)
            {
                grdItems.Rows.Add(c.StockConsumptionItemId, c.ItemName, c.Quantity, c.BatchName, c.ConsumptionTypeString, null, c.Comment, c.TotalCost, c.CreateDate, c.InvoiceId, null, c.ConsumptionTypeString == "Sales" ? "Print" : "", null);
            }
            btnNext.Enabled  = ConsumptionsList.HasNextPage;
            btnPrevious.Enabled = ConsumptionsList.HasPreviousPage;
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, ConsumptionsList.PageCount);
        }
        private void cmbRangeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "today")
            //{
            //    dtpFrom.Value = DateTime.Today;
            //    dtpTo.Value = DateTime.Today;
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "yesterday")
            //{
            //    dtpFrom.Value = DateTime.Today.AddDays(-1);
            //    dtpTo.Value = DateTime.Today.AddDays(-1);
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "last 7 days")
            //{
            //    dtpFrom.Value = DateTime.Today.AddDays(-6);
            //    dtpTo.Value = DateTime.Today;
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "last 30 days")
            //{
            //    dtpFrom.Value = DateTime.Today.AddDays(-29);
            //    dtpTo.Value = DateTime.Today;
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "this month")
            //{
            //    dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            //    dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "last month")
            //{
            //    DateTime LastMonth = DateTime.Today.AddMonths(-1);
            //    dtpFrom.Value = new DateTime(LastMonth.Year, LastMonth.Month, 1);
            //    dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "custom range")
            //{
            //    dtpFrom.Value = DateTime.Today;
            //    dtpTo.Value = DateTime.Today;
            //    return;
            //}
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadStockConsumptions();
        }

        private void txtSearchByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PageNo = 1;
                LoadStockConsumptions();
            }
        }

        private void cmbBatches_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBatches.SelectedIndex > 0)
            {
                PageNo = 1;
                LoadStockConsumptions();
            }
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
            dtpTo.Value = DateTime.Now;
            txtSearchByName.Text = "";
            //cmbRangeFilter.SelectedIndex = -1;
            cmbBatches.SelectedIndex = 0;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            PageNo++;
            LoadStockConsumptions();
            this.UseWaitCursor = false;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            PageNo--;
            LoadStockConsumptions();
            this.UseWaitCursor = false;
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            btnClearFilter.PerformClick();
            this.UseWaitCursor = true;
            PageNo = 1;
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
            LoadStockConsumptions();
            LoadBatches();
            this.UseWaitCursor = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ConsumptionsList.TotalItemCount > 0)
            {
                Reports.StockConsumptionsViewer v = new Reports.StockConsumptionsViewer(ConsumptionsList.ToList(), dtpFrom.Value, dtpTo.Value);
                v.Show();
            }
            else
            {
                MessageBox.Show("Not Items Found To Print", "Data Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                StockConsumptionsRpt r = new StockConsumptionsRpt();
                r.SetDataSource(ConsumptionsList);
                r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                r.SetParameterValue("FromDate", dtpFrom.Value);
                r.SetParameterValue("ToDate", dtpTo.Value);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    r.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Stock Consumptions Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                long StockConsumptionId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colStockConsumptionId"].Value);
                if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                {
                    Form f = SharedFunctions.OpenForm(new frmConsumeStockUpdate(StockConsumptionId), this.MdiParent);
                    f.FormClosed += new FormClosedEventHandler(frmConsumeStockUpdate_Closed);
                    f.Show();
                    return;
                }

                if (e.ColumnIndex == grdItems.Columns["ColDelete"].Index)
                {
                    DialogResult rs = MessageBox.Show("Are You Sure You Want to Delete Selected Stock Consumption", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (rs == System.Windows.Forms.DialogResult.OK)
                    {
                        //unitOfWork.StockConsumptionRespository.SetInActive(StockConsumptionId);
                        MessageBox.Show("Stock Consumption Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        unitOfWork = new UnitOfWork();
                        PageNo = 1;
                        btnRefresh.PerformClick();
                        return;
                    }
                }

                if (e.ColumnIndex == grdItems.Columns["colPrint"].Index)
                {
                    if (grdItems.Rows[e.RowIndex].Cells["colconsumptionType"].Value.ToString() == "Sales")
                    {
                        long InvoiceId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                        Reports.InvoiceViewer v = new InvoiceViewer(InvoiceId);
                        v.Show();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void frmConsumeStockUpdate_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadStockConsumptions();
        }
    }
}
