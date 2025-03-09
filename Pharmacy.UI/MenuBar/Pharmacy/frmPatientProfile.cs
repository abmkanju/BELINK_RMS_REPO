using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Infrastructure;
using Pharmacy.Core;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.UI.Reports;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmPatientProfile : Form
    {
        UnitOfWork unitOfWork;
        int PageNo = 1;
        int PageSize = SharedVariables.PageSize;
        private long PatientId = 0;
        public frmPatientProfile()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        public frmPatientProfile(long PatientId)
        {
            InitializeComponent();
            this.PatientId = PatientId;
            unitOfWork = new UnitOfWork();
        }
        private void frmPatientProfile_Load(object sender, EventArgs e)
        {
            //GetPatientData();
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnPrintInvoiceHistory, btnAddInvoice });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnEditProfile, btnAddTag });
            PatientInvoicesData();
            SharedFunctions.SetGridStyle(grdInvoices);
        }
        private void PatientInvoicesData()
        {
            PatientVM patient = unitOfWork.InvoiceRepository.GetPatienInvoices(this.PatientId, PageNo, PageSize);
            lblPatientName.Text = patient.Name;
            lblMrNo.Text = patient.MRNo.ToString();
            lblHwId.Text = "N/A";
            lblPhone.Text = patient.Phone;
            if (patient.ImagePath != null)
            {
                pbPatientImage.Image = Image.FromFile(patient.ImagePath);
            }
            foreach (Tag objTag in patient.Tags)
            {
                GenerateTag(objTag.TagId, objTag.TagName);
            }
            int count = 0;
            foreach (InvoiceVM i in patient.Invoices)
            {
                string InvoiceItems = "";
                string InvoicePayments = "";
                string PaymentDates = "";
                foreach (InvoiceItemVM ii in i.InvoiceItems)
                {
                    InvoiceItems += ii.ItemName + ", ";
                }
                foreach (InvoicePaymentVM ip in i.InvoicePayments)
                {
                    InvoicePayments += ip.Payment + Environment.NewLine;
                    PaymentDates += ip.PaymentDate.ToShortDateString() + Environment.NewLine;
                }
                InvoiceItems = InvoiceItems.Trim().TrimEnd(',');
                grdInvoices.Rows.Add(++count, i.InvoiceId, i.InvoiceRefNo, InvoiceItems, i.SubTotal, InvoicePayments, i.Due > 0 ? i.Due : 0, i.Due < 0 ? -1 * i.Due : 0, PaymentDates, null, null);
            }
        }
        private void GetPatientData()
        {
            //get patient info
            Patient objPatient = unitOfWork.PatientRepository.GetPatientWithTags_ByPatientId(this.PatientId);
            lblPatientName.Text = objPatient.Name;
            lblMrNo.Text = objPatient.MRNo.ToString();
            lblHwId.Text = "N/A";
            lblPhone.Text = objPatient.Phone;
            if (objPatient.ImagePath != null)
            {
                pbPatientImage.Image = Image.FromFile(objPatient.ImagePath);
            }
            foreach (Tag objTag in objPatient.Tags)
            {
                GenerateTag(objTag.TagId, objTag.TagName);
            }
            //get Last invoice details
            //InvoiceVM objInvoice = unitofWork.InvoiceRepository.GetPatient_LastInvoiceDetail(this.PatientId);
            //if (objInvoice != null)
            //{
            //    foreach (InvoiceItemVM i in objInvoice.InvoiceItems)
            //    {
            //        grdInvoiceItems.Rows.Add(i.ItemName, i.Amount);
            //    }
            //    lblTotalBilledAmount.Text = objInvoice.SubTotal.ToString();
            //    lblAmountPaid.Text = objInvoice.GrandTotal.ToString();
            //}            
            //get treatment plan
        }
        private void GenerateTag(long TagId, string TagName)
        {
            Button btnRemoveTag = new Button();
            btnRemoveTag.BackColor = System.Drawing.Color.White;
            btnRemoveTag.DialogResult = System.Windows.Forms.DialogResult.Yes;
            btnRemoveTag.FlatAppearance.BorderSize = 0;
            btnRemoveTag.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btnRemoveTag.ForeColor = System.Drawing.Color.Red;
            btnRemoveTag.Location = new System.Drawing.Point(157, 5);
            btnRemoveTag.Name = "button1";
            btnRemoveTag.Size = new System.Drawing.Size(25, 23);
            btnRemoveTag.TabIndex = 4;
            btnRemoveTag.Text = "X";
            btnRemoveTag.Tag = TagId;
            btnRemoveTag.UseVisualStyleBackColor = false;
            btnRemoveTag.Click += new EventHandler(RemoveTagBtn_Click);

            Label lblTagName = new Label();
            lblTagName.AutoSize = true;
            lblTagName.Location = new System.Drawing.Point(3, 2);
            lblTagName.MaximumSize = new System.Drawing.Size(150, 0);
            lblTagName.MinimumSize = new System.Drawing.Size(150, 30);
            lblTagName.Name = "lblTag";
            lblTagName.Size = new System.Drawing.Size(150, 30);
            lblTagName.TabIndex = 1;
            lblTagName.Text = TagName;
            lblTagName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            Panel pnlTagData = new Panel();
            pnlTagData.AutoSize = true;
            pnlTagData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlTagData.Controls.Add(btnRemoveTag);
            pnlTagData.Controls.Add(lblTagName);
            pnlTagData.Location = new System.Drawing.Point(3, 3);
            pnlTagData.Name = "pnlTagData";
            pnlTagData.Tag = TagId;
            pnlTagData.Size = new System.Drawing.Size(185, 34);
            pnlTagData.TabIndex = 5;
            pnlAllTags.Controls.Add(pnlTagData);
            pnlAllTags.Controls.SetChildIndex(pnlTagData, 0);
        }
        private void RemoveTagBtn_Click(object sender, EventArgs e)
        {
            long tagId = (long)((Button)sender).Tag;
            DialogResult rs = MessageBox.Show("Are You Sure You Want To Remove This Tag?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (rs == DialogResult.OK)
            {
                try
                {
                    unitOfWork.TagsRepository.RemoveTag(tagId, this.PatientId);
                    foreach (Panel p in pnlAllTags.Controls)
                    {
                        if (((long)p.Tag) == tagId)
                        {
                            pnlAllTags.Controls.Remove(p);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                }
            }
        }
        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            if (this.PatientId > 0)
            {
                Form f = SharedFunctions.OpenForm(new frmAddPatient(this.PatientId), this.MdiParent);
                f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
            }
            else
            {
                MessageBox.Show("No Patient Selected To Edit");
            }
        }

        private void btnAddInvoice_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmInvoice(this.PatientId), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void btnAddTag_Click(object sender, EventArgs e)
        {
            frmAddTag f = new frmAddTag(this.PatientId);
            f.ShowDialog();
            long tagId = f.TagID;
            string tagName = f.TagName;
            if (tagId > 0)
            {
                GenerateTag(tagId, tagName);
            }
        }

        private void lblViewAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmPatientInvoices(this.PatientId), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearForm();
            unitOfWork = new UnitOfWork();
            grdInvoices.Rows.Clear();
            PatientInvoicesData();
        }

        private void ClearForm()
        {
            grdInvoiceItems.Rows.Clear();
            pnlAllTags.Controls.Clear();
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void grdInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }
                long InvoiceId = Convert.ToInt64(grdInvoices.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                if (e.ColumnIndex == grdInvoices.Columns["colEdit"].Index)
                {
                    frmInvoice i = new frmInvoice(true, false, Convert.ToInt64(InvoiceId));
                    i.Show();
                    return;
                }

                if (e.ColumnIndex == grdInvoices.Columns["colRefund"].Index)
                {
                    frmInvoice i = new frmInvoice(false, true, InvoiceId);
                    i.Show();
                    return;
                }
                if (e.ColumnIndex == grdInvoices.Columns["colDelete"].Index)
                {
                    DialogResult rs = MessageBox.Show("Are you Sure You Want To Delete Invoice", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (rs == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
                    Invoice objInvoice = unitOfWork.InvoiceRepository.GetInvoiceById_Inc_Pmnts_cons(InvoiceId);
                    objInvoice.UpdateDate = DateTime.Now;
                    objInvoice.IsActive = false;
                    objInvoice.IsUpdate = true;
                    foreach (InvoiceItem i in objInvoice.InvoiceItems)
                    {
                        i.IsActive = false;
                        i.IsUpdate = true;
                        unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                    }
                    // commecnted while consume stock change
                    foreach (StockConsumptionItem c in objInvoice.StockConsumption.StockConsumptions)
                    {
                        c.IsUpdate = true;
                        c.IsActive = false;
                        c.UpdateDate = DateTime.Now;
                        unitOfWork.GetDbContext().Entry(c).State = System.Data.Entity.EntityState.Modified;
                    }
                    foreach (InvoicePayment p in objInvoice.InvoicePayments)
                    {
                        p.IsUpdate = true;
                        p.IsActive = false;
                        p.UpdatedAt = DateTime.Now;
                        unitOfWork.GetDbContext().Entry(p).State = System.Data.Entity.EntityState.Modified;
                    }
                    unitOfWork.Save();
                    MessageBox.Show("Patient Invoice Deleted Succesfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnRefresh.PerformClick();
                    return;
                }

                if (e.ColumnIndex == grdInvoices.Columns["colPrintInvoice"].Index)
                {
                    Reports.POSInvoices.POSInvoiceViewer v = new Reports.POSInvoices.POSInvoiceViewer(InvoiceId, true);
                    //Reports.Pharmacy.PatientInvoiceViewer v = new Reports.Pharmacy.PatientInvoiceViewer(InvoiceId);
                    v.Show();
                    return;
                }
                if (e.ColumnIndex == grdInvoices.Columns["colPrintInvoiceRefund"].Index)
                {
                    Reports.PatientInvoiceRefundViewer v = new Reports.PatientInvoiceRefundViewer(InvoiceId);
                    v.Show();
                    return;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnPrintInvoiceHistory_Click(object sender, EventArgs e)
        {
            try
            {
                PatientVM patient = unitOfWork.InvoiceRepository.GetPatienInvoices_Rpt(this.PatientId);
                lblPatientName.Text = patient.Name;
                lblMrNo.Text = patient.MRNo.ToString();
                lblHwId.Text = "N/A";
                lblPhone.Text = patient.Phone;
                //if (patient.ImagePath != null)
                //{
                //    pbPatientImage.Image = Image.FromFile(patient.ImagePath);
                //}
                //foreach (Tag objTag in patient.Tags)
                //{
                //    GenerateTag(objTag.TagId, objTag.TagName);
                //}
                List<PatientRptVM> PatientList = new List<PatientRptVM>();
                foreach (InvoiceVM i in patient.Invoices)
                {
                    PatientRptVM model = new PatientRptVM();
                    model.InvoiceRefNo = i.InvoiceRefNo.ToString();
                    foreach (InvoiceItemVM ii in i.InvoiceItems)
                    {
                        model.Description += ii.ItemName + ", ";
                    }
                    foreach (InvoicePaymentVM ip in i.InvoicePayments)
                    {
                        model.Paid += ip.Payment + Environment.NewLine;
                        model.PaymentDate += ip.PaymentDate.ToShortDateString() + Environment.NewLine;
                    }
                    model.Description.Trim().TrimEnd(',');
                    model.Dues = i.Due > 0 ? i.Due.ToString() : "0";
                    model.Total = i.SubTotal.ToString();
                    model.Advance =  i.Due < 0 ? (-1 * i.Due).ToString() : "0";
                    PatientList.Add(model);
                }
                PatientInvoiceViewer f = new PatientInvoiceViewer(PatientList);
                f.Show();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
    }
}