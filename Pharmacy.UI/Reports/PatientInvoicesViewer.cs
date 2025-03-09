using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PagedList;
using Pharmacy.Core;

namespace Pharmacy.UI.Reports
{
    public partial class PatientInvoicesViewer : Form
    {
        private List<ItemsVM> ItemsList;
        public PatientInvoicesViewer()
        {
            InitializeComponent();
        }

        public PatientInvoicesViewer(List<ItemsVM> ItemsList)
        {
            InitializeComponent();
            this.ItemsList = ItemsList;
        }

        private void PatientInvoicesViewer_Load(object sender, EventArgs e)
        {
            try
            {
                PatientInvoicesRpt_Portrait rpt = new PatientInvoicesRpt_Portrait();
                rpt.Database.Tables[0].SetDataSource(ItemsList);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
