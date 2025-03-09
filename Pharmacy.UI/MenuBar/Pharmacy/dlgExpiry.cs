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
    public partial class dlgExpiry : Form
    {
        public DateTime? selectedDate;
        public dlgExpiry()
        {
            InitializeComponent();
        }
        private void btnExpApply_Click(object sender, EventArgs e)
        {
            this.selectedDate = DateTime.Parse(dtpExpiry.SelectionRange.Start.ToString());
            this.Close();
        }

        private void btnExpCancel_Click(object sender, EventArgs e)
        {
            this.selectedDate = null;
            this.Close();
        }
    }
}
