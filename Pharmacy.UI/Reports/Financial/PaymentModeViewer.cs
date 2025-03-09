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

namespace Pharmacy.UI.Reports.Financial
{
    public partial class PaymentModeViewer : Form
    {
        int Orientation = 0;
        DateTime DateFrom;
        DateTime DateTo;
        List<InvoiceTransactionsVM> TransactionsList;
        public PaymentModeViewer()
        {
            InitializeComponent();
        }
        public PaymentModeViewer(int Orientation, DateTime DateFrom, DateTime DateTo, List<InvoiceTransactionsVM> TransactionsList)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.TransactionsList= TransactionsList;
        }
        private void PaymentModeViewer_Load(object sender, EventArgs e)
        {
            try
            {
                if (Orientation == 0)
                {
                    Reports.Financial.PaymentModeRpt_Portrait r = new PaymentModeRpt_Portrait();
                    r.Database.Tables[0].SetDataSource(this.TransactionsList);
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
                    Reports.Financial.PaymentModeRpt_Landscape r = new PaymentModeRpt_Landscape();
                    r.Database.Tables[0].SetDataSource(this.TransactionsList);
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
