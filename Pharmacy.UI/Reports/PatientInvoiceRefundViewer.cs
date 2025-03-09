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
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core.ViewModels;


namespace Pharmacy.UI.Reports
{
    public partial class PatientInvoiceRefundViewer : Form
    {
        long InvoiceId = 0;
        UnitOfWork unitOfWork;
        public PatientInvoiceRefundViewer()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }

        public PatientInvoiceRefundViewer(long InvoiceId)
        {
            InitializeComponent();
            this.InvoiceId = InvoiceId;
            unitOfWork = new UnitOfWork();
        }

        private void PatientInvoiceRefund_Load(object sender, EventArgs e)
        {
            InvoiceVM objInvoice = unitOfWork.InvoiceRepository.GetInvoiceByIdDetails_IncPatient(this.InvoiceId);
            try
            {
                InvoiceVM i = unitOfWork.InvoiceRepository.GetInvoiceByIdDetails_IncPatient(this.InvoiceId);
                List<InvoiceItemVM> ModifiedList = new List<InvoiceItemVM>();
                string Items = "";
                foreach (InvoiceItemVM ii in i.InvoiceItems)
                {
                    Items += ii.ItemName + ", ";
                }
                Items = Items.Trim().TrimEnd(',');
                InvoiceItemVM vm = new InvoiceItemVM();
                vm.ItemName = Items;
                vm.Amount = i.TotalRefund;
                ModifiedList.Add(vm);
                PatientInvoiceRefundRpt r = new PatientInvoiceRefundRpt();
                r.Database.Tables[0].SetDataSource(ModifiedList);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
                r.SetParameterValue("InvoiceId", i.InvoiceId);
                r.SetParameterValue("PatientName", i.Patient.Name);
                r.SetParameterValue("MrNo", i.Patient.MRNo);
                r.SetParameterValue("PatientPhone", i.Patient.Phone);

                r.SetParameterValue("RefundDate", i.RefundDate);
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
