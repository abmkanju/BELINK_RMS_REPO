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
    public partial class POSInvoiceViewer : Form
    {
        string printerName = SharedVariables.AdminInvoiceSetting.PrinterName;//System.Configuration.ConfigurationManager.ConnectionStrings["PrinterName"].ToString();
        private long InvoiceId;
        private bool IsDetailedInvoice;
        private double InvoiceBalance = 0;
        private double InvoiceAdvance = 0;
        private bool IsDeletedInvoice = false;
        private bool IsReturnInvoice = false;
        UnitOfWork unitOfWork;
        public POSInvoiceViewer()
        {
            InitializeComponent();
        }
        public void Print()
        {
            try
            {
                if (IsDetailedInvoice)
                {
                    #region IsDetailInvoice
                    int chkDiscount = 0;
                    InvoiceVM i = new InvoiceVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        i = unitOfWork.InvoiceRepository.GetPOSInvoiceDetailById(this.InvoiceId);
                    }

                    //getInvoiceItem
                    foreach (var check in i.InvoiceItems)
                    {
                        if (check.Discount > 0)
                        {
                            chkDiscount = 1;
                        }
                    }

                    if (i.DiscountType == 2 && chkDiscount == 0)
                    {
                        i.TotalDiscount = (i.SubTotal / 100) * i.TotalDiscount;
                    }
                    foreach (InvoicePaymentVM v in i.InvoicePayments)
                    {
                        if (v.PaymentType == 1) { v.PaymentTypeString = "Cash"; }
                        else if (v.PaymentType == 2) { v.PaymentTypeString = "Cheque"; }
                        else if (v.PaymentType == 3) { v.PaymentTypeString = "Debit/Credit Card"; }
                        else if (v.PaymentType == 4) { v.PaymentTypeString = "Online Payment"; }
                    }
                    if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                    {
                        #region PrintPageSizeA4
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            #region Commented Code
                            //PageMargins p = new PageMargins();
                            //p.leftMargin = 2000;
                            //p.rightMargin= 2000;                            
                            //p.topMargin= 5000;
                            //r.PrintOptions.ApplyPageMargins(p);
                            //PageMargins p = r.PrintOptions.PageMargins.topMargin;
                            //PageMargins p = r.PrintOptions.PageMargins.rightMargin;
                            //PageMargins p = r.PrintOptions.PageMargins.leftMargin;
                            //r.Section1.SectionFormat.BackgroundColor = Color.Green;//.ReportObjects["PharmacyName"].Left = 20;
                            //int l = r.ReportDefinition.ReportObjects["PharmacyName1"].Left = 5000;

                            //int w = r.ReportDefinition.ReportObjects["PharmacyName1"].Width;
                            //Console.WriteLine("w=> " + w.ToString());
                            //r.ReportDefinition.ReportObjects["LogoImage1"].Left = 5000;
                            //foreach (ReportObject reportObject in r.ReportDefinition.ReportObjects)
                            //{
                            //    Console.WriteLine("=> " + reportObject.Name + Environment.NewLine);
                            //}
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            #endregion

                            POSInvoiceA4_Detailed_Portrait_Update r = new POSInvoiceA4_Detailed_Portrait_Update();
                            //POSInvoiceA4_Detailed_Portrait r = new POSInvoiceA4_Detailed_Portrait();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            AssingUnitPack(i.InvoiceItems);
                            AssingUnitsalestax(i.InvoiceItems);

                            r.Database.Tables[0].SetDataSource(i.InvoiceItems);

                            if (i.CashBack > 0)
                            {
                                foreach (var set in i.InvoicePayments)
                                {
                                    int calset = Convert.ToInt32(set.Payment) + Convert.ToInt32(i.CashBack);
                                    set.PaymentString = "Rs. " + calset + "/-";
                                }
                            }
                            r.Database.Tables[2].SetDataSource(i.InvoicePayments);
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
                            //r.Database.Tables[5].SetDataSource(i.InvoicePayments);
                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            double serviceSet = 0;
                            if (i.ServiceType == 2)
                            {
                                i.ServiceType = i.ServiceType - 1;
                                if (i.ServiceType == 1)
                                {
                                    serviceSet = (i.Services / 100) * i.SubTotal;
                                    i.ServiceCheck = serviceSet;
                                }
                            }
                            else
                            {
                                serviceSet = i.Services;
                                i.ServiceCheck = 0;
                                i.ServiceType = 0;
                            }

                            InvoicePortraitA4(i, r);
                            r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount + serviceSet).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            //double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount;
                            //double DueAmount = i.GrandTotal - i.TotalPaid;
                            //double RefundAmount = i.TotalRefund;
                            //r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            //r.SetParameterValue("DueAmountNumVal", DueAmount);
                            //r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            //r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            //r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                            //r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());

                            r.SetParameterValue("HeaderFooterPref", SharedVariables.AdminPrintFormatSetting.HeaderFooterPref);
                            r.SetParameterValue("PatientDetailPref", SharedVariables.AdminPrintFormatSetting.PatientDetailsPref);
                            r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                            r.SetParameterValue("Setting_ShowUserName", SharedVariables.AdminInvoiceSetting.ShowUserName);
                            r.SetParameterValue("Setting_HideRateAndQuantity", SharedVariables.AdminInvoiceSetting.HideRateAndQuantityInPrintFormat);
                            r.SetParameterValue("Setting_HideDuesAndAdvanceFromInvoiceAndPayamnetReceipts", SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts);
                            r.SetParameterValue("Setting_PrintPractiseName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                            //crystalReportViewer1.ReportSource = r;
                            //crystalReportViewer1.Show();
                            try
                            {
                                r.PrintOptions.PrinterName = this.printerName;
                                r.PrintToPrinter(1, true, 0, 0);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting \n Error Message:"+ex.InnerException, "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                            POSInvoiceA4_Detailed_Landscape r = new POSInvoiceA4_Detailed_Landscape();
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreateDate);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            double serviceSet = 0;
                            if (i.ServiceType == 2)
                            {
                                i.ServiceType = i.ServiceType - 1;
                                if (i.ServiceType == 1)
                                {
                                    serviceSet = (i.Services / 100) * i.SubTotal;
                                    i.ServiceCheck = serviceSet;
                                }
                            }
                            else
                            {
                                serviceSet = i.Services;
                                i.ServiceCheck = 0;
                                i.ServiceType = 0;
                            }

                            r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal + serviceSet).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
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
                        }
                        #endregion
                    }
                    else if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
                    {
                        #region PrintPageSizeA5
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA5_Detailed_Portrait r = new POSInvoiceA5_Detailed_Portrait();
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);

                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreateDate);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
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
                        }
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                        }
                        #endregion
                    }
                    else
                    {
                        if (SharedVariables.AdminPrintFormatSetting.PrintFormatIndex == 0)
                        {
                            #region PrintPageSizeA8

                            POSInvoiceA8_Detailed r = new POSInvoiceA8_Detailed();

                            AssignTotalAmountToAmount(i.InvoiceItems);
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            if (i.CashBack > 0)
                            {
                                foreach (var set in i.InvoicePayments)
                                {
                                    int calset = Convert.ToInt32(set.Payment) + Convert.ToInt32(i.CashBack);
                                    set.PaymentString = "Rs. " + calset + "/-";
                                }
                            }
                            r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }
                            InvoiceA8(i, r);
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
                            if (i.CashBack > 0)
                            {
                                r.SetParameterValue("IsInvoiceBalance", true);
                                r.SetParameterValue("InvoiceBalanceVal", "Rs. " + Math.Round(i.CashBack).ToString() + "/-");
                            }
                            else
                            {
                                r.SetParameterValue("IsInvoiceBalance", false);
                                r.SetParameterValue("InvoiceBalanceVal", "");
                            }
                            if (!string.IsNullOrEmpty(i.Note) || !string.IsNullOrEmpty(SharedVariables.AdminInvoiceSetting.InvoiceNote))
                                r.SetParameterValue("NoteVal", true);
                            else
                                r.SetParameterValue("NoteVal", false);
                            r.SetParameterValue("FBRInvoiceNo", i.FBRInvoiceNo);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);
                            //r.SetParameterValue("Services", i.Services > 0 ? "Rs. " + Math.Round(i.Services).ToString() : "");
                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreateDate);

                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                                r.SetParameterValue("PatientAddress", "");
                                r.SetParameterValue("PatientEmail", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.ObjPatient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                                r.SetParameterValue("PatientAddress", i.ObjPatient.Address == null ? "" : i.ObjPatient.Address);
                                r.SetParameterValue("PatientEmail", i.ObjPatient.Email == null ? "" : i.ObjPatient.Email.ToString());
                            }


                            r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                            r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                            r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                            r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                            r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);
                            r.SetParameterValue("ShowPatientAddress", SharedVariables.AdminInvoiceSetting.ShowAddress);
                            r.SetParameterValue("ShowEmail", SharedVariables.AdminInvoiceSetting.ShowEmail);

                            if (i.ServiceType == 2)
                            {
                                i.ServiceType = i.ServiceType - 1;
                                if (i.ServiceType == 1)
                                {
                                    i.Services = (i.Services / 100) * i.SubTotal;
                                }
                            }

                            r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            r.SetParameterValue("TotalDiscount", i.TotalDiscount > 0 ? "Rs. " + Math.Round(i.TotalDiscount).ToString() : "");
                            //if (!SharedVariables.AdminInvoiceSetting.HideLineItemDiscount)
                            //{
                            //    r.SetParameterValue("TotalDiscount", i.TotalDiscount > 0 ? "Rs. " + Math.Round(i.TotalDiscount).ToString() : "");
                            //}
                            //else if(!(i.InvoiceItems.Where(ii=>ii.Discount>0).ToList().Count >0))
                            //{
                            //    r.SetParameterValue("TotalDiscount", i.TotalDiscount > 0 ? "Rs. " + Math.Round(i.TotalDiscount).ToString() : "");
                            //}
                            //else
                            //{
                            //    r.SetParameterValue("TotalDiscount", "");
                            //}
                            r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount + i.Services).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            //double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount - i.Services;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            double AdvanceAmount = -1 * DueAmount;
                            double RefundAmount = i.TotalRefund;
                            string InvoiceType = i.InvoiceType;
                            if (AdvanceAmount > 0 || DueAmount > 0 && SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts == true)
                                r.SetParameterValue("AdvanceDueAmountVal", true);
                            else
                                r.SetParameterValue("AdvanceDueAmountVal", false);
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString() + "/-");
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            r.SetParameterValue("InvoiceType", InvoiceType);
                            r.SetParameterValue("RefundAmountVal", RefundAmount > 0 ? true : false);
                            r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                            r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());

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
                                MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting \n Error Message:" + ex.InnerException, "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            #endregion}
                        }
                        else
                        {
                            #region PrintPageSizeA8

                            POSInvoiceA8_Detailed_2 r = new POSInvoiceA8_Detailed_2();

                            AssignTotalAmountToAmount(i.InvoiceItems);
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            if (i.CashBack > 0)
                            {
                                foreach (var set in i.InvoicePayments)
                                {
                                    int calset = Convert.ToInt32(set.Payment) + Convert.ToInt32(i.CashBack);
                                    set.PaymentString = "Rs. " + calset + "/-";
                                }
                            }
                            r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }
                            InvoiceA8_2(i, r);
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
                            if (i.CashBack > 0)
                            {
                                r.SetParameterValue("IsInvoiceBalance", true);
                                r.SetParameterValue("InvoiceBalanceVal", "Rs. " + Math.Round(i.CashBack).ToString() + "/-");
                            }
                            else
                            {
                                r.SetParameterValue("IsInvoiceBalance", false);
                                r.SetParameterValue("InvoiceBalanceVal", "");
                            }
                            if (!string.IsNullOrEmpty(i.Note) || !string.IsNullOrEmpty(SharedVariables.AdminInvoiceSetting.InvoiceNote))
                                r.SetParameterValue("NoteVal", true);
                            else
                                r.SetParameterValue("NoteVal", false);
                            r.SetParameterValue("FBRInvoiceNo", i.FBRInvoiceNo);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);
                            //r.SetParameterValue("Services", i.Services > 0 ? "Rs. " + Math.Round(i.Services).ToString() : "");
                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreateDate);

                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                                r.SetParameterValue("PatientAddress", "");
                                r.SetParameterValue("PatientEmail", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.ObjPatient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                                r.SetParameterValue("PatientAddress", i.ObjPatient.Address == null ? "" : i.ObjPatient.Address);
                                r.SetParameterValue("PatientEmail", i.ObjPatient.Email == null ? "" : i.ObjPatient.Email.ToString());
                            }


                            r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                            r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                            r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                            r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                            r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);
                            r.SetParameterValue("ShowPatientAddress", SharedVariables.AdminInvoiceSetting.ShowAddress);
                            r.SetParameterValue("ShowEmail", SharedVariables.AdminInvoiceSetting.ShowEmail);

                            if (i.ServiceType == 2)
                            {
                                i.ServiceType = i.ServiceType - 1;
                                if (i.ServiceType == 1)
                                {
                                    i.Services = (i.Services / 100) * i.SubTotal;
                                }
                            }
                            r.SetParameterValue("TotalDiscount", i.TotalDiscount > 0 ? "Rs. " + Math.Round(i.TotalDiscount).ToString() : "");

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //if (!SharedVariables.AdminInvoiceSetting.HideLineItemDiscount)
                            //{
                            //    r.SetParameterValue("TotalDiscount", i.TotalDiscount > 0 ? "Rs. " + Math.Round(i.TotalDiscount).ToString() : "");
                            //}
                            //else if (!(i.InvoiceItems.Where(ii => ii.Discount > 0).ToList().Count > 0))
                            //{
                            //    r.SetParameterValue("TotalDiscount", i.TotalDiscount > 0 ? "Rs. " + Math.Round(i.TotalDiscount).ToString() : "");
                            //}
                            //else
                            //{
                            //    r.SetParameterValue("TotalDiscount", "");
                            //}
                            r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount + i.Services).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            //double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount - i.Services;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            double AdvanceAmount = -1 * DueAmount;
                            double RefundAmount = i.TotalRefund;
                            string InvoiceType = i.InvoiceType;
                            if (AdvanceAmount > 0 || DueAmount > 0 && SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts == true)
                                r.SetParameterValue("AdvanceDueAmountVal", true);
                            else
                                r.SetParameterValue("AdvanceDueAmountVal", false);
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString() + "/-");
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            r.SetParameterValue("InvoiceType", InvoiceType);
                            r.SetParameterValue("RefundAmountVal", RefundAmount > 0 ? true : false);
                            r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                            r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());

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
                            #endregion}
                        }
                    }
                    
                }
                else
                {
                    #region InvoiceWithOutDetails
                    InvoiceVM i = new InvoiceVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        i = unitOfWork.InvoiceRepository.GetPOSInvoiceById(this.InvoiceId);
                    }
                    string Description = "";
                    foreach (InvoiceItemVM item in i.InvoiceItems)
                    {
                        Description += item.ItemName + ", ";
                    }
                    Description = Description.Trim().TrimEnd(',');
                    string PaymentMode = getPaymentModeString(i.PaymentType);
                    //if(SharedVariables.AdminInvoiceSetting == null)
                    //{
                    //    DefaultGeneralPrint(i, Description);
                    //    return;
                    //}
                    if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                    {
                        #region PrintPageSizeA4
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA4_Portrait r = new POSInvoiceA4_Portrait();
                            //string path = @"D:\DesktopApplications\Pharmacy\Pharmacy - InvoiceLinwItemRemove\Pharmacy.UI\Theme\HWlogoWithoutText.jpg";
                            //DataTable dt = SharedFunctions.GetImageTable(path);                            
                            //r.Database.Tables[3].SetDataSource(dt);
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }
                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreateDate);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
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
                        }
                        else if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                            POSInvoiceA4_Landscape r = new POSInvoiceA4_Landscape();

                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }
                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreateDate);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
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
                        }
                        #endregion
                    }
                    else if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
                    {
                        #region PrintPageSizeA5
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA5_Portrait r = new POSInvoiceA5_Portrait();

                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreateDate);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            //crystalReportViewer1.ReportSource = r;
                            //crystalReportViewer1.Show();
                            try
                            {
                                r.PrintOptions.PrinterName = this.printerName;
                                r.PrintToPrinter(1, true, 1, 1);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                            POSInvoiceA5_Landscape r = new POSInvoiceA5_Landscape();

                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreateDate);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
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
                        }
                        #endregion
                    }
                    else //A8
                    {
                        #region PrintPageSizeA8
                        POSInvoiceA8 r = new POSInvoiceA8();
                        //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                        //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                        //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                        if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                        {
                            r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                        }
                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                        r.SetParameterValue("CreateDate", i.CreateDate);
                        r.SetParameterValue("InvoiceNote", i.Note);
                        if (i.ObjPatient == null)
                        {
                            r.SetParameterValue("PatientName", "");
                            r.SetParameterValue("MrNo", "");
                            r.SetParameterValue("PatientPhone", "");
                            r.SetParameterValue("Gender", "");
                            r.SetParameterValue("DateOfBirth", "");
                            r.SetParameterValue("Age", "");
                        }
                        else
                        {
                            r.SetParameterValue("PatientName", i.ObjPatient.Name);
                            r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                            r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                            r.SetParameterValue("Gender", i.ObjPatient.Gender);
                            r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                            r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                        }


                        //r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                        //r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                        //r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                        //r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                        //r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);

                        r.SetParameterValue("Description", Description);
                        r.SetParameterValue("PaymentMode", PaymentMode);

                        //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                        //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                        double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                        double DueAmount = i.GrandTotal - i.TotalPaid;
                        r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                        r.SetParameterValue("DueAmountNumVal", DueAmount);
                        r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                        r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());

                        //r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                        //r.SetParameterValue("ShowPharmaPhone", SharedVariables.AdminInvoiceSetting.ShowPharmaPhone);
                        //r.SetParameterValue("ShowPharmaAddress", SharedVariables.AdminInvoiceSetting.ShowPharmaAddress);
                        //crystalReportViewer1.ReportSource = r;
                        //crystalReportViewer1.Show();
                        try
                        {
                            if (SharedVariables.AdminPractiseSetting.PracticeId == 9
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
                        catch (Exception)
                        {
                            MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        #endregion
                    }
                    #endregion
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
        public POSInvoiceViewer(long InvoiceId, bool IsDetailedInvoice, bool IsReturnInvoice = false, double InvoiceBalance = 0, double InvoiceAdvance = 0 , bool IsDeletedInvoice = false)
        {
            InitializeComponent();
            this.InvoiceId = InvoiceId;
            this.IsDetailedInvoice = IsDetailedInvoice;
            this.InvoiceBalance = InvoiceBalance;
            this.InvoiceAdvance = InvoiceAdvance;
            this.IsReturnInvoice = IsReturnInvoice;
            this.IsDeletedInvoice = IsDeletedInvoice;

        }
        private void POSInvoiceViewer_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsDetailedInvoice)
                {
                    if (IsDeletedInvoice)
                    {
                        int chkDiscount = 0;
                        InvoiceVM i = new InvoiceVM();
                        using (unitOfWork = new UnitOfWork())
                        {
                            i = unitOfWork.InvoiceRepository.GetDeletedInvoiceDetailById(this.InvoiceId);
                        }
                        //i.InvoicePayments.ToList().RemoveAt(0);
                        //if (SharedVariables.AdminInvoiceSetting == null)
                        //{
                        //    DefaultDetailedPrint(i);
                        //    return;
                        //}

                        //getInvoiceItem
                        foreach (var check in i.InvoiceItems)
                        {
                            if (check.Discount > 0)
                            {
                                chkDiscount = 1;
                            }
                        }
                        if (i.DiscountType == 2 && chkDiscount == 0)
                        {
                            i.TotalDiscount = (i.SubTotal / 100) * i.TotalDiscount;
                        }
                        foreach (InvoicePaymentVM v in i.InvoicePayments)
                        {
                            if (v.PaymentType == 1) { v.PaymentTypeString = "Cash"; }
                            else if (v.PaymentType == 2) { v.PaymentTypeString = "Cheque"; }
                            else if (v.PaymentType == 3) { v.PaymentTypeString = "Debit/Credit Card"; }
                            else if (v.PaymentType == 4) { v.PaymentTypeString = "Online Payment"; }
                        }
                        if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                        {
                            #region A4
                            if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                            {
                                POSInvoiceA4_Detailed_Portrait_Update r = new POSInvoiceA4_Detailed_Portrait_Update();
                                //POSInvoiceA4_Detailed_Portrait r = new POSInvoiceA4_Detailed_Portrait();
                                //PageMargins p = new PageMargins();
                                //p.leftMargin = 2000;
                                //p.rightMargin= 2000;                            
                                //p.topMargin= 5000;
                                //r.PrintOptions.ApplyPageMargins(p);
                                //PageMargins p = r.PrintOptions.PageMargins.topMargin;
                                //PageMargins p = r.PrintOptions.PageMargins.rightMargin;
                                //PageMargins p = r.PrintOptions.PageMargins.leftMargin;
                                //r.Section1.SectionFormat.BackgroundColor = Color.Green;//.ReportObjects["PharmacyName"].Left = 20;
                                //int l = r.ReportDefinition.ReportObjects["PharmacyName1"].Left = 5000;

                                //int w = r.ReportDefinition.ReportObjects["PharmacyName1"].Width;
                                //Console.WriteLine("w=> " + w.ToString());
                                //r.ReportDefinition.ReportObjects["LogoImage1"].Left = 5000;
                                //foreach (ReportObject reportObject in r.ReportDefinition.ReportObjects)
                                //{
                                //    Console.WriteLine("=> " + reportObject.Name + Environment.NewLine);
                                //}
                                //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                                //POSInvoiceA4_Detailed_Portrait r = new POSInvoiceA4_Detailed_Portrait();
                                //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                                AssingUnitPack(i.InvoiceItems);
                                AssingUnitsalestax(i.InvoiceItems);
                                r.Database.Tables[0].SetDataSource(i.InvoiceItems);
                                if (i.CashBack > 0)
                                {
                                    foreach (var set in i.InvoicePayments)
                                    {
                                        int calset = Convert.ToInt32(set.Payment) + Convert.ToInt32(i.CashBack);
                                        set.PaymentString = "Rs. " + calset + "/-";
                                    }
                                }
                                r.Database.Tables[2].SetDataSource(i.InvoicePayments);
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
                                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                                r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                                double serviceSet = 0;
                                if (i.ServiceType == 2)
                                {
                                    i.ServiceType = i.ServiceType - 1;
                                    if (i.ServiceType == 1)
                                    {
                                        serviceSet = (i.Services / 100) * i.SubTotal;
                                        i.ServiceCheck = serviceSet;
                                    }
                                }
                                else
                                {
                                    serviceSet = i.Services;
                                    i.ServiceCheck = 0;
                                    i.ServiceType = 0;
                                }

                                InvoicePortraitA4(i, r);
                                r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                                r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount + serviceSet).ToString() + "/-");
                                r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                                //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                                //double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount;
                                //double DueAmount = i.GrandTotal - i.TotalPaid;
                                //double RefundAmount = i.TotalRefund;
                                //r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                                //r.SetParameterValue("DueAmountNumVal", DueAmount);
                                //r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                                //r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                                //r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                                //r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());

                                r.SetParameterValue("HeaderFooterPref", SharedVariables.AdminPrintFormatSetting.HeaderFooterPref);
                                r.SetParameterValue("PatientDetailPref", SharedVariables.AdminPrintFormatSetting.PatientDetailsPref);
                                r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                                r.SetParameterValue("Setting_ShowUserName", SharedVariables.AdminInvoiceSetting.ShowUserName);
                                r.SetParameterValue("Setting_HideRateAndQuantity", SharedVariables.AdminInvoiceSetting.HideRateAndQuantityInPrintFormat);
                                r.SetParameterValue("Setting_HideDuesAndAdvanceFromInvoiceAndPayamnetReceipts", SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts);
                                r.SetParameterValue("Setting_PrintPractiseName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.Show();
                            }
                            if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                            {
                                POSInvoiceA4_Detailed_Landscape r = new POSInvoiceA4_Detailed_Landscape();
                                //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                                r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                                r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                                {
                                    r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                                }

                                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                                r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                                r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                                r.SetParameterValue("CreateDate", i.CreateDate);
                                r.SetParameterValue("InvoiceNote", i.Note);
                                if (i.ObjPatient == null)
                                {
                                    r.SetParameterValue("PatientName", "");
                                    r.SetParameterValue("MrNo", "");
                                    r.SetParameterValue("PatientPhone", "");
                                    r.SetParameterValue("Gender", "");
                                    r.SetParameterValue("DateOfBirth", "");
                                    r.SetParameterValue("Age", "");
                                }
                                else
                                {
                                    r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                    r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                    r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                    r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                    r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                    r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                                }

                                double serviceSet = 0;
                                if (i.ServiceType == 2)
                                {
                                    i.ServiceType = i.ServiceType - 1;
                                    if (i.ServiceType == 1)
                                    {
                                        serviceSet = (i.Services / 100) * i.SubTotal;
                                        i.ServiceCheck = serviceSet;
                                    }
                                }
                                else
                                {
                                    serviceSet = i.Services;
                                    i.ServiceCheck = 0;
                                }
                                r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                                r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount + serviceSet).ToString() + "/-");
                                //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                                r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                                double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                                double DueAmount = i.GrandTotal - i.TotalPaid;
                                r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                                r.SetParameterValue("DueAmountNumVal", DueAmount);
                                r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                                r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.Show();
                            }
                            #endregion
                        }
                        else if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
                        {
                            #region A5
                            if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                            {
                                POSInvoiceA5_Detailed_Portrait r = new POSInvoiceA5_Detailed_Portrait();
                                //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                                r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                                r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                                {
                                    r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                                }

                                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);

                                r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                                r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                                r.SetParameterValue("CreateDate", i.CreateDate);
                                r.SetParameterValue("InvoiceNote", i.Note);
                                if (i.ObjPatient == null)
                                {
                                    r.SetParameterValue("PatientName", "");
                                    r.SetParameterValue("MrNo", "");
                                    r.SetParameterValue("PatientPhone", "");
                                    r.SetParameterValue("Gender", "");
                                    r.SetParameterValue("DateOfBirth", "");
                                    r.SetParameterValue("Age", "");
                                }
                                else
                                {
                                    r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                    r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                    r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                    r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                    r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                    r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                                }

                                r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                                r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                                //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                                r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                                double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                                double DueAmount = i.GrandTotal - i.TotalPaid;
                                r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                                r.SetParameterValue("DueAmountNumVal", DueAmount);
                                r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                                r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.Show();
                            }
                            if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                            {
                            }
                            #endregion
                        }
                        else
                        {
                            if (SharedVariables.AdminPrintFormatSetting.PrintFormatIndex == 0)
                            {
                                #region A8
                                POSInvoiceA8_Detailed r = new POSInvoiceA8_Detailed();
                                AssignTotalAmountToAmount(i.InvoiceItems);
                                AssingUnitsalestax(i.InvoiceItems);
                                r.Database.Tables[1].SetDataSource(i.InvoiceItems);

                                if (i.CashBack > 0)
                                {
                                    foreach (var set in i.InvoicePayments)
                                    {
                                        int calset = Convert.ToInt32(set.Payment) + Convert.ToInt32(i.CashBack);
                                        set.PaymentString = "Rs. " + calset + "/-";
                                    }
                                }
                                r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                                {
                                    r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                                }
                                AssingUnitPack(i.InvoiceItems);

                                r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                                InvoiceA8(i, r);
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
                                if (i.CashBack > 0)
                                {
                                    r.SetParameterValue("IsInvoiceBalance", true);
                                    r.SetParameterValue("InvoiceBalanceVal", "Rs. " + Math.Round(i.CashBack).ToString() + "/-");
                                }
                                else
                                {
                                    r.SetParameterValue("IsInvoiceBalance", false);
                                    r.SetParameterValue("InvoiceBalanceVal", "");
                                }
                                r.SetParameterValue("FBRInvoiceNo", i.FBRInvoiceNo);
                                if (!string.IsNullOrEmpty(i.Note))
                                    r.SetParameterValue("NoteVal", true);
                                else
                                    r.SetParameterValue("NoteVal", false);
                                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                                r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                                r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                                r.SetParameterValue("CreateDate", i.CreateDate);
                                r.SetParameterValue("InvoiceNote", i.Note);
                                if (i.ObjPatient == null)
                                {
                                    r.SetParameterValue("PatientName", "");
                                    r.SetParameterValue("MrNo", "");
                                    r.SetParameterValue("PatientPhone", "");
                                    r.SetParameterValue("PatientAddress", "");
                                    r.SetParameterValue("Gender", "");
                                    r.SetParameterValue("DateOfBirth", "");
                                    r.SetParameterValue("Age", "");
                                    r.SetParameterValue("PatientEmail", "");
                                }
                                else
                                {
                                    r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                    r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                    r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                    r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                    r.SetParameterValue("PatientAddress", i.ObjPatient.Address);
                                    r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.ObjPatient.DateOfBirth.ToString());
                                    r.SetParameterValue("Age", i.ObjPatient.AgeYears == null ? "" : i.ObjPatient.AgeYears.ToString() + " Years");
                                    r.SetParameterValue("PatientEmail", i.ObjPatient.Email == null ? "" : i.ObjPatient.Email.ToString());
                                }
                                r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                                r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                                r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                                r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                                r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);
                                r.SetParameterValue("ShowPatientAddress", SharedVariables.AdminInvoiceSetting.ShowAddress);
                                r.SetParameterValue("ShowEmail", SharedVariables.AdminInvoiceSetting.ShowEmail);

                                r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                                r.SetParameterValue("TotalDiscount", i.TotalDiscount > 0 ? "Rs. " + Math.Round(i.TotalDiscount).ToString() : "");

                                if (i.ServiceType == 2)
                                {
                                    i.ServiceType = i.ServiceType - 1;
                                    if (i.ServiceType == 1)
                                    {
                                        i.Services = (i.Services / 100) * i.SubTotal;
                                    }
                                }
                                string InvoiceType = i.InvoiceType;
                                r.SetParameterValue("InvoiceType", InvoiceType);
                                r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount + i.Services).ToString() + "/-");
                                //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                                r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                                //double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount - i.Services;
                                double DueAmount = i.GrandTotal - i.TotalPaid;

                                double AdvanceAmount = -1 * DueAmount;
                                double RefundAmount = i.TotalRefund;
                                if (AdvanceAmount > 0 || DueAmount > 0 && SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts == true)
                                    r.SetParameterValue("AdvanceDueAmountVal", true);
                                else
                                    r.SetParameterValue("AdvanceDueAmountVal", false);

                                r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                                r.SetParameterValue("DueAmountNumVal", DueAmount);
                                r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                                r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());

                                r.SetParameterValue("RefundAmountVal", RefundAmount > 0 ? true : false);
                                r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                                r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());

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
                                //r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                                //r.SetParameterValue("ShowPharmaPhone", SharedVariables.AdminInvoiceSetting.ShowPharmaPhone);
                                //r.SetParameterValue("ShowPharmaAddress", SharedVariables.AdminInvoiceSetting.ShowPharmaAddress);

                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.Show();

                                #endregion

                            }
                            else
                            {
                                #region A8 Design 2
                                POSInvoiceA8_Detailed_2 r = new POSInvoiceA8_Detailed_2();
                                AssignTotalAmountToAmount(i.InvoiceItems);
                                AssingUnitsalestax(i.InvoiceItems);
                                r.Database.Tables[1].SetDataSource(i.InvoiceItems);

                                if (i.CashBack > 0)
                                {
                                    foreach (var set in i.InvoicePayments)
                                    {
                                        int calset = Convert.ToInt32(set.Payment) + Convert.ToInt32(i.CashBack);
                                        set.PaymentString = "Rs. " + calset + "/-";
                                    }
                                }
                                r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                                {
                                    r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                                }
                                AssingUnitPack(i.InvoiceItems);

                                r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                                InvoiceA8_2(i, r);
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
                                if (i.CashBack > 0)
                                {
                                    r.SetParameterValue("IsInvoiceBalance", true);
                                    r.SetParameterValue("InvoiceBalanceVal", "Rs. " + Math.Round(i.CashBack).ToString() + "/-");
                                }
                                else
                                {
                                    r.SetParameterValue("IsInvoiceBalance", false);
                                    r.SetParameterValue("InvoiceBalanceVal", "");
                                }
                                r.SetParameterValue("FBRInvoiceNo", i.FBRInvoiceNo);
                                if (!string.IsNullOrEmpty(i.Note))
                                    r.SetParameterValue("NoteVal", true);
                                else
                                    r.SetParameterValue("NoteVal", false);
                                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                                r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                                r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                                r.SetParameterValue("CreateDate", i.CreateDate);
                                r.SetParameterValue("InvoiceNote", i.Note);
                                if (i.ObjPatient == null)
                                {
                                    r.SetParameterValue("PatientName", "");
                                    r.SetParameterValue("MrNo", "");
                                    r.SetParameterValue("PatientPhone", "");
                                    r.SetParameterValue("PatientAddress", "");
                                    r.SetParameterValue("Gender", "");
                                    r.SetParameterValue("DateOfBirth", "");
                                    r.SetParameterValue("Age", "");
                                    r.SetParameterValue("PatientEmail", "");
                                }
                                else
                                {
                                    r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                    r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                    r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                    r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                    r.SetParameterValue("PatientAddress", i.ObjPatient.Address);
                                    r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.ObjPatient.DateOfBirth.ToString());
                                    r.SetParameterValue("Age", i.ObjPatient.AgeYears == null ? "" : i.ObjPatient.AgeYears.ToString() + " Years");
                                    r.SetParameterValue("PatientEmail", i.ObjPatient.Email == null ? "" : i.ObjPatient.Email.ToString());
                                }
                                r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                                r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                                r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                                r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                                r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);
                                r.SetParameterValue("ShowPatientAddress", SharedVariables.AdminInvoiceSetting.ShowAddress);
                                r.SetParameterValue("ShowEmail", SharedVariables.AdminInvoiceSetting.ShowEmail);

                                r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                                r.SetParameterValue("TotalDiscount", i.TotalDiscount > 0 ? "Rs. " + Math.Round(i.TotalDiscount).ToString() : "");

                                if (i.ServiceType == 2)
                                {
                                    i.ServiceType = i.ServiceType - 1;
                                    if (i.ServiceType == 1)
                                    {
                                        i.Services = (i.Services / 100) * i.SubTotal;
                                    }
                                }
                                string InvoiceType = i.InvoiceType;
                                r.SetParameterValue("InvoiceType", InvoiceType);
                                r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount + i.Services).ToString() + "/-");
                                //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                                r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                                //double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount - i.Services;
                                double DueAmount = i.GrandTotal - i.TotalPaid;

                                double AdvanceAmount = -1 * DueAmount;
                                double RefundAmount = i.TotalRefund;
                                if (AdvanceAmount > 0 || DueAmount > 0 && SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts == true)
                                    r.SetParameterValue("AdvanceDueAmountVal", true);
                                else
                                    r.SetParameterValue("AdvanceDueAmountVal", false);

                                r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                                r.SetParameterValue("DueAmountNumVal", DueAmount);
                                r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                                r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());

                                r.SetParameterValue("RefundAmountVal", RefundAmount > 0 ? true : false);
                                r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                                r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());

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
                                //r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                                //r.SetParameterValue("ShowPharmaPhone", SharedVariables.AdminInvoiceSetting.ShowPharmaPhone);
                                //r.SetParameterValue("ShowPharmaAddress", SharedVariables.AdminInvoiceSetting.ShowPharmaAddress);

                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.Show();

                                #endregion
                            }
                        }
                    }
                    else
                    {
                        
                        int chkDiscount = 0;
                        InvoiceVM i = new InvoiceVM();
                        using (unitOfWork = new UnitOfWork())
                        {
                            i = unitOfWork.InvoiceRepository.GetPOSInvoiceDetailById(this.InvoiceId);
                        }
                        //i.InvoicePayments.ToList().RemoveAt(0);
                        //if (SharedVariables.AdminInvoiceSetting == null)
                        //{
                        //    DefaultDetailedPrint(i);
                        //    return;
                        //}

                        //getInvoiceItem
                        foreach (var check in i.InvoiceItems)
                        {
                            if (check.Discount > 0)
                            {
                                chkDiscount = 1;
                            }
                        }
                        if (i.DiscountType == 2 && chkDiscount == 0)
                        {
                            i.TotalDiscount = (i.SubTotal / 100) * i.TotalDiscount;
                        }
                        foreach (InvoicePaymentVM v in i.InvoicePayments)
                        {
                            if (v.PaymentType == 1) { v.PaymentTypeString = "Cash"; }
                            else if (v.PaymentType == 2) { v.PaymentTypeString = "Cheque"; }
                            else if (v.PaymentType == 3) { v.PaymentTypeString = "Debit/Credit Card"; }
                            else if (v.PaymentType == 4) { v.PaymentTypeString = "Online Payment"; }
                        }
                        if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                        {
                            #region A4
                            if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                            {
                                POSInvoiceA4_Detailed_Portrait_Update r = new POSInvoiceA4_Detailed_Portrait_Update();
                                //POSInvoiceA4_Detailed_Portrait r = new POSInvoiceA4_Detailed_Portrait();
                                //PageMargins p = new PageMargins();
                                //p.leftMargin = 2000;
                                //p.rightMargin= 2000;                            
                                //p.topMargin= 5000;
                                //r.PrintOptions.ApplyPageMargins(p);
                                //PageMargins p = r.PrintOptions.PageMargins.topMargin;
                                //PageMargins p = r.PrintOptions.PageMargins.rightMargin;
                                //PageMargins p = r.PrintOptions.PageMargins.leftMargin;
                                //r.Section1.SectionFormat.BackgroundColor = Color.Green;//.ReportObjects["PharmacyName"].Left = 20;
                                //int l = r.ReportDefinition.ReportObjects["PharmacyName1"].Left = 5000;

                                //int w = r.ReportDefinition.ReportObjects["PharmacyName1"].Width;
                                //Console.WriteLine("w=> " + w.ToString());
                                //r.ReportDefinition.ReportObjects["LogoImage1"].Left = 5000;
                                //foreach (ReportObject reportObject in r.ReportDefinition.ReportObjects)
                                //{
                                //    Console.WriteLine("=> " + reportObject.Name + Environment.NewLine);
                                //}
                                //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                                //POSInvoiceA4_Detailed_Portrait r = new POSInvoiceA4_Detailed_Portrait();
                                //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                                AssingUnitPack(i.InvoiceItems);
                                AssingUnitsalestax(i.InvoiceItems);
                                r.Database.Tables[0].SetDataSource(i.InvoiceItems);
                                if (i.CashBack > 0)
                                {
                                    foreach (var set in i.InvoicePayments)
                                    {
                                        int calset = Convert.ToInt32(set.Payment) + Convert.ToInt32(i.CashBack);
                                        set.PaymentString = "Rs. " + calset + "/-";
                                    }
                                }
                                r.Database.Tables[2].SetDataSource(i.InvoicePayments);
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
                                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                                r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                                double serviceSet = 0;
                                if (i.ServiceType == 2)
                                {
                                    i.ServiceType = i.ServiceType - 1;
                                    if (i.ServiceType == 1)
                                    {
                                        serviceSet = (i.Services / 100) * i.SubTotal;
                                        i.ServiceCheck = serviceSet;
                                    }
                                }
                                else
                                {
                                    serviceSet = i.Services;
                                    i.ServiceCheck = 0;
                                    i.ServiceType = 0;
                                }

                                InvoicePortraitA4(i, r);
                                r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                                r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount + serviceSet).ToString() + "/-");
                                r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                                //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                                //double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount;
                                //double DueAmount = i.GrandTotal - i.TotalPaid;
                                //double RefundAmount = i.TotalRefund;
                                //r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                                //r.SetParameterValue("DueAmountNumVal", DueAmount);
                                //r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                                //r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                                //r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                                //r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());

                                r.SetParameterValue("HeaderFooterPref", SharedVariables.AdminPrintFormatSetting.HeaderFooterPref);
                                r.SetParameterValue("PatientDetailPref", SharedVariables.AdminPrintFormatSetting.PatientDetailsPref);
                                r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                                r.SetParameterValue("Setting_ShowUserName", SharedVariables.AdminInvoiceSetting.ShowUserName);
                                r.SetParameterValue("Setting_HideRateAndQuantity", SharedVariables.AdminInvoiceSetting.HideRateAndQuantityInPrintFormat);
                                r.SetParameterValue("Setting_HideDuesAndAdvanceFromInvoiceAndPayamnetReceipts", SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts);
                                r.SetParameterValue("Setting_PrintPractiseName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.Show();
                            }
                            if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                            {
                                POSInvoiceA4_Detailed_Landscape r = new POSInvoiceA4_Detailed_Landscape();
                                //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                                r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                                r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                                {
                                    r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                                }

                                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                                r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                                r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                                r.SetParameterValue("CreateDate", i.CreateDate);
                                r.SetParameterValue("InvoiceNote", i.Note);
                                if (i.ObjPatient == null)
                                {
                                    r.SetParameterValue("PatientName", "");
                                    r.SetParameterValue("MrNo", "");
                                    r.SetParameterValue("PatientPhone", "");
                                    r.SetParameterValue("Gender", "");
                                    r.SetParameterValue("DateOfBirth", "");
                                    r.SetParameterValue("Age", "");
                                }
                                else
                                {
                                    r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                    r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                    r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                    r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                    r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                    r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                                }

                                double serviceSet = 0;
                                if (i.ServiceType == 2)
                                {
                                    i.ServiceType = i.ServiceType - 1;
                                    if (i.ServiceType == 1)
                                    {
                                        serviceSet = (i.Services / 100) * i.SubTotal;
                                        i.ServiceCheck = serviceSet;
                                    }
                                }
                                else
                                {
                                    serviceSet = i.Services;
                                    i.ServiceCheck = 0;
                                }
                                r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                                r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount + serviceSet).ToString() + "/-");
                                //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                                r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                                double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                                double DueAmount = i.GrandTotal - i.TotalPaid;
                                r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                                r.SetParameterValue("DueAmountNumVal", DueAmount);
                                r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                                r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.Show();
                            }
                            #endregion
                        }
                        else if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
                        {
                            #region A5
                            if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                            {
                                POSInvoiceA5_Detailed_Portrait r = new POSInvoiceA5_Detailed_Portrait();
                                //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                                r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                                r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                                {
                                    r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                                }

                                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);

                                r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                                r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                                r.SetParameterValue("CreateDate", i.CreateDate);
                                r.SetParameterValue("InvoiceNote", i.Note);
                                if (i.ObjPatient == null)
                                {
                                    r.SetParameterValue("PatientName", "");
                                    r.SetParameterValue("MrNo", "");
                                    r.SetParameterValue("PatientPhone", "");
                                    r.SetParameterValue("Gender", "");
                                    r.SetParameterValue("DateOfBirth", "");
                                    r.SetParameterValue("Age", "");
                                }
                                else
                                {
                                    r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                    r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                    r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                    r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                    r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                    r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                                }

                                r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                                r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                                //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                                r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                                double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                                double DueAmount = i.GrandTotal - i.TotalPaid;
                                r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                                r.SetParameterValue("DueAmountNumVal", DueAmount);
                                r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                                r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.Show();
                            }
                            if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                            {
                            }
                            #endregion
                        }
                        else
                        {
                            if (SharedVariables.AdminPrintFormatSetting.PrintFormatIndex == 0)
                            {
                                #region A8
                                POSInvoiceA8_Detailed r = new POSInvoiceA8_Detailed();
                                AssignTotalAmountToAmount(i.InvoiceItems);
                                AssingUnitsalestax(i.InvoiceItems);
                                r.Database.Tables[1].SetDataSource(i.InvoiceItems);

                                if (i.CashBack > 0)
                                {
                                    foreach (var set in i.InvoicePayments)
                                    {
                                        int calset = Convert.ToInt32(set.Payment) + Convert.ToInt32(i.CashBack);
                                        set.PaymentString = "Rs. " + calset + "/-";
                                    }
                                }
                                r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                                {
                                    r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                                }
                                AssingUnitPack(i.InvoiceItems);

                                r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                                InvoiceA8(i, r);
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
                                if (i.CashBack > 0)
                                {
                                    r.SetParameterValue("IsInvoiceBalance", true);
                                    r.SetParameterValue("InvoiceBalanceVal", "Rs. " + Math.Round(i.CashBack).ToString() + "/-");
                                }
                                else
                                {
                                    r.SetParameterValue("IsInvoiceBalance", false);
                                    r.SetParameterValue("InvoiceBalanceVal", "");
                                }
                                r.SetParameterValue("FBRInvoiceNo", i.FBRInvoiceNo);
                                if (!string.IsNullOrEmpty(i.Note))
                                    r.SetParameterValue("NoteVal", true);
                                else
                                    r.SetParameterValue("NoteVal", false);
                                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                                r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                                r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                                r.SetParameterValue("CreateDate", i.CreateDate);
                                r.SetParameterValue("InvoiceNote", i.Note);
                                if (i.ObjPatient == null)
                                {
                                    r.SetParameterValue("PatientName", "");
                                    r.SetParameterValue("MrNo", "");
                                    r.SetParameterValue("PatientPhone", "");
                                    r.SetParameterValue("PatientAddress", "");
                                    r.SetParameterValue("Gender", "");
                                    r.SetParameterValue("DateOfBirth", "");
                                    r.SetParameterValue("Age", "");
                                    r.SetParameterValue("PatientEmail", "");
                                }
                                else
                                {
                                    r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                    r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                    r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                    r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                    r.SetParameterValue("PatientAddress", i.ObjPatient.Address);
                                    r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.ObjPatient.DateOfBirth.ToString());
                                    r.SetParameterValue("Age", i.ObjPatient.AgeYears == null ? "" : i.ObjPatient.AgeYears.ToString() + " Years");
                                    r.SetParameterValue("PatientEmail", i.ObjPatient.Email == null ? "" : i.ObjPatient.Email.ToString());
                                }
                                r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                                r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                                r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                                r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                                r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);
                                r.SetParameterValue("ShowPatientAddress", SharedVariables.AdminInvoiceSetting.ShowAddress);
                                r.SetParameterValue("ShowEmail", SharedVariables.AdminInvoiceSetting.ShowEmail);

                                r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                                r.SetParameterValue("TotalDiscount", i.TotalDiscount > 0 ? "Rs. " + Math.Round(i.TotalDiscount).ToString() : "");

                                if (i.ServiceType == 2)
                                {
                                    i.ServiceType = i.ServiceType - 1;
                                    if (i.ServiceType == 1)
                                    {
                                        i.Services = (i.Services / 100) * i.SubTotal;
                                    }
                                }
                                string InvoiceType = i.InvoiceType;
                                r.SetParameterValue("InvoiceType", InvoiceType);
                                r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount + i.Services).ToString() + "/-");
                                //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                                r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                                //double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount - i.Services;
                                double DueAmount = i.GrandTotal - i.TotalPaid;

                                double AdvanceAmount = -1 * DueAmount;
                                double RefundAmount = i.TotalRefund;
                                if (AdvanceAmount > 0 || DueAmount > 0 && SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts == true)
                                    r.SetParameterValue("AdvanceDueAmountVal", true);
                                else
                                    r.SetParameterValue("AdvanceDueAmountVal", false);

                                r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                                r.SetParameterValue("DueAmountNumVal", DueAmount);
                                r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                                r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());

                                r.SetParameterValue("RefundAmountVal", RefundAmount > 0 ? true : false);
                                r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                                r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());

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
                                //r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                                //r.SetParameterValue("ShowPharmaPhone", SharedVariables.AdminInvoiceSetting.ShowPharmaPhone);
                                //r.SetParameterValue("ShowPharmaAddress", SharedVariables.AdminInvoiceSetting.ShowPharmaAddress);

                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.Show();

                                #endregion

                            }
                            else
                            {
                                #region A8 Design 2
                                POSInvoiceA8_Detailed_2 r = new POSInvoiceA8_Detailed_2();
                                AssignTotalAmountToAmount(i.InvoiceItems);
                                AssingUnitsalestax(i.InvoiceItems);
                                r.Database.Tables[1].SetDataSource(i.InvoiceItems);

                                if (i.CashBack > 0)
                                {
                                    foreach (var set in i.InvoicePayments)
                                    {
                                        int calset = Convert.ToInt32(set.Payment) + Convert.ToInt32(i.CashBack);
                                        set.PaymentString = "Rs. " + calset + "/-";
                                    }
                                }
                                r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                                {
                                    r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                                }
                                AssingUnitPack(i.InvoiceItems);

                                r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                                InvoiceA8_2(i, r);
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
                                if (i.CashBack > 0)
                                {
                                    r.SetParameterValue("IsInvoiceBalance", true);
                                    r.SetParameterValue("InvoiceBalanceVal", "Rs. " + Math.Round(i.CashBack).ToString() + "/-");
                                }
                                else
                                {
                                    r.SetParameterValue("IsInvoiceBalance", false);
                                    r.SetParameterValue("InvoiceBalanceVal", "");
                                }
                                r.SetParameterValue("FBRInvoiceNo", i.FBRInvoiceNo);
                                if (!string.IsNullOrEmpty(i.Note))
                                    r.SetParameterValue("NoteVal", true);
                                else
                                    r.SetParameterValue("NoteVal", false);
                                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                                r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                                r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                                r.SetParameterValue("CreateDate", i.CreateDate);
                                r.SetParameterValue("InvoiceNote", i.Note);
                                if (i.ObjPatient == null)
                                {
                                    r.SetParameterValue("PatientName", "");
                                    r.SetParameterValue("MrNo", "");
                                    r.SetParameterValue("PatientPhone", "");
                                    r.SetParameterValue("PatientAddress", "");
                                    r.SetParameterValue("Gender", "");
                                    r.SetParameterValue("DateOfBirth", "");
                                    r.SetParameterValue("Age", "");
                                    r.SetParameterValue("PatientEmail", "");
                                }
                                else
                                {
                                    r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                    r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                    r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                    r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                    r.SetParameterValue("PatientAddress", i.ObjPatient.Address);
                                    r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.ObjPatient.DateOfBirth.ToString());
                                    r.SetParameterValue("Age", i.ObjPatient.AgeYears == null ? "" : i.ObjPatient.AgeYears.ToString() + " Years");
                                    r.SetParameterValue("PatientEmail", i.ObjPatient.Email == null ? "" : i.ObjPatient.Email.ToString());
                                }
                                r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                                r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                                r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                                r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                                r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);
                                r.SetParameterValue("ShowPatientAddress", SharedVariables.AdminInvoiceSetting.ShowAddress);
                                r.SetParameterValue("ShowEmail", SharedVariables.AdminInvoiceSetting.ShowEmail);

                                r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                                r.SetParameterValue("TotalDiscount", i.TotalDiscount > 0 ? "Rs. " + Math.Round(i.TotalDiscount).ToString() : "");

                                if (i.ServiceType == 2)
                                {
                                    i.ServiceType = i.ServiceType - 1;
                                    if (i.ServiceType == 1)
                                    {
                                        i.Services = (i.Services / 100) * i.SubTotal;
                                    }
                                }
                                string InvoiceType = i.InvoiceType;
                                r.SetParameterValue("InvoiceType", InvoiceType);
                                r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount + i.Services).ToString() + "/-");
                                //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                                r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                                //double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount - i.Services;
                                double DueAmount = i.GrandTotal - i.TotalPaid;

                                double AdvanceAmount = -1 * DueAmount;
                                double RefundAmount = i.TotalRefund;
                                if (AdvanceAmount > 0 || DueAmount > 0 && SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts == true)
                                    r.SetParameterValue("AdvanceDueAmountVal", true);
                                else
                                    r.SetParameterValue("AdvanceDueAmountVal", false);

                                r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                                r.SetParameterValue("DueAmountNumVal", DueAmount);
                                r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                                r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());

                                r.SetParameterValue("RefundAmountVal", RefundAmount > 0 ? true : false);
                                r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                                r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());

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
                                //r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                                //r.SetParameterValue("ShowPharmaPhone", SharedVariables.AdminInvoiceSetting.ShowPharmaPhone);
                                //r.SetParameterValue("ShowPharmaAddress", SharedVariables.AdminInvoiceSetting.ShowPharmaAddress);

                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.Show();

                                #endregion
                            }
                        }
                    }
                    
                }
                else
                {
                    InvoiceVM i = new InvoiceVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        i = unitOfWork.InvoiceRepository.GetPOSInvoiceById(this.InvoiceId);
                    }
                    string Description = "";
                    foreach (InvoiceItemVM item in i.InvoiceItems)
                    {
                        Description += item.ItemName + ", ";
                    }
                    Description = Description.Trim().TrimEnd(',');
                    string PaymentMode = getPaymentModeString(i.PaymentType);
                    //if(SharedVariables.AdminInvoiceSetting == null)
                    //{
                    //    DefaultGeneralPrint(i, Description);
                    //    return;
                    //}
                    if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                    {
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA4_Portrait r = new POSInvoiceA4_Portrait();
                            //string path = @"D:\DesktopApplications\Pharmacy\Pharmacy - InvoiceLinwItemRemove\Pharmacy.UI\Theme\HWlogoWithoutText.jpg";
                            //DataTable dt = SharedFunctions.GetImageTable(path);                            
                            //r.Database.Tables[3].SetDataSource(dt);
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }
                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreateDate);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            crystalReportViewer1.ReportSource = r;
                            crystalReportViewer1.Show();
                        }
                        else if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                            POSInvoiceA4_Landscape r = new POSInvoiceA4_Landscape();

                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }
                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreateDate);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            crystalReportViewer1.ReportSource = r;
                            crystalReportViewer1.Show();
                        }
                    }
                    else if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
                    {
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA5_Portrait r = new POSInvoiceA5_Portrait();

                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreateDate);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            crystalReportViewer1.ReportSource = r;
                            crystalReportViewer1.Show();
                        }
                        else if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                            POSInvoiceA5_Landscape r = new POSInvoiceA5_Landscape();

                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreateDate);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            crystalReportViewer1.ReportSource = r;
                            crystalReportViewer1.Show();
                        }
                    }
                    else //A8
                    {
                        POSInvoiceA8 r = new POSInvoiceA8();
                        //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                        //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                        //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                        if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                        {
                            r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                        }

                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                        r.SetParameterValue("CreateDate", i.CreateDate);
                        r.SetParameterValue("InvoiceNote", i.Note);
                        if (i.ObjPatient == null)
                        {
                            r.SetParameterValue("PatientName", "");
                            r.SetParameterValue("MrNo", "");
                            r.SetParameterValue("PatientPhone", "");
                            r.SetParameterValue("Gender", "");
                            r.SetParameterValue("DateOfBirth", "");
                            r.SetParameterValue("Age", "");
                        }
                        else
                        {
                            r.SetParameterValue("PatientName", i.ObjPatient.Name);
                            r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                            r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                            r.SetParameterValue("Gender", i.ObjPatient.Gender);
                            r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                            r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                        }

                        //r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                        //r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                        //r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                        //r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                        //r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);

                        r.SetParameterValue("Description", Description);
                        r.SetParameterValue("PaymentMode", PaymentMode);

                        //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                        //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                        double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                        double DueAmount = i.GrandTotal - i.TotalPaid;
                        r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                        r.SetParameterValue("DueAmountNumVal", DueAmount);
                        r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                        r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());

                        //r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                        //r.SetParameterValue("ShowPharmaPhone", SharedVariables.AdminInvoiceSetting.ShowPharmaPhone);
                        //r.SetParameterValue("ShowPharmaAddress", SharedVariables.AdminInvoiceSetting.ShowPharmaAddress);

                        crystalReportViewer1.ReportSource = r;
                        crystalReportViewer1.Show();
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

        private static void InvoiceA8(InvoiceVM i, POSInvoiceA8_Detailed r)
        {
            if (SharedVariables.AdminPharmacySetting.IsFBRUnable)
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(string.IsNullOrEmpty(i.FBRInvoiceNo) ? "" : i.FBRInvoiceNo, QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                r.Database.Tables[4].SetDataSource(SharedFunctions.GetImageTable(qrCodeImage));
            }

            r.SetParameterValue("ShowFBR", SharedVariables.AdminPharmacySetting.IsFBRUnable);
            r.SetParameterValue("ShowReturnQty", i.InvoiceItems.Any(x => x.ReturnedQuantity > 0) == false ? true : false);
            r.SetParameterValue("ShowPOSGrandTotal", SharedVariables.AdminInvoiceSetting.ShowPOSGrandTotal == true ? false : true);
            r.SetParameterValue("ShowPOSSubTotal", SharedVariables.AdminInvoiceSetting.ShowPOSSubTotal == true ? false : true);
            r.SetParameterValue("ShowPOSService", SharedVariables.AdminInvoiceSetting.ShowPOSService == true ? false : true);

            if (SharedVariables.AdminInvoiceSetting.ShowPOSService == true)
            {
                r.SetParameterValue("Services", "");
            }
            else
            {
                if (i.ServiceType == 2)
                {
                    r.SetParameterValue("Services", i.Services > 0 ? "Rs. " + Math.Round(i.Services, 2).ToString() + "" + "%" : "");
                }
                else
                {
                    r.SetParameterValue("Services", i.Services > 0 ? "Rs. " + Math.Round(i.Services, 2).ToString() : "");
                }
            }
        }
        private static void InvoiceA8_2(InvoiceVM i, POSInvoiceA8_Detailed_2 r)
        {
            if (SharedVariables.AdminPharmacySetting.IsFBRUnable)
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(string.IsNullOrEmpty(i.FBRInvoiceNo) ? "" : i.FBRInvoiceNo, QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                r.Database.Tables[4].SetDataSource(SharedFunctions.GetImageTable(qrCodeImage));
            }

            r.SetParameterValue("ShowFBR", SharedVariables.AdminPharmacySetting.IsFBRUnable);
            r.SetParameterValue("ShowReturnQty", i.InvoiceItems.Any(x => x.ReturnedQuantity > 0) == false ? true : false);
            r.SetParameterValue("ShowPOSGrandTotal", SharedVariables.AdminInvoiceSetting.ShowPOSGrandTotal == true ? false : true);
            r.SetParameterValue("ShowPOSSubTotal", SharedVariables.AdminInvoiceSetting.ShowPOSSubTotal == true ? false : true);
            r.SetParameterValue("ShowPOSService", SharedVariables.AdminInvoiceSetting.ShowPOSService == true ? false : true);

            if (SharedVariables.AdminInvoiceSetting.ShowPOSService == true)
            {
                r.SetParameterValue("Services", "");
            }
            else
            {
                if (i.ServiceType == 2)
                {
                    r.SetParameterValue("Services", i.Services > 0 ? "Rs. " + Math.Round(i.Services, 2).ToString() + "" + "%" : "");
                }
                else
                {
                    r.SetParameterValue("Services", i.Services > 0 ? "Rs. " + Math.Round(i.Services, 2).ToString() : "");
                }
            }
        }

        private string getPaymentModeString(int PaymentType)
        {
            if (PaymentType == 1) { return "Cash"; }
            else if (PaymentType == 2) { return "Cheque"; }
            else if (PaymentType == 3) { return "Debit/Credit Card"; }
            else if (PaymentType == 4) { return "Online Payment"; }
            else { return "Incorrect Payment Mode"; }
        }
        int HeaderContentSection = 0;
        private void InvoicePortraitA4(InvoiceVM i, POSInvoiceA4_Detailed_Portrait_Update r)
        {
            //double RefundAmount = i.TotalRefund;
            //double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount;
            //double DueAmount = i.GrandTotal - i.TotalPaid;

            if (i.ServiceCheck == 0) 
            {
                i.ServiceCheck = i.Services;
            }
            else
            {
                //i.Services = i.ServiceCheck;
            }
            //double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount - i.ServiceCheck;

            double DueAmount = i.GrandTotal - i.TotalPaid;
            double AdvanceAmount = -1 * DueAmount;
            double RefundAmount = i.TotalRefund;
            AdminInvoiceSetting setting = new AdminInvoiceSetting();
            if (AdvanceAmount > 0 || DueAmount > 0 && SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts == true)
                r.SetParameterValue("AdvanceDueAmountVal", true);
            else
                r.SetParameterValue("AdvanceDueAmountVal", false);
            //r.SetParameterValue("ShowPOSService", SharedVariables.AdminInvoiceSetting.ShowPOSService == true ? false : true);
            if (SharedVariables.AdminInvoiceSetting.ShowPOSService == true) { r.SetParameterValue("Services", ""); }
            else
            {
                if (i.ServiceType == 1)
                {
                    r.SetParameterValue("Services", i.Services > 0 ? "Rs. " + Math.Round(i.Services, 2).ToString() + "" + "%" : "");
                }
                else
                {
                    r.SetParameterValue("Services", i.Services > 0 ? "Rs. " + Math.Round(i.Services, 2).ToString() : "");
                }
            }
            
            
                r.SetParameterValue("TotalDiscount", i.TotalDiscount > 0 ? "Rs. " + Math.Round(i.TotalDiscount).ToString() : "");
           
            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
            r.SetParameterValue("DueAmountNumVal", DueAmount);
            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());

            r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount + i.Services).ToString() + "/-");
            r.SetParameterValue("RefundAmountVal", RefundAmount > 0 ? true : false);
            r.SetParameterValue("RefundAmountNumVal", RefundAmount);
            r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());
            if (i.CashBack > 0)
            {
                r.SetParameterValue("IsInvoiceBalance", true);
                r.SetParameterValue("InvoiceBalanceVal", "Rs. " + Math.Round(i.CashBack).ToString() + "/-");
            }
            else
            {
                r.SetParameterValue("IsInvoiceBalance", false);
                r.SetParameterValue("InvoiceBalanceVal", "");
            }

            if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.Phone) && SharedVariables.AdminInvoiceSetting.ShowPharmaPhone == true)
                r.SetParameterValue("ShowPharmaPhone", true);
            else
                r.SetParameterValue("ShowPharmaPhone", false);
            if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.Address) && SharedVariables.AdminInvoiceSetting.ShowPharmaAddress == true)
                r.SetParameterValue("ShowPharmaAddress", true);
            else
                r.SetParameterValue("ShowPharmaAddress", false);

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
            if (!string.IsNullOrEmpty(i.Note) || !string.IsNullOrEmpty(SharedVariables.AdminInvoiceSetting.InvoiceNote))
                r.SetParameterValue("NoteVal", true);
            else
                r.SetParameterValue("NoteVal", false);
            r.SetParameterValue("InvoiceNote", i.Note);
            if (i.ObjPatient == null)
            {
                i.ObjPatient = new Core.Entities.Patient();
                r.SetParameterValue("PatientName", "");
                r.SetParameterValue("MrNo", "");
                r.SetParameterValue("PatientPhone", "");
                r.SetParameterValue("Gender", "");
                r.SetParameterValue("DateOfBirth", "");
                r.SetParameterValue("Age", "");
                r.SetParameterValue("PatientAddress", "");
                r.SetParameterValue("PatientEmail", "");
            }
            else
            {
                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.ObjPatient.DateOfBirth.ToString());
                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                r.SetParameterValue("PatientAddress", i.ObjPatient.Address == null ? "" : i.ObjPatient.Address);
                r.SetParameterValue("PatientEmail", i.ObjPatient.Email == null ? "" : i.ObjPatient.Email.ToString());
            }
            r.SetParameterValue("ShowPOSGrandTotal", SharedVariables.AdminInvoiceSetting.ShowPOSGrandTotal == true ? false : true);
            r.SetParameterValue("ShowPOSSubTotal", SharedVariables.AdminInvoiceSetting.ShowPOSSubTotal == true ? false : true);
            r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
            r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
            r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
            r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
            r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
            r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);
            r.SetParameterValue("ShowPatientAddress", SharedVariables.AdminInvoiceSetting.ShowAddress);
            r.SetParameterValue("ShowEmail", SharedVariables.AdminInvoiceSetting.ShowEmail);

            HeaderContentSection += 1;
            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
            r.SetParameterValue("Header1", "Invoice#:");
            r.SetParameterValue("Content1", i.InvoiceRefNo.ToString());

            HeaderContentSection += 1;
            r.SetParameterValue("CreateDate", i.CreateDate);
            r.SetParameterValue("Header2", "Date:");
            r.SetParameterValue("Content2", i.CreateDate.ToString("yyyy-MM-dd hh:ss"));

            //---------------Assign Rigion Patient Details -----------------//
            r.SetParameterValue("Header3", "");
            r.SetParameterValue("Content3", "");
            r.SetParameterValue("Header4", "");
            r.SetParameterValue("Content4", "");
            r.SetParameterValue("Header5", "");
            r.SetParameterValue("Content5", "");
            r.SetParameterValue("Header6", "");
            r.SetParameterValue("Content6", "");
            r.SetParameterValue("Header7", "");
            r.SetParameterValue("Content7", "");
            r.SetParameterValue("Header8", "");
            r.SetParameterValue("Content8", "");
            r.SetParameterValue("Header9", "");
            r.SetParameterValue("Content9", "");
            r.SetParameterValue("Header10", "");
            r.SetParameterValue("Content10", "");



            if (!string.IsNullOrEmpty(i.ObjPatient.Name))
            {
                HeaderContentSection += 1;
                r.SetParameterValue("Header" + HeaderContentSection, "Name:");
                r.SetParameterValue("Content" + HeaderContentSection, i.ObjPatient.Name == null ? "" : i.ObjPatient.Name);
            }
            if (SharedVariables.AdminInvoiceSetting.ShowPhone && !string.IsNullOrEmpty(i.ObjPatient.Phone))
            {
                HeaderContentSection += 1;
                r.SetParameterValue("Header" + HeaderContentSection, "Phone:");
                r.SetParameterValue("Content" + HeaderContentSection, i.ObjPatient.Phone == null ? "" : i.ObjPatient.Phone);
            }
            if (SharedVariables.AdminInvoiceSetting.ShowMR && !string.IsNullOrEmpty(i.ObjPatient.MRNo.ToString()))
            {
                HeaderContentSection += 1;
                r.SetParameterValue("Header" + HeaderContentSection, "MR#:");
                r.SetParameterValue("Content" + HeaderContentSection, i.ObjPatient.MRNo == null ? "" : i.ObjPatient.MRNo.Value.ToString());
            }
            if (SharedVariables.AdminInvoiceSetting.ShowGender && !string.IsNullOrEmpty(i.ObjPatient.Gender))
            {
                HeaderContentSection += 1;
                r.SetParameterValue("Header" + HeaderContentSection, "Gender:");
                r.SetParameterValue("Content" + HeaderContentSection, i.ObjPatient.Gender == null ? "" : i.ObjPatient.Gender);
            }
            if (SharedVariables.AdminInvoiceSetting.ShowDOB && !string.IsNullOrEmpty(i.ObjPatient.DateOfBirth.ToString()))
            {
                HeaderContentSection += 1;
                r.SetParameterValue("Header" + HeaderContentSection, "DOB:");
                r.SetParameterValue("Content" + HeaderContentSection, i.ObjPatient.DateOfBirth == null ? "" : i.ObjPatient.DateOfBirth.ToString());
            }
            if (SharedVariables.AdminInvoiceSetting.ShowAge && !string.IsNullOrEmpty(i.ObjPatient.AgeYears.ToString()))
            {
                HeaderContentSection += 1;
                r.SetParameterValue("Header" + HeaderContentSection, "Age:");
                r.SetParameterValue("Content" + HeaderContentSection, i.ObjPatient.AgeYears.ToString() + " Years");
            }
            if (SharedVariables.AdminInvoiceSetting.ShowEmail && !string.IsNullOrEmpty(i.ObjPatient.Email))
            {
                HeaderContentSection += 1;
                r.SetParameterValue("Header" + HeaderContentSection, "Email:");
                r.SetParameterValue("Content" + HeaderContentSection, i.ObjPatient.Email == null ? "" : i.ObjPatient.Email);
            }
            if (SharedVariables.AdminInvoiceSetting.ShowAddress && !string.IsNullOrEmpty(i.ObjPatient.Address))
            {
                HeaderContentSection += 1;
                r.SetParameterValue("Header" + HeaderContentSection, "Address:");
                r.SetParameterValue("Content" + HeaderContentSection, i.ObjPatient.Address == null ? "" : i.ObjPatient.Address);
            }
            //---------------Assign Rigion Selection -----------------//
            if (HeaderContentSection <= 3)
            {
                r.SetParameterValue("HeaderContentSection", 3);
            }
            else if (HeaderContentSection > 3 && HeaderContentSection <= 6)
            {
                r.SetParameterValue("HeaderContentSection", 6);
            }
            else if (HeaderContentSection > 6 && HeaderContentSection <= 9)
            {
                r.SetParameterValue("HeaderContentSection", 9);
            }
            else if (HeaderContentSection > 9)
            {
                r.SetParameterValue("HeaderContentSection", 10);
            }
            else
            {
                r.SetParameterValue("HeaderContentSection", 3);
            }
            r.SetParameterValue("IsTpOrRate", SharedVariables.AdminInvoiceSetting.ShowTPInsteadOfRate ? "T.P" : "Rate");
            r.SetParameterValue("HideBonusQty", SharedVariables.AdminPharmacySetting.IsBonusQty);
            r.SetParameterValue("ShowFBR", SharedVariables.AdminPharmacySetting.IsFBRUnable);
            r.SetParameterValue("FBRInvoiceNo", i.FBRInvoiceNo);
            r.SetParameterValue("ShowReturnQty", i.InvoiceItems.Any(x => x.ReturnedQuantity > 0) == false ? true : false);
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
        private void AssingUnitsalestax(IEnumerable<InvoiceItemVM> items)
        {
            AdminInvoiceSetting setting = new AdminInvoiceSetting();
            foreach (var item in items)
            {


                string Type = item.SaleTaxType == 0 ? "%" : "";
                
                    item.SaleTax = Convert.ToDouble(item.SaleTax) > 0 ? item.SaleTax + Type : "0";

                
            }
        }
        private void AssignTotalAmountToAmount(IEnumerable<InvoiceItemVM> items)
        {
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

                item.Amount = item.NetAmount;
            }


        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        #region CommentedCode
        //private void DefaultDetailedPrint(InvoiceVM i)
        //{
        //    POSInvoiceA4_Detailed_Portrait r = new POSInvoiceA4_Detailed_Portrait();
        //    //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
        //    foreach (InvoicePaymentVM v in i.InvoicePayments)
        //    {
        //        if (v.PaymentType == 1) { v.PaymentTypeString = "Cash"; }
        //        else if (v.PaymentType == 2) { v.PaymentTypeString = "Cheque"; }
        //        else if (v.PaymentType == 3) { v.PaymentTypeString = "Debit/Credit Card"; }
        //        else if (v.PaymentType == 4) { v.PaymentTypeString = "Online Payment"; }
        //    }
        //    r.Database.Tables[1].SetDataSource(i.InvoiceItems);
        //    r.Database.Tables[2].SetDataSource(i.InvoicePayments);
        //    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
        //    {
        //        r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
        //    }           
        //    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
        //    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
        //    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);          
        //    r.SetParameterValue("InvoiceId", i.InvoiceId.ToString("D6"));
        //    r.SetParameterValue("CreateDate", i.CreateDate);
        //    if (i.ObjPatient == null)
        //    {
        //        r.SetParameterValue("PatientName", "");
        //        r.SetParameterValue("MrNo", "");
        //        r.SetParameterValue("PatientPhone", "");
        //        r.SetParameterValue("Gender", "");
        //        r.SetParameterValue("DateOfBirth", "");
        //        r.SetParameterValue("Age", "");
        //    }
        //    else
        //    {
        //        r.SetParameterValue("PatientName", i.ObjPatient.Name);
        //        r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
        //        r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
        //        r.SetParameterValue("Gender", i.ObjPatient.Gender == 1 ? "Male" : "Female");
        //        r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
        //        r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
        //    }

        //    r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
        //    r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
        //    //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
        //    r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
        //    double AdvanceAmount = i.TotalPaid - i.GrandTotal;
        //    double DueAmount = i.GrandTotal - i.TotalPaid;
        //    r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
        //    r.SetParameterValue("DueAmountNumVal", DueAmount);
        //    r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
        //    r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
        //    crystalReportViewer1.ReportSource = r;
        //    crystalReportViewer1.Show();
        //}

        //private void DefaultGeneralPrint(InvoiceVM i, string Description)
        //{
        //    POSInvoiceA4_Portrait r = new POSInvoiceA4_Portrait();

        //    //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
        //    //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
        //    //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
        //    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
        //    {
        //        r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
        //    }

        //    //r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
        //    //r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
        //    //r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 

        //    r.SetParameterValue("InvoiceId", i.InvoiceId.ToString("D6"));
        //    r.SetParameterValue("CreateDate", i.CreateDate);
        //    if (i.ObjPatient == null)
        //    {
        //        r.SetParameterValue("PatientName", "");
        //        r.SetParameterValue("MrNo", "");
        //        r.SetParameterValue("PatientPhone", "");
        //        r.SetParameterValue("Gender", "");
        //        r.SetParameterValue("DateOfBirth", "");
        //        r.SetParameterValue("Age", "");
        //    }
        //    else
        //    {
        //        r.SetParameterValue("PatientName", i.ObjPatient.Name);
        //        r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
        //        r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
        //        r.SetParameterValue("Gender", i.ObjPatient.Gender == 1 ? "Male" : "Female");
        //        r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
        //        r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
        //    }

        //    r.SetParameterValue("Description", Description);
        //    string PaymentMode = "";
        //    if (i.PaymentType == 1) { PaymentMode = "Cash"; }
        //    else if (i.PaymentType == 2) { PaymentMode = "Cheque"; }
        //    else if (i.PaymentType == 3) { PaymentMode = "Debit/Credit Card"; }
        //    else if (i.PaymentType == 4) { PaymentMode = "Online Payment"; }

        //    r.SetParameterValue("PaymentMode", PaymentMode);

        //    //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
        //    //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
        //    //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
        //    r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(i.TotalPaid));
        //    double AdvanceAmount = i.TotalPaid - i.GrandTotal;
        //    double DueAmount = i.GrandTotal - i.TotalPaid;
        //    r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
        //    r.SetParameterValue("DueAmountNumVal", DueAmount);
        //    r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
        //    r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
        //    crystalReportViewer1.ReportSource = r;
        //    crystalReportViewer1.Show();
        //}
        #endregion
    }
}
                    #endregion