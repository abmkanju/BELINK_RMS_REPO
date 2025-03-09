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

namespace Pharmacy.UI.Reports
{
    public partial class frmItemBarcodeViewer : Form
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        private int ItemId = 0;
        public frmItemBarcodeViewer()
        {
            InitializeComponent();
        }

        public frmItemBarcodeViewer(int ItemId)
        {
            InitializeComponent();
            this.ItemId = ItemId;
        }

        private void frmItemBarcodeViewer_Load(object sender, EventArgs e)
        {
            string barcode = "";
            using(unitOfWork = new UnitOfWork ())
            {
                barcode = unitOfWork.ItemRspository.GetItemBarcode(this.ItemId);
            }
            rptItemBarcode r = new rptItemBarcode();
            r.SetParameterValue("barcode", barcode);
            crystalReportViewer1.ReportSource = r;
            crystalReportViewer1.Show();
        }
    }
}