using OfficeOpenXml;
using Pharmacy.Core;
using Pharmacy.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.Franchise_Deployement
{
    public partial class frmFranchiseDeployement : Form
    {
        public frmFranchiseDeployement()
        {
            InitializeComponent();
            if (chkEnableFranchise.Checked)
            {
                button1.Enabled = true;
                txtPassword.Enabled = true;
                txtUsername.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                txtPractiseID.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
                txtPassword.Enabled = false;
                txtUsername.Enabled = false;
                button4.Enabled = false;
                button3.Enabled = false;
                txtPractiseID.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableFranchise.Checked)
            {
                button4.Enabled = true;
                txtPassword.Enabled = true;
                txtUsername.Enabled = true;
                button1.Enabled = true;
                button3.Enabled = true;
                txtPractiseID.Enabled = true;

            }
            else
            {
                button4.Enabled = false;
                txtPassword.Enabled = false;
                txtUsername.Enabled = false;
                button1.Enabled = false;
                button3.Enabled = false;
                txtPractiseID.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                string query = "SELECT DISTINCT dbo.Items.ItemID, dbo.Items.ItemName, '' as Quantity , dbo.Items.RetailPrice ,dbo.Items.UnitCostPrice as CostPrice, dbo.Items.Unique_itemId as Unique_Id  FROM dbo.Items INNER JOIN dbo.StockItems ON dbo.Items.ItemID = dbo.StockItems.Item_ItemID WHERE dbo.Items.IsActive = 1";
                string ExcelFilePath = "";
                OpenFileDialog ofd = new OpenFileDialog();
                MessageBox.Show("Choose a Blank Excel File to save data....");
                DialogResult dr = ofd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    ExcelFilePath = ofd.FileName;
                }
                this.Cursor = Cursors.WaitCursor;
                if (ExcelFilePath != "")
                {
                    ExportDataToExcel(Shared.GetConnectionString(), query, ExcelFilePath);
                }
                this.Cursor = Cursors.Default;
            }
            else
            {
                string query = "SELECT DISTINCT dbo.Items.ItemID, dbo.Items.ItemName, dbo.Items.ConversionUnit, dbo.Items.NoOfStrips, dbo.Items.Unique_itemId, '' as DefaultName FROM dbo.Items INNER JOIN dbo.StockItems ON dbo.Items.ItemID = dbo.StockItems.Item_ItemID WHERE dbo.Items.IsActive = 1";
                string ExcelFilePath = "";
                OpenFileDialog ofd = new OpenFileDialog();
                MessageBox.Show("Choose a Blank Excel File to save data....");
                DialogResult dr = ofd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    ExcelFilePath = ofd.FileName;
                }
                this.Cursor = Cursors.WaitCursor;
                if (ExcelFilePath != "")
                {
                    ExportDataToExcel(Shared.GetConnectionString(), query, ExcelFilePath);
                }
                this.Cursor = Cursors.Default;
            }
        }
        static void ExportDataToExcel(string connectionString, string sqlQuery, string excelFilePath)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                        using (ExcelPackage excelPackage = new ExcelPackage())
                        {
                            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("DataSheet");
                            worksheet.Cells.LoadFromDataTable(dataTable, true);

                            FileInfo excelFile = new FileInfo(excelFilePath);
                            excelPackage.SaveAs(excelFile);
                        }
                    }
                }
                MessageBox.Show("Data Sheet Generated Successfully.");
                connection.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Config Files (*.config)|*.config";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                txtServiceFile.Text = ofd.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files (*.xls)|*.xls";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                txtItemSheet.Text = ofd.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                try
                {
                    if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtPractiseID.Text) || string.IsNullOrEmpty(txtItemSheet.Text) || string.IsNullOrEmpty(txtServiceFile.Text))
                    {
                        MessageBox.Show("Something is missing, Please Try Again...");
                        return;
                    }

                    UpdateConfigFile(txtServiceFile.Text, Shared.GetConnectionStringEncrypted(), CryptopEngine.Encrypt(txtUsername.Text, GlobalSharing.Salt), CryptopEngine.Encrypt(txtPassword.Text, GlobalSharing.Salt), CryptopEngine.Encrypt(txtPractiseID.Text, GlobalSharing.Salt));
                    new frmMappingProgress(txtUsername.Text, txtPassword.Text, Convert.ToInt64(txtPractiseID.Text), txtItemSheet.Text , true).ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong, Please Try Again....");
                }
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtPractiseID.Text) || string.IsNullOrEmpty(txtItemSheet.Text) || string.IsNullOrEmpty(txtServiceFile.Text))
                    {
                        MessageBox.Show("Something is missing, Please Try Again...");
                        return;
                    }

                    UpdateConfigFile(txtServiceFile.Text, Shared.GetConnectionStringEncrypted(), CryptopEngine.Encrypt(txtUsername.Text, GlobalSharing.Salt), CryptopEngine.Encrypt(txtPassword.Text, GlobalSharing.Salt), CryptopEngine.Encrypt(txtPractiseID.Text, GlobalSharing.Salt));
                    new frmMappingProgress(txtUsername.Text, txtPassword.Text, Convert.ToInt64(txtPractiseID.Text), txtItemSheet.Text).ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something went wrong, Please Try Again....");
                }
            }
        }
        public static void UpdateConfigFile(string configFile, string pharmacyCon, string email, string password, string practiceId)
        {
            try
            {
                var configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configFile };
                var configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

                var connectionStringSettingsCollection = configuration.ConnectionStrings.ConnectionStrings;

                // Update the connection strings
                connectionStringSettingsCollection["PharmacyCon"].ConnectionString = pharmacyCon;
                connectionStringSettingsCollection["Email"].ConnectionString = email;
                connectionStringSettingsCollection["Password"].ConnectionString = password;
                connectionStringSettingsCollection["PracticeId"].ConnectionString = practiceId;
                connectionStringSettingsCollection["IsFranchise"].ConnectionString = "true";

                // Save the changes to the configuration file
                configuration.Save(ConfigurationSaveMode.Modified);

                // Force a reload of the changed section.
                ConfigurationManager.RefreshSection("connectionStrings");

                //MessageBox.Show("Configuration file updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating configuration file: " + ex.Message);
            }
        }

        private void txtPractiseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                button2.Text = "Download Items Sheet";
                label8.Text = "Upload Mapped Items Sheet";
            }
            else
            {
                button2.Text = "Download Stock Sheet";
                label8.Text = "Upload Mapped Stock Sheet";
            }
        }
    }
}
