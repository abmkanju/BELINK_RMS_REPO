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

namespace Pharmacy.UI.Reports.ProcedureInvoices
{
    public partial class ProcedureInvoiceViewer : Form
    {
        UnitOfWork unitOfWork;
        long InvoiceId = 0;
        bool IsDetailedPrint = false;
        public ProcedureInvoiceViewer()
        {
            InitializeComponent();
        }

        public ProcedureInvoiceViewer(long InvoiceId, bool IsDetailedPrint)
        {
            InitializeComponent();
            this.InvoiceId = InvoiceId;
            this.IsDetailedPrint = IsDetailedPrint;
        }

        //private void DefaultDetaildPrint(InvoiceVM Invoice)
        //{
        //    Reports.ProcedureInvoices.ProcedureInvoiceA4_Detailed_Portrait r = new ProcedureInvoiceA4_Detailed_Portrait();
        //    r.Database.Tables[1].SetDataSource(Invoice.InvoiceItems);
        //    foreach (InvoicePaymentVM p in Invoice.InvoicePayments)
        //    {
        //        if (p.PaymentType == 1) { p.PaymentTypeString = "Cash"; }
        //        else if (p.PaymentType == 2) { p.PaymentTypeString = "Cheque"; }
        //        else if (p.PaymentType == 3) { p.PaymentTypeString = "Debit/Credit Card"; }
        //        else if (p.PaymentType == 4) { p.PaymentTypeString = "Online Payment"; }
        //    }
        //    r.Database.Tables[2].SetDataSource(Invoice.InvoicePayments);
        //    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
        //    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
        //    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
        //    r.SetParameterValue("InvoiceId", Invoice.InvoiceId.ToString("D6"));
        //    r.SetParameterValue("CreateDate", Invoice.CreateDate);
        //    if (Invoice.ObjPatient == null)
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
        //        r.SetParameterValue("PatientName", Invoice.ObjPatient.Name);
        //        r.SetParameterValue("MrNo", Invoice.ObjPatient.MRNo);
        //        r.SetParameterValue("PatientPhone", Invoice.ObjPatient.Phone);
        //        r.SetParameterValue("Gender", Invoice.ObjPatient.Gender == 1 ? "Male" : "Female");
        //        r.SetParameterValue("DateOfBirth", Invoice.ObjPatient.DateOfBirth == null ? "" : Invoice.Patient.DateOfBirth.ToString());
        //        r.SetParameterValue("Age", Invoice.ObjPatient.AgeYears.ToString() + " Years");
        //    }


        //    r.SetParameterValue("SubTotal", "Rs. " + Math.Round(Invoice.SubTotal).ToString());
        //    r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(Invoice.GrandTotal).ToString() + "/-");
        //    //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
        //    r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(Invoice.TotalPaid));
        //    double AdvanceAmount = Invoice.TotalPaid - Invoice.GrandTotal;
        //    double DueAmount = Invoice.GrandTotal - Invoice.TotalPaid;
        //    r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
        //    r.SetParameterValue("DueAmountNumVal", DueAmount);
        //    r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
        //    r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
        //    crystalReportViewer1.ReportSource = r;
        //    crystalReportViewer1.Show();   
        //}
        //private void DefaultGeneralPrint(InvoiceVM Invoice, string Description)
        //{
        //    Reports.ProcedureInvoices.ProcedureInvoiceA4_Portrait r = new ProcedureInvoiceA4_Portrait();
        //    r.SetParameterValue("InvoiceId", Invoice.InvoiceId.ToString("D6"));
        //    r.SetParameterValue("CreateDate", Invoice.CreateDate);

        //    r.SetParameterValue("PatientName", string.IsNullOrEmpty(Invoice.Patient.Name) ? "" : Invoice.Patient.Name);
        //    r.SetParameterValue("MrNo", Invoice.Patient.MRNo.ToString());
        //    r.SetParameterValue("PatientPhone", string.IsNullOrEmpty(Invoice.Patient.Phone) ? "" : Invoice.Patient.Phone.ToString());
        //    r.SetParameterValue("Gender", Invoice.Patient.Gender == 1 ? "Male" : "Female");
        //    r.SetParameterValue("DateOfBirth", Invoice.Patient.DateOfBirth == null ? "" : Invoice.Patient.DateOfBirth.ToString());
        //    r.SetParameterValue("Age", Invoice.Patient.AgeYears.ToString() + " Years");

        //    r.SetParameterValue("Description", Description);
        //    string PaymentMode = "";
        //    if (Invoice.PaymentType == 1) PaymentMode = "Cash";
        //    else if (Invoice.PaymentType == 2) PaymentMode = "Cheque";
        //    else if (Invoice.PaymentType == 3) PaymentMode = "Debot/Credit Card";
        //    else if (Invoice.PaymentType == 4) PaymentMode = "Online Payment";
        //    r.SetParameterValue("PaymentMode", PaymentMode);
        //    //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
        //    //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
        //    //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
        //    r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(Invoice.TotalPaid));
        //    double AdvanceAmount = Invoice.TotalPaid - Invoice.GrandTotal;
        //    double DueAmount = Invoice.GrandTotal - Invoice.TotalPaid;
        //    r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
        //    r.SetParameterValue("DueAmountNumVal", DueAmount);
        //    r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
        //    r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
        //    crystalReportViewer1.ReportSource = r;
        //    crystalReportViewer1.Show();
        //}
        private void ProcedureInvoiceViewer_Load(object sender, EventArgs e)
        {
            if (IsDetailedPrint)
            {
                InvoiceVM Invoice = new InvoiceVM();
                using (unitOfWork = new UnitOfWork())
                {
                    Invoice = unitOfWork.InvoiceRepository.GetProcedureInvoiceDetailById(this.InvoiceId);
                }
                //if(SharedVariables.AdminInvoiceSetting == null)
                //{
                //    DefaultDetaildPrint(Invoice);
                //    return;
                //}

                if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                {
                    if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                    {
                        //i.InvoicePayments.ToList().RemoveAt(0);
                        Reports.ProcedureInvoices.ProcedureInvoiceA4_Detailed_Portrait r = new ProcedureInvoiceA4_Detailed_Portrait();
                        r.Database.Tables[1].SetDataSource(Invoice.InvoiceItems);
                        foreach (InvoicePaymentVM p in Invoice.InvoicePayments)
                        {
                            if (p.PaymentType == 1) { p.PaymentTypeString = "Cash"; }
                            else if (p.PaymentType == 2) { p.PaymentTypeString = "Cheque"; }
                            else if (p.PaymentType == 3) { p.PaymentTypeString = "Debit/Credit Card"; }
                            else if (p.PaymentType == 4) { p.PaymentTypeString = "Online Payment"; }
                        }
                        r.Database.Tables[2].SetDataSource(Invoice.InvoicePayments);

                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("InvoiceId", Invoice.InvoiceId.ToString("D6"));
                        r.SetParameterValue("CreateDate", Invoice.CreateDate);
                        if (Invoice.ObjPatient == null)
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
                            r.SetParameterValue("PatientName", Invoice.ObjPatient.Name);
                            r.SetParameterValue("MrNo", Invoice.ObjPatient.MRNo);
                            r.SetParameterValue("PatientPhone", Invoice.ObjPatient.Phone);
                            r.SetParameterValue("Gender", Invoice.ObjPatient.Gender);
                            r.SetParameterValue("DateOfBirth", Invoice.ObjPatient.DateOfBirth == null ? "" : Invoice.Patient.DateOfBirth.ToString());
                            r.SetParameterValue("Age", Invoice.ObjPatient.AgeYears.ToString() + " Years");
                        }


                        r.SetParameterValue("SubTotal", "Rs. " + Math.Round(Invoice.SubTotal).ToString());
                        r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(Invoice.GrandTotal).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(Invoice.TotalPaid));
                        double AdvanceAmount = Invoice.TotalPaid - Invoice.GrandTotal;
                        double DueAmount = Invoice.GrandTotal - Invoice.TotalPaid;
                        r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                        r.SetParameterValue("DueAmountNumVal", DueAmount);
                        r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                        r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                        crystalReportViewer1.ReportSource = r;
                        crystalReportViewer1.Show();                       
                    }
                    else if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                    {
                        Reports.ProcedureInvoices.ProcedureInvoiceA4_Detailed_Landscape r = new ProcedureInvoiceA4_Detailed_Landscape();
                        r.Database.Tables[1].SetDataSource(Invoice.InvoiceItems);
                        foreach (InvoicePaymentVM p in Invoice.InvoicePayments)
                        {
                            if (p.PaymentType == 1) { p.PaymentTypeString = "Cash"; }
                            else if (p.PaymentType == 2) { p.PaymentTypeString = "Cheque"; }
                            else if (p.PaymentType == 3) { p.PaymentTypeString = "Debit/Credit Card"; }
                            else if (p.PaymentType == 4) { p.PaymentTypeString = "Online Payment"; }
                        }
                        r.Database.Tables[2].SetDataSource(Invoice.InvoicePayments);

                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("InvoiceId", Invoice.InvoiceId.ToString("D6"));
                        r.SetParameterValue("CreateDate", Invoice.CreateDate);
                        if (Invoice.ObjPatient == null)
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
                            r.SetParameterValue("PatientName", Invoice.ObjPatient.Name);
                            r.SetParameterValue("MrNo", Invoice.ObjPatient.MRNo);
                            r.SetParameterValue("PatientPhone", Invoice.ObjPatient.Phone);
                            r.SetParameterValue("Gender", Invoice.ObjPatient.Gender);
                            r.SetParameterValue("DateOfBirth", Invoice.ObjPatient.DateOfBirth == null ? "" : Invoice.Patient.DateOfBirth.ToString());
                            r.SetParameterValue("Age", Invoice.ObjPatient.AgeYears.ToString() + " Years");
                        }


                        r.SetParameterValue("SubTotal", "Rs. " + Math.Round(Invoice.SubTotal).ToString());
                        r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(Invoice.GrandTotal).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(Invoice.TotalPaid));
                        double AdvanceAmount = Invoice.TotalPaid - Invoice.GrandTotal;
                        double DueAmount = Invoice.GrandTotal - Invoice.TotalPaid;
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
                        Reports.ProcedureInvoices.ProcedureInvoiceA5_Detailed_Portrait r = new ProcedureInvoiceA5_Detailed_Portrait();
                        r.Database.Tables[1].SetDataSource(Invoice.InvoiceItems);
                        foreach (InvoicePaymentVM p in Invoice.InvoicePayments)
                        {
                            if (p.PaymentType == 1) { p.PaymentTypeString = "Cash"; }
                            else if (p.PaymentType == 2) { p.PaymentTypeString = "Cheque"; }
                            else if (p.PaymentType == 3) { p.PaymentTypeString = "Debit/Credit Card"; }
                            else if (p.PaymentType == 4) { p.PaymentTypeString = "Online Payment"; }
                        }
                        r.Database.Tables[2].SetDataSource(Invoice.InvoicePayments);

                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("InvoiceId", Invoice.InvoiceId.ToString("D6"));
                        r.SetParameterValue("CreateDate", Invoice.CreateDate);
                        if (Invoice.ObjPatient == null)
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
                            r.SetParameterValue("PatientName", Invoice.ObjPatient.Name);
                            r.SetParameterValue("MrNo", Invoice.ObjPatient.MRNo);
                            r.SetParameterValue("PatientPhone", Invoice.ObjPatient.Phone);
                            r.SetParameterValue("Gender", Invoice.ObjPatient.Gender);
                            r.SetParameterValue("DateOfBirth", Invoice.ObjPatient.DateOfBirth == null ? "" : Invoice.Patient.DateOfBirth.ToString());
                            r.SetParameterValue("Age", Invoice.ObjPatient.AgeYears.ToString() + " Years");
                        }


                        r.SetParameterValue("SubTotal", "Rs. " + Math.Round(Invoice.SubTotal).ToString());
                        r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(Invoice.GrandTotal).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(Invoice.TotalPaid));
                        double AdvanceAmount = Invoice.TotalPaid - Invoice.GrandTotal;
                        double DueAmount = Invoice.GrandTotal - Invoice.TotalPaid;
                        r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                        r.SetParameterValue("DueAmountNumVal", DueAmount);
                        r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                        r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                        crystalReportViewer1.ReportSource = r;
                        crystalReportViewer1.Show();          
                    }
                    else if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                    {
                        Reports.ProcedureInvoices.ProcedureInvoiceA5_Detailed_Landscape r = new ProcedureInvoiceA5_Detailed_Landscape();
                        r.Database.Tables[1].SetDataSource(Invoice.InvoiceItems);
                        foreach (InvoicePaymentVM p in Invoice.InvoicePayments)
                        {
                            if (p.PaymentType == 1) { p.PaymentTypeString = "Cash"; }
                            else if (p.PaymentType == 2) { p.PaymentTypeString = "Cheque"; }
                            else if (p.PaymentType == 3) { p.PaymentTypeString = "Debit/Credit Card"; }
                            else if (p.PaymentType == 4) { p.PaymentTypeString = "Online Payment"; }
                        }
                        r.Database.Tables[2].SetDataSource(Invoice.InvoicePayments);

                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("InvoiceId", Invoice.InvoiceId.ToString("D6"));
                        r.SetParameterValue("CreateDate", Invoice.CreateDate);
                        if (Invoice.ObjPatient == null)
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
                            r.SetParameterValue("PatientName", Invoice.ObjPatient.Name);
                            r.SetParameterValue("MrNo", Invoice.ObjPatient.MRNo);
                            r.SetParameterValue("PatientPhone", Invoice.ObjPatient.Phone);
                            r.SetParameterValue("Gender", Invoice.ObjPatient.Gender);
                            r.SetParameterValue("DateOfBirth", Invoice.ObjPatient.DateOfBirth == null ? "" : Invoice.Patient.DateOfBirth.ToString());
                            r.SetParameterValue("Age", Invoice.ObjPatient.AgeYears.ToString() + " Years");
                        }


                        r.SetParameterValue("SubTotal", "Rs. " + Math.Round(Invoice.SubTotal).ToString());
                        r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(Invoice.GrandTotal).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(Invoice.TotalPaid));
                        double AdvanceAmount = Invoice.TotalPaid - Invoice.GrandTotal;
                        double DueAmount = Invoice.GrandTotal - Invoice.TotalPaid;
                        r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                        r.SetParameterValue("DueAmountNumVal", DueAmount);
                        r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                        r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                        crystalReportViewer1.ReportSource = r;
                        crystalReportViewer1.Show();          
                    }
                }
                else if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A8)
                {
                    Reports.ProcedureInvoices.ProcedureInvoiceA8_Detailed r = new ProcedureInvoiceA8_Detailed();
                    r.Database.Tables[1].SetDataSource(Invoice.InvoiceItems);
                    foreach (InvoicePaymentVM p in Invoice.InvoicePayments)
                    {
                        if (p.PaymentType == 1) { p.PaymentTypeString = "Cash"; }
                        else if (p.PaymentType == 2) { p.PaymentTypeString = "Cheque"; }
                        else if (p.PaymentType == 3) { p.PaymentTypeString = "Debit/Credit Card"; }
                        else if (p.PaymentType == 4) { p.PaymentTypeString = "Online Payment"; }
                    }
                    r.Database.Tables[2].SetDataSource(Invoice.InvoicePayments);

                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                    r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                    r.SetParameterValue("InvoiceId", Invoice.InvoiceId.ToString("D6"));
                    r.SetParameterValue("CreateDate", Invoice.CreateDate);
                    if (Invoice.ObjPatient == null)
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
                        r.SetParameterValue("PatientName", Invoice.ObjPatient.Name);
                        r.SetParameterValue("MrNo", Invoice.ObjPatient.MRNo);
                        r.SetParameterValue("PatientPhone", Invoice.ObjPatient.Phone);
                        r.SetParameterValue("Gender", Invoice.ObjPatient.Gender);
                        r.SetParameterValue("DateOfBirth", Invoice.ObjPatient.DateOfBirth == null ? "" : Invoice.Patient.DateOfBirth.ToString());
                        r.SetParameterValue("Age", Invoice.ObjPatient.AgeYears.ToString() + " Years");
                    }

                    r.SetParameterValue("SubTotal", "Rs. " + Math.Round(Invoice.SubTotal).ToString());
                    r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(Invoice.GrandTotal).ToString() + "/-");
                    //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                    r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(Invoice.TotalPaid));
                    double AdvanceAmount = Invoice.TotalPaid - Invoice.GrandTotal;
                    double DueAmount = Invoice.GrandTotal - Invoice.TotalPaid;
                    r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                    r.SetParameterValue("DueAmountNumVal", DueAmount);
                    r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                    r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();    
                }
            }
            else
            {
                InvoiceVM Invoice;
                using (unitOfWork = new UnitOfWork())
                {
                    Invoice = unitOfWork.InvoiceRepository.GetProc_InvoiceDetailsById(InvoiceId);
                }
                string Description = "";
                foreach (InvoiceItemVM i in Invoice.InvoiceItems)
                {
                    Description += i.ProcedureName + ", ";
                }
                Description = Description.Trim().TrimEnd(',');
                //if(SharedVariables.AdminInvoiceSetting == null)
                //{
                //    DefaultGeneralPrint(Invoice, Description);
                //    return;
                //}
                if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                {
                    if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                    {
                        Reports.ProcedureInvoices.ProcedureInvoiceA4_Portrait r = new ProcedureInvoiceA4_Portrait();
                        r.SetParameterValue("InvoiceId", Invoice.InvoiceId.ToString("D6"));
                        r.SetParameterValue("CreateDate", Invoice.CreateDate);

                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("PatientName", string.IsNullOrEmpty(Invoice.Patient.Name) ? "" : Invoice.Patient.Name);
                        r.SetParameterValue("MrNo", Invoice.Patient.MRNo.ToString());
                        r.SetParameterValue("PatientPhone", string.IsNullOrEmpty(Invoice.Patient.Phone) ? "" : Invoice.Patient.Phone.ToString());
                        r.SetParameterValue("Gender", Invoice.Patient.Gender);
                        r.SetParameterValue("DateOfBirth", Invoice.Patient.DateOfBirth == null ? "" : Invoice.Patient.DateOfBirth.ToString());
                        r.SetParameterValue("Age", Invoice.Patient.AgeYears.ToString() + " Years");

                        r.SetParameterValue("Description", Description);
                        string PaymentMode = "";
                        if (Invoice.PaymentType == 1) PaymentMode = "Cash";
                        else if (Invoice.PaymentType == 2) PaymentMode = "Cheque";
                        else if (Invoice.PaymentType == 3) PaymentMode = "Debot/Credit Card";
                        else if (Invoice.PaymentType == 4) PaymentMode = "Online Payment";
                        r.SetParameterValue("PaymentMode", PaymentMode);
                        //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                        //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(Invoice.TotalPaid));
                        double AdvanceAmount = Invoice.TotalPaid - Invoice.GrandTotal;
                        double DueAmount = Invoice.GrandTotal - Invoice.TotalPaid;
                        r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                        r.SetParameterValue("DueAmountNumVal", DueAmount);
                        r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                        r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                        crystalReportViewer1.ReportSource = r;
                        crystalReportViewer1.Show();
                    }
                    else if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                    {
                        Reports.ProcedureInvoices.ProcedureInvoiceA4_Landscape r = new ProcedureInvoiceA4_Landscape();
                        r.SetParameterValue("InvoiceId", Invoice.InvoiceId.ToString("D6"));
                        r.SetParameterValue("CreateDate", Invoice.CreateDate);

                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("PatientName", string.IsNullOrEmpty(Invoice.Patient.Name) ? "" : Invoice.Patient.Name);
                        r.SetParameterValue("MrNo", Invoice.Patient.MRNo.ToString());
                        r.SetParameterValue("PatientPhone", string.IsNullOrEmpty(Invoice.Patient.Phone) ? "" : Invoice.Patient.Phone.ToString());
                        r.SetParameterValue("Gender", Invoice.Patient.Gender);
                        r.SetParameterValue("DateOfBirth", Invoice.Patient.DateOfBirth == null ? "" : Invoice.Patient.DateOfBirth.ToString());
                        r.SetParameterValue("Age", Invoice.Patient.AgeYears.ToString() + " Years");

                        r.SetParameterValue("Description", Description);
                        string PaymentMode = "";
                        if (Invoice.PaymentType == 1) PaymentMode = "Cash";
                        else if (Invoice.PaymentType == 2) PaymentMode = "Cheque";
                        else if (Invoice.PaymentType == 3) PaymentMode = "Debot/Credit Card";
                        else if (Invoice.PaymentType == 4) PaymentMode = "Online Payment";
                        r.SetParameterValue("PaymentMode", PaymentMode);
                        //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                        //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(Invoice.TotalPaid));
                        double AdvanceAmount = Invoice.TotalPaid - Invoice.GrandTotal;
                        double DueAmount = Invoice.GrandTotal - Invoice.TotalPaid;
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
                        Reports.ProcedureInvoices.ProcedureInvoiceA5_Portrait r = new ProcedureInvoiceA5_Portrait();
                        r.SetParameterValue("InvoiceId", Invoice.InvoiceId.ToString("D6"));
                        r.SetParameterValue("CreateDate", Invoice.CreateDate);

                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("PatientName", string.IsNullOrEmpty(Invoice.Patient.Name) ? "" : Invoice.Patient.Name);
                        r.SetParameterValue("MrNo", Invoice.Patient.MRNo.ToString());
                        r.SetParameterValue("PatientPhone", string.IsNullOrEmpty(Invoice.Patient.Phone) ? "" : Invoice.Patient.Phone.ToString());
                        r.SetParameterValue("Gender", Invoice.Patient.Gender);
                        r.SetParameterValue("DateOfBirth", Invoice.Patient.DateOfBirth == null ? "" : Invoice.Patient.DateOfBirth.ToString());
                        r.SetParameterValue("Age", Invoice.Patient.AgeYears.ToString() + " Years");

                        r.SetParameterValue("Description", Description);
                        string PaymentMode = "";
                        if (Invoice.PaymentType == 1) PaymentMode = "Cash";
                        else if (Invoice.PaymentType == 2) PaymentMode = "Cheque";
                        else if (Invoice.PaymentType == 3) PaymentMode = "Debot/Credit Card";
                        else if (Invoice.PaymentType == 4) PaymentMode = "Online Payment";
                        r.SetParameterValue("PaymentMode", PaymentMode);
                        //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                        //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(Invoice.TotalPaid));
                        double AdvanceAmount = Invoice.TotalPaid - Invoice.GrandTotal;
                        double DueAmount = Invoice.GrandTotal - Invoice.TotalPaid;
                        r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                        r.SetParameterValue("DueAmountNumVal", DueAmount);
                        r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                        r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                        crystalReportViewer1.ReportSource = r;
                        crystalReportViewer1.Show();
                    }
                    else if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                    {
                        Reports.ProcedureInvoices.ProcedureInvoiceA5_Landscape r = new ProcedureInvoiceA5_Landscape();
                        r.SetParameterValue("InvoiceId", Invoice.InvoiceId.ToString("D6"));
                        r.SetParameterValue("CreateDate", Invoice.CreateDate);

                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("PatientName", string.IsNullOrEmpty(Invoice.Patient.Name) ? "" : Invoice.Patient.Name);
                        r.SetParameterValue("MrNo", Invoice.Patient.MRNo.ToString());
                        r.SetParameterValue("PatientPhone", string.IsNullOrEmpty(Invoice.Patient.Phone) ? "" : Invoice.Patient.Phone.ToString());
                        r.SetParameterValue("Gender", Invoice.Patient.Gender);
                        r.SetParameterValue("DateOfBirth", Invoice.Patient.DateOfBirth == null ? "" : Invoice.Patient.DateOfBirth.ToString());
                        r.SetParameterValue("Age", Invoice.Patient.AgeYears.ToString() + " Years");

                        r.SetParameterValue("Description", Description);
                        string PaymentMode = "";
                        if (Invoice.PaymentType == 1) PaymentMode = "Cash";
                        else if (Invoice.PaymentType == 2) PaymentMode = "Cheque";
                        else if (Invoice.PaymentType == 3) PaymentMode = "Debot/Credit Card";
                        else if (Invoice.PaymentType == 4) PaymentMode = "Online Payment";
                        r.SetParameterValue("PaymentMode", PaymentMode);
                        //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                        //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(Invoice.TotalPaid));
                        double AdvanceAmount = Invoice.TotalPaid - Invoice.GrandTotal;
                        double DueAmount = Invoice.GrandTotal - Invoice.TotalPaid;
                        r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                        r.SetParameterValue("DueAmountNumVal", DueAmount);
                        r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                        r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                        crystalReportViewer1.ReportSource = r;
                        crystalReportViewer1.Show();
                    }
                }
                else if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A8)
                {

                    Reports.ProcedureInvoices.ProcedureInvoiceA8 r = new ProcedureInvoiceA8();
                    r.SetParameterValue("InvoiceId", Invoice.InvoiceId.ToString("D6"));
                    r.SetParameterValue("CreateDate", Invoice.CreateDate);

                    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                    r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                    r.SetParameterValue("PatientName", string.IsNullOrEmpty(Invoice.Patient.Name) ? "" : Invoice.Patient.Name);
                    r.SetParameterValue("MrNo", Invoice.Patient.MRNo.ToString());
                    r.SetParameterValue("PatientPhone", string.IsNullOrEmpty(Invoice.Patient.Phone) ? "" : Invoice.Patient.Phone.ToString());
                    r.SetParameterValue("Gender", Invoice.Patient.Gender);
                    r.SetParameterValue("DateOfBirth", Invoice.Patient.DateOfBirth == null ? "" : Invoice.Patient.DateOfBirth.ToString());
                    r.SetParameterValue("Age", Invoice.Patient.AgeYears.ToString() + " Years");

                    r.SetParameterValue("Description", Description);
                    string PaymentMode = "";
                    if (Invoice.PaymentType == 1) PaymentMode = "Cash";
                    else if (Invoice.PaymentType == 2) PaymentMode = "Cheque";
                    else if (Invoice.PaymentType == 3) PaymentMode = "Debot/Credit Card";
                    else if (Invoice.PaymentType == 4) PaymentMode = "Online Payment";

                    r.SetParameterValue("PaymentMode", PaymentMode);
                    //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                    //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                    //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                    r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(Invoice.TotalPaid));
                    double AdvanceAmount = Invoice.TotalPaid - Invoice.GrandTotal;
                    double DueAmount = Invoice.GrandTotal - Invoice.TotalPaid;
                    r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                    r.SetParameterValue("DueAmountNumVal", DueAmount);
                    r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                    r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }
            }
        }
    }
}