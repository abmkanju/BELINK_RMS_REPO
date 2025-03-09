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
using Pharmacy.UI.Reports.Pharmacy;
using System.Globalization;
namespace Pharmacy.UI.Reports.Financial
{
    public partial class PaymentModeUI : Form
    {
        private UnitOfWork unitOfWork;
        IPagedList<InvoiceTransactionsVM> Transactions;
        int PageNo = 1;
        bool isPageChanged = false;
        public PaymentModeUI()
        {
            InitializeComponent();
        }

        private void TransactionUI_Load(object sender, EventArgs e)
        {
            cmbPaymentMode.SelectedIndex = 0;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnExcel, btnPrint});
            SharedFunctions.SetGridStyle(grdData);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpTo.ValueChanged -= dtpTo_ValueChanged;
                cmbPaymentMode.SelectedIndexChanged -= cmbPaymentMode_SelectedIndexChanged;
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                cmbPaymentMode.SelectedIndex = 0;
                PageNo = 1;
                LoadPayments();
                cmbPaymentMode.SelectedIndexChanged += cmbPaymentMode_SelectedIndexChanged;
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                dtpTo.ValueChanged += dtpTo_ValueChanged;
                return true;
                
            }    
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LoadPayments()
        {
            try
            {
                int PaymentMode = cmbPaymentMode.SelectedIndex; // index is matching actual values in db, otherwise will need to update this row of code
                SummationsVM Sums = new SummationsVM();
                List<SummationsVM> GraphData = new List<SummationsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    if (cmbPaymentMode.SelectedIndex > 0)
                    {
                        Sums = unitOfWork.InvoiceRepository.GetSums_Financial(dtpFrom.Value, dtpTo.Value , cmbPaymentMode.SelectedIndex);
                    }
                    else
                    {
                        Sums = unitOfWork.InvoiceRepository.GetSums_Financial(dtpFrom.Value, dtpTo.Value);
                    }
                    Transactions = null;/*unitOfWork.InvoiceRepository.GetTransactions_FinancialPay(dtpFrom.Value, dtpTo.Value, PaymentMode, PageNo, SharedVariables.PageSize);*/
                    GraphData = unitOfWork.InvoiceRepository.GetPayment_Financial_Graph(dtpFrom.Value, dtpTo.Value, PaymentMode);
                    foreach (SummationsVM v in GraphData)
                    {
                        if (v.PaymentType == 1)
                        {
                            v.PaymentTypeString = "Cash";
                        }
                        else if (v.PaymentType == 2)
                        {
                            v.PaymentTypeString = "Charge";
                        }
                        else if (v.PaymentType == 3)
                        {
                            v.PaymentTypeString = "Online Payment";
                        }
                        else if (v.PaymentType == 4)
                        {
                            v.PaymentTypeString = "Debit/Credit Card";
                        }
                        else if(v.PaymentType == 5)
                        {
                            v.PaymentTypeString = "Cheque";
                        }
                    }
                }
                //if (Sums != null)
                //{
                //    lblTotalRevenue.Text = Sums.TotalRevenue == null ? "" : Math.Round((double)Sums.TotalRevenue, 2).ToString();
                //}
                //else
                //{
                //    lblTotalRevenue.Text = "0";
                //}
                grdData.Rows.Clear();
                string Description = "";
                string PaymentType = "";
                if(Transactions == null)
                {
                    return;
                }
                foreach (InvoiceTransactionsVM t in Transactions)
                {
                    Description = "";
                    if (t.IsProcedureInvoice)
                    {
                        foreach (InvoiceItemVM i in t.InvoiceItems)
                        {
                            Description += i.ProcedureName + ", ";
                        }
                        if (!string.IsNullOrEmpty(Description))
                        {
                            Description = Description.Trim().TrimEnd(',');
                        }
                    }
                    else
                    {
                        foreach (InvoiceItemVM i in t.InvoiceItems)
                        {
                            Description += i.ItemName + ", ";
                        }
                        if (!string.IsNullOrEmpty(Description))
                        {
                            Description = Description.Trim().TrimEnd(',');
                        }
                    }
                    foreach (InvoicePayment p in t.InvoicePayments)
                    {
                        switch (p.PaymentType)
                        {
                            case 1: PaymentType = "Cash"; break;
                            case 2: PaymentType = "Charge"; break;
                            case 3: PaymentType = "Online Payment"; break;
                            case 4: PaymentType = "Debit/Credit Card"; break;
                            case 5: PaymentType = "Cheque"; break;
                            default: PaymentType = "Cash"; break;
                        }
                        grdData.Rows.Add(t.InvoiceId, t.Patient == null ? "" : t.Patient.MRNo.ToString(), t.Patient == null ? "" : t.Patient.Name, Description, Math.Round(t.GrandTotal, 2), Math.Round(p.Payment, 2), PaymentType, p.PaymentDate, t.IsProcedureInvoice);
                    }
                }
                var Sum = Transactions.Sum(t => t.InvoicePayments.Sum(ip=>ip.Payment));
                if (Sum >0)
                {
                    lblTotalRevenue.Text = Sum <0 ? "0" : Math.Round((double)Sum, 2).ToString();
                }
                else
                {
                    lblTotalRevenue.Text = "0";
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = Transactions.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = Transactions.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, Transactions.PageCount);

                ChartPayments.DataSource = GraphData;
                ChartPayments.Series["Payments"].XValueMember = "PaymentTypeString";
                ChartPayments.Series["Payments"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                ChartPayments.Series["Payments"].YValueMembers = "TotalRevenue";
                ChartPayments.Series["Payments"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadPayments();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadPayments();
        }

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) { return; }
                long InvoiceId = Convert.ToInt64(grdData.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                bool isProcedureInvoice = Convert.ToBoolean(grdData.Rows[e.RowIndex].Cells["colIsProcedureInvoice"].Value);
                if (e.ColumnIndex == grdData.Columns["colPrint"].Index)
                {
                    if (isProcedureInvoice)
                    {
                        Reports.ProcedureInvoices.ProcedureInvoiceViewer v = new ProcedureInvoices.ProcedureInvoiceViewer(InvoiceId, true);
                        v.Show();
                    }
                    else // POS invoice
                    {
                        Reports.POSInvoices.POSInvoiceViewer v = new POSInvoices.POSInvoiceViewer(InvoiceId, true);
                        v.Show();
                    }
                    //Reports.Pharmacy.PatientInvoiceViewer v = new PatientInvoiceViewer(InvoiceId);
                    //v.Show();
                    //return;
                }
            }
            catch (Exception)
            {
                throw;
            }

            //if (e.ColumnIndex == grdData.Columns["colEdit"].Index)
            //{
            //    frmInvoice f = new frmInvoice(true, false, InvoiceId);
            //    f.Show();
            //    return;
            //}

            //if (e.ColumnIndex == grdData.Columns["colReturn"].Index)
            //{
            //    frmInvoice f = new frmInvoice(false, true, InvoiceId);
            //    f.Show();
            //    return;
            //}

        }

        private List<InvoiceTransactionsVM> GetReportData()
        {
            try
            {
                List<InvoiceTransactionsVM> Result;
                List<InvoiceTransactionsVM> FinalResult = new List<InvoiceTransactionsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    string Description = "";
                    string PaymentType = "";
                    Result = unitOfWork.InvoiceRepository.GetTransactions_Financial_Reports(dtpFrom.Value, dtpTo.Value, cmbPaymentMode.SelectedIndex);
                    foreach (InvoiceTransactionsVM t in Result)
                    {
                        Description = "";
                        if (t.IsProcedureInvoice)
                        {
                            foreach (InvoiceItemVM i in t.InvoiceItems)
                            {
                                Description += i.ProcedureName + ", ";
                            }
                            if (!string.IsNullOrEmpty(Description))
                            {
                                Description = Description.Trim().TrimEnd(',');
                            }
                        }
                        else
                        {
                            foreach (InvoiceItemVM i in t.InvoiceItems)
                            {
                                Description += i.ItemName + ", ";
                            }
                            if (!string.IsNullOrEmpty(Description))
                            {
                                Description = Description.Trim().TrimEnd(',');
                            }
                        }
                        foreach (InvoicePayment p in t.InvoicePayments)
                        {
                            switch (p.PaymentType)
                            {
                                case 1: PaymentType = "Cash"; break;
                                case 2: PaymentType = "Charge"; break;
                                case 3: PaymentType = "Online Payment"; break;
                                case 4: PaymentType = "Debit/Credit Card"; break;
                                case 5: PaymentType = "Cheque"; break;
                                default: PaymentType = "Cash"; break;
                            }
                            InvoiceTransactionsVM obj = new InvoiceTransactionsVM();
                            obj.InvoiceId = t.InvoiceId;
                            if (t.Patient != null)
                            {
                                obj.MrNo = t.Patient.MRNo.ToString();
                                obj.PatientName = t.Patient.Name;
                            }
                            else
                            {
                                obj.MrNo = "";
                                obj.PatientName = "";
                            }
                            obj.Description = Description;
                            obj.SubTotal = t.SubTotal;
                            obj.GrandTotal = t.GrandTotal;
                            obj.TotalPaid = p.Payment;
                            obj.TotalDiscount = t.TotalDiscount;
                            obj.Due = t.Due;
                            obj.Advance = t.Advance;
                            obj.PaymentType = PaymentType;
                            obj.PaymentDate = p.PaymentDate;
                            FinalResult.Add(obj);
                        }
                    }
                }
                return FinalResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<InvoiceTransactionsVM> PaymentsList = GetReportData();
            int PaymentMode = cmbPaymentMode.SelectedIndex;
            dlgPrintOptions d = new dlgPrintOptions();
            d.ShowDialog();
            if (!d.IsCancelled)
            {
                if (d.IsPrint)
                {
                    Reports.Financial.PaymentModeViewer v = new PaymentModeViewer(d.Orientation, this.dtpFrom.Value, this.dtpTo.Value, PaymentsList);
                    v.Show();
                }
                else if (d.IsSaveAsPdf)
                {
                    SavePdf(d.Orientation, PaymentsList);
                }
            }
        }

        private void SavePdf(int Orientation, List<InvoiceTransactionsVM> Transactions)
        {
            try
            {
                if (Orientation == 0)
                {
                    Reports.Financial.PaymentModeRpt_Portrait rpt = new Reports.Financial.PaymentModeRpt_Portrait();
                    rpt.SetDataSource(Transactions);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Financial Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    Reports.Financial.PaymentModeRpt_Landscape rpt = new PaymentModeRpt_Landscape();
                    rpt.SetDataSource(Transactions);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Financial Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<InvoiceTransactionsVM> Transactions = GetReportData();
                Reports.Financial.PaymentModeRpt_Landscape rpt = new PaymentModeRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Transactions);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                //rpt.SetDataSource(Transactions);
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Financial Payment Mode Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadPayments();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadPayments();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadPayments();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = Transactions.PageCount;
            LoadPayments();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PageNo = 1;
                LoadPayments();
            }
        }

        private void txtInvoiceNoSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void cmbPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadPayments();
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > Transactions.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadPayments();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            cmbPaymentMode.SelectedIndexChanged -= cmbPaymentMode_SelectedIndexChanged;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            cmbPaymentMode.SelectedIndex = 0;
            PageNo = 1;
            LoadPayments();
            cmbPaymentMode.SelectedIndexChanged += cmbPaymentMode_SelectedIndexChanged;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
        }
    }
}