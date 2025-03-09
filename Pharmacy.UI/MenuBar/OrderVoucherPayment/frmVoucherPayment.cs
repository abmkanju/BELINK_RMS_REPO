using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.Utility;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.ManageOfflineTask;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.OrderVoucherPayment
{
    public partial class frmVoucherPayments : Form
    {
        UnitOfWork unitOfWork;
        long InvoiceId = 0;
        long VoucherId = 0;
        string VoucherName = "";
        Service obj = new Service();
        List<InvoiceVM> invoice = new List<InvoiceVM>();

        public frmVoucherPayments()
        {
            InitializeComponent();
        }
        public frmVoucherPayments(long VoucherId ,  string  VoucherName)
        {
            InitializeComponent();
            this.VoucherId = VoucherId;
            this.VoucherName = VoucherName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                const string quote = "\"";
                InvoicePayment p = new InvoicePayment();
                List<VoucherVM> voucherList = new List<VoucherVM>();
                int count = 0;

                foreach (DataGridViewRow r in grdInvoices.Rows)
                {
                    double pmt = Convert.ToDouble(r.Cells["colPaidAmount"].Value);
                    if (pmt <= 0) continue;

                    Expense exp = new Expense();
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

                    p.PaymentType = 1;
                    p.PaymentDate = DateTime.Now;
                    p.CreatedAt = DateTime.Now;
                    p.UpdatedAt = DateTime.Now;
                    p.IsActive = true;
                    p.IsNew = true;
                    p.Payment = Convert.ToDouble(r.Cells["colDueAmount"].Value);
                    p.MethodType = 1;

                    //insert InvoicePayment
                    int i = updateDB("Insert Into InvoicePayments(PaymentType, Payment , RefundReason , PaymentDate , CreatedAt , UpdatedAt , IsActive , Invoice_InvoiceId , User_UserId , IsNew , IsUpdate , IsSynced , MethodType , Dues) Values(" + 1 + "," + p.Payment + ",'" + " " + "' , '" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "'," + 1 + "," + Convert.ToDouble(r.Cells["colInvoiceId"].Value) + "," + SharedVariables.LoggedInUser.UserId + "," + 1 + "," + 0 + "," + 0 + "," + 1 + "," + 0 + ")");

                    //update invoice 
                    int j = updateDB("Update Invoices set IsVoucherPaid=" + 1 + " where InvoiceId=" + Convert.ToDouble(r.Cells["colInvoiceId"].Value));
                    int m = updateDB("Update Invoices set TotalPaid=" + Convert.ToDouble(r.Cells["colTotalAmount"].Value) + ",Due=" + 0 + " where InvoiceId=" + Convert.ToDouble(r.Cells["colInvoiceId"].Value));

                    invoice[count].Due = 0;
                    invoice[count].TotalPaid = Convert.ToDouble(r.Cells["colTotalAmount"].Value);

                    using (unitOfWork = new UnitOfWork())
                    {
                        invoice[count].User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        if (invoice[count].Patient.PatientId == 0)
                        {
                            invoice[count].Patient = null;
                        }

                        OfflineJsonTask.SaveOfflineTask(unitOfWork, invoice[count], ModuleName.Invoice, ModuleAction.Update, invoice[count].InvoiceId);
                        //List<StockConsumptionItem> StockConsumptionsList = invoice[count].StockConsumption.StockConsumptions;
                        //StockConsumptionsList.ForEach(x => x.User = invoice[count].User);
                        //OfflineJsonTask.SaveOfflineTask(unitOfWork, StockConsumptionsList, ModuleName.BulkStockConsumptionItems, ModuleAction.Update, 0);
                    }

                    count++;
                }

                //Send Email
                string voucherInfo = "The Payment Total " + quote + lblDue.Text + quote + " against Voucher Name  " + quote + this.VoucherName + quote + " has been Paid by Healthwire to Practice Name " + quote + SharedVariables.AdminPharmacySetting.OnlineWebUsername + quote + " ,practiceId= " + quote + SharedVariables.AdminPharmacySetting.PracticeId + quote;
                obj.SendVoucherEmail(voucherInfo);

                //update invoice 
                int n = updateDB("Update OrderVouchers set IsVoucherPaid=" + 1 + " where OrderVoucherId=" + this.VoucherId);

                //update voucher
                int updateVouch = updateDB("Update OrderVouchers set GrandTotal=" + 0 + ",SubTotal=" + 0 + ",Due=" + 0 + " where OrderVoucherId=" + this.VoucherId);

                MessageBox.Show("Payments Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private int updateDB(string qry)
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand com = new SqlCommand(qry, con);
            int i = com.ExecuteNonQuery();
            con.Close();
            return i;
        }

        private void frmVoucherPayments_Load(object sender, EventArgs e)
        {
            using (unitOfWork = new UnitOfWork())
            {
                invoice = unitOfWork.InvoiceRepository.GetInvoiceByInvoiceIdVoucher(this.VoucherName);
            }
            double GrandTotal = 0;
            double TotalPaid = 0;
            double Due = 0;
            double Paid = 0;
            double varTotalPaid = 0;
            for (int s = 0; s < invoice.Count; s++)
            {
                TotalPaid += invoice[s].Due;
                GrandTotal += invoice[s].GrandTotal;
                grdInvoices.Rows.Add(
                    invoice[s].InvoiceId,
                    1,
                    invoice[s].InvoiceRefNo,
                    invoice[s].CreateDate,
                    invoice[s].GrandTotal,
                    "Cash",
                    invoice[s].Due,
                    invoice[s].GrandTotal - invoice[s].Due
                    );
            }

            //already paid
            if (invoice.Count == 0)
            {
                MessageBox.Show("Payments Already Paid", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

            Paid = GrandTotal - TotalPaid;
            Due = TotalPaid;
            lblGrandTotal.Text = GrandTotal.ToString();
            lblPaidAmount.Text = Paid.ToString();
            lblDue.Text = Due.ToString();
            SharedFunctions.SetGridStyle(grdInvoices);
            grdInvoices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdInvoices_ClientSizeChanged(object sender, EventArgs e)
        {
            
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
        public void CalculateTotals()
        {
            double GrandTotal = 0;
            double TotalPaid = 0;
            double Due = 0;
            double Paid = 0;

            foreach (DataGridViewRow r in grdInvoices.Rows)
            {
                DataGridViewCheckBoxCell cc = new DataGridViewCheckBoxCell();
                cc = (DataGridViewCheckBoxCell)r.Cells["colPay"];
                if (cc.Selected)
                {
                    TotalPaid += SharedFunctions.StringToDouble(r.Cells["colTotalAmount"].Value) - SharedFunctions.StringToDouble(r.Cells["colPaidAmount"].Value);
                    GrandTotal += SharedFunctions.StringToDouble(r.Cells["colTotalAmount"].Value);
                }
            }
            Due = TotalPaid;
            Paid = GrandTotal - TotalPaid;
            lblGrandTotal.Text = GrandTotal.ToString();
            lblPaidAmount.Text = Paid.ToString();
            lblDue.Text = Due.ToString();
        }

        private void grdInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == grdInvoices.Columns["colPay"].Index)
            {
                this.CalculateTotals();
            }
        }
    }
}
