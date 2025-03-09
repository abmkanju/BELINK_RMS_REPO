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
    public partial class StockStaticsViewer : Form
    {
        private List<StockStaticsVM> ItemsList;

        public StockStaticsViewer()
        {
            InitializeComponent();
        }
        public StockStaticsViewer(List<StockStaticsVM> ItemsList)
        {
            InitializeComponent();
            this.ItemsList = ItemsList;
        }

        private void StockStaticsViewer_Load(object sender, EventArgs e)
        {
            try
            {
                StockStaticsRpt rpt = new StockStaticsRpt();
                
               
                rpt.Database.Tables[1].SetDataSource(ItemsList.Where(i=>i.totalstock >0));
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
