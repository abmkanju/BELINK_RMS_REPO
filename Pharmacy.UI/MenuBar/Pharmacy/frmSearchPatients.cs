using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
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
    public partial class frmSearchPatients : Form
    {
        static PharmacyContext cxt = new PharmacyContext();
        PatientRepository repPatients = new PatientRepository(cxt);
        public long PatientId = 0;
        public string PatientName = "";
        public frmSearchPatients()
        {
            InitializeComponent();
        }
        private void frmSearchPatients_Load(object sender, EventArgs e)
        {
            LoadPatients();
            SharedFunctions.SetGridStyle(grdPatients);
        }
        private void LoadPatients()
        {
            List<Patient> patients = repPatients.GetAll().ToList();
            grdPatients.DataSource = patients;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            PatientId = Convert.ToInt64(grdPatients.SelectedRows[0].Cells["colPatientId"].Value);
            PatientName = grdPatients.SelectedRows[0].Cells["colPatientId"].Value.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
