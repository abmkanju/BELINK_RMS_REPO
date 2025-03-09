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
using Pharmacy.Core.Entities;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmSearchPatient : Form
    {
        static PharmacyContext cxt = new PharmacyContext();
        PatientRepository repPatients = new PatientRepository(cxt);
        List<Patient> patientsList = new List<Patient>();
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public frmSearchPatient()
        {
            InitializeComponent();
        }

        private void frmSearchPatient_Load(object sender, EventArgs e)
        {
            patientsList = repPatients.GetAll().ToList();
            grdPatients.DataSource = patientsList;
            HideCols();
            SharedFunctions.SetGridStyle(grdPatients);
        }

        private void txtSearchPatient_TextChanged(object sender, EventArgs e)
        {
            string searchString = txtSearchPatient.Text.Trim().ToLower();
            List<Patient> FilteredPatients = new List<Patient>();
            FilteredPatients = patientsList.Where(p => p.MRNo.ToString().ToLower().Contains(searchString) || p.Name.ToLower().Contains(searchString) || p.Phone.ToLower().Contains(searchString)).ToList();
            grdPatients.DataSource = FilteredPatients;
            if(grdPatients.Rows.Count > 0)
            {
                grdPatients.Rows[0].Selected = true;
            }
            HideCols();
        }
        private void HideCols()
        {
            grdPatients.Columns["patientId"].Visible = false;
            grdPatients.Columns["ImagePath"].Visible = false;
        }

        private void txtSearchPatient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdPatients.SelectedRows.Count > 0)
                {
                    GetPatientData();
                    this.Close();
                }
            }
        }        
        private void GetPatientData()
        {
            PatientId = Convert.ToInt64(grdPatients.SelectedRows[0].Cells["patientId"].Value);
            PatientName = grdPatients.SelectedRows[0].Cells["Name"].Value.ToString();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (grdPatients.SelectedRows.Count > 0)
            {
                GetPatientData();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please Select Any Row To Load Customer", "Please Select Row", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdPatients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                PatientId = Convert.ToInt64(grdPatients.Rows[e.RowIndex].Cells["patientId"].Value);
                PatientName = grdPatients.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                this.Close();
            }
        }
    }
}
