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
    public partial class StockStatisticsUI_ : Form
    {
        public StockStatisticsUI_()
        {
            InitializeComponent();
        }

        private void StockStatisticsUI__Load(object sender, EventArgs e)
        {
            this.chart1.Series[0].Points.AddXY("m1", 100);
            this.chart1.Series[0].Points.AddXY("m2", 200);
            this.chart1.Series[0].Points.AddXY("m3", 300);
            this.chart1.Series[0].Points.AddXY("m4", -100);
            this.chart1.Series[0].Points.AddXY("m5", -200);
            this.chart1.Series[0].Points.AddXY("m5", -300);
        }
    }
}