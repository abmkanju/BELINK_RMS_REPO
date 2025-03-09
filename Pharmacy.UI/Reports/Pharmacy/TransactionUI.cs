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
    public partial class TransactionUI : Form
    {

        private UnitOfWork unitOfWork;
        IPagedList<InvoiceTransactionsVM> Transactions;
        enum FilterType { Default = 1, InvoiceNo = 2, Date = 3, Patient = 4, Store };
        FilterType FilterApplied;
        int PageNo = 1;
        public TransactionUI()
        {
            InitializeComponent();
            this.FilterApplied = FilterType.Default;
        }



        private void TransactionUI_Load(object sender, EventArgs e)
        {
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            try
            {
                SharedFunctions.SetGridStyle(grdData);
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint, btnRefreshForm });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                LoadDetails();
                LoadTransactions();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        }
        public void LoadDetails()
        {
            try
            {
                cmbPatient.SelectedIndexChanged -= cmbPatient_SelectedIndexChanged;
                cmbStore.SelectedIndexChanged -= cmbStore_SelectedIndexChanged;
                loadPatients();
                loadStore();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            finally
            {
                cmbPatient.SelectedIndexChanged += cmbPatient_SelectedIndexChanged;
                cmbStore.SelectedIndexChanged += cmbStore_SelectedIndexChanged;
            }

        }

        private void loadStore()
        {
            List<SelectListVM> lst = new List<SelectListVM>();
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
                Name = "Select Store"
            };
            list.Insert(0, model);
            cmbStore.DataSource = list;
            cmbStore.ValueMember = "StoreId";
            cmbStore.DisplayMember = "Name";
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                using (new CursorWait())
                {
                    dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                    dtpTo.ValueChanged -= dtpTo_ValueChanged;
                    cmbStore.SelectedIndexChanged -= cmbStore_SelectedIndexChanged;
                    cmbPatient.SelectedIndexChanged -= cmbPatient_SelectedIndexChanged;
                    dtpFrom.Value = DateTime.Now;
                    dtpTo.Value = DateTime.Now;
                    txtInvoiceNoSearch.Text = "";
                    this.FilterApplied = FilterType.Date;
                    LoadDetails();
                    LoadTransactions();
                    dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                    dtpTo.ValueChanged += dtpTo_ValueChanged;
                    cmbStore.SelectedIndexChanged += cmbStore_SelectedIndexChanged;
                    cmbPatient.SelectedIndexChanged += cmbPatient_SelectedIndexChanged;
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void loadPatients()
        {
            try
            {
                List<SelectListVM> lst = new List<SelectListVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    lst = unitOfWork.PatientRepository.GetSelectList();
                }
                lst.Insert(0, new SelectListVM { Value = 0, Text = "Select Patient" });
                cmbPatient.DataSource = lst;
                cmbPatient.ValueMember = "Text";
                cmbPatient.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading patients data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadTransactions()
        {
            if (txtInvoiceNoSearch.Text.Trim() != "")
            {
                this.FilterApplied = FilterType.InvoiceNo;
            }
            try
            {
                SummationsVM Sums = new SummationsVM();
                DateTime fromTime = dtFrom.Value;
                DateTime toTime = dtTo.Value;
                DateTime fromDate = dtpFrom.Value;
                DateTime toDate = dtpTo.Value;
                fromDate = fromDate.Date + fromTime.TimeOfDay;
                toDate = toDate.Date + toTime.TimeOfDay;
                using (unitOfWork = new UnitOfWork())
                {
                    //if (this.FilterApplied == FilterType.Patient)
                    //{
                    //    //Sums = unitOfWork.InvoiceRepository.GetSums();
                    //    Sums = unitOfWork.InvoiceRepository.GetSums(fromDate, toDate);
                    //    Transactions = unitOfWork.InvoiceRepository.GetTransactions_Rpt(fromDate, toDate, Convert.ToInt64(cmbPatient.SelectedValue), PageNo, SharedVariables.PageSize);
                    //}
                    //else if (this.FilterApplied == FilterType.Store)
                    //{
                    //    //Sums = unitOfWork.InvoiceRepository.GetSums();
                    //    Sums = unitOfWork.InvoiceRepository.GetSums(fromDate, toDate);
                    //    Transactions = unitOfWork.InvoiceRepository.GetTransactionsStore_Rpt(fromDate, toDate, SharedFunctions.StringToInt(cmbStore.SelectedValue), PageNo, SharedVariables.PageSize);
                    //}
                    //else if (this.FilterApplied == FilterType.InvoiceNo && SharedFunctions.StringToLong(txtInvoiceNoSearch.Text) > 0)
                    //{
                    //    long invNo = SharedFunctions.StringToLong(txtInvoiceNoSearch.Text);
                    //    Sums = unitOfWork.InvoiceRepository.GetSums(invNo);
                    //    Transactions = unitOfWork.InvoiceRepository.GetTransactions_rpt(invNo, PageNo, SharedVariables.PageSize);
                    //}
                    //else if (this.FilterApplied == FilterType.Date)
                    //{
                    //    Sums = unitOfWork.InvoiceRepository.GetSums(fromDate, toDate);
                    //    Transactions = unitOfWork.InvoiceRepository.GetTransactions_Rpt(fromDate, toDate, PageNo, SharedVariables.PageSize);
                    //}
                    //else //default
                    //{
                    //    Sums = unitOfWork.InvoiceRepository.GetSums();
                    //    Transactions = unitOfWork.InvoiceRepository.GetTransactions_Rpt(PageNo, SharedVariables.PageSize);
                    //}
                    if (this.FilterApplied == FilterType.Default)//default
                    {
                        Sums = unitOfWork.InvoiceRepository.GetSums();
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions_Rpt(PageNo, SharedVariables.PageSize);
                    }
                    else if (this.FilterApplied == FilterType.Store)
                    {
                        //Sums = unitOfWork.InvoiceRepository.GetSums();
                        Sums = unitOfWork.InvoiceRepository.GetSums(fromDate, toDate);
                        Transactions = unitOfWork.InvoiceRepository.GetTransactionsStore_Rpt(fromDate, toDate, SharedFunctions.StringToInt(cmbStore.SelectedValue), PageNo, SharedVariables.PageSize);
                    }
                    else 
                    {
                        var invoiceRef = (txtInvoiceNoSearch.Text != "")? txtInvoiceNoSearch.Text: "0";
                        var Patientid = cmbPatient.SelectedValue;

                        Sums = unitOfWork.InvoiceRepository.GetSums(fromDate, toDate, Convert.ToDouble(invoiceRef), Convert.ToInt64(Patientid));
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions_Rpt(fromDate, toDate, PageNo, SharedVariables.PageSize,Convert.ToDouble(invoiceRef), Convert.ToInt64(Patientid));
                    }
                }
                //lblTotalRevenue.Text = Math.Round((double)Sums.TotalRevenue, 2).ToString();
                if (FilterApplied == FilterType.Default) // no query is being performed in other casses for performance reasons
                {
                    lblRetailValue.Text = Math.Round((double)Sums.RetailValueOfAvailableStock, 2).ToString();
                    lblCostValue.Text = Math.Round((double)Sums.CostOfAvailableStock, 2).ToString();
                }
                CalculateServices(Transactions);
                grdData.Rows.Clear();
                double Sum = 0;
                foreach (InvoiceTransactionsVM t in Transactions)
                {
                    CalculateServices(t);
                    //LastTransaction = index == 0 ? t : Transactions[index - 1];

                    //if(t.DiscountType == 2)
                    //{
                    //    t.TotalDiscount = t.TotalDiscount / 100 * t.SubTotal;
                    //}
                    //if (LastTransaction.TotalRefund < 0 && LastTransaction.InvoiceId!=t.InvoiceId)
                    //{
                    //    grdData.Rows.Add(LastTransaction.InvoiceId.ToString("D6"), LastTransaction.Patient == null ? "" : LastTransaction.Patient.Name, LastTransaction.SubTotal, LastTransaction.TotalRefund, LastTransaction.PaymentDate, LastTransaction.CreateDate, LastTransaction.IsProcedureInvoice);
                    //}
                    //if(t.TotalPaid > 0)
                    //{                       
                    //   grdData.Rows.Add(t.InvoiceId.ToString("D6"), t.Patient == null ? "" : t.Patient.Name, t.SubTotal, t.TotalPaid, t.PaymentDate, t.CreateDate, t.IsProcedureInvoice);
                    //}
                    //index++;
                            Sum += t.TotalPaid;
                            grdData.Rows.Add(t.InvoiceId, t.InvoiceRefNo, t.Patient == null ? "" : t.Patient.Name, Math.Round(t.SubTotal, 2), Math.Round(t.TotalPaid, 2), t.Services, t.PaymentDate, t.PaymentCreateDate, t.IsProcedureInvoice);
                    
                }
                lblTotalRevenue.Text = Math.Round((double)Sums.TotalRevenue, 2).ToString();

                btnFirstPage.Enabled = btnPrevious.Enabled = Transactions.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = Transactions.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, Transactions.PageCount);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void CalculateServices(InvoiceTransactionsVM t)
        {
            try
            {
                bool IsExist = (from DataGridViewRow r in grdData.Rows select SharedFunctions.StringToLong(r.Cells["colInvoiceId"].Value)).Any(x => x == t.InvoiceId);
                if (IsExist)
                {
                    t.Services = 0;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void CalculateServices(IPagedList<InvoiceTransactionsVM> tran)
        {
            try
            {

                txtTotalServices.Text = Math.Round(tran.Where(x => x.ServiceType == 0 || x.ServiceType == 1)
                    .Select(x => new { x.InvoiceId, x.Services })
                   .Distinct().Sum(x => x.Services), 2).ToString();

                double serviceVal = Convert.ToDouble(txtTotalServices.Text);

                List<Invoice> InvoiceList = new List<Invoice>();
                using (unitOfWork = new UnitOfWork())
                {
                    InvoiceList = unitOfWork.InvoiceRepository.GetAllList();
                }
                foreach(var getCheck in InvoiceList)
                {
                    if(getCheck.ServiceType == 2)
                    {
                        double subTotal = getCheck.SubTotal;
                        double checkService = getCheck.Services;
                        double value = Math.Round((checkService / 100 * subTotal), 2);
                        serviceVal += value;
                    }
                }

                if (serviceVal == 0) {}
                else
                {
                    txtTotalServices.Text = serviceVal.ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTo.Value < dtpFrom.Value)
            {
                dtpTo.Value = dtpFrom.Value;
                MessageBox.Show("Invalid date filter, Date from must be less that Date to.", "Invalid Filter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.FilterApplied = FilterType.Date;
            PageNo = 1;
            LoadTransactions();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

            if (dtpTo.Value < dtpFrom.Value)
            {
                dtpTo.Value = dtpFrom.Value;
                MessageBox.Show("Invalid date filter, Date from must be less that Date to.", "Invalid Filter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.FilterApplied = FilterType.Date;
            PageNo = 1;
            LoadTransactions();
        }

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            long InvoiceId = Convert.ToInt64(grdData.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
            bool IsProcedureInvoice = Convert.ToBoolean(grdData.Rows[e.RowIndex].Cells["colIsProcedureInvoice"].Value);
            if (e.ColumnIndex == grdData.Columns["colPrint"].Index)
            {
                if (IsProcedureInvoice)
                {
                    Reports.ProcedureInvoices.ProcedureInvoiceViewer v = new ProcedureInvoices.ProcedureInvoiceViewer(InvoiceId, true);
                    v.Show();
                }
                else
                {
                    Reports.POSInvoices.POSInvoiceViewer v = new POSInvoices.POSInvoiceViewer(InvoiceId, true);
                    v.Show();
                }
                return;
            }

            if (e.ColumnIndex == grdData.Columns["colEdit"].Index)
            {
                if (IsProcedureInvoice)
                {
                    frmProcedureInvoice2 f = new frmProcedureInvoice2(true, false, InvoiceId);
                    f.Show();
                }
                else
                {
                    frmInvoice f = new frmInvoice(true, false, InvoiceId);
                    f.Show();
                    return;
                }
            }

            if (e.ColumnIndex == grdData.Columns["colReturn"].Index)
            {
                using (unitOfWork = new UnitOfWork())
                {
                    if (unitOfWork.StockConsumptionRepository.CheckAllInvoiceItemReturn(InvoiceId) > 0)
                    {
                        SharedFunctions.MessageBoxShow("All stock has been returned.", MessageType.Info);
                        return;
                    }
                }
                if (IsProcedureInvoice)
                {
                    frmProcedureInvoice2 f = new frmProcedureInvoice2(false, true, InvoiceId);
                    f.Show();
                }
                else
                {
                    frmInvoice f = new frmInvoice(false, true, InvoiceId);
                    f.Show();
                    return;
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //dlgPrintOptions d = new dlgPrintOptions();
                //d.ShowDialog();
                //if (!d.IsCancelled)
                //{
                List<InvoiceTransactionsVM> Transactions = GetDataForReport();
                //    if (d.IsPrint)
                //    {
                InvoiceTransactionsViewer v = new InvoiceTransactionsViewer(0, this.dtpFrom.Value, this.dtpTo.Value, Transactions);
                v.Show();
                //    }
                //    else if (d.IsSaveAsPdf)
                //    {
                //        SavePdf(d.Orientation, Transactions);
                //    }
                //}
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Failed to open report, please try again.", ex.Message, "Print Failed");
            }

        }

        private List<InvoiceTransactionsVM> GetDataForReport()
        {
            List<InvoiceTransactionsVM> Finaltransactions = new List<InvoiceTransactionsVM>();
            List<InvoiceTransactionsVM> Transactions = new List<InvoiceTransactionsVM>();
            //Reports>>Pharmacy>>Transaction>> Date and Patient Filters are not working properly for Print
            DateTime fromTime = dtFrom.Value;
            DateTime toTime = dtTo.Value;
            DateTime fromDate = dtpFrom.Value;
            DateTime toDate = dtpTo.Value;
            fromDate = fromDate.Date + fromTime.TimeOfDay;
            toDate = toDate.Date + toTime.TimeOfDay;
            using (unitOfWork = new UnitOfWork())
            {
                //if (this.FilterApplied == FilterType.Patient)
                //{
                //    Transactions = unitOfWork.InvoiceRepository.GetTransactionsRPT(Convert.ToInt64(cmbPatient.SelectedValue));
                //}
                //else if (this.FilterApplied == FilterType.Store)
                //{
                //    Transactions = unitOfWork.InvoiceRepository.GetTransactionsStoreRPT(SharedFunctions.StringToInt(cmbStore.SelectedValue));
                //}
                //else if (this.FilterApplied == FilterType.InvoiceNo)
                //{
                //    Transactions = unitOfWork.InvoiceRepository.GetTransactions(long.Parse(txtInvoiceNoSearch.Text));
                //}
                //else if (this.FilterApplied == FilterType.Date)
                //{
                Transactions = unitOfWork.InvoiceRepository.GetTransactions(this.dtpFrom.Value, this.dtpTo.Value);
                //}
                //else
                //{
                //    Transactions = unitOfWork.InvoiceRepository.GetTransactions();
                //}
                if (this.FilterApplied == FilterType.Default)
                {
                    Transactions = unitOfWork.InvoiceRepository.GetTransactions();
                }
                else if (this.FilterApplied == FilterType.Store)
                {
                    //Sums = unitOfWork.InvoiceRepository.GetSums();
                    Transactions = unitOfWork.InvoiceRepository.GetTransactionsStoreRPT(SharedFunctions.StringToInt(cmbStore.SelectedValue), fromDate, toDate);
                }
                else //default
                {
                    var invoiceRef = (txtInvoiceNoSearch.Text != "") ? txtInvoiceNoSearch.Text : "0";
                    var Patientid = cmbPatient.SelectedValue;

                    Transactions = unitOfWork.InvoiceRepository.GetTransactions(fromDate, toDate, Convert.ToDouble(invoiceRef), Convert.ToInt64(Patientid));
                }
            }




            //DateTime fromTime = dtFrom.Value;
            //DateTime toTime = dtTo.Value;
            //DateTime fromDate = dtpFrom.Value;
            //DateTime toDate = dtpTo.Value;
            //fromDate = fromDate.Date + fromTime.TimeOfDay;
            //toDate = toDate.Date + toTime.TimeOfDay; 
            //using (unitOfWork = new UnitOfWork())
            //{

            //    if (this.FilterApplied == FilterType.Patient)
            //    {
            //        Transactions = unitOfWork.InvoiceRepository.GetTransactions_Rpt(fromDate, toDate, Convert.ToInt64(cmbPatient.SelectedValue), PageNo, SharedVariables.PageSize).ToList();
            //    }
            //    else if (this.FilterApplied == FilterType.Store)
            //    {
            //        Transactions = unitOfWork.InvoiceRepository.GetTransactionsStore_Rpt(fromDate, toDate, SharedFunctions.StringToInt(cmbStore.SelectedValue), PageNo, SharedVariables.PageSize).ToList();
            //    }
            //    else if (this.FilterApplied == FilterType.InvoiceNo && SharedFunctions.StringToLong(txtInvoiceNoSearch.Text) > 0)
            //    {
            //        long invNo = SharedFunctions.StringToLong(txtInvoiceNoSearch.Text);
            //        Transactions = unitOfWork.InvoiceRepository.GetTransactions_rpt(invNo, PageNo, SharedVariables.PageSize).ToList();
            //    }
            //    else if (this.FilterApplied == FilterType.Date)
            //    {
            //        Transactions = unitOfWork.InvoiceRepository.GetTransactions_Rpt(fromDate, toDate, PageNo, SharedVariables.PageSize).ToList();
            //    }
            //    else //default
            //    {
            //        Transactions = unitOfWork.InvoiceRepository.GetTransactions_Rpt(PageNo, SharedVariables.PageSize).ToList();
            //    }
            //}

            foreach (var item in Transactions)
            {
                if (Finaltransactions.Where(x => x.InvoiceId == item.InvoiceId).Count() > 0)
                {
                    item.Services = 0;
                    item.InvoiceNoString = "0";
                }
                Finaltransactions.Add(item);

            }
            return Finaltransactions;
            //foreach (InvoiceTransactionsVM t in Transactions)
            //{
            //    if (t.TotalRefund < 0)
            //    {
            //        InvoiceTransactionsVM newT = new InvoiceTransactionsVM();
            //        newT.InvoiceId = t.InvoiceId;
            //        newT.CreateDate = t.CreateDate;
            //        newT.TotalAmount = t.TotalAmount;
            //        newT.TotalDiscount = t.TotalDiscount;
            //        newT.DiscountType = t.DiscountType;
            //        newT.GrandTotal = t.GrandTotal;
            //        newT.SubTotal = t.SubTotal;
            //        newT.Due = t.Due;
            //        newT.Advance = t.Advance;
            //        newT.SaleQuantity = t.SaleQuantity;
            //        newT.ReturnedQty = t.ReturnedQty;
            //        newT.PaymentDate = t.PaymentDate;
            //        newT.TotalPaid = t.TotalRefund;
            //        newT.TotalRefund = t.TotalRefund;
            //        newT.Patient = t.Patient;
            //        newT.PatientName = t.PatientName;
            //        newT.IsProcedureInvoice = t.IsProcedureInvoice;
            //        Finaltransactions.Add(newT);
            //    }
            //    Finaltransactions.Add(t);
            //}
            //return Finaltransactions;
        }

        private void SavePdf(int Orientation, List<InvoiceTransactionsVM> Transactions)
        {
            try
            {
                if (Orientation == 0)
                {
                    InvoiceTransactionsRpt_Portrait rpt = new InvoiceTransactionsRpt_Portrait();
                    rpt.SetDataSource(Transactions);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    InvoiceTransactionsRpt_Landscape rpt = new InvoiceTransactionsRpt_Landscape();
                    rpt.SetDataSource(Transactions);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
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
                List<InvoiceTransactionsVM> Transactions = this.GetDataForReport();
                double service = this.Transactions.Select(x => x.Services).Sum();
                InvoiceTransactionsRpt_Landscape rpt = new InvoiceTransactionsRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Transactions);
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
                rpt.SetParameterValue("TotalServices", service);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Transactions Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            LoadTransactions();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadTransactions();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTransactions();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = Transactions.PageCount;
            LoadTransactions();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PageNo = 1;
                LoadTransactions();
            }
        }

        private void txtInvoiceNoSearch_KeyPress(object sender, KeyPressEventArgs e)
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
                    if (_pageNo <= 0 || _pageNo > Transactions.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadTransactions();
                }
            }
        }

        private void cmbPatient_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.PageNo = 1;
                cmbStore.SelectedIndexChanged -= cmbStore_SelectedIndexChanged;
                if (cmbStore.SelectedIndex > 0)
                    cmbStore.SelectedIndex = 0;
                txtInvoiceNoSearch.Text = "";
                if (cmbPatient.SelectedIndex > 0)
                {
                    FilterApplied = FilterType.Patient;
                }
                else
                {
                    FilterApplied = FilterType.Default;
                }
                LoadTransactions();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            finally
            {
                cmbStore.SelectedIndexChanged += cmbStore_SelectedIndexChanged;
            }
        }

        private void cmbStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbPatient.SelectedIndexChanged -= cmbPatient_SelectedIndexChanged;
                this.PageNo = 1;
                if (cmbPatient.SelectedIndex > 0)
                    cmbPatient.SelectedIndex = 0;
                txtInvoiceNoSearch.Text = "";
                if (cmbStore.SelectedIndex > 0)
                {
                    FilterApplied = FilterType.Store;
                }
                else
                {
                    FilterApplied = FilterType.Default;
                }
                LoadTransactions();
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            finally
            {
                cmbPatient.SelectedIndexChanged += cmbPatient_SelectedIndexChanged;
            }
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTransactions();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTransactions();
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            cmbStore.SelectedIndexChanged -= cmbStore_SelectedIndexChanged;
            cmbPatient.SelectedIndexChanged -= cmbPatient_SelectedIndexChanged;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            dtFrom.ValueChanged -= dtFrom_ValueChanged;
            dtTo.ValueChanged -= dtTo_ValueChanged;
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;
            txtInvoiceNoSearch.Text = "";
            this.FilterApplied = FilterType.Date;
            LoadDetails();
            LoadTransactions();
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            dtFrom.ValueChanged += dtFrom_ValueChanged;
            dtTo.ValueChanged += dtTo_ValueChanged;
            cmbStore.SelectedIndexChanged += cmbStore_SelectedIndexChanged;
            cmbPatient.SelectedIndexChanged += cmbPatient_SelectedIndexChanged;
        }
    }
}