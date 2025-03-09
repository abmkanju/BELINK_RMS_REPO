using Essy.Tools.InputBox;
using Microsoft.SqlServer.Management.Common;
using Microsoft.VisualBasic;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.Franchise_Deployement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.MenuBar.UserMenue
{
    public partial class AdminSetting : Form
    {
        UnitOfWork unitOfWork;

        int InvoiceSettingId = 0;
        int ProcedureInvoiceSettingId = 0;
        long PractiseSettingId = 0;
        int PrintFormatSettingId = 0;
        int PharmacySettingId = 0;
        int ShiftMasterSettingId = 0;
        int PatientSettingsId = 0;
        public AdminSetting()
        {
            InitializeComponent();
            
            grdResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            label77.UseMnemonic = false;
            //txtdbname.Text = getdatabasename();
            chkItemDefUnitPOS.Checked = false;
            //rbLifo.Checked = true;
            txtDefaultDatabaseName.Text = Shared.GetConnectionBuilder().InitialCatalog;
            txtDefaultLocation.Text = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + txtDefaultDatabaseName.Text + ".bak"; 
            txtcurrentdbname.Text = Shared.GetConnectionBuilder().InitialCatalog;
            txtRestoreDate.Text = DateTime.Now.ToShortDateString();
            FormatPurchaseReportNote();
            if (SharedVariables.AdminPharmacySetting.DarkMode)
            {
                DarkMode.EnableDarkMode(this);
            }
        }
        private void LoadDefaultBackupSettings()
        {

        }
        private void FormatPurchaseReportNote()
        {
            //txtPurchaseOrderTermsandConditions.SelectionBullet = true;

           


        }
        private void AdminSetting_Load(object sender, EventArgs e)
        {
            LoadDetials();
            tcAdminSetting.TabPages.Remove(tpProcedureInvoiceSetting);// temporary removing procedure invoice as its not required.
            tcAdminSetting.TabPages.Remove(tpPatient);// temporary removing procedure invoice as its not required.
            tcAdminSetting.TabPages.Remove(tpBackup);// temporary removing procedure invoice as its not required.
            //tcAdminSetting.TabPages.Remove(tpPrintSetting);// temporary removed
            LoadAdminInvoiceSetting();
            LoadAdminProcedureInvoiceSetting();
            LoadAdminPractiseSetting();
            LoadAdminPrintFormatSetting();
            LoadAdminPharmacySetting();
            LoadAdminShiftsMasterSetting();
            LoadAdminShiftsSetting();
            LoadFBRAdminDetails();
            chkConsumptionFromMultipleBatches.Checked = false;
            chkItemDefUnitPOS.Checked = false;
            //rbLifo.Checked = true;
            LoadPatientSettings();
            fillbackupdetails();
            //tcAdminSetting.TabPages.Remove(tabPage1);
        }

        private void LoadFBRAdminDetails()
        {
            bool IsAdminUser = false;
            using (unitOfWork = new UnitOfWork())
            {
                IsAdminUser = unitOfWork.UserRoleRepository.CheckAdminUserRole(SharedVariables.LoggedInUser.UserId);
            }
            if (!SharedVariables.AdminPharmacySetting.IsFBRUnable)
            {
                tcAdminSetting.TabPages.Remove(tpFBRCredentials);
                return;
            }
            if(!IsAdminUser)
            {
                tcAdminSetting.TabPages.Remove(tpFBRCredentials);
                return;
            }
            if (SharedVariables.AdminFBRSettings == null)
                return;
            txtPOSID.Text = SharedVariables.AdminFBRSettings.POSIDEnc.ToString();
            txtTocken.Text = SharedVariables.AdminFBRSettings.TockenEnc.ToString();
        }
        private void fillbackupdetails()
        {
            SqlConnectionStringBuilder builder = Shared.GetConnectionBuilder();
            txtDatabase.Text = builder.InitialCatalog;
            txtUsername.Text = builder.UserID;
            txtPassword.Text = builder.Password;
            txtServer.Text = builder.DataSource;
        }
        private void LoadDetials()
        {
            try
            {
                List<KeyValueVM> withholdingTax = new List<KeyValueVM>()
                {
                    new KeyValueVM() {Id = 0, Name = "%"},
                    new KeyValueVM() {Id = 1, Name = "Value"},
                };
                cmbWithholdingTaxType.DataSource = withholdingTax;
                cmbWithholdingTaxType.ValueMember = "Id";
                cmbWithholdingTaxType.DisplayMember = "Name";

                List<KeyValueVM> withAdvanceTax = new List<KeyValueVM>()
                {
                    new KeyValueVM() {Id = 0, Name = "%"},
                    new KeyValueVM() {Id = 1, Name = "Value"},
                };
                cmbAdvanceTaxType.DataSource = withAdvanceTax;
                cmbAdvanceTaxType.ValueMember = "Id";
                cmbAdvanceTaxType.DisplayMember = "Name";

                List<KeyValueVM> POSDiscountType = new List<KeyValueVM>()
                {
                    new KeyValueVM() {Id = 0, Name = "Value"},
                    new KeyValueVM() {Id = 1, Name = "%"},
                };

                cmbPOSDiscount.DataSource = POSDiscountType;
                cmbPOSDiscount.ValueMember = "Id";
                cmbPOSDiscount.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private void LoadAdminInvoiceSetting()
        {
            try
            {
                if (SharedVariables.AdminInvoiceSetting != null)
                {
                    LoadInvoiceSetting(SharedVariables.AdminInvoiceSetting);
                }
                //if(SharedVariables.AdminPractiseSetting!=null)
                //{
                //    LoadPractiseSetting(SharedVariables.AdminPractiseSetting);
                //}
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }
        private void LoadAdminProcedureInvoiceSetting()
        {
            try
            {
                if (SharedVariables.AdminProcedureInvoiceSetting != null)
                {
                    LoadProcedureInvoiceSetting(SharedVariables.AdminProcedureInvoiceSetting);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }
        private void LoadAdminPractiseSetting()
        {
            try
            {
                if (SharedVariables.AdminPractiseSetting != null)
                {
                    LoadPractiseSetting(SharedVariables.AdminPractiseSetting);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }
        private void LoadAdminPrintFormatSetting()
        {
            try
            {
                if (SharedVariables.AdminPrintFormatSetting != null)
                {
                    LoadPrintFormatSetting(SharedVariables.AdminPrintFormatSetting);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }
        private void LoadAdminPharmacySetting()
        {
            try
            {
                if (SharedVariables.AdminPharmacySetting != null)
                {
                    LoadPharmacySetting(SharedVariables.AdminPharmacySetting);
                    LoadBackUpSetting(SharedVariables.AdminPharmacySetting);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }

        private void LoadAdminShiftsMasterSetting()
        {
            try
            {
                if (SharedVariables.AdminShiftSettings != null)
                {
                    LoadShiftMasterSetting(SharedVariables.AdminShiftMasterSetting);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }


        private void LoadAdminShiftsSetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    List<AdminShiftSetting> setting = unitOfWork.AdminShiftsSettingRepository.GetShifts();
                    if (setting != null)
                    {
                        LoadShiftSetting(setting);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }

        private void LoadInvoiceSetting(AdminInvoiceSetting Setting)
        {
            // un register this event here, so at load time checkChanges should not be called, this is showing confirmation dialog which is not required at load time.
            //chkOptionalBatchNo.CheckedChanged -= chkOptionalBatchNo_CheckedChanged;
            InvoiceSettingId = Setting.AdminInvoiceSettingId;
            if (Setting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
            {
                rbA4.Checked = true;
            }
            else if (Setting.PrintPageSize == (int)SharedVariables.PrintPageSize.A8)
            {
                rbA8.Checked = true;
            }
            else if (Setting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
            {
                rbA5.Checked = true;
            }
            numMarginTop.Value = Setting.MarginTop;
            numMarginLeft.Value = Setting.MarginLeft;
            numMarginRight.Value = Setting.MarginRight;
            numMarginBottom.Value = Setting.MarginBottom;
            if (Setting.FontSize == 15) { rbFs15.Checked = true; }
            else if (Setting.FontSize == 13) { rbFs13.Checked = true; }
            else if (Setting.FontSize == 11) { rbFs11.Checked = true; }
            else if (Setting.FontSize == 9) { rbFs9.Checked = true; }
            else if (Setting.FontSize == 7) { rbFs7.Checked = true; }
            if (Setting.PageType == (int)SharedVariables.PageType.PlainPaper) { rbPlainPaper.Checked = true; }
            else if (Setting.PageType == (int)SharedVariables.PageType.LetterHead) { rbLetterHead.Checked = true; }
            if (Setting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait) { rbPortrait.Checked = true; }
            else if (Setting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape) { rbLandscape.Checked = true; }

            if (Setting.ShowUserName) { chkShowUserName.Checked = true; }
            else { chkShowUserName.Checked = false; }

            if (Setting.ShowLogoWaterMark) { chkShowLogoWaterMark.Checked = true; }
            else { chkShowLogoWaterMark.Checked = false; }

            if (Setting.HideRateAndQuantityInPrintFormat) { chkHideRateAndQty.Checked = true; }
            else { chkHideRateAndQty.Checked = false; }

            if (Setting.ShowGrandtotalsInWords) { chkShowGTInWords.Checked = true; }
            else { chkShowGTInWords.Checked = false; }

            if (Setting.HideLineItemDiscount) { chkLineItemDiscount.Checked = true;}
            else { chkLineItemDiscount.Checked = false; }

            if (Setting.GrandTotalsOfInvoiceAsPaymentByDefault) { chkGrndTotalofInvAsPmt.Checked = true; }
            else { chkGrndTotalofInvAsPmt.Checked = false; }

            if (Setting.ShowPOSService) { chkPOSService.Checked = true; }
            else { chkPOSService.Checked = false; }

            if (Setting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts) { chkHideDuesAndAdvance.Checked = true; }
            else { chkHideDuesAndAdvance.Checked = false; }

            chkShoPharmaName.Checked = Setting.PrintPractiseName;
            chkshowPharmaPhone.Checked = Setting.ShowPharmaPhone;
            chkShowPharmaAddress.Checked = Setting.ShowPharmaAddress;

            //chkOptionalBatchNo.Checked = Setting.IsOptionalBatchNo;
            txtInvoiceNote.Text = Setting.InvoiceNote;
            chkAllowBelowCostSale.Checked = Setting.AllowBelowCostSale;
            //chkOptionalBatchNo.CheckedChanged += chkOptionalBatchNo_CheckedChanged;
            chkAskLogin.Checked = Setting.IsAskLoginOnInvSave;
            radPurchaseOderA8.Checked = Setting.IsPurchaseOderA8Format ? true : false;
            txtPrinterName.Text = Setting.PrinterName;
            txtBarcodePrinterName.Text = Setting.BarcodePrinterName;
            //patient details
            chkShowPOSGrandTotal.Checked = Setting.ShowPOSGrandTotal;
            chkShowPOSSubTotal.Checked = Setting.ShowPOSSubTotal;
            chkShowGend.Checked = Setting.ShowGender;
            chkShowPhone.Checked = Setting.ShowPhone;
            chkShowMR.Checked = Setting.ShowMR;
            chkShowEmail.Checked = Setting.ShowEmail;
            chkShowDOB.Checked = Setting.ShowDOB;
            chkShowAge.Checked = Setting.ShowAge;
            chkShowPatientAddress.Checked = Setting.ShowAddress;
            chkShowTPInsteadOfRate.Checked = Setting.ShowTPInsteadOfRate;
            chkMultiplBatchPOS.Checked = Setting.ShowMultipleBatchPOS;
        }
        private void LoadProcedureInvoiceSetting(AdminProcedureInvoiceSetting Setting)
        {
            ProcedureInvoiceSettingId = Setting.AdminProcedureInvoiceSettingId;
            if (Setting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
            {
                rbA4_Proc.Checked = true;
            }
            else if (Setting.PrintPageSize == (int)SharedVariables.PrintPageSize.A8)
            {
                rbA8_Proc.Checked = true;
            }
            else if (Setting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
            {
                rbA5_Proc.Checked = true;
            }
            numMarginTop_Proc.Value = Setting.MarginTop;
            numMarginLeft_Proc.Value = Setting.MarginLeft;
            numMarginRight_Proc.Value = Setting.MarginRight;
            numMarginBottom_Proc.Value = Setting.MarginBottom;
            if (Setting.FontSize == 15) { rbFs15_Proc.Checked = true; }
            else if (Setting.FontSize == 13) { rbFs13_Proc.Checked = true; }
            else if (Setting.FontSize == 11) { rbFs11_Proc.Checked = true; }
            else if (Setting.FontSize == 9) { rbFs9_Proc.Checked = true; }
            else if (Setting.FontSize == 7) { rbFs7_Proc.Checked = true; }
            if (Setting.PageType == (int)SharedVariables.PageType.PlainPaper) { rbPlainPaper_Proc.Checked = true; }
            else if (Setting.PageType == (int)SharedVariables.PageType.LetterHead) { rbLetterHead_Proc.Checked = true; }
            if (Setting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait) { rbPortrait_Proc.Checked = true; }
            else if (Setting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape) { rbLandscape_Proc.Checked = true; }

            if (Setting.ShowUserName) { chkShowUserName_Proc.Checked = true; }
            else { chkShowUserName_Proc.Checked = false; }

            if (Setting.ShowLogoWaterMark) { chkShowLogoWaterMark_Proc.Checked = true; }
            else { chkShowLogoWaterMark_Proc.Checked = false; }

            if (Setting.HideRateAndQuantityInPrintFormat) { chkHideRateAndQty_Proc.Checked = true; }
            else { chkHideRateAndQty_Proc.Checked = false; }

            if (Setting.ShowGrandtotalsInWords) { chkShowGTInWords_Proc.Checked = true; }
            else { chkShowGTInWords_Proc.Checked = false; }

            if (Setting.GrandTotalsOfInvoiceAsPaymentByDefault) { chkGrndTotalofInvAsPmt_Proc.Checked = true; }
            else { chkGrndTotalofInvAsPmt_Proc.Checked = false; }

            if (Setting.HidePaymentsAndDuesFromInvoiceAndPmntReceipts) { chkHideDuesAndAdvance_Proc.Checked = true; }
            else { chkHideDuesAndAdvance_Proc.Checked = false; }
            txtInvoiceNote_Proc.Text = Setting.InvoiceNote;
        }
        private void LoadPractiseSetting(AdminPractiseSetting setting)
        {
            this.PractiseSettingId = setting.AdminPractiseSettingId;
            txtPractiseName.Text = setting.Name;
            txtPractisePhone.Text = setting.Phone;
            txtPractiseAddress.Text = setting.Address;
            if (!string.IsNullOrEmpty(setting.LogoPath))
            {
                try
                {

                    Image img = Image.FromFile(setting.LogoPath);
                    pbLogo.Image = img;
                    lblImagePath.Text = setting.LogoPath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not find attached logo file, it has been moved or removed.", "Logo Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LoadPrintFormatSetting(AdminPrintFormatSetting setting)
        {
            this.PrintFormatSettingId = setting.AdminPrintFormatSettingId;
            if (setting.HeaderFooterPref == 1)
            {
                rbHf1.Checked = true;
            }
            else if (setting.HeaderFooterPref == 2)
            {
                rbHf2.Checked = true;
            }
            else if (setting.HeaderFooterPref == 3)
            {
                rbHf3.Checked = true;
            }
            else if (setting.HeaderFooterPref == 4)
            {
                rbHf4.Checked = true;
            }
            else if (setting.HeaderFooterPref == 5)
            {
                rbHf5.Checked = true;
            }
            else if (setting.HeaderFooterPref == 6)
            {
                rbHf6.Checked = true;
            }

            // patient details selection
            if (setting.PatientDetailsPref == 1)
            {
                rbPd1.Checked = true;
            }
            else if (setting.PatientDetailsPref == 2)
            {
                rbPd2.Checked = true;
            }
            if (setting.PatientDetailsPref == 3)
            {
                rbPd3.Checked = true;
            }
            if (setting.PatientDetailsPref == 4)
            {
                rbPd4.Checked = true;
            }
            if (setting.PatientDetailsPref == 5)
            {
                rbPd5.Checked = true;
            }
            if (setting.PatientDetailsPref == 6)
            {
                rbPd6.Checked = true;
            }
            cmbPrintFormats.SelectedIndex = setting.PrintFormatIndex;
            txtPurchaseOrderTermsandConditions.Text = setting.TermsConditions;
        }
        private void LoadPharmacySetting(AdminPharmacySetting setting)
        {

            this.PharmacySettingId = setting.AdminPharmacySettingId;
            if (setting.IsItemConumptionFifo)
            {
                rbFifo.Checked = true;
            }
            chkAllowNegCons.Checked = setting.AllowNegCons;
            if (!setting.IsItemConumptionFifo)
            {
                rbLifo.Checked = true;
            }
            chkPharmacyTemps.Checked = setting.IsPharmacyTemps;
            chkHoldStockOnInvHold.Checked = setting.IsHolsStockOnInvoiceHold;
            chkUseNewestPrice.Checked = setting.IsUseNewestStockPrice;
            chkItemDefUnitPOS.Checked = setting.IsItemDefUnitOnPOS;
            chkItemDefPackaPOS.Checked = setting.IsItemDefPackOnPOS;
            numExpPeriod.Value = setting.ExpiryPeriod;
            cmbExpPeriodUnit.SelectedIndex = setting.ExpiryPeriodUnit;
            chkShowRackNoInPOS.Checked = setting.ShowRackNoInPOS;
            chkIsStockEditableUnitRetailPrice.Checked = setting.IsStockEditableUnitRetailPrice;
            chkPatientSelectionMandatory.Checked = setting.IsPatientSelectionMandatory;
            chkEnableStores.Checked = setting.IsEnableStores;
            chkIsStockMandatoryFields.Checked = setting.IsStockFieldMandatory;
            chkConsumptionFromMultipleBatches.Checked = setting.IsConsumptionFromMultipleBatches;
            chkBonusQty.Checked = setting.IsBonusQty;
            chkIsItemPrintOnStock.Checked = setting.IsItemPrintOnStock;
            chkSameItemWIthDifferentBatches.Checked = setting.IsSameItemDiffBatches;
            txtWithholdingTax.Text = setting.WithholdingTax.ToString();
            cmbWithholdingTaxType.SelectedIndex = setting.WithholdingTaxType;
            txtAdvanceTax.Text = setting.AdvanceTax.ToString();
            cmbAdvanceTaxType.SelectedIndex = setting.AdvanceTaxType;
            chkPOSUnitCost.Checked = setting.IsPOSUnitCost;
            chkPOSProfitLoss.Checked = setting.IsPOSProfitLoss;
            txtwebEmail.Text = setting.OnlineWebUsername;
            chkaskforpin.Checked = setting.AskForPIN;
            chkshowquery.Checked = setting.devQueryCheck;
            chkLoginbyPINInsteadoffPassword.Checked = setting.LoginbyPINInsteadoffPassword;
            chkNegativeSaleReturn.Checked = setting.NegativeSaleReturn;
            txtwebpassword.Text = setting.OnlineWebPassword;
            chkOpenSaleReturn.Checked = setting.AllowAddingSameItemwithDifferentIvoice;
            txtPOSDiscount.Text = setting.POSDiscount.ToString();
            chkShowStockItems.Checked = setting.ShowStockItems;
            cmbPOSDiscount.SelectedIndex = Convert.ToInt32(setting.POSDiscountType);
            chkDiscountedPrice.Checked = Convert.ToBoolean(setting.DiscountedPrice);
            chkDarkMode.Checked = setting.DarkMode;
            if (!setting.devQueryCheck)
            {
                
                 tabControl1.TabPages.Remove(tabPage6);
                
            }
            else
            {
                tabControl1.TabPages.Add(tabPage6);
            }
            if (setting.AutoUpdateTime == string.Empty)
            {
                cmbAutoUpdate.SelectedIndex = 0;
            }
            else
            {
                cmbAutoUpdate.Text = setting.AutoUpdateTime;
            }
        }
        private void LoadBackUpSetting(AdminPharmacySetting setting)
        {
            this.PharmacySettingId = setting.AdminPharmacySettingId;
            txtDefaultLocation.Text = setting.Lcoation;
            txtBackUpFilename.Text = setting.FileName;
            chkEnableAutoBackup.Checked = setting.EnableAutoBackup;
            chkReplace.Checked = setting.ReplaceBackUp;
            cboDuration.Text = setting.Duration;
            txtLastBackupDate.Text = setting.LastBackupDate;
        }

        private void LoadShiftMasterSetting(AdminShiftMasterSetting setting)
        {
            this.ShiftMasterSettingId = setting.AdminShiftMasterSettingId;
            chkEnableShifts.Checked = setting.ShiftsEnabled;
            chkEnforceLogout.Checked = setting.EnforceLogout;
            
        }

        private void LoadShiftSetting(List<AdminShiftSetting> setting)
        {
            foreach (var s in setting)
            {
                if (s.Code == 1)
                {
                    StTimeMorn.Value = s.StartTime;
                    EnTimeMorn.Value = s.EndTime;
                    chkMorn.Checked = s.IsActive;
                    continue;
                }

                if (s.Code == 2)
                {
                    StTimeAftNoon.Value = s.StartTime;
                    enTimeAftNoon.Value = s.EndTime;
                    chkAftNoon.Checked = s.IsActive;
                }

                if (s.Code == 3)
                {
                    StTimeEve.Value = s.StartTime;
                    enTimeEve.Value = s.EndTime;
                    chkEve.Checked = s.IsActive;
                }


                if (s.Code == 4)
                {
                    StTimeNt.Value = s.StartTime;
                    enTimeNt.Value = s.EndTime;
                    chkNt.Checked = s.IsActive;
                }
            }
        }
        private void AdjustControls(object sender)
        {
            RadioButton SelectedOption = (RadioButton)sender;
            if (SelectedOption == rbA4)
            {
                pnlPageSetting.Visible = true;
                //Point OrgLoc = new Point(8, 208);
                //pnlInvoiceSetting.Location = OrgLoc;
            }
            else if (SelectedOption == rbA8)
            {
                pnlPageSetting.Visible = false;
                int x = pnlPageSetting.Location.X;
                int y = pnlPageSetting.Location.Y;
                //pnlInvoiceSetting.Location = new Point(x, y);
            }
            else if (SelectedOption == rbA5)
            {
                pnlPageSetting.Visible = true;
                //Point OrgLoc = new Point(8, 208);
                //pnlInvoiceSetting.Location = OrgLoc;
            }
        }

        private void rbA8_CheckedChanged(object sender, EventArgs e)
        {
            if (rbA8.Checked)
            {
                AdjustControls(sender);
            }
        }

        private void rbA4_CheckedChanged(object sender, EventArgs e)
        {
            if (rbA4.Checked)
            {
                AdjustControls(sender);
            }
        }

        private void rbA5_CheckedChanged(object sender, EventArgs e)
        {
            if (rbA5.Checked)
            {
                AdjustControls(sender);
            }
        }

        private void SaveSetting()
        {
            try
            {
                AdminInvoiceSetting setting = new AdminInvoiceSetting();
                if (rbA4.Checked)
                {
                    setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A4;
                }
                else if (rbA8.Checked)
                {
                    setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A8;
                }
                else if (rbA5.Checked)
                {
                    setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A5;
                }
                setting.MarginTop = (int)numMarginTop.Value;
                setting.MarginLeft = (int)numMarginLeft.Value;
                setting.MarginRight = (int)numMarginRight.Value;
                setting.MarginBottom = (int)numMarginBottom.Value;
                if (rbFs15.Checked) { setting.FontSize = 15; }
                else if (rbFs13.Checked) { setting.FontSize = 13; }
                else if (rbFs11.Checked) { setting.FontSize = 11; }
                else if (rbFs9.Checked) { setting.FontSize = 9; }
                else if (rbFs7.Checked) { setting.FontSize = 7; }
                if (rbPlainPaper.Checked) { setting.PageType = (int)SharedVariables.PageType.PlainPaper; }
                else if (rbLetterHead.Checked) { setting.PageType = (int)SharedVariables.PageType.LetterHead; }
                if (rbPortrait.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.Portrait; }
                else if (rbLandscape.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.LandScape; }
                setting.ShowUserName = chkShowUserName.Checked;
                setting.ShowLogoWaterMark = chkShowLogoWaterMark.Checked;
                setting.HideRateAndQuantityInPrintFormat = chkHideRateAndQty.Checked;
                setting.ShowGrandtotalsInWords = chkShowGTInWords.Checked;
                setting.GrandTotalsOfInvoiceAsPaymentByDefault = chkGrndTotalofInvAsPmt.Checked;
                setting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts = chkHideDuesAndAdvance.Checked;
                setting.InvoiceNote = txtInvoiceNote.Text.Trim();

                setting.PrintPractiseName = chkShoPharmaName.Checked;
                setting.ShowPharmaPhone = chkshowPharmaPhone.Checked;
                setting.ShowPharmaAddress = chkShowPharmaAddress.Checked;
                if (chkLineItemDiscount.Checked)
                {
                    setting.HideLineItemDiscount = true;
                }
                else
                {
                    setting.HideLineItemDiscount = false;
                }
                

                //setting.IsOptionalBatchNo = chkOptionalBatchNo.Checked;
                setting.AllowBelowCostSale = chkAllowBelowCostSale.Checked;
                setting.IsAskLoginOnInvSave = chkAskLogin.Checked;
                setting.IsPurchaseOderA8Format = radPurchaseOderA8.Checked;
                setting.PrinterName = txtPrinterName.Text;
                setting.BarcodePrinterName = txtBarcodePrinterName.Text;
                setting.ShowPOSGrandTotal = chkShowPOSGrandTotal.Checked;
                setting.ShowPOSSubTotal = chkShowPOSSubTotal.Checked;
                setting.ShowPOSService = chkPOSService.Checked;
                setting.ShowGender = chkShowGend.Checked;
                setting.ShowPhone = chkShowPhone.Checked;
                setting.ShowMR = chkShowMR.Checked;
                setting.ShowEmail = chkShowEmail.Checked;
                setting.ShowDOB = chkShowDOB.Checked;
                setting.ShowAge = chkShowAge.Checked;
                setting.ShowAddress = chkShowPatientAddress.Checked;
                setting.ShowTPInsteadOfRate = chkShowTPInsteadOfRate.Checked;
                setting.ShowMultipleBatchPOS = chkMultiplBatchPOS.Checked;
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.AdminInvoiceSettingRepository.Insert(setting);
                    unitOfWork.Save();
                }
                SharedVariables.AdminInvoiceSetting = setting;
                //MessageBox.Show("Invoice Settings Saved Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Invoice Setting.", ex.Message, "Failed");
            }
        }
        private void AddProcedureInvoiceSetting()
        {
            try
            {
                AdminProcedureInvoiceSetting setting = new AdminProcedureInvoiceSetting();
                if (rbA4_Proc.Checked)
                {
                    setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A4;
                }
                else if (rbA8_Proc.Checked)
                {
                    setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A8;
                }
                else if (rbA5_Proc.Checked)
                {
                    setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A5;
                }
                setting.MarginTop = (int)numMarginTop_Proc.Value;
                setting.MarginLeft = (int)numMarginLeft_Proc.Value;
                setting.MarginRight = (int)numMarginRight_Proc.Value;
                setting.MarginBottom = (int)numMarginBottom_Proc.Value;
                if (rbFs15_Proc.Checked) { setting.FontSize = 15; }
                else if (rbFs13_Proc.Checked) { setting.FontSize = 13; }
                else if (rbFs11_Proc.Checked) { setting.FontSize = 11; }
                else if (rbFs9_Proc.Checked) { setting.FontSize = 9; }
                else if (rbFs7_Proc.Checked) { setting.FontSize = 7; }
                if (rbPlainPaper_Proc.Checked) { setting.PageType = (int)SharedVariables.PageType.PlainPaper; }
                else if (rbLetterHead_Proc.Checked) { setting.PageType = (int)SharedVariables.PageType.LetterHead; }
                if (rbPortrait_Proc.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.Portrait; }
                else if (rbLandscape_Proc.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.LandScape; }
                setting.ShowUserName = chkShowUserName_Proc.Checked;
                setting.ShowLogoWaterMark = chkShowLogoWaterMark_Proc.Checked;
                setting.HideRateAndQuantityInPrintFormat = chkHideRateAndQty_Proc.Checked;
                setting.ShowGrandtotalsInWords = chkShowGTInWords_Proc.Checked;
                setting.GrandTotalsOfInvoiceAsPaymentByDefault = chkGrndTotalofInvAsPmt_Proc.Checked;
                setting.HidePaymentsAndDuesFromInvoiceAndPmntReceipts = chkHideDuesAndAdvance_Proc.Checked;
                setting.PrintPractiseName = chkPrintPractiseName_Proc.Checked;
                setting.InvoiceNote = txtInvoiceNote_Proc.Text.Trim();
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.AdminProcedureInvoiceSettingRepository.Insert(setting);
                    unitOfWork.Save();
                }
                SharedVariables.AdminProcedureInvoiceSetting = setting;
                //MessageBox.Show("Invoice Settings Saved Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Procedure Invoice Setting.", ex.Message, "Failed");
            }
        }

        private void UpdateSetting()
        {
            try
            {
                AdminInvoiceSetting setting = new AdminInvoiceSetting();
                using (unitOfWork = new UnitOfWork())
                {
                    setting = unitOfWork.AdminInvoiceSettingRepository.GetById(InvoiceSettingId);
                    if (rbA4.Checked)
                    {
                        setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A4;
                    }
                    else if (rbA8.Checked)
                    {
                        setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A8;
                    }
                    else if (rbA5.Checked)
                    {
                        setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A5;
                    }
                    setting.MarginTop = (int)numMarginTop.Value;
                    setting.MarginLeft = (int)numMarginLeft.Value;
                    setting.MarginRight = (int)numMarginRight.Value;
                    setting.MarginBottom = (int)numMarginBottom.Value;
                    if (rbFs15.Checked) { setting.FontSize = 15; }
                    else if (rbFs13.Checked) { setting.FontSize = 13; }
                    else if (rbFs11.Checked) { setting.FontSize = 11; }
                    else if (rbFs9.Checked) { setting.FontSize = 9; }
                    else if (rbFs7.Checked) { setting.FontSize = 7; }
                    if (rbPlainPaper.Checked) { setting.PageType = (int)SharedVariables.PageType.PlainPaper; }
                    else if (rbLetterHead.Checked) { setting.PageType = (int)SharedVariables.PageType.LetterHead; }
                    if (rbPortrait.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.Portrait; }
                    else if (rbLandscape.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.LandScape; }
                    setting.ShowUserName = chkShowUserName.Checked;
                    setting.ShowLogoWaterMark = chkShowLogoWaterMark.Checked;
                    setting.HideRateAndQuantityInPrintFormat = chkHideRateAndQty.Checked;
                    setting.ShowGrandtotalsInWords = chkShowGTInWords.Checked;
                    setting.GrandTotalsOfInvoiceAsPaymentByDefault = chkGrndTotalofInvAsPmt.Checked;
                    setting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts = chkHideDuesAndAdvance.Checked;

                    setting.PrintPractiseName = chkShoPharmaName.Checked;
                    setting.ShowPharmaPhone = chkshowPharmaPhone.Checked;
                    setting.ShowPharmaAddress = chkShowPharmaAddress.Checked;
                    if (chkLineItemDiscount.Checked)
                    {
                        setting.HideLineItemDiscount = true;
                    }
                    else
                    {
                        setting.HideLineItemDiscount = false;
                    }

                    setting.InvoiceNote = txtInvoiceNote.Text.Trim();
                    unitOfWork.AdminInvoiceSettingRepository.Update(setting);
                    //setting.IsOptionalBatchNo = chkOptionalBatchNo.Checked;
                    setting.AllowBelowCostSale = chkAllowBelowCostSale.Checked;
                    setting.IsAskLoginOnInvSave = chkAskLogin.Checked;
                    setting.IsPurchaseOderA8Format = radPurchaseOderA8.Checked;
                    setting.PrinterName = txtPrinterName.Text;
                    setting.BarcodePrinterName = txtBarcodePrinterName.Text;
                    setting.ShowPOSGrandTotal = chkShowPOSGrandTotal.Checked;
                    setting.ShowPOSSubTotal = chkShowPOSSubTotal.Checked;
                    setting.ShowPOSService = chkPOSService.Checked;
                    setting.ShowGender = chkShowGend.Checked;
                    setting.ShowPhone = chkShowPhone.Checked;
                    setting.ShowMR = chkShowMR.Checked;
                    setting.ShowEmail = chkShowEmail.Checked;
                    setting.ShowDOB = chkShowDOB.Checked;
                    setting.ShowAge = chkShowAge.Checked;
                    setting.ShowAddress = chkShowPatientAddress.Checked;
                    setting.ShowTPInsteadOfRate = chkShowTPInsteadOfRate.Checked;
                    setting.ShowMultipleBatchPOS = chkMultiplBatchPOS.Checked;
                    unitOfWork.Save();
                    SharedVariables.AdminInvoiceSetting = setting; // update shared value
                    //MessageBox.Show("Invoice Settings Updated Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Invoice Setting.", ex.Message, "Failed");
            }
        }
        private void UpdateProcedureInvoiceSetting()
        {
            try
            {
                AdminProcedureInvoiceSetting setting;
                using (unitOfWork = new UnitOfWork())
                {
                    setting = unitOfWork.AdminProcedureInvoiceSettingRepository.GetById(InvoiceSettingId);
                    if (rbA4_Proc.Checked)
                    {
                        setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A4;
                    }
                    else if (rbA8_Proc.Checked)
                    {
                        setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A8;
                    }
                    else if (rbA5_Proc.Checked)
                    {
                        setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A5;
                    }
                    setting.MarginTop = (int)numMarginTop_Proc.Value;
                    setting.MarginLeft = (int)numMarginLeft_Proc.Value;
                    setting.MarginRight = (int)numMarginRight_Proc.Value;
                    setting.MarginBottom = (int)numMarginBottom_Proc.Value;
                    if (rbFs15_Proc.Checked) { setting.FontSize = 15; }
                    else if (rbFs13_Proc.Checked) { setting.FontSize = 13; }
                    else if (rbFs11_Proc.Checked) { setting.FontSize = 11; }
                    else if (rbFs9_Proc.Checked) { setting.FontSize = 9; }
                    else if (rbFs7_Proc.Checked) { setting.FontSize = 7; }
                    if (rbPlainPaper_Proc.Checked) { setting.PageType = (int)SharedVariables.PageType.PlainPaper; }
                    else if (rbLetterHead_Proc.Checked) { setting.PageType = (int)SharedVariables.PageType.LetterHead; }
                    if (rbPortrait_Proc.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.Portrait; }
                    else if (rbLandscape_Proc.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.LandScape; }
                    setting.ShowUserName = chkShowUserName_Proc.Checked;
                    setting.ShowLogoWaterMark = chkShowLogoWaterMark_Proc.Checked;
                    setting.HideRateAndQuantityInPrintFormat = chkHideRateAndQty_Proc.Checked;
                    setting.ShowGrandtotalsInWords = chkShowGTInWords_Proc.Checked;
                    setting.GrandTotalsOfInvoiceAsPaymentByDefault = chkGrndTotalofInvAsPmt_Proc.Checked;
                    setting.HidePaymentsAndDuesFromInvoiceAndPmntReceipts = chkHideDuesAndAdvance_Proc.Checked;
                    setting.PrintPractiseName = chkPrintPractiseName_Proc.Checked;
                    setting.InvoiceNote = txtInvoiceNote_Proc.Text.Trim();
                    unitOfWork.AdminProcedureInvoiceSettingRepository.Update(setting);
                    unitOfWork.Save();
                    SharedVariables.AdminProcedureInvoiceSetting = setting; // update shared value
                    //MessageBox.Show("Invoice Settings Updated Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Procedure Invoice Setting.", ex.Message, "Failed");
            }
        }

        private void SaveInvoiceSetting()
        {
            if (InvoiceSettingId > 0)
            {
                UpdateSetting();
            }
            else
            {
                SaveSetting();
            }
        }
        private void SaveProcedureInvoiceSetting()
        {
            if (ProcedureInvoiceSettingId > 0)
            {
                UpdateProcedureInvoiceSetting();
            }
            else
            {
                AddProcedureInvoiceSetting();
            }
        }

        private void SavePractiseSetting()
        {
            if (PractiseSettingId > 0)
            {
                UpdatePractiseSetting();
            }
            else
            {
                InsertPractiseSetting();
            }
        }

        private void UpdatePractiseSetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdminPractiseSetting obj = unitOfWork.AdminPractiseSettingRepository.GetById(this.PractiseSettingId);
                    if (obj != null)
                    {
                        obj.Name = txtPractiseName.Text.Trim();
                        obj.Phone = txtPractisePhone.Text;
                        obj.Address = txtPractiseAddress.Text;
                        obj.LogoPath = lblImagePath.Text.Trim().ToLower() == "no file choosen" ? "" : lblImagePath.Text.Trim().ToLower();
                        unitOfWork.AdminPractiseSettingRepository.Update(obj);
                        unitOfWork.Save();
                        SharedVariables.AdminPractiseSetting = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Practise Setting.", ex.Message, "Error");
            }
        }

        private void InsertPractiseSetting()
        {
            try
            {
                AdminPractiseSetting obj = new AdminPractiseSetting();
                obj.Name = txtPractiseName.Text.Trim();
                obj.Phone = txtPractisePhone.Text.Trim();
                obj.Address = txtPractiseAddress.Text.Trim();

                obj.LogoPath = lblImagePath.Text.Trim().ToLower() == "no file choosen" ? "" : lblImagePath.Text.Trim().ToLower();
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.AdminPractiseSettingRepository.Insert(obj);
                    unitOfWork.Save();
                    SharedVariables.AdminPractiseSetting = obj;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Practise Setting.", ex.Message, "Error");
            }
        }

        private void UpdatePrintformatSetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdminPrintFormatSetting obj = unitOfWork.AdminPrintFormatSettingRepository.GetById(this.PrintFormatSettingId);
                    if (obj != null)
                    {
                        if (rbHf1.Checked)
                        {
                            obj.HeaderFooterPref = 1;
                        }
                        else if (rbHf2.Checked)
                        {
                            obj.HeaderFooterPref = 2;
                        }
                        else if (rbHf3.Checked)
                        {
                            obj.HeaderFooterPref = 3;
                        }
                        else if (rbHf4.Checked)
                        {
                            obj.HeaderFooterPref = 4;
                        }
                        if (rbHf5.Checked)
                        {
                            obj.HeaderFooterPref = 5;
                        }
                        if (rbHf6.Checked)
                        {
                            obj.HeaderFooterPref = 6;
                        }
                        // patient details setting
                        if (rbPd1.Checked)
                        {
                            obj.PatientDetailsPref = 1;
                        }
                        else if (rbPd2.Checked)
                        {
                            obj.PatientDetailsPref = 2;
                        }
                        else if (rbPd3.Checked)
                        {
                            obj.PatientDetailsPref = 3;
                        }
                        else if (rbPd4.Checked)
                        {
                            obj.PatientDetailsPref = 4;
                        }
                        else if (rbPd5.Checked)
                        {
                            obj.PatientDetailsPref = 5;
                        }
                        else if (rbPd6.Checked)
                        {
                            obj.PatientDetailsPref = 6;
                        }
                        obj.TermsConditions = txtPurchaseOrderTermsandConditions.Text;
                        obj.PrintFormatIndex = radioButton1.Checked ? 0 : 1;
                        using (unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.AdminPrintFormatSettingRepository.Update(obj);
                            unitOfWork.Save();
                            SharedVariables.AdminPrintFormatSetting = obj;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Print Format Setting.", ex.Message, "Error");
            }
        }
        private void UpdatePharmacySetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdminPharmacySetting obj = unitOfWork.AdminPharmacySettingRepository.GetById(this.PharmacySettingId);
                    if (obj != null)
                    {
                        if (obj.EnableAutoBackup != chkEnableAutoBackup.Checked)
                        {
                            Properties.Settings.Default.Date = null;
                        }
                        obj.IsItemConumptionFifo = rbFifo.Checked ? true : false;
                        obj.IsPharmacyTemps = chkPharmacyTemps.Checked;
                        obj.AllowNegCons = chkAllowNegCons.Checked;
                        obj.IsHolsStockOnInvoiceHold = chkHoldStockOnInvHold.Checked;
                        obj.IsUseNewestStockPrice = chkUseNewestPrice.Checked;
                        obj.IsItemDefUnitOnPOS = chkItemDefUnitPOS.Checked;
                        obj.IsItemDefPackOnPOS = chkItemDefPackaPOS.Checked;
                        obj.ExpiryPeriod = (int)numExpPeriod.Value;
                        obj.AskForPIN = chkaskforpin.Checked;
                        obj.ExpiryPeriodUnit = (int)((SharedVariables.ExpiryPeriodUnit)cmbExpPeriodUnit.SelectedIndex); // could have been assigned simply index from dropdown, but for sake of understading applied cast to relevant Enum type;
                        obj.ShowRackNoInPOS = chkShowRackNoInPOS.Checked;
                        obj.IsStockEditableUnitRetailPrice = chkIsStockEditableUnitRetailPrice.Checked;
                        obj.IsPatientSelectionMandatory = chkPatientSelectionMandatory.Checked;
                        obj.IsEnableStores = chkEnableStores.Checked;
                        obj.IsStockFieldMandatory = chkIsStockMandatoryFields.Checked;
                        obj.IsConsumptionFromMultipleBatches = chkConsumptionFromMultipleBatches.Checked;
                        obj.IsBonusQty = chkBonusQty.Checked;
                        obj.IsItemPrintOnStock = chkIsItemPrintOnStock.Checked;
                        obj.IsSameItemDiffBatches = chkSameItemWIthDifferentBatches.Checked;
                        obj.WithholdingTax = SharedFunctions.StringToDouble(txtWithholdingTax.Text);
                        obj.WithholdingTaxType = cmbWithholdingTaxType.SelectedIndex;
                        obj.AdvanceTax = SharedFunctions.StringToDouble(txtAdvanceTax.Text);
                        obj.AdvanceTaxType = cmbAdvanceTaxType.SelectedIndex;
                        obj.POSDiscount = SharedFunctions.StringToDouble(txtPOSDiscount.Text);
                        obj.POSDiscountType = cmbPOSDiscount.SelectedIndex;
                        obj.IsPOSProfitLoss = chkPOSProfitLoss.Checked;
                        obj.IsPOSUnitCost = chkPOSUnitCost.Checked;
                        obj.IsPOSProfitLoss = chkPOSProfitLoss.Checked;
                        obj.AllowAddingSameItemwithDifferentIvoice = chkOpenSaleReturn.Checked;
                        obj.OnlineWebUsername = txtwebEmail.Text;
                        obj.OnlineWebPassword = txtwebpassword.Text;
                        obj.DiscountedPrice = chkDiscountedPrice.Checked;
                        obj.AutoUpdateTime = cmbAutoUpdate.Text != null ? cmbAutoUpdate.Text : "Never";
                        obj.LastBackupDate = txtLastBackupDate.Text;
                        obj.Lcoation = txtDefaultLocation.Text;
                        obj.ReplaceBackUp = chkReplace.Checked;
                        obj.devQueryCheck = chkshowquery.Checked;
                        obj.FileName = txtBackUpFilename.Text;
                        obj.Duration = cboDuration.Text;
                        obj.LoginbyPINInsteadoffPassword = chkLoginbyPINInsteadoffPassword.Checked;
                        obj.NegativeSaleReturn = chkNegativeSaleReturn.Checked;
                        obj.EnableAutoBackup = chkEnableAutoBackup.Checked;
                        obj.ShowStockItems = chkShowStockItems.Checked;
                        obj.DarkMode = chkDarkMode.Checked;
                        
                        using (unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.AdminPharmacySettingRepository.Update(obj);
                            unitOfWork.Save();
                            SharedVariables.AdminPharmacySetting = obj;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Pharmacy Setting.", ex.Message, "Error");
            }
        }
        private void InsertPharmacySetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdminPharmacySetting obj = new AdminPharmacySetting();
                    
                    Properties.Settings.Default.Date = null;
                    
                    obj.IsItemConumptionFifo = rbFifo.Checked ? true : false;
                    obj.IsPharmacyTemps = chkPharmacyTemps.Checked;
                    obj.AllowNegCons = chkAllowNegCons.Checked;
                    obj.IsHolsStockOnInvoiceHold = chkHoldStockOnInvHold.Checked;
                    obj.IsUseNewestStockPrice = chkUseNewestPrice.Checked;
                    obj.IsItemDefUnitOnPOS = chkItemDefUnitPOS.Checked;
                    obj.IsItemDefPackOnPOS = chkItemDefPackaPOS.Checked;
                    obj.ExpiryPeriod = (int)numExpPeriod.Value;
                    obj.ExpiryPeriodUnit = (int)((SharedVariables.ExpiryPeriodUnit)cmbExpPeriodUnit.SelectedIndex); // could have been assigned simply index from dropdown, but for sake of understading applied cast to relevant Enum type;
                    obj.ShowRackNoInPOS = chkShowRackNoInPOS.Checked;
                    obj.IsStockEditableUnitRetailPrice = chkIsStockEditableUnitRetailPrice.Checked;
                    obj.IsPatientSelectionMandatory = chkPatientSelectionMandatory.Checked;
                    obj.IsEnableStores = chkEnableStores.Checked;
                    obj.IsStockFieldMandatory = chkIsStockMandatoryFields.Checked;
                    obj.IsConsumptionFromMultipleBatches = chkConsumptionFromMultipleBatches.Checked;
                    obj.IsBonusQty = chkBonusQty.Checked;
                    obj.IsItemPrintOnStock = chkIsItemPrintOnStock.Checked;
                    obj.IsSameItemDiffBatches = chkSameItemWIthDifferentBatches.Checked;
                    obj.WithholdingTax = SharedFunctions.StringToDouble(txtWithholdingTax.Text);
                    obj.WithholdingTaxType = cmbWithholdingTaxType.SelectedIndex;
                    obj.AdvanceTax = SharedFunctions.StringToDouble(txtAdvanceTax.Text);
                    obj.AdvanceTaxType = cmbAdvanceTaxType.SelectedIndex;
                    obj.POSDiscount = SharedFunctions.StringToDouble(txtPOSDiscount.Text);
                    obj.POSDiscountType = cmbPOSDiscount.SelectedIndex;
                    obj.IsPOSUnitCost = chkPOSUnitCost.Checked;
                    obj.IsPOSProfitLoss = chkPOSProfitLoss.Checked;
                    obj.DiscountedPrice = chkDiscountedPrice.Checked;
                    obj.LastBackupDate = txtLastBackupDate.Text;
                    obj.Lcoation = txtDefaultLocation.Text;
                    obj.ReplaceBackUp = chkReplace.Checked;
                    obj.FileName = txtBackUpFilename.Text;
                    obj.Duration = cboDuration.Text;
                    obj.devQueryCheck = chkshowquery.Checked;
                    obj.OnlineWebUsername = txtwebEmail.Text;
                    obj.OnlineWebPassword = txtwebpassword.Text;
                    obj.NegativeSaleReturn = chkNegativeSaleReturn.Checked;
                    obj.AllowAddingSameItemwithDifferentIvoice = chkOpenSaleReturn.Checked;
                    obj.AskForPIN = chkaskforpin.Checked;
                    obj.AutoUpdateTime = cmbAutoUpdate.Text != null ?cmbAutoUpdate.Text : "Never";
                    obj.EnableAutoBackup = chkEnableAutoBackup.Checked;
                    obj.LoginbyPINInsteadoffPassword = chkLoginbyPINInsteadoffPassword.Checked;
                    obj.ShowStockItems = chkShowStockItems.Checked;
                    obj.DarkMode = chkDarkMode.Checked;
                    using (unitOfWork = new UnitOfWork())
                    {
                        unitOfWork.AdminPharmacySettingRepository.Insert(obj);
                        unitOfWork.Save();
                        SharedVariables.AdminPharmacySetting = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Pharmacy Setting.", ex.Message, "Error");
            }
        }
        private void InsertPrintFormatSetting()
        {
            try
            {
                AdminPrintFormatSetting obj = new AdminPrintFormatSetting();
                if (rbHf1.Checked)
                {
                    obj.HeaderFooterPref = 1;
                }
                else if (rbHf2.Checked)
                {
                    obj.HeaderFooterPref = 2;
                }
                else if (rbHf3.Checked)
                {
                    obj.HeaderFooterPref = 3;
                }
                else if (rbHf4.Checked)
                {
                    obj.HeaderFooterPref = 4;
                }
                if (rbHf5.Checked)
                {
                    obj.HeaderFooterPref = 5;
                }
                if (rbHf6.Checked)
                {
                    obj.HeaderFooterPref = 6;
                }
                // patient details setting
                if (rbPd1.Checked)
                {
                    obj.PatientDetailsPref = 1;
                }
                else if (rbPd2.Checked)
                {
                    obj.PatientDetailsPref = 2;
                }
                else if (rbPd3.Checked)
                {
                    obj.PatientDetailsPref = 3;
                }
                else if (rbPd4.Checked)
                {
                    obj.PatientDetailsPref = 4;
                }
                else if (rbPd5.Checked)
                {
                    obj.PatientDetailsPref = 5;
                }
                else if (rbPd6.Checked)
                {
                    obj.PatientDetailsPref = 6;
                }
                else if (chkLineItemDiscount.Checked)
                {
                    obj.PatientDetailsPref = 6;
                }
                obj.PrintFormatIndex = radioButton1.Checked ? 0 : 1;
                obj.TermsConditions = txtPurchaseOrderTermsandConditions.Text;
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.AdminPrintFormatSettingRepository.Insert(obj);
                    unitOfWork.Save();
                    SharedVariables.AdminPrintFormatSetting = obj;
                }

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Print Format Setting.", ex.Message, "Error");
            }
        }

        private void InsertShiftMasterSetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdminShiftMasterSetting obj = new AdminShiftMasterSetting();

                    obj.ShiftsEnabled = chkEnableShifts.Checked;
                    obj.EnforceLogout = chkEnforceLogout.Checked;

                    using (unitOfWork = new UnitOfWork())
                    {
                        unitOfWork.AdminShiftMasterSettingRepository.Insert(obj);
                        unitOfWork.Save();
                        SharedVariables.AdminShiftMasterSetting = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Pharmacy Setting.", ex.Message, "Error");
            }
        }
        private void UpdateShiftMasterSetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdminShiftMasterSetting obj = unitOfWork.AdminShiftMasterSettingRepository.GetById(this.ShiftMasterSettingId);
                    if (obj != null)
                    {
                        obj.ShiftsEnabled = chkEnableShifts.Checked;
                        obj.EnforceLogout = chkEnforceLogout.Checked;
                        using (unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.AdminShiftMasterSettingRepository.Update(obj);
                            unitOfWork.Save();
                            SharedVariables.AdminShiftMasterSetting = obj;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Shifts Setting.", ex.Message, "Error");
            }
        }


        private void UpdateShiftsSetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    List<AdminShiftSetting> obj = unitOfWork.AdminShiftsSettingRepository.GetAll().ToList();
                    if (obj != null)
                    {
                        using (unitOfWork = new UnitOfWork())
                        {
                            foreach (var s in obj)
                            {
                                if (s.Code == 1)
                                {
                                    s.StartTime = StTimeMorn.Value;
                                    s.EndTime = EnTimeMorn.Value;
                                    s.IsActive = chkMorn.Checked;
                                }

                                if (s.Code == 2)
                                {
                                    s.StartTime = StTimeAftNoon.Value;
                                    s.EndTime = enTimeAftNoon.Value;
                                    s.IsActive = chkAftNoon.Checked;
                                }

                                if (s.Code == 3)
                                {
                                    s.StartTime = StTimeEve.Value;
                                    s.EndTime = enTimeEve.Value;
                                    s.IsActive = chkEve.Checked;
                                }

                                if (s.Code == 4)
                                {
                                    s.StartTime = StTimeNt.Value;
                                    s.EndTime = enTimeNt.Value;
                                    s.IsActive = chkNt.Checked;
                                }
                                unitOfWork.AdminShiftsSettingRepository.Update(s);
                            }

                            unitOfWork.Save();
                            SharedVariables.AdminShiftSettings = unitOfWork.AdminShiftsSettingRepository.GetAll().ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Shifts Setting.", ex.Message, "Error");
            }
        }
        private void LoadPatientSettings()
        {
            PatientFormFeilds settings = new PatientFormFeilds();
            using (unitOfWork = new UnitOfWork())
            {
                settings = unitOfWork.AdminPatientSettings.GetById(1);
            }
            if (settings!= null)
            {
                fillPatientSettings(settings);
            } 
        }
        private void fillPatientSettings(PatientFormFeilds settings)
        {
            
            if (settings.MR > 0)
            {
                chkMR.Checked = true;
                chkRequiredMR.Checked = settings.MR == 1 ? true : false;
            }
            else
            {
                chkMR.Checked = false;

            }
            if (settings.Name > 0)
            {
                chkName.Checked = true;
                chkRequiredName.Checked = settings.Name == 1 ? true : false;
            }
            else
            {
                chkName.Checked = false;
            }
            if (settings.Phone > 0)
            {
                chkPhone.Checked = true;
                chkRequiredPhone.Checked = settings.Phone == 1 ? true : false;
            }
            else
            {
                chkPhone.Checked = false;
            }
            if (settings.RefferedBy > 0)
            {
                chkRefferedBy.Checked = true;
                chkRequiredRefferedBy.Checked = settings.RefferedBy == 1 ? true : false;
            }
            else
            {
                chkRefferedBy.Checked = false;
            }
            if (settings.RegistrationDate > 0)
            {
                chkRegistrationDate.Checked = true;
                chkRequiredRegistrationDatae.Checked = settings.RegistrationDate==1 ? true : false;
            }
            else
            {
                chkRegistrationDate.Checked = false;
            }
            if (settings.Tags > 0)
            {
                chkTags.Checked = true;
                chkRequiredTags.Checked = settings.Tags == 1 ? true : false;
            }
            else
            {
                chkTags.Checked = false;
            }
            if (settings.Gender > 0)
            {
                chkGender.Checked = true;
                chkRequiredGender.Checked = settings.Gender == 1 ? true : false;
            }
            else
            {
                chkGender.Checked = false;
            }
            if (settings.Email> 0)
            {
                chkEmail.Checked = true;
                chkRequiredEmail.Checked = settings.Email == 1 ? true : false;
            }
            else
            {
                chkEmail.Checked = false;

            }
            if (settings.BloodGroup > 0)
            {
                chkBloodGroup.Checked = true;
                chkRequiredBloodGroup.Checked = settings.BloodGroup == 1 ? true : false;
            }
            else
            {
                chkBloodGroup.Checked = false;
            }
            if (settings.BirthDate > 0)
            {
                chkBirthDate.Checked = true;
                chkRequiredBirthDate.Checked = settings.BirthDate == 1 ? true : false;
            }
            else
            {
                chkBirthDate.Checked = false;
            }
            if (settings.Age > 0)
            {
                chkAge.Checked = true;
                chkRequiredAge.Checked = settings.Age == 1 ? true : false;
            }
            else
            {
                chkAge.Checked = false;
            }
            if (settings.Address > 0)
            {
                chkAddress.Checked = true;
                chkRequiredAddress.Checked = settings.Address == 1 ? true : false;
            }
            else
            {
                chkAddress.Checked = false;
            }
            if (settings.AddPhoto > 0)
            {
                chkAddPhoto.Checked = true;
                chkRequiredAddPhoto.Checked = settings.AddPhoto == 1 ? true : false;
            }
            else
            {
                chkAddPhoto.Checked = false;
            }
            this.PatientSettingsId = settings.Id;

        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                
                SavePractiseSetting();
                SaveInvoiceSetting();
                SaveProcedureInvoiceSetting();
                SavePrintFormatSetting();
                SavePharmacySetting();
                SaveShiftMasterSetting();
                SaveShiftsSetting();
                SavePatientSettings();
                if (!chkshowquery.Checked)
                {
                    tabControl1.TabPages.Remove(tabPage6);
                }
                else
                {
                    tabControl1.TabPages.Add(tabPage6);
                }
                MessageBox.Show("Practice setting updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                if (restart)
                {
                    Application.Restart();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An error occurred while updating practise setting.", ex.Message, "Error");
            }
        }
        private void SavePatientSettings()
        {
            if (this.PatientSettingsId > 0)
            {
                UpdatePatientSettings();
            }
            else
            {
                InsertPatientSettings();
            }
        }
        private void UpdatePatientSettings()
        {
            try
            {
                PatientFormFeilds obj = new PatientFormFeilds();
                obj.Id = this.PatientSettingsId;
                if (chkMR.Checked)
                {
                    obj.MR = chkRequiredMR.Checked ? 1 : 2;
                }
                else
                {
                    obj.MR = 0;
                }
                if (chkName.Checked)
                {
                    obj.Name = chkRequiredName.Checked ? 1 : 2;
                }
                else
                {
                    obj.Name = 0;
                }
                if (chkEmail.Checked)
                {
                    obj.Email = chkRequiredEmail.Checked ? 1 : 2;
                }
                else
                {
                    obj.Email = 0;
                }
                if (chkPhone.Checked)
                {
                    obj.Phone = chkRequiredPhone.Checked ? 1 : 2;
                }
                else
                {
                    obj.Phone = 0;
                }
                if (chkGender.Checked)
                {
                    obj.Gender = chkRequiredGender.Checked ? 1 : 2;
                }
                else
                {
                    obj.Gender = 0;
                }
                if (chkBirthDate.Checked)
                {
                    obj.BirthDate = chkRequiredBirthDate.Checked ? 1 : 2;
                }
                else
                {
                    obj.BirthDate = 0;
                }
 
                if (chkAge.Checked)
                {
                    obj.Age = chkRequiredAge.Checked ? 1 : 2;
                }
                else
                {
                    obj.Age = 0;
                }
                if (chkRegistrationDate.Checked)
                {
                    obj.RegistrationDate = chkRequiredRegistrationDatae.Checked ? 1 : 2;
                }
                else
                {
                    obj.RegistrationDate = 0;
                }
                if (chkAddress.Checked)
                {
                    obj.Address = chkRequiredAddress.Checked ? 1 : 2;
                }
                else
                {
                    obj.Address = 0;
                }
                if (chkRefferedBy.Checked)
                {
                    obj.RefferedBy = chkRequiredRefferedBy.Checked ? 1 : 2;
                }
                else
                {
                    obj.RefferedBy = 0;
                }
                if (chkTags.Checked)
                {
                    obj.Tags = chkRequiredTags.Checked ? 1 : 2;
                }
                else
                {
                    obj.Tags = 0;
                }
                if (chkBloodGroup.Checked)
                {
                    obj.BloodGroup = chkRequiredBloodGroup.Checked ? 1 : 2;
                }
                else
                {
                    obj.BloodGroup = 0;
                }
                if (chkAddPhoto.Checked)
                {
                    obj.AddPhoto = chkRequiredAddPhoto.Checked ? 1 : 2;
                }
                else
                {
                    obj.AddPhoto = 0;
                }
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.AdminPatientSettings.Update(obj);
                    unitOfWork.Save();
                    SharedVariables.PatientFormFeilds = obj;
                    
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Print Format Setting.", ex.Message, "Error");
            }
        }
        private void InsertPatientSettings()
        {
            try
            {
                PatientFormFeilds obj = new PatientFormFeilds();
                obj.Id = 1;
                if (chkMR.Checked)
                {
                    obj.MR = chkRequiredMR.Checked ? 1 : 2;
                }
                else
                {
                    obj.MR = 0;
                }
                if (chkName.Checked)
                {
                    obj.Name = chkRequiredName.Checked ? 1 : 2;
                }
                else
                {
                    obj.Name = 0;
                }
                if (chkEmail.Checked)
                {
                    obj.Email = chkRequiredEmail.Checked ? 1 : 2;
                }
                else
                {
                    obj.Email = 0;
                }
                if (chkPhone.Checked)
                {
                    obj.Phone = chkRequiredPhone.Checked ? 1 : 2;
                }
                else
                {
                    obj.Phone = 0;
                }
                if (chkGender.Checked)
                {
                    obj.Gender = chkRequiredGender.Checked ? 1 : 2;
                }
                else
                {
                    obj.Gender = 0;
                }
                if (chkBirthDate.Checked)
                {
                    obj.BirthDate = chkRequiredBirthDate.Checked ? 1 : 2;
                }
                else
                {
                    obj.BirthDate = 0;
                }

                if (chkAge.Checked)
                {
                    obj.Age = chkRequiredAge.Checked ? 1 : 2;
                }
                else
                {
                    obj.Age = 0;
                }
                if (chkRegistrationDate.Checked)
                {
                    obj.RegistrationDate = chkRequiredRegistrationDatae.Checked ? 1 : 2;
                }
                else
                {
                    obj.RegistrationDate = 0;
                }
                if (chkAddress.Checked)
                {
                    obj.Address = chkRequiredAddress.Checked ? 1 : 2;
                }
                else
                {
                    obj.Address = 0;
                }
                if (chkRefferedBy.Checked)
                {
                    obj.RefferedBy = chkRequiredRefferedBy.Checked ? 1 : 2;
                }
                else
                {
                    obj.RefferedBy = 0;
                }
                if (chkTags.Checked)
                {
                    obj.Tags = chkRequiredTags.Checked ? 1 : 2;
                }
                else
                {
                    obj.Tags = 0;
                }
                if (chkBloodGroup.Checked)
                {
                    obj.BloodGroup = chkRequiredBloodGroup.Checked ? 1 : 2;
                }
                else
                {
                    obj.BloodGroup = 0;
                }
                if (chkAddPhoto.Checked)
                {
                    obj.AddPhoto = chkRequiredAddPhoto.Checked ? 1 : 2;
                }
                else
                {
                    obj.AddPhoto = 0;
                }
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.AdminPatientSettings.Insert(obj);
                    
                    unitOfWork.Save();
                    SharedVariables.PatientFormFeilds = obj;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Print Format Setting.", ex.Message, "Error");
            }
        }
        private void SavePrintFormatSetting()
        {
            if (this.PrintFormatSettingId > 0)
            {
                UpdatePrintformatSetting();
            }
            else
            {
                InsertPrintFormatSetting();
            }
        }
        private void savedayofweek()
        {
            if (cmbAutoUpdate.Text != "Weekly" && Properties.Settings.Default.Day != null)
            {
                
            }
            else
            {
                Properties.Settings.Default.Day = ((int)DateTime.Now.DayOfWeek).ToString();
               
            }
        }
        private void SavePharmacySetting()
        {
            savedayofweek();
            if (this.PharmacySettingId > 0)
            {
                UpdatePharmacySetting();
            }
            else
            {
                InsertPharmacySetting();
            }
        }

        private void SaveShiftMasterSetting()
        {
            if (this.ShiftMasterSettingId > 0)
            {
                UpdateShiftMasterSetting();
            }
            else
            {
                InsertShiftMasterSetting();
            }
        }

        private void SaveShiftsSetting()
        {
            UpdateShiftsSetting();
        }

        private void lblRemoveFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblImagePath.Text = "No File Choosen";
        }

        private void btnAttachImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select Invoice Image";
            fd.Filter = "Image Files|*.jpg;*.jpeg;";
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.Multiselect = false;
            fd.RestoreDirectory = true;
            DialogResult rs = fd.ShowDialog();
            if (rs == DialogResult.OK)
            {
                string selectedFile = fd.FileName;
                pbLogo.Image = Image.FromFile(selectedFile);
                string DestPath = SharedFunctions.CopyFileToLocal(selectedFile);
                lblImagePath.Text = DestPath;
            }
        }

        private void rbHf_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                foreach (Control other in pnlHeadersFooters.Controls)
                {
                    if (other is Panel)
                    {
                        foreach (Control ctr in other.Controls)
                        {
                            if (ctr.GetType() == typeof(RadioButton))
                            {
                                if (ctr == rb)
                                {
                                    continue;
                                }
                                ((RadioButton)ctr).Checked = false;
                            }
                        }
                    }
                }
            }
        }

        private void rbPd_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                foreach (Control other in pnlPatientDetails.Controls)
                {
                    if (other is Panel)
                    {
                        foreach (Control ctr in other.Controls)
                        {
                            if (ctr.GetType() == typeof(RadioButton))
                            {
                                if (ctr == rb)
                                {
                                    continue;
                                }
                                ((RadioButton)ctr).Checked = false;
                            }
                        }
                    }
                }
            }
        }

        private void chkOptionalBatchNo_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkOptionalBatchNo.Checked)
            //{
            //    DialogResult res = MessageBox.Show("If any stock item has multiple batches, then on POS screen available quantity will be picked depending on FIFO/LIFO consumption setting. All other batches and there quantity will be hidden. you can revert this setting any time to show all batches and their stock on POS screen.", "Critical Alert", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            //    if (res != System.Windows.Forms.DialogResult.Yes)
            //    {
            //        chkOptionalBatchNo.Checked = false;
            //    }
            //}
        }

        private void chkEnableShifts_CheckedChanged(object sender, EventArgs e)
        {
            pnlShifts.Visible = chkEnableShifts.Checked;
        }

        private void chkEnforceLogout_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkEnableShifts.Checked)
            {
                chkEnforceLogout.Checked = false;
            }
        }

        private void btnRemoveDefaultItems_Click(object sender, EventArgs e)
        {
            removeDefaultItems();
        }
        private void removeDefaultItems()
        {
            // remove default items if none of the followings is done
            // stock added
            // consumptions done OR invoice added(both basically add consumption)
            // adjustment added
            bool canDeleteDefaults = true;
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.StockRepository.anyActiveStockExists())
                {
                    canDeleteDefaults = false;
                }
                if (unitOfWork.StockConsumptionRepository.AnyActiveConsExists())
                {
                    canDeleteDefaults = false;
                }
                if (unitOfWork.AdjustmentRepository.AnyActiveAdjExists())
                {
                    canDeleteDefaults = false;
                }
                if (canDeleteDefaults)
                {
                    if (unitOfWork.GeneralRepository.ClearDefaultData())
                    {
                        try
                        {
                            AdminPractiseSetting setting = unitOfWork.AdminPractiseSettingRepository.getAdminPracStting();
                            setting.UserDeletedDefaultData = true;
                            unitOfWork.AdminPractiseSettingRepository.Update(setting);
                            unitOfWork.Save();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to update default items deletion status, item will get loaded again, please contact healthwire if this issue persists.", "Status Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        MessageBox.Show("Default data cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Restart();
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while default data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Default data can't be cleared, You have performed some transactions.", "Operation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ValidateDouble_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void btnUpdateFBR_Click(object sender, EventArgs e)
        {
            SaveFBRCredentials();
        }

        private void SaveFBRCredentials()
        {
            try
            {
                if (string.IsNullOrEmpty(txtPOSID.Text.Trim()) || string.IsNullOrEmpty(txtTocken.Text.Trim()))
                {
                    SharedFunctions.MessageBoxShow("Each field is required", MessageType.Warning);
                    return;
                }
                using (unitOfWork = new UnitOfWork())
                {
                    AdminFBRSetting obj = unitOfWork.AdminFBRSettingRepository.GetFBRRecord();
                    if (obj == null)
                    {
                        obj = new AdminFBRSetting();
                        obj.POSID = CryptopEngine.Encrypt(txtPOSID.Text, GlobalSharing.Salt);
                        obj.Tocken = CryptopEngine.Encrypt(txtTocken.Text, GlobalSharing.Salt);
                        unitOfWork.AdminFBRSettingRepository.Insert(obj);
                    }
                    else
                    {
                        obj.POSID = CryptopEngine.Encrypt(txtPOSID.Text, GlobalSharing.Salt);
                        obj.Tocken = CryptopEngine.Encrypt(txtTocken.Text, GlobalSharing.Salt);
                    }
                    unitOfWork.Save();
                    MessageBox.Show("Credentials updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SharedVariables.AdminFBRSettings = unitOfWork.AdminFBRSettingRepository.GetFBRRecord();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating FBR Settings.", ex.Message, "Error");
            }
        }

        private void label96_Click(object sender, EventArgs e)
        {

        }

        private void tpPharmacy_Click(object sender, EventArgs e)
        {

        }

        private void chkConsumptionFromMultipleBatches_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMultiplBatchPOS.Checked)
            {
                chkConsumptionFromMultipleBatches.Checked = false;
                MessageBox.Show("Please UnCheck Allow Same Items with multiple batches", "Operation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void MultipleBatches_CheckedChanged(object sender, EventArgs e)
        {
            if (chkConsumptionFromMultipleBatches.Checked)
            {
                chkMultiplBatchPOS.Checked = false;
                MessageBox.Show("Please UnCheck Stock Consumption with multiple batches", "Operation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void chkConsumptionFromMultipleBatches_VisibleChanged(object sender, EventArgs e)
        {
            //chkConsumptionFromMultipleBatches.Checked = false;
        }

        private void line1_Click(object sender, EventArgs e)
        {

        }

        private void chkGender_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGender.Checked)
            {
                chkOptionalGender.Checked = true;
            }
            else
            {
                chkOptionalGender.Checked = false;
                chkRequiredGender.Checked = false;

            }
        }

        private void chkBirthDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBirthDate.Checked)
            {
                chkOptionalBirthDate.Checked = true;
            }
            else
            {
                chkOptionalBirthDate.Checked = false;
                chkRequiredBirthDate.Checked = false;

            }
        }

        private void chkAge_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAge.Checked)
            {
                chkOptionalAge.Checked = true;
            }
            else
            {
                chkOptionalAge.Checked = false;
                chkRequiredAge.Checked = false;

            }
        }

        private void chkRegistrationDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRegistrationDate.Checked)
            {
                chkOptionalRegistrationNumber.Checked = true;
            }
            else
            {
                chkOptionalRegistrationNumber.Checked = false;
                chkRequiredRegistrationDatae .Checked = false;

            }
        }

        private void chkAddress_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAddress.Checked)
            {
                chkOptionalAddress.Checked = true;
            }
            else
            {
                chkOptionalAddress.Checked = false;
                chkRequiredAddress.Checked = false;

            }
        }

        private void chkRefferedBy_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRefferedBy.Checked)
            {
                chkOptionalRefferedBy.Checked = true;
            }
            else
            {
                chkOptionalRefferedBy.Checked = false;

                chkRequiredRefferedBy.Checked = false;

            }
        }

        private void chkTags_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTags.Checked)
            {
                chkOptionalTags.Checked = true;
            }
            else
            {
                chkOptionalTags.Checked = false;
                chkRequiredTags.Checked = false;

            }
        }

        private void chkBloodGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBloodGroup.Checked)
            {
                chkOptionalBloodGroup.Checked = true;
            }
            else
            {
                chkOptionalBloodGroup.Checked = false;
                chkRequiredBloodGroup.Checked = false;

            }
        }

        private void chkAddPhoto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAddPhoto.Checked)
            {
                chkOptionalAddPhoto.Checked = true;

            }
            else
            {
                chkOptionalAddPhoto.Checked = false;
                chkRequiredAddPhoto.Checked = false;

            }
        }

        private void chkOptionalMR_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalMR.Checked)
            {
                chkRequiredMR.Checked = false;
            }
        }

        private void chkOptionalName_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalName.Checked)
            {
                chkRequiredName.Checked = true;
            }
        }

        private void chkOptionalEmail_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalEmail.Checked)
            {
                chkRequiredEmail.Checked = false;
            }
        }

        private void chkOptionalPhone_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalPhone.Checked)
            {
                chkRequiredPhone.Checked = false;
            }
        }

        private void chkOptionalGender_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalGender.Checked)
            {
                chkRequiredGender.Checked = false;
            }
        }

        private void chkOptionalBirthDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalBirthDate.Checked)
            {
                chkRequiredBirthDate.Checked = false;
            }
        }

        private void chkOptionalAge_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalAge.Checked)
            {
                chkRequiredAge.Checked = false;
            }
        }

        private void chkOptionalRegistrationNumber_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalRegistrationNumber.Checked)
            {
                chkRequiredRegistrationDatae.Checked = false;
            }
        }

        private void chkOptionalAddress_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalAddress.Checked)
            {
                chkRequiredAddress.Checked = false;
            }
        }

        private void chkOptionalRefferedBy_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalRefferedBy.Checked)
            {
                chkRequiredRefferedBy.Checked = false;
            }
        }

        private void chkOptionalTags_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalTags.Checked)
            {
                chkRequiredTags.Checked = false;
            }
        }

        private void chkOptionalBloodGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalBloodGroup.Checked)
            {
                chkRequiredBloodGroup.Checked = false;
            }
        }

        private void chkOptionalAddPhoto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalAddPhoto.Checked)
            {
                chkRequiredAddPhoto.Checked = false;
            }
        }

        private void chkRequiredMR_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredMR.Checked)
            {
                chkOptionalMR.Checked = false;
            }
        }

        private void chkRequiredName_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredName.Checked)
            {
                chkOptionalName.Checked = false;
            }
        }

        private void chkRequiredEmail_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredEmail.Checked)
            {
                chkOptionalEmail.Checked = false;
            }
        }

        private void chkRequiredPhone_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredPhone.Checked)
            {
                chkOptionalPhone.Checked = false;
            }
        }

        private void chkRequiredGender_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredGender.Checked)
            {
                chkOptionalGender.Checked = false;
            }
        }

        private void chkRequiredBirthDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredBirthDate.Checked)
            {
                chkOptionalBirthDate.Checked = false;
            }
        }

        private void chkRequiredAge_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredAge.Checked)
            {
                chkOptionalAge.Checked = false;
            }
        }

        private void chkRequiredRegistrationDatae_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredRegistrationDatae.Checked)
            {
                chkOptionalRegistrationNumber.Checked = false;
            }
        }

        private void chkRequiredAddress_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredAddress.Checked)
            {
                chkOptionalAddress.Checked = false;
            }
        }

        private void chkRequiredRefferedBy_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredRefferedBy.Checked)
            {
                chkOptionalRefferedBy.Checked = false;
            }
        }

        private void chkRequiredTags_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredTags.Checked)
            {
                chkOptionalTags.Checked = false;
            }
        }

        private void chkRequiredBloodGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredBloodGroup.Checked)
            {
                chkOptionalBloodGroup.Checked = false;
            }
        }

        private void chkRequiredAddPhoto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRequiredAddPhoto.Checked)
            {
                chkOptionalAddPhoto.Checked = false;
            }
        }

        private void txtPOSDiscount_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            
        }
        
        
        
        private void button3_Click(object sender, EventArgs e)
        {
           
           


        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
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
        private void BackupDB()
        {
            
            L:
            ProgressBar.Value = 0;
            lblStatus.Text = "InProgress";
           
            ProgressBar.Value = 10;
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            ProgressBar.Value = 20;
            //string location = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string database = txtDatabase.Text;
            try
            {
                ProgressBar.Value = 30;
                if (txtDevice.Text == string.Empty)
                {
                    MessageBox.Show("please enter backup file location");
                }
                else
                {
                    ProgressBar.Value = 40;
                    string cmd = "BACKUP DATABASE [" + database + "] TO DISK='" + txtDevice.Text + "\\" + txtFileName.Text + "-" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".bak'";
                    ProgressBar.Value = 50;
                    using (SqlCommand command = new SqlCommand(cmd, con))
                    {
                        ProgressBar.Value = 60;
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }
                        int a = command.ExecuteNonQuery();
                        ProgressBar.Value = 80;
                        if (a != 0)
                        {
                            ProgressBar.Value = 90;
                            con.Close();
                            
                            ProgressBar.Value = 100;
                            lblStatus.Text = "Completed";
                            lblPercent.Text = "Backup Finished .... " + 100 + "%"; 
                            MessageBox.Show("database backup done successefully");
                        }
                        else
                        {
                            //ProgressBar.Value = 10;
                            
                            MessageBox.Show("UnExpected Error Occurred , Backup Failed");
                            lblStatus.Text = "UnExpected Error Occurred , Backup Failed";
                        }
                        //BackupButton.Enabled = false;
                    }
                }

            }
            catch(Exception ex)
            {
                if(ex.Message.Contains("Cannot open backup device"))
                {
                    
                    MessageBox.Show("UnExpected Error Occurred , Backup Failed");
                    lblStatus.Text = "UnExpected Error Occurred , Backup Failed";
                    DialogResult dr = new DialogResult();
                    dr = MessageBox.Show("Access Denied", "The User" + System.Environment.UserName + " is Not Allowed to Write in " + txtDevice.Text + " Do You want Modify SQL Service Credentials ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(dr == DialogResult.Yes)
                    {
                        UpdateService();
                        goto L;
                    }
                    else
                    {
                        
                        MessageBox.Show("UnExpected Error Occurred , Backup Failed");
                        lblStatus.Text = "UnExpected Error Occurred , Backup Failed";
                        return;
                    }
                }
            }
        }
        private void UpdateService()
        {
            string objPath = string.Format("Win32_Service.Name='{0}'", "MSSQL$SQLEXPRESS");
            using (ManagementObject service = new ManagementObject(new ManagementPath(objPath)))
            {
                object[] wmiParams = new object[11];

                
                    wmiParams[6] = "LocalSystem";
                    wmiParams[7] = "";
                    //wmiParams["DesktopInteract"] = true;



                object invokeResult = service.InvokeMethod("Change", wmiParams);

                // handle invokeResult - no error up to this point
            }
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            if(txtDevice.Text == string.Empty || txtFileName.Text == string.Empty)
            {
                MessageBox.Show(txtFileName.Text != string.Empty || txtDevice.Text != string.Empty ? (txtFileName.Text == string.Empty ? "File Name Can't be Empty" : "Backup  Location can't be Empty") : "Backup File Name and Backup  Location is Necessary");
                return;
            }
            BackupDB();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFileName.Text))
            {
                MessageBox.Show("Please Enter File Name First .... ");
                return;
            }
            
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath.Contains("C:\\"))
            {
                DialogResult dr = MessageBox.Show("You have selected System Drive as Backup Location , It can  cause some User Authentication Error while saving Backup from local user system. \n It is Recommended that select Backup Location other then system drive. ", "System Drive Auth Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dr == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
            }
            txtDevice.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SQL Server BackUp File (*.bak)|*.bak";
            ofd.Title = "Choose Backup File";
            ofd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            if(ofd.ShowDialog() == DialogResult.OK) 
            {
                txtbackupfileaddress.Text = ofd.FileName;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            lblRestoreProgress.Text = "InProgress .... ";
            lblRestorePercent.Text = "Restore is In Progress ..." + "(2%)";
            RestoreProgressBar.Value = 10;
            RestoreDB();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBackUpFilename.Text))
            {
                MessageBox.Show("Please Enter File Name First .... ");
                return;
            }
            folderBrowserDialog1.ShowDialog();
            if (txtBackUpFilename.Text != string.Empty)
            {
                if (folderBrowserDialog1.SelectedPath.Contains("C:\\"))
                {
                    DialogResult dr = MessageBox.Show("You have selected System Drive as Backup Location , It can  cause some User Authentication Error while saving Backup from local user system. \n It is Recommended that select Backup Location other then system drive. ", "System Drive Auth Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dr == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
                }
                txtDefaultLocation.Text = folderBrowserDialog1.SelectedPath + "\\" + txtBackUpFilename.Text + ".bak";
            }
            else
            {
                txtDefaultLocation.Text = folderBrowserDialog1.SelectedPath +"\\" +  txtDefaultDatabaseName.Text + ".bak";
            }
        }
        public string a = "";
        public DataTable selectdt;
        private void ExecuteQuery(string query)
        {
            try
            {
                if (CheckKeyWord(query))
                {
                    string Query = txtquery.Text;
                    SqlConnection con = new SqlConnection(Shared.GetConnectionString());
                    SqlCommand com = new SqlCommand(Query, con);
                    if (Query.ToLower().Contains("select"))
                    {
                        SqlDataAdapter sda = new SqlDataAdapter(com);
                        selectdt = new DataTable();
                        con.Open();
                        sda.Fill(selectdt);
                        grdResult.Visible = true;
                        grdResult.DataSource = selectdt;
                        if (selectdt.Columns.Count > 8)
                        {
                            grdResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

                        }
                        else
                        {
                            grdResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        }
                        a = "select";
                        con.Close();
                    }
                    else
                    {
                        grdResult.Visible = false;
                        con.Open();
                        int s = com.ExecuteNonQuery();
                        a = s.ToString();
                        if (s > 0)
                        {
                            if (s == 1)
                            {
                                txtresult.Text = a + " row Updated Successfully";
                            }
                            else
                            {
                                txtresult.Text = a + " rows Updated Successfully";
                            }
                        }
                        else if (s == 0)
                        {
                            txtresult.Text = a + " rows Updated, Error Occured while executing query";
                        }
                        else
                        {
                            txtresult.Text = "Query Execution Finished ...";
                        }
                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                txtresult.Text = ex.Message;
            }
        }
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public bool checkfornet()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }
        private bool CheckKeyWord(string Query)
        {
            L:
            bool status = true;
            if (Query.ToLower().Contains("sys.databases") || Query.ToLower().Contains("master") || Query.ToLower().Contains("__MigrationHistory") || Query.ToLower().Contains("hwdatas") || Query.ToLower().Contains("loginhistory") || Query.ToLower().Contains("offlinetasks") || Query.ToLower().Contains("rightusers") || Query.ToLower().Contains("rights") || Query.ToLower().Contains("userpermissions") || Query.ToLower().Contains("userroles") || Query.ToLower().Contains("userroleusers") || Query.ToLower().Contains("users") || Query.ToLower().Contains("dbo.hwdatas") || Query.ToLower().Contains("drop") || Query.ToLower().Contains("delete"))
            {
                
                    status = false;
                    DialogResult dr = new DialogResult();
                    dr = MessageBox.Show("Entered Query Contains UnAllowed Keywords , To Execute these types of Queries OTP Required? Do you want to continue ..... ?", "Access Denied", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    if (checkfornet())
                    {
                        Random r = new Random();
                        long OTP = r.Next(4290535, 9898795);
                        string supportEmail = "healthwiredeployments@gmail.com";
                        string from = "healthwiredeployments@gmail.com";
                        string password = "yvtdlppnyrfwayef";
                        string message = "<h1>" + "OTP Request for Execuating  UnAllowed Queries" + "</h1>" + "<br><p>" + "Pharmacy: " + SharedVariables.AdminPractiseSetting.Name + "<br>" + "Practice ID: " + SharedVariables.AdminPractiseSetting.PracticeId + "<br>" + "Pharmacy Address: " + SharedVariables.AdminPractiseSetting.Address + "<br>" + "Phone Phone: " + SharedVariables.AdminPractiseSetting.Phone + "<br>" + "" + "<mark><h3>Query: " + txtquery.Text + " </h3></mark><br> OTP for the Pharmacy is Given  Below, Kindly Share the OTP with Pharmacy if Pharmacy Allowed to use these Queries <br> OTP for Query Execuation is : </p><br><center><h1><mark>" + OTP + "<mark></center></h1>";
                        string to = "Hassan.rasool@healthwire.pk";
                        string subject = "OTP Request for Query Execuation";
                        string smtpserver = "smtp.gmail.com";
                        string attachment = "";
                        int rslt = SendEmail(from, to, subject, message, smtpserver, from, password, attachment);
                        if (rslt == 1)
                        {
                            string opt = InputBox.ShowInputBox("Enter OTP", true);
                            if (opt == OTP.ToString())
                            {
                                status = true;
                            }
                            else
                            {
                                status = false;
                                //txtquery.Text = "";
                                txtresult.Text = "Invalid OTP , Access Denied";
                                //MessageBox.Show("Invalid OTP");
                            }
                        }
                       

                    }
                    else
                    {
                        DialogResult d = MessageBox.Show("OTP Sent Failed ..... !\n Reason: Internet Connection Not  Available.", "OTP Sent Failed" , MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        if (d == DialogResult.Retry)
                        {
                            goto L;
                        }
                        else
                        {
                            status = false;
                            return status;
                        }
                    }
                }
                else
                {
                    status = false;
                    MessageBox.Show("Query Execuation Cancelled .... ");
                    txtquery.Text = "";
                }
                
                

            }
            else
            {
                status = true;
            }
            return status;
        }
        private void button6_Click_1(object sender, EventArgs e)
        {
            ExecuteQuery(txtquery.Text);
        }
        private bool checkEmail(string s)
        {
            bool t = false;
            if (s.Contains("@"))
            {
                t = true;
            }
            else
            {
                t = false;
            }
            return t;
        }
        private int SendEmail(string from, string To, string subject, string message, string smtpserver, string user, string pass, string attachement)
        {
            int result = 0;
            try
            {
                if (checkEmail(from) && checkEmail(To))
                {
                    if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(To) && !string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(smtpserver) && !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass))
                    {
                        MailMessage mail = new MailMessage();
                        mail.To.Add(To);
                        mail.From = new MailAddress(from);
                        mail.Subject = subject;
                        mail.Body = message;
                        mail.IsBodyHtml = true;
                        if (!string.IsNullOrEmpty(attachement))
                        {
                            mail.Attachments.Add(new Attachment(attachement));
                        }
                        SmtpClient client = new SmtpClient(smtpserver);

                        client.Port = 587;
                        client.EnableSsl = true;
                        //client.Port = 25;
                        client.Host = "smtp.gmail.com";
                        client.UseDefaultCredentials = false;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.Credentials = new System.Net.NetworkCredential(user, pass);
                        
                        client.Send(mail);
                        result = 1;
                        
                    }
                    else
                    {
                        MessageBox.Show("OTP Send Failed , Error: Something Missing");
                    }
                }
                else
                {
                    MessageBox.Show("OTP Send Failed , Error: Invalid Gmail");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mail Sent Failed , \n" + "Error Message: " + ex.Message);
            }
            return result;
        }

        private void txtBackUpFilename_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void rbFifo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(lblImagePath.Text) || lblImagePath.Text == "No File Choosen")
            {
                MessageBox.Show("Attach A logo First Please");
                return;
            }
            frmLogoEditor frm = new frmLogoEditor(lblImagePath.Text);
            frm.Show();
        }
        public bool restart = false;
        private void chkDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            if(chkDarkMode.Checked != SharedVariables.AdminPharmacySetting.DarkMode)
            {
                restart = true;
            }
            else
            {
                restart = false;
            }
        }
        public bool change = true;
        private void txtPurchaseOrderTermsandConditions_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                string pin = Interaction.InputBox("Please Security Key.....");
                
                
                if (pin != null && pin != "Admin123#")
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Please Contact Healthwire!, You are not allowed to Perform This  Action", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    new frmFranchiseDeployement().ShowDialog();
                    this.Cursor = Cursors.Default;
                }
                
            }
            catch (Exception ex)
            {
               
                this.Cursor = Cursors.Default;
                MessageBox.Show("Please Contact Healthwire, You are not allowed to Perform This  Action", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            
        }

        private void tpPractise_Click(object sender, EventArgs e)
        {

        }

        private void cmbPrintFormats_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}