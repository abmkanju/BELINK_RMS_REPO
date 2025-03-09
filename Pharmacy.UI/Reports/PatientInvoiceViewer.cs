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
using Pharmacy.Core.ViewModels;

namespace Pharmacy.UI.Reports
{
    public partial class PatientInvoiceViewer : Form
    {
        private long InvoiceId = 0;
        UnitOfWork unitOfWork;
        private List<PatientRptVM> list;

        public PatientInvoiceViewer(List<PatientRptVM> list)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.list = list;
        }

        private void PatientInvoice_Load(object sender, EventArgs e)
        {
            try
            {
                PatientInvoicesRpt_Portrait r = new PatientInvoicesRpt_Portrait();
                r.Database.Tables[0].SetDataSource(this.list);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                };
                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
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