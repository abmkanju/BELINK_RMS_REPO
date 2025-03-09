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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Reflection;
using System.Windows.Forms.DataVisualization.Charting;

namespace Pharmacy.UI.Reports.Financial
{
    public partial class FinancialTransactionUI : Form
    {
        private UnitOfWork unitOfWork;
        List<InvoiceTransactionsVM> Transactions;
        int PageNo = 1;
        bool isPageChanged = false;
        public FinancialTransactionUI()
        {
            InitializeComponent();
            
            //lblloader.Visible = false;
        }

        private void FinancialTransactionUI_Load(object sender, EventArgs e)
        {
            cmbPaymentMode.SelectedIndex = 0;
            //SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh });
            //SharedFunctions.SetGridStyle(grdData);
            GridDesigner.CustomizeDataGridView(grdData);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpTo.ValueChanged -= dtpTo_ValueChanged;
                cmbPaymentMode.SelectedIndexChanged -= cmbPaymentMode_SelectedIndexChanged;
                cmbPaymentMode.SelectedIndex = 0;
                PageNo = 1;
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                txtInvoiceNoSearch.Text = "";
                LoadTransactions();
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                dtpTo.ValueChanged += dtpTo_ValueChanged;
                cmbPaymentMode.SelectedIndexChanged += cmbPaymentMode_SelectedIndexChanged;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        private void LoadTransactionsInBackground(object parameters)
        {
            try
            {
                lblloader.Invoke(new Action(() =>
                {
                    lblloader.Visible = true;
                }));
                
                var parameterValues = (Tuple<int , string , string , DateTime , DateTime , string>)parameters;
                int PaymentMode = parameterValues.Item1;
                var MRno = parameterValues.Item2;
                var InvoiceRefno = parameterValues.Item3;
                DateTime dtfrom = parameterValues.Item4;
                DateTime dtto = parameterValues.Item5;
                var Invoicenum = parameterValues.Item6;
                SummationsVM Sums = new SummationsVM();
                List<SummationsVM> GraphData = new List<SummationsVM>();
                SummationsVM Graph1 = new SummationsVM();
                using (unitOfWork = new UnitOfWork())
                {
                    
                    var MRnodoub = (MRno != "") ? Convert.ToDouble(MRno) : 0;
                    
                    var InvoiceRefLong = (InvoiceRefno != "") ? Convert.ToInt64(InvoiceRefno) : 0;
                    if (PaymentMode > 0)
                    {
                        Sums = unitOfWork.InvoiceRepository.GetSums_Financial(dtpFrom.Value, dtpTo.Value, cmbPaymentMode.SelectedIndex, MRnodoub, InvoiceRefLong);
                    }
                    else
                    {
                        Sums = unitOfWork.InvoiceRepository.GetSums_Financial(dtpFrom.Value, dtpTo.Value, MRnodoub, InvoiceRefLong);
                    }
                    Transactions = unitOfWork.InvoiceRepository.GetTransactions_FinancialPay(dtpFrom.Value, dtpTo.Value, PaymentMode, MRnodoub, InvoiceRefLong);
                    GraphData = unitOfWork.InvoiceRepository.GetPayment_Financial_Graph(dtpFrom.Value, dtpTo.Value, PaymentMode, MRnodoub, InvoiceRefLong);
                    
                    if (GraphData.Count == 0)
                    {
                        panel6.Invoke(new Action(() =>
                        {
                            panel6.Visible = false;
                        }));
                        
                    }
                    else
                    {
                        panel6.Invoke(new Action(() =>
                        {
                            panel6.Visible = true;
                        }));
                    }

                    if (Transactions.Count < 1)
                    {

                        dtpFrom.Invoke(new Action(() => {
                            dtpFrom.Enabled = true;
                        }));
                        dtpTo.Invoke(new Action(() => {
                            dtpTo.Enabled = true;
                        }));
                        cmbPaymentMode.Invoke(new Action(() => {
                            cmbPaymentMode.Enabled = true;
                        }));
                        txtInvoiceNoSearch.Invoke(new Action(() => {
                            txtInvoiceNoSearch.Enabled = true;
                        }));
                        txtMRNoSearch.Invoke(new Action(() => {
                            txtMRNoSearch.Enabled = true;
                        }));
                        lblloader.Invoke(new Action(() =>
                        {
                            lblloader.Visible = false;
                        }));
                        loadDataThread.Abort();
                    }

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
                        else if (v.PaymentType == 5)
                        {
                            v.PaymentTypeString = "Cheque";
                        }
                    }
                }
               
                if (Sums != null)
                {
                    string totalRevenue = Sums.TotalRevenue == null ? "0" : Math.Round((double)Sums.TotalRevenue, 2).ToString();
                    lblTotalRevenue.Invoke(new Action(() => lblTotalRevenue.Text = totalRevenue));
                }
                else
                {
                    lblTotalRevenue.Invoke(new Action(() => lblTotalRevenue.Text = "0"));
                }

                if (Sums != null)
                {
                    string grossSale = Sums.GrossSale == null ? "0" : Math.Round((double)Sums.GrossSale, 2).ToString();
                    lblGrossSale.Invoke(new Action(() => lblGrossSale.Text = grossSale));
                }
                else
                {
                    lblGrossSale.Invoke(new Action(() => lblGrossSale.Text = "0"));
                }
                

                grdData.Invoke(new Action(() =>
                {
                    grdData.Rows.Clear();
                    string Description = "";
                    string PaymentType = "";
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
                            
                            
                            
                            if (MRno != "")
                            {
                                if (Invoicenum != "")
                                {
                                    var inv = Convert.ToInt64(Invoicenum);
                                    if (t.InvoiceRefNo == inv)
                                    {
                                        if (t.Patient != null)
                                        {
                                            if (t.Patient.MRNo.ToString().Contains(MRno))
                                            {
                                                if (p.PaymentDate >= dtfrom && p.PaymentDate <= dtto)
                                                {
                                                   
                                                    grdData.Rows.Add(t.InvoiceId, t.InvoiceRefNo, t.Patient == null ? "" : t.Patient.MRNo.ToString(), t.Patient == null ? "" : t.Patient.Name, Description, Math.Round(t.GrandTotal, 2), Math.Round(p.Payment, 2), t.TotalDiscount, t.Due, t.Advance, PaymentType, p.PaymentDate, t.IsProcedureInvoice);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (t.Patient != null)
                                    {
                                        if (t.Patient.MRNo.ToString().Contains(MRno))
                                        {
                                            if (p.PaymentDate >= dtfrom && p.PaymentDate <= dtto)
                                            {
                                              
                                                grdData.Rows.Add(t.InvoiceId, t.InvoiceRefNo, t.Patient == null ? "" : t.Patient.MRNo.ToString(), t.Patient == null ? "" : t.Patient.Name, Description, Math.Round(t.GrandTotal, 2), Math.Round(p.Payment, 2), t.TotalDiscount, t.Due, t.Advance, PaymentType, p.PaymentDate, t.IsProcedureInvoice);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Invoicenum != "")
                                {
                                    var inv = Convert.ToInt64(Invoicenum);

                                    if (Convert.ToInt64(t.InvoiceRefNo) == inv)
                                    {
                                        if (p.PaymentDate >= dtfrom && p.PaymentDate <= dtto)
                                        {
                                           
                                            grdData.Rows.Add(t.InvoiceId, t.InvoiceRefNo, t.Patient == null ? "" : t.Patient.MRNo.ToString(), t.Patient == null ? "" : t.Patient.Name, Description, Math.Round(t.GrandTotal, 2), Math.Round(p.Payment, 2), t.TotalDiscount, t.Due, t.Advance, PaymentType, p.PaymentDate, t.IsProcedureInvoice);
                                        }
                                    }
                                }
                                else
                                {
                                    if (p.PaymentDate >= dtfrom && p.PaymentDate <= dtto)
                                    {
                                       
                                        grdData.Rows.Add(t.InvoiceId, t.InvoiceRefNo, t.Patient == null ? "" : t.Patient.MRNo.ToString(), t.Patient == null ? "" : t.Patient.Name, Description, Math.Round(t.GrandTotal, 2), Math.Round(p.Payment, 2), t.TotalDiscount, t.Due, t.Advance, PaymentType, p.PaymentDate, t.IsProcedureInvoice);
                                    }
                                }
                            }
                        }
                    }
                }));
                ChartPayments.Invoke(new Action(() =>
                {
                    ChartPayments.DataSource = GraphData;
                    ChartPayments.Series["Payments"].XValueMember = "PaymentTypeString";
                    ChartPayments.Series["Payments"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                    ChartPayments.Series["Payments"].YValueMembers = "TotalRevenue";
                    ChartPayments.Series["Payments"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                    ChartPayments1.DataSource = Sums;
                    ChartPayments1.Series["Payments"].XValueMember = "GrossSale";
                    ChartPayments1.Series["Payments"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
                    ChartPayments1.Series["Payments"].YValueMembers = "TotalRevenue";
                    ChartPayments1.Series["Payments"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                }));
                
                dtpFrom.Invoke(new Action(() => {
                    dtpFrom.Enabled = true;
                }));
                dtpTo.Invoke(new Action(() => {
                    dtpTo.Enabled = true;
                }));
                cmbPaymentMode.Invoke(new Action(() => {
                    cmbPaymentMode.Enabled = true;
                }));
                txtInvoiceNoSearch.Invoke(new Action(() => {
                    txtInvoiceNoSearch.Enabled = true;
                }));
                txtMRNoSearch.Invoke(new Action(() => {
                    txtMRNoSearch.Enabled = true;
                }));
                lblloader.Invoke(new Action(() =>
                {
                    lblloader.Visible = false;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }
        private Thread loadDataThread;
        private void LoadTransactions([CallerMemberName] string memberName = "")
        {
            lblloader.Visible = true;
            cmbPaymentMode.Enabled = false;
            dtpFrom.Enabled = false;
            dtpTo.Enabled = false;
            txtInvoiceNoSearch.Enabled = false;
            txtMRNoSearch.Enabled = false;
            
            int PaymentMode = cmbPaymentMode.SelectedIndex;
            var MRno = txtMRNoSearch.Text;
            var InvoiceRefno = txtInvoiceNoSearch.Text;
            DateTime dtfrom = Convert.ToDateTime(dtpFrom.Value.ToShortDateString());
            DateTime dtto = Convert.ToDateTime(dtpTo.Value.ToShortDateString()).AddDays(1);
            var Invoicenum = txtInvoiceNoSearch.Text;
            var parameters = Tuple.Create(PaymentMode, MRno , InvoiceRefno , dtfrom , dtto , Invoicenum);
            // Create a new background thread and start loading data
            loadDataThread = new Thread(LoadTransactionsInBackground);
            loadDataThread.Start(parameters);
            
           
            

            /*try
            {
                int PaymentMode = cmbPaymentMode.SelectedIndex; // index is matching actual values in db, otherwise will need to update this row of code
                SummationsVM Sums = new SummationsVM();
                List<SummationsVM> GraphData = new List<SummationsVM>();
                using (unitOfWork = new UnitOfWork())
                {//Implementation of Search By MR# Filter In Reports>Financial>Transaction 
                    var MRno = txtMRNoSearch.Text;
                    var MRnodoub =(MRno!="")? Convert.ToDouble(MRno):0;
                    var InvoiceRefno = txtInvoiceNoSearch.Text;
                    var InvoiceRefLong =(InvoiceRefno != "")? Convert.ToInt64(InvoiceRefno) :0;
                    if (cmbPaymentMode.SelectedIndex > 0)
                    {
                        Sums = unitOfWork.InvoiceRepository.GetSums_Financial(dtpFrom.Value, dtpTo.Value, cmbPaymentMode.SelectedIndex, MRnodoub, InvoiceRefLong);
                    }
                    else
                    {
                        Sums = unitOfWork.InvoiceRepository.GetSums_Financial(dtpFrom.Value, dtpTo.Value, MRnodoub, InvoiceRefLong);
                    }
                    Transactions = unitOfWork.InvoiceRepository.GetTransactions_FinancialPay(dtpFrom.Value, dtpTo.Value, PaymentMode , MRnodoub, InvoiceRefLong);
                    GraphData = unitOfWork.InvoiceRepository.GetPayment_Financial_Graph(dtpFrom.Value, dtpTo.Value, PaymentMode, MRnodoub, InvoiceRefLong);
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
                        else if (v.PaymentType == 5)
                        {
                            v.PaymentTypeString = "Cheque";
                        }
                    }
                }
                if (Sums != null)
                {
                    lblTotalRevenue.Text = Sums.TotalRevenue == null ? "0" : Math.Round((double)Sums.TotalRevenue, 2).ToString();
                }
                else
                {
                    lblTotalRevenue.Text = "0";
                }
                if (Sums != null)
                {
                    lblGrossSale.Text = Sums.GrossSale == null ? "0" : Math.Round((double)Sums.GrossSale, 2).ToString();
                }
                else
                {
                    lblGrossSale.Text = "0";
                }

                grdData.Rows.Clear();
                string Description = "";
                string PaymentType = "";
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
                        DateTime dtfrom = Convert.ToDateTime(dtpFrom.Value.ToShortDateString());
                        DateTime dtto = Convert.ToDateTime(dtpTo.Value.ToShortDateString()).AddDays(1);
                        var MRno = txtMRNoSearch.Text;
                        var Invoicenum = txtInvoiceNoSearch.Text;
                        if (MRno != "")
                        {
                            if(Invoicenum != "")
                            {
                                var inv = Convert.ToInt64(Invoicenum);
                                if (t.InvoiceRefNo == inv)
                                {
                                    if (t.Patient != null)
                                    {
                                        if (t.Patient.MRNo.ToString().Contains(MRno))
                                        {
                                            if (p.PaymentDate >= dtfrom && p.PaymentDate <= dtto) 
                                            {
                                                grdData.Rows.Add(t.InvoiceId, t.InvoiceRefNo, t.Patient == null ? "" : t.Patient.MRNo.ToString(), t.Patient == null ? "" : t.Patient.Name, Description, Math.Round(t.GrandTotal, 2), Math.Round(p.Payment, 2), t.TotalDiscount, t.Due, t.Advance, PaymentType, p.PaymentDate, t.IsProcedureInvoice);
                                            } 
                                        }
                                    }

                                }
                            }
                            else
                            {
                                if (t.Patient != null)
                                {
                                    if (t.Patient.MRNo.ToString().Contains(MRno))
                                    {
                                        if (p.PaymentDate >= dtfrom && p.PaymentDate <= dtto)
                                        {
                                            grdData.Rows.Add(t.InvoiceId, t.InvoiceRefNo, t.Patient == null ? "" : t.Patient.MRNo.ToString(), t.Patient == null ? "" : t.Patient.Name, Description, Math.Round(t.GrandTotal, 2), Math.Round(p.Payment, 2), t.TotalDiscount, t.Due, t.Advance, PaymentType, p.PaymentDate, t.IsProcedureInvoice);
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (Invoicenum != "")
                            {
                                var inv = Convert.ToInt64(Invoicenum);

                                if (Convert.ToInt64(t.InvoiceRefNo) == inv)
                                {
                                    if (p.PaymentDate >= dtfrom && p.PaymentDate <= dtto)
                                    {
                                        grdData.Rows.Add(t.InvoiceId, t.InvoiceRefNo, t.Patient == null ? "" : t.Patient.MRNo.ToString(), t.Patient == null ? "" : t.Patient.Name, Description, Math.Round(t.GrandTotal, 2), Math.Round(p.Payment, 2), t.TotalDiscount, t.Due, t.Advance, PaymentType, p.PaymentDate, t.IsProcedureInvoice);
                                    }
                                }
                            }
                            else
                            {
                                if (p.PaymentDate >= dtfrom && p.PaymentDate <= dtto)
                                {
                                    grdData.Rows.Add(t.InvoiceId, t.InvoiceRefNo, t.Patient == null ? "" : t.Patient.MRNo.ToString(), t.Patient == null ? "" : t.Patient.Name, Description, Math.Round(t.GrandTotal, 2), Math.Round(p.Payment, 2), t.TotalDiscount, t.Due, t.Advance, PaymentType, p.PaymentDate, t.IsProcedureInvoice);
                                }
                            }
                        }
                        
                    }
                }
                
                //btnFirstPage.Enabled = btnPrevious.Enabled = Transactions.HasPreviousPage;
                //btnLastPage.Enabled = btnNext.Enabled = Transactions.HasNextPage;
                //SharedFunctions.ShowPageNo(lblPageNo, PageNo, Transactions.PageCount);

                ChartPayments.DataSource = GraphData;
                ChartPayments.Series["Payments"].XValueMember = "PaymentTypeString";
                ChartPayments.Series["Payments"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                ChartPayments.Series["Payments"].YValueMembers = "TotalRevenue";
                ChartPayments.Series["Payments"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }*/
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            grdData.Rows.Clear();
            LoadTransactions();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            grdData.Rows.Clear();
            LoadTransactions();
        }

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            long InvoiceId = Convert.ToInt64(grdData.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
            bool IsProcedureInvoice = Convert.ToBoolean(grdData.Rows[e.RowIndex].Cells["colIsProcedureInvoice"].Value);
            if (e.ColumnIndex == grdData.Columns["colPrint"].Index)
            {
                if (IsProcedureInvoice)
                {
                    Reports.ProcedureInvoices.ProcedureInvoiceViewer v = new ProcedureInvoices.ProcedureInvoiceViewer(InvoiceId, true);
                    v.Show();
                }
                else
                {
                    Reports.POSInvoices.POSInvoiceViewer v = new POSInvoices.POSInvoiceViewer(InvoiceId, true);
                    v.Show();
                }
                //Reports.Pharmacy.PatientInvoiceViewer v = new PatientInvoiceViewer(InvoiceId);
                //v.Show();
                //return;
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
                    var MRno = txtMRNoSearch.Text;
                    var MRnodoub = (MRno != "") ? Convert.ToDouble(MRno) : 0;
                    var InvoiceRefno = txtInvoiceNoSearch.Text;
                    var InvoiceRefLong = (InvoiceRefno != "") ? Convert.ToInt64(InvoiceRefno) : 0;
                    Result = unitOfWork.InvoiceRepository.GetTransactions_Financial_Reports(dtpFrom.Value, dtpTo.Value, cmbPaymentMode.SelectedIndex, MRnodoub, InvoiceRefLong);
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
                        DateTime dtfrom = Convert.ToDateTime(dtpFrom.Value.ToShortDateString());
                        DateTime dtto = Convert.ToDateTime(dtpTo.Value.ToShortDateString()).AddDays(1);
                        foreach (InvoicePayment p in t.InvoicePayments)
                        {
                            
                            if (p.PaymentDate >= dtfrom &&  p.PaymentDate <= dtto) 
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
                                obj.InvoiceId = t.InvoiceRefNo;
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
            List<InvoiceTransactionsVM> Transactions = GetReportData();
            int PaymentMode = cmbPaymentMode.SelectedIndex;
            dlgPrintOptions d = new dlgPrintOptions();
            d.ShowDialog();
            if (!d.IsCancelled)
            {
                if (d.IsPrint)
                {
                    Reports.Financial.TrancationsViewer v = new TrancationsViewer(d.Orientation, this.dtpFrom.Value, this.dtpTo.Value, Transactions);
                    v.Show();
                }
                else if (d.IsSaveAsPdf)
                {
                    SavePdf(d.Orientation, Transactions);
                }
            }
        }

        private void SavePdf(int Orientation, List<InvoiceTransactionsVM> Transactions)
        {
            try
            {
                if (Orientation == 0)
                {
                    Reports.Financial.TransactionsRpt_Portrait rpt = new Reports.Financial.TransactionsRpt_Portrait();
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
                    Reports.Financial.TransactionsRpt_Landscape rpt = new Reports.Financial.TransactionsRpt_Landscape();
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
                Reports.Financial.TransactionsRpt_Landscape rpt = new TransactionsRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Transactions);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                //rpt.SetDataSource(Transactions);
                //rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                //rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                //rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Financial Transactions Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            LoadTransactions();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadTransactions();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTransactions();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            //PageNo = Transactions.PageCount;
            LoadTransactions();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PageNo = 1;
                LoadTransactions();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PageNo = 1;
                LoadTransactions();
            }
        }

        private void txtInvoiceNoSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtMRNoSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    PageNo = 1;
            //    SharedFunctions.isValidIntegerKey(sender, e);
            //    LoadTransactions();
            //}
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void cmbPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            
            LoadTransactions();
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
           /* if (e.KeyCode == Keys.Enter)
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
                    LoadTransactions();
                }
            }*/
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtInvoiceNoSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            cmbPaymentMode.SelectedIndexChanged -= cmbPaymentMode_SelectedIndexChanged;
            cmbPaymentMode.SelectedIndex = 0;
            PageNo = 1;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            txtInvoiceNoSearch.Text = "";
            txtMRNoSearch.Text = "";
            LoadTransactions();
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            cmbPaymentMode.SelectedIndexChanged += cmbPaymentMode_SelectedIndexChanged;
        }

        private void txtMRNoSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnExcel_MouseEnter(object sender, EventArgs e)
        {
            btnExcel.ForeColor = Color.White;
        }

        private void btnExcel_MouseLeave(object sender, EventArgs e)
        {
            btnExcel.ForeColor = Color.Black;
        }

        private void btnPrint_MouseEnter(object sender, EventArgs e)
        {
            btnPrint.ForeColor = Color.White;
        }

        private void btnPrint_MouseLeave(object sender, EventArgs e)
        {
            btnPrint.ForeColor = Color.Black;
        }

        private void btnRefresh_MouseEnter(object sender, EventArgs e)
        {
            btnRefresh.ForeColor = Color.White;
        }

        private void btnRefresh_MouseLeave(object sender, EventArgs e)
        {
            btnRefresh.ForeColor = Color.Black;
        }
    }
}