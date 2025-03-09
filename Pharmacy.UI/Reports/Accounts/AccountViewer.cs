using Pharmacy.Core;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.Reports.Accounts;
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
    public partial class AccountViewer : Form
    {
        int Orientation = 0;
        DateTime DateFrom;
        DateTime DateTo;
        UnitOfWork unitOfWork;
        bool GetFullDiscountSales = false;
        List<AccountExpenseRptVM> objList;
        public AccountViewer()
        {
            InitializeComponent();
        }

        public AccountViewer(int Orientation, DateTime DateFrom, DateTime DateTo, List<AccountExpenseRptVM> objList)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.objList = objList;
        }
        private void AccountViewer_Load(object sender, EventArgs e)
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    if (Orientation == 0)
                    {
                        AccountRpt_Portrait r = new AccountRpt_Portrait();
                        r.Database.Tables[0].SetDataSource(this.objList);
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
                        r.Database.Tables[0].SetDataSource(this.objList);
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
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}