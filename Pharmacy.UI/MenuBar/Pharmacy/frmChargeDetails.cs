using Pharmacy.Core.Entities;
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

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmChargeDetails : Form
    {
        static PharmacyContext cxt = new PharmacyContext();
        public ChargeInfo chargeinfo = new ChargeInfo();
        public frmChargeDetails()
        {
            InitializeComponent();
           
        }

        public frmChargeDetails(ChargeInfo newchargeInfo)
        {
            InitializeComponent();
            this.chargeinfo = newchargeInfo;
            if (chargeinfo != null)
            {
                txtInvoiceNumber.Text = chargeinfo.P_InvoiceNo;
                txtRiderName.Text = chargeinfo.P_RiderName;
                txtVendor.Text = chargeinfo.P_Vendor;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtInvoiceNumber.Text != string.Empty && txtRiderName.Text != string.Empty && txtVendor.Text != string.Empty)
            {
                this.chargeinfo.P_InvoiceNo = txtInvoiceNumber.Text;
                this.chargeinfo.P_RiderName = txtRiderName.Text;
                this.chargeinfo.P_Vendor = txtVendor.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Something is Missing ... ");
            }
        }

        private void bntCancel_Click(object sender, EventArgs e)
        {
            this.chargeinfo = null;
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtInvoiceNumber.Clear();
            txtRiderName.Clear();
            txtVendor.Clear();
        }
    }
}
