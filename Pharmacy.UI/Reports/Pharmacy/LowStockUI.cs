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
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using PagedList;
using System.IO;
using Pharmacy.UI.MenuBar.Pharmacy;
namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class LowStockUI : Form
    {
        private UnitOfWork unitOfWork;
        IPagedList<LowStockVM> LowStocksList;
        int PageNo = 1;
        public LowStockUI()
        {
            InitializeComponent();

        }


        private void LowStockUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint, btnRefreshForm });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                LoadSuppliers();
                LoadLowStocks();
                LoadCategories();
                SharedFunctions.SetGridStyle(grdData);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                RefreshForm();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RefreshForm()
        {
            using (new CursorWait())
            {
                LoadSuppliers();
                LoadCategories();
                LoadLowStocks();
            }
        }


        private void LoadSuppliers()
        {
            cmbSuppliers.SelectedIndexChanged -= cmbSuppliers_SelectedIndexChanged;
            List<Supplier> Suppliers = new List<Supplier>();
            using (unitOfWork = new UnitOfWork())
            {
                Suppliers = unitOfWork.SupplierRepository.GetActiveSuppliers();
            }
            Supplier s = new Supplier
            {
                SupplierID = 0,
                Name = "Select Supplier"
            };
            Suppliers.Insert(0, s);
            cmbSuppliers.DataSource = Suppliers;
            cmbSuppliers.ValueMember = "SupplierID";
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
        }

        private void LoadCategories()
        {
            try
            {
                cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
                List<Category> cts = new List<Category>();
                using (unitOfWork = new UnitOfWork())
                {
                    cts = unitOfWork.CategoryRepository.GetAllActiveCategories();
                }
                Category c = new Category();
                c.CategoryId = 0;
                c.Name = "Select Category";
                cts.Insert(0, c);
                cmbCategory.DataSource = cts;
                cmbCategory.ValueMember = "CategoryId";
                cmbCategory.DisplayMember = "Name";
                cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private void LoadLowStocks()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    long SId = 0, CId = 0;
                    long.TryParse(cmbSuppliers.SelectedValue.ToString(), out SId);
                    long.TryParse(cmbCategory.SelectedIndex.ToString(), out CId);

                    LowStocksList = unitOfWork.ItemRspository.GenericSearchForLowStockItem(CId, SId, PageNo, SharedVariables.PageSize);
                    //CommentedCode
                    //if (SId > 0)
                    //{
                    //    LowStocksList = unitOfWork.ItemRspository.GetLowStockItems_BySupplier(SId, PageNo, SharedVariables.PageSize);
                    //}
                    //else if (CId > 0)
                    //{
                    //    LowStocksList = unitOfWork.ItemRspository.GetLowStockItems_ByCategory(CId, PageNo, SharedVariables.PageSize);
                    //}
                    //else
                    //{
                    //    LowStocksList = unitOfWork.ItemRspository.GetLowStockItems(PageNo, SharedVariables.PageSize);
                    //}
                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, LowStocksList.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = LowStocksList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = LowStocksList.HasNextPage;
                grdData.Rows.Clear();
                foreach (LowStockVM l in LowStocksList)
                {
                    l.ItemsLeft = l.TotalStock - l.ConsumedStock - l.ExpiredStock + l.AdjustedStock;
                    if (l.ItemsLeft <= l.ReOrderingLevel)
                    {
                        grdData.Rows.Add(l.ItemId, l.ItemName, l.Category, l.ReOrderingLevel, l.ItemsLeft, l.LastStockAddedDate);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadLowStocks();
        }

        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadLowStocks();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<LowStockRptVM> LowStocksList = GetReportData();
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            //    if (d.IsPrint)
            //{
            LowStocsViewer v = new LowStocsViewer(0, LowStocksList);
            v.Show();
            //}
            //else if (d.IsSaveAsPdf)
            //{
            //    SavePdf(d.Orientation);
            //}
            //}
        }

        private List<LowStockRptVM> GetReportData()
        {
            List<LowStockVM> Result = new List<LowStockVM>();
            List<LowStockRptVM> ResutlList = new List<LowStockRptVM>();
            try
            {
                //long SId = Convert.ToInt64(cmbSuppliers.SelectedValue);
                //long CId = Convert.ToInt64(cmbCategory.SelectedIndex);
                using (unitOfWork = new UnitOfWork())
                {
                    long SId = 0, CId = 0;
                    long.TryParse(cmbSuppliers.SelectedValue.ToString(), out SId);
                    long.TryParse(cmbCategory.SelectedIndex.ToString(), out CId);
                    Result = unitOfWork.ItemRspository.GenericSearchForLowStockItem_Rpt(CId, SId);
                    //if (SId > 0)
                    //{
                    //    Result = unitOfWork.ItemRspository.GetLowStockItems_BySupplier_Rpt(SId);
                    //}
                    //else if (CId > 0)
                    //{
                    //    Result = unitOfWork.ItemRspository.GetLowStockItems_ByCategory_Rpt(CId);
                    //}
                    //else
                    //{
                    //    Result = unitOfWork.ItemRspository.GetLowStockItems_Rpt();
                    //}
                }
                foreach (LowStockVM r in Result)
                {
                    LowStockRptVM model = new LowStockRptVM();
                    model.ItemID = r.ItemId;
                    model.ItemName = r.ItemName;
                    model.Category = r.Category;
                    model.LastStockAdded = r.LastStockAddedDate.HasValue == true ? r.LastStockAddedDate.Value.ToString("dd/MM/yyyy HH:mm:tt") : "";
                    model.ItemsLeft = r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock;
                    ResutlList.Add(model);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }

            return ResutlList;
        }

        private void SavePdf(int Orientation)
        {
            try
            {
                List<LowStockVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    if (cmbSuppliers.SelectedIndex > 0)
                    {
                        Result = unitOfWork.ItemRspository.GetLowStockItems_BySupplier_Rpt(Convert.ToInt32(cmbSuppliers.SelectedValue));
                    }
                    else
                    {
                        Result = unitOfWork.ItemRspository.GetLowStockItems_Rpt();
                    }
                }
                if (Orientation == 0)
                {
                    Reports.Pharmacy.LowStocksRpt_Portrait r = new LowStocksRpt_Portrait();
                    r.SetDataSource(Result);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        r.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    LowStocksRpt_Landscape rpt = new LowStocksRpt_Landscape();
                    rpt.SetDataSource(Result);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (SharedVariables.AdminPractiseSetting == null)
                {
                    throw new Exception("Failed to load report headers, please try after login again");
                }
                List<LowStockRptVM> ResultList = GetReportData();
                LowStocksRpt_Landscape rpt = new LowStocksRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(ResultList);
                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("ItemWise Sales Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        public string GetLowStockExcelSheet()
        {
            try
            {
                if (SharedVariables.AdminPractiseSetting == null)
                {
                    throw new Exception("Failed to load report headers, please try after login again");
                }
                List<LowStockRptVM> Result = GetReportData();
                LowStocksRpt_Landscape rpt = new LowStocksRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Result);
                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                try
                {
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");
                    System.IO.Directory.CreateDirectory(filePath);
                    filePath = Path.Combine(filePath, "LowStocksEmail" + SharedFunctions.GetTimestamp(DateTime.Now) + ".xls");
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, filePath);
                    return filePath;
                }
                catch (Exception ex)
                {
                    throw new Exception("Unable to extract data to excel sheet");
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
                return null;
            }
        }

     

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                if (e.ColumnIndex == grdData.Columns["colAddPO"].Index)
                {
                    long ItemId = Convert.ToInt32(grdData.Rows[e.RowIndex].Cells["colItemId"].Value);
                    if (ItemId <= 0) return;
                    Form f = SharedFunctions.OpenForm(new frmPurchaseOrder(ItemId, true), this.MdiParent);
                    f.FormClosed += new FormClosedEventHandler(childForm_Closed);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private void childForm_Closed(object sender, FormClosedEventArgs e)
        {
            RefreshForm();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadLowStocks();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = LowStocksList.PageCount;
            LoadLowStocks();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadLowStocks();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadLowStocks();
        }
        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > LowStocksList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadLowStocks();
                }
            }
        }

        private void txtGotoPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            RefreshForm();
        }
    }
}
