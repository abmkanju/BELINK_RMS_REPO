using Pharmacy.Core;
using Pharmacy.UI.Reports.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.Reports.Product
{
    public partial class frmProductReports : Form
    {
        public frmProductReports()
        {
            InitializeComponent();
        }

        private void frmPharmacyReports_Load(object sender, EventArgs e)
        {

        }

        private void btn1_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ProfitMarginUI(), this.MdiParent);
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new DiscountedPriceUI(), this.MdiParent);
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ProductPriceUI(), this.MdiParent);
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ProductExpiryUI(), this.MdiParent);
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new AllItemsDiscountsUI(), this.MdiParent);
        }
    }
}