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
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using PagedList;
using Pharmacy.Core;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;
using System.Data.SqlClient;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmInvoiceHistory : Form
    {
        enum FilterType { Default = 1, Date = 2, Patient = 3, User = 4, Invoice = 5, InvoiceType = 6 }
        FilterType filter = FilterType.Default;
        UnitOfWork unitOfWork;
        IPagedList<InvoiceTransactionsVM> Transactions;
        System.Timers.Timer PatientSearchTimer = new System.Timers.Timer();
        int PageNo = 1;
        bool isFilterapplied = false;
        int pageNoPatients = 1;
        bool IsNameSearching, IsPhoneSearching = false;
        long SelectedPatientId = 0;
        List<PatientSearchVM> PatientsGlobal = new List<PatientSearchVM>();

        public frmInvoiceHistory()
        {
            InitializeComponent();
            PatientSearchTimer.Enabled = true;
            PatientSearchTimer.Interval = SharedVariables.AsyncDataLoadDelay;
            PatientSearchTimer.AutoReset = true;
            PatientSearchTimer.Elapsed += new System.Timers.ElapsedEventHandler(PatientSearchTimer_Elapsed);
            PatientSearchTimer.Stop();
        }
        private void updateEstimateBills()
        {
            foreach (DataGridViewRow row in grdInvoices.Rows)
            {
                if (row.Cells["ColInvoiceType"].Value == "Estimate Bill")
                {
                    row.Cells["colCopy"].Value = "Create";
                }
                else
                {
                    row.Cells["colCopy"].Value = "Copy";
                }

            }
        }
        private void frmInvoiceHistory_Load(object sender, EventArgs e)
        {
            try
            {
                //GridDesigner.CustomizeDataGridView(grdInvoices);
                SharedFunctions.SetGridStyle(grdInvoices);
                this.WindowState = FormWindowState.Maximized;
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddInvoice, btnRefreshForm, btnClose });
                LoadInvoices();
                LoadUsers();
                updateEstimateBills();
                //LoadPatients();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }
        private void LoadUsers()
        {
            try
            {
                List<UserSelectVM> us = new List<UserSelectVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    us = unitOfWork.UserRepository.GetAllActiveUsers();
                }
                UserSelectVM v = new UserSelectVM
                {
                    UserId = 0,
                    UserName = "Select All"
                };
                us.Insert(0, v);
                cmbUsers.SelectedIndexChanged -= cmbUsers_SelectedIndexChanged;
                cmbUsers.DataSource = us;
                cmbUsers.DisplayMember = "UserName";
                cmbUsers.ValueMember = "UserId";
                cmbUsers.SelectedIndexChanged += cmbUsers_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading users data. Please try again.", "User Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadInvoices()
        {
            try
            {
                InvoiceHistorySumVM details = new InvoiceHistorySumVM();
                using (unitOfWork = new UnitOfWork())
                {
                    if (filter == FilterType.Date)
                    {
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                        details = unitOfWork.InvoiceRepository.GetTransactionsSum(dtpFrom.Value, dtpTo.Value);
                    }
                    else if (filter == FilterType.Patient) // + date filter
                    {
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions(dtpFrom.Value, dtpTo.Value, this.SelectedPatientId, PageNo, SharedVariables.PageSize);// Convert.ToInt64(cmbPatient.SelectedValue));
                        details = unitOfWork.InvoiceRepository.GetTransactionsSum(dtpFrom.Value, dtpTo.Value, this.SelectedPatientId);
                    }
                    else if (filter == FilterType.User) // + date filter
                    {
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions(dtpFrom.Value, dtpTo.Value, Convert.ToInt32(cmbUsers.SelectedValue), PageNo, SharedVariables.PageSize);
                        details = unitOfWork.InvoiceRepository.GetTransactionsSum(dtpFrom.Value, dtpTo.Value, Convert.ToInt32(cmbUsers.SelectedValue));
                    }
                    else if (filter == FilterType.Invoice && SharedFunctions.StringToLong(txtInvoiceNo.Text) > 0) // date filter not required
                    {
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions(Convert.ToInt64(txtInvoiceNo.Text), PageNo, SharedVariables.PageSize);
                        details = unitOfWork.InvoiceRepository.GetTransactionsSum(Convert.ToInt64(txtInvoiceNo.Text));
                    }
                    else if (filter == FilterType.InvoiceType && cmbInvoiceType.SelectedIndex != -1)
                    {
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions(PageNo, SharedVariables.PageSize, cmbInvoiceType.SelectedIndex);
                        details = unitOfWork.InvoiceRepository.GetTransactionsSum(cmbInvoiceType.SelectedIndex);
                    }
                    else
                    {
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions(PageNo, SharedVariables.PageSize);
                        details = unitOfWork.InvoiceRepository.GetTransactionsSum();
                    }


                }
                string PatientName = "";
                grdInvoices.Rows.Clear();
                if (details != null)
                {
                    lblTotalInvoices.Text = details.TotalInvoice.ToString();
                    lblTotalQty.Text = details.TotalQty.ToString();
                    lblTotalSales.Text = details.TotalPaid.ToString("0.00");
                    lblTotalDue.Text = details.TotalDue.ToString();
                    lblTotalAdvance.Text = Math.Abs(details.TotalAdvance).ToString();
                    TotalRefundText.Text = Math.Abs(details.TotalRefund).ToString();
                }
                else
                {
                    lblTotalInvoices.Text = "0";
                    lblTotalQty.Text = "0";
                    lblTotalSales.Text = "0";
                    lblTotalDue.Text = "0";
                    lblTotalAdvance.Text = "0";
                }
                foreach (InvoiceTransactionsVM i in Transactions)
                {
                    PatientName = "";
                    if (i.Patient != null)
                    {
                        PatientName = i.Patient.Name;
                    }
                    grdInvoices.Rows.Add
                    (
                        i.IsOnlineIntegrate,
                        i.InvoiceId,
                        i.InvoiceRefNo,
                        PatientName,
                        (i.SaleQuantity - i.ReturnedQty),
                        i.SubTotal,
                        i.TotalDiscount,
                        i.DiscountType == 1 ? "Value" : "%",
                        i.Services,
                        i.GrandTotal,
                        i.TotalPaid,
                        i.Due > 0 ? i.Due : 0,
                        i.Due < 0 ? -i.Due : 0,
                        i.TotalRefund > 0 ? i.TotalRefund : 0,
                        i.IsQInvoice ? "Estimate Bill" : "Sale invoice",
                        i.CreateDate,
                        i.IsProcedureInvoice,
                        i.UserName
                    ); ;
                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, Transactions.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = Transactions.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = Transactions.HasNextPage;
                updateEstimateBills();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            dtpTo.Value = DateTime.Now;
            dtpFrom.Value = DateTime.Now;
            txtSearchByName.Text = "";
            ClearForm();
            LoadUsers();
            LoadInvoices();
            updateEstimateBills();
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
        }

        private void ClearForm()
        {
            PageNo = 1;
            dtpFrom.Value = dtpTo.Value = DateTime.Now;
            txtInvoiceNo.Text = "";
            cmbPatient.SelectedIndex = -1;
            isFilterapplied = false;
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadInvoices();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = Transactions.PageCount;
            LoadInvoices();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadInvoices();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadInvoices();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            PageNo = 1;
            LoadInvoices();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            PageNo = 1;
            LoadInvoices();
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.F5))
            {
                btnRefreshForm.PerformClick();
                return true;
            }
            return false;
        }
        private void txtInvoiceNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtInvoiceNo.Text.Trim() != "")
                {
                    filter = FilterType.Invoice;
                }
                else
                {
                    filter = FilterType.Default;
                }
                cmbUsers.SelectedIndex = 0;
                txtSearchByName.Text = "";
                PageNo = 1;
                LoadInvoices();
            }
        }

        private void txtInvoiceNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void cmbPatient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt64(cmbPatient.SelectedValue) > 0)
            {
                txtInvoiceNo.Text = "";
                isFilterapplied = true;
            }
            PageNo = 1;
            LoadInvoices();
        }

        private void grdInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) { return; }
                long InvoiceId = Convert.ToInt64(grdInvoices.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                bool IsProcedureInvoice = Convert.ToBoolean(grdInvoices.Rows[e.RowIndex].Cells["colIsProcedureInvoice"].Value);
                string IsOnline = grdInvoices.Rows[e.RowIndex].Cells["IsOnlineIntgrte"].Value.ToString();
                
                if (e.ColumnIndex == grdInvoices.Columns["colEdit"].Index)
                {
                    if (IsOnline == "False")
                    {
                        if (!SharedFunctions.IsActionallowed("Edit Invoice") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        {
                            SharedFunctions.AccessDeniedMsg();
                            return;
                        }
                        if (grdInvoices.Rows[e.RowIndex].Cells["ColInvoiceType"].Value == "Estimate Bill")
                        {
                            MessageBox.Show("Estimate Invoices Can't be Edited .... ");
                            return;
                        }
                        using (unitOfWork = new UnitOfWork())
                        {
                            List<InvoiceItem> a = unitOfWork.InvoiceRepository.IsInvoiceHasReturn(InvoiceId);
                            if (a.Count != 0)
                            {
                                MessageBox.Show("Invoices After Refund Can't be Edited .... ");
                                return;
                            }
                        }
                        bool IsStoreitem = false;
                        bool IsMultipleConsumptionItems = false;
                        using (unitOfWork = new UnitOfWork())
                        {
                            IsStoreitem = unitOfWork.InvoiceRepository.IsStoreItem(InvoiceId);
                            IsMultipleConsumptionItems = unitOfWork.InvoiceRepository.IsMultipleConsumptionItems(InvoiceId);
                        }
                        if (IsStoreitem == true && SharedVariables.AdminPharmacySetting.IsEnableStores == false)
                        {
                            SharedFunctions.MessageBoxShow("Please enable store to view this.", MessageType.Warning);
                            return;
                        }


                        //if (IsMultipleConsumptionItems)
                        //{
                        //    SharedFunctions.MessageBoxShow("Multiple Consumption POS : This functionality is disabled temporarily.", MessageType.Warning);
                        //    return;
                        //}
                        if (IsProcedureInvoice)
                        {
                            frmProcedureInvoice2 f = new frmProcedureInvoice2(true, false, InvoiceId);
                            f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
                            f.Show();
                            return;
                        }
                        else
                        {
                            frmInvoice f = new frmInvoice(true, false, InvoiceId);
                            f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
                            f.Show();
                            return;
                        }
                    }
                    else
                    {
                        SharedFunctions.MessageBoxShow("Not Allowed Online Invoices.", MessageType.Warning);
                        return;
                    }
                }

                if (e.ColumnIndex == grdInvoices.Columns["colRefund"].Index)
                {
                    if (IsOnline == "False")
                    {
                        bool IsMultipleConsumptionItems = false;
                        if (!SharedFunctions.IsActionallowed("Refund Payment") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        {
                            SharedFunctions.AccessDeniedMsg();
                            return;
                        }
                        if (grdInvoices.Rows[e.RowIndex].Cells["ColInvoiceType"].Value == "Estimate Bill")
                        {
                            MessageBox.Show("Estimate Invoices Can't be Refunded .... ");
                            return;
                        }
                        using (unitOfWork = new UnitOfWork())
                        {
                            if (unitOfWork.StockConsumptionRepository.CheckAllInvoiceItemReturn(InvoiceId) > 0)
                            {
                                SharedFunctions.MessageBoxShow("All stock has been returned.", MessageType.Info);
                                return;
                            }
                            bool IsStoreitem = false;
                            IsStoreitem = unitOfWork.InvoiceRepository.IsStoreItem(InvoiceId);
                            if (IsStoreitem == true && SharedVariables.AdminPharmacySetting.IsEnableStores == false)
                            {
                                SharedFunctions.MessageBoxShow("Please enable store to view this.", MessageType.Warning);
                                return;
                            }
                            IsMultipleConsumptionItems = unitOfWork.InvoiceRepository.IsMultipleConsumptionItems(InvoiceId);
                        }
                        //if (IsMultipleConsumptionItems)
                        //{
                        //    SharedFunctions.MessageBoxShow("Multiple Consumption POS : This functionality is disabled temporarily.", MessageType.Warning);
                        //    return;
                        //}
                        if (IsProcedureInvoice)
                        {
                            //MessageBox.Show("You Can't Refund Procedure Invoice", "Stop", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //return;
                            frmProcedureInvoice2 f = new frmProcedureInvoice2(false, true, InvoiceId);
                            f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
                            f.Show();
                            return;
                        }
                        else
                        {
                            frmInvoice f = new frmInvoice(false, true, InvoiceId);
                            f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
                            f.Show();
                            return;
                        }
                    }
                    else
                    {
                        SharedFunctions.MessageBoxShow("Not Allow Online Order Invoices.", MessageType.Warning);
                        return;
                    }
                }

                if (e.ColumnIndex == grdInvoices.Columns["colPrint"].Index)
                {
                    if (IsProcedureInvoice)
                    {
                        Reports.ProcedureInvoices.ProcedureInvoiceViewer v = new Reports.ProcedureInvoices.ProcedureInvoiceViewer(InvoiceId, true);
                        //v.Show();

                    }
                    else
                    {
                        //Reports.Pharmacy.PatientInvoiceViewer v = new Reports.Pharmacy.PatientInvoiceViewer(InvoiceId);
                        Reports.POSInvoices.POSInvoiceViewer v = new Reports.POSInvoices.POSInvoiceViewer(InvoiceId, true);
                        //v.Show();
                        v.Print();
                    }
                    return;
                }

                if (e.ColumnIndex == grdInvoices.Columns["colPreview"].Index)
                {
                    if (IsProcedureInvoice)
                    {
                        Reports.ProcedureInvoices.ProcedureInvoiceViewer v = new Reports.ProcedureInvoices.ProcedureInvoiceViewer(InvoiceId, true);
                        //v.Show();

                    }
                    else
                    {
                        //Reports.Pharmacy.PatientInvoiceViewer v = new Reports.Pharmacy.PatientInvoiceViewer(InvoiceId);
                        Reports.POSInvoices.POSInvoiceViewer v = new Reports.POSInvoices.POSInvoiceViewer(InvoiceId, true);
                        //v.Show();
                        v.Show();
                    }
                    return;
                }

                if (e.ColumnIndex == grdInvoices.Columns["colDelete"].Index)
                {
                    if (IsOnline == "False")
                    {
                        if (!SharedFunctions.IsActionallowed("Delete Invoice"))
                        {
                            SharedFunctions.AccessDeniedMsg();
                            return;
                        }
                        else
                        {
                            if (grdInvoices.Rows[e.RowIndex].Cells["ColInvoiceType"].Value == "Estimate Bill")
                            {
                                MessageBox.Show("Estimate Invoices Can't be Deleted .... ");
                                return;
                            }
                            DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Invoice?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (rs == System.Windows.Forms.DialogResult.OK)
                            {
                                using (unitOfWork = new UnitOfWork())
                                {
                                    Invoice obj = unitOfWork.InvoiceRepository.GetInvoiceToDelete(InvoiceId);
                                    obj.IsActive = false;
                                    obj.DeletedAt = DateTime.Now;
                                    foreach (InvoiceItem i in obj.InvoiceItems)
                                    {
                                        i.IsActive = false;
                                        i.DeletedAt = DateTime.Now;
                                        unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                                    }
                                    obj.StockConsumption.IsActive = false;
                                    unitOfWork.GetDbContext().Entry(obj.StockConsumption).State = System.Data.Entity.EntityState.Modified;
                                    foreach (StockConsumptionItem si in obj.StockConsumption.StockConsumptions)
                                    {
                                        si.IsActive = false;
                                        si.DeletedAt = DateTime.Now;
                                        unitOfWork.GetDbContext().Entry(si).State = System.Data.Entity.EntityState.Modified;
                                    }
                                    foreach (InvoicePayment ip in obj.InvoicePayments)
                                    {
                                        ip.IsActive = false;
                                        ip.DeletedAt = DateTime.Now;

                                        unitOfWork.GetDbContext().Entry(ip).State = System.Data.Entity.EntityState.Modified;
                                    }
                                    //SOT
                                    UpdateDB("Update SalesReturnItems set IsActive = 0 where InvoiceRefNo=" + obj.InvoiceId);
                                    UpdateDB("Update ProfitStatementTransactions set IsDeleted = 1 where InvoiceId=" + obj.InvoiceRefNo);
                                    OfflineJsonTask.SaveOfflineTask(unitOfWork, obj, ModuleName.Invoice, ModuleAction.Delete, obj.InvoiceId);
                                    unitOfWork.InvoiceRepository.Update(obj);
                                    AuditLogTask.CreateAuditLog(unitOfWork, "Invoice# " + obj.InvoiceRefNo, AuditModule.Invoice, AuditAction.deleted);
                                    unitOfWork.Save();
                                    MessageBox.Show("Invoice deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    btnRefreshForm.PerformClick();
                                }
                            }
                        }
                    }
                    else
                    {
                        SharedFunctions.MessageBoxShow("Not Allow Online Order Invoic", MessageType.Warning);
                        return;
                    }

                }
                if (e.ColumnIndex == grdInvoices.Columns["colCopy"].Index)
                {
                    if (IsOnline == "False")
                    {
                        //MessageBox.Show("coppied");
                        if (e.RowIndex < 0) { return; }
                        long Id = Convert.ToInt64(grdInvoices.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                        frmInvoice f;
                        if (grdInvoices.Rows[e.RowIndex].Cells["ColInvoiceType"].Value == "Estimate Bill")
                        {
                            f = new frmInvoice(true, Id, true);
                        }
                        else
                        {
                            f = new frmInvoice(true, Id, false);
                        }

                        f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
                        f.Show();
                        return;
                    }
                }
                else
                {
                    SharedFunctions.MessageBoxShow("Not Allow Online Order Invoice", MessageType.Warning);
                    return;
                }

            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void UpdateDB(String Query)
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            SqlCommand com = new SqlCommand(Query, con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            com.ExecuteNonQuery();
            con.Close();

        }
        private void ChildForm_CLosed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }

        private void btnAddInvoice_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmInvoice(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Search Patiend
        private void searchPatient()
        {
            string searchString = IsNameSearching ? txtSearchByName.Text.Trim().ToLower() : "";// txtSearchByPhone.Text.Trim().ToLower();
            if (searchString == "")
            {
                PatientSearchTimer.Stop();
                this.Invoke(new Action(() =>
                {
                    grdSuggestion.Visible = false;
                }));
                return;
            }

            IPagedList<PatientSearchVM> PatientsLocal = null;
            using (unitOfWork = new UnitOfWork())
            {
                PatientsLocal = unitOfWork.PatientRepository.SearchPatient(IsNameSearching, IsPhoneSearching, false, searchString, pageNoPatients);
                if (PatientsLocal.Count <= 0) PatientSearchTimer.Stop();
                PatientsGlobal.AddRange(PatientsLocal);
            }
            foreach (PatientSearchVM p in PatientsLocal)
            {
                this.Invoke(new Action(() =>
                {
                    grdSuggestion.Rows.Add(p.PatientId, p.MrNo, p.PatientName, p.Phone);
                }));
            }
            if (PatientsGlobal != null && PatientsGlobal.Count > 0)
            {
                this.Invoke(new Action(() =>
                {
                    //grdSuggestion.Rows[0].Selected = false;
                    grdSuggestion.Visible = true;
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    grdSuggestion.Visible = false;
                }));
            }
        }
        private void LoadSearcherPatient()
        {
            PatientSearchTimer.Stop();
            if (grdSuggestion.SelectedRows.Count > 0)
            {
                //txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged;
                txtSearchByName.TextChanged -= txtSearchByName_TextChanged;
                this.SelectedPatientId = Convert.ToInt64(grdSuggestion.SelectedRows[0].Cells["colPatientId"].Value);
                this.txtSearchByName.Text = grdSuggestion.SelectedRows[0].Cells["colGrdPatient"].Value.ToString();
                //this.txtSearchByPhone.Text = grdSuggestion.SelectedRows[0].Cells["colPhone"].Value.ToString();
                grdSuggestion.Visible = false;
                //txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged;
                txtSearchByName.TextChanged += txtSearchByName_TextChanged;
                if (this.SelectedPatientId > 0)
                {
                    txtInvoiceNo.Text = "";
                }
                PageNo = 1;
                filter = FilterType.Patient;
                LoadInvoices();
            }
        }
        private void PatientSearchTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            pageNoPatients += 1;
            string searchString = txtSearchByName.Text.ToLower();
            searchPatient();
        }
        private void txtSearchByName_Enter(object sender, EventArgs e)
        {
            grdSuggestion.Visible = false;
            grdSuggestion.BringToFront();
            grdSuggestion.Location = new Point(txtSearchByName.Location.X, txtSearchByName.Location.Y + txtSearchByName.Height + 4);
        }
        private void txtSearchByName_Leave(object sender, EventArgs e)
        {
            //grdSuggestion.Visible = false;
        }
        private void txtSearchByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtSearchByName.Text.Trim().Equals(""))
                {
                    filter = FilterType.Default;
                    LoadInvoices();
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                ShiftCtrlToSuggGrid();
            }
        }
        private void txtSearchByName_TextChanged(object sender, EventArgs e)
        {
            PatientSearchTimer.Stop();
            pageNoPatients = 1;
            PatientsGlobal = new List<PatientSearchVM>();
            grdSuggestion.Location = new Point(txtSearchByName.Location.X + 15, txtSearchByName.Location.Y + txtSearchByName.Height + 4);
            grdSuggestion.BringToFront();
            grdSuggestion.Rows.Clear();
            IsNameSearching = true;
            //IsPhoneSearching = false;
            searchPatient();
            PatientSearchTimer.Start();
        }
        private void grdSuggestion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            LoadSearcherPatient();
        }
        private void grdSuggestion_KeyPress(object sender, KeyPressEventArgs e)
        {
            // CHECKING FOR ALPHABET 
            TextBox TargetText = new TextBox();
            if (IsNameSearching) { TargetText = txtSearchByName; }
            //else if (IsPhoneSearching) { TargetText = txtSearchByPhone; }
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
        private void ShiftCtrlToSuggGrid()
        {
            if (grdSuggestion.Rows.Count > 0)
            {
                grdSuggestion.BringToFront();
                grdSuggestion.Focus();
                if (grdSuggestion.Rows.Count > 1)
                {
                    grdSuggestion.Rows[0].Selected = false;
                    grdSuggestion.Rows[1].Selected = true;
                }
                else
                {
                    grdSuggestion.Rows[0].Selected = true;
                }
            }
        }
        #endregion

        private void tbGotPageNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtGotoPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > Transactions.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadInvoices();
                }
            }
        }

        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtInvoiceNo.Text = "";
            txtSearchByName.Text = "";
            this.PageNo = 1;
            if (cmbUsers.SelectedIndex == 0)
            {
                filter = FilterType.Default;
            }
            else
            {
                filter = FilterType.User;
            }
            LoadInvoices();
        }

        private void line1_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void cmbInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtInvoiceNo.Text = "";
            txtSearchByName.Text = "";
            this.PageNo = 1;
            if (cmbInvoiceType.SelectedIndex == 0)
            {
                filter = FilterType.Default;
            }
            else
            {
                filter = FilterType.InvoiceType;
            }
            LoadInvoices();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}