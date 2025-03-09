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
using System.Windows.Forms;

namespace Pharmacy.UI
{
    public partial class Dashboard : Form
    {
        UnitOfWork unitOfWork;
        public Dashboard()
        {
            InitializeComponent();

        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void loadOverAllTotals()
        {
            using (unitOfWork = new UnitOfWork())
            {
                int outOfStock = unitOfWork.ItemRspository.GetOutOfStockItemsCount();
                int lowStock = unitOfWork.ItemRspository.GetLowStockItemsCount();
                int expiredStock = unitOfWork.ItemRspository.GetExpiredItemsCount(null);
                int totalInvoices = unitOfWork.InvoiceRepository.GetTotalInvoiceCount();
                lblOutOfStock.Text = outOfStock.ToString();
                lblLowStock.Text = lowStock.ToString();
                lblExpired.Text = expiredStock.ToString();
                lblTotalInvoices.Text = totalInvoices.ToString();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            loadOverAllTotals();
            btnSale.PerformClick();
            LoadTodaysReport();
        }

        private void LoadStockCostGraph()
        {
            List<StockGraphVM> graph = new List<StockGraphVM>();
            using (unitOfWork = new UnitOfWork())
            {
                DateTime dateStart = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, 1);
                DateTime dateEnd = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, DateTime.DaysInMonth(dtpDate.Value.Year, dtpDate.Value.Month));
                graph = unitOfWork.StockRepository.GetCostValueOfAvailableStock(dateStart, dateEnd);
            }
            this.CostStockChart.DataSource = graph;
            this.CostStockChart.Series["Cost Value"].XValueMember = "MonthDay";
            this.CostStockChart.Series["Cost Value"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.CostStockChart.Series["Cost Value"].YValueMembers = "GrandTotal";
            this.CostStockChart.Series[0].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
        }
        private void LoadStockRetailGraph()
        {
            List<StockGraphVM> graph = new List<StockGraphVM>();
            using (unitOfWork = new UnitOfWork())
            {
                DateTime dateStart = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, 1);
                DateTime dateEnd = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, DateTime.DaysInMonth(dtpDate.Value.Year, dtpDate.Value.Month));
                graph = unitOfWork.StockRepository.GetRetailValueOfAvailableStock(dateStart, dateEnd);
            }
            this.RetailStockChart.DataSource = graph;
            this.RetailStockChart.Series["Retail Value"].XValueMember = "MonthDay";
            this.RetailStockChart.Series["Retail Value"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.RetailStockChart.Series["Retail Value"].YValueMembers = "GrandTotal";
            this.RetailStockChart.Series[0].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
        }
        private void LoadSaleGraph()
        {
            SaleChart.BringToFront();
            List<SaleGraphVM> graph = new List<SaleGraphVM>();
            using (unitOfWork = new UnitOfWork())
            {
                DateTime dateStart = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, 1);
                DateTime dateEnd = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, DateTime.DaysInMonth(dtpDate.Value.Year, dtpDate.Value.Month));
                graph = unitOfWork.InvoiceRepository.GetMonthlySalesGraph(dateStart, dateEnd);
            }
            this.SaleChart.DataSource = graph;
            this.SaleChart.Series["Sale"].XValueMember = "MonthDay";
            this.SaleChart.Series["Sale"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.SaleChart.Series["Sale"].YValueMembers = "GrandTotal";
            this.SaleChart.Series[0].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
        }
        private void LoadRevenueGraph()
        {
            RevenueGraph.BringToFront();
            List<SaleGraphVM> graph = new List<SaleGraphVM>();
            using (unitOfWork = new UnitOfWork())
            {
                DateTime dateStart = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, 1);
                DateTime dateEnd = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, DateTime.DaysInMonth(dtpDate.Value.Year, dtpDate.Value.Month));
                graph = unitOfWork.InvoiceRepository.GetMonthlyRevenueGraph(dateStart, dateEnd);
            }
            this.RevenueGraph.DataSource = graph;
            this.RevenueGraph.Series["Revenue"].XValueMember = "MonthDay";
            this.RevenueGraph.Series["Revenue"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.RevenueGraph.Series["Revenue"].YValueMembers = "GrandTotal";
            this.RevenueGraph.Series[0].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
        }
        public void LoadPurchaseOrdersGraph()
        {
            List<SaleGraphVM> graph = new List<SaleGraphVM>();
            using (unitOfWork = new UnitOfWork())
            {
                DateTime dateStart = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, 1);
                DateTime dateEnd = new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, DateTime.DaysInMonth(dtpDate.Value.Year, dtpDate.Value.Month));
                graph = unitOfWork.InvoiceRepository.GetMonthlySalesGraph(dateStart, dateEnd);
            }
            this.PurchaseOrdersGraph.DataSource = graph;
            this.PurchaseOrdersGraph.Series["Purchase Orders"].XValueMember = "MonthDay";
            this.PurchaseOrdersGraph.Series["Purchase Orders"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.PurchaseOrdersGraph.Series["Purchase Orders"].YValueMembers = "GrandTotal";
            this.PurchaseOrdersGraph.Series[0].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
        }

        private void LoadTodaysReport()
        {
            grdTodaysReport.Rows.Clear();
            grdTodaysReport.DataSource = null;
            TodaysReportVM vm = new TodaysReportVM();
            using (unitOfWork = new UnitOfWork())
            {
                vm = unitOfWork.GeneralRepository.GetTodyasReportForDashboard();
            }
            grdTodaysReport.Rows.Add("Sales", vm.SalesTotal);
            grdTodaysReport.Rows.Add("Purchase Orders", vm.PurchaseOrdersTotal);
            grdTodaysReport.Rows.Add("Total Refund", vm.TotalRefund);
            grdTodaysReport.Rows.Add("Cheque Payments", vm.ChecquePaymentsTotal);
            grdTodaysReport.Rows.Add("Debit/Credit Payments", vm.DebitCreditTotal);
            grdTodaysReport.Rows.Add("Online Payments", vm.OnlinePaymentsTotal);
            grdTodaysReport.Rows.Add("Charge Payments", vm.TotalChargePayment);
            grdTodaysReport.ClearSelection();
        }
        private void btStockCostValue_Click(object sender, EventArgs e)
        {
            //RetailStockChart.SendToBack();
            //SaleChart.SendToBack();
            CostStockChart.BringToFront();
            LoadStockCostGraph();
        }
        private void btnStockRetailValue_Click(object sender, EventArgs e)
        {

            RetailStockChart.BringToFront();
            LoadStockRetailGraph();
        }
        private void btnSale_Click(object sender, EventArgs e)
        {
            SaleChart.BringToFront();
            LoadSaleGraph();
        }
        private void btnRevenue_Click(object sender, EventArgs e)
        {
            RevenueGraph.BringToFront();
            LoadRevenueGraph();
        }
        private void btnPurchaseOrders_Click(object sender, EventArgs e)
        {
            PurchaseOrdersGraph.BringToFront();
            LoadPurchaseOrdersGraph();
        }

        private void pnlOutOfStock_MouseEnter(object sender, EventArgs e)
        {
            lblOutOfStock.Font = new Font("Microsoft Sans Serif", 18);
        }

        private void pnlOutOfStock_MouseLeave(object sender, EventArgs e)
        {
            lblOutOfStock.Font = new Font("Microsoft Sans Serif", 15);
        }

        private void pnlLowStock_MouseEnter(object sender, EventArgs e)
        {
            lblLowStock.Font = new Font("Microsoft Sans Serif", 18);
        }

        private void pnlLowStock_MouseLeave(object sender, EventArgs e)
        {
            lblLowStock.Font = new Font("Microsoft Sans Serif", 15);
        }

        private void pnlExpired_MouseEnter(object sender, EventArgs e)
        {
            lblExpired.Font = new Font("Microsoft Sans Serif", 18);
        }

        private void pnlExpired_MouseLeave(object sender, EventArgs e)
        {
            lblExpired.Font = new Font("Microsoft Sans Serif", 15);
        }

        private void pnlInvoices_MouseEnter(object sender, EventArgs e)
        {
            lblTotalInvoices.Font = new Font("Microsoft Sans Serif", 18);
        }

        private void pnlInvoices_MouseLeave(object sender, EventArgs e)
        {
            lblTotalInvoices.Font = new Font("Microsoft Sans Serif", 15);
        }

        private void radChat1_Click(object sender, EventArgs e)
        {
            
        }
    }
}