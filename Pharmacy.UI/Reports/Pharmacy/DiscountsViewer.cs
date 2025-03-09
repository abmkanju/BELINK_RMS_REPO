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

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class DiscountsViewer : Form
    {
        int Orientation = 0;
        DateTime DateFrom;
        DateTime DateTo;
        UnitOfWork unitOfWork;
        bool GetFullDiscountSales = false;
        List<DiscountVM> DiscountsList;
        public DiscountsViewer()
        {
            InitializeComponent();
        }

         public DiscountsViewer(int Orientation, DateTime DateFrom, DateTime DateTo, List<DiscountVM> DiscountsList)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.DiscountsList = DiscountsList;
        }
         public DiscountsViewer(int Orientation, bool GetFullDiscountSales, DateTime DateFrom, DateTime DateTo, List<DiscountVM> DiscountsList)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.GetFullDiscountSales = GetFullDiscountSales;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.DiscountsList = DiscountsList;
        }
        private void DiscountsViewer_Load(object sender, EventArgs e)
        {
            try
            {
                using (unitOfWork = new UnitOfWork())               
                if (Orientation == 0)
                {
                    Reports.Pharmacy.DiscountsRpt_Portrait r = new DiscountsRpt_Portrait();
                    r.Database.Tables[0].SetDataSource(this.DiscountsList);
                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
                    r.SetParameterValue("DateFrom", this.DateFrom);
                    r.SetParameterValue("DateTo", this.DateTo);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }
                else
                {
                    Reports.Pharmacy.DiscountsRpt_Landscape r = new DiscountsRpt_Landscape();
                    r.Database.Tables[0].SetDataSource(this.DiscountsList);
                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
                    r.SetParameterValue("DateFrom", this.DateFrom);
                    r.SetParameterValue("DateTo", this.DateTo);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
    }
}