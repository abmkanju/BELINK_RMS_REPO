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
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Core;
namespace Pharmacy.UI.Reports.Product
{
    public partial class DiscountedItemViewer : Form
    {
        int Orientation = 0;
        List<DiscountedItemVM> pmLis;
        List<AllItemsDiscVM> allItemsVM;
        public bool IsAllItemsReport { get; set; }
        public DiscountedItemViewer()
        {
            InitializeComponent();
        }

        public DiscountedItemViewer(int Orientation)
        {
            InitializeComponent();
            this.Orientation = Orientation;
        }

        public DiscountedItemViewer(int Orientation, List<DiscountedItemVM> pmLis)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.pmLis = pmLis;
            this.IsAllItemsReport = false;
        }

        private void LowStocsViewer_Load(object sender, EventArgs e)
        {
            try
            {
                if (Orientation == 0)
                {
                    Reports.Product.DiscountedItemRpt_Portrait r = new DiscountedItemRpt_Portrait();
                    r.Database.Tables[0].SetDataSource(this.pmLis);
                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                    //r.SetParameterValue("DateFrom", this.DateFrom);
                    //r.SetParameterValue("DateTo", this.DateTo);
                    //r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }
                //else
                //{
                //    Reports.Pharmacy.LowStocksRpt_Landscape r = new LowStocksRpt_Landscape();
                //    r.Database.Tables[0].SetDataSource(this.pmLis);
                //    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                //    {
                //        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                //    }
                //    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                //    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                //    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
                //    //r.SetParameterValue("DateFrom", this.DateFrom);
                //    //r.SetParameterValue("DateTo", this.DateTo);
                //    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                //    crystalReportViewer1.ReportSource = r;
                //    crystalReportViewer1.Show();
                //}
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
    }
}