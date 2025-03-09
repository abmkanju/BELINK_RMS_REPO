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
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using PagedList;
using Pharmacy.UI.MenuBar.Pharmacy;

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class DiscountsUI : Form
    {
        enum FilterType { Default = 1, Date = 2, InvNo = 3, Patient = 4 };
        FilterType filter = FilterType.Default;
        UnitOfWork unitOfWork;
        IPagedList<DiscountVM> Discounts;
        int PageNo = 1;
        public DiscountsUI()
        {
            InitializeComponent();
        }

        private void DicountsUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint, btnRefreshForm });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                loadPatients();
                LoadDiscounts();
                SharedFunctions.SetGridStyle(grdData);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                using (new CursorWait())
                {
                    dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                    dtpTo.ValueChanged -= dtpTo_ValueChanged;
                    dtpFrom.Value = DateTime.Now;
                    dtpTo.Value = DateTime.Now;
                    txtInvNo.Text = "";
                    txtDiscount.Text = "";
                    chk100Per.Checked = false;
                    filter = FilterType.Default;
                    loadPatients();
                    LoadDiscounts();
                    dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                    dtpTo.ValueChanged += dtpTo_ValueChanged;
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void loadPatients()
        {
            try
            {
                cmbPatient.SelectedIndexChanged -= cmbPatient_SelectedIndexChanged;
                List<SelectListVM> lst = new List<SelectListVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    lst = unitOfWork.PatientRepository.GetSelectList();
                }
                lst.Insert(0, new SelectListVM { Value = 0, Text = "Select Patient" });
                cmbPatient.DataSource = lst;
                cmbPatient.ValueMember = "Text";
                cmbPatient.ValueMember = "Value";
                cmbPatient.SelectedIndexChanged += cmbPatient_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading patients data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadDiscounts()
        {
            try
            {
                DiscountsMasterVM DiscountsMaster = new DiscountsMasterVM();
                using (unitOfWork = new UnitOfWork())
                {
                    if (filter == FilterType.Date || filter == FilterType.Default)
                    {
                        if (this.chk100Per.Checked)
                        {
                            DiscountsMaster = unitOfWork.InvoiceRepository.GetDiscounts(dtpFrom.Value, dtpTo.Value, chk100Per.Checked, PageNo, SharedVariables.PageSize);
                        }
                        else
                        {
                            DiscountsMaster = unitOfWork.InvoiceRepository.GetDiscounts(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                        }
                    }
                    else if (filter == FilterType.InvNo)
                    {
                        if (txtInvNo.Text != "")
                        {
                            long invNo = 0;
                            Int64.TryParse(txtInvNo.Text.Trim(), out invNo);
                            DiscountsMaster = unitOfWork.InvoiceRepository.GetDiscountByInvNo(invNo, PageNo, SharedVariables.PageSize);
                        }
                    }
                    else if (filter == FilterType.Patient)
                    {
                        DiscountsMaster = unitOfWork.InvoiceRepository.GetDiscountByPatient(Convert.ToInt64(cmbPatient.SelectedValue), PageNo, SharedVariables.PageSize);
                    }
                }


                this.Discounts = DiscountsMaster.Discounts;
                string PaymentDate = "";
                grdData.Rows.Clear();
                string InvoiceItems = "";
                double TotalDiscount = 0;
                double dubDiscount = 0.00;
                double.TryParse(txtDiscount.Text, out dubDiscount);
                int intDiscount = 0;
                if (dubDiscount % 1 == 0)
                {
                    intDiscount = (int)dubDiscount;
                }
                foreach (DiscountVM d in this.Discounts)
                {
                    PaymentDate = "";
                    InvoiceItems = "";
                    //TotalDiscount += d.ModifiedDiscount;
                    if (d.PaymentDate != null)
                    {
                        PaymentDate = d.PaymentDate.ToString();
                    }
                    foreach (InvoiceItemVM i in d.InvoiceItems)
                    {
                        InvoiceItems += i.ItemName + ", ";
                    }
                    InvoiceItems = string.IsNullOrEmpty(InvoiceItems) ? "" : InvoiceItems.Trim().TrimEnd(',');
                    if (dubDiscount <= 0.0 && intDiscount <= 0)
                    {
                        if (d.Patient != null)
                        {
                            grdData.Rows.Add(d.InvoiceId, d.InvoiceRefNo, d.Patient.MRNo, d.Patient.Name, InvoiceItems, Math.Round(d.SubTotal, 2), Math.Round(d.TotalPaid, 2), Math.Round(d.ModifiedDiscount, 2), "", PaymentDate);
                        }
                        else
                        {
                            grdData.Rows.Add(d.InvoiceId, d.InvoiceRefNo, "", "", InvoiceItems, Math.Round(d.SubTotal, 2), Math.Round(d.TotalPaid, 2), Math.Round(d.ModifiedDiscount, 2), "", PaymentDate);
                        }
                    }
                    else if (intDiscount > 0)
                    {
                        if (intDiscount == (int)d.ModifiedDiscount)
                        {
                            if (d.Patient != null)
                            {
                                grdData.Rows.Add(d.InvoiceId, d.InvoiceRefNo, d.Patient.MRNo, d.Patient.Name, InvoiceItems, Math.Round(d.SubTotal, 2), Math.Round(d.TotalPaid, 2), Math.Round(d.ModifiedDiscount, 2), "", PaymentDate);
                            }
                            else
                            {
                                grdData.Rows.Add(d.InvoiceId, d.InvoiceRefNo, "", "", InvoiceItems, Math.Round(d.SubTotal, 2), Math.Round(d.TotalPaid, 2), Math.Round(d.ModifiedDiscount, 2), "", PaymentDate);
                            }
                        }
                    }
                    else if (dubDiscount > 0.00)
                    {
                        if (dubDiscount == Math.Round(d.ModifiedDiscount, 2))
                        {
                            if (d.Patient != null)
                            {
                                grdData.Rows.Add(d.InvoiceId, d.InvoiceRefNo, d.Patient.MRNo, d.Patient.Name, InvoiceItems, Math.Round(d.SubTotal, 2), Math.Round(d.TotalPaid, 2), Math.Round(d.ModifiedDiscount, 2), "", PaymentDate);
                            }
                            else
                            {
                                grdData.Rows.Add(d.InvoiceId, d.InvoiceRefNo, "", "", InvoiceItems, Math.Round(d.SubTotal, 2), Math.Round(d.TotalPaid, 2), Math.Round(d.ModifiedDiscount, 2), "", PaymentDate);
                            }
                        }
                    }
                }
                if (PageNo == 1)
                {
                    lblTotalDiscount.Text = Math.Round(DiscountsMaster.TotalDiscount).ToString();
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = Discounts.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = Discounts.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, Discounts.PageCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<DiscountVM> GetReportData()
        {
            List<DiscountVM> DiscountsList;
            using (unitOfWork = new UnitOfWork())
            {
                if (this.chk100Per.Checked)
                {
                    DiscountsList = unitOfWork.InvoiceRepository.GetDiscounts_Rpt(chk100Per.Checked, dtpFrom.Value, dtpTo.Value);
                }
                else
                {
                    DiscountsList = unitOfWork.InvoiceRepository.GetDiscounts_Rpt(dtpFrom.Value, dtpTo.Value);
                }
            }

            string PaymentDate = "";
            string InvoiceItems = "";
            foreach (DiscountVM d in DiscountsList)
            {
                PaymentDate = "";
                InvoiceItems = "";
                if (d.PaymentDate != null)
                {
                    PaymentDate = d.PaymentDate.ToString();
                }
                foreach (InvoiceItemVM i in d.InvoiceItems)
                {
                    InvoiceItems += i.ItemName + ", ";
                }
                d.Description = string.IsNullOrEmpty(InvoiceItems) ? "" : InvoiceItems.Trim().TrimEnd(',');
                if (d.Patient != null)
                {
                    d.PatientName = d.Patient.Name;
                    d.MrNoString = d.Patient.MRNo.ToString();
                    //grdData.Rows.Add(d.InvoiceId, d.Patient.MRNo, d.Patient.Name, InvoiceItems, d.SubTotal, d.TotalPaid, d.ModifiedDiscount, "", PaymentDate);
                }
                else
                {
                    d.PatientName = "";
                    d.MrNoString = "";
                    //grdData.Rows.Add(d.InvoiceId, "", "", InvoiceItems, d.SubTotal, d.TotalPaid, d.ModifiedDiscount, "", PaymentDate);
                }
            }
            return DiscountsList;
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            filter = FilterType.Date;
            LoadDiscounts();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            filter = FilterType.Date;
            LoadDiscounts();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadDiscounts();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = Discounts.PageCount;
            LoadDiscounts();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadDiscounts();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDiscounts();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            List<DiscountVM> DiscountsList = GetReportData();
            //if (d.IsPrint)
            //{
            //    if(chk100Per.Checked)
            //    {
            DiscountsViewer v = new DiscountsViewer(0, this.chk100Per.Checked, this.dtpFrom.Value, this.dtpTo.Value, DiscountsList);
            v.Show();
            //    }
            //    else
            //    { 
            //        DiscountsViewer v = new DiscountsViewer(d.Orientation, this.dtpFrom.Value, this.dtpTo.Value, DiscountsList);
            //        v.Show();
            //    }
            //}
            //else if (d.IsSaveAsPdf)
            //{
            //    SavePdf(d.Orientation, DiscountsList);
            //}
            //}
        }

        private void SavePdf(int Orientation, List<DiscountVM> DiscountsList)
        {
            try
            {
                if (Orientation == 0)
                {
                    Reports.Pharmacy.DiscountsRpt_Portrait r = new DiscountsRpt_Portrait();
                    r.SetDataSource(DiscountsList);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    r.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    r.SetParameterValue("DateTo", this.dtpTo.Value);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        r.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Discounts Reports Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    DiscountsRpt_Landscape rpt = new DiscountsRpt_Landscape();
                    rpt.SetDataSource(DiscountsList);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Discounts Reports Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<DiscountVM> Result = GetReportData();
                DiscountsRpt_Landscape rpt = new DiscountsRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Result);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Discounts Report Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDiscounts();
        }

        private void chk100Per_CheckedChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDiscounts();
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > Discounts.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadDiscounts();
                }
            }
        }

        private void txtInvNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtInvNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtInvNo.Text == "")
                {
                    filter = FilterType.Default;
                }
                else
                {
                    filter = FilterType.InvNo;
                }
                PageNo = 1;
                LoadDiscounts();
            }
        }

        private void cmbPatient_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageNo = 1;
            if (cmbPatient.SelectedIndex > 0)
            {
                filter = FilterType.Patient;
            }
            else
            {
                filter = FilterType.Default;
            }
            LoadDiscounts();
        }

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtRetailPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void txtDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {

                    PageNo = 1;
                    LoadDiscounts();

                    //if (txtInvNo.Text == "")
                    //{
                    //    filter = FilterType.Default;
                    //}
                    //else
                    //{
                    //    filter = FilterType.InvNo;
                    //}

                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            using (new CursorWait())
            {
                PageNo = 1;
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpTo.ValueChanged -= dtpTo_ValueChanged;
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                txtInvNo.Text = "";
                txtDiscount.Text = "";
                chk100Per.Checked = false;
                filter = FilterType.Default;
                loadPatients();
                LoadDiscounts();
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                dtpTo.ValueChanged += dtpTo_ValueChanged;
            }

        }
    }
}