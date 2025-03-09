
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

namespace Pharmacy.UI.MenuBar.UserMenue
{
    public partial class frmRestoreData : Form
    {
        public frmRestoreData()
        {
            InitializeComponent();
            txtcurrentdbname.Text = Shared.GetConnectionBuilder().InitialCatalog;
            txtRestoreDate.Text = DateTime.Now.ToShortDateString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lblRestoreProgress.Text = "InProgress .... ";
            lblRestorePercent.Text = "Restore is In Progress ..." + "(2%)";
            RestoreProgressBar.Value = 10;
            RestoreDB();
        }
        private void RestoreDB()
        {
            lblRestoreProgress.Text = "InProgress .... ";
            lblRestorePercent.Text = "Restore is In Progress ..." + "15%)";
            RestoreProgressBar.Value = 15;
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            string database = txtcurrentdbname.Text;
            if (con.State != ConnectionState.Open)
            {
                con.Open();
                lblRestoreProgress.Text = "InProgress .... ";
                lblRestorePercent.Text = "Restore is In Progress ..." + "30%)";
                RestoreProgressBar.Value = 50;
            }
            try
            {
                string sqlStmt2 = string.Format("ALTER DATABASE [" + database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                SqlCommand bu2 = new SqlCommand(sqlStmt2, con);
                bu2.ExecuteNonQuery();
                lblRestoreProgress.Text = "InProgress .... ";
                lblRestorePercent.Text = "Restore is In Progress ..." + "50%)";
                RestoreProgressBar.Value = 50;
                string sqlStmt3 = "USE MASTER RESTORE DATABASE [" + database + "] FROM DISK='" + txtbackupfileaddress.Text + "' WITH REPLACE;";
                SqlCommand bu3 = new SqlCommand(sqlStmt3, con);
                int a = bu3.ExecuteNonQuery();
                lblRestoreProgress.Text = "InProgress .... ";
                lblRestorePercent.Text = "Restore is In Progress ..." + "70%)";
                RestoreProgressBar.Value = 70;
                string sqlStmt4 = string.Format("ALTER DATABASE [" + database + "] SET MULTI_USER");
                SqlCommand bu4 = new SqlCommand(sqlStmt4, con);
                bu4.ExecuteNonQuery();
                lblRestoreProgress.Text = "InProgress .... ";
                lblRestorePercent.Text = "Restore is In Progress ..." + "99%)";
                RestoreProgressBar.Value = 99;
                if (a != 0)
                {
                    MessageBox.Show("database restoration done successefully");
                    con.Close();
                    lblRestoreProgress.Text = "Finisheds .... ";
                    lblRestorePercent.Text = "Restore Completed ..." + "100%)";
                    RestoreProgressBar.Value = 100;
                }
                else
                {
                    lblRestoreProgress.Text = "UnExpected Error Occurred .... ";
                    lblRestorePercent.Text = "Restore Failed ..." + "0%)";
                    RestoreProgressBar.Value = 00;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SQL Server BackUp File (*.bak)|*.bak";
            ofd.Title = "Choose Backup File";
            ofd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtbackupfileaddress.Text = ofd.FileName;
            }
        }
    }
}
