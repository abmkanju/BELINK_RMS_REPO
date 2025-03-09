using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.Reports.POSInvoices
{
    public partial class SalesReturnViewer : Form
    {
        string printerName = SharedVariables.AdminInvoiceSetting.PrinterName;
        private long SalesReturnId;
        public bool isPrint = false;

        UnitOfWork unitOfWork;
        public SalesReturnViewer()
        {
            InitializeComponent();
        }
        public SalesReturnViewer(long SalesReturnId, bool isPrnt)
        {
            InitializeComponent();
            this.isPrint = isPrnt;
            this.SalesReturnId = SalesReturnId;
        }
        public void Print()
        {
            try
            {
                if (this.SalesReturnId > 0)
                {
                    SaleReturnAll ii = new SaleReturnAll();
                    InvoiceVM i = new InvoiceVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        i = unitOfWork.InvoiceRepository.GetPOSInvoiceDetailById(0);
                    }

                    using (unitOfWork = new UnitOfWork())
                    {
                        ii = unitOfWork.SalesReturnRepository.GetSalesReturnItemById(SalesReturnId);
                    }
                    if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                    {
                        #region PrintPageSizeA4
                        SaleReturnA4_Detailed_Portrait_Update r = new SaleReturnA4_Detailed_Portrait_Update();
                        //AssingUnitPack(i.InvoiceItems);
                        r.Database.Tables[6].SetDataSource(ii.salesReturn);
                        if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                        {
                            r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                        }
                        if (SharedVariables.AdminPharmacySetting.IsFBRUnable)
                        {
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(string.IsNullOrEmpty(i.FBRInvoiceNo) ? "" : i.FBRInvoiceNo, QRCodeGenerator.ECCLevel.H);
                            QRCode qrCode = new QRCode(qrCodeData);
                            Bitmap qrCodeImage = qrCode.GetGraphic(20);
                            r.Database.Tables[5].SetDataSource(SharedFunctions.GetImageTable(qrCodeImage));
                        }
                        r.SetParameterValue("PharmacyName1", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);

                        InvoicePortraitA4(ii.salesReturn, r);
                        r.SetParameterValue("SubTotal", "Rs. " + 0);
                        r.SetParameterValue("GrandTotal", "Rs. " + 0 + "/-");
                        r.SetParameterValue("TotalPaid", "Rs. " + 0);

                        if (ii.objPatient == null)
                        {
                            r.SetParameterValue("PatientName", "");
                            r.SetParameterValue("MrNo", "");
                        }
                        else
                        {
                            r.SetParameterValue("PatientName", ii.objPatient.Name);
                            r.SetParameterValue("MRNo", ii.objPatient.MRNo.ToString());
                        }
                        r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                        r.SetParameterValue("Setting_HideRateAndQuantity", SharedVariables.AdminInvoiceSetting.HideRateAndQuantityInPrintFormat);
                        r.SetParameterValue("Setting_HideDuesAndAdvanceFromInvoiceAndPayamnetReceipts", SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts);
                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                        //crystalReportViewer1.ReportSource = r;
                        //crystalReportViewer1.Show();
                        try
                        {
                            r.PrintOptions.PrinterName = this.printerName;
                            r.PrintToPrinter(1, true, 0, 0);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        #endregion
                    }
                    else if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A8)
                    {
                        #region PrintPageSizeA8
                        SalesReturnA8 r = new SalesReturnA8();
                        //AssignTotalAmountToAmount(i.InvoiceItems);
                        r.Database.Tables[5].SetDataSource(ii.salesReturn);
                        if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                        {
                            r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                        }

                        if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath) && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.Name) && SharedVariables.AdminInvoiceSetting.PrintPractiseName)
                        {
                            r.SetParameterValue("PharmacyNameLogoVal", true);
                            r.SetParameterValue("PharmacyNameVal", false);
                            r.SetParameterValue("PharmacyLogoOnly", false);
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
                            r.SetParameterValue("PharmacyLogoOnly", !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath) ? true : false);
                        }

                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);

                        if (ii.objPatient == null)
                        {
                            r.SetParameterValue("PatientName", "");
                            r.SetParameterValue("MrNo", "");
                        }
                        else
                        {
                            r.SetParameterValue("PatientName", ii.objPatient.Name);
                            r.SetParameterValue("MrNo", ii.objPatient.MRNo.ToString());
                        }

                        r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                        r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                        r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                        r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                        r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);
                        r.SetParameterValue("ShowPatientAddress", SharedVariables.AdminInvoiceSetting.ShowAddress);
                        r.SetParameterValue("ShowEmail", SharedVariables.AdminInvoiceSetting.ShowEmail);

                        r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                        r.SetParameterValue("Setting_ShowUserName", SharedVariables.AdminInvoiceSetting.ShowUserName);
                        r.SetParameterValue("Setting_HideRateAndQuantity", SharedVariables.AdminInvoiceSetting.HideRateAndQuantityInPrintFormat);
                        r.SetParameterValue("Setting_HideDuesAndAdvanceFromInvoiceAndPayamnetReceipts", SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts);

                        r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                        if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.Phone) && SharedVariables.AdminInvoiceSetting.ShowPharmaPhone == true)
                            r.SetParameterValue("ShowPharmaPhone", true);
                        else
                            r.SetParameterValue("ShowPharmaPhone", false);
                        if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.Address) && SharedVariables.AdminInvoiceSetting.ShowPharmaAddress == true)
                            r.SetParameterValue("ShowPharmaAddress", true);
                        else
                            r.SetParameterValue("ShowPharmaAddress", false);

                        //crystalReportViewer1.Show();
                        //crystalReportViewer1.PrintMode = CrystalDecisions.Windows.Forms.PrintMode.PrintToPrinter;
                        try
                        {
                            if (
                                SharedVariables.AdminPractiseSetting.PracticeId == 9
                                || SharedVariables.AdminPractiseSetting.PracticeId == 4
                                || SharedVariables.AdminPractiseSetting.PracticeId == 11
                                )// needed to add this, direct print was not working for this.
                            {
                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.ShowPrintButton = false;
                                crystalReportViewer1.PrintReport();
                            }
                            else
                            {
                                r.PrintOptions.PrinterName = this.printerName;
                                r.PrintToPrinter(1, true, 0, 0);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.InnerException.Message, "Error");
                }
                else
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                }
            }
        }
        private void SalesReturnViewer_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.isPrint == false)
                {
                    if (this.SalesReturnId > 0)
                    {
                        SaleReturnAll ii = new SaleReturnAll();
                        InvoiceVM i = new InvoiceVM();
                        using (unitOfWork = new UnitOfWork())
                        {
                            i = unitOfWork.InvoiceRepository.GetPOSInvoiceDetailById(0);
                        }

                        using (unitOfWork = new UnitOfWork())
                        {
                            ii = unitOfWork.SalesReturnRepository.GetSalesReturnItemById(SalesReturnId);
                        }
                        //if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                        //{
                        SaleReturnA4_Detailed_Portrait_Update r = new SaleReturnA4_Detailed_Portrait_Update();
                        //AssingUnitPack(i.InvoiceItems);
                        r.Database.Tables[6].SetDataSource(ii.salesReturn);
                        if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                        {
                            r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                        }
                        if (SharedVariables.AdminPharmacySetting.IsFBRUnable)
                        {
                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(string.IsNullOrEmpty(i.FBRInvoiceNo) ? "" : i.FBRInvoiceNo, QRCodeGenerator.ECCLevel.H);
                            QRCode qrCode = new QRCode(qrCodeData);
                            Bitmap qrCodeImage = qrCode.GetGraphic(20);
                            r.Database.Tables[5].SetDataSource(SharedFunctions.GetImageTable(qrCodeImage));
                        }
                        r.SetParameterValue("PharmacyName1", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);

                        InvoicePortraitA4(ii.salesReturn, r);
                        r.SetParameterValue("SubTotal", "Rs. " + 0);
                        r.SetParameterValue("GrandTotal", "Rs. " + 0 + "/-");
                        r.SetParameterValue("TotalPaid", "Rs. " + 0);

                        r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                        r.SetParameterValue("Setting_HideRateAndQuantity", SharedVariables.AdminInvoiceSetting.HideRateAndQuantityInPrintFormat);
                        r.SetParameterValue("Setting_HideDuesAndAdvanceFromInvoiceAndPayamnetReceipts", SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts);
                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                        crystalReportViewer1.ReportSource = r;
                        crystalReportViewer1.Show();
                        //try
                        //{
                        //    r.PrintOptions.PrinterName = this.printerName;
                        //    r.PrintToPrinter(1, true, 0, 0);
                        //}
                        //catch (Exception)
                        //{
                        //    MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //}
                    }
                }
                else
                {
                    Print();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.InnerException.Message, "Error");
                }
                else
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                }
            }
        }

        private void InvoicePortraitA4(List<SalesReturnItemVM> i, SaleReturnA4_Detailed_Portrait_Update r)
        {
            AdminInvoiceSetting setting = new AdminInvoiceSetting();
            if (SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts == true)
                r.SetParameterValue("AdvanceDueAmountVal", true);
            else
                r.SetParameterValue("AdvanceDueAmountVal", false);
            //r.SetParameterValue("ShowPOSService", SharedVariables.AdminInvoiceSetting.ShowPOSService == true ? false : true);
            if (SharedVariables.AdminInvoiceSetting.ShowPOSService == true) { r.SetParameterValue("Services", ""); }
            else
            {
                    r.SetParameterValue("Services", 0);
            }


            r.SetParameterValue("TotalDiscount", 0);
            r.SetParameterValue("AdvanceAmountNumVal", 0);
            r.SetParameterValue("DueAmountNumVal", 0);
            r.SetParameterValue("AdvanceAmount", "Rs. " + 0);
            r.SetParameterValue("DueAmount", "Rs. " + 0);

            r.SetParameterValue("GrandTotal", "Rs. " + 0 + "/-");
            r.SetParameterValue("RefundAmountVal", 0);
            r.SetParameterValue("RefundAmountNumVal", 0);
            r.SetParameterValue("RefundAmount", "Rs. " + 0);

            r.SetParameterValue("IsInvoiceBalance", false);
            r.SetParameterValue("InvoiceBalanceVal", "");
            
            r.SetParameterValue("ShowPOSGrandTotal", SharedVariables.AdminInvoiceSetting.ShowPOSGrandTotal == true ? false : true);
            r.SetParameterValue("ShowPOSSubTotal", SharedVariables.AdminInvoiceSetting.ShowPOSSubTotal == true ? false : true);
            //r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
            r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
            r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
            r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
            r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
            r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);
            r.SetParameterValue("ShowPatientAddress", SharedVariables.AdminInvoiceSetting.ShowAddress);
            r.SetParameterValue("ShowEmail", SharedVariables.AdminInvoiceSetting.ShowEmail);

            r.SetParameterValue("InvoiceId", 0);
            //r.SetParameterValue("Header1", "Invoice#:");
            //r.SetParameterValue("Content1", 0);

            r.SetParameterValue("CreateDate", 0);
            r.SetParameterValue("IsTpOrRate", SharedVariables.AdminInvoiceSetting.ShowTPInsteadOfRate ? "T.P" : "Rate");
            r.SetParameterValue("HideBonusQty", SharedVariables.AdminPharmacySetting.IsBonusQty);
            r.SetParameterValue("ShowReturnQty", false);
        }
        private void AssingUnitPack(IEnumerable<InvoiceItemVM> items)
        {
            AdminInvoiceSetting setting = new AdminInvoiceSetting();
            foreach (var item in items)
            {


                string discountType = item.DiscountType == 1 ? "%" : "";
                if (!SharedVariables.AdminInvoiceSetting.HideLineItemDiscount)
                {
                    item.DiscString = item.Discount > 0 ? item.Discount.ToString() + discountType : "0";

                }
                else
                {
                    item.DiscString = "";
                    item.ProcedureName = "";
                }
                item.Quantity = item.Unit == 0 ? item.Quantity / item.ConUnit : item.Quantity;
                item.BonusQuantity = item.Unit == 0 ? item.BonusQuantity / item.ConUnit : item.BonusQuantity;
                item.Rate = item.Unit == 0 ? item.Rate * item.ConUnit : item.Rate;

            }
        }
    }
}
