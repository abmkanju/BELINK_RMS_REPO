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
    public partial class IncomeStatementViewer : Form
    {
        int Orientation = 0;
        DateTime DateFrom;
        DateTime DateTo;
        string totalexpense = "0";
        IncomeStatementVM IncomeStatement;
        public IncomeStatementViewer()
        {
            InitializeComponent();
        }

        public IncomeStatementViewer(int Orientation, DateTime DateFrom, DateTime DateTo, IncomeStatementVM IncomeStatement , string expense)
        {
            InitializeComponent();
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.Orientation = Orientation;
            this.IncomeStatement = IncomeStatement;
            totalexpense = expense;
        }

        private void IncomeStatementViewer_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StatementType", typeof(string));
            dt.Columns.Add("Value", typeof(double));
            dt.Rows.Add("Total Revenue", this.IncomeStatement.TotalRevenue);
            dt.Rows.Add("Total Stock Payments", this.IncomeStatement.TotalStockPayments);
            
            dt.Rows.Add("Total Expense", totalexpense);
            dt.Rows.Add("NET INCOME", this.IncomeStatement.TotalRevenue - (this.IncomeStatement.TotalStockPayments + Convert.ToDouble(totalexpense)));

            if(this.Orientation == 0)
            {
                IncomeStatementRpt_Portrait r = new IncomeStatementRpt_Portrait();
                r.Database.Tables[0].SetDataSource(dt);
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
                IncomeStatementRpt_Landscape r = new IncomeStatementRpt_Landscape();
                r.Database.Tables[0].SetDataSource(dt);
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
}
