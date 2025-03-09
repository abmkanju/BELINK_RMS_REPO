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

namespace Pharmacy.UI.Reports
{
    public partial class StockConsumptionViewer : Form
    {
        private long ConsumptionId = 0;

        public StockConsumptionViewer()
        {
            InitializeComponent();
        }

        public StockConsumptionViewer(long ConsumptionId)
        {
            InitializeComponent();
            this.ConsumptionId = ConsumptionId;
        }
        private void StockConsumptionViewer_Load(object sender, EventArgs e)
        {
            try
            {
                StockConsumptionVM sc;
                using (UnitOfWork uw = new UnitOfWork())
                {
                    sc = uw.StockConsumptionRepository.GetStockConsumptionById(ConsumptionId);
                }
                foreach (StockConsumptionItemVM v in sc.StockConsumptionsList)
                {
                    if (v.ConsumptionType == 1) v.ConsumptionTypeString = "Services";
                    else if (v.ConsumptionType == 2) v.ConsumptionTypeString = "Sales";
                    else if (v.ConsumptionType == 3) v.ConsumptionTypeString = "Damages";
                    else if (v.ConsumptionType == 4) v.ConsumptionTypeString = "Returned";
                }
                StockConsumptionRpt_Portrait r = new StockConsumptionRpt_Portrait();
                r.Database.Tables[0].SetDataSource(sc.StockConsumptionsList);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
                r.SetParameterValue("AddedBy", sc.User.UserName);

                crystalReportViewer1.ReportSource = r;
                crystalReportViewer1.Show();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error Occurred While Loading Data For Print, Please Try Again.", ex.Message, "Print Error");
            }
        }
    }
}
