using Pharmacy.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI
{
    public partial class DataSyncingUI : Form
    {
        public DataSyncingUI()
        {
            InitializeComponent();
            if (DataSyncingManagement.GetStatusPharmacySyncingService())
            {
                button3.Text = "Start Service";
            }
            else
            {
                button3.Text = "Stop Service";
            }
            this.Cursor = Cursors.WaitCursor;
            dataGridView1.Columns.Clear();
            DataSyncingManagement.SetDataSource(dataGridView1 ,  checkBox.Checked);
            this.Cursor = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                DialogResult dr = MessageBox.Show("Are You Sure You want to Change the Syncing Status of The Task?", "Changing Status of Task", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    DataGridViewCheckBoxCell cell = this.dataGridView1.CurrentCell as DataGridViewCheckBoxCell;
                    if (cell != null)
                    {
                        cell.Value = cell.Value == null || !((bool)cell.Value);
                        this.dataGridView1.RefreshEdit();
                        this.dataGridView1.NotifyCurrentCellDirty(true);
                        int id = Convert.ToInt32(this.dataGridView1.Rows[e.RowIndex].Cells["OfflineTaskId"].Value.ToString());

                        ExecuteQuery(id, ((bool)cell.Value));
                    }
                }
            }
            
        }
        private void ExecuteQuery(int id , bool st)
        {
            string Query = "update OfflineTasks set IsActive = '" + st + "' where OfflineTaskId=" + id;
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            SqlCommand com = new SqlCommand(Query, con);
            if(con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            com.ExecuteNonQuery();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                string json = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                

                new DataViewUI(json).ShowDialog();
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
           

            this.Cursor = Cursors.WaitCursor;
            dataGridView1.Columns.Clear();
            DataSyncingManagement.SetDataSource(dataGridView1, checkBox.Checked);
            this.Cursor = Cursors.Default;
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            dataGridView1.Columns.Clear();
            DataSyncingManagement.SetDataSource(dataGridView1, checkBox.Checked);
            this.Cursor = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DataSyncingManagement.GetStatusPharmacySyncingService())
            {
                button3.Text = "Stop Service";
                DataSyncingManagement.StartPharmacySyncingService();
            }
            else
            {
                button3.Text = "Start Service";
                DataSyncingManagement.StopPharmacySyncingService();
            }
        }
    }
}
