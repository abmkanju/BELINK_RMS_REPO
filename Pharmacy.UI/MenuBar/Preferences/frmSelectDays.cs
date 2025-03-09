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

namespace Pharmacy.UI.MenuBar.Preferences
{
    public partial class frmSelectDays : Form
    {
        public string SelectedDays = "";
        public frmSelectDays()
        {
            InitializeComponent();
        }
        public frmSelectDays(string SelectedDays)
        {
            InitializeComponent();
            this.SelectedDays = SelectedDays;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder days = new StringBuilder();
                int id = 0;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    bool Checked = Convert.ToBoolean(r.Cells["colSelect"].Value);
                    if (Checked)
                    {
                        id = Convert.ToInt32(r.Cells["colDayId"].Value);
                        days.Append(id.ToString() + ",");
                    }
                }
                SelectedDays = days.ToString().TrimEnd(',');
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while loading days data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
        }

        private void frmSelectDays_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetSmallButtonsStyle(new[] { btnOk, btnCancel });
                grdItems.Rows.Add(1, false, "Monday");
                grdItems.Rows.Add(2, false, "Tuesday");
                grdItems.Rows.Add(3, false, "Wednesday");
                grdItems.Rows.Add(4, false, "Thursday");
                grdItems.Rows.Add(5, false, "Friday");
                grdItems.Rows.Add(6, false, "Saturday");
                grdItems.Rows.Add(7, false, "Sunday");
                string[] days = this.SelectedDays.Split(',');
                foreach(var d in days)
                {
                    foreach(DataGridViewRow r in grdItems.Rows)
                    {
                        if(r.Cells["colDayId"].Value.ToString().Equals(d))
                        {
                            r.Cells["colSelect"].Value = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while loading days data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {            
            this.Close();
        }
    }
}