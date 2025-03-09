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
using System.Runtime.CompilerServices;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;


namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmStockReturn : Form
    {
        UnitOfWork unitOfWork;
        int pageNo = 1;
        IPagedList<StockReturnVM> StockReturnList;
        bool isDateChanged = false;
        int srno = 1;
        #region LoadDetials
        public frmStockReturn()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        private void frmStockReturn_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnAddNewStockReturn });
            dtpFrom.Checked = true;
            dtpTo.Checked = true;
            LoadRacks();
        }
        private void LoadRacks()
        {
            flowPanel.Controls.Clear();
            DateTime? dtFrom = null;
            if (dtpFrom.Checked)
                dtFrom = dtpFrom.Value;
            DateTime? dtTo = null;
            if (dtpTo.Checked)
                dtTo = dtpTo.Value;
            using (unitOfWork = new UnitOfWork())
            {
                StockReturnList = unitOfWork.StockReturnRepository.GetStockReturn(
                    FromDate: dtFrom,
                    ToDate: dtTo,
                    ItemName: txtSearchByName.Text.Trim().ToLower(),
                    StockDocumentNo: txtStockDocNo.Text,
                    StockRetDocumentNo: txtRtStockDocNo.Text,
                    pageNo: pageNo,
                    PageSize: SharedVariables.PageSize);
            }
            foreach (StockReturnVM s in StockReturnList)
            {
                this.flowPanel.SuspendLayout();
                BuildPanel(s);
                this.flowPanel.ResumeLayout(true);
            }
            btnLastPage.Enabled = btnNext.Enabled = StockReturnList.HasNextPage ? true : false;
            btnFirstPage.Enabled = btnPrevious.Enabled = StockReturnList.HasPreviousPage ? true : false;
            SharedFunctions.ShowPageNo(lblPageNo, pageNo, StockReturnList.PageCount);
        }   
        #endregion

        #region ShortCutKeys
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            {
                btnAddNewStockReturn.PerformClick();
                return true;
            }
            if (keyData == (Keys.F5))
            {
                btnRefresh.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region LoadPanel
        private void BuildPanel(StockReturnVM s)
        {
            Panel pnlHeader = new Panel();
            // 
            // lblStockDocNo
            // 
            Label lblStockDocNo = new Label();
            lblStockDocNo.AutoSize = true;
            lblStockDocNo.Location = new System.Drawing.Point(14, 15);
            lblStockDocNo.Name = "lblStockDocNo";
            lblStockDocNo.Size = new System.Drawing.Size(35, 13);
            lblStockDocNo.TabIndex = 1;
            ImplementStockDocNo(s, lblStockDocNo);
            // 
            // lblStockDocNo
            // 
            Label lblStockReturnNo = new Label();
            lblStockReturnNo.AutoSize = true;
            lblStockReturnNo.Location = new System.Drawing.Point(200, 15);
            lblStockReturnNo.Name = "lblStockReturnNo";
            lblStockReturnNo.Size = new System.Drawing.Size(35, 13);
            lblStockReturnNo.TabIndex = 1;
            lblStockReturnNo.Text = "Return Document #: " + s.DocumentNo.ToString();
            // 
            // lblDate
            // 
            Label lblDate = new Label();
            lblDate.AutoSize = true;
            lblDate.Location = new System.Drawing.Point(523, 15);
            lblDate.Name = "lblDate";
            lblDate.Size = new System.Drawing.Size(35, 13);
            lblDate.TabIndex = 1;
            lblDate.Text = s.CreateDate.ToString();

            // 
            // lblAddedBy
            // 
            Label lblAddedBy = new Label();
            lblAddedBy.AutoSize = true;
            lblAddedBy.Location = new System.Drawing.Point(1205 - 500, 15);
            lblAddedBy.Name = "lblAddedBy";
            lblAddedBy.Size = new System.Drawing.Size(35, 13);
            lblAddedBy.TabIndex = 1;
            lblAddedBy.Text = "Added By: " + s.User.UserName;
            // 
            // btnPrint
            // 
            Button btnPrint = new Button();
            btnPrint.Location = new System.Drawing.Point(1205, 0);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new System.Drawing.Size(51, 39);
            btnPrint.TabIndex = 1;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Tag = s.StockReturnId;
            btnPrint.Click += new EventHandler(btnPrint_Click);
            // 
            // btnDelete
            // 
            Button btnDelete = new Button();
            btnDelete.Location = new System.Drawing.Point(1152, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(51, 39);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Tag = s.StockReturnId;
            btnDelete.Click += new EventHandler((sender,e) => btnDelete_Click(sender, e, s.DocumentNo));
            // 
            // btnEdit
            //
            Button btnEdit = new Button();
            btnEdit.Location = new System.Drawing.Point(1099, 0);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(51, 39);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Tag = s.StockReturnId;
            btnEdit.Click += new EventHandler(btnEdit_Click);

            pnlHeader.BackColor = Color.FromArgb(241, 241, 241);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1258, 39);
            pnlHeader.BackColor = Color.AliceBlue;
            ////pnlButton.BackColor = Color.AliceBlue;
            
            pnlHeader.TabIndex = 0;
            pnlHeader.Controls.Add(btnEdit);
            //pnlHeader.Controls.Add(btnDelete);
            pnlHeader.Controls.Add(btnPrint);
            //pnlHeader.Controls.Add(lblDate);
            //pnlHeader.Controls.Add(lblAddedBy);
            //pnlHeader.Controls.Add(lblStockDocNo);
            //pnlHeader.Controls.Add(lblStockReturnNo);


            Panel pnlButton = new Panel();
            // 
            // label8
            // 
            Label label8 = new Label();
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(1170, 10);
            label8.Size = new System.Drawing.Size(50, 10);
            label8.Name = "label8";
            label8.Text = "Total: " + s.GrandTotal.ToString();

            pnlButton.BackColor = Color.FromArgb(241, 241, 241);
            pnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlButton.Location = new System.Drawing.Point(1152, 0);
            pnlButton.Name = "pnlButton";
            pnlButton.Size = new System.Drawing.Size(1258, 39);
            pnlButton.TabIndex = 3;
            //pnlButton.Controls.Add(label8);
            btnEdit.Dock = DockStyle.Left;
            btnPrint.Dock = DockStyle.Left;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnPrint.FlatStyle = FlatStyle.Flat;
            //lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top)));
            //btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            //btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            //btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // grdStockItems
            // 
            DataGridView grdStockItem = new DataGridView();
            GenerateDatagridView(grdStockItem);
            SharedFunctions.SetGridStyle(grdStockItem);
            grdStockItem.Dock = System.Windows.Forms.DockStyle.Fill;
            grdStockItem.Location = new System.Drawing.Point(0, 39);
            grdStockItem.Name = "grdStockItems";
            grdStockItem.Size = new System.Drawing.Size(1258, 201);
            grdStockItem.TabIndex = 1;
            grdStockItem.AllowUserToAddRows = false;
            grdStockItem.AllowUserToDeleteRows = false;
            foreach (StockReturnItemVM i in s.StockReturnItems)
            {
                i.Quantity = i.Unit == 0 ? i.Quantity / i.Item.ConversionUnit : i.Quantity;
                i.UnitCost = i.Unit == 0 ? i.UnitCost * i.Item.ConversionUnit : i.UnitCost;
                grdStockItem.Rows.Add(
                    srno,
                    i.Item.ItemID,
                    i.Item.ItemName,
                    //i.Unit == 0 ? "Pack" : "Units",
                    //i.Item.ConversionUnit,
                    i.Quantity,
                    i.Batch.BatchName,
                    i.UnitCost,
                    i.DiscountValue,
                    i.SalesTax,
                    i.NetValue,
                    i.Discount,
                    i.SalesTax,
                    i.NetValue
                    );
                srno++;

            }
            //LoadSerial();
            // 
            // pnlDetail
            // 
            Panel pnlDetail = new Panel();
            pnlDetail.Tag = s.StockReturnId;

            pnlDetail.Controls.Add(grdStockItem);
            pnlDetail.Controls.Add(pnlHeader);
            pnlDetail.Controls.Add(pnlButton);
            pnlDetail.Location = new System.Drawing.Point(3, 3);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Size = new System.Drawing.Size(1658, 240);
            pnlDetail.TabIndex = 0;
            pnlDetail.BackColor = Color.AliceBlue;
            pnlDetail.Dock = DockStyle.Top;
            flowPanel.Controls.Add(pnlDetail);
            pnlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            SharedFunctions.SetGridStyle(grdStockItem);
            grdStockItem.Dock = DockStyle.Fill;
            grdStockItem.BackgroundColor = Color.AliceBlue;
            flowPanel.BackColor = Color.AliceBlue;
            flowPanel.Refresh();
        }
        private void ImplementStockDocNo(StockReturnVM s, Label lblStockDocNo)
        {
            try
            {
                lblStockDocNo.Text = "Supplier Invoice #: " + s.Stock.SupplierInvoiceNo;
            }
            catch (Exception) { }
        }
        private void GenerateDatagridView(DataGridView SourceGrid)
        {
            srno = 1;
            DataGridViewTextBoxColumn colSrNo;
            DataGridViewTextBoxColumn colItemId;
            DataGridViewTextBoxColumn colItemName;
            //DataGridViewTextBoxColumn colUnit;
            //DataGridViewTextBoxColumn colConvUnit;
            DataGridViewTextBoxColumn colQuantity;
            DataGridViewTextBoxColumn colBatch;
            DataGridViewTextBoxColumn colUnitCost;
            DataGridViewTextBoxColumn colDiscount;
            DataGridViewTextBoxColumn colSalesTax;
            DataGridViewTextBoxColumn colNetValue;
            DataGridViewTextBoxColumn colCreatedAt;

            colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSrNo.HeaderText = "Sr#";
            colSrNo.ReadOnly = true;
            colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colUnitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSalesTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colNetValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();

            SourceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colSrNo,
            colItemId,
            colItemName,
            //colUnit,
            //colConvUnit,
            colQuantity,
            colBatch,
            colUnitCost,
            colDiscount,
            colSalesTax,
            colNetValue});
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
            colItemName.MinimumWidth = 150;
            colItemName.Width = 200 ;

            // 
            // colUnit
            // 
            /*colUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colUnit.HeaderText = "Unit";
            colUnit.Width = 70;
            colUnit.Name = "colUnit";
            colUnit.ReadOnly = true;*/

            // 
            // colConvUnit
            // 
      /*      colConvUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colConvUnit.MinimumWidth = 40;
            colConvUnit.HeaderText = "Conv.Unit";
            colConvUnit.Name = "colConvUnit";
            colConvUnit.ReadOnly = true;*/
            // 
            // colQuantity
            // 
            //colQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colQuantity.Width = 120;
            colQuantity.MinimumWidth = 120;
            colQuantity.HeaderText = "Return Quantity";
            colQuantity.Name = "colQuantity";
            colQuantity.ReadOnly = true;
            // 
            // colBatch
            // 
            colBatch.HeaderText = "Batch";
            colBatch.Name = "colBatch";
            colBatch.MinimumWidth = 150;
            colBatch.Width = 150;
            colBatch.ReadOnly = true;
            // 
            // colUnitCost
            // 
            colUnitCost.Width = 120;
            colUnitCost.MinimumWidth = 120;
            colUnitCost.HeaderText = "UnitCost";
            colUnitCost.Name = "colUnitCost";
            colUnitCost.ReadOnly = true;
            // 
            // colDiscount
            // 
            colDiscount.Width = 120;
            colDiscount.MinimumWidth = 120;
            colDiscount.HeaderText = "Discount";
            colDiscount.Name = "colDiscount";
            colDiscount.ReadOnly = true;
            // 
            // colSalesTax
            // 
            colSalesTax.Width = 120;
            colSalesTax.MinimumWidth = 120;
            colSalesTax.HeaderText = "Sales Tax";
            colSalesTax.Name = "colSalesTax";
            colSalesTax.ReadOnly = true;
            // 
            // colNetValue
            // 
            colNetValue.Width = 120;
            colNetValue.MinimumWidth = 120;
            colNetValue.HeaderText = "Return Value";
            colNetValue.Name = "colNetValue";
            colNetValue.ReadOnly = true;
            // 
            // colCreatedAt
            // 
            colCreatedAt.HeaderText = "Create At";
            colCreatedAt.Name = "colCreatedAt";
            colCreatedAt.ReadOnly = true;
            colNetValue.Width = 120;
            colNetValue.MinimumWidth = 120;
        }
        private void flowPanel_SizeChanged(object sender, EventArgs e)
        {
            foreach (Panel p in flowPanel.Controls)
            {
                p.Width = flowPanel.Width - 23;
            }
        }
        private void flowPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        private void flowPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            Panel AddedPanel = (Panel)e.Control;
            AddedPanel.Width = flowPanel.Width - 23;
        }
        #endregion

        #region DataGridEvents
        private void btnEdit_Click(object sender, EventArgs e)
        {
            long StockReturnId = (long)((Button)sender).Tag;
            Form f = SharedFunctions.OpenForm(new frmStockReturnItem(StockReturnId), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
            f.Show();
        }
        private void btnDelete_Click(object sender, EventArgs e, long DocumentNo)
        {
            DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Record", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (rs == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            long Stockid = (long)((Button)sender).Tag;
            using (unitOfWork = new UnitOfWork())
            {
                unitOfWork.StockReturnRepository.SetInactive(Stockid);
                AuditLogTask.CreateAuditLog(unitOfWork, DocumentNo.ToString(), AuditModule.Stock_Return, AuditAction.deleted);
                //SOT
                OfflineJsonTask.SaveOfflineTask(unitOfWork, new StockReturn() { StockReturnId = Stockid }, ModuleName.StockReturn, ModuleAction.Delete, Stockid);
            }
            foreach (Panel p in flowPanel.Controls)
            {
                if (Convert.ToInt64(p.Tag) == Stockid)
                {
                    flowPanel.Controls.Remove(p);
                }
            }
            btnRefresh.PerformClick();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            long Stockid = (long)((Button)sender).Tag;
            StockDetailViewer v = new StockDetailViewer(Stockid, true);
            v.Show();
        }
        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmManageStockShortCuts f = new ShortCutDialogs.frmManageStockShortCuts();
            f.ShowDialog();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        #endregion

        #region HeaderButtons
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            isDateChanged = false;
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            if (SharedVariables.manageStockReturnRef == true)
            {
                dtpFrom.Checked = true;
                dtpTo.Checked = true;
            }
            else
            {
                dtpFrom.Checked = false;
                dtpTo.Checked = false;
            }
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            unitOfWork = new UnitOfWork();
            txtSearchByName.Text = "";
            txtStockDocNo.Text = "";
            LoadRacks();
            SharedVariables.manageStockReturnRef = false;
        }
        private void btnAddNewStock_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmStockReturnItem(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadRacks();
        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadRacks();
        }
        #endregion

        #region PaginationButtons
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
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadRacks();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = StockReturnList.PageCount;
            LoadRacks();
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
                    if (_pageNo <= 0 || _pageNo > StockReturnList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.pageNo = _pageNo;
                    LoadRacks();
                }
            }
        }
        #endregion

        #region Filters
        private void txtSearchByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pageNo = 1;
                LoadRacks();
            }
        }
        private void txtStockDocNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private void txtStockDocNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    pageNo = 1;
                    LoadRacks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void txtStockDocNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRtStockDocNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    pageNo = 1;
                    LoadRacks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}