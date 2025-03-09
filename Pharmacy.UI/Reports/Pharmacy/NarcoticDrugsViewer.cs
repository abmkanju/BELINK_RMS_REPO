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
    public partial class NarcoticDrugsViewer : Form
    {
        DateTime DateFrom;
        DateTime DateTo;
        List<NarcoticDrugsPrintReportVM> Invoices;
        UnitOfWork unitOfWork;
        int Orientation = 0;
        public NarcoticDrugsViewer()
        {
            InitializeComponent();
        }

        public NarcoticDrugsViewer(int Orientation, DateTime DateFrom, DateTime DateTo, List<NarcoticDrugsPrintReportVM> Invoices)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.Invoices = Invoices;
        }

        private void NarcoticViewer_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Orientation == 0)
                {
                    NarcoticDrugsRpt_Portrait_Updated r = new NarcoticDrugsRpt_Portrait_Updated();
                    r.Database.Tables[0].SetDataSource(this.Invoices);
                    //r.Database.Tables[0].SetDataSource(this.Invoices);
                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    try
                    {

                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to load report headers data, please try login again.");
                    }
                    r.SetParameterValue("DateFrom", this.DateFrom);
                    r.SetParameterValue("DateTo", this.DateTo);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    // r.SetParameterValue("TotalPaid", this.Invoices.Sum(rs=>rs.ItemId));

                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }
                //else
                //{
                //    NarcoticDrugsRpt_Landscape r = new NarcoticDrugsRpt_Landscape();
                //    //r.Database.Tables[0].SetDataSource(this.Invoices);
                //    r.Database.Tables[0].SetDataSource(this.Invoices);
                //    r.Database.Tables[2].SetDataSource(this.Invoices);
                //    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                //    {
                //        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                //    }
                //    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                //    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                //    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
                //    r.SetParameterValue("DateFrom", this.DateFrom);
                //    r.SetParameterValue("DateTo", this.DateTo);
                //    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                //    r.SetParameterValue("TotalPaid", this.Invoices.Sum(rs => rs.));

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
