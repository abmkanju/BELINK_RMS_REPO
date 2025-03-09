using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Pharmacy.UI.Reports.Pharmacy
{

    public partial class PatientInvoiceViewer : Form
    {
        UnitOfWork unitOfWork;
        long InvoiceId = 0;
        public PatientInvoiceViewer()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }

        public PatientInvoiceViewer(long InvoiceId)
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            this.InvoiceId = InvoiceId;
        }

        private void PatientInvoiceViewer_Load(object sender, EventArgs e)
        {
            try
            {
                InvoiceVM i = new InvoiceVM();
                using (unitOfWork = new UnitOfWork())
                {
                    i = unitOfWork.InvoiceRepository.GetPOSInvoiceDetailById(this.InvoiceId);
                }
                i.InvoicePayments.ToList().RemoveAt(0);
                PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                r.SetParameterValue("InvoiceId", i.InvoiceRefNo);
                r.SetParameterValue("CreateDate", i.CreateDate);
                if (i.ObjPatient == null)
                {
                    r.SetParameterValue("PatientName", "");
                    r.SetParameterValue("MrNo", "");
                    r.SetParameterValue("PatientPhone", "");
                    r.SetParameterValue("Gender", "");
                    r.SetParameterValue("DateOfBirth", "");
                    r.SetParameterValue("Age", "");
                }
                else
                {
                    r.SetParameterValue("PatientName", i.ObjPatient.Name);
                    r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                    r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                    r.SetParameterValue("Gender", i.ObjPatient.Gender == null? "" : i.ObjPatient.Gender);
                    r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.ObjPatient.DateOfBirth.ToString());
                    r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                }


                r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                double DueAmount = i.GrandTotal - i.TotalPaid;
                r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                r.SetParameterValue("DueAmountNumVal", DueAmount);
                r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                crystalReportViewer1.ReportSource = r;
                crystalReportViewer1.Show();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
    }
}
