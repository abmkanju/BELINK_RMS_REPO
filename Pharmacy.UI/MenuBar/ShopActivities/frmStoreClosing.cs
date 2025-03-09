using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.ManageAuditLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.ShopActivities
{
    public partial class frmStoreClosing : Form
    {
        private UnitOfWork unitOfWork;
        DateTime LastClosingDate ;

        #region LoadDetails
        public frmStoreClosing()
        {
            InitializeComponent();
        }
        private void frmStoreClosing_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefreshForm, btnSave, btnCancel, btnCS_ShowDetail, btnCS_Detail });
            LoadCashSummary();
        }
        private void LoadCashSummary()
        {
            CashSummaryVM vm = new CashSummaryVM();
            using (unitOfWork = new UnitOfWork())
            {
                vm = unitOfWork.StoreClosingRepository.GetLastCashSummary(dtpClosingDate.Value);
            }
            this.LastClosingDate = vm.LastClosingDate;
            txxCS_OpCash.Enabled = vm.IsFirstClosing;
            txxCS_OpCash.Text = vm.OpeningCash.ToString();
            txxCS_TInflow.Text = vm.TotalInflow.ToString();
            txxCS_TOutFlow.Text = vm.TotalOutFlow.ToString();
            txxCS_SysCash.Text = (vm.TotalInflow + vm.OpeningCash - vm.TotalOutFlow).ToString();
            //using (unitOfWork = new UnitOfWork())
            //{
            //    vm = unitOfWork.StoreClosingRepository.getLastCashSummary();
            //}
            //this.LastClosingDate = vm.LastClosingDate;
            //txxCS_OpCash.Enabled = vm.IsFirstClosing;
            //txxCS_OpCash.Text = vm.OpeningCash.ToString();
            //txxCS_TInflow.Text = vm.TotalInflow.ToString();
            //txxCS_TOutFlow.Text = vm.TotalOutFlow.ToString();
            //txxCS_SysCash.Text = (vm.TotalInflow + vm.OpeningCash - vm.TotalOutFlow).ToString();
        }
        #endregion

        #region TextBoxEvents
        private void txxCS_PhyCash_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }
        private void txxCS_PhyCash_TextChanged(object sender, EventArgs e)
        {
            //double PhyCash = 0;
            //double SysCash = 0;
            //double.TryParse(txxCS_PhyCash.Text.Trim(), out PhyCash);
            //double.TryParse(txxCS_SysCash.Text.Trim(), out SysCash);
            //txxCS_CashDiff.Text = (SysCash - PhyCash).ToString();
            CalculateFields();
        }
        private void txxCS_OpCash_Leave(object sender, EventArgs e)
        {
            //double OpCash = 0, inflow = 0, outflow = 0;
            //double.TryParse(txxCS_OpCash.Text.Trim(), out OpCash);
            //double.TryParse(txxCS_TInflow.Text.Trim(), out inflow);
            //double.TryParse(txxCS_TOutFlow.Text.Trim(), out outflow);
            //txxCS_SysCash.Text = (OpCash + inflow - outflow).ToString();
            CalculateFields();
        }
        private void txtBankSubmitted_TextChanged(object sender, EventArgs e)
        {
            CalculateFields();
        }
        private void txtHOSubmitted_TextChanged(object sender, EventArgs e)
        {
            CalculateFields();
        }
        private void txtBankSubmitted_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }
        private void txtHOSubmitted_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }
        private void dtpClosingDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpClosingDate.Value <= this.LastClosingDate)
            {
                MessageBox.Show("Closing date must be greater than previous closing date.", "Invalid Closing Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.dtpClosingDate.Value = DateTime.Now;
                return;
            }
            LoadCashSummary();
        }
        #endregion

        #region Save
        private void btnSave_Click(object sender, EventArgs e)
        {
            insert(); 
        }
        private void insert()
        {
            try
            {
                StoreClosing obj = new StoreClosing();
                using (unitOfWork = new UnitOfWork())
                {

                    double opCash = 0, inflow = 0, outflow = 0, systemCash = 0,
                           phyCash = 0, CashDiff = 0, BankSubmitted = 0, HoSubmitted = 0,
                           TCashSubmitted = 0, ClosingCash = 0;

                    double.TryParse(txxCS_OpCash.Text.Trim(), out opCash);
                    double.TryParse(txxCS_TInflow.Text.Trim(), out inflow);
                    double.TryParse(txxCS_TOutFlow.Text.Trim(), out outflow);
                    double.TryParse(txxCS_SysCash.Text.Trim(), out systemCash);
                    double.TryParse(txxCS_PhyCash.Text.Trim(), out phyCash);
                    double.TryParse(txxCS_CashDiff.Text.Trim(), out CashDiff);
                    double.TryParse(txtBankSubmitted.Text.Trim(), out BankSubmitted);
                    double.TryParse(txtHOSubmitted.Text.Trim(), out HoSubmitted);
                    double.TryParse(txtTCashSubmitted.Text.Trim(), out TCashSubmitted);
                    double.TryParse(txtClosingCash.Text.Trim(), out ClosingCash);

                    obj.OpeningCash = opCash;
                    obj.TotalInflow = inflow;
                    obj.TotalOutFlow = outflow;
                    obj.SystemCash = systemCash;
                    obj.PhysicalCash = phyCash;
                    obj.CashDiff = CashDiff;
                    obj.CashSubmittedToBank = BankSubmitted;
                    obj.CashSubmittedToHO = HoSubmitted;
                    obj.TotalCashSubmitted = TCashSubmitted;
                    obj.ClosingCash = ClosingCash;

                    obj.ClosingDate = dtpClosingDate.Value;

                    obj.CratedAt = DateTime.Now;
                    obj.UpdatedAt = DateTime.Now;
                    obj.IsActive = true;
                    obj.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);

                    unitOfWork.StoreClosingRepository.Insert(obj);
                    AuditLogTask.CreateAuditLog(unitOfWork, obj.ClosingDate.ToString("yyyy/MM/dd HH:mm:ss"), AuditModule.POS_Closing, AuditAction.created);
                    unitOfWork.Save();
                    MessageBox.Show("Store closing save successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving store closing, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CalculateFields()
        {
            double opCash = 0, inflow = 0, outflow = 0, systemCash = 0,
                           phyCash = 0, CashDiff = 0, BankSubmitted = 0, HoSubmitted = 0,
                           TCashSubmitted = 0, ClosingCash = 0;

            double.TryParse(txxCS_OpCash.Text.Trim(), out opCash);
            double.TryParse(txxCS_TInflow.Text.Trim(), out inflow);
            double.TryParse(txxCS_TOutFlow.Text.Trim(), out outflow);
            double.TryParse(txxCS_SysCash.Text.Trim(), out systemCash);
            double.TryParse(txxCS_PhyCash.Text.Trim(), out phyCash);
            double.TryParse(txxCS_CashDiff.Text.Trim(), out CashDiff);
            double.TryParse(txtBankSubmitted.Text.Trim(), out BankSubmitted);
            double.TryParse(txtHOSubmitted.Text.Trim(), out HoSubmitted);
            double.TryParse(txtTCashSubmitted.Text.Trim(), out TCashSubmitted);
            double.TryParse(txtClosingCash.Text.Trim(), out ClosingCash);

            systemCash = opCash + inflow - outflow;
            CashDiff = systemCash - phyCash;
            TCashSubmitted = BankSubmitted + HoSubmitted;
            ClosingCash = phyCash - TCashSubmitted;

            txxCS_SysCash.Text = systemCash.ToString();
            txxCS_CashDiff.Text = CashDiff.ToString();
            txtTCashSubmitted.Text = TCashSubmitted.ToString();
            txtClosingCash.Text = ClosingCash.ToString();
        }
        #endregion

        #region CommentedCode
        private void btnCS_Detail_Click(object sender, EventArgs e)
        {
            Pharmacy.frmNoteCalculator f = new Pharmacy.frmNoteCalculator();
            f.ShowDialog();
            txxCS_PhyCash.Text = SharedVariables.TotalNotesAmount.ToString();
            SharedVariables.TotalNotesAmount = 0;
        }
        private void btnCS_ShowDetail_Click(object sender, EventArgs e)
        {
            if (btnCS_ShowDetail.Tag.ToString() == "0")
            {
                btnCS_ShowDetail.Tag = "1";
                btnCS_ShowDetail.Text = "Hide Detail";
            }
            else
            {
                btnCS_ShowDetail.Tag = "0";
                btnCS_ShowDetail.Text = "Show Detail";
            }
        }
        #endregion

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            dtpClosingDate.Value = DateTime.Now;
            LoadCashSummary();
        }
    }
}