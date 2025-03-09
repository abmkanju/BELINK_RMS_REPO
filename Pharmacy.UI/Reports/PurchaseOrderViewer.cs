using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core.ViewModels;

namespace Pharmacy.UI.Reports
{
    public partial class PurchaseOrderViewer : Form
    {
        private UnitOfWork unitOfWork;
        long PurchaseOrderId = 0;

        public PurchaseOrderViewer()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        public PurchaseOrderViewer(long PurchaseOrderId)
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            this.PurchaseOrderId = PurchaseOrderId;
        }
        private void PurchaseOrderViewer_Load(object sender, EventArgs e)
        {
            try
            {
                PurchaseOrderVM p = unitOfWork.PurchaseOrderRepository.GetPurchaseOrderById_print(this.PurchaseOrderId);
                string manufecturer = "";
                if (p.Manufecturers != null)
                {
                    foreach (Manufacturer manu in p.Manufecturers)
                    {
                        manufecturer += manu.Name + ", \n";
                    }
                }
                else
                {
                    manufecturer = "";
                }
                foreach (var objItem in p.PurchaseOrderItems)
                {
                    objItem.Quantity = objItem.Unit == 0 ? objItem.Quantity / objItem.ConversionUnit : objItem.Quantity;
                    objItem.UnitCost = objItem.Unit == 0 ? objItem.UnitCost * objItem.ConversionUnit : objItem.UnitCost;
                    objItem.Discount = objItem.DiscountType == 0 ? (objItem.Discount / 100) * objItem.GrossAmount : objItem.Discount; 
                }
                List<PurchaseOrderItemVM> FinalList = new List<PurchaseOrderItemVM>();
                if (SharedVariables.AdminInvoiceSetting.IsPurchaseOderA8Format)
                {
                    PurchaseOrderA8Rpt r = new PurchaseOrderA8Rpt();
                    r.Database.Tables[0].SetDataSource(p.PurchaseOrderItems);
                    
                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    else
                    {
                        r.Database.Tables[1].SetDataSource("");
                    }
                    if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath) && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.Name) && SharedVariables.AdminInvoiceSetting.PrintPractiseName)
                    {
                        r.SetParameterValue("PharmacyNameLogoVal", true);
                        r.SetParameterValue("PharmacyNameVal", false);
                    }
                    else
                    {
                        r.SetParameterValue("PharmacyNameLogoVal", false);
                        if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.Name) && SharedVariables.AdminInvoiceSetting.PrintPractiseName)
                        {
                            r.SetParameterValue("PharmacyNameVal", true);
                        }
                        else
                        {
                            r.SetParameterValue("PharmacyNameVal", false);
                        }
                    }
                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                    r.SetParameterValue("InvoiceId", p.PurchaseOrderNo.ToString());
                    r.SetParameterValue("CreateDate", p.CreateDate);
                    r.SetParameterValue("ShowPharmaName", true);
                    r.SetParameterValue("GrandTotal", p.PurchaseOrderItems.Sum(x => x.TotalAmount));
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    r.SetParameterValue("Supplier", p.Supplier == null ? "" : p.Supplier.Name);
                    r.SetParameterValue("Manufecturer", manufecturer);
                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }
                else
                {
                    PurchaseOrderRpt r = new PurchaseOrderRpt();
                    
                    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                    {
                        r.Database.Tables[0].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                    }
                    else
                    {
                        r.Database.Tables[0].SetDataSource("");
                    }
                    r.Database.Tables[1].SetDataSource(p.PurchaseOrderItems);
                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                    r.SetParameterValue("PurchaseOrderNo", p.PurchaseOrderNo.ToString());
                    r.SetParameterValue("Manufecturer", manufecturer);
                    r.SetParameterValue("STN", p.Supplier ==  null  ? "" : (p.Supplier.STN == null ? "" : p.Supplier.STN.ToString()));
                    r.SetParameterValue("NTN", p.Supplier == null ? "" : (p.Supplier.NTN == null ? "" : p.Supplier.NTN.ToString()));
                    r.SetParameterValue("DeliveryDate", p.DeliveryDate.ToShortDateString());
                    r.SetParameterValue("Date", p.CreateDate);
                    r.SetParameterValue("Supplier", p.Supplier == null ? "" : p.Supplier.Name);
                    r.SetParameterValue("TermsConditions", SharedVariables.AdminPrintFormatSetting.TermsConditions == null ? "" :SharedVariables.AdminPrintFormatSetting.TermsConditions.ToString());
                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }

                manufecturer = "";

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
    }
}