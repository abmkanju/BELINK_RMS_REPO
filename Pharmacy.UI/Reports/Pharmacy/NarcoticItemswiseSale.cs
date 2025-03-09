using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using PagedList;
using Pharmacy.UI.MenuBar.Pharmacy;
namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class NarcoticItemswiseSale : Form
    {
        int PageNo = 1;
        private UnitOfWork unitOfWork;
        enum FilterType { Default = 1, ItemName = 2, Manufacturer = 3 };
        FilterType filter = FilterType.Default;
        private IPagedList<NarcoticsDrugInvoiceVM> Invoices;
        public NarcoticItemswiseSale()
        {
            InitializeComponent();
        }

        private void NarcoticUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint , btnRefreshForm });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                LoadDrugs();
                LoadManufacturers();
                SharedFunctions.SetGridStyle(grdData);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }

        private void LoadManufacturers()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    List<Manufacturer> cts = new List<Manufacturer>();
                    cts = unitOfWork.ManufacturerRepository.GetAllActiveManufacturers();

                    Manufacturer c = new Manufacturer();
                    c.ManufacturerId = 0;
                    c.Name = "Select Manufacturer";
                    cts.Insert(0, c);
                    cmbManufacturer.DataSource = cts;
                    cmbManufacturer.ValueMember = "ManufacturerId";
                    cmbManufacturer.DisplayMember = "Name";
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow("Error occured while loading maufacturers : " + ex.Message, MessageType.Error);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                using (new CursorWait())
                {
                    dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                    dtpTo.ValueChanged -= dtpTo_ValueChanged;
                    cmbManufacturer.SelectedIndexChanged -= cmbManufacturer_SelectedIndexChanged;
                    txtSearchDrug.KeyDown -= txtSearchDrug_KeyDown;
                    this.PageNo = 1;
                    dtpTo.Value = DateTime.Now;
                    dtpFrom.Value = DateTime.Now;
                    cmbManufacturer.SelectedIndex = 0;
                    txtSearchDrug.Text = "";
                    LoadDrugs();
                    dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                    dtpTo.ValueChanged += dtpTo_ValueChanged;
                    cmbManufacturer.SelectedIndexChanged += cmbManufacturer_SelectedIndexChanged;
                    txtSearchDrug.KeyDown += txtSearchDrug_KeyDown;
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void LoadDrugs()
        {
            try
            {
                using (new CursorWait())
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        if (filter == FilterType.ItemName)
                        {
                            Invoices = unitOfWork.InvoiceRepository.NarcoticItemwiseSalesByItemName(dtpFrom.Value, dtpTo.Value, txtSearchDrug.Text.Trim().ToLower(), PageNo, SharedVariables.PageSize);
                        }
                        else if (filter == FilterType.Manufacturer)
                        {
                            Invoices = unitOfWork.InvoiceRepository.NarcoticItemwiseSalesByManufacturer(dtpFrom.Value, dtpTo.Value, cmbManufacturer.SelectedIndex, PageNo, SharedVariables.PageSize);
                        }
                        else
                        {
                            Invoices = unitOfWork.InvoiceRepository.NarcoticItemwiseSales(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                        }
                    }
                    grdData.Rows.Clear();

                    int Quantity = 0;
                    int ReturnedQuantity = 0;
                    double TotalRefund = 0;
                    double Value = 0;
                    foreach (NarcoticsDrugInvoiceVM i in Invoices)
                    {
                        TotalRefund = 0;
                        ReturnedQuantity = 0;
                        Quantity = 0;
                        Value = 0;
                        if (i.DiscountType == 2)
                        {
                            i.TotalDiscount = (i.TotalDiscount / 100) * i.SubTotal;
                        }
                        string SupplierName = "";
                        foreach (InvoiceItemVM ii in i.InvoiceItems)
                        {
                            i.NarcoticDrugs += ii.ItemName + ", ";
                            ii.SupplierNameList = unitOfWork.InvoiceRepository.LoadSupplierName(ii.ItemId, ii.BatchId);
                            if (ii.SupplierNameList != null && ii.SupplierNameList.Count() > 0)
                                SupplierName += string.Join(",", ii.SupplierNameList) + ", ";
                            Quantity += ii.Quantity;
                            ReturnedQuantity += ii.ReturnedQuantity;
                            if (i.Payment < 0)
                            {
                                TotalRefund += (((ii.NetAmount / (i.SubTotal - i.TotalDiscount)) * i.TotalPaid) / Quantity) * ReturnedQuantity;
                            }
                            Value += ((ii.NetAmount / (i.SubTotal - i.TotalDiscount)) * i.Payment);
                        }
                        if (!string.IsNullOrEmpty(i.NarcoticDrugs))
                        {
                            i.NarcoticDrugs = i.NarcoticDrugs.Trim().TrimEnd(',');
                        }
                        SupplierName = !string.IsNullOrEmpty(SupplierName) ? SupplierName.Trim().TrimEnd(',') : "";
                        if (i.Payment < 0)
                        {
                            grdData.Rows.Add(i.InvoiceId, i.InvoiceRefNo.ToString("D6"), i.PatientName, i.NarcoticDrugs, SupplierName, ReturnedQuantity, Math.Round(-1 * TotalRefund, 2), i.PaymentDate == DateTime.MinValue ? "" : i.PaymentDate.ToString(), i.CreateDate);
                        }
                        else
                        {
                            grdData.Rows.Add(i.InvoiceId, i.InvoiceRefNo.ToString("D6"), i.PatientName, i.NarcoticDrugs, SupplierName, Quantity, Math.Round(Value, 2), i.PaymentDate == DateTime.MinValue ? "" : i.PaymentDate.ToString(), i.CreateDate);
                        }
                    }
                    btnFirstPage.Enabled = btnPrevious.Enabled = Invoices.HasPreviousPage;
                    btnLastPage.Enabled = btnNext.Enabled = Invoices.HasNextPage;
                    SharedFunctions.ShowPageNo(lblPageNo, PageNo, Invoices.PageCount);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<NarcoticDrugsItemwiseReportVM> reportList = GetReportData();
            if (reportList == null || reportList.Count() <= 0)
            {
                SharedFunctions.MessageBoxShow("No record found", MessageType.Info);
                return;
            }
            NarcoticItemswiseSaleViewer v = new NarcoticItemswiseSaleViewer(0, this.dtpFrom.Value, this.dtpTo.Value, reportList);
            v.Show();
        }

        private List<NarcoticDrugsItemwiseReportVM> GetReportData()
        {
            List<NarcoticDrugsItemwiseReportVM> rptList = new List<NarcoticDrugsItemwiseReportVM>();
            List<NarcoticsDrugInvoiceVM> Invs;
            List<NarcoticsDrugInvoiceVM> FinalTransactions = new List<NarcoticsDrugInvoiceVM>();
            using (unitOfWork = new UnitOfWork())
            {
                if (filter == FilterType.ItemName)
                {
                    Invs = unitOfWork.InvoiceRepository.NarcoticItemwiseSalesByItemNameReport(dtpFrom.Value, dtpTo.Value, txtSearchDrug.Text.Trim().ToLower());
                }
                else if (filter == FilterType.Manufacturer)
                {
                    Invs = unitOfWork.InvoiceRepository.NarcoticItemwiseSalesByManufacturerReport(dtpFrom.Value, dtpTo.Value, cmbManufacturer.SelectedIndex);
                }
                else
                {
                    Invs = unitOfWork.InvoiceRepository.NarcoticItemwiseSalesReport(dtpFrom.Value, dtpTo.Value);
                }
                foreach (NarcoticsDrugInvoiceVM i in Invs)
                {
                    foreach (InvoiceItemVM ii in i.InvoiceItems)
                    {
                        var item = rptList.Where(x => x.InvoiceId == i.InvoiceNoString && x.ItemId == ii.ItemId).FirstOrDefault();
                        if (item != null)
                        {
                            continue;
                        }
                        NarcoticDrugsItemwiseReportVM model = new NarcoticDrugsItemwiseReportVM();
                        model.InvoiceId = i.InvoiceNoString;
                        model.ItemId = ii.ItemId;
                        model.CreateByName = i.CreateBy;
                        model.BatchName = ii.BatchName;
                        model.BatchExpiry = ii.BatchExpiry;
                        model.OutQty = ii.Quantity;
                        model.CreatedOn = ii.CreatedDate;
                        model.ManufacturerName = ii.ManufacturerName;
                        model.InQty = 0;
                        model.Description = "Sale Customer";
                        model.NarcoticDrugName = ii.ItemName;
                        List<string> SupplierList = unitOfWork.InvoiceRepository.LoadSupplierName(ii.ItemId, ii.BatchId);
                        if (SupplierList != null && SupplierList.Count() > 0)
                            model.SupplierName = string.Join(",", SupplierList) + ", ";
                        else
                            model.SupplierName = "";
                        model.RefundQty = ii.ReturnedQuantity;
                        model.OutQty = ii.ReturnedQuantity > 0 ? ii.Quantity - ii.ReturnedQuantity : ii.Quantity;
                        rptList.Add(model);
                    }
                }
                if (rptList != null && rptList.Count() > 0)
                {
                    var itemsList = rptList.GroupBy(u => u.ItemId).Select(grp => grp.ToList()).ToList();
                    rptList = new List<NarcoticDrugsItemwiseReportVM>();
                    foreach (var item in itemsList.ToList())
                    {
                        long ItemId = item.FirstOrDefault().ItemId;
                        List<NarcoticDrugsItemwiseReportVM> items = GetStockItems(ItemId);
                        items.AddRange(item);
                        int PreviousDateTimeStock = PreviousDateStock(ItemId);
                        //-----Assign Supplier Name ------//
                        string suppliername = item.Where(k => k.SupplierName != string.Empty).Select(x => x.SupplierName).FirstOrDefault();
                        items.ForEach(x => { x.OpeningBalance = PreviousDateTimeStock; x.SupplierName = suppliername; });
                        //-----Assign Manufacturer Name ------//
                        string manufacturerName = item.Where(k => k.ManufacturerName != string.Empty).Select(x => x.ManufacturerName).FirstOrDefault();
                        items.ForEach(x => x.ManufacturerName = manufacturerName);
                        rptList.AddRange(items.OrderBy(x => x.CreatedOn).ToList());
                    }
                }
                return rptList;
            }
        }

        public int PreviousDateStock(long ItemId)
        {
            int previousItemStock = 0;
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    previousItemStock = unitOfWork.InvoiceRepository.GetPreviousDateItemStock(ItemId, dtpFrom.Value);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            return previousItemStock;
        }

        private List<NarcoticDrugsItemwiseReportVM> GetStockItems(long ItemId)
        {
            List<NarcoticDrugsItemwiseReportVM> stockItems = new List<NarcoticDrugsItemwiseReportVM>();
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    stockItems = unitOfWork.InvoiceRepository.DateTimeAvailableItemStock(ItemId, dtpFrom.Value, dtpTo.Value);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            return stockItems;
        }
        private void SavePdf(int Orientation, List<NarcoticsDrugInvoiceVM> Invoices)
        {
            try
            {
                if (Orientation == 0)
                {
                    NarcoticDrugsRpt_Portrait rpt = new NarcoticDrugsRpt_Portrait();
                    rpt.SetDataSource(Invoices);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Narcotic Drugs Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    NarcoticDrugsRpt_Landscape rpt = new NarcoticDrugsRpt_Landscape();
                    rpt.SetDataSource(Invoices);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Narcotic Drugs Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadDrugs();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = Invoices.PageCount;
            LoadDrugs();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadDrugs();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDrugs();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (SharedVariables.AdminPractiseSetting == null)
                {
                    throw new Exception("Failed to load report headers, please try after login again");
                }
                List<NarcoticDrugsItemwiseReportVM> reportList = GetReportData();
                if (reportList == null || reportList.Count() <= 0)
                {
                    SharedFunctions.MessageBoxShow("No record found", MessageType.Info);
                    return;
                }
                NarcoticDrugsItemswiseRpt_Landscape rpt = new NarcoticDrugsItemswiseRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(reportList);
                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                rpt.SetParameterValue("TotalPaid", Invoices.Sum(rs => rs.TotalPaid));
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Narcotic Drugs Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            if (e.ColumnIndex == grdData.Columns["colPrint"].Index)
            {
                long InvoiceId = Convert.ToInt64(grdData.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                Reports.POSInvoices.POSInvoiceViewer v = new POSInvoices.POSInvoiceViewer(InvoiceId, true);
                v.Show();
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            this.LoadDrugs();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            this.LoadDrugs();
        }


        private void txtSearchDrug_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtSearchDrug.Text.Trim().Equals(""))
                {
                    cmbManufacturer.SelectedIndexChanged -= cmbManufacturer_SelectedIndexChanged;
                    cmbManufacturer.SelectedIndex = 0;
                    filter = FilterType.ItemName;
                    PageNo = 1;
                    this.LoadDrugs();
                    cmbManufacturer.SelectedIndexChanged += cmbManufacturer_SelectedIndexChanged;
                }
                else
                {
                    cmbManufacturer.SelectedIndexChanged -= cmbManufacturer_SelectedIndexChanged;
                    cmbManufacturer.SelectedIndex = 0;
                    filter = FilterType.Default;
                    PageNo = 1;
                    this.LoadDrugs();
                    cmbManufacturer.SelectedIndexChanged += cmbManufacturer_SelectedIndexChanged;
                }
            }
        }

        private void cmbManufacturer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.PageNo = 1;
                txtSearchDrug.KeyDown -= txtSearchDrug_KeyDown;
                txtSearchDrug.Text = "";
                filter = cmbManufacturer.SelectedIndex > 0 ? FilterType.Manufacturer : FilterType.Default;
                LoadDrugs();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            finally
            {
                txtSearchDrug.KeyDown += txtSearchDrug_KeyDown;
            }
        }



        private void txtSearchInv_KeyPress(object sender, KeyPressEventArgs e)
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
                    if (_pageNo <= 0 || _pageNo > Invoices.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadDrugs();
                }
            }
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            cmbManufacturer.SelectedIndexChanged -= cmbManufacturer_SelectedIndexChanged;
            txtSearchDrug.KeyDown -= txtSearchDrug_KeyDown;
            this.PageNo = 1;
            dtpTo.Value = DateTime.Now;
            dtpFrom.Value = DateTime.Now;
            cmbManufacturer.SelectedIndex = 0;
            txtSearchDrug.Text = "";
            LoadDrugs();
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            cmbManufacturer.SelectedIndexChanged += cmbManufacturer_SelectedIndexChanged;
            txtSearchDrug.KeyDown += txtSearchDrug_KeyDown;
        }
    }
}