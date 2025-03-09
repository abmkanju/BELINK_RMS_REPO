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

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmPatientInvoices : Form
    {
        static PharmacyContext cxt;
        InvoiceRepository repInvoices;
        UnitOfWork unitOfWork;
        int PageNo = 1;
        int PageSize = SharedVariables.PageSize;
        long PatientId = 0;
        public frmPatientInvoices()
        {
            InitializeComponent();
            InitializeObjects();
        }
        private void InitializeObjects()
        {
            cxt = new PharmacyContext();
            repInvoices = new InvoiceRepository(cxt);
            unitOfWork = new UnitOfWork();
        }
        public frmPatientInvoices(long PatientId)
        {
            InitializeComponent();
            this.PatientId = PatientId;
            InitializeObjects();
        }

        private void frmPatientInvoices_Load(object sender, EventArgs e)
        {
            PatientInvoicesData();
            SharedFunctions.SetGridStyle(grdInvoices);
        }

        private void PatientInvoicesData()
        {
            PatientVM patient = repInvoices.GetPatienInvoices(this.PatientId, PageNo, PageSize);
            lblPatientInfo.Text = patient.MRNo + "-" + patient.Name + "-Invoices";
            int count = 0;
            foreach (InvoiceVM i in patient.Invoices)
            {
                string InvoiceItems = "";
                string InvoicePayments = "";
                string PaymentDates = "";
                foreach (InvoiceItemVM ii in i.InvoiceItems)
                {
                    InvoiceItems += ii.ItemName + ", ";
                }
                foreach (InvoicePaymentVM ip in i.InvoicePayments)
                {
                    InvoicePayments += ip.Payment + Environment.NewLine;
                    PaymentDates += ip.PaymentDate.ToShortDateString() + Environment.NewLine;
                }
                InvoiceItems = InvoiceItems.Trim().TrimEnd(',');
                grdInvoices.Rows.Add(count++, i.InvoiceId, i.InvoiceRefNo, InvoiceItems, i.SubTotal, InvoicePayments, i.Due, PaymentDates, null, null);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }
                long InvoiceId = Convert.ToInt64(grdInvoices.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                if (e.ColumnIndex == grdInvoices.Columns["colEdit"].Index)
                {
                    frmInvoice i = new frmInvoice(true, false, Convert.ToInt64(InvoiceId));
                    i.Show();
                    return;
                }

                if (e.ColumnIndex == grdInvoices.Columns["colRefund"].Index)
                {
                    frmInvoice i = new frmInvoice(false, true, InvoiceId);
                    i.Show();
                    return;
                }
                if (e.ColumnIndex == grdInvoices.Columns["colDelete"].Index)
                {
                    DialogResult rs = MessageBox.Show("Are you Sure You Want To Delete Invoice", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (rs == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }                    
                    Invoice objInvoice = unitOfWork.InvoiceRepository.GetInvoiceById_Inc_Pmnts_cons(InvoiceId);
                    objInvoice.UpdateDate = DateTime.Now;
                    objInvoice.IsActive = false;
                    objInvoice.IsUpdate = true;
                    foreach (InvoiceItem i in objInvoice.InvoiceItems)
                    {
                        i.IsActive = false;
                        i.IsUpdate = true;
                        unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                    }
                    //manage this piece of code...
                    foreach (StockConsumptionItem c in objInvoice.StockConsumption.StockConsumptions)
                    {
                        c.IsUpdate = true;
                        c.IsActive = false;
                        c.UpdateDate = DateTime.Now;
                        unitOfWork.GetDbContext().Entry(c).State = System.Data.Entity.EntityState.Modified;
                    }
                    foreach (InvoicePayment p in objInvoice.InvoicePayments)
                    {
                        p.IsUpdate = true;
                        p.IsActive = false;
                        p.UpdatedAt = DateTime.Now;
                        unitOfWork.GetDbContext().Entry(p).State = System.Data.Entity.EntityState.Modified;
                    }
                    unitOfWork.Save();
                    MessageBox.Show("Patient Invoice Deleted Succesfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnRefresh.PerformClick();
                    return;
                }

                if(e.ColumnIndex == grdInvoices.Columns["colPrintInvoice"].Index)
                {
                    Reports.Pharmacy.PatientInvoiceViewer v = new Reports.Pharmacy.PatientInvoiceViewer(InvoiceId);
                    v.Show();
                    return;
                }
                if (e.ColumnIndex == grdInvoices.Columns["colPrintInvoiceRefund"].Index)
                {
                    Reports.PatientInvoiceRefundViewer v = new Reports.PatientInvoiceRefundViewer(InvoiceId);
                    v.Show();
                    return;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            frmInvoice f = new frmInvoice(this.PatientId);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
            f.Show();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            InitializeObjects();
            grdInvoices.Rows.Clear();
            PatientInvoicesData();
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void btnPrintInvoiceHistory_Click(object sender, EventArgs e)
        {

        }

    }
}