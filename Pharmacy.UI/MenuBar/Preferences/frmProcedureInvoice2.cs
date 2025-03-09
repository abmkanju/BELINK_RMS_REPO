using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Infrastructure;
using System.Transactions;
using Pharmacy.UI.MenuBar.Preferences;
using Pharmacy.Core;
using PagedList;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmProcedureInvoice2 : Form
    {
        private UnitOfWork unitOfWork;
        List<InvoicePayment> DeletedPayments = new List<InvoicePayment>();
        DataTable dtStockConsumption = new DataTable();
        List<InvoiceItemVM> DeletedInvoiceItems;
        long PatientId = 0;
        long InvoiceEditId = 0;
        bool IsInvoiceEdit = false;
        bool IsInvoiceRefund = false;
        bool IsDiscountByItem = false;
        int EditingRowIndex = -1;
        bool IsNameSearching, IsPhoneSearching = false;
        bool IsReturnedQuantityFound = false;
        private double InitialTotalRefundValue = 0;
        private double LoadedInvoice_TotalPaid = 0;
        public frmProcedureInvoice2()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            DeletedInvoiceItems = new List<InvoiceItemVM>();
        }
        public frmProcedureInvoice2(long PatientId)
        {
            InitializeComponent();
            this.PatientId = PatientId;
            unitOfWork = new UnitOfWork();
            DeletedInvoiceItems = new List<InvoiceItemVM>();
        }
        public frmProcedureInvoice2(bool IsInvoiceEdit, bool IsInvoiceRefund, long InvoiceEditId)
        {
            InitializeComponent();
            this.InvoiceEditId = InvoiceEditId;
            this.IsInvoiceEdit = IsInvoiceEdit;
            this.IsInvoiceRefund = IsInvoiceRefund;
            unitOfWork = new UnitOfWork();
            DeletedInvoiceItems = new List<InvoiceItemVM>();
        }
        private void setNumericUD_toEmpty()
        {
            numRate.ResetText();
            numDiscount.ResetText();
        }
        private void frmConsumeStock_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged;
            txtSearchByName.TextChanged -= txtSearchByName_TextChanged;
            //txtSearchByName.Enter -= txtSearchByName_Enter;
            cmbPmntType1.SelectedIndex = 0;
            cmbPmntType2.SelectedIndex = 0;
            cmbPmntType3.SelectedIndex = 0;
            cmbPmntType4.SelectedIndex = 0;
            cmbPmntType5.SelectedIndex = 0;
            cmbTotalDiscountType.SelectedIndex = 0;

            try
            {
                Tuple<double, double, double> invoiceTotals = null;
                if ((IsInvoiceEdit || IsInvoiceRefund) && this.InvoiceEditId > 0)
                {
                    if (IsInvoiceRefund)
                    {
                        pnlDues.Visible = false;
                        grdItems.Columns["colEdit"].Visible = false;
                        grdItems.Columns["colRemove"].Visible = false;
                        grdItems.Columns["colReturnToggle"].Visible = true;
                        pnlPatientDetails.Enabled = false;
                    }
                    invoiceTotals = LoadInvoice(this.InvoiceEditId);
                    if (IsReturnedQuantityFound)
                    {
                        grdItems.Columns["colEdit"].Visible = false;
                        grdItems.Columns["colRemove"].Visible = false;
                        pnlAddItem.Enabled = false;
                    }
                    if (IsInvoiceEdit)
                    {
                        this.Text = "Invoice Edit";
                    }
                    if (IsInvoiceRefund)
                    {
                        this.Text = "Invoice Refund";
                        pnlAddItem.Visible = false;
                        grdItems.Location = new Point(grdItems.Location.X, grdItems.Location.Y - 80);
                        grdItems.Height = 300;
                        grdItems.Columns["colReturnQuantity"].Visible = true;
                        pnlRefund.Visible = true;
                        numTotalDiscount.Enabled = false;
                        cmbTotalDiscountType.Enabled = false;
                    }
                }
                if (this.PatientId > 0)// patient invoice
                {
                    LoadPateint();
                }
                if (!IsInvoiceRefund)// load items when its not refunding invoice
                {
                    cmbProcedure.SelectedIndexChanged -= cmbProcedure_SelectedIndexChanged;
                    LoadItems();
                    cmbProcedure.SelectedIndexChanged += cmbProcedure_SelectedIndexChanged;
                }
                if (invoiceTotals != null && !IsDiscountByItem)
                {
                    numTotalDiscount.Value = (decimal)invoiceTotals.Item1;
                    //lblGrandTotal.Text = invoiceTotals.Item2.ToString();
                    //cmbTotalDiscountType.SelectedIndex = 0;
                }
                // don't move below two lines up, otherwise auto-calculation on invoice-edit/refund will be disturbed.
                cmbDiscountType.SelectedIndexChanged -= cmbDiscountType_SelectedIndexChanged;
                cmbDiscountType.SelectedIndex = 1; // for add item panel
                cmbDiscountType.SelectedIndexChanged += cmbDiscountType_SelectedIndexChanged;
                cmbProcedure.SelectedIndex = -1; // for add item panel
                // don't below line anywhere else, this will updted numModifieddiscount values after automatic calculatoin
                if (invoiceTotals != null)
                {
                    numModifiedDiscount.Value = (decimal)invoiceTotals.Item3;
                }
                //if (!IsInvoiceEdit && !IsInvoiceRefund) // commented 2020-05-13
                //{
                //    cmbPmntType1.SelectedIndex = 0;
                //    cmbPmntType2.SelectedIndex = 0;
                //    cmbPmntType3.SelectedIndex = 0;
                //    cmbPmntType4.SelectedIndex = 0;
                //    cmbTotalDiscountType.SelectedIndex = 0;
                //}
                SharedFunctions.GenerateStockConsumptionTable(dtStockConsumption);

                setNumericUD_toEmpty();
                txtAmount.Text = "";
                txtNetAmount.Text = "";
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged;
            txtSearchByName.TextChanged += txtSearchByName_TextChanged;
            cmbProcedure.Focus();
        }
        private void LoadPateint()
        {
            Patient objPatient = unitOfWork.PatientRepository.GetById(this.PatientId);
            if (objPatient != null)
            {
                pnlPatientDetails.Enabled = false;
            }
            txtSearchByName.Text = objPatient.Name;
            txtSearchByPhone.Text = objPatient.Phone;
        }
        private Tuple<double, double, double> LoadInvoice(long InvoiceId)
        {
            InvoiceVM objInvoice = unitOfWork.InvoiceRepository.GetProcedureInvoiceByInvoiceId(InvoiceId);
            txtSearchByName.Text = objInvoice.Patient.Name;
            txtSearchByPhone.Text = objInvoice.Patient.Phone;
            this.PatientId = objInvoice.Patient.PatientId;
            lblInvoiceNo.Text = objInvoice.InvoiceId.ToString("D6");
            pnlLoadedInvoiceData.Visible = true;
            this.LoadedInvoice_TotalPaid = objInvoice.TotalPaid;
            txtInvoiceNote.Text = objInvoice.Note;
            foreach (InvoiceItemVM i in objInvoice.InvoiceItems)
            {
                if (!i.IsActive) { continue; }
                if (i.ReturnedQuantity > 0) { IsReturnedQuantityFound = true; }
                AddItemtoGrid(i);
            }
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                r.DefaultCellStyle.BackColor = Color.LightGray;
                r.Cells["colReturnQuantity"].ReadOnly = true;
            }
            numDiscount_ValueChanged(null, null);
            cmbTotalDiscountType.SelectedIndex = objInvoice.DiscountType - 1;
            numTotalDiscount.Value = (decimal)objInvoice.TotalDiscount;
            numModifiedDiscount.Value = (decimal)objInvoice.ModifiedDiscount;
            int count = 1;
            cmbPmntType1.SelectedIndexChanged -= cmbPmntType1_SelectedIndexChanged;
            cmbPmntType2.SelectedIndexChanged -= cmbPmntType2_SelectedIndexChanged;
            cmbPmntType3.SelectedIndexChanged -= cmbPmntType3_SelectedIndexChanged;
            cmbPmntType4.SelectedIndexChanged -= cmbPmntType4_SelectedIndexChanged;
            cmbPmntType5.SelectedIndexChanged -= cmbPmntType5_SelectedIndexChanged;
            foreach (InvoicePaymentVM p in objInvoice.InvoicePayments)
            {
                if (p.Payment < 0)
                {
                    InitialTotalRefundValue += Math.Abs(p.Payment);
                    continue;
                }
                if (count == 1)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt1.Enabled = false;
                        chkPmnt1.Enabled = false;
                        chkPmnt1.Checked = true;

                        chkPmnt2.Visible = false;
                        chkPmnt2.Checked = false;
                        chkPmnt2.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt1.Enabled = true;
                        chkPmnt1.Enabled = true;
                        chkPmnt1.Checked = true;

                        chkPmnt2.Checked = false;
                        chkPmnt2.Enabled = true;
                        pnlPmnt2.Enabled = true;
                        pnlPmnt2.Visible = false;
                    }
                    txtInvPmntId1.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType1.Value = p.PaymentDate;
                    cmbPmntType1.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt1.Value = (decimal)p.Payment;
                    txtChequeInfoId1.Text = p.ChequeInfoId.ToString();
                }
                if (count == 2)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt2.Enabled = false;
                        chkPmnt2.Checked = true;
                        chkPmnt2.Enabled = false;

                        chkPmnt3.Visible = false;
                        chkPmnt3.Checked = false;
                        chkPmnt3.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt1.Enabled = false;
                        chkPmnt1.Enabled = false;
                        chkPmnt1.Checked = true;

                        pnlPmnt2.Enabled = true;
                        chkPmnt2.Enabled = true;
                        chkPmnt2.Checked = true;

                        chkPmnt3.Checked = false;
                        chkPmnt3.Enabled = true;
                        pnlPmnt3.Enabled = true;
                        pnlPmnt3.Visible = false;
                    }

                    txtInvPmntId2.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType2.Value = p.PaymentDate;
                    cmbPmntType2.SelectedIndex = p.PaymentType - 1; // because index is zero based.
                    numPmnt2.Value = (decimal)p.Payment;
                    txtChequeInfoId2.Text = p.ChequeInfoId.ToString();
                }
                if (count == 3)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt3.Enabled = false;
                        chkPmnt3.Checked = true;
                        chkPmnt3.Enabled = false;

                        chkPmnt4.Visible = false;
                        chkPmnt4.Checked = false;
                        chkPmnt4.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt2.Enabled = false;
                        chkPmnt2.Enabled = false;
                        chkPmnt2.Checked = true;

                        pnlPmnt3.Enabled = true;
                        chkPmnt3.Enabled = true;
                        chkPmnt3.Checked = true;

                        chkPmnt4.Checked = false;
                        chkPmnt4.Enabled = true;
                        pnlPmnt4.Enabled = true;
                        pnlPmnt4.Visible = false;
                    }
                    txtInvPmntId3.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType3.Value = p.PaymentDate;
                    cmbPmntType3.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt3.Value = (decimal)p.Payment;
                    txtChequeInfoId3.Text = p.ChequeInfoId.ToString();
                }
                if (count == 4)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt4.Enabled = false;
                        chkPmnt4.Checked = true;
                        chkPmnt4.Enabled = false;

                        chkPmnt5.Visible = false;
                        chkPmnt5.Checked = false;
                        chkPmnt5.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt3.Enabled = false;
                        chkPmnt3.Enabled = false;
                        chkPmnt3.Checked = true;

                        pnlPmnt4.Enabled = true;
                        chkPmnt4.Enabled = true;
                        chkPmnt4.Checked = true;

                        chkPmnt5.Checked = false;
                        chkPmnt5.Enabled = true;
                        pnlPmnt5.Enabled = true;
                        pnlPmnt5.Visible = false;
                    }
                    txtInvPmntId4.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType4.Value = p.PaymentDate;
                    cmbPmntType4.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt4.Value = (decimal)p.Payment;
                    txtChequeInfoId4.Text = p.ChequeInfoId.ToString();
                }
                if (count == 5)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        chkPmnt5.Checked = true;
                        pnlPmnt5.Enabled = false;
                        chkPmnt5.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt4.Enabled = false;
                        chkPmnt4.Enabled = false;
                        chkPmnt4.Checked = true;

                        pnlPmnt5.Enabled = true;
                        chkPmnt5.Enabled = true;
                        chkPmnt5.Checked = true; ;
                    }
                    txtInvPmntId5.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType5.Value = p.PaymentDate;
                    cmbPmntType5.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt5.Value = (decimal)p.Payment;
                    txtChequeInfoId5.Text = p.ChequeInfoId.ToString();
                }
                count++;
            }
            //ShowLatestRefund();
            txtRefundAmount.Text = InitialTotalRefundValue.ToString();
            cmbPmntType1.SelectedIndexChanged += cmbPmntType1_SelectedIndexChanged;
            cmbPmntType2.SelectedIndexChanged += cmbPmntType2_SelectedIndexChanged;
            cmbPmntType3.SelectedIndexChanged += cmbPmntType3_SelectedIndexChanged;
            cmbPmntType4.SelectedIndexChanged += cmbPmntType4_SelectedIndexChanged;
            cmbPmntType5.SelectedIndexChanged += cmbPmntType5_SelectedIndexChanged;
            return new Tuple<double, double, double>(objInvoice.TotalDiscount, objInvoice.GrandTotal, objInvoice.ModifiedDiscount);
        }
        private void ShowLatestRefund(InvoicePaymentVM p)
        {
            dtpRedundDate.Value = p.PaymentDate;
            txtRefundReason.Text = p.RefundReason;
            txtRefundAmount.Text = p.Payment.ToString();
        }
        private void SetDisabledColor()
        {
        }
        private void LoadItems()
        {
            List<Procedure> Procedures = unitOfWork.ProcedureRepository.GetActiveProceduresList();
            cmbProcedure.DataSource = Procedures;
            cmbProcedure.ValueMember = "ProcedureId";
            cmbProcedure.DisplayMember = "ProcedureName";
        }
        private Tuple<double, double, double> GetGrandTotals()
        {
            int ReturnedQty = 0;
            double subTotal = 0;
            double GrandTotal = 0;
            double ReturnTotal = 0;
            double Discount = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                //if (r.Index != EditingRowIndex)
                //{
                    double colSaleQty = Convert.ToDouble(r.Cells["colQuantity"].Value);
                    double colRate = Convert.ToDouble(r.Cells["colRate"].Value);
                    double colAmount = Convert.ToDouble(r.Cells["colAmount"].Value);
                    double colDiscount = Convert.ToDouble(r.Cells["colDiscount"].Value);
                    int colDiscountType = Convert.ToInt32(r.Cells["colDiscountTypeid"].Value);
                    ReturnedQty = Convert.ToInt32(r.Cells["colReturnQtyInitial"].Value);
                    subTotal += colAmount;
                    if (colDiscountType == 2)
                    {
                        colDiscount = (colDiscount / 100) * colAmount;
                    }
                    ReturnTotal += (colRate * ReturnedQty) - ((colDiscount / colSaleQty) * ReturnedQty);
                    Discount += colDiscount;
                //}
            }
            if (!IsDiscountByItem)
            {
                Discount = Convert.ToDouble(numTotalDiscount.Value);
            }
            GrandTotal = subTotal - Discount - ReturnTotal;
            return new Tuple<double, double, double>(subTotal, GrandTotal, Discount);
        }
        private void CalculateTotals()
        {
            Tuple<double, double, double> GrandTotals = GetGrandTotals();
            double SubTotal = GrandTotals.Item1;
            double GrandTotal = GrandTotals.Item2;
            double TotalDiscount = GrandTotals.Item3;

            double rate = (double)numRate.Value;
            int qty = (int)numQuantity.Value;
            double amount = rate * qty;
            double discount = (double)numDiscount.Value;
            string discountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
            double Refund = 0;
            Refund = txtCurrentRefund.Text == "" ? 0 : double.Parse(txtCurrentRefund.Text);
            if (discountType.ToLower() == "%")
            {
                discount = (discount / 100) * amount;
            }
            SubTotal += amount;
            GrandTotal += amount - discount + Refund;
            //if (IsDiscountChanged)
            // {
            TotalDiscount += discount;
            //}
            txtAmount.Text = amount.ToString();
            txtNetAmount.Text = (amount - discount).ToString();
            lblSubTotal.Text = Math.Round(SubTotal, 2).ToString();
            lblGrandTotal.Text = Math.Round(GrandTotal, 2).ToString();
            numTotalDiscount.Value = (decimal)TotalDiscount;
            numModifiedDiscount.Value = (decimal)TotalDiscount;
            if (!(this.IsInvoiceRefund))
            {
                if(numPmnt1.Enabled && this.LoadedInvoice_TotalPaid == 0)
                {
                    numPmnt1.Value = (decimal)GrandTotal;
                }
                //var NewPayable = GetEditablePaymentBox();
                //if (NewPayable != null)
                //{
                //    NumericUpDown PmntBox = (NumericUpDown)NewPayable.Item1;
                //    string n = PmntBox.Name;
                //    double newPayment = GrandTotal - LoadedInvoice_TotalPaid;
                //    PmntBox.Value = (decimal)newPayment;
                //}
            }
            GetCustomerTotals();
        }
        private void CalculateTotals(DataGridViewRow r)
        {
            Tuple<double, double, double> GrandTotals = GetGrandTotals();
            double SubTotal = GrandTotals.Item1;
            double GrandTotal = GrandTotals.Item2;
            double TotalDiscount = GrandTotals.Item3;

            double rate = Convert.ToDouble(r.Cells["colRate"].Value);
            int qty = Convert.ToInt32(r.Cells["colQuantity"].Value);
            double amount = rate * qty;
            double discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
            int discountType = Convert.ToInt32(r.Cells["colDiscountTypeId"].Value);
            double Refund = 0;
            Refund = txtCurrentRefund.Text == "" ? 0 : double.Parse(txtCurrentRefund.Text);
            if (discountType == 2)
            {
                discount = (discount / 100) * amount;
            }
            //SubTotal += amount;
            //GrandTotal += amount - discount + Refund;
            //TotalDiscount += discount;            
            lblSubTotal.Text = Math.Round(SubTotal, 2).ToString();
            lblGrandTotal.Text = Math.Round(GrandTotal, 2).ToString();
            numTotalDiscount.Value = (decimal)TotalDiscount;
            numModifiedDiscount.Value = (decimal)TotalDiscount;
            if (!(this.IsInvoiceRefund))
            {
                if (numPmnt1.Enabled && this.LoadedInvoice_TotalPaid == 0)
                {
                    numPmnt1.Value = (decimal)GrandTotal;
                }
                //var NewPayable = GetEditablePaymentBox();
                //if (NewPayable != null)
                //{
                //    NumericUpDown PmntBox = (NumericUpDown)NewPayable.Item1;
                //    string n = PmntBox.Name;
                //    double newPayment = GrandTotal - LoadedInvoice_TotalPaid;
                //    PmntBox.Value = (decimal)newPayment;
                //}
            }
            GetCustomerTotals();
        }
        private Tuple<Control, double> GetEditablePaymentBox()
        {
            double TotalPaid = (double)(numPmnt1.Value + numPmnt2.Value + numPmnt3.Value + numPmnt4.Value + numPmnt5.Value);
            if (numPmnt1.Enabled && numPmnt1.Visible) return new Tuple<Control, double>(numPmnt1, TotalPaid);
            if (numPmnt2.Enabled && numPmnt2.Visible) return new Tuple<Control, double>(numPmnt2, TotalPaid);
            if (numPmnt3.Enabled && numPmnt3.Visible) return new Tuple<Control, double>(numPmnt3, TotalPaid);
            if (numPmnt4.Enabled && numPmnt4.Visible) return new Tuple<Control, double>(numPmnt4, TotalPaid);
            if (numPmnt5.Enabled && numPmnt5.Visible) return new Tuple<Control, double>(numPmnt5, TotalPaid);
            else return null;
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                InvoiceItem objInvoiceItem = new InvoiceItem();
                FillObject(objInvoiceItem);
                if (EditingRowIndex >= 0)
                {
                    if (!isValidQuantityEntered())
                    {
                        MessageBox.Show("Sold Quantity Can't be Greater Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        numQuantity.Focus();
                        numQuantity.Select(0, numQuantity.Value.ToString().Length);
                        return;
                    }
                    UpdateRow(objInvoiceItem);
                }
                else if (ProcedureAlreadyAdded(Convert.ToInt32(cmbProcedure.SelectedValue)))
                {
                    MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    if (!isValidQuantityEntered())
                    {
                        MessageBox.Show("Sold Quantity Can't be Greater Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        numQuantity.Focus();
                        numQuantity.Select(0, numQuantity.Value.ToString().Length);
                        return;
                    }
                    AddItemtoGrid(objInvoiceItem);
                }
                Clear();
                cmbProcedure.Focus();
            }
        }
        private void UpdateRow(InvoiceItem objInvoiceItem)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            
            r.Cells["colDescription"].Value = objInvoiceItem.ProcedureDescription;
            r.Cells["colRate"].Value = objInvoiceItem.Rate;
            r.Cells["colQuantity"].Value = objInvoiceItem.Quantity;
            r.Cells["colAmount"].Value = objInvoiceItem.Amount;
            r.Cells["colNetAmount"].Value = objInvoiceItem.NetAmount;
            r.Cells["colDiscount"].Value = objInvoiceItem.Discount;
            r.Cells["colDiscountType"].Value = objInvoiceItem.DiscountTypeString;
            r.Cells["colDiscountTypeId"].Value = objInvoiceItem.DiscountType;
            DataGridViewRowsAddedEventArgs e = new DataGridViewRowsAddedEventArgs(EditingRowIndex, 1);
            grdItems_RowsAdded(null, e);
            //r.Cells["colIsNewRow"].Value = true;
        }
        private void AddItemtoGrid(InvoiceItem objInvoiceItem)
        {
            grdItems.Rows.Add(0, objInvoiceItem.ProcedureId, objInvoiceItem.ProcedureName, objInvoiceItem.ProcedureDescription, objInvoiceItem.Rate, objInvoiceItem.Quantity, objInvoiceItem.Amount, objInvoiceItem.NetAmount, objInvoiceItem.Discount, objInvoiceItem.DiscountTypeString, objInvoiceItem.DiscountType, true);
        }
        private void AddItemtoGrid(InvoiceItemVM objInvoiceItem)
        {
            grdItems.Rows.Add(objInvoiceItem.InvoiceItemId, objInvoiceItem.ProcedureId, objInvoiceItem.ProcedureName, objInvoiceItem.ProcedureDescription, objInvoiceItem.Rate, objInvoiceItem.Quantity, objInvoiceItem.Amount, objInvoiceItem.NetAmount, objInvoiceItem.Discount, objInvoiceItem.DiscountTypeString, objInvoiceItem.DiscountType, false, objInvoiceItem.ReturnedQuantity, objInvoiceItem.ReturnedQuantity);
        }
        private void FillObject(InvoiceItem objInvoiceItem)
        {
            objInvoiceItem.ProcedureId = Convert.ToInt32(cmbProcedure.SelectedValue);
            objInvoiceItem.ProcedureName = cmbProcedure.GetItemText(cmbProcedure.SelectedItem);
            objInvoiceItem.ProcedureDescription = txtDescription.Text.Trim();
            //objInvoiceItem.BatchId = Convert.ToInt32(cmbSelectBatch.SelectedValue);
            //objInvoiceItem.BatchName = cmbSelectBatch.GetItemText(cmbSelectBatch.SelectedItem);
            objInvoiceItem.Rate = (double)numRate.Value;
            objInvoiceItem.Quantity = (int)numQuantity.Value;
            objInvoiceItem.Amount = double.Parse(txtAmount.Text);
            objInvoiceItem.NetAmount = double.Parse(txtNetAmount.Text);
            objInvoiceItem.Discount = (double)numDiscount.Value;
            objInvoiceItem.DiscountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
            objInvoiceItem.DiscountTypeString = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
        }
        private void FillObject(InvoiceItem objInvoiceItem, DataGridViewRow r)
        {
            objInvoiceItem.ProcedureId = Convert.ToInt32(r.Cells["colProcedureId"].Value);
            objInvoiceItem.ProcedureDescription = r.Cells["colDescription"].Value.ToString();
            objInvoiceItem.Rate = Convert.ToDouble(r.Cells["colRate"].Value);
            objInvoiceItem.Quantity = Convert.ToInt32(r.Cells["colQuantity"].Value);
            objInvoiceItem.Amount = Convert.ToDouble(r.Cells["colAmount"].Value);
            objInvoiceItem.NetAmount = Convert.ToDouble(r.Cells["colNetAmount"].Value);
            objInvoiceItem.Discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
            objInvoiceItem.DiscountType = Convert.ToInt32(r.Cells["colDiscountTypeId"].Value);
        }
        private bool IsValidInput()
        {
            ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            bool ErrFound = false;
             if (Convert.ToInt32(cmbProcedure.SelectedValue) < 0)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrSelectItem.Visible = false;
            }          
            if (Convert.ToDouble(numRate.Value) <= 0)
            {
                errRate.Visible = true;
                ErrFound = true;
                numRate.Focus();
                ErrMessage.Visible = true;
                return false;
            }
            else
            {
                ErrMessage.Visible = false;
                errRate.Visible = false;
            }
            if (Convert.ToInt32(numQuantity.Value) <= 0)
            {
                ErrQuantity.Visible = true;
                ErrFound = true;
                numQuantity.Focus();
                ErrMessage.Visible = true;
                return false;
            }
            else
            {
                ErrMessage.Visible = false;
                ErrQuantity.Visible = false;
            }
            if (!IsCorrectItemDiscount())
            {
                Errdisc.Visible = true;
                ErrMessage.Text = "Incorrect Discount Values.";
                ErrFound = true;
            }
            else
            {
                ErrMessage.Visible = false;
                Errdisc.Visible = false;
            }

            if (!ErrFound)
            {
                //ErrMessage.Visible = false;
                return true;
            }
            else
            {
                ErrMessage.Visible = true;
                return false;
            }
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index
                //|| e.ColumnIndex == grdItems.Columns["colRemove"].Index
                )
            {
                if ((IsInvoiceEdit || IsInvoiceRefund) && !Convert.ToBoolean(grdItems.Rows[e.RowIndex].Cells["colIsNewRow"].Value))
                {
                    MessageBox.Show("Can't Edit This Row", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {
                Clear();
                RegisterEvents(false);
                cmbProcedure.Enabled = false;
                FillForm(grdItems.Rows[e.RowIndex]);
                grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
                EditingRowIndex = e.RowIndex;
                this.btnAddItem.Text = "Update Item";
                RegisterEvents(true);
                //grdItems.Rows.RemoveAt(e.RowIndex);
            }
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {
                long InvoiceItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colInvoiceItemId"].Value);
                if (InvoiceItemId > 0)
                {
                    DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete Selected Item From Invoice?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
                    AddToDeletedItems(grdItems.Rows[e.RowIndex]);
                }
                if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                grdItems.Rows.RemoveAt(e.RowIndex);
            }

            if (e.ColumnIndex == grdItems.Columns["colReturnToggle"].Index)
            {
                int RQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colReturnQtyInitial"].Value);
                if (RQty > 0)
                {
                    grdItems.Rows[e.RowIndex].Cells["colReturnToggle"].ToolTipText = "Can't Return";
                    return;
                }
                if (grdItems.Rows[e.RowIndex].Cells["colReturnToggle"].Value.ToString() == "Return")
                {
                    DataGridViewButtonCell BtnCell = (DataGridViewButtonCell)grdItems.Rows[e.RowIndex].Cells["colReturnToggle"];
                    BtnCell.UseColumnTextForButtonValue = false;
                    BtnCell.Value = "Cancel";
                    grdItems.CurrentCell = grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"];
                    grdItems.CurrentCell.Selected = true;
                    grdItems.BeginEdit(false);
                    grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].ReadOnly = false;
                }
                else if (grdItems.Rows[e.RowIndex].Cells["colReturnToggle"].Value.ToString() == "Cancel")
                {
                    var BtnCell = (DataGridViewButtonCell)grdItems.Rows[e.RowIndex].Cells["colReturnToggle"];
                    BtnCell.Value = "Return";
                    grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].ReadOnly = true;
                    grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = 0;
                }
            }
        }
        private void AddToDeletedItems(DataGridViewRow r)
        {
            InvoiceItemVM DeletedITem = new InvoiceItemVM();
            DeletedITem.InvoiceItemId = Convert.ToInt32(r.Cells["colInvoiceItemId"].Value);
            DeletedITem.ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            DeletedITem.BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);
            DeletedInvoiceItems.Add(DeletedITem);
        }
        private void RegisterEvents(bool Register)
        {
            if (Register)
            {
                numRate.ValueChanged += numRate_ValueChanged;
                numQuantity.ValueChanged += numQuantity_ValueChanged;
                numDiscount.ValueChanged += numDiscount_ValueChanged;
                cmbDiscountType.SelectedIndexChanged += cmbDiscountType_SelectedIndexChanged;
            }
            else
            {
                numRate.ValueChanged -= numRate_ValueChanged;
                numQuantity.ValueChanged -= numQuantity_ValueChanged;
                numDiscount.ValueChanged -= numDiscount_ValueChanged;
                cmbDiscountType.SelectedIndexChanged -= cmbDiscountType_SelectedIndexChanged;
            }
        }
        private void FillForm(DataGridViewRow r)
        {
            long ProcId = Convert.ToInt64(r.Cells["colProcedureId"].Value);
            cmbProcedure.SelectedValue = ProcId;
            txtDescription.Text = r.Cells["colDescription"].Value.ToString();
            numRate.Value = Convert.ToDecimal(r.Cells["colRate"].Value);
            numQuantity.Value = Convert.ToInt32(r.Cells["colQuantity"].Value);
            numDiscount.Value = Convert.ToDecimal(r.Cells["colDiscount"].Value);
            cmbDiscountType.SelectedItem = r.Cells["colDiscounttype"].Value.ToString();
            txtAmount.Text = r.Cells["colAmount"].Value.ToString();
            txtNetAmount.Text = r.Cells["colNetAmount"].Value.ToString();
        }
        private bool ItemAlreadyAdded(int ItemId, long BatchId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId
                    && Convert.ToInt32(r.Cells["colBatchId"].Value) == BatchId)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ProcedureAlreadyAdded(int ProcedureId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colProcedureId"].Value) == ProcedureId)
                {
                    return true;
                }
            }
            return false;
        }
        private bool isValidQuantityEntered()
        {
            return true;
            //int AvailableStock = BatchStockList.Where(x => x.BatchId == Convert.ToInt32(cmbSelectBatch.SelectedValue)).FirstOrDefault().AvailableStock;
            //if (numQuantity.Value > AvailableStock)
            //{
            //    ToolTip tt = new ToolTip();
            //    numQuantity.Value = 1;
            //    ErrMessage.Text = "Alert : Sold Quantity Can't be Greater Than Available Quantity.";
            //    ErrMessage.Visible = true;
            //    tt.Show("Please Enter Valid Quantity", numQuantity, 800);
            //    numQuantity.Select(0, 1);
            //    return false;
            //}
            //else
            //{
            //    ErrMessage.Visible = false;
            //}
            //return true;
        }
        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            CalculateItemTotals();
            ////IsDiscountChanged = false;
            //if (cmbSelectBatch.SelectedIndex >= 0)
            //{
            //    //isValidQuantityEntered();
            //    CalculateTotals();
            //}
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void Clear()
        {
            try
            {
                //RegisterEvents(false);
                cmbProcedure.SelectedIndex = -1;
                cmbProcedure.Enabled = true;
                numQuantity.Value = 1;
                numRate.Value = 0;
                numDiscount.Value = 0;
                this.btnAddItem.Text = "Add Item";
                if (EditingRowIndex > -1)
                {
                    grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                EditingRowIndex = -1;
                setNumericUD_toEmpty();
                txtAmount.Text = "";
                txtNetAmount.Text = "";
                txtAvlQty.Text = "";
                if (grdItems.Rows.Count == 0)
                {
                    numTotalDiscount.Enabled = true;
                    numModifiedDiscount.Value = 0;
                    numTotalDiscount.Value = 0;
                }
                //RegisterEvents(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAll(false);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Clearing Screen, Please Try Again OR ReOpen This Screen.", ex.Message, "Error");
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveInvoice(true);
        }
        private long SaveInvoice(bool ShowSuccessDialog)
        {
            long InvoiceId = 0;
            if (grdItems.Rows.Count <= 0)
            {
                MessageBox.Show("Can't Save Empty Invoice, ", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }
            if (EditingRowIndex > -1)
            {
                MessageBox.Show("Can't Save Invoice While Editing an Item, ", "Editing Invoice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }
            if (this.InvoiceEditId > 0)
            {
                if (IsInvoiceEdit)
                {
                    InvoiceId = EditInvoice(ShowSuccessDialog);
                }
                else if (IsInvoiceRefund)
                {
                    InvoiceId = RefundInvoice(ShowSuccessDialog);
                }
                if (InvoiceId > 0)
                {
                    ClearAll(true);
                }
                return InvoiceId;
            }
            InvoiceId = InsertInvoice(ShowSuccessDialog);
            if (InvoiceId > 0)
            {
                ClearAll(true);
            }
            return InvoiceId;
        }
        private long InsertInvoice(bool ShowSuccessMsg)
        {
            TransactionScope scope;
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice objInvoice = new Invoice();
                        objInvoice.CreateDate = DateTime.Now;
                        objInvoice.SubTotal = double.Parse(lblSubTotal.Text);
                        objInvoice.GrandTotal = double.Parse(lblGrandTotal.Text);
                        objInvoice.TotalDiscount = Convert.ToDouble(numTotalDiscount.Value);
                        objInvoice.ModifiedDiscount = Convert.ToDouble(numModifiedDiscount.Value);
                        objInvoice.DiscountType = cmbTotalDiscountType.GetItemText(cmbTotalDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        objInvoice.Due = double.Parse(lblDue.Text);
                        objInvoice.CreateDate = DateTime.Now;
                        objInvoice.UpdateDate = DateTime.Now;
                        objInvoice.IsNew = true;
                        objInvoice.IsActive = true;
                        objInvoice.InvoiceItems = new List<InvoiceItem>();
                        objInvoice.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        objInvoice.IsProcedureInvoice = false;
                        objInvoice.Note = txtInvoiceNote.Text.Trim();
                        objInvoice.IsProcedureInvoice = true;
                        //StockConsumption ObjCons = new StockConsumption();
                        //ObjCons.CreateDate = DateTime.Now;
                        //ObjCons.UpdateDate = DateTime.Now;
                        //ObjCons.IsNew = true;
                        //ObjCons.User = SharedVariables.LoggedInUser;
                        //ObjCons.IsActive = true;
                        //ObjCons.StockConsumptions = new List<StockConsumptionItem>();
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            InvoiceItem objInvoiceItem = new InvoiceItem();
                            FillObject(objInvoiceItem, r);
                            objInvoiceItem.Item = unitOfWork.ItemRspository.GetDefaultItem();
                            objInvoiceItem.Procedure = unitOfWork.ProcedureRepository.GetById(objInvoiceItem.ProcedureId);                          
                            objInvoiceItem.IsNew = true;
                            objInvoiceItem.IsActive = true;
                            objInvoiceItem.CreateDate = DateTime.Now;
                            objInvoiceItem.UpdateDate = DateTime.Now;                            
                            objInvoice.InvoiceItems.Add(objInvoiceItem);
                            //StockConsumptionItem sc = new StockConsumptionItem();
                            //sc.Quantity = objInvoiceItem.Quantity;
                            //sc.ConsumptionType = (int)SharedVariables.ConsumptionType.Sales;
                            //sc.Comment = "Sale Transaction";
                            //sc.Item = objInvoiceItem.Item;
                            //sc.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                            //sc.CreateDate = DateTime.Now;
                            //sc.UpdateDate = DateTime.Now;
                            //sc.IsNew = true;
                            //sc.IsActive = true;
                            //ObjCons.StockConsumptions.Add(sc);
                            //dtStockConsumption.Rows.Add(objInvoiceItem.Quantity, (int)SharedVariables.ConsumptionType.Sales, "Sale Transaction", objInvoiceItem.BatchId, objInvoiceItem.ItemId, null, DateTime.Now);
                        }

                        if (!AddInvoicePayments(objInvoice))
                        {
                            return 0;
                        }
                        //if (double.Parse(lblDue.Text) > 0)
                        //{
                        //    MessageBox.Show("Due Amount is (" + lblDue.Text.ToString() + " ). You Can't Insert Invoice With Due Payments.", "Due Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return;
                        //}
                        if (PatientId > 0)
                        {
                            objInvoice.Patient = unitOfWork.PatientRepository.GetById(PatientId);
                        }
                        else
                        {
                            if (txtSearchByName.Text.Trim() != "")
                            {
                                Patient newP = new Patient();
                                newP.MRNo = unitOfWork.PatientRepository.GetNewMrNo();
                                newP.Name = txtSearchByName.Text.Trim();
                                newP.Phone = txtSearchByPhone.Text.Trim();
                                newP.Address = txtAddress.Text.Trim();
                                newP.CreateDate = DateTime.Now;
                                newP.UpdateDate = DateTime.Now;
                                newP.RegistrationDate = DateTime.Now;
                                newP.IsActive = true;
                                newP.IsNew = true;
                                objInvoice.Patient = newP;
                            }
                        }
                        //objInvoice.StockConsumption = ObjCons;
                        //unitOfWork.StockConsumptionRepository.Insert(ObjCons);
                        unitOfWork.InvoiceRepository.Insert(objInvoice);
                        //long invoiceId = unitOfWork.InvoiceRepository.InsertInvoice(objInvoice);
                        //Invoice InvoiceGet = unitOfWork.InvoiceRepository.GetById(invoiceId);
                        //ConsMaster.Invoice = InvoiceGet;
                        //foreach (StockConsumption sc in StockConsumptions)
                        //{
                        //    sc.Invoice = IvoiceGet;
                        //    unitOfWork.StockConsumptionRespository.Insert(sc);
                        //}
                        //unitOfWork.StockConsumptionRespository.BulkInsert(dtStockConsumption);
                        unitOfWork.Save();
                        scope.Complete();
                        if (ShowSuccessMsg)
                        {
                            MessageBox.Show("Invoice Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return objInvoice.InvoiceId;
                    }
                }
                catch (Exception ex)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                    scope.Dispose();
                    return 0;
                }
            }
        }
        private long EditInvoice(bool ShowSuccessMsg)
        {
            TransactionScope scope;
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice objInvoice = new Invoice();
                        objInvoice = unitOfWork.InvoiceRepository.GetInvoicceByIdWithDetails(this.InvoiceEditId);
                        objInvoice.CreateDate = DateTime.Now;
                        objInvoice.SubTotal = double.Parse(lblSubTotal.Text);
                        objInvoice.GrandTotal = double.Parse(lblGrandTotal.Text);
                        objInvoice.TotalDiscount = Convert.ToDouble(numTotalDiscount.Value);
                        objInvoice.ModifiedDiscount = Convert.ToDouble(numTotalDiscount.Value);
                        objInvoice.DiscountType = cmbTotalDiscountType.GetItemText(cmbTotalDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        objInvoice.Due = double.Parse(lblDue.Text);                        
                        objInvoice.InvoiceNote = txtInvoiceNote.Text.Trim();
                        objInvoice.UpdateDate = DateTime.Now;
                        objInvoice.IsUpdate = true;
                        objInvoice.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        objInvoice.Note = txtInvoiceNote.Text.Trim();
                        long InvoiceItemId = 0;
                        foreach (InvoiceItemVM DeletedItem in DeletedInvoiceItems)
                        {
                            InvoiceItem _DeletedItem = objInvoice.InvoiceItems.Where(i => i.Item.ItemID == DeletedItem.ItemId && i.Batch.BatchId == DeletedItem.BatchId && i.IsActive).FirstOrDefault();
                            _DeletedItem.IsUpdate = true;
                            _DeletedItem.IsActive = false;
                            _DeletedItem.IsSynced = false;
                            _DeletedItem.UpdateDate = DateTime.Now;
                            unitOfWork.GetDbContext().Entry(_DeletedItem).State = System.Data.Entity.EntityState.Modified;
                            // commected for consume stock change
                            //StockConsumptionItem DeletedCon = objInvoice.StockConsumption.StockConsumptions.Where(c => c.Item.ItemID == DeletedItem.ItemId && c.Batch.BatchId == DeletedItem.BatchId && c.IsActive).FirstOrDefault();

                            //StockConsumptionItem DeletedCon = new StockConsumptionItem();//objInvoice.StockConsumptions.Where(c => c.Item.ItemID == DeletedItem.ItemId && c.Batch.BatchId == DeletedItem.BatchId && c.IsActive).FirstOrDefault();DeletedCon.IsSynced = false;
                            //DeletedCon.IsActive = false;
                            //DeletedCon.IsUpdate = true;
                            //DeletedCon.UpdateDate = DateTime.Now;
                            //unitOfWork.GetDbContext().Entry(DeletedCon).State = System.Data.Entity.EntityState.Modified;
                        }
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            InvoiceItemId = Convert.ToInt32(r.Cells["colInvoiceItemId"].Value);
                            //if (Convert.ToBoolean(r.Cells["colIsNewRow"].Value) == 0)
                            if (InvoiceItemId == 0) // new items
                            {
                                InvoiceItem objInvoiceItem = new InvoiceItem();
                                FillObject(objInvoiceItem, r);
                                objInvoiceItem.Item = unitOfWork.ItemRspository.GetById(objInvoiceItem.ItemId);
                                objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemID, objInvoiceItem.Batch.BatchId); 
                                objInvoiceItem.TotalCostPrice = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                objInvoiceItem.IsNew = true;
                                objInvoiceItem.IsActive = true;
                                objInvoiceItem.CreateDate = DateTime.Now;
                                objInvoiceItem.UpdateDate = DateTime.Now;
                                objInvoice.InvoiceItems.Add(objInvoiceItem);
                                //StockConsumptionItem scItem = new StockConsumptionItem();
                                //scItem.Quantity = objInvoiceItem.Quantity;
                                //scItem.ConsumptionType = (int)SharedVariables.ConsumptionType.Sales;
                                //scItem.Comment = "Sale Transaction";
                                //scItem.Item = objInvoiceItem.Item;
                                //scItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                //scItem.CreateDate = DateTime.Now;
                                //scItem.UpdateDate = DateTime.Now;
                                //scItem.IsNew = true;
                                //scItem.IsActive = true;
                                //objInvoice.StockConsumption.StockConsumptions.Add(scItem);
                                // commected for consume stock change
                                //objInvoice.StockConsumptions.Add(sc);
                                //dtStockConsumption.Rows.Add(objInvoiceItem.Quantity, (int)SharedVariables.ConsumptionType.Sales, "Sale Transaction", objInvoiceItem.BatchId, objInvoiceItem.ItemId, null, DateTime.Now);
                            }
                            else
                            {
                                InvoiceItem objInvoiceItem = objInvoice.InvoiceItems.Where(item => item.InvoiceItemId == InvoiceItemId).FirstOrDefault();
                                FillObject(objInvoiceItem, r);
                                //UpdateRow(objInvoiceItem);
                                // item is not being updated in this case
                                //objInvoiceItem.Item = unitOfWork.ItemRspository.GetById(objInvoiceItem.ItemId);
                                objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemID, objInvoiceItem.Batch.BatchId); 
                                objInvoiceItem.TotalCostPrice=  objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                objInvoiceItem.IsUpdate = true;
                                objInvoiceItem.IsSynced = false;
                                objInvoiceItem.UpdateDate = DateTime.Now;
                                unitOfWork.GetDbContext().Entry(objInvoiceItem).State = System.Data.Entity.EntityState.Modified;
                                // commecnted for consume stock change..
                                //StockConsumptionItem ModifiedCon = objInvoice.StockConsumption.StockConsumptions.Where(c => c.Item == objInvoiceItem.Item && c.Batch == objInvoiceItem.Batch).FirstOrDefault();
                                //StockConsumptionItem ModifiedCon = new StockConsumptionItem();//objInvoice.StockConsumptions.Where(c => c.Item == objInvoiceItem.Item && c.Batch == objInvoiceItem.Batch).FirstOrDefault();
                                //ModifiedCon.Quantity = objInvoiceItem.Quantity;
                                //ModifiedCon.ConsumptionType = (int)SharedVariables.ConsumptionType.Sales;
                                //ModifiedCon.Comment = "Sale Transaction";
                                //sc.Item = objInvoiceItem.Item;
                                //ModifiedCon.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                //ModifiedCon.IsUpdate = true;
                                //ModifiedCon.UpdateDate = DateTime.Now;
                                //ModifiedCon.IsSynced = false;
                                //unitOfWork.GetDbContext().Entry(ModifiedCon).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                        List<InvoicePayment> NewPaymentsMade = new List<InvoicePayment>();
                        if (!AddInvoicePayments(NewPaymentsMade))
                        {
                            return 0;
                        }
                        bool found = false;

                        foreach (InvoicePayment dp in DeletedPayments)
                        {
                            InvoicePayment DeletedPayment = objInvoice.InvoicePayments.Where(p => p.InvoicePaymentId == dp.InvoicePaymentId).FirstOrDefault();
                            DeletedPayment.IsUpdate = true;
                            DeletedPayment.IsSynced = false;
                            DeletedPayment.UpdatedAt = DateTime.Now;
                            DeletedPayment.IsActive = false;
                            unitOfWork.GetDbContext().Entry(DeletedPayment).State = System.Data.Entity.EntityState.Modified;
                        }
                        foreach (InvoicePayment p in NewPaymentsMade)
                        {
                            found = false;
                            foreach (InvoicePayment p2 in objInvoice.InvoicePayments)
                            {
                                if (p.InvoicePaymentId == p2.InvoicePaymentId && p.InvoicePaymentId != 0)
                                {
                                    found = true;
                                    p2.Payment = p.Payment;
                                    p2.IsUpdate = true;
                                    p2.UpdatedAt = DateTime.Now;
                                    unitOfWork.GetDbContext().Entry(p2).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                            if (!found)
                            {
                                objInvoice.InvoicePayments.Add(p);
                            }
                        }
                        //if (double.Parse(lblDue.Text) > 0)
                        //{
                        //    MessageBox.Show("Due Amount is (" + lblDue.Text.ToString() + " ). You Can't Insert Invoice With Due Payments.", "Due Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return;
                        //}
                        if (PatientId > 0)
                        {
                            objInvoice.Patient = unitOfWork.PatientRepository.GetById(PatientId);
                        }
                        else
                        {
                            if (txtSearchByName.Text.Trim() != "")
                            {
                                Patient newP = new Patient();
                                newP.MRNo = unitOfWork.PatientRepository.GetNewMrNo();
                                newP.Name = txtSearchByName.Text.Trim();
                                newP.Phone = txtSearchByPhone.Text.Trim();
                                newP.Address = txtAddress.Text.Trim();
                                newP.CreateDate = DateTime.Now;
                                newP.UpdateDate = DateTime.Now;
                                newP.RegistrationDate = DateTime.Now;
                                newP.IsActive = true;
                                newP.IsNew = true;
                                objInvoice.Patient = newP;
                            }
                        }
                        //commented on 2020-05-22
                        //foreach (StockConsumption sc in StockConsumptions)
                        //{
                        //    sc.Invoice = objInvoice;
                        //    unitOfWork.StockConsumptionRespository.Insert(sc);
                        //}
                        //unitOfWork.StockConsumptionRespository.BulkInsert(dtStockConsumption);
                        unitOfWork.InvoiceRepository.Update(objInvoice);
                        unitOfWork.Save();
                        scope.Complete();
                        if (ShowSuccessMsg)
                        {
                            MessageBox.Show("Invoice Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return this.InvoiceEditId;
                    }
                }
                catch (Exception ex)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                    scope.Dispose();
                    return 0;
                }
            }
        }
        private long RefundInvoice(bool ShowSuccessMsg)
        {
            TransactionScope scope;
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice objInvoice = new Invoice();
                        objInvoice = unitOfWork.InvoiceRepository.GetInvoiceById_Inc_Pmnts_cons(this.InvoiceEditId);
                        objInvoice.SubTotal = double.Parse(lblSubTotal.Text);
                        objInvoice.GrandTotal = double.Parse(lblGrandTotal.Text);
                        objInvoice.TotalDiscount = Convert.ToDouble(numTotalDiscount.Value);
                        objInvoice.ModifiedDiscount = Convert.ToDouble(numModifiedDiscount.Value);
                        objInvoice.DiscountType = cmbTotalDiscountType.GetItemText(cmbTotalDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        objInvoice.Due = double.Parse(lblDue.Text);                        
                        objInvoice.UpdateDate = DateTime.Now;
                        objInvoice.IsUpdate = true;
                        objInvoice.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser);
                        objInvoice.Note = txtInvoiceNote.Text.Trim();
                        int ItemId = 0;
                        int RtrndQty = 0;
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                            RtrndQty = Convert.ToInt32(r.Cells["colReturnQuantity"].Value);
                            if (RtrndQty > 0)
                            {
                                foreach (InvoiceItem i in objInvoice.InvoiceItems)
                                {
                                    if (i.Item.ItemID == ItemId)
                                    {
                                        i.IsUpdate = true;
                                        i.UpdateDate = DateTime.Now;
                                        i.IsSynced = false;
                                        i.ReturnedQuantity = RtrndQty;
                                        i.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(i.Item.ItemID, i.Batch.BatchId);
                                        i.TotalCostPrice = (i.PerUnitCostPrice* i.Quantity) - (i.PerUnitCostPrice * i.ReturnedQuantity);
                                        unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                                    }
                                }
                                //commented for consume stock change
                                //foreach (StockConsumptionItem s in objInvoice.StockConsumption.StockConsumptions)
                                //{
                                //    if (s.Item.ItemID == ItemId)
                                //    {
                                //        int NewQty = s.Quantity - RtrndQty;
                                //        s.Quantity = NewQty;
                                //        s.UpdateDate = DateTime.Now;
                                //        s.IsUpdate = true;
                                //    }
                                //    unitOfWork.GetDbContext().Entry(s).State = System.Data.Entity.EntityState.Modified;
                                //}
                            }
                        }
                        InvoicePayment ExistingRefund = objInvoice.InvoicePayments.Where(p => p.Payment < 0).Select(p => p).FirstOrDefault();
                        if (ExistingRefund == null)
                        {
                            double refund = 0;
                            double.TryParse(txtRefundAmount.Text, out refund);
                            if (refund > 0)
                            {
                                InvoicePayment p = new InvoicePayment();
                                p.PaymentDate = dtpRedundDate.Value;
                                p.UpdatedAt = dtpRedundDate.Value;
                                p.IsActive = true;
                                p.IsNew = true;
                                p.Payment = -1 * refund;
                                p.CreatedAt = DateTime.Now;
                                objInvoice.InvoicePayments.Add(p);
                            }
                        }
                        else
                        {
                            double refund = 0;
                            double.TryParse(txtRefundAmount.Text, out refund);
                            if (refund > 0)
                            {
                                ExistingRefund.IsUpdate = true;
                                ExistingRefund.IsSynced = false;
                                ExistingRefund.UpdatedAt = DateTime.Now;
                                ExistingRefund.Payment = -1 * refund;
                            }
                        }
                        //List<InvoicePayment> NewPaymentsMade = new List<InvoicePayment>();
                        //if (!AddInvoicePayments(NewPaymentsMade))
                        //{
                        //    return 0;
                        //}
                        //bool found = false;
                        //foreach (InvoicePayment p in NewPaymentsMade)
                        //{
                        //    //found = false;
                        //    //foreach (InvoicePayment p2 in objInvoice.InvoicePayments)
                        //    //{
                        //    //    if (p.InvoicePaymentId == p2.InvoicePaymentId && p.InvoicePaymentId > 0)
                        //    //    {
                        //    //        found = true;
                        //    //        p2.Payment = p.Payment;
                        //    //        p.IsUpdate = true;
                        //    //        p.UpdateDate = DateTime.Now;
                        //    //        unitOfWork.GetDbContext().Entry(p2).State = System.Data.Entity.EntityState.Modified;
                        //    //    }
                        //    //}
                        //    //if (!found)
                        //    //{
                        //    //    objInvoice.InvoicePayments.Add(p);
                        //    //}
                        //    if (p.Payment < 0)
                        //    {
                        //        objInvoice.InvoicePayments.Add(p);
                        //    }
                        //}
                        //if (double.Parse(lblDue.Text) > 0)
                        //{
                        //    MessageBox.Show("Due Amount is (" + lblDue.Text.ToString() + " ). You Can't Insert Invoice With Due Payments.", "Due Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return;
                        //}
                        if (PatientId > 0)
                        {
                            objInvoice.Patient = unitOfWork.PatientRepository.GetById(PatientId);
                        }
                        //foreach (DataRow r in dtStockConsumption.Rows)
                        //{
                        //    r["Invoice_InvoiceId"] = this.InvoiceEditId;
                        //}
                        //unitOfWork.StockConsumptionRespository.BulkInsert(dtStockConsumption);
                        unitOfWork.InvoiceRepository.Update(objInvoice);
                        unitOfWork.Save();
                        scope.Complete();
                        if (ShowSuccessMsg)
                        {
                            MessageBox.Show("Invoice Refund Saved Successfully", "Refund Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return this.InvoiceEditId;
                    }
                }
                catch (Exception ex)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                    scope.Dispose();
                    return 0;
                }
            }
        }
        private bool AddInvoicePayments(Invoice objInvoice)
        {
            objInvoice.InvoicePayments = new List<InvoicePayment>();
            //if (numPmnt1.Value <= 0)
            //{
            //    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            InvoicePayment objIp = new InvoicePayment();
            objIp.InvoicePaymentId = int.Parse(txtInvPmntId1.Text);
            objIp.PaymentType = cmbPmntType1.SelectedIndex + 1;
            objIp.Payment = (double)numPmnt1.Value;
            objIp.PaymentDate = dtpPmntType1.Value;
            objIp.IsNew = true;
            objIp.IsActive = true;
            objIp.CreatedAt = DateTime.Now;
            objIp.UpdatedAt = DateTime.Now;
            if (objIp.PaymentType == 2)//cheque selected
            {
                if (txtChequeInfoId1.Text != "")
                {
                    long chequeId = Convert.ToInt64(txtChequeInfoId1.Text);
                    if (chequeId > 0)
                    {
                        //objIp.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    }
                }
                else
                {
                    DialogResult rs = MessageBox.Show("Cheque Details Not Found For First Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs != DialogResult.OK)
                    {
                        return false;
                    }
                }
            }
            objInvoice.InvoicePayments.Add(objIp);

            if (chkPmnt2.Checked)
            {
                if (numPmnt2.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp2 = new InvoicePayment();
                objIp.InvoicePaymentId = int.Parse(txtInvPmntId2.Text);
                objIp2.PaymentType = cmbPmntType2.SelectedIndex + 1;
                objIp2.Payment = (double)numPmnt2.Value;
                objIp2.PaymentDate = dtpPmntType2.Value;
                objIp2.IsNew = true;
                objIp2.IsActive = true;
                objIp2.CreatedAt = DateTime.Now;
                objIp2.UpdatedAt = DateTime.Now;
                if (objIp2.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId2.Text != "")
                    {
                        long chequeId = Convert.ToInt64(txtChequeInfoId2.Text);
                        if (chequeId > 0)
                        {
                            //objIp2.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                        }
                    }
                    else
                    {
                        DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }
                objInvoice.InvoicePayments.Add(objIp2);
            }
            if (chkPmnt3.Checked)
            {
                if (numPmnt3.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp3 = new InvoicePayment();
                objIp.InvoicePaymentId = int.Parse(txtInvPmntId3.Text);
                objIp3.PaymentType = cmbPmntType3.SelectedIndex + 1;
                objIp3.Payment = (double)numPmnt3.Value;
                objIp3.PaymentDate = dtpPmntType3.Value;
                objIp3.IsNew = true;
                objIp3.IsActive = true;
                objIp3.CreatedAt = DateTime.Now;
                objIp3.UpdatedAt = DateTime.Now;
                if (objIp3.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId3.Text != "")
                    {
                        long chequeId = Convert.ToInt64(txtChequeInfoId3.Text);
                        if (chequeId > 0)
                        {
                            //objIp3.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                        }
                    }
                    else
                    {
                        DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }
                objInvoice.InvoicePayments.Add(objIp3);
            }

            if (chkPmnt4.Checked)
            {
                if (numPmnt4.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp4 = new InvoicePayment();
                objIp.InvoicePaymentId = int.Parse(txtInvPmntId4.Text);
                objIp4.PaymentType = cmbPmntType4.SelectedIndex + 1;
                objIp4.Payment = (double)numPmnt4.Value;
                objIp4.PaymentDate = dtpPmntType4.Value;
                objIp4.IsNew = true;
                objIp4.IsActive = true;
                objIp4.CreatedAt = DateTime.Now;
                objIp4.UpdatedAt = DateTime.Now;
                if (objIp4.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId4.Text != "")
                    {
                        long chequeId = Convert.ToInt64(txtChequeInfoId4.Text);
                        if (chequeId > 0)
                        {
                            //objIp4.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                        }
                    }
                    else
                    {
                        DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }
                objInvoice.InvoicePayments.Add(objIp4);
            }
            if (chkPmnt5.Checked)
            {
                if (numPmnt5.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp5 = new InvoicePayment();
                objIp.InvoicePaymentId = int.Parse(txtInvPmntId5.Text);
                objIp5.PaymentType = cmbPmntType5.SelectedIndex + 1;
                objIp5.Payment = (double)numPmnt5.Value;
                objIp5.PaymentDate = dtpPmntType5.Value;
                objIp5.IsNew = true;
                objIp5.IsActive = true;
                objIp5.CreatedAt = DateTime.Now;
                objIp5.UpdatedAt = DateTime.Now;
                if (objIp5.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId5.Text != "")
                    {
                        long chequeId = Convert.ToInt64(txtChequeInfoId5.Text);
                        if (chequeId > 0)
                        {
                            //objIp5.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                        }
                    }
                    else
                    {
                        DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }
                objInvoice.InvoicePayments.Add(objIp5);
            }
            return true;
        }
        private bool AddInvoicePayments(List<InvoicePayment> PaymentsList)
        {
            //if (numPmnt1.Value <= 0)
            //{
            //    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}

            if (IsInvoiceRefund)// this is set true if invoice is loaded from return button
            {
                InvoicePayment objIp6 = new InvoicePayment();
                objIp6.PaymentType = 1;
                objIp6.RefundReason = txtRefundReason.Text.Trim();
                objIp6.Payment = txtCurrentRefund.Text == "" ? 0 : -double.Parse(txtCurrentRefund.Text);
                objIp6.PaymentDate = dtpRedundDate.Value;
                objIp6.IsNew = true;
                objIp6.IsActive = true;
                objIp6.CreatedAt = DateTime.Now;
                objIp6.UpdatedAt = DateTime.Now;
                PaymentsList.Add(objIp6);
                return true;
            }
            InvoicePayment objIp = new InvoicePayment();
            objIp.InvoicePaymentId = int.Parse(txtInvPmntId1.Text);
            objIp.PaymentType = cmbPmntType1.SelectedIndex + 1;
            objIp.Payment = (double)numPmnt1.Value;
            objIp.PaymentDate = dtpPmntType1.Value;
            objIp.IsNew = true;
            objIp.IsActive = true;
            objIp.CreatedAt = DateTime.Now;
            objIp.UpdatedAt = DateTime.Now;
            if (objIp.PaymentType == 2)//cheque selected
            {
                if (txtChequeInfoId1.Text != "")
                {
                    long chequeId = Convert.ToInt64(txtChequeInfoId1.Text);
                    if (chequeId > 0)
                    {
                        //objIp.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    }
                }
                else
                {
                    DialogResult rs = MessageBox.Show("Cheque Details Not Found For First Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs != DialogResult.OK)
                    {
                        return false;
                    }
                }
            }
            PaymentsList.Add(objIp);

            if (chkPmnt2.Checked)
            {
                if (numPmnt2.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp2 = new InvoicePayment();
                objIp2.InvoicePaymentId = int.Parse(txtInvPmntId2.Text);
                objIp2.PaymentType = cmbPmntType2.SelectedIndex + 1;
                objIp2.Payment = (double)numPmnt2.Value;
                objIp2.PaymentDate = dtpPmntType2.Value;
                objIp2.IsNew = true;
                objIp2.IsActive = true;
                objIp2.CreatedAt = DateTime.Now;
                objIp2.UpdatedAt = DateTime.Now;
                if (objIp2.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId2.Text != "")
                    {
                        long chequeId = Convert.ToInt64(txtChequeInfoId2.Text);
                        if (chequeId > 0)
                        {
                           // objIp2.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                        }
                    }
                    else
                    {
                        DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }
                PaymentsList.Add(objIp2);
            }

            if (chkPmnt3.Checked)
            {
                if (numPmnt3.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp3 = new InvoicePayment();
                objIp3.InvoicePaymentId = int.Parse(txtInvPmntId3.Text);
                objIp3.PaymentType = cmbPmntType3.SelectedIndex + 1;
                objIp3.Payment = (double)numPmnt3.Value;
                objIp3.PaymentDate = dtpPmntType3.Value;
                objIp3.IsNew = true;
                objIp3.IsActive = true;
                objIp3.CreatedAt = DateTime.Now;
                objIp3.UpdatedAt = DateTime.Now;
                if (objIp3.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId3.Text != "")
                    {
                        long chequeId = Convert.ToInt64(txtChequeInfoId3.Text);
                        if (chequeId > 0)
                        {
                            //objIp3.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                        }
                    }
                    else
                    {
                        DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }
                PaymentsList.Add(objIp3);
            }

            if (chkPmnt4.Checked)
            {
                if (numPmnt4.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp4 = new InvoicePayment();
                objIp4.InvoicePaymentId = int.Parse(txtInvPmntId4.Text);
                objIp4.PaymentType = cmbPmntType4.SelectedIndex + 1;
                objIp4.Payment = (double)numPmnt4.Value;
                objIp4.PaymentDate = dtpPmntType4.Value;
                objIp4.IsNew = true;
                objIp4.IsActive = true;
                objIp4.CreatedAt = DateTime.Now;
                objIp4.UpdatedAt = DateTime.Now;
                if (objIp4.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId4.Text != "")
                    {
                        long chequeId = Convert.ToInt64(txtChequeInfoId4.Text);
                        if (chequeId > 0)
                        {
                           // objIp4.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                        }
                    }
                    else
                    {
                        DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }
                PaymentsList.Add(objIp4);
            }
            if (chkPmnt5.Checked)
            {
                if (numPmnt5.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp5 = new InvoicePayment();
                objIp5.InvoicePaymentId = int.Parse(txtInvPmntId5.Text);
                objIp5.PaymentType = cmbPmntType5.SelectedIndex + 1;
                objIp5.Payment = (double)numPmnt5.Value;
                objIp5.PaymentDate = dtpPmntType5.Value;
                objIp5.IsNew = true;
                objIp5.IsActive = true;
                objIp5.CreatedAt = DateTime.Now;
                objIp5.UpdatedAt = DateTime.Now;
                if (objIp5.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId5.Text != "")
                    {
                        long chequeId = Convert.ToInt64(txtChequeInfoId5.Text);
                        if (chequeId > 0)
                        {
                            //objIp5.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                        }
                    }
                    else
                    {
                        DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }
                PaymentsList.Add(objIp5);
            }
            return true;
        }
        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            pnlPmnt2.Visible = true;
        }
        private void chkPmnt2_CheckedChanged(object sender, EventArgs e)
        {
            //pnlPmnt2.Enabled = chkPmnt2.Checked;
            if (chkPmnt3.Checked)
            {
                chkPmnt2.Checked = true;
                return;
            }
            if (chkPmnt2.Checked)
            {
                if (numPmnt1.Value <= 0)
                {
                    if (!pnlPmnt1.Enabled)
                    {

                    }
                    else
                    {
                        chkPmnt2.Checked = false;
                        ToolTip tt = new ToolTip();
                        tt.Show("Enter Payment Here.", numPmnt1, 1000);
                        return;
                    }
                }
                chkPmnt3.Visible = true;
                pnlPmnt2.Visible = true;
            }
            else
            {
                pnlPmnt2.Visible = false;

                chkPmnt3.Checked = false;
                chkPmnt3.Visible = false;
                pnlPmnt3.Visible = false;

                chkPmnt4.Checked = false;
                chkPmnt4.Visible = false;
                pnlPmnt4.Visible = false;

                long pmntId = 0;
                long.TryParse(txtInvPmntId2.Text, out pmntId);
                if (pmntId > 0)
                {
                    DeletedPayments.Add(new InvoicePayment { InvoicePaymentId = pmntId });
                }

                numPmnt2.Value = 0;
                dtpPmntType2.Value = DateTime.Now;
                txtChequeInfoId2.Text = "";
                txtInvPmntId2.Text = "0";

            }
        }
        private void chkPmnt3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPmnt4.Checked)
            {
                chkPmnt3.Checked = true;
                return;
            }
            if (numPmnt2.Value <= 0)
            {
                if (!pnlPmnt2.Enabled) { }
                else
                {
                    chkPmnt3.Checked = false;
                    ToolTip tt = new ToolTip();
                    tt.Show("Enter Payment Here.", numPmnt2, 1000);
                    return;
                }
            }
            //pnlPmnt3.Enabled = chkPmnt3.Checked;
            if (chkPmnt3.Checked)
            {
                chkPmnt4.Visible = true;
                pnlPmnt3.Visible = true;
            }
            else
            {
                pnlPmnt3.Visible = false;
                chkPmnt4.Checked = false;
                chkPmnt4.Visible = false;
                pnlPmnt4.Visible = false;


                long pmntId = 0;
                long.TryParse(txtInvPmntId3.Text, out pmntId);
                if (pmntId > 0)
                {
                    DeletedPayments.Add(new InvoicePayment { InvoicePaymentId = pmntId });
                }

                numPmnt3.Value = 0;
                dtpPmntType3.Value = DateTime.Now;
                txtChequeInfoId3.Text = "";
                txtInvPmntId3.Text = "0";
            }
        }
        private void chkPmnt4_CheckedChanged(object sender, EventArgs e)
        {

            if (chkPmnt5.Checked)
            {
                chkPmnt4.Checked = true;
                return;
            }
            if (numPmnt3.Value <= 0)
            {
                if (!pnlPmnt3.Enabled) { }
                else
                {
                    chkPmnt4.Checked = false;
                    ToolTip tt = new ToolTip();
                    tt.Show("Enter Payment Here.", numPmnt2, 1000);
                    return;
                }
            }
            //pnlPmnt3.Enabled = chkPmnt3.Checked;
            if (chkPmnt4.Checked)
            {
                chkPmnt5.Visible = true;
                pnlPmnt4.Visible = true;
            }
            else
            {
                pnlPmnt4.Visible = false;
                chkPmnt5.Checked = false;
                chkPmnt5.Visible = false;
                pnlPmnt5.Visible = false;

                long pmntId = 0;
                long.TryParse(txtInvPmntId4.Text, out pmntId);
                if (pmntId > 0)
                {
                    DeletedPayments.Add(new InvoicePayment { InvoicePaymentId = pmntId });
                }

                numPmnt4.Value = 0;
                dtpPmntType4.Value = DateTime.Now;
                txtChequeInfoId4.Text = "";
                txtInvPmntId4.Text = "0";
            }
            ////////////////////////////////

        }
        private void chkPmnt5_CheckedChanged(object sender, EventArgs e)
        {
            if (numPmnt4.Value <= 0)
            {
                if (!pnlPmnt4.Enabled) { }
                else
                {
                    chkPmnt5.Checked = false;
                    ToolTip tt = new ToolTip();
                    tt.Show("Enter Payment Here.", numPmnt3, 1000);
                    return;
                }
            }
            pnlPmnt5.Visible = chkPmnt5.Checked;
            if (!chkPmnt5.Checked)
            {
                long pmntId = 0;
                long.TryParse(txtInvPmntId5.Text, out pmntId);
                if (pmntId > 0)
                {
                    DeletedPayments.Add(new InvoicePayment { InvoicePaymentId = pmntId });
                }

                numPmnt5.Value = 0;
                dtpPmntType5.Value = DateTime.Now;
                txtChequeInfoId5.Text = "";
                txtInvPmntId5.Text = "0";
            }
        }
        private void numDiscount_ValueChanged(object sender, EventArgs e)
        {
            //numTotalDiscount.Enabled = false;
            if (IsCorrectItemDiscount())
            {
                CalculateItemTotals();
                //numTotalDiscount.Value = 0;
                //numModifiedDiscount.Value = 0;
                //CalculateTotals();
            }
            else
            {
                MessageBox.Show("You are giving more thatn 100 % discount, Please tet it to tess than er equal 100 %", "Incorrect Discount", MessageBoxButtons.OK, MessageBoxIcon.Information);
               // numDiscount.Value = 100;
                //cmbDiscountType.SelectedIndex = 1;
            }
        }
        private bool IsCorrectItemDiscount()
        {
            int discountType = cmbDiscountType.SelectedIndex + 1;
            double Disc = (double)numDiscount.Value;
            double Amount = 0;
            double.TryParse(txtAmount.Text.Trim(), out Amount);
            if (discountType == 2)
            {
                Disc = (Disc / 100) * Amount;
            }

            if (Disc > Amount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CalculateItemTotals()
        {
            double rate = (double)numRate.Value;
            int qty = (int)numQuantity.Value;
            double amount = rate * qty;
            double discount = (double)numDiscount.Value;
            string discountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
            double Refund = 0;
            Refund = txtCurrentRefund.Text == "" ? 0 : double.Parse(txtCurrentRefund.Text);
            if (discountType.ToLower() == "%")
            {
                discount = (discount / 100) * amount;
            }
            txtAmount.Text = amount.ToString();
            txtNetAmount.Text = (amount - discount).ToString();
            //if (IsDiscountChanged)
            // {            
            //}            
            //numTotalDiscount.Value = 0;
            //numModifiedDiscount.Value = 0;
            //CalculateTotals();
        }

        private void numRate_ValueChanged(object sender, EventArgs e)
        {
            CalculateItemTotals();   
        }
        private void cmbDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //numTotalDiscount.Enabled = false;
            if (IsCorrectItemDiscount())
            {
                CalculateItemTotals();
                //numTotalDiscount.Value = 0;
                //numModifiedDiscount.Value = 0;
                //CalculateTotals();
            }
            else
            {
                MessageBox.Show("You are giving more thatn 100 % discount, Please tet it to tess than er equal 100 %", "Incorrect Discount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //numDiscount.Value = 100;
               //cmbDiscountType.SelectedIndex = 1;
            }
        }
        private long showChequeInfoDialog()
        {
            //frmChequeInfo f = new frmChequeInfo();
            //f.ShowDialog();
            //long ChequeInfoId = f.ChequeInfoId;
            //return ChequeInfoId;
            return 0;
        }
        private void cmbPmntType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPmntType1.SelectedIndex == 1)
            {
                long id = showChequeInfoDialog();
                txtChequeInfoId1.Text = id == 0 ? "" : id.ToString();
            }
        }
        private void cmbPmntType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPmntType2.SelectedIndex == 1)
            {
                long id = showChequeInfoDialog();
                txtChequeInfoId2.Text = id == 0 ? "" : id.ToString();
            }
        }
        private void cmbPmntType3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPmntType3.SelectedIndex == 1)
            {
                long id = showChequeInfoDialog();
                txtChequeInfoId3.Text = id == 0 ? "" : id.ToString();
            }
        }
        private void cmbPmntType4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPmntType4.SelectedIndex == 1)
            {
                long id = showChequeInfoDialog();
                txtChequeInfoId4.Text = id == 0 ? "" : id.ToString();
            }
        }
        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                //if (Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value) > 0)
                //{
                //    IsDiscountByItem = true;
                //    numTotalDiscount.Enabled = false;
                //    cmbTotalDiscountType.Enabled = false;
                //}
                CalculateTotals(grdItems.Rows[e.RowIndex]);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void grdItems_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            bool discountFound = false;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToDouble(r.Cells["colDiscount"].Value) > 0)
                {
                    discountFound = true;
                }
            }
            if (!discountFound)
            {
                numTotalDiscount.Enabled = true;
                cmbTotalDiscountType.Enabled = true;
            }
            Tuple<double, double, double> GrandTotals = GetGrandTotals();
            lblSubTotal.Text = Math.Round(GrandTotals.Item1, 2).ToString();
            lblGrandTotal.Text = Math.Round(GrandTotals.Item2, 2).ToString();
            if (!this.IsInvoiceRefund && numPmnt1.Enabled)
            {
                numPmnt1.Value = (decimal)Math.Round(GrandTotals.Item2, 2);
            }
            numTotalDiscount.Value = (decimal)GrandTotals.Item3;
            numModifiedDiscount.Value = (decimal)GrandTotals.Item3;
            GetCustomerTotals();
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.I))
            {
                cmbProcedure.Focus();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.A))
            {
                btnAddItem.PerformClick();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.G))
            {
                grdItems.Focus();
                if (grdItems.Rows.Count >= 1)
                {
                    grdItems.Rows[0].Selected = true;
                }
                return true;
            }
            if (keyData == (Keys.Alt| Keys.D))
            {
                btnClear.PerformClick();
                return true;
            }

            if (keyData == (Keys.Alt | Keys.X))
            {
                numTotalDiscount.Focus();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.A))
            {
                btnAddNewProcedure.PerformClick();
                return true;
            }            

            if (keyData == (Keys.Control | Keys.S))
            {
                btnSave.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.P))
            {
                btnSaveAndPrint.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.D))
            {
                btnClearAll.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.N))
            {
                txtSearchByName.Focus();
                return true;
            }
            #region grid short cuts
            if (keyData == (Keys.Alt | Keys.E))
            {
                if (grdItems.SelectedRows.Count > 0)
                {
                    int colIndex = grdItems.Columns["colEdit"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);                    
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.R))
            {
                {
                    int colIndex = grdItems.Columns["colRemove"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void Payments_ValueChanged(object sender, EventArgs e)
        {
            GetCustomerTotals();
        }
        private void GetCustomerTotals()
        {
            double Receiveable = double.Parse(lblGrandTotal.Text);
            double payments = (double)(numPmnt1.Value + numPmnt2.Value + numPmnt3.Value + numPmnt4.Value + numPmnt5.Value);
            double advance = 0;
            double due = 0;

            if (Receiveable - payments >= 0)
            {
                due = Receiveable - payments;
                advance = 0;
            }
            else if (payments - Receiveable >= 0)
            {
                advance = payments - Receiveable;
                due = 0;
            }
            //if(due > 0)
            //{
            //    pnlDue.Visible = true;
            //}
            lblDue.Text = due.ToString();
            lblAdvance.Text = advance.ToString();
        }
        private void numTotalDiscount_ValueChanged(object sender, EventArgs e)
        {
            if(grdItems.Rows.Count <=0)
            { return; }
            if (isCorrectTotalDiscount())
            {
                if (numTotalDiscount.Enabled)
                {
                    GetTotalsByTotalDiscountChanges();
                }
            }
        }

        private bool isCorrectTotalDiscount()
        {
            double SubTotal = double.Parse(lblSubTotal.Text);
            int discType = cmbTotalDiscountType.SelectedIndex + 1;
            double TotalDisc = (double)numTotalDiscount.Value;
            if (discType == 2) // percent
            {
                double val = (TotalDisc / 100); 
                TotalDisc = val * SubTotal;
            }
            if (TotalDisc > SubTotal)
            {
                ToolTip tt = new ToolTip();
                MessageBox.Show("Incorrect total discount value", "Incorrect discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numTotalDiscount.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }
        private void cmbTotalDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count <= 0)
            { return; }
            if (isCorrectTotalDiscount())
            {
                if (numTotalDiscount.Enabled)
                {
                    GetTotalsByTotalDiscountChanges();
                }
            }
        }
        private void GetTotalsByTotalDiscountChanges()
        {
            Tuple<double, double, double> GrandTotals = GetGrandTotals();
            double SubTotal = GrandTotals.Item1;
            double GrandTotal = GrandTotals.Item2;
            double discount = (double)numTotalDiscount.Value;
            if (cmbTotalDiscountType.SelectedIndex == 1)
            {
                discount = (discount / 100) * SubTotal;
            }
            lblSubTotal.Text = Math.Round(SubTotal, 2).ToString();
            if (SubTotal == 0)
            {
                lblGrandTotal.Text = (0).ToString();
                if (!this.IsInvoiceRefund && chkPmnt1.Enabled)
                {
                    numPmnt1.Value = 0;
                }
            }
            else
            {
                lblGrandTotal.Text = Math.Round((SubTotal - discount), 2).ToString();
                if (numPmnt1.Enabled && this.LoadedInvoice_TotalPaid == 0)
                {
                    numPmnt1.Value = (decimal)Math.Round((SubTotal - discount), 2);
                }
            }
            GetCustomerTotals();
        }        
        private void txtSearchPatient_KeyDown(object sender, KeyEventArgs e)
        {
            ShowPatientData();
        }
        private void txtSearchPatient_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowPatientData();
        }
        private void ShowPatientData()
        {
            frmSearchPatient f = new frmSearchPatient();
            f.ShowDialog();
            if (f.PatientId > 0)
            {
                PatientId = f.PatientId;
                txtSearchPatient.Text = f.PatientName;
            }
        }
        private void ResetPayments()
        {
            pnlPmnt1.Enabled = true;
            pnlPmnt2.Enabled = true;
            pnlPmnt3.Enabled = true;
            pnlPmnt4.Enabled = true;
            pnlPmnt5.Enabled = true;

            chkPmnt2.Enabled = true;
            chkPmnt3.Enabled = true;
            chkPmnt4.Enabled = true;
            chkPmnt5.Enabled = true;

            pnlPmnt2.Visible = false;
            pnlPmnt3.Visible = false;
            pnlPmnt4.Visible = false;
            pnlPmnt5.Visible = false;

            chkPmnt3.Visible = false;
            chkPmnt4.Visible = false;
            chkPmnt5.Visible = false;

            dtpPmntType1.Value = DateTime.Now;
            dtpPmntType2.Value = DateTime.Now;
            dtpPmntType3.Value = DateTime.Now;
            dtpPmntType4.Value = DateTime.Now;
            dtpPmntType5.Value = DateTime.Now;

            cmbPmntType1.SelectedIndex = 0;
            cmbPmntType2.SelectedIndex = 0;
            cmbPmntType3.SelectedIndex = 0;
            cmbPmntType4.SelectedIndex = 0;
            cmbPmntType5.SelectedIndex = 0;

            numPmnt1.Value = 0;
            numPmnt2.Value = 0;
            numPmnt3.Value = 0;
            numPmnt4.Value = 0;
            numPmnt5.Value = 0;

            txtChequeInfoId1.Text = "";
            txtChequeInfoId2.Text = "";
            txtChequeInfoId3.Text = "";
            txtChequeInfoId4.Text = "";
            txtChequeInfoId5.Text = "";

            txtInvPmntId1.Text = "0";
            txtInvPmntId2.Text = "0";
            txtInvPmntId3.Text = "0";
            txtInvPmntId4.Text = "0";
            txtInvPmntId5.Text = "0";

            chkPmnt5.Checked = false;
            chkPmnt4.Checked = false;
            chkPmnt3.Checked = false;
            chkPmnt2.Checked = false;
        }
        private void ClearAll(bool isAfterSave)
        {
            if (grdItems.Rows.Count > 0)
            {
                DialogResult rs = new DialogResult();
                if (isAfterSave)
                {
                    rs = DialogResult.OK;
                }
                else
                {
                    rs = MessageBox.Show("There are Some Unsaved Items in Grid, Are You Sure You Want To Clear", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                }
                if (rs == DialogResult.OK)
                {
                    Clear();
                    grdItems.Rows.Clear();
                    PatientId = 0;
                    lblGrandTotal.Text = "0";
                    lblAdvance.Text = "0";
                    lblDue.Text = "0";
                    lblSubTotal.Text = "0";
                    dtStockConsumption.Rows.Clear();
                    dtStockConsumption.AcceptChanges();
                    txtSearchPatient.Text = "";
                    InvoiceEditId = 0;
                    IsInvoiceEdit = false;
                    IsInvoiceRefund = false;
                    IsDiscountByItem = false;
                    EditingRowIndex = -1;
                    IsNameSearching = false;
                    IsPhoneSearching = false;
                    numDiscount.Value = 0;
                    numTotalDiscount.Value = 0;
                    cmbTotalDiscountType.SelectedIndex = 0;
                    //numModifiedDiscount.Value = 0;
                    //patient details controls clear
                    txtSearchByName.Text = "";
                    txtSearchByPhone.Text = "";
                    txtAddress.Text = "";

                    ResetPayments();

                    pnlPatientDetails.Enabled = true;

                    grdItems.Columns["colEdit"].Visible = true;
                    grdItems.Columns["colRemove"].Visible = true;
                    grdItems.Columns["colReturnToggle"].Visible = false;

                    IsReturnedQuantityFound = false;
                    DeletedPayments = new List<InvoicePayment>();
                    unitOfWork = new UnitOfWork();
                    DeletedInvoiceItems = new List<InvoiceItemVM>();
                    txtInvoiceNote.Text = "";
                    LoadedInvoice_TotalPaid = 0;
                }
            }
        }
        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddPatient(), this.MdiParent);
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            try
            {
                LoadItems();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Refershing Items, Please Try Reopen Sale Form.", ex.Message, "Error");
            }
        }
        private void grdItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == grdItems.Columns["colReturnQuantity"].Index)
                {
                    int Qty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);
                    var r = grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value;
                    int RQty = r == null || r.ToString() == "" ? 0 : Convert.ToInt32(r);
                    grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = RQty; // this to avoid funrther checks on calculateFormuale
                    if (RQty > Qty)
                    {
                        MessageBox.Show("Return Quantity Can't be Greater Than Sale Quantity", "Invalid Return", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = Qty;
                        grdItems.CellValueChanged += grdItems_CellValueChanged;
                        return;
                    }
                    CalculateRefundAmount2();
                }
            }
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }
        private void CalculateRefundAmount()
        {
            double totalRefundAmount = 0;
            double GrandTotal = double.Parse(lblGrandTotal.Text);
            double SubTotal = double.Parse(lblSubTotal.Text);

            double Rate = 0;
            double ItemTotal = 0;
            double ItemReturnTotal = 0;
            double ItemReturnGrandTotal = 0;
            double DiscAmt = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                int rtQty = Convert.ToInt32(r.Cells["colReturnQuantity"].Value);
                int SaleQty = Convert.ToInt32(r.Cells["colQuantity"].Value);
                if (rtQty <= 0)
                {
                    continue;
                }
                Rate = Convert.ToInt32(r.Cells["colRate"].Value);
                ItemTotal = Convert.ToDouble(r.Cells["colAmount"].Value);
                ItemReturnTotal = rtQty * Rate;
                ItemReturnGrandTotal += ItemReturnTotal;
                DiscAmt = 0;
                if (IsDiscountByItem)
                {
                    int DiscountType = Convert.ToInt32(r.Cells["colDiscountTypeId"].Value);
                    DiscAmt = Convert.ToDouble(r.Cells["colDiscount"].Value);
                    if (DiscountType == 2)
                    {
                        DiscAmt = (DiscAmt / 100) * ItemTotal;
                    }
                    DiscAmt = (DiscAmt / SaleQty) * rtQty;
                }
                else
                {
                    DiscAmt = GetGrandDiscount();
                    double Ratio = ItemTotal / SubTotal;
                    DiscAmt = ((DiscAmt * Ratio) / SaleQty) * rtQty;
                }
                double RefundAmount = ItemReturnTotal - DiscAmt;
                totalRefundAmount += RefundAmount;
            }
            double GDisc = 0;
            GDisc = GetGrandDiscount();
            GrandTotal = SubTotal - GDisc - totalRefundAmount;
            lblGrandTotal.Text = Math.Round(GrandTotal, 2).ToString();
            double CurrentRefund = totalRefundAmount - InitialTotalRefundValue;
            txtCurrentRefund.Text = Math.Round(CurrentRefund, 2).ToString();
            txtRefundAmount.Text = Math.Round(totalRefundAmount, 2).ToString();
            double due = GrandTotal - GetPaymentsSum();

            if (due < 0)
            {
                lblAdvance.Text = Math.Round((-1 * due), 2).ToString();
                lblDue.Text = "0";
            }
            else
            {
                lblAdvance.Text = "0";
                lblDue.Text = Math.Round(due, 2).ToString();
            }
        }
        private void CalculateRefundAmount2()
        {
            //_WOD : Without Deductions
            double GrandTotal_WOD = double.Parse(lblSubTotal.Text.ToString()) - GetGrandDiscount();

            double TotalPaid = GetPaymentsSum();
            double totalRefundAmount = 0;
            double totalRefundAmountWP = 0;
            double GrandTotal = double.Parse(lblGrandTotal.Text);
            double SubTotal = double.Parse(lblSubTotal.Text);


            double RetQtydiscount = 0;

            double Rate = 0;
            double ItemTotal = 0;
            double ItemReturnTotal = 0;
            double ItemReturnTotalWP = 0; // WP : for when customer has paid some amount prior to returning
            double ItemReturnGrandTotalWP = 0; // WP : for when customer has paid some amount prior to returning
            double ItemReturnGrandTotal = 0;
            double DiscAmt = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                int rtQty = Convert.ToInt32(r.Cells["colReturnQuantity"].Value);
                int SaleQty = Convert.ToInt32(r.Cells["colQuantity"].Value);
                if (rtQty <= 0)
                {
                    continue;
                }

                Rate = Convert.ToInt32(r.Cells["colRate"].Value);
                ItemTotal = Convert.ToDouble(r.Cells["colAmount"].Value);

                ItemReturnTotal = rtQty * Rate;
                ItemReturnGrandTotal += ItemReturnTotal;
                DiscAmt = 0;
                if (IsDiscountByItem)
                {

                    int DiscountType = Convert.ToInt32(r.Cells["colDiscountTypeId"].Value);
                    DiscAmt = Convert.ToDouble(r.Cells["colDiscount"].Value);
                    double NetAmount = Convert.ToDouble(r.Cells["colNetAmount"].Value);
                    if (DiscountType == 2)
                    {
                        DiscAmt = (DiscAmt / 100) * ItemTotal;
                    }
                    if (TotalPaid > 0) // only show refund if customer have paid some amount
                    {
                        // 4 rows commecnted at 2020-07-17 1626
                        //double Ratio = ((ItemTotal - DiscAmt) / GrandTotal_WOD);
                        //double Share = (Ratio * TotalPaid) / SaleQty;
                        //ItemReturnTotalWP = Share * rtQty;
                        //ItemReturnGrandTotalWP += ItemReturnTotalWP;
                        // 2 rows added at 2020-07-17 1626          
                        double Ratio = ((NetAmount) / GrandTotal_WOD);
                        double Share = (Ratio * TotalPaid) / SaleQty;
                        ItemReturnTotalWP = Share * rtQty;
                        ItemReturnGrandTotalWP += ItemReturnTotalWP;
                    }
                    DiscAmt = (DiscAmt / SaleQty) * rtQty;

                }
                else
                {
                    DiscAmt = GetGrandDiscount();
                    double Ratio = ItemTotal / SubTotal;
                    double DiscByLineItem = (DiscAmt * Ratio);
                    DiscAmt = ((DiscAmt * Ratio) / SaleQty) * rtQty;
                    if (TotalPaid > 0) // only show refund if customer have paid some amount
                    {
                        double Ratio2 = ((ItemTotal - DiscByLineItem) / GrandTotal_WOD);
                        double Share = (Ratio2 * TotalPaid) / SaleQty;
                        ItemReturnTotalWP = Share * rtQty;
                        ItemReturnGrandTotalWP += ItemReturnTotalWP;
                    }
                }
                //double RefundAmountWP = ItemReturnTotalWP + DiscAmt;
                double RefundAmountWP = ItemReturnTotalWP;
                double RefundAmount = ItemReturnTotal - DiscAmt;
                totalRefundAmount += RefundAmount;
                totalRefundAmountWP += RefundAmountWP;
                RetQtydiscount += DiscAmt; // this value contains value for discount share for returned item quantity.
            }
            double GDisc = 0;
            GDisc = GetGrandDiscount();
            GrandTotal = SubTotal - GDisc - totalRefundAmount;
            lblGrandTotal.Text = Math.Round(GrandTotal, 2).ToString();
            double CurrentRefund = totalRefundAmountWP - InitialTotalRefundValue;
            txtCurrentRefund.Text = Math.Round(CurrentRefund, 2).ToString();
            txtRefundAmount.Text = Math.Round(totalRefundAmountWP, 2).ToString();
            double due = GrandTotal - GetPaymentsSum();

            // gog here next
            numModifiedDiscount.Value = (decimal)(GDisc - RetQtydiscount);

            if (due < 0)
            {
                lblAdvance.Text = Math.Round((-1 * due), 2).ToString();
                lblDue.Text = "0";
            }
            else
            {
                lblAdvance.Text = "0";
                lblDue.Text = Math.Round(due, 2).ToString();
            }
        }
        private double GetPaymentsSum()
        {
            double TotalPaid = 0;
            TotalPaid = (double)(numPmnt1.Value + numPmnt2.Value + numPmnt3.Value + numPmnt4.Value + numPmnt5.Value);
            return TotalPaid;
        }
        private double GetGrandDiscount()
        {
            double DiscAmt = 0;
            double SubTotal = double.Parse(lblSubTotal.Text);
            int DiscountType = cmbTotalDiscountType.SelectedIndex + 1;
            DiscAmt = (double)numTotalDiscount.Value;
            if (DiscountType == 2)
            {
                DiscAmt = (DiscAmt / 100) * SubTotal;
            }
            return DiscAmt;
        }
        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colReturnQuantity"].Index)
            {
                e.Control.KeyPress += new KeyPressEventHandler(SharedFunctions.isValidIntegerKey);
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
        }
        private void grdSuggestion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadSearcherPatient();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                grdSuggestion.Visible = false;
            }
        }
        private void LoadSearcherPatient()
        {

            if (grdSuggestion.SelectedRows.Count > 0)
            {
                txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged;
                txtSearchByName.TextChanged -= txtSearchByName_TextChanged;
                this.PatientId = Convert.ToInt64(grdSuggestion.SelectedRows[0].Cells["colPatientId"].Value);
                this.txtSearchByName.Text = grdSuggestion.SelectedRows[0].Cells["colPatient"].Value.ToString();
                this.txtSearchByPhone.Text = grdSuggestion.SelectedRows[0].Cells["colPhone"].Value.ToString();
                grdSuggestion.Visible = false;
                txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged;
                txtSearchByName.TextChanged += txtSearchByName_TextChanged;
            }
        }
        private void txtSearchByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ShiftCtrlToSuggGrid();
            }
            //if (e.KeyCode == Keys.Enter)
            //{
            //    grdSuggestion.Rows.Clear();
            //    string searchString = txtSearchByName.Text.ToLower();
            //    if (searchString != "")
            //    {
            //        List<PatientSearchVM> Patients = unitOfWork.PatientRepository.SearchPatient(true, false, false, searchString);
            //        foreach (PatientSearchVM p in Patients)
            //        {
            //            grdSuggestion.Rows.Add(p.PatientId, p.PatientName);
            //        }
            //        if (Patients.Count > 0)
            //        {
            //            grdSuggestion.Location = new Point(txtSearchByName.Location.X, grdSuggestion.Location.Y);
            //            grdSuggestion.Visible = true;
            //            grdSuggestion.Focus();
            //            grdSuggestion.Rows[0].Selected = true;
            //        }
            //    }
            //}
        }
        private void txtSearchByName_TextChanged(object sender, EventArgs e)
        {
            grdSuggestion.Location = new Point(txtSearchByName.Location.X + 15, txtSearchByName.Location.Y + txtSearchByName.Height + 4);
            grdSuggestion.BringToFront();
            //if (txtSearchByPhone.Text != "") { txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged; txtSearchByPhone.Text = ""; txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged; }           
            grdSuggestion.Rows.Clear();
            string searchString = txtSearchByName.Text.ToLower();
            if (searchString != "")
            {
                IPagedList<PatientSearchVM> Patients = unitOfWork.PatientRepository.SearchPatient(true, false, false, searchString, 2);
                foreach (PatientSearchVM p in Patients)
                {
                    grdSuggestion.Rows.Add(p.PatientId, p.MrNo, p.PatientName, p.Phone);
                }
                if (Patients.Count > 0)
                {
                    grdSuggestion.Rows[0].Selected = false;
                    grdSuggestion.Visible = true;
                }
                else
                {
                    grdSuggestion.Visible = false;
                }
            }
            else
            {
                grdSuggestion.Visible = false;
            }
        }
        private void txtSearchByPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ShiftCtrlToSuggGrid();
            }
            //if (e.KeyCode == Keys.Enter)
            //{
            //    grdSuggestion.Rows.Clear();
            //    string searchString = txtSearchByName.Text.ToLower();
            //    if (searchString != "")
            //    {
            //        List<PatientSuggVM> Patients = unitOfWork.PatientRepository.SearchPatient(false, true, false, searchString);
            //        foreach (PatientSuggVM p in Patients)
            //        {
            //            grdSuggestion.Rows.Add(p.PatientId, p.Result);
            //        }
            //        if (Patients.Count > 0)
            //        {
            //            grdSuggestion.Location = new Point(txtSearchByPhone.Location.X, grdSuggestion.Location.Y);
            //            grdSuggestion.Visible = true;
            //            grdSuggestion.Focus();
            //            grdSuggestion.Rows[0].Selected = true;
            //        }
            //    }
            //}
        }
        private void txtSearchByPhone_TextChanged(object sender, EventArgs e)
        {
            grdSuggestion.Location = new Point(txtSearchByPhone.Location.X + 15, txtSearchByPhone.Location.Y + txtSearchByPhone.Height + 4);
            grdSuggestion.BringToFront();
            IsPhoneSearching = true;
            // if (txtSearchByName.Text != "") { txtSearchByName.TextChanged -= txtSearchByName_TextChanged; txtSearchByName.Text = ""; txtSearchByName.TextChanged += txtSearchByName_TextChanged; }
            grdSuggestion.Rows.Clear();
            string searchString = txtSearchByPhone.Text.ToLower();
            if (searchString != "")
            {
                IPagedList<PatientSearchVM> Patients = unitOfWork.PatientRepository.SearchPatient(false, true, false, searchString, 2);
                foreach (PatientSearchVM p in Patients)
                {
                    grdSuggestion.Rows.Add(p.PatientId, p.MrNo, p.PatientName, p.Phone);
                }
                if (Patients.Count > 0)
                {
                    grdSuggestion.Rows[0].Selected = false;
                    grdSuggestion.Visible = true;
                }
                else
                {
                    grdSuggestion.Visible = false;
                }
            }
            else
            {
                grdSuggestion.Visible = false;
            }
        }
        private void grdSuggestion_KeyPress(object sender, KeyPressEventArgs e)
        {
            // CHECKING FOR ALPHABET 
            TextBox TargetText = new TextBox();
            if (IsNameSearching) { TargetText = txtSearchByName; }
            else if (IsPhoneSearching) { TargetText = txtSearchByPhone; }
            if ((e.KeyChar >= 65 && e.KeyChar <= 90)
                || (e.KeyChar >= 97 && e.KeyChar <= 122) || e.KeyChar >= 48 && e.KeyChar <= 57)
            {
                TargetText.Focus();
                TargetText.Text += (char)e.KeyChar;
                TargetText.SelectionStart = txtSearchByName.Text.Length;
            }

            if (e.KeyChar == (char)Keys.Back)
            {
                TargetText.Focus();
                TargetText.SelectionStart = txtSearchByName.Text.Length;
                //txtSearchByName.Text = txtSearchByName.Text.Substring(txtSearchByName.Text.Length, 1);
                //txtSearchByName.SelectionStart = 0;
                //txtSearchByName.SelectionLength = 1;
            }
        }
        private void grdSuggestion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            LoadSearcherPatient();
        }
        private void txtSearchByPhone_Enter(object sender, EventArgs e)
        {
            grdSuggestion.Visible = false;
            grdSuggestion.BringToFront();
            grdSuggestion.Location = new Point(txtSearchByPhone.Location.X, txtSearchByPhone.Location.Y + txtSearchByPhone.Height);
        }
        private void txtSearchByPhone_Leave(object sender, EventArgs e)
        {
            //grdSuggestion.Visible = false;
        }
        private void txtSearchByName_Enter(object sender, EventArgs e)
        {
            grdSuggestion.Visible = false;
            grdSuggestion.BringToFront();
            grdSuggestion.Location = new Point(txtSearchByName.Location.X, txtSearchByPhone.Location.Y + txtSearchByName.Height);
        }
        private void txtSearchByName_Leave(object sender, EventArgs e)
        {
            //grdSuggestion.Visible = false;
        }
        private void ShiftCtrlToSuggGrid()
        {
            if (grdSuggestion.Rows.Count > 0)
            {
                grdSuggestion.Focus();
                grdSuggestion.Rows[0].Selected = true;
                //grdSuggestion.Rows[0].Selected = false;
                //grdSuggestion.Rows[1].Selected = true;
                //grdSuggestion.Rows[0].Selected = false;
            }
            //else if (grdSuggestion.Rows.Count > 0)
            //{
            //    grdSuggestion.Focus();
            //    grdSuggestion.Rows[0].Selected = true;
            //}
        }
        private void cmbPmntType5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPmntType5.SelectedIndex == 1)
            {
                long id = showChequeInfoDialog();
                txtChequeInfoId5.Text = id == 0 ? "" : id.ToString();
            }
        }
        private void numPmnt5_ValueChanged(object sender, EventArgs e)
        {
            GetCustomerTotals();
        }
        private void btnSaveAndPrint_Click(object sender, EventArgs e)
        {
            long invoiceid = SaveInvoice(false);
            if (invoiceid > 0)
            {
                //Reports.Pharmacy.PatientInvoiceViewer v = new Reports.Pharmacy.PatientInvoiceViewer(invoiceid);
                Reports.ProcedureInvoices.ProcedureInvoiceViewer v = new Reports.ProcedureInvoices.ProcedureInvoiceViewer(invoiceid, false);
                v.Show();
            }
        }
        private void cmbSelectItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void lblDue_TextChanged(object sender, EventArgs e)
        {
            //if(double.Parse(lblDue.Text) == 0)
            //{
            //    pnlDue.Visible = false;
            //}
            //else
            //{
            //    pnlDue.Visible = true;
            //}
        }
        private void cmbSelectBatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void btnAddNewItem_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmItems(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void btnAddStock_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddStock(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void numQuantity_Enter(object sender, EventArgs e)
        {
            numQuantity.Select(0, numQuantity.Value.ToString().Length);
        }

        private void numRate_Enter(object sender, EventArgs e)
        {
            numRate.Select(0, numRate.Value.ToString().Length);
        }

        private void numDiscount_Enter(object sender, EventArgs e)
        {
            numDiscount.Select(0, numDiscount.Value.ToString().Length);
        }

        private void numTotalDiscount_Enter(object sender, EventArgs e)
        {
            numTotalDiscount.Select(0, numTotalDiscount.Value.ToString().Length);
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmInvoiceShortCusts f = new ShortCutDialogs.frmInvoiceShortCusts();
            f.ShowDialog();
        }

        private void numTotalDiscount_Leave(object sender, EventArgs e)
        {
            // stop user from leaving if total discount amount goes greater than sub-total.
            //if (!isCorrectTotalDiscount())
            //{
            //    numTotalDiscount.Focus();
            //}
        }

        private void pnlLoadedInvoiceData_Paint(object sender, PaintEventArgs e)
        {
        }

        private void numRate_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void numQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void numDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void cmbDiscountType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void cmbProcedure_SelectedIndexChanged(object sender, EventArgs e)
        {
             try
            {
                if (cmbProcedure.SelectedIndex >= 0)
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        double Price = unitOfWork.ProcedureRepository.GetProcedurePrice((long)cmbProcedure.SelectedValue);
                        numRate.Value = (decimal)Price;
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error Occurred While Fetching Procedure Price, Please Try Again", ex.Message, "Error");
            }
        }

        private void btnAddNewProcedure_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new AddProcedure(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);           
        }
    }
}