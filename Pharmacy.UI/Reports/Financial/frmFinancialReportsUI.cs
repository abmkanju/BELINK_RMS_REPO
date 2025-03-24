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

namespace Pharmacy.UI.Reports.Financial
{
    public partial class frmFinancialReportsUI : Form
    {
        public frmFinancialReportsUI()
        {
            InitializeComponent();
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
        }

        private void frmFinancialReportsUI_Load(object sender, EventArgs e)
        {

        }

        private void btnShift_Click(object sender, EventArgs e)
        {
        }

        private void btnPaymentMode_Click(object sender, EventArgs e)
        {
        }

        private void btnPendingPayments_Click(object sender, EventArgs e)
        {
        }

        private void btnRefundReport_Click(object sender, EventArgs e)
        {
        }

        private void btnDeletedInvoices_Click(object sender, EventArgs e)
        {
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
        }

        private void btnStockDetailedReport_Click(object sender, EventArgs e)
        {
        }

        private void radTileElement1_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new FinancialTransactionUI(), this.MdiParent);

        }

        private void radTileElement3_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ShiftUI(), this.MdiParent);

        }

        private void radTileElement5_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new PaymentModeUI(), this.MdiParent);


        }

        private void radTileElement7_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new StaffUI(), this.MdiParent);

        }

        private void radTileElement2_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new PendingPayemntUI(), this.MdiParent);

        }

        private void radTileElement4_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new DeletedInvoicesUI(), this.MdiParent);

        }

        private void radTileElement6_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new RefundUI(), this.MdiParent);

        }

        private void radTileElement8_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new StockDetailReportUI(), this.MdiParent);

        }
    }
}