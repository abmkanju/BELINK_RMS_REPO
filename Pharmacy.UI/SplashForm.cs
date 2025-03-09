using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
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

namespace Pharmacy.UI
{
    public partial class SplashForm : Form
    {
        //System.Timers.Timer t;
        public SplashForm()
        {
            InitializeComponent();
            //LoadVersion
            lblVersion.Text = SharedVariables.ApplicationVersion;
        }

        private void SplashForm_Shown(object sender, EventArgs e)
        {
            
            LoadDetails();
            //t = new System.Timers.Timer();
            //t.Interval = 1000;
            //t.Enabled = true;
            //t.Elapsed += new System.Timers.ElapsedEventHandler(Elapsed);
            //t.Start();
        }
        private async void LoadDetails()
        {
            await Task.Delay(100); //--------- Wait to run the splash form -------------//
            try
            {
                //bool crashSystem = false;
                // load all configurations and any other application wise data here
                using (UnitOfWork uw = new UnitOfWork())
                {
                    LoadConfigurations(uw);
                    /*                    HwData objHw = uw.HwDataRepository.GetAll().OrderByDescending(r => r.HwDataId).FirstOrDefault();
                                        if (string.IsNullOrEmpty(objHw.SystemExpiry))
                                        {
                                            crashSystem = true;
                                        }
                                        else
                                        {
                                            DateTime dtExp;
                                            string datetime = CryptopEngine.Decrypt(objHw.SystemExpiry.ToString(), GlobalSharing.Salt);
                                            bool parsed = DateTime.TryParse(CryptopEngine.Decrypt(objHw.SystemExpiry.ToString(), GlobalSharing.Salt), out dtExp);
                                            if (!parsed)
                                            {

                                                crashSystem = true;
                                            }
                                            else
                                            {
                                                if (DateTime.Now.Date < dtExp)
                                                {
                                                    crashSystem = false;
                                                }
                                                else
                                                {
                                                    crashSystem = true;
                                                }
                                            }
                                        }
                                    }
                                    if (crashSystem)
                                    {
                                        // call api here to check if aplication has been activated(practiseId)
                                        MessageBox.Show("System Crashed and stopped working, please contact healthwire for further processing.", "System Crashed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                        Application.Exit();
                                        //DialogResult dr = MessageBox.Show("System Subcription Expired, Do you want to ReSubcribe the Package?", "System Expired", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        //if (dr == DialogResult.Yes && !Properties.Settings.Default.KeyForword)
                                        //{
                                        //    string key = ProductActivation.GenerateActivationKey();
                                        //    string subject = "Product Activation Request by " + SharedVariables.AdminPractiseSetting.Name;
                                        //    string Body = "<h3>Pracise Name: " + SharedVariables.AdminPractiseSetting.Name + "<br>" + "Pracise ID: " + SharedVariables.AdminPractiseSetting.PracticeId + "<br>" + " Pharmacy Address: " + SharedVariables.AdminPractiseSetting.Address + "<br>" + "Pharmacy Contact# : " + SharedVariables.AdminPractiseSetting.Phone + "</h3>";
                                        //    Body = Body + "<Center><h1>" + key + "</h1></Center>";
                                        //    bool mail = ProductActivation.SendEmail(subject, Body);
                                        //    if (mail)
                                        //    {
                                        //        Properties.Settings.Default.ActivationKey = key;
                                        //        Properties.Settings.Default.Save();
                                        //        MessageBox.Show("Activation Request Send to Healthwire.pk , Please Contact Healthwire for Activation Key....");
                                        //    }
                                        //    else
                                        //    {
                                        //        MessageBox.Show("Connection to The Internet Connection Failed , Please Try Again .... ");
                                        //        return;
                                        //    }
                                        //    new frmActivation().ShowDialog();
                                        //    Application.Restart();
                                        //}
                                        //else if (Properties.Settings.Default.KeyForword)
                                        //{
                                        //    new frmActivation().ShowDialog();
                                        //    Application.Restart();
                                        //}
                                        //else
                                        //{
                                        //    MessageBox.Show("Thank You for Using HCloud PMS System, Contact Healthwire for More Information.", "Thank you for Choosing HCloud", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        //    Application.Exit();
                                        //}
                                    }
                                    this.Invoke(new Action(() => { this.Hide(); }));
                                    if (IsApplicationActivated())
                                    {*/
                    this.Invoke(new Action(() => { frmLogin f = new frmLogin(); f.Show(); }));
                    this.Hide();
                    /*}
                    else
                    {
                        this.Invoke(new Action(() => { ActivationForm f = new ActivationForm(); f.Show(); }));
                    }*/
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                    {
                        MessageBox.Show(ex.InnerException.InnerException.Message);
                        if (ex.InnerException.InnerException.InnerException != null)
                        {
                            MessageBox.Show(ex.InnerException.Message);
                        }
                    }
                }
                MessageBox.Show("System Crashed and stopped working, please contact healthwire for further processing. Exception :  " + ex.Message, "System Crashed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //Application.Exit();
                this.Invoke(new Action(() => { frmMain f = new frmMain(); f.Show(); }));
                this.Hide();
            }
        }

        private bool IsApplicationActivated()
        {
            try
            {
                string Status = CryptopEngine.Decrypt(Properties.Settings.Default.Activated.ToString(), GlobalSharing.Salt);
                if (Convert.ToBoolean(Status))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void OpenMainFrom(object obj)
        {
            frmMain f = new frmMain();
            f.WindowState = FormWindowState.Maximized;
            Application.Run(f);
        }

        private void OpenActivationFrom(object obj)
        {
            ActivationForm f = new ActivationForm();
            f.BringToFront();
            //f.WindowState = FormWindowState.Maximized;
            Application.Run(f);
        }

        private void LoadConfigurations(UnitOfWork unitOfWork)
        {
            try
            {
                #region Admin Invoice
/*                List<FlatDiscount> TodayDiscounts = new List<FlatDiscount>();
                DataTable dt = new DataTable();
                dt.Columns.Add("ItemId", typeof(int));
                dt.Columns.Add("Item Name", typeof(string));
                dt.Columns.Add("Barcode", typeof(string));
                dt.Columns.Add("Generic Name", typeof(string));
                dt.Columns.Add("Unit", typeof(string));
                dt.Columns.Add("Manufacturer", typeof(string));
                dt.Columns.Add("Cost Price", typeof(double));
                dt.Columns.Add("Retail Price", typeof(double));
                dt.Columns.Add("Quantity", typeof(int));*/

                //List<BulkItemsVM> bulkList = new List<BulkItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    SharedVariables.AdminInvoiceSetting = unitOfWork.AdminInvoiceSettingRepository.GetAll().LastOrDefault();
                    SharedVariables.AdminProcedureInvoiceSetting = unitOfWork.AdminProcedureInvoiceSettingRepository.GetAll().LastOrDefault();
                    SharedVariables.AdminPractiseSetting = unitOfWork.AdminPractiseSettingRepository.GetAll().LastOrDefault();
                    SharedVariables.AdminPrintFormatSetting = unitOfWork.AdminPrintFormatSettingRepository.GetAll().LastOrDefault();
                    SharedVariables.AdminPharmacySetting = unitOfWork.AdminPharmacySettingRepository.GetAll().LastOrDefault();
                    if (!SharedVariables.IsTesting) // this will avoid extra load time when there is no need to load items data for testing
                    {
                        SharedVariables.BulkLoadedItemsList = unitOfWork.ItemRspository.GetBulkAllActiveItems();
                    }
                    //SharedVariables.TodayDiscounts = unitOfWork.FlatDiscountRepository.getTodayDiscounts();
                    SharedVariables.AdminShiftSettings = unitOfWork.AdminShiftsSettingRepository.GetActiveShifts();
                    SharedVariables.AdminShiftMasterSetting = unitOfWork.AdminShiftMasterSettingRepository.GetAll().FirstOrDefault();
                    //SharedVariables.AdminFBRSettings = unitOfWork.AdminFBRSettingRepository.GetFBRRecord();
                }
                //WWW Comment the code because know we generate it at run time
                //foreach (var i in SharedVariables.BulkLoadedItemsList)
                //{
                //    dt.Rows.Add(i.ItemId, i.ItemName, i.Barcode, i.GenericName, i.Unit, i.Manufacturer, i.CostPrice, i.RetailPrice, 1);
                //}
                //SharedVariables.BulkLoadedItemsDataView = new DataView(dt);
                #endregion Admin Invoice Settings
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                    {
                        MessageBox.Show(ex.InnerException.InnerException.Message);
                        if (ex.InnerException.InnerException.InnerException != null)
                        {
                            MessageBox.Show(ex.InnerException.Message);
                        }
                    }
                }
                SharedFunctions.ShowErrorMessage("Failed to load Some Settings, you can Continue With Your Work.", ex.Message, "Failed to Load Some Settings.");
            }
        }

        private void LoadDefaultItems()
        {
            frmWaiting f = new frmWaiting();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}