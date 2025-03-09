using Pharmacy.Core;
using Pharmacy.Core.ViewModels;
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
    public partial class StockConsumptionsViewer : Form
    {
        private List<StockConsumptionItemVM> StockConsumptions;
        DateTime FromDate;
        DateTime ToDate;
        public StockConsumptionsViewer()
        {
            InitializeComponent();
        }

        public StockConsumptionsViewer(List<StockConsumptionItemVM> StockConsumptions, DateTime FromDate, DateTime ToDate)
        {
            InitializeComponent();
            this.StockConsumptions = StockConsumptions;
            this.FromDate = FromDate;
            this.ToDate = ToDate;
        }

        private void StockConsumptionsViewer_Load(object sender, EventArgs e)
        {
            StockConsumptionsRpt r = new StockConsumptionsRpt();
            r.Database.Tables[0].SetDataSource(this.StockConsumptions);
            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
            {
                r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
            }
            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
            r.SetParameterValue("FromDate", this.FromDate);
            r.SetParameterValue("ToDate", this.ToDate);
            crystalReportViewer1.ReportSource = r;
            crystalReportViewer1.Show();
        }
    }
}
