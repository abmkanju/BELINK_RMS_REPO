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

namespace Pharmacy.UI.Reports
{
    public partial class ExpiryViewer : Form
    {
        int Orientation = 0;
        DateTime DateFrom;
        DateTime DateTo;
        UnitOfWork unitOfWork;
        public ExpiryViewer()
        {
            InitializeComponent();
        }

        public ExpiryViewer(int Orientation, DateTime DateFrom, DateTime DateTo)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
        }
        private void ExpiryViewer_Load(object sender, EventArgs e)
        {
            try
            {
                List<ExpiryVM> Result;
                List<ExpiryVM> Result1 = new List<ExpiryVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    Result = unitOfWork.ItemRspository.GetItemsExpiry_Rpt(this.DateFrom, this.DateTo);

                }

                foreach (ExpiryVM ee in Result)
                {
                    ItemDetailVM vm = new ItemDetailVM();
                    //ItemsVM item = new ItemsVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        //item = unitOfWork.ItemRspository.GetItemWithAvQtywithoutexpiry(e.ItemId);
                        vm = unitOfWork.ItemRspository.GetBatchStockByItemId_1(ee.ItemId);
                    }
                    foreach (BatchStockVM v in vm.BatchStockList)
                    {
                        if ((v.ConsumedStock < v.ExpiredStock && v.BatchName == ee.BatchName) || (ee.ExpiryDate >= DateTime.Now && v.BatchName == ee.BatchName))
                        {
                            ee.Quantity = ee.Quantity - v.ConsumedStock;
                            ee.TotalCost = v.CostPrice * ee.Quantity;
                            ee.TotalRetailValue = v.RetailPrice * ee.Quantity;
                            Result1.Add(ee);
                        }
                    }
                }
                if (Orientation == 0)
                {
                    Reports.Pharmacy.ExpiryRpt_Portraitrpt r = new Pharmacy.ExpiryRpt_Portraitrpt();
                    r.Database.Tables[0].SetDataSource(Result1);
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
                    Reports.Pharmacy.ExpiryRpt_Landscape r = new Pharmacy.ExpiryRpt_Landscape();
                    r.Database.Tables[0].SetDataSource(Result);
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
