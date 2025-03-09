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
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmStockAdjustments : Form
    {
        enum FilterType { Default = 1, Date = 2, Item = 3 };
        FilterType filter = FilterType.Default;
        UnitOfWork unitOfWork;
        int pageNo = 1;
        int srno = 1;
        IPagedList<Adjustment> AdjustmentList;
        List<Adjustment> objListAdj = null;

        int SelectedItemId = 0;
        public frmStockAdjustments()
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }

        private void frmManageStocks_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnAddNewAdjustment });
            filter = FilterType.Date;
            dtpFrom.Checked = true;
            dtpTo.Checked = true;
            LoadAdjustments();
            flowPanel_SizeChanged(null, null);
        }
        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            if (this.SelectedItemId > 0)
            {
                filter = FilterType.Item;
                LoadAdjustments();
            }
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.F5))
            {
                btnRefresh.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.J))
            {
                btnAddNewAdjustment.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadAdjustments()
        {
            objListAdj = new List<Adjustment>();
            flowPanel.Controls.Clear();

            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (dtpFrom.Checked)
                fromDate = dtpFrom.Value;
            if (dtpTo.Checked)
                toDate = dtpTo.Value;

            using (unitOfWork = new UnitOfWork())
            {
                if (filter == FilterType.Date)
                {
                    objListAdj = unitOfWork.AdjustmentRepository.GetAdjustmentSum(fromDate, toDate);
                    AdjustmentList = unitOfWork.AdjustmentRepository.GetAdjustments(fromDate, toDate, pageNo, SharedVariables.PageSize);
                }
                else if (filter == FilterType.Item)
                {
                    objListAdj = unitOfWork.AdjustmentRepository.GetAdjustmentSum(this.SelectedItemId);
                    AdjustmentList = unitOfWork.AdjustmentRepository.GetAdjustments(this.SelectedItemId, this.pageNo, SharedVariables.PageSize);
                }
                else if (filter == FilterType.Default)
                {
                    objListAdj = unitOfWork.AdjustmentRepository.GetAdjustmentSum();
                    AdjustmentList = unitOfWork.AdjustmentRepository.GetAdjustments(pageNo, SharedVariables.PageSize);
                }
            }
            double TotalSum = 0;
            TotalSum = CalculateTotalAdjustment(TotalSum);
            lblTotalAdjustment.Text = TotalSum.ToString();
            foreach (Adjustment a in AdjustmentList)
            {
                BuildPanel(a);
            }
            btnNext.Enabled = AdjustmentList.HasNextPage ? true : false;
            btnPrevious.Enabled = AdjustmentList.HasPreviousPage ? true : false;
            SharedFunctions.ShowPageNo(lblPageNo, pageNo, AdjustmentList.PageCount);
            this.SelectedItemId = 0;
        }

        private double CalculateTotalAdjustment(double TotalSum)
        {
            try
            {
                foreach (var item in objListAdj.Select(x => x.AdjustmentItems))
                {
                    foreach (var i in item.Where(x => x.IsActive).ToList())
                    {
                        double qty = i.Unit == 0 ? i.Quantity / i.Item.ConversionUnit : i.Quantity;
                        double costprice = i.Unit == 0 ? i.CostPrice * i.Item.ConversionUnit : i.CostPrice;
                        TotalSum += qty * costprice;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return TotalSum;
        }
        private void BuildPanel(Adjustment a)
        {
            Panel pnlHeader = new Panel();
            // 
            // lblDate
            // 
            Label lblset = new Label();
            lblset.AutoSize = true;
            lblset.Location = new System.Drawing.Point(9, 15);
            lblset.Name = "lblset";
            lblset.Size = new System.Drawing.Size(35, 13);
            lblset.TabIndex = 1;
            lblset.Text = "DateTime:";
            // 
            // lblDate
            // 
            Label lblDate = new Label();
            lblDate.AutoSize = true;
            lblDate.Location = new System.Drawing.Point(9, 15);
            lblDate.Name = "lblDate";
            lblDate.Size = new System.Drawing.Size(35, 13);
            lblDate.TabIndex = 1;
            lblDate.Text = lblset.Text + " " + a.CreateDate.ToString();
            // 
            // 
            // lblAddedBy
            // 
            Label lblAddedBy = new Label();
            lblAddedBy.AutoSize = true;
            lblAddedBy.Name = "lblAddedBy";
            lblAddedBy.Location = new System.Drawing.Point(1205 - 500, 15);
            lblAddedBy.Size = new System.Drawing.Size(35, 13);
            lblAddedBy.TabIndex = 1;
            lblAddedBy.Text = "Added By: " + a.User.UserName;
            // 
            // btnPrint
            // 
            Button btnPrint = new Button();
            btnPrint.Location = new System.Drawing.Point(1205, 0); // 1205
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new System.Drawing.Size(51, 39);
            btnPrint.TabIndex = 1;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Tag = a.AdjustmentId;
            btnPrint.Click += new EventHandler(btnPrint_Click);
            // 
            // btnDelete
            // 
            Button btnDelete = new Button();
            btnDelete.Location = new System.Drawing.Point(1152, 0); // 1205
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(51, 39);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Tag = a.AdjustmentId;
            btnDelete.Click += new EventHandler(btnDelete_Click);
            // 
            // btnEdit
            //
            Button btnEdit = new Button();
            btnEdit.Location = new System.Drawing.Point(1095, 0); // 1152
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(51, 39);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Tag = a.AdjustmentId;
            btnEdit.Click += new EventHandler(btnEdit_Click);

            pnlHeader.BackColor = Color.FromArgb(241, 241, 241);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1258, 39);
            pnlHeader.TabIndex = 0;
            btnEdit.Dock = DockStyle.Left;
            btnPrint.Dock = DockStyle.Left;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnEdit.FlatStyle = FlatStyle.Flat;
            pnlHeader.Controls.Add(btnEdit);
            //pnlHeader.Controls.Add(btnDelete);
            pnlHeader.Controls.Add(btnPrint);
            //pnlHeader.Controls.Add(lblDate);
            //pnlHeader.Controls.Add(lblAddedBy);
            //pnlHeader.Controls.Add(lblSupplier);
            //lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            //btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            //btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            //btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // grdStockItems
            // 
            DataGridView grdItem = new DataGridView();
            GenerateDatagridView(grdItem);
            SharedFunctions.SetGridStyle(grdItem);
            //grdItem.EnableHeadersVisualStyles = false;
            //grdItem.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //grdItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            //grdItem.ColumnHeadersHeight = 35;
            //grdItem.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            //grdItem.RowTemplate.Height = 35;
            //grdItem.RowHeadersVisible = false;
            //grdItem.BackgroundColor = Color.White;
            grdItem.Dock = System.Windows.Forms.DockStyle.Fill;
            grdItem.Location = new System.Drawing.Point(0, 39);
            grdItem.Name = "grdStockItems";
            grdItem.Size = new System.Drawing.Size(1558, 201);
            grdItem.TabIndex = 1;
            grdItem.AllowUserToAddRows = false;
            grdItem.AllowUserToDeleteRows = false;
            //grdItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (AdjustmentItem i in a.AdjustmentItems)
            {
                if (i.IsActive)
                {
                    i.UnitString = i.Unit == 0 ? i.Item.Unit : "Units";
                    i.Quantity = i.Unit == 0 ? i.Quantity / i.Item.ConversionUnit : i.Quantity;
                    i.CostPrice = i.Unit == 0 ? i.CostPrice * i.Item.ConversionUnit : i.CostPrice;
                    i.AdjustmentValue = i.Quantity * i.CostPrice;
                    grdItem.Rows.Add(srno, i.Item.ItemID, i.Item.ItemName, i.Batch != null ? i.Batch.BatchName : null, i.CostPrice,i.Quantity, i.AdjustmentValue, i.Reason, i.CreateDate);
                }
                srno++;
            }
            SharedFunctions.SetGridStyle(grdItem);

            // 
            // pnlDetailBottom
            // 
            Panel pnlDetailBottom = new Panel();
            Label lblTotal = new Label();
            lblTotal.AutoSize = true;
            lblTotal.Name = "lblTotal";
            lblTotal.Location = new System.Drawing.Point(1205 - 500, 15);
            lblTotal.Size = new System.Drawing.Size(35, 13);
            //lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));

            lblTotal.Text = "Total Adjustment Value: " + a.AdjustmentItems.Where(x => x.IsActive).Sum(x => x.AdjustmentValue).ToString();
            pnlDetailBottom.BackColor = Color.FromArgb(241, 241, 241);
            pnlDetailBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlDetailBottom.Name = "pnlDetailBottom";
            pnlDetailBottom.Size = new System.Drawing.Size(1558, 39);
            pnlDetailBottom.TabIndex = 0;
            //pnlDetailBottom.Controls.Add(lblTotal);
            // 
            // pnlDetail
            // 
            Panel pnlDetail = new Panel();
            pnlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            pnlDetail.BackColor = Color.AliceBlue;
            pnlHeader.BackColor = Color.AliceBlue;
            pnlDetailBottom.BackColor = Color.AliceBlue;
            pnlDetail.Dock = DockStyle.Top;
            pnlDetail.Controls.Add(grdItem);
            pnlDetail.Controls.Add(pnlHeader);
            pnlDetail.Controls.Add(pnlDetailBottom);
            pnlDetail.Location = new System.Drawing.Point(3, 3);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Size = new System.Drawing.Size(1558, 240);
            pnlDetail.TabIndex = 0;




            grdItem.BackgroundColor = Color.AliceBlue;
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.Controls.Add(pnlDetail);
        }
        private void GenerateDatagridView(DataGridView SourceGrid)
        {
            srno = 1;
            DataGridViewTextBoxColumn colSrNo;
            DataGridViewTextBoxColumn colItemId;
            DataGridViewTextBoxColumn colItemName;
            DataGridViewTextBoxColumn colBatch;
            DataGridViewTextBoxColumn colCostPrice;
            //DataGridViewTextBoxColumn colUnit;
            DataGridViewTextBoxColumn colAdjustment;
            DataGridViewTextBoxColumn colAdjustmentPrice;
            DataGridViewTextBoxColumn colReason;
            DataGridViewTextBoxColumn colDate;

            colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSrNo.HeaderText = "Sr#";
            colSrNo.ReadOnly = true;
            colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCostPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colUnit = new DataGridViewTextBoxColumn();
            colAdjustment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colAdjustmentPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();

            SourceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colSrNo,
            colItemId,
            colItemName,
            colBatch,
            colCostPrice,
            //colUnit,
            colAdjustment,
            colAdjustmentPrice,
            colReason,
            colDate
            });
            grdITems.Location = new System.Drawing.Point(15, 28);
            grdITems.Name = "grdITems";
            grdITems.Size = new System.Drawing.Size(1258, 150);
            grdITems.TabIndex = 12;
            grdITems.Visible = false;
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
            colItemName.MinimumWidth = 300;
            
            colItemName.ReadOnly = true;
            // 
            // colQuantity
            // 
            colAdjustment.HeaderText = "Adjustment";
            colAdjustment.Name = "colAdjustment";
            colAdjustment.ReadOnly = true;

            // 
            // colAdjustmentPrice
            // 
            colAdjustmentPrice.HeaderText = "Adjustment Price";
            colAdjustmentPrice.Name = "colAdjustmentPrice";
            colAdjustmentPrice.ReadOnly = true;

            // 
            // colBatch
            // 
            colBatch.HeaderText = "Batch";
            colBatch.Name = "colBatch";
            colBatch.ReadOnly = true;
            // 
            // colCostPrice
            // 
            colCostPrice.HeaderText = "Cost Price";
            colCostPrice.Name = "colCostPrice";
            colCostPrice.ReadOnly = true;
            // 
            // colUnit
            // 
            /*colUnit.HeaderText = "Unit";
            colUnit.Name = "colUnit";
            colUnit.ReadOnly = true;*/
            //
            // colReason
            //
            colReason.HeaderText = "Reason";
            colReason.Name = "colReason";
            colReason.ReadOnly = true;
            //
            // colAdjustmentDate
            //
            colDate.HeaderText = "Adjustment Date";
            colDate.Name = "colDate";
            colDate.ReadOnly = true;
        }
        private void flowPanel_SizeChanged(object sender, EventArgs e)
        {
            foreach (Panel p in flowPanel.Controls)
            {
                p.Width = flowPanel.Width - 23;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            long Adjustment = (long)((Button)sender).Tag;
            Form f = SharedFunctions.OpenForm(new frmStockAdjustment(Adjustment), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(frmStockAdjustment_Closed);
        }

        private void frmStockAdjustment_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult rs = MessageBox.Show("Are You Sure, You Want to Delete This Adjustment", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (rs == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                long AdjustmentId = (long)((Button)sender).Tag;
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.AdjustmentRepository.SetInactive(AdjustmentId);
                    AuditLogTask.CreateAuditLog(unitOfWork, AuditModule.Stock_Adjustment, AuditAction.deleted);
                    //SOT
                    OfflineJsonTask.SaveOfflineTask(unitOfWork, new Adjustment { AdjustmentId = AdjustmentId }, ModuleName.StockAdjustment, ModuleAction.Delete, AdjustmentId);

                }
                MessageBox.Show("Adjustment Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnRefresh.PerformClick();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            long Adjustment = (long)((Button)sender).Tag;
            Reports.AdjustmentsViewer v = new Reports.AdjustmentsViewer(Adjustment);
            v.Show();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo++;
            LoadAdjustments();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo--;
            LoadAdjustments();
        }
        private void Clear()
        {
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            dtpTo.Value = dtpFrom.Value = DateTime.Now;
            filter = FilterType.Default;
            pageNo = 1;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnAddNewAdjustment_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmStockAdjustment(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void ToggleEvents(bool Register)
        {
            if (Register)
            {
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpTo.ValueChanged -= dtpTo_ValueChanged;
            }
            else
            {
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                dtpTo.ValueChanged += dtpTo_ValueChanged;
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //ToggleEvents(false);
            Clear();
            filter = FilterType.Date;
            if (SharedVariables.manageStockAdjsRef == true)
            {
                dtpFrom.Checked = true;
                dtpTo.Checked = true;
            }
            else
            {
                dtpFrom.Checked = false;
                dtpTo.Checked = false;
            }
            LoadAdjustments();
            SharedVariables.manageStockAdjsRef = false;
            flowPanel_SizeChanged(null, null);
            //ToggleEvents(true);
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadAdjustments();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = AdjustmentList.PageCount;
            LoadAdjustments();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            pageNo = 1;
            LoadAdjustments();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            pageNo = 1;
            LoadAdjustments();
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmStockAdjustmentsShortCuts f = new ShortCutDialogs.frmStockAdjustmentsShortCuts();
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
                    if (_pageNo <= 0 || _pageNo > AdjustmentList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.pageNo = _pageNo;
                    LoadAdjustments();
                }
            }
        }

        private void flowPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowPanel_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}