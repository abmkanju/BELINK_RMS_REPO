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
using Pharmacy.Core.ViewModels;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;
using System.Data.SqlClient;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmStockConsumptions : Form
    {
        UnitOfWork unitOfWork;
        int pageNo = 1;
        bool IsFilterApplied = false;
        IPagedList<StockConsumptionVM> ConsumptionsList;
        int SelectedItemId = 0;
        int srno = 1;

        #region LoadDetails
        public frmStockConsumptions()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        private void frmManageStocks_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SearchableCombobox(cmbItems);
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnAddNewConsumption });
            dtpFrom.Checked = true;
            dtpTo.Checked = true;
            LoadControls();
            LoadConsumptions();
            flowPanel_SizeChanged(null, null);
        }
        private void LoadControls()
        {
            //-------------- Register Events --------------//
            cmbItems.SelectedIndexChanged -= cmbItems_SelectedIndexChanged;
            cmbUsers.SelectedIndexChanged -= cmbUsers_SelectedIndexChanged;
            cmbStore.SelectedIndexChanged -= cmbStore_SelectedIndexChanged;
            try
            {
                LoadUsers();
                LoadItems();
                LoadStores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading users data. Please try again." + ex.Message, "Control Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                //-------------- Un Register Events --------------//
                cmbItems.SelectedIndexChanged += cmbItems_SelectedIndexChanged;
                cmbUsers.SelectedIndexChanged += cmbUsers_SelectedIndexChanged;
                cmbStore.SelectedIndexChanged += cmbStore_SelectedIndexChanged;
            }
        }

        private void LoadStores()
        {
            if (!SharedVariables.AdminPharmacySetting.IsEnableStores)
            {
                lblStore.Visible = false;
                cmbStore.Visible = false;
                return;
            }
            List<StoreVM> list = new List<StoreVM>();
            using (unitOfWork = new UnitOfWork())
            {
                list = unitOfWork.StoreRepository.GetAllStores();
            }
            StoreVM model = new StoreVM
            {
                StoreId = 0,
                Name = "Select All"
            };
            list.Insert(0, model);
            cmbStore.DataSource = list;
            cmbStore.ValueMember = "StoreId";
            cmbStore.DisplayMember = "Name";
        }
        private void LoadItems()
        {
            List<ItemsVM> list = new List<ItemsVM>();
            using (unitOfWork = new UnitOfWork())
            {
                list = unitOfWork.StockConsumptionRepository.GetStockConsumptionsItems();
            }
            ItemsVM model = new ItemsVM
            {
                ItemId = 0,
                ItemName = "Select All"
            };
            list.Insert(0, model);

            cmbItems.DataSource = list;
            cmbItems.DisplayMember = "ItemName";
            cmbItems.ValueMember = "ItemId";

        }
        private void LoadUsers()
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

            cmbUsers.DataSource = us;
            cmbUsers.DisplayMember = "UserName";
            cmbUsers.ValueMember = "UserId";
        }
        private void LoadConsumptions()
        {
            flowPanel.Controls.Clear();
            using (unitOfWork = new UnitOfWork())
            {
                DateTime? fromDate = null;
                DateTime? toDate = null;
                if (dtpFrom.Checked)
                    fromDate = dtpFrom.Value;
                if (dtpTo.Checked)
                    toDate = dtpTo.Value;

                ConsumptionsList = unitOfWork.StockConsumptionRepository.GenericSearchForStockConsumptions
                    (
                    fromDate: fromDate,
                    toDate: toDate,
                    UserId: SharedFunctions.StringToInt(cmbUsers.SelectedValue),
                    ItemId: SharedFunctions.StringToLong(cmbItems.SelectedValue),
                    InvoiceNo: SharedFunctions.StringToInt(txtInvoiceNo.Text.Trim()),
                    StoreId: !SharedVariables.AdminPharmacySetting.IsEnableStores ? 0 : SharedFunctions.StringToInt(cmbStore.SelectedValue),
                    PageNo: pageNo,
                    PageSize: SharedVariables.PageSize
                    );
            }
            foreach (StockConsumptionVM a in ConsumptionsList)
            {
                if (SharedVariables.AdminPharmacySetting.IsConsumptionFromMultipleBatches)
                {
                    int ItemId = a.StockConsumptionsList.GroupBy(x => x.ItemId).Where(x => x.Count() > 1).Select(y => y.Key).FirstOrDefault();
                    var stockConsumptionItem = a.StockConsumptionsList.Where(x => x.ItemId == ItemId).OrderBy(x=>x.StockConsumptionItemId).FirstOrDefault();
                    a.StockConsumptionsList.Where(x => x.ItemId == ItemId).ToList().ForEach(s => s.TotalCost = stockConsumptionItem.UnitPrice * s.Quantity);
                }
                BuildPanel(a);
            }
            btnLastPage.Enabled = btnNext.Enabled = ConsumptionsList.HasNextPage ? true : false;
            btnFirstPage.Enabled = btnPrevious.Enabled = ConsumptionsList.HasPreviousPage ? true : false;
            SharedFunctions.ShowPageNo(lblPageNo, pageNo, ConsumptionsList.PageCount);
        }
        private void BuildPanel(StockConsumptionVM a)
        {
            Panel pnlHeader = new Panel();

            // 
            // lblSupplier
            // 
            Label lblInvoiceNo = new Label();
            lblInvoiceNo.AutoSize = true;
            lblInvoiceNo.Location = new System.Drawing.Point(80, 15);
            lblInvoiceNo.Name = "lblInvoiceNo";
            lblInvoiceNo.Size = new System.Drawing.Size(90, 13);
            lblInvoiceNo.TabIndex = 1;
            lblInvoiceNo.Text = "Invoice No:" + (a.InvoiceRefNo == 0 ? "N/A" : a.InvoiceRefNo.ToString());
            // 
            // lblDate
            // 
            Label lblDate = new Label();
            lblDate.AutoSize = true;
            lblDate.Location = new System.Drawing.Point(250, 15);
            lblDate.Name = "lblDate";
            lblDate.Size = new System.Drawing.Size(35, 13);
            lblDate.TabIndex = 1;
            lblDate.Text = a.CreateDate.ToString();
            // 
            // lblAddedBy
            // 
            Label lblAddedBy = new Label();
            lblAddedBy.AutoSize = true;
            lblAddedBy.Location = new System.Drawing.Point(1205 - 500, 15);
            lblAddedBy.Name = "lblAddedBy";
            lblAddedBy.Size = new System.Drawing.Size(35, 13);
            lblAddedBy.TabIndex = 1;
            lblAddedBy.Text = "Added By: " + a.UserName;
            // 
            // btnPrint
            // 
            Button btnPrint = new Button();
            btnPrint.Location = new System.Drawing.Point(1205, 0); // 1205
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new System.Drawing.Size(71, 39);
            btnPrint.TabIndex = 1;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Tag = a.StockConsumptionId;
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
            btnDelete.Tag = a.StockConsumptionId;
            btnDelete.Click += new EventHandler(btnDelete_Click);

            // 
            // btnEdit
            //
            Button btnEdit = new Button();
            btnEdit.Location = new System.Drawing.Point(1100, 0); // 1152
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(71, 39);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Tag = a.StockConsumptionId;
            btnEdit.Click += new EventHandler(btnEdit_Click);

            // 
            // btnReturn
            //
            Button btnReturn = new Button();
            btnReturn.Location = new System.Drawing.Point(1048, 0); // 1152
            btnReturn.Name = "btnEdit";
            btnReturn.Size = new System.Drawing.Size(71, 39);
            btnReturn.TabIndex = 1;
            btnReturn.Text = "Return";
            btnReturn.UseVisualStyleBackColor = true;
            btnReturn.Tag = a.InvoiceId;
            btnReturn.Click += new EventHandler(btnReturn_Click);

            pnlHeader.BackColor = Color.AliceBlue;
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1258, 39);
            pnlHeader.TabIndex = 0;
            pnlHeader.Controls.Add(btnReturn);
            pnlHeader.Controls.Add(btnEdit);
            //pnlHeader.Controls.Add(btnDelete);
            pnlHeader.Controls.Add(btnPrint);
            //pnlHeader.Controls.Add(lblDate);
            //pnlHeader.Controls.Add(lblAddedBy);
            //pnlHeader.Controls.Add(lblInvoiceNo);
            //lblInvoiceNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top)));
            //lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top)));
            //btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //btnReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnEdit.Dock = DockStyle.Left;
            btnPrint.Dock = DockStyle.Left;
            btnReturn.Dock = DockStyle.Left;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnReturn.FlatStyle = FlatStyle.Flat;
            // 
            // 
            DataGridView grdItem = new DataGridView();
            GenerateDatagridView(grdItem);
            SharedFunctions.SetGridStyle(grdItem);
            //grdItem.EnableHeadersVisualStyles = false;
            //grdItem.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //grdItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            grdItem.ColumnHeadersHeight = 35;
            //grdItem.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            //grdItem.RowTemplate.Height = 35;
            //grdItem.RowHeadersVisible = false;
            //grdItem.BackgroundColor = Color.White;
            grdItem.Dock = System.Windows.Forms.DockStyle.Fill;
            grdItem.Location = new System.Drawing.Point(0, 39);
            grdItem.Name = "grdStockItems";
            grdItem.Size = new System.Drawing.Size(1558, 201);
            grdItem.TabIndex = 1;
            grdItem.BackgroundColor = Color.AliceBlue;
            grdItem.AllowUserToAddRows = false;
            grdItem.AllowUserToDeleteRows = false;

            //grdItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (StockConsumptionItemVM i in a.StockConsumptionsList)
            {
                //colItemId;
                //colItemName;
                //colConsumedQuantity
                //colBatchId;
                //colBatch;
                //colConsumptionType;
                //colUser;
                //colComment;
                //colTotalCost;
                //colCreatedAt;
                double discountedPrice = getDiscountedPrice(i.UnitPrice, i.ConversionUnit, i.UnitString, i.BatchId, i.ItemId);
                if (discountedPrice == i.TotalCost) { }
                else { i.TotalCost = discountedPrice * i.Quantity; }

                if (i.IsActive)
                {
                    if (i.ConsumptionTypeString.ToLower() == "sales" && a.InvoiceId > 0)
                    {
                        btnDelete.Enabled = false;
                        btnEdit.Enabled = false;
                        btnReturn.Enabled = true;
                    }
                    else
                    {
                        btnReturn.Enabled = false;
                    }
                    grdItem.Rows.Add(srno, i.ItemId, i.ItemName, i.BatchName, i.UnitString, i.Store != null ? i.Store.Name : "", i.ConversionUnit, i.Quantity, i.ConsumptionTypeString, i.User.UserName, i.Comment, i.UnitString.ToLower() == "pack" ? i.UnitPrice * i.ConversionUnit : i.UnitPrice, discountedPrice, i.TotalCost, i.CreateDate);
                }
                srno++;
            }

            // 
            // pnlDetail
            // 
            Panel pnlDetail = new Panel();
            grdItem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdItem.ColumnHeadersHeight = 50;
            pnlDetail.Controls.Add(grdItem);
            pnlDetail.Controls.Add(pnlHeader);
            pnlDetail.Location = new System.Drawing.Point(3, 3);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Size = new System.Drawing.Size(1558, 240);
            pnlDetail.TabIndex = 0;
            flowPanel.Controls.Add(pnlDetail);
            pnlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
        }

        private DataTable get(string qry)
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            con.Open();
            SqlCommand com = new SqlCommand(qry, con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = com;
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public double getDiscountedPrice(double unitPrice, int convUnit, string unitString, int batchId, int ItemId)
        {
            string unit = unitString;
            Double UnitCost = 0;
            Double DiscountedPrice = 0;
            DataTable dt = get("Select Discount ,UnitCost, PercDiscType, TotalCost, Quantity, RetailPrice from StockItems where Item_ItemID=" + ItemId + "and Batch_BatchID =" + batchId);

            if (dt.Rows.Count > 0)
            {
                int stockqty = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Quantity"].ToString());
                Double Discount = Convert.ToDouble(dt.Rows[dt.Rows.Count - 1]["Discount"].ToString());
                UnitCost = unitPrice;
                int DiscountType = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["PercDiscType"].ToString());
                if (DiscountType == 0)
                {
                    Discount = ((Discount / 100) * UnitCost);
                    if (unit.ToLower() == "pack")
                    {
                        DiscountedPrice = Math.Round(((UnitCost - Discount) * convUnit), 2);
                    }
                    else
                    {
                        DiscountedPrice = Math.Round(((UnitCost - Discount)), 2);
                    }
                }
                else
                {
                    if (unit.ToLower() == "pack")
                    {
                        DiscountedPrice = Math.Round(((UnitCost - Discount/stockqty) * convUnit), 2);
                    }
                    else
                    {
                        DiscountedPrice = Math.Round(((UnitCost - Discount/stockqty)), 2);
                    }
                }
            }
            return DiscountedPrice;
        }

        private void toggleEvents(bool Register)
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
        #endregion

        #region HeaderButtons
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                toggleEvents(true);
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                if (SharedVariables.manageStockConsumRef == true)
                {
                    dtpTo.Checked = true;
                    dtpFrom.Checked = true;
                }
                else
                {
                    dtpTo.Checked = false;
                    dtpFrom.Checked = false;
                }
                //this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
                //uC_SearchItems1.SetText = "";
                //this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
                unitOfWork = new UnitOfWork();
                //Clear();
                pageNo = 1;
                LoadControls();
                LoadConsumptions();
                SharedVariables.manageStockConsumRef = false;
                flowPanel_SizeChanged(null, null);
                toggleEvents(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading data, please try again after reloading form.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnAddNewAdjustment_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmStockConsumption(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        #endregion

        #region ShortCutKeys
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.F5))
            {
                btnRefresh.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.N))
            {
                btnAddNewConsumption.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region DataGridEvents
        private void GenerateDatagridView(DataGridView SourceGrid)
        {
            srno = 1;
            DataGridViewTextBoxColumn colSrNo;
            DataGridViewTextBoxColumn colItemId;
            DataGridViewTextBoxColumn colItemName;
            DataGridViewTextBoxColumn colConsumedQuantity;
            //DataGridViewTextBoxColumn colBatchId;
            DataGridViewTextBoxColumn colBatch;

            DataGridViewTextBoxColumn colUnit;
            DataGridViewTextBoxColumn colStore;
            DataGridViewTextBoxColumn colConvUnit;
            DataGridViewTextBoxColumn colConsumptionType;
            DataGridViewTextBoxColumn colUser;
            DataGridViewTextBoxColumn colComment;
            DataGridViewTextBoxColumn colUnitCost;
            DataGridViewTextBoxColumn colTotalDiscount;
            DataGridViewTextBoxColumn colTotalCost;
            DataGridViewTextBoxColumn colCreatedAt;

            colSrNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSrNo.HeaderText = "Sr#";
            colSrNo.ReadOnly = true;

            colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colConsumedQuantity = new DataGridViewTextBoxColumn();
            
            //colBatchId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();

            colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colStore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();

            colConsumptionType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colComment = new DataGridViewTextBoxColumn();
            colUnitCost = new DataGridViewTextBoxColumn();
            colTotalCost = new DataGridViewTextBoxColumn();
            colTotalDiscount = new DataGridViewTextBoxColumn();
            colCreatedAt = new DataGridViewTextBoxColumn();

            SourceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colSrNo,
            colItemId,
            colItemName,
            //colBatchId,
            colBatch,
            colUnit,
            colStore,
            colConvUnit,
            colConsumedQuantity,
            colConsumptionType,
            colUser,
            colComment,
            colUnitCost,
            colTotalDiscount,
            colTotalCost,
            colCreatedAt
            });

            grdITems.Location = new System.Drawing.Point(15, 28);
            grdITems.Name = "grdITems";
            grdITems.Size = new System.Drawing.Size(1300, 150);
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
            //colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colItemName.HeaderText = "Item";
            colItemName.Name = "colItemName";
            colItemName.MinimumWidth = 144;
            colItemName.ReadOnly = true;
            // 
            // colQuantity
            // 
            colConsumedQuantity.HeaderText = "Consumed Quantity";
            colConsumedQuantity.Name = "colConsumedQuantity";
            colConsumedQuantity.ReadOnly = true;
            // 
            // colBatchId
            // 
            //colBatchId.HeaderText = "Batch Id";
            //colBatchId.Name = "colBatchId";
            //colBatchId.ReadOnly = true;
            // 
            // colBatch
            // 
            colBatch.HeaderText = "Batch";
            colBatch.Name = "colBatch";
            colBatch.ReadOnly = true;
            // 
            // colUnit
            // 
            colUnit.HeaderText = "Unit";
            colUnit.Name = "colUnit";
            colUnit.ReadOnly = true;
            // 
            // colStore
            // 
            colStore.HeaderText = "Store";
            colStore.Name = "colStore";
            colStore.Visible = SharedVariables.AdminPharmacySetting.IsEnableStores;
            colStore.ReadOnly = true;
            // 
            // colConvUnit
            // 
            colConvUnit.HeaderText = "Conv. Unit";
            colConvUnit.Name = "colConvUnit";
            colConvUnit.ReadOnly = true;

            //
            // colReason
            //
            colConsumptionType.HeaderText = "Consumption type";
            colConsumptionType.Name = "colConsumptionType";
            colConsumptionType.ReadOnly = true;
            //
            // colAdjustmentDate
            //
            colUser.HeaderText = "Added By";
            colUser.Name = "colUser";
            colUser.ReadOnly = true;
            //
            // colComment
            //
            colComment.HeaderText = "Comment";
            colComment.Name = "colComment";
            colComment.ReadOnly = true;
            //
            // colUnitCost
            //
            colUnitCost.HeaderText = "Unit Cost";
            colUnitCost.Name = "colUnitCost";
            colUnitCost.ReadOnly = true;
            //
            // colTotalDiscount
            //
            colTotalDiscount.HeaderText = "Disc Price";
            colTotalDiscount.Name = "colTotalDiscount";
            colTotalDiscount.Visible = SharedVariables.AdminPharmacySetting.DiscountedPrice;
            colTotalDiscount.MinimumWidth = 70;
            colTotalDiscount.ReadOnly = true;
            //
            // colTotalCost
            //
            colTotalCost.HeaderText = "Total Cost";
            colTotalCost.Name = "colTotalCost";
            colTotalCost.ReadOnly = true;
            //
            // colCreatedAt
            //
            colCreatedAt.HeaderText = "Created At";
            colCreatedAt.Name = "colCreatedAt";
            colCreatedAt.ReadOnly = true;
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
            long ConsumptionId = (long)((Button)sender).Tag;
            Form f = SharedFunctions.OpenForm(new frmStockConsumption(ConsumptionId), this.MdiParent);
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
                long ConsumptionId = (long)((Button)sender).Tag;
                using (unitOfWork = new UnitOfWork())
                {
                    StockConsumption sc = unitOfWork.StockConsumptionRepository.GetStockConumptionById_(ConsumptionId);
                    sc.IsActive = false;
                    sc.IsSynced = false;
                    sc.IsUpdate = true;
                    sc.UpdateDate = DateTime.Now;
                    sc.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    foreach (StockConsumptionItem i in sc.StockConsumptions)
                    {
                        i.IsActive = false;
                        i.UpdateDate = DateTime.Now;
                        i.IsSynced = false;
                        i.IsUpdate = true;
                    }
                    unitOfWork.StockConsumptionRepository.Update(sc);
                    AuditLogTask.CreateAuditLog(unitOfWork, AuditModule.Stock_Consumption, AuditAction.deleted);
                    unitOfWork.Save();
                    //SOT
                    OfflineJsonTask.SaveOfflineTask(unitOfWork, sc.StockConsumptions, ModuleName.BulkStockConsumptionItems, ModuleAction.Delete, 0);
                }
                MessageBox.Show("Consumption Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnRefresh.PerformClick();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                long InvoiceId = (long)((Button)sender).Tag;
                if (InvoiceId <= 0)
                    return;
                if (!SharedFunctions.IsActionallowed("Refund Payment") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                {
                    SharedFunctions.AccessDeniedMsg();
                    return;
                }
                using (unitOfWork = new UnitOfWork())
                {
                    if (unitOfWork.StockConsumptionRepository.CheckAllInvoiceItemReturn(InvoiceId) > 0)
                    {
                        SharedFunctions.MessageBoxShow("All stock has been returned.", MessageType.Info);
                        return;
                    }
                }
                frmInvoice f = new frmInvoice(false, true, InvoiceId);
                f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
                f.Show();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }

        }
        private void ChildForm_CLosed(object sender, FormClosedEventArgs e)
        {
            LoadConsumptions();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            long ConsumptionId = (long)((Button)sender).Tag;
            Reports.StockConsumptionViewer v = new Reports.StockConsumptionViewer(ConsumptionId);
            v.Show();
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }
        private void flowPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            Panel AddedPanel = (Panel)(e.Control);
            AddedPanel.Width = flowPanel.Width - 23;
        }
        #endregion

        #region Filters
        private void txtInvoiceNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadConsumptions();
        }
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadConsumptions();
        }
        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadConsumptions();
        }
        private void cmbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadConsumptions();
        }
        private void cmbStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadConsumptions();
        }
        private void txtInvoiceNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pageNo = 1;
                LoadConsumptions();
            }
        }
        #endregion

        #region PaginationButtons
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadConsumptions();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = ConsumptionsList.PageCount;
            LoadConsumptions();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo++;
            LoadConsumptions();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo--;
            LoadConsumptions();
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
                    if (_pageNo <= 0 || _pageNo > ConsumptionsList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.pageNo = _pageNo;
                    LoadConsumptions();
                }
            }
        }
        #endregion

        #region CommentedCode
        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            //this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            //if (this.SelectedItemId > 0)
            //{
            //    LoadConsumptions();
            //}
        }



        //private void Clear()
        //{
        //    dtpTo.Value = dtpFrom.Value = DateTime.Now;
        //    pageNo = 1;
        //}

        //private void LoadConsumptions()
        //{
        //    flowPanel.Controls.Clear();
        //    using (unitOfWork = new UnitOfWork())
        //    {
        //        if (filter == FilterType.Date)
        //        {
        //            ConsumptionsList = unitOfWork.StockConsumptionRepository.GetStockConsumptions(dtpFrom.Value, dtpTo.Value, pageNo, SharedVariables.PageSize);
        //        }
        //        else if (filter == FilterType.Item)
        //        {
        //            ConsumptionsList = unitOfWork.StockConsumptionRepository.GetStockConsumptions(this.SelectedItemId, this.pageNo, SharedVariables.PageSize);
        //        }
        //        else if (filter == FilterType.User)
        //        {
        //            ConsumptionsList = unitOfWork.StockConsumptionRepository.GetStockConsumptions(dtpFrom.Value, dtpTo.Value, Convert.ToInt32(cmbUsers.SelectedValue), pageNo, SharedVariables.PageSize);
        //        }
        //        else
        //        {
        //            ConsumptionsList = unitOfWork.StockConsumptionRepository.GetStockConsumptions(pageNo, SharedVariables.PageSize);
        //        }
        //    }
        //    foreach (StockConsumptionVM a in ConsumptionsList)
        //    {
        //        BuildPanel(a);
        //    }
        //    btnLastPage.Enabled = btnNext.Enabled = ConsumptionsList.HasNextPage ? true : false;
        //    btnFirstPage.Enabled = btnPrevious.Enabled = ConsumptionsList.HasPreviousPage ? true : false;
        //    SharedFunctions.ShowPageNo(lblPageNo, pageNo, ConsumptionsList.PageCount);
        //}
        #endregion
    }
}