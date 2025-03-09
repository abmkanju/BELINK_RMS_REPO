using Pharmacy.Core;
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
    public partial class AdjustmentsViewer : Form
    {
        long AdjustmentID = 0;
        public AdjustmentsViewer()
        {
            InitializeComponent();
        }

        public AdjustmentsViewer(long AdjustmentId)
        {
            InitializeComponent();
            this.AdjustmentID = AdjustmentId;
        }

        private void AdjustmentsViewer_Load(object sender, EventArgs e)
        {
            AdjustmentsVM ad = null;
            using (UnitOfWork uow = new UnitOfWork())
            {
                ad = uow.AdjustmentRepository.GetAdjustments_Rpt(this.AdjustmentID);
            }
            AdjustmentsRpt r = new AdjustmentsRpt();
            foreach (var i in ad.AdjustmentItems)
            {
                i.Quantity = i.Unit == 0 ? i.Quantity / i.ConversionUnit : i.Quantity;
                i.CostPrice = i.Unit == 0 ? i.CostPrice * i.ConversionUnit : i.CostPrice;
                i.AdjustmentValue = i.Quantity * i.CostPrice;
                i.AdjustmentValue = i.Quantity * i.CostPrice;
            }
            r.Database.Tables[0].SetDataSource(ad.AdjustmentItems);
            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
            {
                r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
            }
            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
            r.SetParameterValue("AdjustmentNo", ad.AdjustmentId);
            crystalReportViewer1.ReportSource = r;
            crystalReportViewer1.Show();
        }
    }
}
