using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.Reports;
using PagedList;
using Pharmacy.Core;
using Pharmacy.UI.ManageAuditLog;

namespace Pharmacy.UI.MenuBar.ShopActivities
{
    public partial class frmPOSCosing : Form
    {

        bool isDateChanged = false;
        int PageNo = 1;
        bool IsFilterApplied = false;
        static PharmacyContext cxt = new PharmacyContext();
        ItemsRepository repItems = new ItemsRepository(cxt);
        UnitOfWork unitOfWork;
        IPagedList<ItemsVM> itemsList;
        bool PrintSkimmedCash = false;
        public frmPOSCosing()
        {
            InitializeComponent();
        }

        private void bntCR_Ok_Click(object sender, EventArgs e)
        {
            try
            {
                double rcvdCash = 0;
                POSReceivedCash obj = new POSReceivedCash();
                double.TryParse(txtCR.Text.Trim(), out rcvdCash);
                obj.Cash = rcvdCash;
                obj.UserId = SharedVariables.LoggedInUser.UserId;
                obj.POSCode = int.Parse(lblPOSCode.Text);
                obj.IsActive = true;
                obj.IsNew = true;
                obj.IsSynced = false;
                obj.IsUpdate = false;
                obj.CreatedAt = DateTime.Now;
                obj.UpdateAt = DateTime.Now;
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.POSCashReceiveRepository.Insert(obj);
                    unitOfWork.Save();
                }
                loadCR_History();
                MessageBox.Show("Cash receiving saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving cash receiving, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCR_Cancel_Click(object sender, EventArgs e)
        {
            txtCR.Text = "";
        }

        private void btnCR_History_Click(object sender, EventArgs e)
        {
            loadCR_History();
        }

        private void loadCR_History()
        {
            grdCR.Rows.Clear();
            using (unitOfWork = new UnitOfWork())
            {
                List<POSReceivedCash> lst = unitOfWork.POSCashReceiveRepository.GetCR_CurrSess(SharedVariables.LoggedInUser.UserId);
                foreach (var c in lst)
                {
                    grdCR.Rows.Add(c.POSReceivedCashId, c.CreatedAt, c.Cash);
                }
                lblCR.Text = lst.Sum(c => c.Cash).ToString();
            }
        }

        private void loadCS_History()
        {
            grdCS.Rows.Clear();
            using (unitOfWork = new UnitOfWork())
            {
                List<POSSkimmedCash> lst = unitOfWork.POSCashSkimmedRepository.GetCS_CurrSess(SharedVariables.LoggedInUser.UserId);
                foreach (var c in lst)
                {
                    grdCS.Rows.Add(c.POSSkimmedCashId, c.CreatedAt, c.Cash);
                }
                lblCS.Text = lst.Sum(c => c.Cash).ToString();
            }
        }

        private void txtCR_Cash_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void frmPOSCosing_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdCR);
            SharedFunctions.SetGridStyle(grdCS);
            SharedFunctions.SetGridStyle(grdNotes);
            SharedFunctions.SetGridStyle(grdXRpt);
            ShowPOSData();
        }

        private void ShowPOSData()
        {
            var res = getPOSData();
            lblCS.Text = res.Item1.ToString();
            lblCR.Text = res.Item2.ToString();
            lblOp.Text = res.Item3.ToString();
            lblPOSCode.Text = res.Item4.ToString();
        }

        private Tuple<double, double, double, int> getPOSData()
        {
            double CS = 0;
            double CR = 0;
            double OC = 0;
            int Code = 0;
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    CS = unitOfWork.POSCashSkimmedRepository.GetCSSum_CurrSess(SharedVariables.LoggedInUser.UserId);
                    CR = unitOfWork.POSCashReceiveRepository.GetCRSum_CurrSess(SharedVariables.LoggedInUser.UserId);
                    OC = unitOfWork.POSClosingRepository.GetLastCashClosingVal();
                    Code = unitOfWork.POSClosingRepository.GetNewClosingCode();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while getting some values, please try reloading current screen.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return new Tuple<double, double, double, int>(CS, CR, OC, Code);
        }

        private void grdCashRCcv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                int Id = Convert.ToInt32(grdCR.Rows[e.RowIndex].Cells["colId"].Value);
                if (e.ColumnIndex == grdCR.Columns["colDel"].Index)
                {
                    var res = MessageBox.Show("Are you sure you want to delete seleted cash receiving.", "Please Make Sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        using (unitOfWork = new UnitOfWork())
                        {
                            var obj = unitOfWork.POSCashReceiveRepository.GetById(Id);
                            obj.IsActive = false;
                            obj.UserId = SharedVariables.LoggedInUser.UserId;
                            if (obj.IsSynced)
                            {
                                obj.IsNew = false;
                                obj.IsUpdate = true;
                                obj.IsSynced = false;
                            }
                            obj.UpdateAt = DateTime.Now;
                            unitOfWork.POSCashReceiveRepository.Update(obj);
                            unitOfWork.Save();
                        }
                        MessageBox.Show("Cash receiving deleted successfully.", "Succss", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while performing required operation, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCR_Click(object sender, EventArgs e)
        {
            loadCR_History();
            pnlXRpt.Visible = false;
            pnlCS.Visible = false;
            pnlCashClosing.Visible = false;
            pnlCR.Visible = true;
        }

        private void btnCS_Click(object sender, EventArgs e)
        {
            loadCS_History();
            pnlXRpt.Visible = false;
            pnlCR.Visible = false;
            pnlCashClosing.Visible = false;
            pnlCS.Visible = true;
        }

        private void txtCS_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void bntCS_OK_Click(object sender, EventArgs e)
        {
            try
            {
                double skimmedCash = 0;
                POSSkimmedCash obj = new POSSkimmedCash();
                double.TryParse(txtCS.Text.Trim(), out skimmedCash);
                obj.Cash = skimmedCash;
                obj.UserId = SharedVariables.LoggedInUser.UserId;
                obj.IsActive = true;
                obj.POSCode = int.Parse(lblPOSCode.Text);
                obj.IsNew = true;
                obj.IsSynced = false;
                obj.IsUpdate = false;
                obj.CreatedAt = DateTime.Now;
                obj.UpdateAt = DateTime.Now;
                using (unitOfWork = new UnitOfWork())
                {
                    obj = unitOfWork.POSCashSkimmedRepository.Insert(obj);
                    unitOfWork.Save();
                }
                loadCS_History();
                MessageBox.Show("Cash skimming saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.PrintSkimmedCash)
                {
                    frmSkimedCashViewer v = new frmSkimedCashViewer(chkPrint.Checked, obj.POSSkimmedCashId);
                    v.Show();
                    this.PrintSkimmedCash = false;
                    this.chkPrint.Checked = false;
                }
                txtCS.Text = "";
                txtCS.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving cash skimming, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCS_Cancel_Click(object sender, EventArgs e)
        {
            txtCS.Text = "";
        }

        private void btnCS_History_Click(object sender, EventArgs e)
        {
            loadCS_History();
        }

        private void grdCS_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                int Id = Convert.ToInt32(grdCR.Rows[e.RowIndex].Cells["colId"].Value);
                if (e.ColumnIndex == grdCR.Columns["colDel"].Index)
                {
                    var res = MessageBox.Show("Are you sure you want to delete seleted cash skimming.", "Please Make Sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        using (unitOfWork = new UnitOfWork())
                        {
                            var obj = unitOfWork.POSCashSkimmedRepository.GetById(Id);
                            obj.IsActive = false;
                            obj.UserId = SharedVariables.LoggedInUser.UserId;
                            if (obj.IsSynced)
                            {
                                obj.IsNew = false;
                                obj.IsUpdate = true;
                                obj.IsSynced = false;
                            }
                            obj.UpdateAt = DateTime.Now;
                            unitOfWork.POSCashSkimmedRepository.Update(obj);
                            unitOfWork.Save();
                        }
                        MessageBox.Show("Cash skimming deleted successfully.", "Succss", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while performing required operation, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkPrint_CheckedChanged(object sender, EventArgs e)
        {
            this.PrintSkimmedCash = true;
        }

        private void txtCashCounted_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtCashSubmitted_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtCashCounted_TextChanged(object sender, EventArgs e)
        {
            txtCashTillIn.Text = calculateCashTillIn().ToString();
        }

        private void txtCashSubmitted_TextChanged(object sender, EventArgs e)
        {
            txtCashTillIn.Text = calculateCashTillIn().ToString();
        }

        private double calculateCashTillIn()
        {
            if (txtCashCounted.Text == "") txtCashCounted.Text = "0";
            if (txtCashSubmitted.Text == "") txtCashSubmitted.Text = "0";
            return double.Parse(txtCashCounted.Text) - double.Parse(txtCashSubmitted.Text);
        }

        private void btnClosing_Click(object sender, EventArgs e)
        {
            pnlXRpt.Visible = false;
            pnlCR.Visible = false;
            pnlCS.Visible = false;
            pnlCashClosing.Visible = true;
        }

        private void btnNotes_Click(object sender, EventArgs e)
        {
            grdNotes.Visible = !grdNotes.Visible;
            if (grdNotes.Visible)
            {
                grdNotes.Rows.Clear();
                grdNotes.Rows.Add(1, 5000, 0, 0);
                grdNotes.Rows.Add(1, 1000, 0, 0);
                grdNotes.Rows.Add(1, 500, 0, 0);
                grdNotes.Rows.Add(1, 100, 0, 0);
                grdNotes.Rows.Add(1, 50, 0, 0);
                grdNotes.Rows.Add(1, 20, 0, 0);
                grdNotes.Rows.Add(1, 10, 0, 0);
                grdNotes.Rows.Add(1, 5, 0, 0);
                grdNotes.Rows.Add(1, 2, 0, 0);
                grdNotes.Rows.Add(1, 1, 0, 0);
            }
        }

        private void grdNotes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                if (e.ColumnIndex == grdNotes.Columns["colCount"].Index)
                {
                    double total = 0;
                    int count = 0; bool parsed = int.TryParse(grdNotes.Rows[e.RowIndex].Cells["colCount"].Value.ToString(), out count);
                    if (!parsed)
                    {
                        grdNotes.Rows[e.RowIndex].Cells["colCount"].Value = 0;
                    }
                    int Note = Convert.ToInt32(grdNotes.Rows[e.RowIndex].Cells["colNote"].Value);
                    grdNotes.Rows[e.RowIndex].Cells["colTotal"].Value = count * Note;
                    foreach (DataGridViewRow r in grdNotes.Rows)
                    {
                        total += Convert.ToDouble(r.Cells["colTotal"].Value);
                    }
                    txtCashCounted.Text = total.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while performing notes counting, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dtpClosingDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime lastclosing;
                using (unitOfWork = new UnitOfWork())
                {
                    lastclosing = unitOfWork.POSClosingRepository.GetLastPOSClosingDate();
                }
                if (dtpClosingDate.Value <= lastclosing)
                {
                    dtpClosingDate.Value = DateTime.Now;
                    MessageBox.Show("Closing date must be greate than previous last closing date.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                dtpClosingDate.Value = DateTime.Now;
                MessageBox.Show("An error occurred while loading previous closing date, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClsoing_OK_Click(object sender, EventArgs e)
        {
            try
            {
                double cashCounted = 0;
                double cashSubmitted = 0;
                double CashTillIn = 0;
                double.TryParse(txtCashCounted.Text, out cashCounted);
                double.TryParse(txtCashSubmitted.Text, out cashSubmitted);
                double.TryParse(txtCashTillIn.Text, out CashTillIn);
                POSClosing obj = new POSClosing();

                obj.ClosingDate = dtpClosingDate.Value;
                obj.CashCounted = cashCounted;
                obj.CashSubmitted = cashSubmitted;
                obj.CashTillIn = CashTillIn;

                obj.IsActive = true;
                obj.IsNew = true;
                obj.CreatedAt = DateTime.Now;
                obj.UpdatedAt = DateTime.Now;
                obj.IsSynced = false;
                obj.UserId = SharedVariables.LoggedInUser.UserId;
                obj.POSCode = int.Parse(lblPOSCode.Text);

                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.POSClosingRepository.Insert(obj);
                    AuditLogTask.CreateAuditLog(unitOfWork, obj.ClosingDate.ToString("yyyy/MM/dd HH:mm:ss"), AuditModule.POS_Closing, AuditAction.created);
                    unitOfWork.Save();
                }
                MessageBox.Show("POS closing performed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowPOSData();
                /////////////////////////  reset data //////////////////////
                btnNotes.PerformClick();
                txtCashCounted.Text = "";
                txtCashSubmitted.Text = "";
                txtCashTillIn.Text = "";
                dtpClosingDate.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while performing required operation, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXR_Click(object sender, EventArgs e)
        {
            pnlCR.Visible = false;
            pnlCS.Visible = false;
            pnlCashClosing.Visible = false;
            pnlXRpt.Visible = true;

            grdXRpt.Rows.Clear();
            using (unitOfWork = new UnitOfWork())
            {
                List<XRptVM> xrpt = unitOfWork.InvoiceRepository.getXRpt(SharedVariables.LoggedInUser.UserId);
                int sr = 1;
                foreach (var r in xrpt)
                {
                    grdXRpt.Rows.Add(sr, r.InvoiceRef, r.TrTime, r.Amount);
                }
            }
        }
    }
}
