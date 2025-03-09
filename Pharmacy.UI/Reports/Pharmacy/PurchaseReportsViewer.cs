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
    public partial class PurchaseReportsViewer : Form
    {
        int Orientation = 0;
        DateTime DateFrom;
        DateTime DateTo;
        UnitOfWork unitOfWork;
        long SupplierId = 0;
        public PurchaseReportsViewer()
        {
            InitializeComponent();
        }

        public PurchaseReportsViewer(int Orientation, DateTime DateFrom, DateTime DateTo)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
        }
        public PurchaseReportsViewer(int Orientation, long SupplierId)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.SupplierId = SupplierId;
        }

        private void PurchaseReportsViewer_Load(object sender, EventArgs e)
        {
            try
            {
                List<PurchaseReportVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    if (this.SupplierId > 0)
                    {
                        Result = unitOfWork.StockRepository.GetAllPurchases_Rpt(this.SupplierId);
                    }
                    else
                    {
                        Result = unitOfWork.StockRepository.GetAllPurchases_Rpt(this.DateFrom, this.DateTo);
                    }
                }
                if (Orientation == 0)
                {
                    //Reports.Pharmacy.PurchaseReports_Landscape r = new PurchaseReports_Landscape();
                    Reports.Pharmacy.PurchaseReports_Portrait r = new PurchaseReports_Portrait();

                    r.Database.Tables[0].SetDataSource(Result);
                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);                  
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }
                else
                {
                    Reports.Pharmacy.PurchaseReports_Landscape r = new PurchaseReports_Landscape();
                    r.Database.Tables[0].SetDataSource(Result);
                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
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