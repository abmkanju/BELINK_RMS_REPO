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
    public partial class NarcoticItemswiseSaleViewer : Form
    {
        DateTime DateFrom;
        DateTime DateTo;
        List<NarcoticDrugsItemwiseReportVM> invoices;
        int Orientation = 0;

        public NarcoticItemswiseSaleViewer(int Orientation, DateTime DateFrom, DateTime DateTo, List<NarcoticDrugsItemwiseReportVM> Invoices)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.invoices = Invoices;
        }

        private void NarcoticViewer_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Orientation == 0)
                {
                    NarcoticDrugsItemswiseRpt_Portrait r = new NarcoticDrugsItemswiseRpt_Portrait();
                    r.Database.Tables[0].SetDataSource(this.invoices);

                    if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    else
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.EmptyImageTable());
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
