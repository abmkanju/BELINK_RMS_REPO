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

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class GrossProfitStatementViewer : Form
    {
        
        DateTime DateFrom;
        DateTime DateTo;
        List<ProfitStatementTransactionVM> ProfitStatements;

        public GrossProfitStatementViewer()
        {
            InitializeComponent();
        }

        public GrossProfitStatementViewer(int Orientation, DateTime DateFrom, DateTime DateTo, List<ProfitStatementTransactionVM> ProfitStatements)
        {
            InitializeComponent();
            
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.ProfitStatements = ProfitStatements;
        }

        private void GrossProfitStatementViewer_Load(object sender, EventArgs e)
        {
            try
            {
               
                    Reports.Pharmacy.GrossProfitStatementRpt_Portrait r = new GrossProfitStatementRpt_Portrait();

                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[0].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    else
                    {
                        r.Database.Tables[0].SetDataSource(ProfitStatements);
                    }
                    r.Database.Tables[1].SetDataSource(ProfitStatements);
                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                    r.SetParameterValue("DateFrom", this.DateFrom);
                    r.SetParameterValue("DateTo", this.DateTo);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                
                
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
    }
}
