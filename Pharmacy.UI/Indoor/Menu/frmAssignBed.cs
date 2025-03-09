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
using Pharmacy.Infrastructure;
namespace Pharmacy.UI.Indoor.Menu
{
    public partial class frmAssignBed : Form
    {
        UnitOfWork unitOfWork;
        private long AdmittedPatientId = 0;
        private bool errPatientAlredyAdmitted = false;
        private List<User> LinkedDoctors;
        public frmAssignBed()
        {
            InitializeComponent();
        }
        public frmAssignBed(long AdmittedPatientId)
        {
            InitializeComponent();
            this.AdmittedPatientId = AdmittedPatientId;
        }

        private void frmAssignBed_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave, btnClear });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnAddPatient });
            try
            {
                LoadPatients();
                LoadWards();
                LoadBeds();
                ShowAdmissionNo();
                LoadDoctors();
                if (this.AdmittedPatientId > 0)
                {
                    LoaddmittedPatientData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading form data, please try again.", "Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoaddmittedPatientData()
        {
            AdmittedPatient obj = new AdmittedPatient();
            using (unitOfWork = new UnitOfWork())
            {
                obj = unitOfWork.AdmitPatientsRepository.GetAdmittedPatientDataById(this.AdmittedPatientId);
            }
            cmbPatient.Enabled = false;
            cmbPatient.SelectedIndexChanged -= cmbPatient_SelectedIndexChanged;// to avoid err : patient already admitted
            cmbPatient.SelectedValue = obj.Patient.PatientId;
            cmbPatient.SelectedIndexChanged += cmbPatient_SelectedIndexChanged;
            cmbWard.SelectedValue = obj.Ward.WardId;
            cmbBedNo.SelectedValue = obj.Bed.BedId;
            txtReasonForAdm.Text = obj.ReasonForAdmission;
            numAdmNo.Enabled = false;
            numAdmNo.Value = obj.AdmissionNo;
            txtDiagnosis.Text = obj.Diagnosis;
            txtConsultant.Text = obj.Consultant;
            txtAnesthetist.Text = obj.Anesthetist;
            dtpOpDate.Value = obj.OperationDate;
            LinkedDoctors = new List<User>();
            foreach (User u in obj.Doctors)
            {
                lbDoctors.SelectedIndex = lbDoctors.FindStringExact(u.UserName);
                LinkedDoctors.Add(u);

            }
        }
        private void LoadPatients()
        {
            cmbPatient.SelectedIndexChanged -= cmbPatient_SelectedIndexChanged;
            List<Patient> ps = new List<Patient>();
            using (unitOfWork = new UnitOfWork())
            {
                ps = unitOfWork.PatientRepository.GetActivePatients();
            }
            Patient p = new Patient();
            p.PatientId = 0;
            p.Name = "Select Patient";
            ps.Insert(0, p);
            cmbPatient.DataSource = ps;
            cmbPatient.ValueMember = "PatientId";
            cmbPatient.DisplayMember = "Name";
            cmbPatient.SelectedIndexChanged += cmbPatient_SelectedIndexChanged;
        }
        private void LoadWards()
        {
            cmbWard.SelectedIndexChanged -= cmbWard_SelectedIndexChanged;
            List<Ward> wards = new List<Ward>();
            using (unitOfWork = new UnitOfWork())
            {
                wards = unitOfWork.WardRepository.GetAllActiveWards();
            }
            Ward w = new Ward();
            w.WardId = 0;
            w.Name = "Select Ward";
            wards.Insert(0, w);
            cmbWard.DataSource = wards;
            cmbWard.ValueMember = "WardId";
            cmbWard.DisplayMember = "Name";
            cmbWard.SelectedIndexChanged += cmbWard_SelectedIndexChanged;
        }
        private void LoadBeds()
        {
            List<Bed> Beds = new List<Bed>();
            using (unitOfWork = new UnitOfWork())
            {
                Beds = unitOfWork.BedRepository.GetAllActiveBeds();
            }
            Bed b = new Bed();
            b.BedId = 0;
            b.RoomNo = "Select Bed";
            Beds.Insert(0, b);
            cmbBedNo.DataSource = Beds;
            cmbBedNo.ValueMember = "BedId";
            cmbBedNo.DisplayMember = "RoomNo";
        }
        private void LoadDoctors()
        {
            List<User> Users = new List<User>();
            using (unitOfWork = new UnitOfWork())
            {
                Users = unitOfWork.UserRepository.GetActiveDoctors();
                lbDoctors.DataSource = Users;
                lbDoctors.ValueMember = "UserId";
                lbDoctors.DisplayMember = "UserName";
                lbDoctors.ClearSelected();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {                                
                if (this.AdmittedPatientId > 0)
                {
                    UpdateData();
                }
                else
                {
                    Add();
                }
            }
        }
        private void FillObject(ref AdmittedPatient obj)
        {
            obj.ReasonForAdmission = txtReasonForAdm.Text.Trim();
            obj.AdmissionNo = (long)numAdmNo.Value;
            obj.Diagnosis = txtDiagnosis.Text.Trim();
            obj.Consultant = txtConsultant.Text.Trim();
            obj.Anesthetist = txtAnesthetist.Text.Trim();
            obj.OperationDate = dtpOpDate.Value;
            obj.UpdateDate = DateTime.Now;
            obj.IsActive = true;
        }
        private void Add()
        {
            try
            {
                AdmittedPatient obj = new AdmittedPatient();
                FillObject(ref obj);
                using (unitOfWork = new UnitOfWork())
                {
                    obj.Patient = unitOfWork.PatientRepository.GetById(cmbPatient.SelectedValue);
                    obj.Ward = unitOfWork.WardRepository.GetById(cmbWard.SelectedValue);
                    obj.Bed = unitOfWork.BedRepository.GetById(cmbBedNo.SelectedValue);
                    obj.Bed.IsAvailable = false;
                    obj.CreateDate = DateTime.Now;
                    obj.Bed.UpdateDate = DateTime.Now;
                    obj.Bed.IsSynced = false;
                    obj.Bed.IsUpdate = true;
                    //obj.Vitals = null;
                    //unitOfWork.GetDbContext().Entry(obj.Bed).State = System.Data.Entity.EntityState.Modified;
                    obj.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);// SharedVariables.LoggedInUser;
                    if (lbDoctors.SelectedItems.Count > 0)
                    {
                        obj.Doctors = new List<User>();
                        foreach (User Doc in lbDoctors.SelectedItems)
                        {
                            User objDoctor = unitOfWork.UserRepository.GetById(Doc.UserId);
                            obj.Doctors.Add(objDoctor);
                        }
                    }
                    unitOfWork.AdmitPatientsRepository.Insert(obj);
                    unitOfWork.Save();
                }
                MessageBox.Show("Bed assigned successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while assigning bed to patient, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateData()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdmittedPatient obj = unitOfWork.AdmitPatientsRepository.GetAdmittedPatientDataById(this.AdmittedPatientId);
                    FillObject(ref obj);
                    obj.UpdateDate = DateTime.Now;
                    //obj.Patient = unitOfWork.PatientRepository.GetById(cmbPatient.SelectedValue);
                    obj.Ward = unitOfWork.WardRepository.GetById(cmbWard.SelectedValue);
                    // re-allocate existing bed if its been changed
                    Bed ExistingBed = unitOfWork.BedRepository.GetById(obj.Bed.BedId);
                    ExistingBed.IsUpdate = true;
                    ExistingBed.IsSynced = false;
                    ExistingBed.UpdateDate = DateTime.Now;
                    ExistingBed.IsAvailable = true;
                    unitOfWork.GetDbContext().Entry(ExistingBed).State = System.Data.Entity.EntityState.Modified;
                    obj.Bed = unitOfWork.BedRepository.GetById(cmbBedNo.SelectedValue);
                    obj.Bed.IsUpdate = true;
                    obj.Bed.IsSynced = false;
                    obj.Bed.UpdateDate = DateTime.Now;
                    obj.Bed.IsAvailable = false;
                    unitOfWork.GetDbContext().Entry(obj.Bed).State = System.Data.Entity.EntityState.Modified;
                    obj.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    List<User> DeSelectedDoctors = new List<User>();
                    List<User> NewSelectedDoctors = new List<User>();
                    bool Found = false;
                    foreach (User s in LinkedDoctors)
                    {
                        Found = false;
                        foreach (User u in lbDoctors.SelectedItems)
                        {
                            if (s.UserId == u.UserId)
                            {
                                Found = true;
                            }
                        }
                        if (!Found)
                        {
                            DeSelectedDoctors.Add(s);
                        }
                    }
                    Found = false;
                    foreach (User s in lbDoctors.SelectedItems)
                    {
                        Found = false;
                        foreach (User u in LinkedDoctors)
                        {
                            if (s.UserId == u.UserId)
                            {
                                Found = true;
                            }
                        }
                        if (!Found)
                        {
                            NewSelectedDoctors.Add(s);
                        }
                    }
                    foreach (User doc in DeSelectedDoctors)
                    {
                        User exs = obj.Doctors.Where(d => d.UserId == doc.UserId).FirstOrDefault();
                        obj.Doctors.Remove(exs);
                    }
                    //foreach (User doc in LinkedDoctors)
                    //{
                    //    //User exs = obj.Doctors.Where(d => d.UserId == doc.UserId).FirstOrDefault();
                    //    obj.Doctors.Remove(doc);
                    //}
                    foreach (User doc in NewSelectedDoctors)
                    {
                        User d = unitOfWork.UserRepository.GetById(doc.UserId);
                        obj.Doctors.Add(d);
                    }
                   
                    unitOfWork.AdmitPatientsRepository.Update(obj);
                    unitOfWork.Save();
                    unitOfWork.Dispose();
                }
                MessageBox.Show("Bed assign updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while updating assigned bed to patient, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool IsValidInput()
        {
            ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            bool ErrFound = false;

            if (cmbPatient.SelectedIndex <= 0)
            {
                errPatient.Visible = true;
                ErrFound = true;
                cmbWard.Focus();
            }
            else
            {
                errPatient.Visible = false;
            }

            if (cmbWard.SelectedIndex <= 0)
            {
                errWard.Visible = true;
                ErrFound = true;
                cmbWard.Focus();
            }
            else
            {
                errWard.Visible = false;
            }

            if (cmbBedNo.SelectedIndex <= 0)
            {
                errBedNo.Visible = true;
                ErrFound = true;
                cmbWard.Focus();
            }
            else
            {
                errBedNo.Visible = false;
            }

            if (numAdmNo.Value <= 0)
            {
                errAdmNo.Visible = true;
                ErrFound = true;
            }
            else
            {
                errAdmNo.Visible = false;
            }

            if (errPatientAlredyAdmitted)
            {
                ErrFound = true;
            }

            if (!ErrFound)
            {
                ErrMessage.Visible = false;
                return true;
            }
            else
            {
                ErrMessage.Visible = true;
                return false;
            }
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new MenuBar.Pharmacy.frmAddPatient(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
        }

        private void ChildForm_CLosed(object sender, FormClosedEventArgs e)
        {
            LoadPatients();
        }

        private void cmbWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (unitOfWork = new UnitOfWork())
            {
                if (cmbWard.SelectedIndex > 0)
                {
                    long WardId = Convert.ToInt64(cmbWard.SelectedValue);
                    int count = unitOfWork.WardRepository.GetActiveBedsCount(WardId);
                    List<Bed> Beds = unitOfWork.WardRepository.GetAvailableActiveBedsByWardId(WardId);
                    if (count <= 0)
                    {
                        cmbBedNo.SelectedIndex = 0;
                        cmbBedNo.Visible = false;
                        errBeds.Visible = true;
                        return;
                    }
                    cmbBedNo.Visible = true;
                    errBeds.Visible = false;
                    Bed b = new Bed { BedId = 0, RoomNo = "Select Bed" };
                    Beds.Insert(0, b);
                    cmbBedNo.DataSource = Beds;
                    cmbBedNo.ValueMember = "BedId";
                    cmbBedNo.DisplayMember = "RoomNo";
                }
            }
        }

        private void cmbPatient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPatient.SelectedIndex > 0)
            {
                long PatientId = Convert.ToInt64(cmbPatient.SelectedValue);
                using (unitOfWork = new UnitOfWork())
                {
                    if (unitOfWork.AdmitPatientsRepository.IsPatientAlreadyAdmitted(PatientId))
                    {
                        errPatientAdmitted.Visible = true;
                        errPatientAlredyAdmitted = true;
                    }
                    else
                    {
                        errPatientAdmitted.Visible = false;
                        errPatientAlredyAdmitted = false;
                    }
                }
            }
        }

        private void ClearForm()
        {
            this.AdmittedPatientId = 0;
            errPatientAdmitted.Visible = false;
            this.errPatientAlredyAdmitted = false;
            cmbPatient.SelectedIndex = 0;
            cmbWard.SelectedIndex = 0;
            cmbBedNo.SelectedIndex = 0;
            errBeds.Visible = false;
            ErrMessage.Visible = false;
            txtReasonForAdm.Text = "";
            ShowAdmissionNo();
            txtDiagnosis.Text = "";
            txtConsultant.Text = "";
            txtAnesthetist.Text = "";
            lbDoctors.ClearSelected();
            dtpOpDate.Value = DateTime.Now;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ShowAdmissionNo()
        {
            using (unitOfWork = new UnitOfWork())
            {
                numAdmNo.Value = unitOfWork.AdmitPatientsRepository.GetNewAdmissionNoi();
            }
        }
    }
}