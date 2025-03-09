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
using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;

namespace Pharmacy.UI.Indoor.Menu
{
    public partial class frmAdmittedPatientTreatment : Form
    {
        UnitOfWork unitOfWork;
        IPagedList<Vital> Vitals;
        int PageNo = 1;
        long AdmittedPatientId = 0;
        private bool LoadVitalGraphs = false;

        public frmAdmittedPatientTreatment()
        {
            InitializeComponent();
        }

        public frmAdmittedPatientTreatment(long AdmittedPatientId)
        {
            InitializeComponent();
            this.AdmittedPatientId = AdmittedPatientId;
        }

        private void btnNewVital_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddVital(0,this.AdmittedPatientId), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
        }

        private void ChildForm_CLosed(object sender, FormClosedEventArgs e)
        {
            btnRefreshVitals.PerformClick();
        }      

        private void frmAdmittedPatientTreatment_Load(object sender, EventArgs e)
        {
            try
            {
                #region vitals data
                LoadVitalGraphs = true;
                LoadVitalsData();
                #endregion vitals data
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadVitalsData()
        {
            List<Vital> VitalsGraphData = new List<Vital>();
            using(unitOfWork = new UnitOfWork ())
            {
                Vitals = unitOfWork.VitalRepository.GetVitals_Paginated(PageNo, SharedVariables.PageSize, this.AdmittedPatientId);
                if (LoadVitalGraphs)
                {
                    dtpFrom_VitalGraph.ValueChanged -= dtpFrom_VitalGraph_ValueChanged;
                    dtpFrom_VitalGraph.Value = unitOfWork.AdmitPatientsRepository.GetAdmissionDate(this.AdmittedPatientId);
                    dtpFrom_VitalGraph.ValueChanged += dtpFrom_VitalGraph_ValueChanged;
                    VitalsGraphData = unitOfWork.VitalRepository.GetVitals(dtpFrom_VitalGraph.Value, dtpTo_VitalGraph.Value, this.AdmittedPatientId);
                }

            }
            grdVitals.Rows.Clear();
            foreach(Vital v in Vitals)
            {
                grdVitals.Rows.Add(v.VitalId, v.Temprature, v.Weight, v.BloodPressure, v.BloodSugar, v.Test, v.RespiratoryRate, v.OxygenSaturation, v.User.UserName, v.CreateDate);
            }
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, Vitals.PageCount);
            btnFirstPage.Enabled = btnPrevious.Enabled = Vitals.HasPreviousPage;
            btnLastPage.Enabled = btnNext.Enabled = Vitals.HasNextPage;
            if(LoadVitalGraphs)
            {
                chartWeight.DataSource = VitalsGraphData;
                chartWeight.Series["Weight"].XValueMember = "CreateDate";
                chartWeight.Series["Weight"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                chartWeight.Series["Weight"].YValueMembers = "Weight";
                chartWeight.Series["Weight"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                chartTemprature.DataSource = VitalsGraphData;
                chartTemprature.Series["Temprature"].XValueMember = "CreateDate";
                chartTemprature.Series["Temprature"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                chartTemprature.Series["Temprature"].YValueMembers = "Temprature";
                chartTemprature.Series["Temprature"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                chartBloodPressure.DataSource = VitalsGraphData;
                chartBloodPressure.Series["BloodPressure"].XValueMember = "CreateDate";
                chartBloodPressure.Series["BloodPressure"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                chartBloodPressure.Series["BloodPressure"].YValueMembers = "BloodPressure";
                chartBloodPressure.Series["BloodPressure"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                chartTest.DataSource = VitalsGraphData;
                chartTest.Series["Test"].XValueMember = "CreateDate";
                chartTest.Series["Test"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                chartTest.Series["Test"].YValueMembers = "Test";
                chartTest.Series["Test"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                chartRespiratoryRate.DataSource = VitalsGraphData;
                chartRespiratoryRate.Series["Respiratory Rate"].XValueMember = "CreateDate";
                chartRespiratoryRate.Series["Respiratory Rate"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                chartRespiratoryRate.Series["Respiratory Rate"].YValueMembers = "RespiratoryRate";
                chartRespiratoryRate.Series["Respiratory Rate"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                chartBloodSugar.DataSource = VitalsGraphData;
                chartBloodSugar.Series["Blood Sugar"].XValueMember = "CreateDate";
                chartBloodSugar.Series["Blood Sugar"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                chartBloodSugar.Series["Blood Sugar"].YValueMembers = "BloodSugar";
                chartBloodSugar.Series["Blood Sugar"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                chartOxygenSaturation.DataSource = VitalsGraphData;
                chartOxygenSaturation.Series["Oxygen Saturation"].XValueMember = "CreateDate";
                chartOxygenSaturation.Series["Oxygen Saturation"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                chartOxygenSaturation.Series["Oxygen Saturation"].YValueMembers = "OxygenSaturation";
                chartOxygenSaturation.Series["Oxygen Saturation"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            }
        }

        private void btnRefreshVitals_Click(object sender, EventArgs e)
        {
            LoadVitalGraphs = true;
            PageNo = 1;
            LoadVitalsData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            LoadVitalGraphs = false;
            PageNo += 1;
            LoadVitalsData();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            LoadVitalGraphs = false;
            PageNo = Vitals.PageCount;
            LoadVitalsData();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            LoadVitalGraphs = false;
            PageNo -= 1;
            LoadVitalsData();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            LoadVitalGraphs = false;
            PageNo = 1;
            LoadVitalsData();
        }

        private void grdVitals_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            long VitalId = Convert.ToInt64(grdVitals.Rows[e.RowIndex].Cells["colVitalId"].Value);
            if(e.ColumnIndex == grdVitals.Columns["colEdit"].Index)
            {
                Form f = SharedFunctions.OpenForm(new frmAddVital(VitalId, this.AdmittedPatientId), this.MdiParent);
                f.FormClosed += new FormClosedEventHandler (ChildForm_CLosed);
            }
            else if (e.ColumnIndex == grdVitals.Columns["colDelete"].Index)
            {
                DialogResult res = MessageBox.Show("Are you sure you want to delete this vital.", "Pleae Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (res == System.Windows.Forms.DialogResult.Cancel) return;
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Vital obj = unitOfWork.VitalRepository.GetById(VitalId);
                        obj.IsUpdate = true;
                        obj.UpdateDate = DateTime.Now;
                        obj.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        obj.IsSynced = false;
                        obj.IsActive = false;
                        unitOfWork.VitalRepository.Update(obj);
                        unitOfWork.Save();
                        btnRefreshVitals.PerformClick();
                        MessageBox.Show("Selected vital deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("An error occurred while deleting selected vital, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtpFrom_VitalGraph_ValueChanged(object sender, EventArgs e)
        {
            LoadVitalGraphs = true;
            PageNo = 1;
            LoadVitalsData();
        }

        private void dtpTo_VitalGraph_ValueChanged(object sender, EventArgs e)
        {
            LoadVitalGraphs = true;
            PageNo = 1;
            LoadVitalsData();
        }
    }
}