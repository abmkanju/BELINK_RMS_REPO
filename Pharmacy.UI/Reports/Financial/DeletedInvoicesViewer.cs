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

namespace Pharmacy.UI.Reports.Financial
{
    public partial class TrancationsViewer : Form
    {
        int Orientation = 0;
        DateTime DateFrom;
        DateTime DateTo;
        int PaymentMode = 0;
        List<InvoiceTransactionsVM> Transactions;
        UnitOfWork unitOfWork;
        public TrancationsViewer()
        {
            InitializeComponent();
        }

        public TrancationsViewer(int Orientation, DateTime DateFrom, DateTime DateTo, List<InvoiceTransactionsVM> Transactions)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.Transactions = Transactions;
        }

        private void TrancationsViewer_Load(object sender, EventArgs e)
        {
            try
            {               
                if (Orientation == 0)
                {
                    Reports.Financial.TransactionsRpt_Portrait r = new TransactionsRpt_Portrait();
                    r.SetDataSource(this.Transactions);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    r.SetParameterValue("DateFrom", this.DateFrom);
                    r.SetParameterValue("DateTo", this.DateTo);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }
                else
                {
                    Reports.Financial.TransactionsRpt_Landscape r = new TransactionsRpt_Landscape();
                    r.SetDataSource(this.Transactions);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
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