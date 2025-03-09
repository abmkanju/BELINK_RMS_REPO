using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Infrastructure;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Core;

namespace Pharmacy.UI.Reports
{
    public partial class InvoiceViewer : Form
    {
        UnitOfWork unitOfwork;
        long InvoiceId = 0;
        public InvoiceViewer()
        {
            InitializeComponent();
            unitOfwork = new UnitOfWork();
        }

        public InvoiceViewer(long InvoiceId)
        {
            InitializeComponent();
            this.InvoiceId = InvoiceId;
            unitOfwork = new UnitOfWork();
        }

        private void InvoiceViewer_Load(object sender, EventArgs e)
        {
            try
            {
                InvoiceVM i = unitOfwork.InvoiceRepository.GetInvoiceById_IncludeDetails(this.InvoiceId);
                if(i == null)
                {
                    MessageBox.Show("No Invoice Found Against Selected Stock Consumption", "Invoice Not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }
                InvoiceRpt r = new InvoiceRpt();
                r.SetDataSource(i.InvoiceItems);
                r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                r.SetParameterValue("InvoiceId", i.InvoiceId);
                r.SetParameterValue("CreateDate", i.CreateDate);
                r.SetParameterValue("SubTotal", "Rs. " + i.SubTotal.ToString());
                r.SetParameterValue("GrandTotal", "Rs. " + i.GrandTotal.ToString() + "/-");
                r.SetParameterValue("PaidAmount", "Rs. " + i.TotalPaid.ToString() + "/-");
                r.SetParameterValue("Advance", "Rs. " + i.Advance.ToString());
                r.SetParameterValue("NextAppointment", DateTime.Now);
                crystalReportViewer1.ReportSource = r;
                crystalReportViewer1.Show();
            }
            catch(Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
    }
}