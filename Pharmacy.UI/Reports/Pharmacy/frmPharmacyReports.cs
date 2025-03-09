using Pharmacy.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class frmPharmacyReports : Form
    {
        public frmPharmacyReports()
        {
            InitializeComponent();
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
        }

        private void btnNarcotic_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new NarcoticDrugsUI(), this.MdiParent);
        }

        private void btnItemWiseSale_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ItemWiseSaleUI(), this.MdiParent);
        }

        private void btnItemWiseRevenue_Click(object sender, EventArgs e)
        {
        }

        private void btnLowStocks_Click(object sender, EventArgs e)
        {
        }

        private void btnIncomeStatement_Click(object sender, EventArgs e)
        {
        }

        private void btnPurchaseReports_Click(object sender, EventArgs e)
        {
        }

        private void btnDiscounts_Click(object sender, EventArgs e)
        {
        }

        private void btnProfitStatement_Click(object sender, EventArgs e)
        {
        }

        private void btnDeadStock_Click(object sender, EventArgs e)
        {
        }

        private void btnExpiry_Click(object sender, EventArgs e)
        {
        }

        private void btnStockStatistics_Click(object sender, EventArgs e)
        {
        }
        private void btnItemWisePurchase_Click(object sender, EventArgs e)
        {
        }

        private void btnNarcoticWiseItems_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new NarcoticItemswiseSale(), this.MdiParent);
        }

        private void btSalesReturnReports_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void radTileElement1_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new TransactionUI(), this.MdiParent);

        }

        private void radTileElement4_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ItemWiseRevenueUI(), this.MdiParent);

        }

        private void radTileElement7_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new LowStockUI(), this.MdiParent);

        }

        private void radTileElement10_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new IncomeStatementUI(), this.MdiParent);

        }

        private void radTileElement13_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new PurchaseReportsUI(), this.MdiParent);

        }

        private void radTileElement16_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new SalesReturnReport(), this.MdiParent);

        }

        private void radTileElement2_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ItemWisePurchaseUI(), this.MdiParent);

        }

        private void radTileElement5_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new DiscountsUI(), this.MdiParent);

        }

        private void radTileElement8_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new DeadStockUI(), this.MdiParent);

        }

        private void radTileElement11_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ProfiStatementUI(), this.MdiParent);

        }

        private void radTileElement14_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ExpiryUI(), this.MdiParent);

        }

        private void radTileElement17_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new GrossProfitStatementUI(), this.MdiParent);

        }

        private void radTileElement3_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new StockStatisticsUI(), this.MdiParent);

        }
    }
}