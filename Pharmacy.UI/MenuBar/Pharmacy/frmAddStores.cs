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
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core;
using System.Transactions;
using PagedList;
using Pharmacy.UI.ShortCutDialogs;
using System.Runtime.CompilerServices;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmAddStores : Form
    {
        private int StoreId = 0;
        private int EditingRowIndex = 0;
        UnitOfWork unitOfWork;
        public frmAddStores()
        {
            InitializeComponent();
        }
        public frmAddStores(int StoreId)
        {
            InitializeComponent();
            this.StoreId = StoreId;
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.G))
            {
                grdStores.Focus();
                if (grdStores.Rows.Count >= 1)
                {
                    grdStores.Rows[0].Selected = true;
                }
                return true;
            }
            if (keyData == (Keys.Control | Keys.S))
            {
                btnSave.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void frmAddStores_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                SharedFunctions.SetGridStyle(this.grdStores);
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave });
                LoadDetails();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void LoadDetails()
        {
            if (this.StoreId > 0)
            {
                LoadEditDetail();
            }
        }

        private void LoadEditDetail()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    Store store = unitOfWork.StoreRepository.GetStoreEditDetail(this.StoreId);
                    grdStores.Enabled = false;
                    btnAdd.Enabled = false;
                    txtName.Text = store.Name;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void grdStores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == grdStores.Columns["colDelete"].Index)
                {
                    if (e.RowIndex == EditingRowIndex)
                    {
                        Clear();
                        EditingRowIndex = -1;
                    }
                    else if (e.RowIndex < EditingRowIndex)
                    {
                        EditingRowIndex--;
                    }
                    grdStores.Rows.RemoveAt(e.RowIndex);
                }
                if (e.ColumnIndex == grdStores.Columns["colEdit"].Index)
                {
                    btnAdd.Text = "Update Store";
                    EditingRowIndex = e.RowIndex;
                    txtName.Text = grdStores.Rows[e.RowIndex].Cells["colName"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckDuplicateStoreFromDB())
                return;
            if (btnAdd.Text == "Update Store")
            {
                if (string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    SharedFunctions.MessageBoxShow("Please enter a value.", MessageType.Info);
                    return;
                }
                if (grdStores.Rows.Cast<DataGridViewRow>().Any(r => r.Index != EditingRowIndex && r.Cells["colName"].Value.ToString().Equals(txtName.Text.Trim())))
                {
                    SharedFunctions.MessageBoxShow("Store already exists.", MessageType.Info);
                    return;
                }
                grdStores.Rows[EditingRowIndex].Cells["colName"].Value = txtName.Text.Trim();
                Clear();
            }
            else
            {
                if (string.IsNullOrEmpty(txtName.Text.Trim()))
                    return;
                if (grdStores.Rows.Cast<DataGridViewRow>().Any(r => r.Cells["colName"].Value.ToString().Equals(txtName.Text.Trim())))
                {
                    SharedFunctions.MessageBoxShow("Store already exists.", MessageType.Info);
                    return;
                }
                grdStores.Rows.Add(txtName.Text.Trim());
                Clear();
            }
        }
        private void Clear()
        {
            txtName.Text = "";
            EditingRowIndex = 0;
            btnAdd.Text = "Add";
        }

        #region DataGrid
        private void grdStores_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void grdStores_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }
        private void grdStores_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {

        }
        private void grdStores_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void grdStores_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
        }
        private void grdStores_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {

        }
        private void grdStores_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

        }
        private void grdStores_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.StoreId > 0)
                {
                    if (string.IsNullOrEmpty(txtName.Text.Trim()))
                    {
                        SharedFunctions.MessageBoxShow("Please enter a value.", MessageType.Info);
                        return;
                    }
                    if (CheckDuplicateStoreFromDB())
                        return;
                    UpdateStock();
                }
                else
                {
                    if (grdStores.Rows.Count <= 0)
                    {
                        MessageBox.Show("Please Add Some Stores Before Saving", "Ivalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    InsertStock();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private bool CheckDuplicateStoreFromDB()
        {
            bool IsExists = false;
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.StoreRepository.CheckDuplicateItem(txtName.Text, this.StoreId))
                {
                    SharedFunctions.MessageBoxShow("Store already exists.", MessageType.Info);
                    IsExists = true;
                }
            }
            return IsExists;
        }
        private void InsertStock()
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        List<Store> list = new List<Store>();
                        foreach (DataGridViewRow r in grdStores.Rows)
                        {
                            Store str = new Store();
                            str.Name = r.Cells["colName"].Value.ToString();
                            str.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                            str.UpdateDate = DateTime.Now;
                            str.CreateDate = DateTime.Now;
                            str.IsActive = true;
                            str.IsNew = true;
                            str.IsUpdate = false;
                            str.IsSynced = false;
                            list.Add(str);
                            AuditLogTask.CreateAuditLog(unitOfWork, str.Name, AuditModule.Store, AuditAction.created);
                        }
                        unitOfWork.StoreRepository.InsertStores(list);
                        foreach (var item in list)
                        {
                            OfflineJsonTask.SaveOfflineTask(unitOfWork, list, ModuleName.BulkStores, ModuleAction.Create, 0);
                        }
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    throw new Exception("Error occurred while inserting store, please try again.");
                }
            }
            MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void UpdateStock()
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Store store = unitOfWork.StoreRepository.GetStoreEditDetail(this.StoreId);
                        store.Name = txtName.Text;
                        store.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        if (!store.IsNew)
                        {
                            store.UpdateDate = DateTime.Now;
                            store.IsUpdate = true;
                        }
                        unitOfWork.StoreRepository.Update(store);
                        AuditLogTask.CreateAuditLog(unitOfWork, store.Name, AuditModule.Store, AuditAction.updated);
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, store, ModuleName.Store, ModuleAction.Update, store.StoreId);
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    throw new Exception("Error occurred while inserting store, please try again.");
                }
            }
            MessageBox.Show("Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}