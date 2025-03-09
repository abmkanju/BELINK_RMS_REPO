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
    public partial class ItemSalesViewer : Form
    {
        int Orientation = 0;
        DateTime DateFrom;
        DateTime DateTo;
        UnitOfWork unitOfWork;
        int ItemId = 0;
        public ItemSalesViewer()
        {
            InitializeComponent();
        }

        public ItemSalesViewer(int Orientation, DateTime DateFrom, DateTime DateTo)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
        }

        public ItemSalesViewer(int Orientation, int ItemId, DateTime DateFrom, DateTime DateTo)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.ItemId = ItemId;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
        }

        private void ItemSalesViewer_Load(object sender, EventArgs e)
        {
            try
            {
                List<ItemWiseSaleVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    if (ItemId > 0)
                    {
                        //Result = unitOfWork.ItemRspository.GetItemWiseSales_Rpt(this.DateFrom, this.DateTo, ItemId);
                        Result = unitOfWork.ItemRspository.GetItemWiseSales_Rpt1(this.DateFrom, this.DateTo, ItemId);
                    }
                    else
                    {
                        //Result = unitOfWork.ItemRspository.GetItemWiseSales_Rpt(this.DateFrom, this.DateTo);
                        Result = unitOfWork.ItemRspository.GetItemWiseSales_Rpt1(this.DateFrom, this.DateTo);
                    }
                }
                if(Orientation == 0)
                {
                    Reports.Pharmacy.ItemSalesRpt_Portrait r = new ItemSalesRpt_Portrait();
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
                else
                {
                    Reports.Pharmacy.ItemSalesRpt_Landscape r = new ItemSalesRpt_Landscape();
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