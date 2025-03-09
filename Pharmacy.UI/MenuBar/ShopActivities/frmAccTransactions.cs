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
using Pharmacy.UI.MenuBar.Pharmacy;

namespace Pharmacy.UI.MenuBar.ShopActivities
{
    public partial class frmAccTransactions : Form
    {
        enum FilterType { Default = 1, Date = 2 };
        FilterType filter = FilterType.Default;
        bool isDateChanged = false;
        int PageNo = 1;
        bool IsFilterApplied = false;
        static PharmacyContext cxt = new PharmacyContext();
        ItemsRepository repItems = new ItemsRepository(cxt);
        UnitOfWork unitOfWork;
        IPagedList<ItemsVM> itemsList;
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }
        private void frmAllItems_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdItems);
            //SharedFunctions.UpdateCommonStyle(grdItems);
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetGridStyle(grdItems);
            LoadTransactions();
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {

            if (keyData == (Keys.Control | Keys.P))
            {
                btnPrint.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.E))
            {
                btnExcel.PerformClick();
                return true;
            }
            if (keyData == (Keys.F5))
            {
                btnRefreshForm.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.M))
            {
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.I))
            {
                return true;
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            {
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

            #region grid short cuts
            if (keyData == (Keys.Alt | Keys.E))
            {
                if (grdItems.SelectedRows.Count > 0)
                {
                    int colIndex = grdItems.Columns["colEdit"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.D))
            {
                {
                    int colIndex = grdItems.Columns["colDelete"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.V))
            {
                {
                    int colIndex = grdItems.Columns["colView"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadTransactions()
        {
            try
            {
                grdItems.Rows.Clear();
                using (unitOfWork = new UnitOfWork())
                {
                    if (filter == FilterType.Date)
                    {
                    }
                    else
                    {
                        //unitOfWork
                    }
                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, itemsList.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = itemsList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = itemsList.HasNextPage;                
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading transactions data.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AppendSuppliers(List<ItemsVM> itemsList)
        {
            List<Item_SuppliersVM> itemSupList = repItems.GetSuppliersByItemIds(itemsList);
            foreach (ItemsVM i in itemsList)
            {
                foreach (Item_SuppliersVM ii in itemSupList)
                {
                    if (i.ItemId == ii.ItemId)
                    {
                        i.Suppliers += ii.SupplierName.ToString() + ",";
                    }
                }
                string suppliers = i.Suppliers.Length > 0 ? i.Suppliers.TrimEnd(',') : "";
                i.Suppliers = suppliers;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadTransactions();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadTransactions();
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ItemssRpt rpt = new ItemssRpt();
                rpt.Database.Tables[0].SetDataSource(this.GetReportData());
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }

                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Items Date Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            ItemsViewer f = new ItemsViewer(GetReportData());
            f.Show();
        }

        private List<ItemsVM> GetReportData()
        {
            try
            {
                List<ItemsVM> Items = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    if (IsFilterApplied)
                    {
                        //if (FilterString != "")
                        //{
                        //    Items = unitOfWork.ItemRspository.GetItemsWithStockDataByItemNameFilter(FilterString.ToLower());
                        //}
                        //else
                        //{
                        //    Items = unitOfWork.ItemRspository.GetItemsWithStockDataByStockDate(dtpStockDate.Value);
                        //}
                    }
                    else
                    {
                        Items = unitOfWork.ItemRspository.GetItemsWithStockData();
                    }
                }
                string suppliers = "";
                foreach (ItemsVM i in Items)
                {
                    suppliers = "";
                    i.AvailableStock = i.TotalStock - i.ExpiredStock + i.AdjustedStock - i.ConsumedStock;
                    i.TotalStock = i.TotalStock + i.AdjustedStock;
                    foreach (var s in i.SuppliersList)
                    {
                        suppliers += s.Name + ", ";
                    }
                    suppliers = suppliers.TrimEnd(',');
                    i.Suppliers = suppliers;
                }
                return Items;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while fetching report data, please try again.", ex.Message, "Failed.");
                return null;
            }
        }
        private void dtpStockDate_ValueChanged(object sender, EventArgs e)
        {
            IsFilterApplied = true;
            PageNo = 1;
            LoadTransactions();
        }
        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            dtpStockDate.Value = DateTime.Now;
            isDateChanged = false;
            IsFilterApplied = false;
            LoadTransactions();
            PageNo = 1;
            dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                    if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                    {
                        if (!SharedFunctions.IsActionallowed("Edit Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        {
                            SharedFunctions.AccessDeniedMsg();
                            return;
                        }
                        frmItems f = new frmItems(ItemId);
                        f.MdiParent = this.MdiParent;
                        f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                        f.Show();
                        return;
                    }
                    if (e.ColumnIndex == grdItems.Columns["colDelete"].Index)
                    {
                        if (!SharedFunctions.IsActionallowed("Delete Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        {
                            SharedFunctions.AccessDeniedMsg();
                            return;
                        }
                        int ConsumedQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colConsumedStock"].Value);
                        if (ConsumedQty > 0)
                        {
                            MessageBox.Show("Item Has Consumed Quantity, it Can't be Deleted", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Item?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs == System.Windows.Forms.DialogResult.OK)
                        {
                            //Item i = unitOfWork.ItemRspository.GetById(ItemId);
                            //i.IsActive = false;
                            //unitOfWork.ItemRspository.Update(i);

                            using (unitOfWork = new UnitOfWork())
                            {
                                //WWW If item have a stock and has one stockitem with the same name
                                //unitOfWork.ItemRspository.
                                List<Stock> stocks = unitOfWork.ItemRspository.IsItemHaveOneStock(ItemId);
                                foreach (var stock in stocks)
                                {
                                    List<StockItem> stockList = cxt.StockItems.Where(x => x.Stock.StockId == stock.StockId).ToList();
                                    if (stockList != null && stockList.Count() == 1)
                                    {
                                        stock.IsActive = false;
                                        stock.IsNew = false;
                                        stock.IsUpdate = false;
                                        stock.DeletedAt = DateTime.Now;
                                        unitOfWork.StockRepository.Update(stock);
                                    }
                                }
                                unitOfWork.ItemRspository.SetItemDataInactive(ItemId);
                                unitOfWork.Save();
                            }
                            grdItems.Rows.RemoveAt(e.RowIndex);
                            MessageBox.Show("Item Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            PageNo = 1;
                            btnRefreshForm.PerformClick();
                        }
                        return;
                    }
                    if (e.ColumnIndex == grdItems.Columns["colView"].Index)
                    {
                        frmItemInfo f = new frmItemInfo(ItemId);
                        f.MdiParent = this.MdiParent;
                        f.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = itemsList.PageCount;
            LoadTransactions();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTransactions();
        }

        private void frmAllItems_Activated(object sender, EventArgs e)
        {
            btnRefreshForm.PerformClick();
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmAllItemsShortCuts f = new ShortCutDialogs.frmAllItemsShortCuts();
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
                    if (_pageNo <= 0 || _pageNo > itemsList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    PageNo = _pageNo;
                    LoadTransactions();
                }
            }
        }
    }
}