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
using Pharmacy.Infrastructure;
using Pharmacy.Core.Entities;

namespace Pharmacy.UI.Indoor.Menu
{
    public partial class frmUpdateBed : Form
    {
        UnitOfWork unitOfWork;
        private long BedId = 0;
        public frmUpdateBed()
        {
            InitializeComponent();
        }

        public frmUpdateBed(long BedId)
        {
            InitializeComponent();
            this.BedId = BedId;
        }


        private void frmUpdateBed_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnUpdateBed, btnClearForm });
            try
            {
                LoadWards();
                LoadBedData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading form data. please try again.", "Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadWards()
        {
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
        }

        private void LoadBedData()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Bed obj = unitOfWork.BedRepository.GetBedWithWardData(this.BedId);
                if (obj != null)
                {
                    cmbWard.SelectedValue = obj.Ward.WardId;
                    txtRoomNo.Text = obj.RoomNo;
                    txtCharges.Text = obj.ChargesPerDay.ToString();
                }
                else
                {
                    MessageBox.Show("Failed to laod bed data for editing, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnUpdateBed.Enabled = false;
                }
            }
        }

        private void txtCharges_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private bool IsValidInput()
        {
            ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            bool ErrFound = false;
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
            if (txtRoomNo.Text.Trim().Equals(""))
            {
                errRoomNo.Visible = true;
                ErrFound = true;
            }
            else
            {
                errRoomNo.Visible = false;
            }

            if (txtCharges.Text.Trim().Equals(""))
            {
                errCharges.Visible = true;
                ErrFound = true;
            }
            else
            {
                errCharges.Visible = false;
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

        private void btnUpdateBed_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                if (this.BedId > 0)
                {
                    UpdateBed();
                }
            }
        }
        private void UpdateBed()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    Bed b = unitOfWork.BedRepository.GetById(this.BedId);
                    if (b != null)
                    {
                        b.Ward = unitOfWork.WardRepository.GetById(cmbWard.SelectedValue);
                        b.RoomNo = txtRoomNo.Text.Trim();
                        b.ChargesPerDay = double.Parse(txtCharges.Text);
                        unitOfWork.BedRepository.Update(b);
                        unitOfWork.Save();
                        MessageBox.Show("Bed detaild updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updated bed details, please try again.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearForm_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void Clear()
        {
            cmbWard.SelectedIndex = 0;
            txtRoomNo.Text = "";
            txtCharges.Text = "";          
        }
    }
}