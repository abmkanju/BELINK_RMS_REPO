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
using PagedList;
using Pharmacy.Core;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmSuppliers : Form
    {
        UnitOfWork unitOfWork;
        IPagedList<Supplier> suppliersList;
        int PageNo = 1;
        public frmSuppliers()
        {
            InitializeComponent();
        }

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddSupplier(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            PageNo = 1;
            LoadSuppliers();
            fillSuppliersCombo();
        }

        private void frmSuppliers_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdSuppliers);
            LoadSuppliers();
            fillSuppliersCombo();
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddSupplier, btnClose });
            SharedFunctions.SetGridStyle(grdSuppliers);
        }


        private void fillSuppliersCombo()
        {
            List<Supplier> sps = new List<Supplier>();
            using (unitOfWork = new UnitOfWork())
            {
                sps = unitOfWork.SupplierRepository.GetActiveSuppliers();
            }
            sps.Insert(0, new Supplier { SupplierID = 0, Name = "Select All" });
            cmbSuppliers.DataSource = sps;
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.ValueMember = "SupplierId";
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.G))
            {
                grdSuppliers.Focus();
                if (grdSuppliers.Rows.Count >= 1)
                {
                    grdSuppliers.Rows[0].Selected = true;
                }
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.F))
            {
                SharedFunctions.OpenForm(new frmAddSupplier(), this.MdiParent);
                return true;
            }
            
            #region grid short cuts
            if (keyData == (Keys.Alt | Keys.E))
            {
                if (grdSuppliers.SelectedRows.Count > 0)
                {
                    int colIndex = grdSuppliers.Columns["colEdit"].Index;
                    int rowIndex = grdSuppliers.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdSuppliers_CellContentClick(grdSuppliers, e);
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.V))
            {
                {
                    int colIndex = grdSuppliers.Columns["colView"].Index;
                    int rowIndex = grdSuppliers.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdSuppliers_CellContentClick(grdSuppliers, e);
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadSuppliers()
        {
            try
            {
                grdSuppliers.Rows.Clear();
                using (unitOfWork = new UnitOfWork())
                {
                    if (cmbSuppliers.SelectedIndex == 0)
                    {
                        suppliersList = unitOfWork.SupplierRepository.GetActiveSuppliers(PageNo, SharedVariables.PageSize);
                    }
                    else
                    {
                        suppliersList = unitOfWork.SupplierRepository.GetActiveSuppliers(Convert.ToInt64(cmbSuppliers.SelectedValue));
                    }
                }

                foreach (var s in suppliersList)
                {
                    grdSuppliers.Rows.Add(s.SupplierID,  s.Name, s.NTN ,s.STN, s.Address,  s.CreatedAt);
                }
                btnPrevious.Enabled = suppliersList.HasPreviousPage;
                btnNext.Enabled = suppliersList.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, suppliersList.PageCount);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo++;
            LoadSuppliers();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo--;
            LoadSuppliers();
        }

        private void grdSuppliers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            long SupplierId = Convert.ToInt64(grdSuppliers.Rows[e.RowIndex].Cells["colSupplierId"].Value);
            if (e.ColumnIndex == grdSuppliers.Columns["colEdit"].Index)
            {
                frmAddSupplier f = new frmAddSupplier(SupplierId);
                f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                f.Show();
                return;
            }

            if (e.ColumnIndex == grdSuppliers.Columns["colView"].Index)
            {
                frmSupplierInfo f = new frmSupplierInfo(SupplierId);
                f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                f.Show();
                return;
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = suppliersList.PageCount;
            LoadSuppliers();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadSuppliers();
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmSuppliersShortCuts f = new ShortCutDialogs.frmSuppliersShortCuts();
            f.ShowDialog();
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
                    if (_pageNo <= 0 || _pageNo > suppliersList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadSuppliers();
                }
            }
        }

        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSuppliers();
        }

        private void frmSuppliers_VisibleChanged(object sender, EventArgs e)
        {
            fillSuppliersCombo();
            
        }
    }
}
