using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class StockStatisticsUI : Form
    {
        UnitOfWork unitOfWork;
        List<ItemsVM> itemsList;
        public StockStatisticsUI()
        {
            InitializeComponent();
            SharedFunctions.SetGridStyle(grdItems);
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefreshForm});
            loadfilters();
            LoadItems();
        }
        private void loadfilters()
        {
            try
            {
                cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;
               
                cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;
                
                cmbRack.SelectedIndexChanged -= cmbRack_SelectedIndexChanged;
                
                using (unitOfWork = new UnitOfWork())
                {
                    loadSuppliers();
                    
                    loadManufacturers();
                    
                    loadRack();
                    
                }
                cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
                
                cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;
                
                cmbRack.SelectedIndexChanged += cmbRack_SelectedIndexChanged;
                
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void loadRack()
        {
            List<Rack> racks = new List<Rack>();
            racks = unitOfWork.RackRepository.GetAllActiveRacks();
            racks.Insert(0, new Rack { RackId = 0, Name = "Select Rack" });
            cmbRack.DataSource = racks;
            cmbRack.ValueMember = "RackId";
            cmbRack.DisplayMember = "Name";
        }
        private void loadManufacturers()
        {
            List<Manufacturer> manfs = new List<Manufacturer>();
            manfs = unitOfWork.ManufacturerRepository.GetAllActiveManufacturers();
            manfs.Insert(0, new Manufacturer { ManufacturerId = 0, Name = "Select Manufacturer" });
            cmbManuf.DataSource = manfs;
            cmbManuf.ValueMember = "ManufacturerId";
            cmbManuf.DisplayMember = "Name";
        }
        private void loadSuppliers()
        {
            List<Supplier> sups = new List<Supplier>();
            sups = unitOfWork.SupplierRepository.GetActiveSuppliers();
            sups.Insert(0, new Supplier() { SupplierID = 0, Name = "Select Supplier" });
            cmbSupplier.DataSource = sups;
            cmbSupplier.ValueMember = "SupplierId";
            cmbSupplier.DisplayMember = "Name";

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                using (new CursorWait())
                {
                    dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                    dtpTo.ValueChanged -= dtpTo_ValueChanged;
                    dtpFrom.Value = DateTime.Now;
                    dtpTo.Value = DateTime.Now;
                    LoadRackstatistics();
                    dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                    dtpTo.ValueChanged += dtpTo_ValueChanged;
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadItems()
        {
            try
            {
                grdItems.Rows.Clear();
                checkBox1.Checked = false;
                using (unitOfWork = new UnitOfWork())
                {
                    
                    DateTime dtfrom = DateTime.Now;
                    DateTime dtto = DateTime.Now;
                    dtfrom = Convert.ToDateTime(dtpFrom.Value.ToShortDateString());
                    dtto = dtpTo.Value;
                    itemsList = unitOfWork.ItemRspository.GenericSearchForAllItems1(RackId: SharedFunctions.StringToInt(cmbRack.SelectedValue),
                        ManunfId: SharedFunctions.StringToInt(cmbManuf.SelectedValue),
                        SuppId: SharedFunctions.StringToInt(cmbSupplier.SelectedValue),
                        dtfrom:dtfrom,
                        dtto:dtto , Item: txtNameFilter.Text);
                }
                
                string Low = "";
                foreach (ItemsVM item in itemsList)
                {

                    foreach (Supplier s in item.SuppliersList)
                    {
                        item.Suppliers += s.Name + ", ";
                    }
                    if (!string.IsNullOrEmpty(item.Suppliers))
                    {
                        item.Suppliers = item.Suppliers.Trim().TrimEnd(',');
                    }
                    Low = "";

                    if (item.ConsumedStock < 0) { item.ConsumedStock = System.Math.Abs(item.ConsumedStock); }
                    //-------CalculateAvailableQty----------//
                    SharedFunctions.CalculateItemAvailableQty(item);
                    //item.TotalStock = item.TotalStock + item.AdjustedStock;
                    if (item.ReorderingLevel > 0)
                    {
                        Low = item.AvailableStock <= item.ReorderingLevel ? "LOW" : "";
                    }
                    string Expired = item.ExpiredStock > 0 ? "EXPIRED" : "";
                    item.ItemType = item.IsNarcotic ? "Narcotic" : "Drug";
                    if (item.TotalStock != 0)
                    {
                        grdItems.Rows.Add(item.ItemId, item.ItemName, item.chemicalname, item.Suppliers, item.Manufacturer == null ? "" : item.Manufacturer.Name, "", item.TotalStock, item.AdjustedStock, item.ExpiredStock, item.ConsumedStock, item.AvailableStock, item.UnitCost, item.RetailPrice, item.AvailableStock * item.UnitCost, item.AvailableStock*item.RetailPrice);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void LoadItems1()
        {
            try
            {
                grdItems.Rows.Clear();

                using (unitOfWork = new UnitOfWork())
                {

                    DateTime dtfrom = DateTime.Now;
                    DateTime dtto = DateTime.Now;
                    dtfrom = Convert.ToDateTime(dtpFrom.Value.ToShortDateString());
                    dtto = dtpTo.Value;
                    itemsList = unitOfWork.ItemRspository.GenericSearchForAllItems2();
                }

                string Low = "";
                foreach (ItemsVM item in itemsList)
                {

                    foreach (Supplier s in item.SuppliersList)
                    {
                        item.Suppliers += s.Name + ", ";
                    }
                    if (!string.IsNullOrEmpty(item.Suppliers))
                    {
                        item.Suppliers = item.Suppliers.Trim().TrimEnd(',');
                    }
                    Low = "";

                    if (item.ConsumedStock < 0) { item.ConsumedStock = System.Math.Abs(item.ConsumedStock); }
                    //-------CalculateAvailableQty----------//
                    SharedFunctions.CalculateItemAvailableQty(item);
                    //item.TotalStock = item.TotalStock + item.AdjustedStock;
                    if (item.ReorderingLevel > 0)
                    {
                        Low = item.AvailableStock <= item.ReorderingLevel ? "LOW" : "";
                    }
                    string Expired = item.ExpiredStock > 0 ? "EXPIRED" : "";
                    item.ItemType = item.IsNarcotic ? "Narcotic" : "Drug";
                    if (item.TotalStock != 0)
                    {
                        grdItems.Rows.Add(item.ItemId, item.ItemName, item.chemicalname, item.Suppliers, item.Manufacturer == null ? "" : item.Manufacturer.Name, "", item.TotalStock, item.AdjustedStock, item.ExpiredStock, item.ConsumedStock, item.AvailableStock, item.UnitCost, item.RetailPrice, item.AvailableStock * item.UnitCost, item.AvailableStock * item.RetailPrice);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void calcuatecostandRetail()
        {
            double retail = 0;
            double cost = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                retail += Convert.ToDouble(r.Cells["colRetailValueofAvailableStock"].Value.ToString());
                cost += Convert.ToDouble(r.Cells["colCostofAvailableStock"].Value.ToString());

            }
            lblCostValue.Text = cost.ToString();
            lblRetailValue.Text = retail.ToString();
        }
        private void StockStatisticsUI_Load(object sender, EventArgs e)
        {
            LoadRackstatistics();
            
            //LoadItems();
            //this.RetailValueChart.Series[0].Points.AddXY("m1", 100);
            //this.RetailValueChart.Series[0].Points.AddXY("m2", 200);
            //this.RetailValueChart.Series[0].Points.AddXY("m3", 300);
            //this.RetailValueChart.Series[0].Points.AddXY("m4", -100);
            //this.RetailValueChart.Series[0].Points.AddXY("m5", -200);
            //this.RetailValueChart.Series[0].Points.AddXY("m5", -300);
        }

        private void LoadRackstatistics()
        {
            StockStatisticsParentVM Result = null;
            using (unitOfWork = new UnitOfWork())
            {
                //var Result = 
                Result = unitOfWork.StockRepository.GetStockStatistics(dtpFrom.Value, dtpTo.Value);
            }
            if (Result != null)
            {
                //unitOfWork.StockRepository.TestMethod(dtpFrom.Value, dtpTo.Value);

                //"Retail Value"
                //"Retail Value"
                //"Retail Value"

                this.RetailValueChart.DataSource = Result.StockStatisticsList;
                this.RetailValueChart.Series["Retail Value"].XValueMember = "MonthDay";
                this.RetailValueChart.Series["Retail Value"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                this.RetailValueChart.Series["Retail Value"].YValueMembers = "TotalRetailValue";
                this.CostValueChart.Series[0].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;


                //this.RetailValueChart.Series[0].Points.AddXY("m1", 100);
                //this.RetailValueChart.Series[0].Points.AddXY("m2", 200);
                //this.RetailValueChart.Series[0].Points.AddXY("m3", 300);
                //this.RetailValueChart.Series[0].Points.AddXY("m4", -100);
                //this.RetailValueChart.Series[0].Points.AddXY("m5", -200);
                //this.RetailValueChart.Series[0].Points.AddXY("m5", -300);
                //this.RetailValueChart.Update();


                this.CostValueChart.DataSource = Result.StockStatisticsList;
                this.CostValueChart.Series["Cost Value"].XValueMember = "MonthDay";
                this.CostValueChart.Series["Cost Value"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                this.CostValueChart.Series["Cost Value"].YValueMembers = "TotalCostValue";
                this.CostValueChart.Series["Cost Value"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;


                /////////////////////
                //retailChart2.DataSource = Result.StockStatisticsList;
                //retailChart2.Series["Retail Value"].XValueMember = "MonthDay";
                //retailChart2.Series["Retail Value"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                //retailChart2.Series["Retail Value"].YValueMembers = "TotalRetailValue";

                //costChart2.DataSource = Result.StockStatisticsList;
                //costChart2.Series["Cost Value"].XValueMember = "MonthDay";
                //costChart2.Series["Cost Value"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                //costChart2.Series["Cost Value"].YValueMembers = "TotalCostValue";

                lblRetailValue.Text = Math.Round(Result.TotalRetValOfAvailableStock, 2).ToString();
                lblCostValue.Text = Math.Round(Result.TotalCostValOfAvailableStock, 2).ToString();

            }
            calcuatecostandRetail();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadItems();
            LoadRackstatistics();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadItems();
            LoadRackstatistics();
        }

        private void cmbManuf_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItems();
            LoadRackstatistics();
        }

        private void cmbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItems();
            LoadRackstatistics();

        }

        private void cmbRack_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadItems();
            LoadRackstatistics();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            txtNameFilter.Text = "";
            loadfilters();
            LoadItems();
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            
            LoadRackstatistics();
        }

        private void btnReturnAll_Click(object sender, EventArgs e)
        {
            StockStaticsViewer s = new StockStaticsViewer(GetReportData());
           s.Show();
        }
        private List<StockStaticsVM> GetReportData()
        {
            try
            {
                List<StockStaticsVM> itemsList = new List<StockStaticsVM>();
               
                using (unitOfWork = new UnitOfWork())
                {

                    DateTime dtfrom = DateTime.Now;
                    DateTime dtto = DateTime.Now;
                    dtfrom = Convert.ToDateTime(dtpFrom.Value.ToShortDateString());
                    dtto = dtpTo.Value;
                    if (!checkBox1.Checked)
                    {
                        itemsList = unitOfWork.ItemRspository.GenericSearchForAllItems1_Rpt(RackId: SharedFunctions.StringToInt(cmbRack.SelectedValue),
                            ManunfId: SharedFunctions.StringToInt(cmbManuf.SelectedValue),
                            SuppId: SharedFunctions.StringToInt(cmbSupplier.SelectedValue),
                            dtfrom: dtfrom,
                            dtto: dtto,
                            Item: txtNameFilter.Text);
                    }
                    else
                    {
                        itemsList = unitOfWork.ItemRspository.GenericSearchForAllItems2_Rpt();
                    }
                }
                string suppliers = "";
                foreach (StockStaticsVM i in itemsList)
                {
                    suppliers = "";
                    SharedFunctions.CalculateItemAvailableQty_New(i);
                    //i.AvailableStock = i.TotalStock - i.ExpiredStock + i.AdjustedStock - i.ConsumedStock;
                    i.TotalStock = i.TotalStock + i.TotalAdjustment;
                    foreach (var s in i.SuppliersList)
                    {
                        suppliers += s.Name + ", ";
                    }
                    suppliers = suppliers.TrimEnd(',');
                    i.Suppliers = suppliers;
                }
                return itemsList;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while fetching report data, please try again.", ex.Message, "Failed.");
                return null;
            }
        }

        private void txtNameFilter_TextChanged(object sender, EventArgs e)
        {
            LoadItems();
            calcuatecostandRetail();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                LoadItems1();
                calcuatecostandRetail();
            }
            else
            {
                LoadItems();
                calcuatecostandRetail();
            }
        }
    }
}