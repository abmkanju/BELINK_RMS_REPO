using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core;
using Pharmacy.UI.ManageOfflineTask;
using Pharmacy.UI.ManageAuditLog;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmAddPatient : Form
    {
        UnitOfWork unitOfWork;
        private long PatientId = 0;

        public frmAddPatient()
        {
            InitializeComponent();
            LoadFormFeilds();
        }
        private void LoadFormFeilds()
        {

            var Feilds = SharedVariables.PatientFormFeilds;
            if (Feilds != null)
            {
                if (Feilds.Gender > 0)
                {
                    PanelGender.Visible = true;
                    lblGender.Visible = Feilds.Gender == 1 ? true : false;
                }
                else
                {
                    PanelGender.Visible = false;
                }
                if (Feilds.BirthDate > 0)
                {
                    PanelBirthDate.Visible = true;
                    lblBirthDate.Visible = Feilds.BirthDate == 1 ? true : false;
                }
                else
                {
                    PanelBirthDate.Visible = false;
                }
                if (Feilds.Age > 0)
                {
                    PanelAge.Visible = true;
                    lblAge.Visible = Feilds.Age == 1 ? true : false;
                }
                else
                {
                    PanelAge.Visible = false;
                }
                if (Feilds.RegistrationDate > 0)
                {
                    PanelRegistrationDate.Visible = true;
                    lblRegistrationDate.Visible = Feilds.RegistrationDate == 1 ? true : false;
                }
                else
                {
                    PanelRegistrationDate.Visible = false;
                }
                if (Feilds.Address > 0)
                {
                    PanelAddress.Visible = true;
                    lblAddress.Visible = Feilds.Address == 1 ? true : false;
                }
                else
                {
                    PanelAddress.Visible = false;
                }
                if (Feilds.RefferedBy > 0)
                {
                    PanelRefferedBy.Visible = true;
                    lblRefferedBy.Visible = Feilds.RefferedBy == 1 ? true : false;
                }
                else
                {
                    PanelRefferedBy.Visible = false;
                }
                if (Feilds.Tags > 0)
                {
                    PanelTags.Visible = true;
                    lblTags.Visible = Feilds.Tags == 1 ? true : false;
                }
                else
                {
                    PanelTags.Visible = false;
                }
                if (Feilds.BloodGroup > 0)
                {
                    PanelBloodGroup.Visible = true;
                    lblBloodGroup.Visible = Feilds.BloodGroup == 1 ? true : false;
                }
                else
                {
                    PanelBloodGroup.Visible = false;
                }
                if (Feilds.AddPhoto > 0)
                {
                    PanelAddPhoto.Visible = true;
                    lblAddPhoto.Visible = Feilds.AddPhoto == 1 ? true : false;
                }
                else
                {
                    PanelAddPhoto.Visible = false;
                }
            }

        }
        public frmAddPatient(long PatientId)
        {
            InitializeComponent();
            this.PatientId = PatientId;
            LoadFormFeilds();
        }
        private void btnShowSecContacts_Click(object sender, EventArgs e)
        {
            pnlSecondaryContacts.Visible = true;
        }

        private void btnHideSecContacts_Click(object sender, EventArgs e)
        {
            pnlSecondaryContacts.Visible = false;
        }

        private void txtTags_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (isTagAdded())
                {
                    ToolTip tt = new ToolTip();
                    tt.Show("Tag Already Used", txtTags, 800);
                    return;
                }
                GenerateTag(txtTags.Text.Trim().ToUpper());
            }
        }

        private void GenerateTag(string TagText)
        {
            Label lblTag = new Label();
            lblTag.AutoSize = true;
            lblTag.BackColor = System.Drawing.SystemColors.Highlight;
            lblTag.ForeColor = System.Drawing.Color.White;
            lblTag.Location = new System.Drawing.Point(333, 354);
            var margin = lblTag.Margin;
            margin.Top = 5;
            lblTag.Margin = margin;
            lblTag.MinimumSize = new System.Drawing.Size(0, 18);
            lblTag.Name = "lblTag";
            lblTag.Size = new System.Drawing.Size(41, 18);
            lblTag.TabIndex = 20;
            lblTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblTag.Text = TagText;
            lblTag.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblTag.DoubleClick += new EventHandler(lblTag_DoubleClicked);
            pnlTags.Controls.Add(lblTag);
            txtTags.Text = "";
        }
        private bool isTagAdded()
        {
            foreach (Label l in pnlTags.Controls)
            {
                if (l.Text.ToLower() == txtTags.Text.Trim().ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        private void lblTag_DoubleClicked(object sender, EventArgs e)
        {
            Label ClickedLbl = (Label)sender;
            pnlTags.Controls.Remove(ClickedLbl);
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    if (this.PatientId > 0)
                    {
                        UpdatePatient();
                        return;
                    }
                    InsertPatient();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void UpdatePatient()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Patient objPatient = unitOfWork.PatientRepository.GetPatientWithTags_ByPatientId(this.PatientId);
                string LastImagePath = objPatient.ImagePath;
                List<Tag> ExistingTags = objPatient.Tags.ToList();
                foreach (Tag t in ExistingTags)
                {
                    objPatient.Tags.Remove(t);
                }
                if (!fillObject(objPatient))
                    return;
                if (objPatient.IsSynced)
                {
                    objPatient.IsNew = false;
                    objPatient.IsUpdate = true;
                    objPatient.IsSynced = false;
                }
                if (objPatient.ImagePath != null)
                {
                    objPatient.ImagePath = SharedFunctions.CopyFileToLocal(objPatient.ImagePath);
                }

                objPatient.Tags = new List<Tag>();
                Tag objTag = null;
                if (Convert.ToInt64(cmbTags.SelectedValue) > 0)
                {
                    objTag = unitOfWork.TagsRepository.GetById(cmbTags.SelectedValue);
                    objPatient.Tags.Add(objTag);
                }
                else
                {
                    foreach (Label l in pnlTags.Controls)
                    {
                        objTag = unitOfWork.TagsRepository.FindTagByName(l.Text);
                        if (objTag == null)
                        {
                            objTag = new Tag();
                            objTag.TagName = l.Text;
                        }
                        objPatient.Tags.Add(objTag);
                    }
                }
                //SOT
                unitOfWork.PatientRepository.Update(objPatient);
                AuditLogTask.CreateAuditLog(unitOfWork, "MR# " + objPatient.MRNo, AuditModule.Patient, AuditAction.updated);
                OfflineJsonTask.SaveOfflineTask(unitOfWork, objPatient, ModuleName.Patient, ModuleAction.Update, objPatient.PatientId);
            }
            MessageBox.Show("Patient Data Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void InsertPatient()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Patient objPatient = new Patient();
                if (!fillObject(objPatient))
                    return;
                if (objPatient.ImagePath != null)
                {
                    objPatient.ImagePath = SharedFunctions.CopyFileToLocal(objPatient.ImagePath);
                }
                objPatient.Tags = new List<Tag>();
                Tag objTag = null;
                if (Convert.ToInt64(cmbTags.SelectedValue) > 0)
                {
                    objTag = unitOfWork.TagsRepository.GetById(cmbTags.SelectedValue);
                    objPatient.Tags.Add(objTag);
                }
                else
                {
                    foreach (Label l in pnlTags.Controls)
                    {
                        objTag = unitOfWork.TagsRepository.FindTagByName(l.Text);
                        if (objTag == null)
                        {
                            objTag = new Tag();
                            objTag.TagName = l.Text;
                        }
                        objPatient.Tags.Add(objTag);
                    }
                }
                unitOfWork.PatientRepository.Insert(objPatient);
                AuditLogTask.CreateAuditLog(unitOfWork, "MR# " + objPatient.MRNo, AuditModule.Patient, AuditAction.created);
                unitOfWork.Save();
                //SOT
                OfflineJsonTask.SaveOfflineTask(unitOfWork, objPatient, ModuleName.Patient, ModuleAction.Create, objPatient.PatientId);
            }
            MessageBox.Show("Patient Data Saved Sucessfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private bool fillObject(Patient objPatient)
        {
            objPatient.MRNo = decimal.Parse(txtMrNo.Text);
            if (unitOfWork.PatientRepository.CheckMrNoExists(this.PatientId, objPatient.MRNo))
            {
                SharedFunctions.MessageBoxShow("Mr number should be unique.", MessageType.Warning);
                return false;
            }
            objPatient.Name = txtName.Text.Trim();
            objPatient.Email = !string.IsNullOrEmpty(txtEmail.Text) ? txtEmail.Text.Trim() : "";
            objPatient.Phone = txtPhone.Text;
            objPatient.Phone2 = txtPhone2.Text;
            objPatient.Phone3 = txtPhone3.Text;
            objPatient.CountryCode1 = "+92";
            objPatient.CountryCode2 = "+92";
            objPatient.CountryCode3 = "+92";
            if (rbMale.Checked) { objPatient.Gender = "male"; }
            else if (rbFemale.Checked) { objPatient.Gender = "female"; }
            else { objPatient.Gender = "other"; }
            //else { objPatient.Gender = 0; }
            objPatient.DateOfBirth = dtpDOB.Value;
            objPatient.AgeYears = (int)numAgeYears.Value;
            objPatient.AgeMonths = (int)numAgeYears.Value;
            objPatient.AgeDays = (int)numAgeDays.Value;
            objPatient.RegistrationDate = dtpRegistrationDate.Value;
            objPatient.Address = txtAddress.Text.Trim();
            objPatient.ReferredBy = txtRefferedBy.Text.Trim();
            objPatient.ImagePath = lblFilePath.Text.ToLower().Equals("no file choosen") ? null : lblFilePath.Text;
            SharedVariables.PatientStatus patientStatus;
            Enum.TryParse(cmbStatus.GetItemText(cmbStatus.SelectedItem), out patientStatus);
            objPatient.Status = (int)patientStatus;
            objPatient.SMSPreferrence = rbEnglish.Checked == true ? 1 : 2;
            objPatient.IsDeceased = chkDeceased.Checked;
            objPatient.IsActive = true;
            if (this.PatientId <= 0)
            {
                objPatient.IsNew = true;
                objPatient.CreateDate = DateTime.Now;
                objPatient.IsActive = true;
            }
            objPatient.UpdateDate = DateTime.Now;
            objPatient.BloodType = cmbBloodGroup.SelectedIndex;// > 0 ? cmbBloodGroup.SelectedIndex - 1 : -1;
            objPatient.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
            return true;
        }

        private bool IsValidInput()
        {
            int tags = 0;
            bool ErrFound = false;
            if (string.IsNullOrEmpty(txtMrNo.Text))
            {
                SharedFunctions.MessageBoxShow("Mr number is required. ", MessageType.Warning);
                return ErrFound;
            }
            if (!string.IsNullOrEmpty(txtEmail.Text) && !SharedFunctions.ValidationEmail(txtEmail.Text))
            {
                SharedFunctions.MessageBoxShow("Invalid email address.", MessageType.Warning);
                return ErrFound;
            }
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                ErrName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrName.Visible = false;
            }
            if (!txtPhone.MaskCompleted)
            {
                ErrPhone.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrPhone.Visible = false;
            }
            foreach (Label l in pnlTags.Controls)
            {
                tags++; 
            }
            
            if (lblAge.Visible && numAgeYears.Value == 0 && numAgeMonths.Value == 0 && numAgeDays.Value == 0)
            {
                ErrMessage.Visible = true;
                return false;
            }
            if (lblAddress.Visible && string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                ErrMessage.Visible = true;
                return false;
            }
            if (lblRefferedBy.Visible && string.IsNullOrWhiteSpace(txtRefferedBy.Text))
            {
                ErrMessage.Visible = true;
                return false;
            }
            if (lblTags.Visible && tags== 0)
            {
                ErrMessage.Visible = true;
                return false;
            }
            if (lblBloodGroup.Visible && cmbBloodGroup.SelectedIndex == -1)
            {
                ErrMessage.Visible = true;
                return false;
            }
            if (lblAddPhoto.Visible && lblFilePath.Text == "No File Choosen")
            {
                ErrMessage.Visible = true;
                return false;
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


        private void frmAddPatient_Load(object sender, EventArgs e)
        {
            try
            {
                cmbBloodGroup.SelectedIndex = 0;
                if (this.PatientId > 0)
                {
                    btnAddPatient.Text = "Update";
                    LoadPatientData();
                }
                else
                {
                    ShowNewMRNo();
                }
                LoadTags();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void LoadPatientData()
        {
            pnlUpdateCntrols.Visible = true;
            Patient p = new Patient();
            using (unitOfWork = new UnitOfWork())
            {
                p = unitOfWork.PatientRepository.GetPatientWithTags_ByPatientId(this.PatientId);
            }
            txtMrNo.Text = p.MRNo.ToString();
            txtName.Text = p.Name;
            txtEmail.Text = p.Email;
            txtPhone.Text = p.Phone;
            if (!string.IsNullOrEmpty(p.Phone2))
            {
                pnlSecondaryContacts.Visible = true;
                txtPhone2.Text = p.Phone2;
                txtPhone3.Text = p.Phone3;
            }
            if (p.Gender.Equals("male"))
            {
                rbMale.Checked = true;
            }
            else if (p.Gender.Equals("female"))
            {
                rbFemale.Checked = true;
            }
            else if (p.Gender.Equals("other"))
            {
                rbOther.Checked = true;
            }
            if (p.DateOfBirth != null)
                dtpDOB.Value = (DateTime)p.DateOfBirth;
            if (p.RegistrationDate != null)
                dtpRegistrationDate.Value = p.RegistrationDate;
            numAgeYears.Value = p.AgeYears == null ? 0 : (int)p.AgeYears;
            numAgeMonths.Value = p.AgeMonths == null ? 0 : (int)p.AgeMonths;
            numAgeDays.Value = p.AgeDays == null ? 0 : (int)p.AgeDays;
            txtAddress.Text = p.Address;
            txtRefferedBy.Text = p.ReferredBy;
            foreach (Tag t in p.Tags)
            {
                GenerateTag(t.TagName);
            }
            lblFilePath.Text = p.ImagePath == null ? "No File Choosen" : p.ImagePath;
            cmbStatus.SelectedItem = ((SharedVariables.PatientStatus)p.Status).ToString();
            chkDeceased.Checked = p.IsDeceased;
            if (p.SMSPreferrence == 1)
            {
                rbEnglish.Checked = true;
            }
            else
            {
                rbUrdu.Checked = true;
            }
            if (p.BloodType > 0)
            {
                cmbBloodGroup.SelectedIndex = p.BloodType + 1;
            }
            cmbBloodGroup.SelectedIndex = p.BloodType; // 0 => defaults to : blood Type
        }
        private void LoadTags()
        {
            List<Tag> tags = new List<Core.Entities.Tag>();
            using (unitOfWork = new UnitOfWork())
            {
                tags = unitOfWork.TagsRepository.GetAll().ToList();
            }
            Tag DefaultTag = new Tag();
            DefaultTag.TagId = 0;
            DefaultTag.TagName = "Select Tag";
            tags.Insert(0, DefaultTag);
            cmbTags.DataSource = tags;
            cmbTags.ValueMember = "TagId";
            cmbTags.DisplayMember = "TagName";
        }
        private void ShowNewMRNo()
        {
            using (unitOfWork = new UnitOfWork())
            {
                txtMrNo.Text = unitOfWork.PatientRepository.GetNewMrNo().ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ShowNewMRNo();
            btnAddPatient.Text = "Add";
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select Invoice Image";
            fd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.Multiselect = false;
            fd.RestoreDirectory = true;
            DialogResult rs = fd.ShowDialog();
            if (rs == DialogResult.OK)
            {
                string selectedFile = fd.FileName;
                lblFilePath.Text = selectedFile;
            }
        }

        private void cmbTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlTags.Controls.Clear();
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidAlphabaticKey(sender, e);
        }

        private void txtRefferedBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidAlphabaticKey(sender, e);
        }

        private void txtMrNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtTags_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlTags_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnTakePhoto_Click(object sender, EventArgs e)
        {
            new frmCamera(this).ShowDialog();
        }
        
    }
}