using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmChequeInfo : Form
    {
        static PharmacyContext cxt = new PharmacyContext();
        ChequeInfoRepository repChequeInfo = new ChequeInfoRepository(cxt);
        public ChequeInfo ChequeInfo = new ChequeInfo();
        public frmChequeInfo()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtXhequeNo.Text = "";
            txtBankName.Text = "";
            txtChequeStatus.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {               
                this.ChequeInfo.ChequeNo = txtXhequeNo.Text.Trim();
                this.ChequeInfo.BankName = txtBankName.Text.Trim();
                this.ChequeInfo.Status = txtChequeStatus.Text.Trim();                
                //SharedFunctions.ShowSuccessMessage(SharedVariables.GeneralSuccMsg, "Success");
                this.Close();
            }
            catch(Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void frmChequeInfo_Load(object sender, EventArgs e)
        {

        }

        private void bntCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}