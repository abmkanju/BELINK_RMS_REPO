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
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmStockPayments : Form
    {
        UnitOfWork unitOfWork;
        long SupplierId = 0;
        public frmStockPayments()
        {
            InitializeComponent();
        }

        public frmStockPayments(long SupplierId)
        {
            InitializeComponent();
            this.SupplierId = SupplierId;
        }

        private void frmStockPayments_Load(object sender, EventArgs e)
        {
            SupplierStockVM suppStocks = new SupplierStockVM();
            using (unitOfWork = new UnitOfWork())
            {
                suppStocks = unitOfWork.SupplierRepository.GetSupplierStockAndPayments(this.SupplierId);
            }
            lblSupplierName.Text = suppStocks.Supplier.Name;
            double GrandTotal = 0;
            double TotalReturnAmount = 0;
            double TotalPaid = 0;
            double Due = 0;
            double varTotalPaid = 0;
            foreach (StockBillVM s in suppStocks.StockPayments)
            {
                varTotalPaid = (double)(s.TotalPaid == null ? 0 : s.TotalPaid);
                TotalPaid += varTotalPaid;
                GrandTotal += s.TotalAmount;
                TotalReturnAmount += s.TotalReturnStockAmount;
                if ((GrandTotal - TotalPaid - s.TotalReturnStockAmount) > 0)
                {
                    grdInvoices.Rows.Add(s.StockId, Math.Round(s.DocumentNo), s.SupplierInvoiceNo, s.SupplierInvoiceDate, s.TotalAmount - s.TotalReturnStockAmount, "Cash", (s.TotalAmount - varTotalPaid - s.TotalReturnStockAmount));
                }
            }
            GrandTotal = GrandTotal - TotalReturnAmount;
            Due = GrandTotal - TotalPaid;
            lblGrandTotal.Text = GrandTotal.ToString();
            lblPaidAmount.Text = TotalPaid.ToString();
            lblDue.Text = Due.ToString();
            DueAmountAfterCheckedTextbox.Text = Due.ToString();
            CheckedPaidTotalText.Text = Convert.ToString(0);
            SharedFunctions.SetGridStyle(grdInvoices);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<Expense> expenseList = new List<Expense>();
                List<Stock> stocks = new List<Stock>();
                using (unitOfWork = new UnitOfWork())
                {
                    foreach (DataGridViewRow r in grdInvoices.Rows)
                    {
                        double pmt = Convert.ToDouble(r.Cells["colPaidAmount"].Value);
                        if (pmt <= 0) continue;
                        long StockID = Convert.ToInt64(r.Cells["colStockId"].Value);
                        long DocNum = Convert.ToInt64(r.Cells["colDocumentNo"].Value);
                        Expense exp = new Expense();
                        //InvoicePayment ip = new InvoicePayment();
                        int PaymentType = 1;
                        string paymentMode = r.Cells["colPaymentMode"].Value.ToString().ToLower();
                        switch (paymentMode)
                        {
                            case "cash":
                                PaymentType = 1;
                                break;
                            case "cheque":
                                PaymentType = 2;
                                break;
                            case "debit/credit card":
                                PaymentType = 3;
                                break;
                            case "online payment":
                                PaymentType = 4;
                                break;
                            default:
                                PaymentType = 1;
                                break;
                        }
                        exp.description = "Stock Payment(" + DocNum + ")";
                        exp.PaymentMode = PaymentType;
                        exp.Date = DateTime.Now;
                        exp.CreatedAt = DateTime.Now;
                        exp.UpdatedAt = DateTime.Now;
                        exp.PracticeId = SharedVariables.PracticeId;
                        exp.AutoAdded = true;
                        exp.Amount = pmt;
                        exp.Stock = unitOfWork.StockRepository.GetById(StockID);
                        exp.Stock.TotalPaid += pmt;
                        if (exp.Stock.IsSynced)
                        {
                            exp.Stock.IsNew = false;
                            exp.Stock.IsUpdate = true;
                            exp.Stock.IsSynced = false;
                        }
                        exp.Stock.UpdateDate = DateTime.Now;
                        unitOfWork.StockRepository.Update(exp.Stock);
                        exp.Supplier = unitOfWork.SupplierRepository.GetById(SupplierId);
                        exp.IsActive = true;
                        exp.IsNew = true;
                        exp.VoucherNo = unitOfWork.ExpenseRepository.GetVoucherNo();
                        AuditLogTask.CreateAuditLog(unitOfWork, "Document# " + ((long)exp.Stock.DocumentNo).ToString(), AuditModule.Stock, AuditAction.paid);
                        unitOfWork.ExpenseRepository.Insert(exp);
                        stocks.Add(exp.Stock);
                        expenseList.Add(exp);
                    }
                    unitOfWork.Save();
                    //SOT
                    foreach (var expense in expenseList)
                    {
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, expense, ModuleName.Expense, ModuleAction.Create, expense.ExpenseId);
                    }
                }
                MessageBox.Show("Payments Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdInvoices_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == grdInvoices.Columns["colPaidAmount"].Index)
            {
                double dueAmt = SharedFunctions.StringToDouble(grdInvoices.Rows[e.RowIndex].Cells["colDueAmount"].Value);
                double PaidAmt = SharedFunctions.StringToDouble(grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value);
                if (PaidAmt > dueAmt)
                {
                    grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value = dueAmt;
                    MessageBox.Show("Can't pay more than due amount.", "Extra Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void grdInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == grdInvoices.Columns["colCheckedPaidAmount"].Index)
            {
                
               
                bool t = Convert.ToBoolean(grdInvoices.Rows[e.RowIndex].Cells["colCheckedPaidAmount"].EditedFormattedValue);
                if (t)
                {
                    grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value = SharedFunctions.StringToDouble(grdInvoices.Rows[e.RowIndex].Cells["colDueAmount"].Value);
                    if(SharedFunctions.StringToDouble(DueAmountAfterCheckedTextbox.Text) > SharedFunctions.StringToDouble(grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value))
                    {
                        DueAmountAfterCheckedTextbox.Text = (SharedFunctions.StringToDouble(DueAmountAfterCheckedTextbox.Text) - SharedFunctions.StringToDouble(grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value)).ToString();
                    }
                    else
                    {
                        DueAmountAfterCheckedTextbox.Text = (-SharedFunctions.StringToDouble(DueAmountAfterCheckedTextbox.Text) + SharedFunctions.StringToDouble(grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value)).ToString();
                    }

                    CheckedPaidTotalText.Text = (SharedFunctions.StringToDouble(grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value) + SharedFunctions.StringToDouble(CheckedPaidTotalText.Text)).ToString();
                }
                else
                {
                    DueAmountAfterCheckedTextbox.Text = (SharedFunctions.StringToDouble(grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value) + SharedFunctions.StringToDouble(DueAmountAfterCheckedTextbox.Text)).ToString();
                    if(SharedFunctions.StringToDouble(CheckedPaidTotalText.Text) < SharedFunctions.StringToDouble(grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value)){
                        CheckedPaidTotalText.Text = (-SharedFunctions.StringToDouble(CheckedPaidTotalText.Text) + SharedFunctions.StringToDouble(grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value)).ToString();
                    }
                    else
                    {
                        CheckedPaidTotalText.Text = (SharedFunctions.StringToDouble(CheckedPaidTotalText.Text) - SharedFunctions.StringToDouble(grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value)).ToString();
                    }
                    grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value = 0;

                }
            }
        }
    }
}