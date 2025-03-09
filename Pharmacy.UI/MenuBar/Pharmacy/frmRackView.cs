using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Core;
using Pharmacy.Core.Interfaces;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.UI.Reports;
using Pharmacy.Core.ViewModels;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;


namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmRackView : Form
    {
        UnitOfWork unitOfWork;
        int pageNo = 1;
        IPagedList<RackVM> racks;

        #region LoadRack
        public frmRackView()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        private void frmManageStocks_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnAddRack });
            LoadRacks();
        }
        private void LoadRacks()
        {
            try
            {
                flowPanel.Controls.Clear();
                using (unitOfWork = new UnitOfWork())
                {
                    racks = unitOfWork.RackRepository.GetActiveRacks(pageNo, SharedVariables.PageSize);
                }
                foreach (var item in racks)
                {
                    BuildPanel(item);
                }
                btnNext.Enabled = racks.HasNextPage ? true : false;
                btnPrevious.Enabled = racks.HasPreviousPage ? true : false;
                SharedFunctions.ShowPageNo(lblPageNo, pageNo, racks.PageCount);
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region BindingDetials
        private void BuildPanel(RackVM item)
        {
            Panel pnlHeader = new Panel();
            // 
            // lblSupplier
            // 
            Label lblRack = new Label();
            lblRack.AutoSize = true;
            lblRack.Location = new System.Drawing.Point(5, 15);
            lblRack.Name = "lblRackName";
            lblRack.Size = new System.Drawing.Size(35, 13);
            lblRack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblRack.TabIndex = 1;
            lblRack.Text = item.Name;

            // 
            // btnPrint
            // 
            //Button btnPrint = new Button();
            //btnPrint.Name = "btnPrint";
            //btnPrint.Location = new System.Drawing.Point(1205, 0);
            //btnPrint.Size = new System.Drawing.Size(51, 39);
            //btnPrint.TabIndex = 1;
            //btnPrint.Text = "Print";
            //btnPrint.UseVisualStyleBackColor = true;
            //btnPrint.Tag = 0;
            //btnPrint.Click += new EventHandler(btnPrint_Click);
            // 
            // btnDelete
            // 
            Button btnDelete = new Button();
            btnDelete.Name = "btnDelete";
            btnDelete.Location = new System.Drawing.Point(1205, 0);
            btnDelete.Size = new System.Drawing.Size(51, 39);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Tag = item.RackId;
            btnDelete.Enabled = true;//ConsumptionsCount > 0 ? false : true;
            btnDelete.Click += new EventHandler(btnDelete_Click);
            // 
            // btnEdit
            //
            Button btnEdit = new Button();
            btnEdit.Name = "btnEdit";
            btnEdit.Location = new System.Drawing.Point(1152, 0);
            btnEdit.Size = new System.Drawing.Size(51, 39);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Tag = item.RackId;
            btnEdit.Enabled = true;// ConsumptionsCount == s.StockItems.Count ? false : true;
            btnEdit.Click += new EventHandler(btnEdit_Click);

            pnlHeader.BackColor = Color.FromArgb(241, 241, 241);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1258, 39);
            pnlHeader.TabIndex = 0;
            pnlHeader.Controls.Add(btnEdit);
            pnlHeader.Controls.Add(btnDelete);
            //pnlHeader.Controls.Add(btnPrint);
            //pnlHeader.Controls.Add(lblDate);
            pnlHeader.Controls.Add(lblRack);
            //lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top)));
            btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // grdStockItems
            // 
            DataGridView grdStockItem = new DataGridView();
            GenerateDatagridView(grdStockItem);
            grdStockItem.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grdStockItem.ColumnHeadersHeight = 35;
            grdStockItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            grdStockItem.EnableHeadersVisualStyles = false;
            grdStockItem.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grdStockItem.RowTemplate.Height = 35;
            grdStockItem.RowHeadersVisible = false;
            grdStockItem.BackgroundColor = Color.White;
            grdStockItem.Dock = System.Windows.Forms.DockStyle.Fill;
            grdStockItem.Location = new System.Drawing.Point(0, 39);
            grdStockItem.Name = "grdStockItems";
            grdStockItem.Size = new System.Drawing.Size(1258, 201);
            grdStockItem.TabIndex = 1;
            grdStockItem.AllowUserToAddRows = false;
            grdStockItem.AllowUserToDeleteRows = false;
            grdStockItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            SharedFunctions.SetGridStyle(grdStockItem);
            foreach (RackItemsVM i in item.RackItems)
            {
                grdStockItem.Rows.Add(i.ItemId, i.ItemName);
            }
            // 
            // pnlDetail
            // 
            Panel pnlDetail = new Panel();
            pnlDetail.Tag = item.RackId;

            pnlDetail.Controls.Add(grdStockItem);
            pnlDetail.Controls.Add(pnlHeader);
            pnlDetail.Location = new System.Drawing.Point(3, 3);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Size = new System.Drawing.Size(1258, 240);
            pnlDetail.TabIndex = 0;
            flowPanel.Controls.Add(pnlDetail);
            pnlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            flowPanel.Refresh();
            SharedFunctions.SetGridStyle(grdStockItem);
        }
        private void GenerateDatagridView(DataGridView SourceGrid)
        {
            DataGridViewTextBoxColumn colItemId;
            DataGridViewTextBoxColumn colItemName;

            colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();

            SourceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colItemId,
            colItemName,
            });
            dataGridView1.Location = new System.Drawing.Point(15, 28);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new System.Drawing.Size(1258, 150);
            dataGridView1.TabIndex = 12;
            dataGridView1.Visible = false;
            // 
            // colItemId
            // 
            colItemId.HeaderText = "ItemId";
            colItemId.Name = "colItemId";
            colItemId.ReadOnly = true;
            colItemId.Visible = false;
            // 
            // colItemName
            // 
            colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colItemName.HeaderText = "Item";
            colItemName.Name = "colItemName";
            colItemName.ReadOnly = true;

        }
        private void flowPanel_SizeChanged(object sender, EventArgs e)
        {
            foreach (Panel p in flowPanel.Controls)
            {
                p.Width = flowPanel.Width - 23;
            }
        }
        private void flowPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            Panel AddedPanel = (Panel)e.Control;
            AddedPanel.Width = flowPanel.Width - 23;
        }
        #endregion

        #region ManageRacks
        private void btnAddRack_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddRack(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int RackId = (int)((Button)sender).Tag;
                Form f = SharedFunctions.OpenForm(new frmAddRack(RackId), this.MdiParent);
                f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                f.Show();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int RackId = (int)((Button)sender).Tag;
                if (RackId <= 0)
                    return;
                DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Rack?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == System.Windows.Forms.DialogResult.OK)
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Rack rack = unitOfWork.RackRepository.GetRackDetailsById(RackId);
                        if (rack == null)
                        {
                            SharedFunctions.MessageBoxShow("Unable to get rack detail", MessageType.Warning);
                            return;
                        }
                        rack.IsActive = false;
                        rack.Expiry = DateTime.Now;
                        if (rack.RackItem != null && rack.RackItem.Count() > 0)
                        {
                            rack.RackItem.ToList().ForEach(x => { x.IsActive = false; x.DeletedAt = DateTime.Now; });
                        }
                        AuditLogTask.CreateAuditLog(unitOfWork, rack.Name, AuditModule.Rack, AuditAction.deleted);
                        unitOfWork.Save();
                        //SOT
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, rack, ModuleName.Rack, ModuleAction.Delete, rack.RackId);
                        btnRefresh.PerformClick();
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region Pagination
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
                    if (_pageNo <= 0 || _pageNo > racks.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.pageNo = _pageNo;
                    LoadRacks();
                }
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo++;
            LoadRacks();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo--;
            LoadRacks();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            unitOfWork = new UnitOfWork();
            LoadRacks();
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadRacks();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = racks.PageCount;
            LoadRacks();
        }
        #endregion

        #region KeysDetials
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.F5))
            {
                btnRefresh.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
    }
}