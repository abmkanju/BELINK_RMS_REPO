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
    public partial class dlgPrintOptions : Form
    {
        public int Orientation = 0;
        public bool IsPrint = false;
        public bool IsSaveAsPdf = false;
        public bool IsCancelled = true;
        public dlgPrintOptions()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Orientation = cmbOrientation.SelectedIndex;
            IsPrint = true;
            IsCancelled = false;
            this.Close();
        }

        private void btnSaveAsPdf_Click(object sender, EventArgs e)
        {
            Orientation = cmbOrientation.SelectedIndex;
            IsSaveAsPdf = true;
            IsCancelled = false;
            this.Close();
        }

        private void dlgPrintOptions_Load(object sender, EventArgs e)
        {
            cmbOrientation.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
