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

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmTemplates : Form
    {
        bool isDateChanged = false;
        int PageNo = 1;
        bool IsFilterApplied = false;
        static PharmacyContext cxt = new PharmacyContext();
        ItemsRepository repItems = new ItemsRepository(cxt);
        UnitOfWork unitOfWork;
        IPagedList<TemplateVM> TemplatesList;
        public frmTemplates()
        {
            InitializeComponent();
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Add Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                SharedFunctions.AccessDeniedMsg();
                return;
            }
            Form f = SharedFunctions.OpenForm(new frmItems(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }
        private void btnAddMultipleItems_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Add Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                SharedFunctions.AccessDeniedMsg();
                return;
            }
            Form f = SharedFunctions.OpenForm(new frmMultipleItems(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        //private void btnAddStock_Click(object sender, EventArgs e)
        //{
        //    if (!SharedFunctions.IsActionallowed("Add Stock") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
        //    {
        //        SharedFunctions.AccessDeniedMsg();
        //        return;
        //    }
        //    Form f = SharedFunctions.OpenForm(new frmAddStock(), this.MdiParent);
        //    f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        //}
        private void frmAllItems_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdItems);
            //SharedFunctions.UpdateCommonStyle(grdItems);
            this.WindowState = FormWindowState.Maximized;
            LoadTemplates();
            SharedFunctions.SetGridStyle(grdItems);
        }
        private void LoadTemplates()
        {
            try
            {
                grdItems.Rows.Clear();
                using (unitOfWork = new UnitOfWork())
                {
                    if (IsFilterApplied)
                    {
                        string FilterString = txtNameFilter.Text.Trim();
                        if (FilterString != "")
                        {
                            TemplatesList = unitOfWork.TemplateRepository.getTemplates_filter(FilterString.ToLower(), PageNo, SharedVariables.PageSize);
                        }
                        //else
                        //{
                        //    TemplatesList = unitOfWork.ItemRspository.GetItemsWithStockDataByStockDate(dtpStockDate.Value, PageNo, SharedVariables.PageSize);
                        //}
                    }
                    else
                    {
                        TemplatesList = unitOfWork.TemplateRepository.getTemplates(PageNo, SharedVariables.PageSize);
                    }
                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, TemplatesList.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = TemplatesList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = TemplatesList.HasNextPage;
                StringBuilder sb = new StringBuilder ();
                foreach (TemplateVM t in TemplatesList)
                {
                    sb = new StringBuilder ();
                    foreach(TemplateItemVm ti in t.TemplateItems)
                    {
                        sb.Append(ti.Item.ItemName + ", ");
                    }
                                      
                    if(sb.Length >0)
                    {
                        sb = sb.Remove(sb.Length-2, 2);
                    }
                    grdItems.Rows.Add(t.TemplateId, t.Name, sb);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void AppendSuppliers(List<ItemsVM> TemplatesList)
        {
            List<Item_SuppliersVM> itemSupList = repItems.GetSuppliersByItemIds(TemplatesList);
            foreach (ItemsVM i in TemplatesList)
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
            LoadTemplates();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadTemplates();
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
                        string FilterString = txtNameFilter.Text.Trim();
                        if (FilterString != "")
                        {
                            Items = unitOfWork.ItemRspository.GetItemsWithStockDataByItemNameFilter(FilterString.ToLower());
                        }
                        else
                        {
                            Items = unitOfWork.ItemRspository.GetItemsWithStockDataByStockDate(dtpStockDate.Value);
                        }
                    }
                    else
                    {
                        Items = unitOfWork.ItemRspository.GetItemsWithStockData();
                    }
                }
                foreach (ItemsVM i in Items)
                {
                    i.AvailableStock = i.TotalStock - i.ExpiredStock + i.AdjustedStock - i.ConsumedStock;
                    i.TotalStock = i.TotalStock + i.AdjustedStock;
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
            LoadTemplates();
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

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            dtpStockDate.Value = DateTime.Now;
            txtNameFilter.Text = "";
            isDateChanged = false;
            IsFilterApplied = false;
            LoadTemplates();
            PageNo = 1;
            dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
        }
        private void txtNameFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (txtNameFilter.Text.Trim() != "")
                {
                    isDateChanged = false;
                    IsFilterApplied = true;
                    PageNo = 1;
                    LoadTemplates();
                }
            }
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int templateId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colTemplateId"].Value);
                    if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                    {
                        //if (!SharedFunctions.IsActionallowed("Edit Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        //{
                        //    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    return;
                        //}
                        frmAddTemplate f = new frmAddTemplate(templateId);                        
                        f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                        f.Show();
                        return;
                    }
                    if (e.ColumnIndex == grdItems.Columns["colDelete"].Index)
                    {
                        //if (!SharedFunctions.IsActionallowed("Delete Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        //{
                        //    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    return;
                        //}
                        
                        DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Item?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs == System.Windows.Forms.DialogResult.OK)
                        {
                            //Item i = unitOfWork.ItemRspository.GetById(ItemId);
                            //i.IsActive = false;
                            //unitOfWork.ItemRspository.Update(i);
                            using (unitOfWork = new UnitOfWork())
                            {
                                unitOfWork.TemplateRepository.SetTemplateInactive(templateId);
                                unitOfWork.Save();
                            }
                            grdItems.Rows.RemoveAt(e.RowIndex);
                            MessageBox.Show("Template Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            PageNo = 1;
                            btnRefreshForm.PerformClick();
                        }
                        return;
                    }
                    //if (e.ColumnIndex == grdItems.Columns["colView"].Index)
                    //{
                    //    frmItemInfo f = new frmItemInfo(ItemId);
                    //    f.MdiParent = this.MdiParent;
                    //    f.Show();
                    //}
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = TemplatesList.PageCount;
            LoadTemplates();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTemplates();
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
                    if (_pageNo <= 0 || _pageNo > TemplatesList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    PageNo = _pageNo;
                    LoadTemplates();
                }
            }
        }

        private void cmbStockUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTemplates();
        }

        private void btnATemplates_Click(object sender, EventArgs e)
        {
            frmAddTemplate f = new frmAddTemplate();
            f.FormClosed += new FormClosedEventHandler(this.ChildForm_Closed);
            f.ShowDialog();
        }

    }
}