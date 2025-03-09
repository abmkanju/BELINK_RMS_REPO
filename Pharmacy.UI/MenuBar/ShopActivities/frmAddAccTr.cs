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

namespace Pharmacy.UI.MenuBar.ShopActivities
{
    public partial class frmAddAccTr : Form
    {
        UnitOfWork unitofwork;
        public frmAddAccTr()
        {
            InitializeComponent();
        }

        private void frmAddAccTr_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                AccTransaction tr = new AccTransaction();
                tr.TrDate = dtpTrDate.Value;
                tr.Amount = double.Parse(txtAmount.Text);
                tr.Type = cmbChangeTo.SelectedIndex - 1;
                tr.Description = txtcomments.Text.Trim();
                using (unitofwork = new UnitOfWork())
                {
                    unitofwork.AccTransactionsRepository.Insert(tr);
                    unitofwork.Save();
                }
                MessageBox.Show("Account transaction inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while inserting accoutn transation, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}