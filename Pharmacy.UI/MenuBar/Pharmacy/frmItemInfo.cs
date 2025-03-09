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
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.UI.ManageOfflineTask;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmItemInfo : Form
    {
        enum FilterType { Default = 1, Batch = 2 };
        FilterType filter = FilterType.Default;
        UnitOfWork unitOfWork;
        int ItemId = 0;
        double RetailPrice = 0;
        public frmItemInfo()
        {
            InitializeComponent();
        }

        public frmItemInfo(int ItemId)
        {
            InitializeComponent();
            this.ItemId = ItemId;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void LoadData()
        {
            try
            {
                grdConsumptions.Rows.Clear();
                grdStocks.Rows.Clear();
                grdAdjustments.Rows.Clear();
                LoadItemInfo();
                LoadItemStocks();
                LoadStockConsumptions();
                LoadStockAdjusmtents();

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }


        private void frmItemInfo_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdAdjustments);
            SharedFunctions.SetControlForeColor(new[] { lblName, lblItemName, lblSu, lblStockingUnit, lblCategory, lblRL, lblReOrderLvl, lblCtg, lblbcd, lblBarcode });
            SharedFunctions.SetGridStyle(grdConsumptions);
            SharedFunctions.SetGridStyle(grdStocks);
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnGenBC, btnPrintBC, btnClose, btnAddStoc });
            RetailPrice = 0;
            LoadData();
            LoadItemBatches();
        }


        private void LoadItemBatches()
        {
            List<SelectListVM> batches = new List<SelectListVM>();
            using (unitOfWork = new UnitOfWork())
            {
                batches = unitOfWork.BatchRepository.GetItemBatches(this.ItemId);
                if (batches != null && batches.Count() > 0)
                {
                    batches = batches.GroupBy(x => new { x.Text, x.Value }).Select(x => new SelectListVM 
                    { 
                        Text  =  x.Key.Text, 
                        Value = x.Key.Value
                    }).ToList();
                }
            }
            batches.Insert(0, new SelectListVM { Value = 0, Text = "Select Batch" });

            cmbBatch.SelectedIndexChanged -= cmbBatch_SelectedIndexChanged;
            cmbBatch.DataSource = batches;
            cmbBatch.ValueMember = "Value";
            cmbBatch.DisplayMember = "Text";
            cmbBatch.SelectedIndexChanged += cmbBatch_SelectedIndexChanged;
        }

        private void LoadItemInfo()
        {
            double retailPrice = 0;
            Item i;
            using (unitOfWork = new UnitOfWork())
            {
                i = unitOfWork.ItemRspository.GetById(this.ItemId);
            }
            lblItemName.Text = i.ItemName;
            lblStockingUnit.Text = i.Unit.ToString();

            retailPrice = i.RetailPrice;
            RetailPrice = retailPrice * i.ConversionUnit;
            lblReOrderLvl.Text = i.ReOrderingLevel.ToString();
            //lblType.Text = i.IsNarcotic ? "Narcotic" : "Drug";
            lblBarcode.Text = i.Barcode;

            btnGenBC.Enabled = string.IsNullOrEmpty(i.Barcode);
            btnPrintBC.Enabled = !string.IsNullOrEmpty(i.Barcode);

        }

        private void LoadItemStocks()
        {
            string suppliersnamesstring = "";
            List<BatchStockVM> BatchStocks;
            using (unitOfWork = new UnitOfWork())
            {
                BatchStocks = unitOfWork.ItemRspository.GetStockItemBatchkByItemId2(this.ItemId);
            }
            
            grdStocks.Columns["colStore"].Visible = SharedVariables.AdminPharmacySetting.IsEnableStores;
            foreach (BatchStockVM i in BatchStocks)
            {
                //foreach(var sup in i.Suppliers)
                //{
                //    suppliersnamesstring += sup + ",";
                //}
                //i.AvailableStock = i.TotalStock - i.ExpiredStock - i.ConsumedStock + i.AdjustedStock;
                SharedFunctions.CalculateInvoiceItemQty(i);
               
                grdStocks.Rows.Add(i.BatchName, i.Suppliers.Name , i.StoreName, (i.TotalStock + i.AdjustedStock), i.AvailableStock, i.Expiry, i.CreatedAt);
            }
        }
        private void LoadStockConsumptions()
        {
            grdConsumptions.Rows.Clear();
            List<StockConsumptionItem> result;
            using (unitOfWork = new UnitOfWork())
            {
                if (filter == FilterType.Batch)
                {
                    result = unitOfWork.StockConsumptionRepository.GetConsStockByBatchId(Convert.ToInt32(cmbBatch.SelectedValue), this.ItemId);
                }
                else // default
                {
                    result = unitOfWork.StockConsumptionRepository.GetConsumptionsByItemId(this.ItemId);
                }
            }
            grdConsumptions.Columns["colStoreCon"].Visible = SharedVariables.AdminPharmacySetting.IsEnableStores;
            foreach (StockConsumptionItem c in result)
            {
                grdConsumptions.Rows.Add(c.Batch.BatchName, c.Store != null ? c.Store.Name : "",  c.Quantity, ((SharedVariables.ConsumptionType)c.ConsumptionType).ToString(), c.CreateDate);

            }
        }
        private void LoadStockAdjusmtents()
        {
            List<AdjustmentItem> Items;
            using (unitOfWork = new UnitOfWork())
            {
                Items = unitOfWork.AdjustmentRepository.GetStockAdjustmentsByItemId(this.ItemId);
            }
            foreach (AdjustmentItem i in Items)
            {
                grdAdjustments.Rows.Add(i.Batch.BatchName, i.Quantity, i.Reason, i.CreateDate);
            }
        }
        private void frmItemInfo_Shown(object sender, EventArgs e)
        {
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void frmItemInfo_Activated(object sender, EventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void btnGenBC_Click(object sender, EventArgs e)
        {
            try
            {
                string barcode = SharedFunctions.GetTimestamp2(DateTime.Now);
                using (unitOfWork = new UnitOfWork())
                {
                    Item obj = unitOfWork.ItemRspository.GetById(this.ItemId);
                    obj.Barcode = barcode;
                    unitOfWork.ItemRspository.Update(obj);
                    unitOfWork.Save();
                    if (!obj.IsFileItem)
                        OfflineJsonTask.SaveOfflineTask(unitOfWork, obj, ModuleName.Item, ModuleAction.Update, obj.ItemID);
                    lblBarcode.Text = barcode;
                    btnGenBC.Enabled = false;
                    btnPrintBC.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while generating barcodes, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnPrintBC_Click(object sender, EventArgs e)
        {
            string barcode = "";
            using (unitOfWork = new UnitOfWork())
            {
                barcode = unitOfWork.ItemRspository.GetItemBarcode(this.ItemId);
            }
            new frmPrintBarcode(barcode, lblItemName.Text, RetailPrice).ShowDialog();

            //Reports.frmItemBarcodeViewer v = new Reports.frmItemBarcodeViewer(this.ItemId);
            //v.Show();
        }

        private void cmbBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter = FilterType.Default;
            int batchId = Convert.ToInt32(cmbBatch.SelectedValue);
            if (batchId > 0)
            {
                filter = FilterType.Batch;
            }
            LoadStockConsumptions();
        }

        private void btnAddStoc_Click(object sender, EventArgs e)
        {
            frmAddStock f = new frmAddStock(this.ItemId);
            f.Show();
        }
    }
}