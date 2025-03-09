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
using Pharmacy.UI.Reports.Product;
using Pharmacy.UI.MenuBar.Pharmacy;
namespace Pharmacy.UI.Reports.Products
{
    public partial class AllItemsDiscountsUI : Form
    {
        enum FilterType { Default = 1, Date = 2 }
        FilterType filter = FilterType.Default;
        private UnitOfWork unitOfWork;
        IPagedList<FlatDiscountVM> pmList;
        int PageNo = 1;
        public AllItemsDiscountsUI()
        {
            InitializeComponent();
        }

        private void LowStockUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                SharedFunctions.SetGridStyle(grdData);
                LoadDiscountedItems();
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
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpTo.ValueChanged -= dtpFrom_ValueChanged;
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                LoadDiscountedItems();
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                dtpTo.ValueChanged += dtpFrom_ValueChanged;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadDiscountedItems()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    if (filter == FilterType.Date)
                    {
                        pmList = unitOfWork.FlatDiscountRepository.GetAllItemDiscounts(DateFrom: dtpFrom.Value, DateTo: dtpTo.Value, pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                    }
                    else
                    {
                        pmList = unitOfWork.FlatDiscountRepository.GetAllItemDiscounts(pageNo: this.PageNo, pageSize: SharedVariables.PageSize);
                    }
                }

                grdData.Rows.Clear();
                foreach (var d in pmList)
                {
                    grdData.Rows.Add(d.FlatDiscountId, d.Name, d.Code, d.DiscountType == 0 ? "%" : "Value", Math.Round(d.Discount, 2), d.IsAllItems);
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = pmList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = pmList.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, pmList.PageCount);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                List<FlatDiscountVM> pmList = GetReportData();
                //dlgPrintOptions d = new dlgPrintOptions();
                //d.ShowDialog();
                //if (!d.IsCancelled)
                //{
                //    if (d.IsPrint)
                //{
                DiscountsViewer v = new DiscountsViewer(0, pmList);
                v.Show();
                //}
                //else if (d.IsSaveAsPdf)
                //{
                //    SavePdf(d.Orientation);
                //}
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading report data, please try loading again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<FlatDiscountVM> GetReportData()
        {
            try
            {
                List<FlatDiscountVM> list = new List<FlatDiscountVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    if (filter == FilterType.Date)
                    {
                        list = unitOfWork.FlatDiscountRepository.GetAllItemDiscounts_rpt(DateFrom: dtpFrom.Value, DateTo: dtpTo.Value);
                    }
                    else
                    {
                        list = unitOfWork.FlatDiscountRepository.GetAllItemDiscounts_rpt();
                    }
                }               
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetReportsData()
        {
            //long SId = Convert.ToInt64(cmbSuppliers.SelectedValue);
            //if (SId > 0)
            //{
            //    pmList = unitOfWork.ItemRspository.GetLowStockItems_BySupplier(SId, PageNo, SharedVariables.PageSize);
            //}
            //else
            //{
            //    pmList = unitOfWork.ItemRspository.GetLowStockItems(PageNo, SharedVariables.PageSize);
            //}
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (SharedVariables.AdminPractiseSetting == null)
                {
                    throw new Exception("Failed to load report headers, please try after login again");
                }
                List<FlatDiscountVM> Result = GetReportData();
                DiscountsRpt_Portrait rpt = new DiscountsRpt_Portrait();
                rpt.Database.Tables[0].SetDataSource(Result);
                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                //rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Discounted Price Report Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        public string GetLowStockExcelSheet()
        {
            return "";
            //try
            //{
            //    if (SharedVariables.AdminPractiseSetting == null)
            //    {
            //        throw new Exception("Failed to load report headers, please try after login again");
            //    }
            //    List<LowStockVM> Result = GetReportData();
            //    LowStocksRpt_Landscape rpt = new LowStocksRpt_Landscape();
            //    rpt.Database.Tables[0].SetDataSource(Result);
            //    if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
            //    {
            //        rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
            //    }
            //    rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
            //    rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
            //    rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
            //    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
            //    try
            //    {
            //        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");
            //        System.IO.Directory.CreateDirectory(filePath);
            //        filePath = Path.Combine(filePath, "LowStocksEmail" + SharedFunctions.GetTimestamp(DateTime.Now) + ".xls");
            //        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, filePath);
            //        return filePath;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception("Unable to extract data to excel sheet");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            //    return null;
            //}
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > pmList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadDiscountedItems();
                }
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            this.PageNo = 1;
            LoadDiscountedItems();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            this.PageNo = 1;
            LoadDiscountedItems();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadDiscountedItems();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = pmList.PageCount;
            LoadDiscountedItems();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadDiscountedItems();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDiscountedItems();
        }

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if(e.ColumnIndex ==  grdData.Columns["colViewDetail"].Index)
            {
                bool isAllItems= Convert.ToBoolean(grdData.Rows[e.RowIndex].Cells["colAllItems"].Value);
                int id = Convert.ToInt32(grdData.Rows[e.RowIndex].Cells["colDiscId"].Value);
                SharedFunctions.OpenForm(new AllItems_DiscountedPriceUI(id, isAllItems), this.MdiParent);
                AllItems_DiscountedPriceUI f = new AllItems_DiscountedPriceUI(id, isAllItems);
                f.MdiParent = this.MdiParent;
                f.Show();
                f.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDiscountedItems();
        }
    }
}