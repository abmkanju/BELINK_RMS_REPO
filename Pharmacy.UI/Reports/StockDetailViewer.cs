using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core.ViewModels;
using Pharmacy.Core;

namespace Pharmacy.UI.Reports
{
    public partial class StockDetailViewer : Form
    {
        static PharmacyContext cxt;
        StockRepository repStocks;
        long StockId = 0;
        private readonly bool IsReturnStock = false;
        private void InitializedObjects()
        {
            cxt = new PharmacyContext();
            repStocks = new StockRepository(cxt);
        }
        public StockDetailViewer(long StockId, bool IsReturnStock = false)
        {
            InitializeComponent();
            this.StockId = StockId;
            this.IsReturnStock = IsReturnStock;
            InitializedObjects();
        }

        private void StockDetailViewer_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsReturnStock)
                {
                    StockReturnVM StockObject;
                    using (UnitOfWork u = new UnitOfWork())
                    {
                        StockObject = u.StockReturnRepository.GetStockReturnItemById(this.StockId);
                    }
                    foreach (var item in StockObject.StockReturnItems)
                    {
                        item.UnitString = item.Unit == 0 ? "Unit" : "Pack";
                    }
                    StockReturnDetailRpt r = new StockReturnDetailRpt();
                    r.Database.Tables[0].SetDataSource(StockObject.StockReturnItems);
                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);

                    // reportspecific info
                    r.SetParameterValue("NetValue", StockObject.GrandTotal);
                    r.SetParameterValue("TotalSale", StockObject.NetSalesTax);
                    r.SetParameterValue("TotalDiscount", StockObject.NetDiscount);
                    r.SetParameterValue("DocumentNo", StockObject.DocumentNo);
                    r.SetParameterValue("Date", StockObject.CreateDate);
                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }
                else
                {
                    StockVM StockObject;
                    using (UnitOfWork u = new UnitOfWork())
                    {
                        StockObject = u.StockRepository.GetStockDetailById(this.StockId);
                    }
                    foreach (var i in StockObject.StockItems)
                    {
                        i.Quantity = i.Unit == 0 ? i.Quantity / i.ConversionUnit : i.Quantity;
                        i.UnitCost = i.Unit == 0 ? i.UnitCost * i.ConversionUnit : i.UnitCost;
                        i.RetailPrice = i.Unit == 0 ? i.RetailPrice * i.ConversionUnit : i.RetailPrice;
                    }
                    StockDetailRpt r = new StockDetailRpt();
                    r.Database.Tables[0].SetDataSource(StockObject.StockItems);
                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                    r.SetParameterValue("SupplierName", StockObject.SupplierName);
                    r.SetParameterValue("SupInvNo", StockObject.SupplierInvoiceNo);
                    r.SetParameterValue("NetValue", StockObject.GrandTotal);
                    r.SetParameterValue("TotalSale", StockObject.TotalSaleTax);
                    r.SetParameterValue("TotalDiscount", StockObject.TotalDiscount);
                    r.SetParameterValue("DocumentNo", (decimal)StockObject.DocumentNo);
                    string withHoldingType = StockObject.WithholdingTaxType == 0 ? "%" : "";
                    string advaceType = StockObject.AdvanceWithholdingTaxType == 0 ? "%" : "";
                    r.SetParameterValue("WithholdingInvoice", StockObject.WithholdingTax.ToString() + withHoldingType);
                   
                    r.SetParameterValue("AdvanceTax", StockObject.AdvanceWithholdingTax.ToString() + advaceType);
                    r.SetParameterValue("Date", StockObject.CreateDate);
                    r.SetParameterValue("SupplierInvoiceDate", StockObject.SupplierIvoiceDate);
                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}