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
    public partial class InvoiceTransactionsViewer : Form
    {
        private int Orienation = 0;
        DateTime DateFrom;
        DateTime DateTo;
        UnitOfWork unitOfWork;
        List<InvoiceTransactionsVM> Transactions;
        public InvoiceTransactionsViewer()
        {
            InitializeComponent();
        }

        public InvoiceTransactionsViewer(int Orientation, DateTime DateFrom, DateTime DateTo, List<InvoiceTransactionsVM> Transactions)
        {
            InitializeComponent();
            this.Orienation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.Transactions = Transactions;
        }

        private void InvoiceTransactionsViewer_Load(object sender, EventArgs e)
        {
            try
            {
                double service = this.Transactions.Select(x => x.Services).Sum();
                if (this.Orienation == 0)
                {
                    InvoiceTransactionsRpt_Portrait r = new InvoiceTransactionsRpt_Portrait();
                    r.Database.Tables[0].SetDataSource(this.Transactions);
                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
                    r.SetParameterValue("DateFrom", this.DateFrom);
                    r.SetParameterValue("DateTo", this.DateTo);
                    r.SetParameterValue("TotalServices", service);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }
                else
                {
                    InvoiceTransactionsRpt_Landscape r = new InvoiceTransactionsRpt_Landscape();
                    r.Database.Tables[0].SetDataSource(Transactions);
                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
                    r.SetParameterValue("DateFrom", this.DateFrom);
                    r.SetParameterValue("DateTo", this.DateTo);
                    r.SetParameterValue("TotalServices", service);
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