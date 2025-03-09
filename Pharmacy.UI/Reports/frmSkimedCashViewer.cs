using Pharmacy.Core;
using Pharmacy.Core.Entities;
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

namespace Pharmacy.UI.Reports
{
    public partial class frmSkimedCashViewer : Form
    {
        bool isPrint = false;
        int Id = 0;
        public frmSkimedCashViewer(bool isPrint, int Id)
        {
            InitializeComponent();
            this.isPrint = isPrint;
            this.Id = Id;
        }
        public frmSkimedCashViewer()
        {
            InitializeComponent();
        }

        private void frmSkimedCashViewer_Load(object sender, EventArgs e)
        {
            try
            {
                POSSkimmedCash obj = new POSSkimmedCash();
                using (UnitOfWork unitofwork = new UnitOfWork())
                {
                    obj = unitofwork.POSCashSkimmedRepository.GetById(this.Id);
                }
                string printerName = System.Configuration.ConfigurationManager.ConnectionStrings["PrinterName"].ToString();

                CashSkimmingRpt_Portrait rpt = new CashSkimmingRpt_Portrait();
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[0].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }

                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("Date", obj.CreatedAt.Date);
                rpt.SetParameterValue("Time", obj.CreatedAt.ToString("HH:mm:sss"));

                rpt.SetParameterValue("POSCode", 01);
                rpt.SetParameterValue("User", SharedVariables.LoggedInUser.UserName);
                rpt.SetParameterValue("SkimmedCash", Math.Round(obj.Cash));
                rpt.PrintOptions.PrinterName = printerName;
                //if (this.isPrint)
                //{
                //    rpt.PrintToPrinter(1, true, 0, 0);
                //}
                //else
                //{
                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.Show();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while printing skimmed cash report.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
