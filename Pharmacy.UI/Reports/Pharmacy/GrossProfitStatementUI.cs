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
using System.Globalization;
using System.Data.Entity;

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class GrossProfitStatementUI : Form
    {
        UnitOfWork unitOfWork;
        public GrossProfitStatementUI()
        {
            InitializeComponent();
        }
        private void CalculateCardValues(DataGridView grd)
        {
            double TotalRevenue = 0;
            double TotalCostOfSales = 0;
            double TotalDiscCostOfSales = 0;
            double TotalDiscount = 0;
            double TotalExpense = 0;
            double TotalRefund = 0;
            double Profit = 0;
            double ProfitMargin = 0;
            foreach (DataGridViewRow row in grd.Rows)
            {
                TotalRevenue =TotalRevenue + (double)row.Cells[1].Value;
                TotalCostOfSales = TotalCostOfSales + (double)row.Cells[2].Value;
                TotalDiscCostOfSales = TotalDiscCostOfSales + (double)row.Cells[3].Value;
                TotalDiscount = TotalDiscount + (double)row.Cells[4].Value;
                TotalExpense = TotalExpense + (double)row.Cells[5].Value;
                TotalRefund = TotalRefund + (double)row.Cells[6].Value;
            }
            Profit = TotalRevenue - TotalDiscCostOfSales;
            Profit = Profit - TotalExpense;
            ProfitMargin = Profit / TotalRevenue * 100;
            lblDiscountedCostofSales.Text = TotalDiscCostOfSales.ToString();
            lblTotalDiscount.Text = TotalDiscount.ToString();
            lblTotalExpense.Text = TotalExpense.ToString();
            lblTotalRefund.Text = TotalRefund.ToString();
            lblTotalSales.Text = TotalRevenue.ToString();
            lblTotalCost.Text = TotalCostOfSales.ToString();
            lblProfitMargin.Text = Math.Round(ProfitMargin , 2).ToString();
            label2.Text = Math.Round(Profit , 2).ToString();

        }
        private void ProfiStatementUI_Load(object sender, EventArgs e)
        {
            try
            {

                LoadProfitStatements(); 
                CalculateCardValues(grdData);
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnPrint, btnRefreshForm});
                //SharedFunctions.SetGridStyle(grdData);
                GridDesigner.CustomizeDataGridView(grdData);
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
                    dtpFrom.Value = DateTime.Now;
                    dtpTo.Value = DateTime.Now;
                    LoadProfitStatements();
                    CalculateCardValues(grdData);
                    dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                    dtpTo.ValueChanged += dtpTo_ValueChanged;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private List<ProfitStatementTransactionVM> GetGrossReportData()
        {
            List<ProfitStatementTransactionVM> PSt = new List<ProfitStatementTransactionVM>();
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    PSt = unitOfWork.InvoiceRepository.GetGrossProfitStatements_Reports(dtpFrom.Value, dtpTo.Value);
                }
                foreach (ProfitStatementTransactionVM d in PSt)
                {
                    d.Profit = d.TotalRevenue - d.DiscountedCostOfSales;
                    d.Profit = d.Profit - d.Expense;


                    d.Date = d.PaymentDate.ToShortDateString();
                     
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return PSt;
        }
        private void LoadProfitStatements()
        {
            double TotalExpense = 0;
            double Totaldebit = 0;

            try
            {
                List<ProfitStatementTransactionVM> vm = new List<ProfitStatementTransactionVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    vm = unitOfWork.ProfitStatementTransactionsRepository.GetGrossProfitStatements(dtpFrom.Value, dtpTo.Value);
                }
                
                double profit = 0;
                grdData.Rows.Clear();
                foreach (ProfitStatementTransactionVM d in vm)
                {
                    //TotalRevenue += d.TotalRevenue;
                    //NetTotalSales += d.NetTotalSale;
                    //// GrandTotals += d.GrandTotals;
                    //TotalCost += d.CostOfSales;
                    //TotalDiscount += d.Discount;
                    ////profit = d.TotalRevenue-d.CostOfSales-d.Discount;
                    ////if (profit < 0) { profit = 0; }
                    profit = d.TotalRevenue - d.DiscountedCostOfSales;
                    profit = profit - d.Expense;

                    // (profit < 0) { profit = 0; }
                    grdData.Rows.Add(Convert.ToDateTime(d.PaymentDate).ToString("yyyy-MM-dd"), Math.Round(d.TotalRevenue, 2), Math.Round(d.CostOfSales, 2), Math.Round(d.DiscountedCostOfSales, 2), Math.Round(d.Discount, 2), Math.Round(d.Expense, 2), Math.Round(d.TotalRefund, 2), Math.Round(((profit / d.TotalRevenue) * 100), 2), Math.Round(profit, 2));
                }
                
                //double TotalProfit = (TotalRevenue - TotalCost - TotalDiscount);
               
                //if (TotalProfit < 0)
                //{
                //    TotalProfit = 0;
                //}
                //else
                //{
                lblTotalExpense.Text = Math.Round(Totaldebit - TotalExpense, 2).ToString();
               
               
                //}
                //btnFirstPage.Enabled = btnPrevious.Enabled = Discounts.HasPreviousPage;
                //btnLastPage.Enabled = btnNext.Enabled = Discounts.HasNextPage;
                //SharedFunctions.ShowPageNo(lblPageNo, PageNo, Discounts.PageCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadProfitStatements();
            CalculateCardValues(grdData);
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadProfitStatements();
            CalculateCardValues(grdData);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            LoadProfitStatements();
            CalculateCardValues(grdData);
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            LoadProfitStatements();
            CalculateCardValues(grdData);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            LoadProfitStatements();
            CalculateCardValues(grdData);
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            LoadProfitStatements();
            CalculateCardValues(grdData);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //double TotalExpense = 0;
            //List<ProfitStatementVM> PStmts = GetGrossReportData();
            ////dlgPrintOptions d = new dlgPrintOptions();
            ////d.ShowDialog();
            ////if (!d.IsCancelled)
            ////{
            ////    if (d.IsPrint)
            ////    {
            //ProfitStatementViewer v = new ProfitStatementViewer(0, this.dtpFrom.Value, this.dtpTo.Value, PStmts);
            //v.Show();
            ////    }
            ////    else if (d.IsSaveAsPdf)
            ////    {
            ////        SavePdf(d.Orientation, PStmts);
            ////    }
            ////}
        }

        private void SavePdf(int Orientation, List<ProfitStatementVM> ProfitStatements)
        {
            try
            {
                if (Orientation == 0)
                {
                    Reports.Pharmacy.ProfitStatementRpt_Portrair r = new ProfitStatementRpt_Portrair();
                    r.SetDataSource(ProfitStatements);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    r.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    r.SetParameterValue("DateTo", this.dtpTo.Value);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        r.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Profit Statements Reports Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    ProfitStatementRpt_Landscape rpt = new ProfitStatementRpt_Landscape();
                    rpt.SetDataSource(ProfitStatements);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Discounts Reports Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<ProfitStatementVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    //Result = GetGrossReportData();
                }
                ProfitStatementRpt_Landscape rpt = new ProfitStatementRpt_Landscape();
                //rpt.Database.Tables[1].SetDataSource(Result);

                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[0].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                else
                {
                    //rpt.Database.Tables[0].SetDataSource(Result);
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
                    MessageBox.Show("Profit Statements Report Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProfitStatements();
            CalculateCardValues(grdData);
        }

        private void chk100Per_CheckedChanged(object sender, EventArgs e)
        {
            LoadProfitStatements();
            CalculateCardValues(grdData);
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {

            double TotalExpense = 0;
            List<ProfitStatementTransactionVM> PStmts = GetGrossReportData();
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            //    if (d.IsPrint)
            //    {
            GrossProfitStatementViewer v = new GrossProfitStatementViewer(0, this.dtpFrom.Value, this.dtpTo.Value, PStmts);
            v.Show();
            //    }
            //    else if (d.IsSaveAsPdf)
            //    {
            //        SavePdf(d.Orientation, PStmts);
            //    }
            //}
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            using (new CursorWait())
            {
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpTo.ValueChanged -= dtpTo_ValueChanged;
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                LoadProfitStatements();
                CalculateCardValues(grdData);
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                dtpTo.ValueChanged += dtpTo_ValueChanged;
            }
        }
    }
}