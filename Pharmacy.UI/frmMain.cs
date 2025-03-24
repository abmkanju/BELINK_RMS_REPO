using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.UI.MenuBar.Pharmacy;
using Pharmacy.UI.MenuBar.Help;
using Pharmacy.Core.Entities;
using Pharmacy.UI.MenuBar.UserMenue;
using Pharmacy.Infrastructure;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Pharmacy.Core;
using Pharmacy.UI.Indoor.Menu;
using System.Timers;
using Microsoft.Win32;
using System.ServiceProcess;
using Pharmacy.UI.MenuBar;
using Pharmacy.Core.ViewModels;
using PagedList;
using Pharmacy.UI.MenuBar.Preferences;
using Pharmacy.UI.ManageAuditLog;
using System.Management;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Net.Http;
using Pharmacy.Core.Utility;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Diagnostics;
using Ionic.Zip;
using System.Text.RegularExpressions;
using Pharmacy.UI.FranchiseModuels;
using Pharmacy.UI.MenuBar.OrderVouchers;
using System.Transactions;
using Pharmacy.UI.Reports.Financial;
using Pharmacy.UI.MenuBar.ShopActivities;

namespace Pharmacy.UI
{

    public partial class frmMain : Form
    {
        BackgroundWorker bgW_LowStockItems = new BackgroundWorker();
        private bool isMenuCpsd = false;
        System.Timers.Timer PnlAnimTimer = new System.Timers.Timer();
        System.Timers.Timer CheckLogout = new System.Timers.Timer();
        ToolStripMenuItem clickedItem;
        UnitOfWork unitOfWork;
        System.Timers.Timer EmailRetryTimer = new System.Timers.Timer();
        Service obj = new Service();
        enum CurrentNotitype { LowStock = 1, Expiry = 2 };
        CurrentNotitype currNotiType = CurrentNotitype.LowStock;
        //System.Timers.Timer tCheckFormsCollection = new System.Timers.Timer();
        int PnlMenuWidth = 0;
        private bool isMenuPanelVisible = true;

        public frmMain()
        {
            InitializeComponent();
            
            //LoadVersion
            timer3.Start();
            timer2.Start();
            timer1.Start();
            lblVersion.Text = SharedVariables.ApplicationVersion;
            this.PnlMenuWidth = pnlMenu.Width;
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            //LoadCustomCursor();
            //Properties.Settings.Default.Date = null;
            if (SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
            {
                timer9.Start();
            }

        }
        public string UnpackOrders()
        {
            DataTable order = get("Select * from OnlineOrders where Pack=" + 0);
            return order.Rows.Count.ToString();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleparam = base.CreateParams;
                handleparam.ExStyle |= 0x02000000;
                return handleparam;
            }
        }
        private void LoadCustomCursor()
        {
            // Load the custom cursor from the embedded resource
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream cursorStream = assembly.GetManifestResourceStream("Pharmacy.UI.icons8-hand-cursor-32_1.cur");

            // Create the custom cursor from the stream
            Cursor customCursor = new Cursor(cursorStream);

            // Set the form's cursor to the custom cursor
            this.Cursor = customCursor;

            // If you want to set a custom cursor for a specific control, use:
            // someControl.Cursor = customCursor;
        }
        private void SetactiveItem(object sender)
        {
            clickedItem = (ToolStripMenuItem)sender;
            SetToolStripItems(menuStrip1.Items);
        }
        private void view_Notifications()
        {
            
            string[] notifications = GetNotifications();

            NotificationManager notificationManager = new NotificationManager(lblnotify, notifications);
            notificationManager.Start();

            // Do other operations...

            //notificationManager.Stop();

        }
        private string[] GetNotifications()
        {
            string[] notifications = { "Notification 1 that is for the checking responsiveness of Label Control", "Notification 2", "Notification 3" };

            return notifications;
        }
        private void stop_Notifications()
        {

            string[] notifications = GetNotifications();

            NotificationManager notificationManager = new NotificationManager(lblnotify, notifications);
            //notificationManager.Start();

            // Do other operations...

            notificationManager.Stop();

        }
        private void SetToolStripItems(ToolStripItemCollection dropDownItems)
        {
            try
            {
                foreach (object obj in dropDownItems)
                //for each object.
                {
                    ToolStripMenuItem subMenu = obj as ToolStripMenuItem;
                    //Try cast to ToolStripMenuItem as it could be toolstrip separator as well.

                    if (subMenu != null)
                    //if we get the desired object type.
                    {
                        if (subMenu.HasDropDownItems) // if subMenu has children
                        {
                            SetToolStripItems(subMenu.DropDownItems); // Call recursive Method.
                        }
                        else // Do the desired operations here.
                        {

                            //subMenu.Visible = UserRights.Where(x =>
                            //x.FormID == Convert.ToInt32(subMenu.Tag)).First().CanAccess;
                            if (subMenu.Text == clickedItem.Text)
                            {
                                subMenu.BackColor = SharedVariables.MenuSelectionBackColor;
                                subMenu.ForeColor = SharedVariables.MenuSelectionForeColor;
                            }
                            else
                            {
                                subMenu.BackColor = SystemColors.Control;
                                subMenu.ForeColor = Color.Black;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SetToolStripItems",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void iTemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmAllItems(), this);
        }
        private void addStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddStock(), this);
        }
        private void addSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddSupplier(), this);
        }
        private void addMultipleItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmMultipleItems(), this);
        }
        private void addPurchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmPurchaseOrder(), this);
        }
        private void purchaseOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmPurchaseOrders(), this);
        }
        private void allItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAllItems(), this);
        }
        private void addPatientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddPatient(), this);
        }
        private void consumeStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmConsumeStocks(), this);
        }
        private void pateintsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmPatients(), this);
        }
        private void invoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmInvoice(), this);
        }
        private void shortCutKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmShortCutKeys(), this);
        }
        private void addPurchaseOrderStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddStock2(), this);
        }
        private void manageStocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmManageStocks(), this);
        }
        private void stockAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmStockAdjustments(), this);
        }
        private void stockAdjustmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmStockAdjustments(), this);
        }
        private void tCheckFormsCollection_Elapsed(object sender, ElapsedEventArgs e)
        {
            //FormCollection openFroms = Application.OpenForms;
            //int activeFormsCount = 0;
            //foreach (Form form in openFroms)
            //{
            //    if (Form.ActiveForm.Name == form.Name)
            //    {
            //        activeFormsCount += 1;
            //    }               
            //}

            //int count = Application.OpenForms.Count;
            //if (count == 2)
            //{
            //    this.Invoke(new Action(() =>
            //    {
            //        pnlDashBoard.BringToFront();
            //    }));
            //}
        }
        public void setSideBarVisble(bool flag)
        {
            pnlMenu.Visible = flag;
            pnlSideTopHeader.Visible = flag;
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            btnToggleLogin.Text = "Log Out";
            pnlNotifications.MouseWheel += new MouseEventHandler(pnlNotifications_MouseWheel);
            //key.SetValue("d1", DateTime.Now);
            //NotificationTimer.Start();
            //Notify();
            //exp();
            // timer to auto show dashboard
            //System.Timers.Timer tCheckFormsCollection = new System.Timers.Timer();
            //tCheckFormsCollection.Interval = 2000;
            //tCheckFormsCollection.Enabled = true;
            //tCheckFormsCollection.Elapsed += new ElapsedEventHandler(tCheckFormsCollection_Elapsed);
            //tCheckFormsCollection.Start();
            SharedVariables.SideBarWidth = panel1.Width;
            SharedVariables.TopBarHeight = panel11.Height;
            SharedVariables.TopMenueBarHeight = menuStrip1.Height;
            pnlMenu.Visible = false;
            pnlSideTopHeader.Visible = false;
            backupdb();
            pharmacyToolStripMenuItem.Visible = false;
            patientsToolStripMenuItem.Visible = false;
            pereferencesToolStripMenuItem.Visible = false;
            reportsToolStripMenuItem.Visible = false;
            userToolStripMenuItem.Visible = false;
            securityToolStripMenuItem.Visible = false;
            //windowToolStripMenuItem.Visible = false;
            indoorManagementsToolStripMenuItem.Visible = false;
            indoorPatientsToolStripMenuItem.Visible = false;

            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Maximized;
            this.BringToFront();
            lblAppStatus.Text = "Please Login to Continue...";
            //Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.FromArgb(42, 196, 244);
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.White;
            //SharedFunctions.SetApplicationStartupValues();
            DisableNotRequiredModules();
            //LoginUser();
            //if (SharedVariables.LoggedInUser != null) // go further if user is logged in successfully
            //{
            //    CheckForLowStockMail();
            //}
            if (SharedVariables.AdminPharmacySetting.DarkMode)
            {
                DarkMode.EnableDarkMode(this);
            }
            /*Color startColor = Color.FromArgb(64, 0, 64); // Yellow
            Color endColor = Color.Aqua; // Cyan
            
            LabelBlinker.SetGradientBackground(lblNotifiylabel, startColor, endColor);*/
            
            if (checkfornet())
            {
                CheckForUpdateNotificationAsync();
                
            }
            LabelBlinker.TypeStringsAsync(notificationslabel);
            //LoadDefaultItems();

            //GenerateExpNotification();

            //timer5.Start();


            // register and run background workers
            //this.bgW_LowStockItems.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgW_LowStockItems_DoWork);
            //this.bgW_LowStockItems.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgW_LowStockItems_RWComp);
            //this.bgW_LowStockItems.RunWorkerAsync();

        }

        private async void CheckForUpdateNotificationAsync()
        {
            try
            {
                string version = Application.ProductVersion.ToString();
                WebClient webClient = new WebClient();
                string updateFileContent = await webClient.DownloadStringTaskAsync("https://www.dropbox.com/s/zubh9socoz5d5pu/updates.txt?dl=1");

                string latestVersion = GetLatestProductVersion(updateFileContent);

                if (!string.IsNullOrEmpty(latestVersion) && IsNewerVersion(latestVersion, version))
                {
                    await Task.Run(() =>
                    {
                    // Show the update notification form on a separate thread.
                    new frmUpdateNotification().ShowDialog();
                    });
                }
            }
            catch (Exception) { }
        }

        private string GetLatestProductVersion(string updateFileContent)
        {
            string latestVersion = null;
            string pattern = @"ProductVersion\s*=\s*(\d+\.\d+\.\d+\.\d+)";

            MatchCollection matches = Regex.Matches(updateFileContent, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string version = match.Groups[1].Value;
                if (IsNewerVersion(version, latestVersion))
                {
                    latestVersion = version;
                }
            }

            return latestVersion;
        }

        private bool IsNewerVersion(string version1, string version2)
        {
            // Assuming version format is X.Y.Z.W
            if (string.IsNullOrEmpty(version2))
                return true;

            string[] v1Parts = version1.Split('.');
            string[] v2Parts = version2.Split('.');

            for (int i = 0; i < 4; i++)
            {
                int v1 = int.Parse(v1Parts[i]);
                int v2 = int.Parse(v2Parts[i]);

                if (v1 > v2)
                    return true;
                else if (v1 < v2)
                    return false;
            }

            return false;
        }

        private void CheckAuditLog()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    if (unitOfWork.UserRoleRepository.CheckAdminUserRole(SharedVariables.LoggedInUser.UserId))
                    {
                        if (unitOfWork.AuditLogsRepository.CheckAuditLogReachLimit())
                        {
                            SharedFunctions.MessageBoxShow("Your audit log entries exceeded 50,000. Need to delete 30,000 entries to proceed further", MessageType.Info);
                            unitOfWork.AuditLogsRepository.DeleteBulkAuditLog();
                            unitOfWork.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow("Error occured while deleteing the Audit Log:" + ex.Message, MessageType.Error);
            }
        }


        private void LogoutUser_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                //&& !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin)
                CheckLogout.Enabled = false;
                CheckLogout.Stop();
                if (SharedVariables.LoggedInUser == null)
                    return;

                if (SharedVariables.LoggedInUser.Shift != null &&
                    SharedVariables.LoggedInUser.Shift.IsActive &&
                    SharedVariables.AdminShiftMasterSetting != null &&
                    SharedVariables.AdminShiftMasterSetting.EnforceLogout &&
                    SharedVariables.LoggedInUser.Shift.EndTime.TimeOfDay <= DateTime.Now.TimeOfDay)
                {
                    MessageBox.Show("Your shift timing is over, system is going to logout.", "Shit Eneded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Properties.Settings.Default.IsSavePassword = false;
                    Properties.Settings.Default.UserName = "";
                    Properties.Settings.Default.Password = "";
                    Application.Restart();
                }

                //if (SharedVariables.LoggedInUser.Shift != null &&
                //    SharedVariables.LoggedInUser.Shift.IsActive &&
                //    SharedVariables.AdminShiftMasterSetting.EnforceLogout &&
                //    SharedVariables.LoggedInUser.Shift.EndTime.TimeOfDay <= DateTime.Now.TimeOfDay)
                //{

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("The following error occur :" + ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                CheckLogout.Enabled = true;
                CheckLogout.Start();
            }
        }
        private void LoadDefaultItems()
        {
            try
            {
                //bool proceedToLoadData = false;
                //using (unitOfWork = new UnitOfWork())
                //{
                //    if (unitOfWork.SupplierRepository.SuppliersExist() && unitOfWork.ItemRspository.ItemsCount() > 1)
                //    {
                //        MessageBox.Show("Data already loaded", "Data exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        return;
                //    }
                //    else if (!unitOfWork.SupplierRepository.SuppliersExist() || unitOfWork.ItemRspository.ItemsCount() == 1)
                //    {
                //        proceedToLoadData = true;
                //    }
                //}
                //if (proceedToLoadData)
                //{
                //    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Inventory-list.xls");
                //    DataTable dt = ReadExcel(path, ".xls");
                //    loadDefaultItemsData_FromExcelSheet(dt);
                //    //loadInventoryData_FromExcelSheet(dt);
                //}
                using (unitOfWork = new UnitOfWork())
                {
                    bool exists = unitOfWork.ItemRspository.ExistsAny();
                    bool removed = unitOfWork.AdminPractiseSettingRepository.IsUserDeletedDefaultData();

                    if (exists || removed)
                    {
                        return;
                    }
                }
                frmWaiting f = new frmWaiting();
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DisableNotRequiredModules()
        {
            Disable_IndoorModule();
            Disable_ProcedureInvoice();
            DisableNotRquiredItems_Pharmacy();
        }
        private void DisableNotRquiredItems_Pharmacy()
        {
            financialReportsToolStripMenuItem.Visible = false;
        }
        private void Disable_ProcedureInvoice()
        {
            proceduresListToolStripMenuItem.Visible = false;
            procedureInvoiceToolStripMenuItem.Visible = false;
        }
        private void Disable_IndoorModule()
        {
            indoorManagementsToolStripMenuItem.Visible = false;
            indoorPatientsToolStripMenuItem.Visible = false;
            pereferencesToolStripMenuItem.Visible = false;
        }
        private void CheckForLowStockMail()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    //DateTime LastSent = DateTime.Now.AddDays(-16);
                    DateTime LastSent = unitOfWork.LowStockMailRepository.GetLastMailSentDate();
                    int daysDiff = (DateTime.Now.Date - LastSent.Date).Days;
                    if (daysDiff > 15)
                    {
                        Thread worker = new Thread(() => { sendAsyncronousEmails(); });
                        worker.Name = "LowStockMailSender";
                        worker.IsBackground = true;
                        worker.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("System failed to send low stock mail to practise." + Environment.NewLine + "Please check your internet connection." + Environment.NewLine + " Sytem will try after " + SharedVariables.LowStockMailRetryInterval + " minutes.", "Failed to send low stock mail.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EmailRetryTimer.Interval = SharedVariables.LowStockMailRetryInterval * 60 * 1000;
                EmailRetryTimer.Enabled = true;
                EmailRetryTimer.Elapsed += new System.Timers.ElapsedEventHandler(EmailRetryTimer_Elapsed);
                EmailRetryTimer.AutoReset = true;
                EmailRetryTimer.Start();
            }
        }
        private void sendAsyncronousEmails()
        {
            try
            {
                if (!SharedFunctions.IsInternetAvailable())
                {
                    throw new Exception("Intenet un-available.");
                }
                Reports.Pharmacy.LowStockUI f = new Reports.Pharmacy.LowStockUI();
                string filePath = f.GetLowStockExcelSheet();
                List<string> AdminUsers = new List<string>();
                using (unitOfWork = new UnitOfWork())
                {
                    // AdminUsers = unitOfWork.UserRepository.GetAdminUsersEmails();
                }
                //SharedFunctions.Email(filePath , new List<string> { "faani1786@gmail.com", "umar.waqas.se@gmail.com" });
                //SharedFunctions.Email("Low Stock", "email from umar", new[] { filePath }, "umar.getReports@gmail.com", "Alikes1425",AdminUsers);
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.LowStockMailRepository.AddLastMailSentDate(new LowStockMail { LastSent = DateTime.Now, CreatedAt = DateTime.Now });
                    unitOfWork.Save();
                }
                if (EmailRetryTimer.Enabled)
                {
                    EmailRetryTimer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                this.BeginInvoke(new Action(() => { MessageBox.Show("An error occurred while generating low stock mail to admins, please check your internet connection.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information); }));
            }
        }
        private void EmailRetryTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckForLowStockMail();
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            //#region short cuts for main screens
            //if (keyData == (Keys.Control | Keys.A))
            //{
            //    SharedFunctions.OpenForm(new frmAllItems(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Alt | Keys.A))
            //{
            //    SharedFunctions.OpenForm(new frmMultipleItems(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Shift | Keys.A))
            //{
            //    SharedFunctions.OpenForm(new frmItems(), this);
            //}

            //if (keyData == (Keys.Control | Keys.B))
            //{
            //    SharedFunctions.OpenForm(new frmManageStocks(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Shift | Keys.B))
            //{
            //    SharedFunctions.OpenForm(new frmAddStock(), this);
            //}

            //if (keyData == (Keys.Control | Keys.C))
            //{
            //    SharedFunctions.OpenForm(new frmPurchaseOrders(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Shift | Keys.C))
            //{
            //    SharedFunctions.OpenForm(new frmPurchaseOrder(), this);
            //}

            //if (keyData == (Keys.Control | Keys.D))
            //{
            //    SharedFunctions.OpenForm(new frmStockConsumptions(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Shift | Keys.D))
            //{
            //    SharedFunctions.OpenForm(new frmStockConsumption(), this);
            //}

            //if (keyData == (Keys.Control | Keys.E))
            //{
            //    SharedFunctions.OpenForm(new frmStockAdjustments(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Shift | Keys.E))
            //{
            //    SharedFunctions.OpenForm(new frmStockAdjustment(), this);
            //}

            //if (keyData == (Keys.Control | Keys.F))
            //{
            //    SharedFunctions.OpenForm(new frmSuppliers(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Shift | Keys.F))
            //{
            //    SharedFunctions.OpenForm(new frmAddSupplier(), this);
            //}

            //if (keyData == (Keys.Control | Keys.O))
            //{
            //    frmInvoice f = new frmInvoice();
            //    f.MdiParent = this;
            //    f.Show();
            //}

            if (keyData == (Keys.Control | Keys.Shift | Keys.M))
            {
                MainMenue_Click(lblPharmacy, null);
            }
            if (keyData == Keys.F12)
            {
                OpenGame();
            }

            //if (keyData == (Keys.Control | Keys.H))
            //{
            //    SharedFunctions.OpenForm(new frmDrugReturn(), this);
            //}
            //#endregion short cuts for main screens
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void OpenGame()
        {

            string ex2 = Application.StartupPath + "\\";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = ex2 + "HWGame.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void ApplyUserRights()
        {
            //pharmacyToolStripMenuItem.Enabled = true;
            //patientsToolStripMenuItem.Enabled = true;
            //pereferencesToolStripMenuItem.Enabled = true;
            //reportsToolStripMenuItem.Enabled = true;
            //securityToolStripMenuItem.Enabled = true;
            lblPharmacy.Enabled = true;
            lblPatients.Enabled = true;
            lblSettings.Enabled = true;
            lblReports.Enabled = true;
            lblUsers.Enabled = true;
            lblPreferences.Enabled = true;
            btnDashboard.Enabled = true;
            lblshopactivities.Enabled = true;
            lblContactUs.Enabled = true;
        }
        private void patientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmPatients(), this);
        }
        private void pOSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            frmInvoice f = new frmInvoice();
            f.MdiParent = this;
            f.Show();
        }
        private void drugReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmDrugReturn(), this);
        }
        private void stockSuppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmSuppliers(), this);
        }
        private void createUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddUser(), this);
        }
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmUsers(), this);
        }
        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            //LoginUser();
        }
        private bool LoginUser()
        {
            frmLogin login = new frmLogin();
            login.BringToFront();
            login.StartPosition = FormStartPosition.CenterParent;
            login.ShowDialog();

            if (SharedVariables.LoggedInUser == null)
            {
                btnToggleLogin.Text = "Login";
                return false;
            }
            using (unitOfWork = new UnitOfWork())
            {
                AuditLogTask.CreateAuditLogInOut(unitOfWork, SharedVariables.LoggedInUser.UserName, AuditModule.Logged_In, AuditAction.logged_in);
                unitOfWork.Save();
            }
            CheckAuditLog();
            CheckLogout.Interval = 1000 * 5; //seconds
            CheckLogout.Enabled = true;
            CheckLogout.AutoReset = true;
            CheckLogout.Elapsed += new ElapsedEventHandler(LogoutUser_Elapsed);
            CheckLogout.Start();
            LoadDefaultItems();
            lblAppStatus.Text = "Welcome :: " + SharedVariables.LoggedInUser.UserName + "@" + " [Session Started at " + DateTime.Now + "]";
            lblUserName.Text = SharedVariables.LoggedInUser.UserName;
            btnCreateInvoice.Enabled = true;
            btnInvoiceHistory.Enabled = true;
            btnPurchaseOrders.Enabled = true;
            btnItems.Enabled = true;
            if (SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                adminSettingToolStripMenuItem.Visible = true;
                lblSettings.Visible = true;
            }
            else
            {
                lblSettings.Visible = false;
            }
            this.setSideBarVisble(true);
            ApplyUserRights();
            SharedFunctions.OpenForm(new frmInvoice(), this);
            btnToggleLogin.Text = "Logout";
            btnToggleLogin.Tag = "1";
            return true;
        }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            LogoutUser();
        }
        private void LogoutUser()
        {
            try
            {
                Properties.Settings.Default.IsSavePassword = false;
                Properties.Settings.Default.UserName = "";
                Properties.Settings.Default.Password = "";
                using (unitOfWork = new UnitOfWork())
                {
                    AuditLogTask.CreateAuditLogInOut(unitOfWork, SharedVariables.LoggedInUser.UserName, AuditModule.Logged_Out, AuditAction.logged_out);
                    unitOfWork.Save();
                }
                Application.Restart();
            }
            catch (Exception ex)
            {
            }
        }
        private void serialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmShowSerialKey(), this);
        }
        private void pharmacyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("View Pharmacy Report") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.Description.Equals("Super Admin")))
            {
                SharedFunctions.AccessDeniedMsg();
                return;
            }
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new Reports.Pharmacy.frmPharmacyReports(), this);
        }
        private void invoiceHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmInvoiceHistory(), this);
        }
        private void financialReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            if (!SharedFunctions.IsActionallowed("View Financial Reports") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                SharedFunctions.AccessDeniedMsg();
                return;
            }
            SharedFunctions.OpenForm(new Reports.Financial.frmFinancialReportsUI(), this);
        }
        private void proceduresListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new Pharmacy.UI.MenuBar.Preferences.ProcedureList(), this);
        }
        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new Pharmacy.UI.Reports.Accounts.frmAccountsReportsUI(), this);
        }
        private void procedureInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmProcedureInvoice2(), this);
        }
        private void adminSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new AdminSetting(), this);
        }
        private void consumeStock2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmStockConsumptions(), this);
        }
        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            LayoutMdi(MdiLayout.Cascade);
        }
        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            LayoutMdi(MdiLayout.TileVertical);
        }
        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
        private void ToggleStatusBarMenuStripItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            if (statusStrip.Visible == true)
            {
                statusStrip.Visible = false;
                ToggleStatusBarMenuStripItem.Text = "Show Status Bar";
            }
            else
            {
                statusStrip.Visible = true;
                ToggleStatusBarMenuStripItem.Text = "Hide Status Bar";
            }
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void departmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new MenuBar.Preferences.frmDepartments(), this);
        }
        private void wardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmWards(), this);
        }
        private void bedDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmBeds(), this);
        }
        private void loadDefaultDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void amountConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAmountConverter(), this);
        }
        private void admittedPAtientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAdmittedPatients(), this);
        }
        private void bedAllocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmBedAllocation(), this);
        }
        private int lastclicledIndex = -1;
        private void MainMenue_Click(object sender, EventArgs e)
        {
            if (this.isMenuCpsd)
            {
                HideShowLable(true);
                this.toggleSideMennu(true);
                this.isMenuCpsd = false;
               
            }

            //if (sender is Label)
            //{
            //    string senderName = ((Label)sender).Name;
            //    if (senderName == lblPharmacy.Name)
            //    {
            //        sender = MPharmacy;

            //    }
            //    else if (senderName == lblPatients.Name)
            //    {
            //        sender = MPatients;
            //    }
            //    else if (senderName == lblStocks.Name)
            //    {
            //        sender = MStocks;
            //    }
            //    else if (senderName == lblStockAdjustments.Name)
            //    {
            //        sender = MStockAdjustments;
            //    }
            //    else if (senderName == lblPurchases.Name)
            //    {
            //        sender = MPurchases;
            //    }
            //    else if (senderName == lblUsers.Name)
            //    {
            //        sender = MUsers;
            //    }
            //    else if (senderName == lblReports.Name)
            //    {
            //        sender = MReports;
            //    }
            //}
            ShowSubCategories(sender);
            if (SharedVariables.AdminPharmacySetting.DarkMode)
            {
                DarkMode.EnableDarkMode(this);
            }
        }
        private void ShowSubCategories(object sender)
        {
            //Panel p = (Panel)sender;
            Label p = (Label)sender;
            int Menuindex = pnlMenu.Controls.GetChildIndex(p);
            int subMenuIndex = pnlMenu.Controls.GetChildIndex(pnlSubMenu);
            PopulateSubMenu(p.Name);
            if (Menuindex == lastclicledIndex)
            {
                pnlSubMenu.Visible = !pnlSubMenu.Visible;
            }
            else
            {
                int localIndex = Menuindex;
                if (subMenuIndex > Menuindex)
                {
                    localIndex += 1;
                }
                pnlSubMenu.Visible = true;
                pnlMenu.Controls.SetChildIndex(pnlSubMenu, localIndex);
            }
            if (Menuindex > 0)
            {
                if (subMenuIndex < Menuindex)
                {
                    lastclicledIndex = Menuindex - 1;
                }
                else
                {
                    lastclicledIndex = Menuindex;
                }
            }
            else
            {
                lastclicledIndex = Menuindex;
            }
            Label l = new Label();
            l.Name = "lblActiveSymb";
            l.Width = 6;
            l.Height = 35;
            l.BackColor = Color.FromArgb(64, 0, 64);
            foreach (Control mItem in pnlMenu.Controls)
            {
                if (mItem is FlowLayoutPanel)
                {
                    continue;
                }
                foreach (Control c in mItem.Controls)
                {
                    if (c is Label)
                    {
                        if (c.Name == l.Name)
                        {
                            mItem.Controls.Remove(c);
                        }
                    }
                }
                mItem.BackColor = Color.FromArgb(64, 0, 64);
            }
            p.BackColor = Color.FromArgb(64, 0, 64);
            p.Controls.Add(l);
            l.BringToFront();
        }
        private void setSubmenuStyle(Label lbl, string lblText)
        {
            lbl.Text = lblText;
            lbl.Tag = "level2";
            lbl.Width = 200;
            lbl.Height = 32;
            lbl.Padding = new System.Windows.Forms.Padding(17, 0, 0, 0);
            lbl.BackColor = Color.FromArgb(64, 0, 64);
            lbl.ForeColor = Color.White;
            lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14, GraphicsUnit.Pixel);
            lbl.TextAlign = ContentAlignment.MiddleLeft;

            lbl.Click += new EventHandler(subMenu_Clcik);
            lbl.MouseEnter += new EventHandler(subMenu_MouseEnter);
            lbl.MouseLeave += new EventHandler(subMenu_MouseLeave);
        }

        private void subMenu_MouseEnter(object sender, EventArgs e)
        {
            ToggleSubMenuHoverStyle(sender, true);
        }
        private void subMenu_MouseLeave(object sender, EventArgs e)
        {
            ToggleSubMenuHoverStyle(sender, false);
        }
        private void ToggleMainMenuHoverStyle(object sender, bool isMouseEnter)
        {
            Label l = (Label)sender;
            if (isMouseEnter)
            {
                if (SharedVariables.AdminPharmacySetting.DarkMode)
                {
                    l.BackColor = Color.FromArgb(64, 0, 64);
                    if (!this.isMenuCpsd)
                    {
                        l.ImageAlign = ContentAlignment.MiddleRight;
                    }
                    l.BorderStyle = BorderStyle.None;
                }
                else
                {
                    l.BackColor = Color.FromArgb(64, 0, 64);
                    if (!this.isMenuCpsd)
                    {
                        l.ImageAlign = ContentAlignment.MiddleRight;
                    }
                    l.BorderStyle = BorderStyle.None;
                }
                //l.Font = new Font("Microsoft Sans Serif", 18, GraphicsUnit.Pixel);
            }
            else
            {
                if (SharedVariables.AdminPharmacySetting.DarkMode)
                {
                    l.BackColor = Color.FromArgb(64, 0, 64);
                    l.ForeColor = Color.White;
                    //l.BorderStyle = BorderStyle.FixedSingle;

                    l.ImageAlign = ContentAlignment.MiddleLeft;
                }
                else
                {
                    l.BackColor = Color.FromArgb(64, 0, 64);
                    l.ForeColor = Color.White;
                    //l.BorderStyle = BorderStyle.FixedSingle;

                    l.ImageAlign = ContentAlignment.MiddleLeft;
                }
                //l.Font = new Font("Microsoft Sans Serif", 16, GraphicsUnit.Pixel);
            }
        }
        private void ToggleSubMenuHoverStyle(object sender, bool isMouseEnter)
        {
            Label l = (Label)sender;
            if (isMouseEnter)
            {
                l.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
            }
            else
            {
                l.Font = new Font("Microsoft Sans Serif", 14, GraphicsUnit.Pixel);
            }
        }
        private void subMenu_Clcik(object sender, EventArgs e)
        {
            Label clickedOption = (Label)sender;
            switch (clickedOption.Name.ToLower())
            {
                //pharmacy sub menu
                case "lblitems": SharedFunctions.OpenForm(new frmAllItems(), this); break;
                case "lblmanagestock": SharedFunctions.OpenForm(new frmManageStocks(), this); break;
                case "lblstockreturn": SharedFunctions.OpenForm(new frmStockReturn(), this); break;
                case "lblpurchaseorders": SharedFunctions.OpenForm(new frmPurchaseOrders(), this); break;
                case "lblonlineintegrate": SharedFunctions.OpenForm(new frmOnlineIntegrate(), this); break;
                case "lblordervoucher": SharedFunctions.OpenForm(new frmOrderVoucher(), this); break;
                case "lblconsumestock": SharedFunctions.OpenForm(new frmStockConsumptions(), this); break;
                case "lblstores": SharedFunctions.OpenForm(new frmManageStores(), this); break;
                case "lblstockadjustment": SharedFunctions.OpenForm(new frmStockAdjustments(), this); break;
                case "lblstocksupplier": SharedFunctions.OpenForm(new frmSuppliers(), this); break;
                case "lblpos": SharedFunctions.OpenForm(new frmInvoice(), this); break;
                case "lblsr": SharedFunctions.OpenForm(new frmSalesReturn(), this); break;
                case "lblinvoicehistory": SharedFunctions.OpenForm(new frmInvoiceHistory(), this); break;
                case "lbldrugreturn": SharedFunctions.OpenForm(new frmDrugReturn(), this); break;
                case "lblmanufacturers": SharedFunctions.OpenForm(new frmManufacturers(), this); break;
                case "lbltemplates": SharedFunctions.OpenForm(new frmTemplates(), this); break;
                case "lblstockaudit": SharedFunctions.OpenForm(new frmStockAudits(), this); break;
                case "lblauditlog": SharedFunctions.OpenForm(new frmAuditLogs(), this); break;
                case "lblrackview": SharedFunctions.OpenForm(new frmRackView(), this); break;
                case "lblmissedsales": SharedFunctions.OpenForm(new frmMissedSales(), this); break;


                case "lblfinancial":
                    if (!SharedFunctions.IsActionallowed("View Financial Reports") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    SharedFunctions.OpenForm(new Reports.Financial.frmFinancialReportsUI(), this);
                    break;

                case "lblaccounts":
                    if (!SharedFunctions.IsActionallowed("View Pharmacy Report") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    SharedFunctions.OpenForm(new Reports.Accounts.frmAccountsReportsUI(), this);
                    break;
                case "lblpharmacy":
                    if (!SharedFunctions.IsActionallowed("View Pharmacy Report") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    SharedFunctions.OpenForm(new Reports.Pharmacy.frmPharmacyReports(), this);
                    break;
                case "lblproductreports":
                    if (!SharedFunctions.IsActionallowed("View Pharmacy Report") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                    {
                        SharedFunctions.AccessDeniedMsg();
                        return;
                    }
                    SharedFunctions.OpenForm(new Reports.Product.frmProductReports(), this);
                    break;

                //users sub menu
                case "lblusers": SharedFunctions.OpenForm(new frmUsers(), this); break;
                case "lbllogin": SharedFunctions.OpenForm(new frmLogin(), this); break;
                case "lbllogout": LogoutUser(); break;
                //case "lbladminsetting": SharedFunctions.OpenForm(new AdminSetting(), this); break;
                //case "lblloaddefaultitems": LoadDefaultItems(); break;
                case "lblremovedefaultitems": removeDefaultItems(); break;
                // main menu
                case "lblpatients": SharedFunctions.OpenForm(new frmPatients(), this); break;
                case "lblsettings": SharedFunctions.OpenForm(new AdminSetting(), this); break;
                case "lbldiscounts": SharedFunctions.OpenForm(new frmDiscounts(), this); break;
                case "lblexpensecategories": SharedFunctions.OpenForm(new frmExpenseCategories(), this); break;

                // shop activities
                case "lblposclosing": SharedFunctions.OpenForm(new Pharmacy.UI.MenuBar.ShopActivities.frmPOSCosing(), this); break;
                case "lblstoreclosing": SharedFunctions.OpenForm(new Pharmacy.UI.MenuBar.ShopActivities.frmStoreClosing(), this); break;
            }
        }

        private void PopulateSubMenu(string MainMenu)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            pnlSubMenu.Controls.Clear();
            if (MainMenu.ToLower().Equals("lblpharmacy"))
            {
                Label l1 = new Label();
                l1.Name = "lblitems";
                //l1.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l1.ImageAlign = ContentAlignment.MiddleLeft;
                this.setSubmenuStyle(l1, "Items");

                Label l2 = new Label();
                //l2.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l2.ImageAlign = ContentAlignment.MiddleLeft;
                l2.Name = "lblmanagestock";
                this.setSubmenuStyle(l2, "Manage Stock");

                Label l3 = new Label();
                //l3.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l3.ImageAlign = ContentAlignment.MiddleLeft;
                l3.Name = "lblstockreturn";
                l3.Visible = true;
                this.setSubmenuStyle(l3, "Stock Return");


                Label l18 = new Label();
                l18.Name = "lblonlineintegrate";
                l18.Visible = SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable;
                this.setSubmenuStyle(l18, "Online Order");

                Label l19 = new Label();
                l19.Name = "lblordervoucher";
                l19.Visible = SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable;
                this.setSubmenuStyle(l19, "Voucher Payments");

                Label l4 = new Label();
                //l4.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l4.ImageAlign = ContentAlignment.MiddleLeft;
                l4.Name = "lblpurchaseorders";
                this.setSubmenuStyle(l4, "Purchase Orders");

                Label l5 = new Label();
                //l5.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l5.ImageAlign = ContentAlignment.MiddleLeft;
                l5.Name = "lblconsumestock";
                l5.Visible = IsStockAdjustment();
                this.setSubmenuStyle(l5, "Consume Stock");

                Label l6 = new Label();
                //l6.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l6.ImageAlign = ContentAlignment.MiddleLeft;
                l6.Name = "lblstores";
                l6.Visible = false;//IsStoreEnable(); //----- Comment Code For Syncing
                this.setSubmenuStyle(l6, "Stores && Transfer Stock");

                Label l7 = new Label();
                //l7.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l7.ImageAlign = ContentAlignment.MiddleLeft;
                l7.Name = "lblstockadjustment";
                l7.Visible = IsStockAdjustment();
                this.setSubmenuStyle(l7, "Stock Adjustment");

                Label l8 = new Label();
                //l8.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l8.ImageAlign = ContentAlignment.MiddleLeft;
                l8.Name = "lblstocksupplier";
                this.setSubmenuStyle(l8, "Stock Supplier");

                Label l9 = new Label();
                //l9.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l9.ImageAlign = ContentAlignment.MiddleLeft;
                l9.Name = "lblpos";
                this.setSubmenuStyle(l9, "POS");

                Label l99 = new Label();
                //l99.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l99.ImageAlign = ContentAlignment.MiddleLeft;
                l99.Name = "lblsr";
                l99.Visible = true; //
                this.setSubmenuStyle(l99, "Sales Return");

                Label l10 = new Label();
                //l10.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l10.ImageAlign = ContentAlignment.MiddleLeft;
                l10.Name = "lblinvoicehistory";
                this.setSubmenuStyle(l10, "Invoice History");

                Label l11 = new Label();
                //l11.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l11.ImageAlign = ContentAlignment.MiddleLeft;
                l11.Name = "lbldrugreturn";
                this.setSubmenuStyle(l11, "Drug Return");

                Label l12 = new Label();
                //l12.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l12.ImageAlign = ContentAlignment.MiddleLeft;
                l12.Name = "lblmanufacturers";
                this.setSubmenuStyle(l12, "Manufacturers");

                Label l13 = new Label();
                //l13.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l13.ImageAlign = ContentAlignment.MiddleLeft;
                l13.Name = "lbltemplates";
                l13.Visible = SharedVariables.AdminPharmacySetting.IsPharmacyTemps;
                this.setSubmenuStyle(l13, "Templates");

                Label l14 = new Label();
                //l14.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l14.ImageAlign = ContentAlignment.MiddleLeft;
                l14.Name = "lblstockaudit";
                this.setSubmenuStyle(l14, "Stock Audit");

                Label l15 = new Label();
                //l15.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l15.ImageAlign = ContentAlignment.MiddleLeft;
                l15.Name = "lblrackview";
                l15.Visible = true; //----- Comment Code For Syncing
                this.setSubmenuStyle(l15, "Manage Racks");

                Label l16 = new Label();
                //l16.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                //l16.ImageAlign = ContentAlignment.MiddleLeft;
                l16.Name = "lblmissedsales";
                this.setSubmenuStyle(l16, "Missed Sales");

                Label l17 = new Label();
                using (unitOfWork = new UnitOfWork())
                {
                    l17.Name = "lblauditlog";
                    //l17.Image = ((System.Drawing.Image)(resources.GetObject("lblPharmacy.Image")));
                    //l17.ImageAlign = ContentAlignment.MiddleLeft;
                    l17.Visible = unitOfWork.UserRoleRepository.CheckAdminUserRole(SharedVariables.LoggedInUser.UserId);
                    this.setSubmenuStyle(l17, "Audit Logs");
                }

                pnlSubMenu.Controls.AddRange(new[] { l1, l2, l3, l4,l18, l19, l5, l6, l7, l8, l9, l99, l10, l11, l12, l13, l14, l15, l16, l17 });
            }
            else if (MainMenu.ToLower().Equals("lblpatients"))
            {
                SharedFunctions.OpenForm(new frmPatients(), this);
            }
            else if (MainMenu.ToLower().Equals("lblsettings"))
            {
                SharedFunctions.OpenForm(new AdminSetting(), this);
            }
            else if (MainMenu.ToLower().Equals("lblreports"))
            {
                Label l1 = new Label();
                l1.Name = "lblfinancial";
                this.setSubmenuStyle(l1, "Financial");
                Label l2 = new Label();
                l2.Name = "lblaccounts";
                this.setSubmenuStyle(l2, "Accounts");
                Label l3 = new Label();
                l3.Name = "lblpharmacy";
                this.setSubmenuStyle(l3, "RMS");
                Label l4 = new Label();
                l4.Name = "lblproductreports";
                this.setSubmenuStyle(l4, "Product Reports");
                pnlSubMenu.Controls.AddRange(new[] { l1, l2, l3, l4 });
            }
            else if (MainMenu.ToLower().Equals("lblusers"))
            {
                Label l1 = new Label();
                l1.Name = "lblusers";
                this.setSubmenuStyle(l1, "Users");
                //Label l2 = new Label();
                //l2.Name = "lbllogin";
                //this.setSubmenuStyle(l2, "Login");
                //Label l3 = new Label();
                //l3.Name = "lbllogout";
                //this.setSubmenuStyle(l3, "Logout");

                //Label l4 = new Label();
                //l4.Name = "lbladminsetting";
                //this.setSubmenuStyle(l4, "Admin Setting");

                //Label l5 = new Label();
                //l5.Name = "lblloaddefaultitems";
                //this.setSubmenuStyle(l5, "Load Default Items");
                if (!SharedVariables.AdminPractiseSetting.UserDeletedDefaultData)
                {
                    //Label l6 = new Label();
                    //l6.Name = "lblremovedefaultitems";
                    //this.setSubmenuStyle(l6, "Remove Default Items");
                    //pnlSubMenu.Controls.Add(l6);
                }
                pnlSubMenu.Controls.AddRange(new[] { l1, /*l2, l3, l4*/ });
            }
            else if (MainMenu.ToLower().Equals("lblpreferences"))
            {
                Label l1 = new Label();
                l1.Name = "lbldiscounts";
                this.setSubmenuStyle(l1, "Discounts");
                pnlSubMenu.Controls.AddRange(new[] { l1 });

                Label l2 = new Label();
                l2.Name = "lblExpenseCategories";
                this.setSubmenuStyle(l2, "Expense Categories");
                pnlSubMenu.Controls.AddRange(new[] { l1, l2 });
            }
            else if (MainMenu.ToLower().Equals("lblshopactivities"))
            {
                Label l1 = new Label();
                l1.Name = "lblposclosing";
                this.setSubmenuStyle(l1, "POS Closing");

                Label l2 = new Label();
                l2.Name = "lblstoreclosing";
                this.setSubmenuStyle(l2, "Store Closing");

                pnlSubMenu.Controls.AddRange(new[] { l1, l2 });
            }
            else if (MainMenu.ToLower().Equals("lblcontactus"))
            {
                frmContactUs f = new frmContactUs();
                f.StartPosition = FormStartPosition.Manual;
                f.StartPosition = FormStartPosition.CenterScreen;
                f.ShowDialog();
            }
        }
        private bool IsStoreEnable()
        {
            bool IsStockAdjust = false;
            try
            {
                if (SharedVariables.AdminPharmacySetting.IsEnableStores)
                {
                    IsStockAdjust = true;
                }
            }
            catch (Exception ex)
            {
                //showerror
            }
            return IsStockAdjust;
        }
        private bool IsStockAdjustment()
        {
            bool IsStockAdjust = true;
            try
            {
                if (!SharedFunctions.IsActionallowed("Adjust Stock"))
                {
                    IsStockAdjust = false;
                }
            }
            catch (Exception ex)
            {
                //showerror
            }
            return IsStockAdjust;
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
                    lblAppStatus.BackColor = Color.Yellow;
                    lblAppStatus.Text = "Please hold on, we clearing default data...";
                    lblAppStatus.BackColor = System.Drawing.SystemColors.Control;
                    if (unitOfWork.GeneralRepository.ClearDefaultData())
                    {
                        MessageBox.Show("Default data cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        //private void pnlAnimTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    this.BeginInvoke(new Action(() =>
        //        {
        //            if (this.isMenuPanelVisible)
        //            {
        //                while (pnlMenu.Width > 0)
        //                {
        //                    pnlMenu.Width -= 20;
        //                    if (pnlMenu.Width <= 0)
        //                    {
        //                        PnlAnimTimer.Stop();
        //                    }
        //                }
        //            }
        //            else
        //            {

        //                while (pnlMenu.Width < PnlMenuWidth)
        //                {
        //                    pnlMenu.Width += 20;
        //                    if (pnlMenu.Width >= PnlMenuWidth)
        //                    {
        //                        PnlAnimTimer.Stop();
        //                    }
        //                }
        //            }
        //        }));
        //    this.Invalidate();
        //}
        private void pnlMenu_Paint(object sender, PaintEventArgs e)
        {
            if (pnlMenu.VerticalScroll.Visible)
            {
                Console.WriteLine("scrool bar visible.");
            }
        }
        private void MPharmacy_MouseEnter(object sender, EventArgs e)
        {
            ToggleMainMenuHoverStyle(sender, true);
        }
        private void MPharmacy_MouseLeave(object sender, EventArgs e)
        {
            ToggleMainMenuHoverStyle(sender, false);
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void loadDefaultItemsData_FromExcelSheet(DataTable dtSource)
        {
            string suppName = "";
            using (unitOfWork = new UnitOfWork())
            {

                if (unitOfWork.SupplierRepository.SuppliersExist())
                {
                    goto LoadItems;
                }
                List<Supplier> sl = new List<Supplier>();
                foreach (DataRow r in dtSource.Rows)
                {
                    suppName = "";
                    suppName = r["Supplier"].ToString().Trim();
                    if (suppName == "")
                    {
                        continue;
                    }
                    Supplier objSupplier = sl.Where(sp => sp.Name.ToLower().Equals(suppName.ToLower())).FirstOrDefault();
                    if (objSupplier == null)
                    {
                        objSupplier =
                        new Supplier
                        {
                            Name = suppName,
                            Phone = "",
                            Address = "",
                            PrimaryContactPersonName = "",
                            PrimaryContactPersonPhone = "",
                            OpeningBalance = 0,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            IsActive = true,
                            IsNew = true,
                            IsUpdate = false,
                            IsSynced = false
                        };
                        sl.Add(objSupplier);
                    }
                    else
                    {

                    }
                }
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.SupplierRepository.InsertRange(sl);
                    unitOfWork.Save();
                }
            }

        LoadItems:
            List<Item> il = new List<Item>();
            using (unitOfWork = new UnitOfWork())
            {
                foreach (DataRow r in dtSource.Rows)
                {
                    Item objItem = new Item();
                    objItem.ItemName = r["Product Name"].ToString().Trim();
                    if (objItem.ItemName == "")
                    {
                        continue;
                    }
                    objItem.Barcode = r["Product Code"].ToString();
                    objItem.RetailPrice = Convert.ToDouble(r["Retail Price"]);
                    suppName = "";
                    suppName = r["Supplier"].ToString().Trim();
                    Supplier objSupplier = unitOfWork.SupplierRepository.GetSupplierByName(suppName.ToLower());
                    objItem.Rack = null;
                    objItem.Unit = "";
                    objItem.ReOrderingLevel = 0;
                    objItem.OpeningStock = 0;
                    objItem.ChecmicalName = "";
                    objItem.UnitCostPrice = 0;
                    objItem.IsNarcotic = false;
                    objItem.IsActive = true;
                    objItem.IsNew = true;
                    objItem.IsUpdate = false;
                    objItem.IsSynced = false;
                    objItem.CreateDate = DateTime.Now;
                    objItem.UpdateDate = DateTime.Now;
                    objItem.IsDefault = false;
                    objItem.Suppliers = new List<Supplier>();
                    objItem.Suppliers.Add(objSupplier);
                    il.Add(objItem);
                }
                unitOfWork.ItemRspository.InsertRange(il);
                unitOfWork.Save();
            }
            MessageBox.Show("Data loaded successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //tCheckFormsCollection.Stop();
            EmailRetryTimer.Stop();
            //stop_Notifications();

        }

        //bool pnlShown = false;
        private void brnDashboard_Click(object sender, EventArgs e)
        {
            //if (pnlShown)
            //{
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.UserRoleRepository.CheckAdminUserRole(SharedVariables.LoggedInUser.UserId))
                {
                    Form f = SharedFunctions.OpenForm(new Dashboard(), this);
                    f.WindowState = FormWindowState.Maximized;
                }
            }
            //pnlShown = false;
            //}
            //else
            //{
            //    this.pnlDashBoard.BringToFront();
            //    pnlShown = true;
            //}
            //foreach(frosm)
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void btnToggleLogin_Click(object sender, EventArgs e)
        {
            if (btnToggleLogin.Tag.ToString().Equals("0"))
            {
                /*if (LoginUser())
                {
                    btnToggleLogin.Text = "Logout";
                    btnToggleLogin.Tag = "1";
                }*/
            }
            else
            {
                LogoutUser();
            }
        }

        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmInvoice(), this.MdiParent);
        }

        private void btnInvoiceHistory_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmInvoiceHistory(), this);
        }

        private void btnPurchaseOrders_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmPurchaseOrders(), this);
        }

        private void btnDashboard_Enter(object sender, EventArgs e)
        {
            //btnDashboard.Image = 
        }

        private void btnDashboard_Leave(object sender, EventArgs e)
        {

        }

        private void btnSyncData_Click(object sender, EventArgs e)
        {
            try
            {
                if (!SharedFunctions.IsInternetAvailable())
                {
                    MessageBox.Show("Please check your internet connection and try again.", "No Internet", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                ServiceController sc = new ServiceController("PharmacySyncingService");
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.Start();
                    MessageBox.Show("Syncing process started, it will be auotmatically be completed in backend." + Environment.NewLine + " you can continue with your work.", "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (sc.Status == ServiceControllerStatus.StopPending)
                {
                    MessageBox.Show("Please wait for service to stop, then try again.", "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    sc.Start();
                    MessageBox.Show("Syncing process started, it will be auotmatically be completed in backend." + Environment.NewLine + " you can continue with your work.", "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while starting syncing process, please try again later.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnItems_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAllItems(), this);
        }
        public void toggleSideMennu(bool expand)
        {
            if (expand)
            {
                //btnTogMenu.Location = new Point(btnTogMenu.Location.X - 50, btnTogMenu.Location.Y);
                panel1.Width = SharedVariables.SideBarWidth;
                pnlMenu.Width = 230;
                pnlSideTopHeader.Width = 231;
                line1.Width = 230;

                pictureBox1.Location = new Point(pictureBox1.Location.X + 89, pictureBox1.Location.Y);
                lblTogBtn.Location = new Point(lblTogBtn.Location.X + 178, lblTogBtn.Location.Y);
                lblTogBtn.Image = Properties.Resources.MenuCollapse;
/*                pictureBox1.Width = 45;
                pictureBox1.Height = 65;*/
                lblUserName.Visible = true;

                pnlMenu.AutoScroll = true;
                pnlMenu.HorizontalScroll.Enabled = true;
                pnlMenu.HorizontalScroll.Visible = true;
            }
            else
            {
                panel1.Width = 50;
                pnlSideTopHeader.Width = 50;
                line1.Width = 50;

                pictureBox1.Location = new Point(pictureBox1.Location.X - 89, pictureBox1.Location.Y);
                lblTogBtn.Location = new Point(lblTogBtn.Location.X - 178, lblTogBtn.Location.Y);
                lblTogBtn.Image = Properties.Resources.MenuExpand;
                /*pictureBox1.Width = 45;
                pictureBox1.Height = 40;*/
                lblUserName.Visible = false;
                //pnlMenu.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                pnlMenu.Width = 44;
                pnlMenu.AutoScroll = false;
                pnlMenu.HorizontalScroll.Visible = false;
                //pnlMenu.HorizontalScroll.Maximum = 0;
                //pnlMenu.AutoScroll = false;
                //pnlMenu.VerticalScroll.Visible = false;
                //pnlMenu.AutoScroll = true;
            }
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams handleParam = new CreateParams();
        //        handleParam.ExStyle |= 0x02000000;
        //        return handleParam;
        //    }
        //}

        private void btnNotification_Click(object sender, EventArgs e)
        {
            this.LS_PageNo = 1;

            if (pnlNotisContainer.Tag.ToString() == "0")
            {
                //if (!bgW_LowStockItems.IsBusy)
                //{
                //    bgW_LowStockItems.RunWorkerAsync();
                //}
                this.index = 0;
                pnlNotifications.Controls.Clear();
                LoadLStockItems(LS_PageNo);
                pnlNotisContainer.Visible = true;
                pnlNotisContainer.Tag = "1";
                pnlNotisContainer.Location = new Point(btnNotification.Location.X - 110, btnNotification.Location.Y + 60);
            }
            else if(pnlNotisContainer.Tag.ToString() == "2")
            {
                this.index = 0;
                pnlNotifications.Controls.Clear();
                LoadLStockItems(LS_PageNo);
                pnlNotisContainer.Visible = true;
                pnlNotisContainer.Tag = "1";
                pnlNotisContainer.Location = new Point(btnNotification.Location.X - 110, btnNotification.Location.Y + 60);
            }
            else
            {
                pnlNotisContainer.Tag = "0";
                pnlNotisContainer.Visible = false;
            }
        }

        private void pnlNotifications_Paint(object sender, PaintEventArgs e)
        {

        }
        private void lblTogBtn_Click(object sender, EventArgs e)
        {
            if (this.isMenuCpsd)
            {
                toggleSideMennu(expand: true);
                HideShowLable(true);
               
            }
            else
            {
                toggleSideMennu(expand: false);
                HideShowLable(false);
                
            }
            this.isMenuCpsd = !this.isMenuCpsd;
        }

        public void HideShowLable(bool IsShow)
        {
            lblVersionContent.Visible = IsShow;
            lblVersion.Visible = IsShow;
        }




        IPagedList<LowStockNotiVM> LowStockNotis;
        List<LowStockNotiVM> LowStockNotisList;
        IPagedList<ExpiryNotiVM> ExpiryNotis;
        List<ExpiryNotiVM> ExpiryNotisList;
        /// <summary>
        /// page no for low stock items dynamic loading.
        /// </summary>
        int LS_PageNo = 1;
        /// <summary>
        /// page no for expiry items dynamic loading.
        /// </summary>
        int Exp_PageNo = 1;
        int pageSize = 20;
        // background workers
        int index = 0;
        private void bgW_LowStockItems_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        private void bgW_LowStockItems_RWComp(object sender, RunWorkerCompletedEventArgs e)
        {

            //this.bgW_LowStockItems.RunWorkerAsync();
        }

        private void pnlNotifications_Scroll(object sender, ScrollEventArgs e)
        {


            if (e.NewValue > e.OldValue)
            {
                NextItems();
            }
        }

        private void NextItems()
        {
            if (currNotiType == CurrentNotitype.LowStock)
            {
                LS_PageNo += 1;
                LoadLStockItems(LS_PageNo);
            }
            else
            {
                Exp_PageNo += 1;
                LoadExpItems(Exp_PageNo);
            }
        }

        private void pnlNotifications_MouseWheel(object sender, MouseEventArgs e)
        {
            NextItems();
        }
        int TotalNotifications = 0;
        private void LoadLStockItems(int PageNo)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LowStockNotis = unitOfWork.ItemRspository.GetLowStockNotis(PageNo, pageSize);
            }
            foreach (var i in LowStockNotis)
            {
                if (i.AvailableStock >= i.ReorderingLevel) { continue; }
                index++;
                Panel pnlNoti = new Panel();
                pnlNoti.SuspendLayout();
                pnlNoti.BorderStyle = BorderStyle.FixedSingle;
                pnlNoti.Location = new System.Drawing.Point(3, 3);
                pnlNoti.Name = "pnlNoti";
                pnlNoti.Size = new System.Drawing.Size(378, 52);
                pnlNoti.TabIndex = 0;
                pnlNoti.BackColor = Color.FromArgb(255, 157, 150);
                pnlNoti.Tag = i.ItemId;
                pnlNoti.Click += (sender, EventArgs) => { pnlNoti_Clicked(sender, EventArgs, i.ItemId); };// new EventHandler(pnlNoti_Clicked);
                pnlNoti.Cursor = Cursors.Hand;

                Label lblCount = new Label();
                lblCount.AutoSize = true;
                lblCount.Location = new System.Drawing.Point(2, 10);
                lblCount.Name = "lblCount";
                lblCount.Size = new System.Drawing.Size(38, 14);
                lblCount.TabIndex = 0;
                lblCount.Text = index.ToString();

                Label lblDescription = new Label();
                lblDescription.AutoSize = true;
                lblDescription.Location = new System.Drawing.Point(41, 10);
                lblDescription.Name = "lblDescription";
                lblDescription.Size = new System.Drawing.Size(38, 14);
                lblDescription.TabIndex = 0;
                lblDescription.Click += (sender, EventArgs) => { pnlNoti_Clicked(sender, EventArgs, i.ItemId); };
                lblDescription.Text = i.ItemName;

                Label lblNotiType = new Label();
                lblNotiType.AutoSize = true;
                lblNotiType.Location = new System.Drawing.Point(pnlNoti.Width - 60, 2);
                lblNotiType.Name = "lblType";
                lblNotiType.Size = new System.Drawing.Size(38, 14);
                lblNotiType.TabIndex = 0;
                lblNotiType.Click += (sender, EventArgs) => { pnlNoti_Clicked(sender, EventArgs, i.ItemId); };
                lblNotiType.Text = "Low Stock";

                Label lblTime = new Label();
                lblTime.AutoSize = true;
                lblTime.Location = new System.Drawing.Point(41, 26);
                lblTime.Name = "lblTime";
                lblTime.Size = new System.Drawing.Size(38, 14);
                lblTime.TabIndex = 0;
                lblTime.Text = DateTime.Now.ToString();
                lblTime.Click += (sender, EventArgs) => { pnlNoti_Clicked(sender, EventArgs, i.ItemId); };
                pnlNoti.Controls.Add(lblTime);

                pnlNoti.Controls.Add(lblCount);
                pnlNoti.Controls.Add(lblDescription);
                //pnlNoti.ResumeLayout(false);
                pnlNoti.Controls.Add(lblNotiType);
                this.pnlNotifications.Controls.Add(pnlNoti);
            }
            //this.pnlNotifications.ResumeLayout(true);
        }
        private int GetLStockItemsCount()
        {
            int i = 0;
            using (unitOfWork = new UnitOfWork())
            {
                i = unitOfWork.ItemRspository.GetLowStockNotiscount();
            }

            return i;
        }
        private int LoadExpItemscount()
        {
            int i = 0;
            int ExpiryPeriodCount = SharedVariables.AdminPharmacySetting.ExpiryPeriod;

            if (ExpiryPeriodCount <= 0)
            {
                return 0;
            }

            DateTime ExpNotiPeriod = DateTime.Now;
            if (SharedVariables.AdminPharmacySetting.ExpiryPeriodUnit == (int)SharedVariables.ExpiryPeriodUnit.Days)
            {
                ExpNotiPeriod = DateTime.Now.AddDays(ExpiryPeriodCount);
            }
            else
            {
                ExpNotiPeriod = DateTime.Now.AddMonths(ExpiryPeriodCount);
            }


            using (unitOfWork = new UnitOfWork())
            {
                i = unitOfWork.ItemRspository.GetExpiryNotiscount(ExpNotiPeriod);
            }
            return i;
            
        }
        private void LoadExpItems(int PageNo)
        {
            int ExpiryPeriodCount = SharedVariables.AdminPharmacySetting.ExpiryPeriod;

            if (ExpiryPeriodCount <= 0)
            {
                return;
            }

            DateTime ExpNotiPeriod = DateTime.Now;
            if (SharedVariables.AdminPharmacySetting.ExpiryPeriodUnit == (int)SharedVariables.ExpiryPeriodUnit.Days)
            {
                ExpNotiPeriod = DateTime.Now.AddDays(ExpiryPeriodCount);
            }
            else
            {
                ExpNotiPeriod = DateTime.Now.AddMonths(ExpiryPeriodCount);
            }


            using (unitOfWork = new UnitOfWork())
            {
                ExpiryNotis = unitOfWork.ItemRspository.GetExpiryNotis(ExpNotiPeriod, Exp_PageNo, pageSize);
            }

            int daysExpReaching = 0;
            if (ExpiryNotis != null)
            {
                TotalNotifications += ExpiryNotis.Count;
            }

            //this.pnlNotifications.SuspendLayout();
            foreach (var i in ExpiryNotis)
            {
                index++;
                Panel pnlNoti = new Panel();
                pnlNoti.SuspendLayout();
                pnlNoti.BorderStyle = BorderStyle.FixedSingle;
                pnlNoti.Location = new System.Drawing.Point(3, 3);
                pnlNoti.Name = "pnlNoti";
                pnlNoti.Size = new System.Drawing.Size(378, 52);
                pnlNoti.TabIndex = 0;
                pnlNoti.BackColor = Color.FromArgb(254, 233, 128);
                pnlNoti.Click += (sender, EventArgs) => { pnlNotiPurchaseOrder_Clicked(sender, EventArgs, i.ItemId); };// new EventHandler(pnlNoti_Clicked);
                pnlNoti.Tag = i.ItemId;
                pnlNoti.Cursor = Cursors.Hand;

                Label lblCount = new Label();
                lblCount.AutoSize = true;
                lblCount.Location = new System.Drawing.Point(2, 10);
                lblCount.Name = "lblCount";
                lblCount.Size = new System.Drawing.Size(38, 14);
                lblCount.TabIndex = 0;
                lblCount.Text = index.ToString();

                Label lblDescription = new Label();
                lblDescription.AutoSize = true;
                lblDescription.Location = new System.Drawing.Point(41, 10);
                lblDescription.Name = "lblDescription";
                lblDescription.Size = new System.Drawing.Size(38, 14);
                lblDescription.TabIndex = 0;
                lblDescription.Text = i.ItemName;

                Label lblNotiType = new Label();
                lblNotiType.AutoSize = true;
                lblNotiType.Location = new System.Drawing.Point(pnlNoti.Width - 150, 2);
                lblNotiType.Name = "lblType";
                lblNotiType.Size = new System.Drawing.Size(38, 14);
                lblNotiType.TabIndex = 0;

                daysExpReaching = (i.BatchExp - DateTime.Now).Days;
                lblNotiType.Text = "Expiry Reaching in " + daysExpReaching + (daysExpReaching == 1 ? " Day" : " Days");

                // 
                // lblTime
                // 
                Label lblTime = new Label();
                lblTime.AutoSize = true;
                lblTime.Location = new System.Drawing.Point(41, 26);
                lblTime.Name = "lblTime";
                lblTime.Size = new System.Drawing.Size(38, 14);
                lblTime.TabIndex = 0;
                string expDate = i.BatchExp.ToString("yyyy-MMM-dd");
                lblTime.Text = "Batch: " + i.BatchNo + " => Expiry : " + expDate;
                pnlNoti.Controls.Add(lblTime);

                pnlNoti.Controls.Add(lblCount);
                pnlNoti.Controls.Add(lblDescription);
                //pnlNoti.ResumeLayout(false);
                pnlNoti.Controls.Add(lblNotiType);
                this.pnlNotifications.Controls.Add(pnlNoti);
            }
            //this.pnlNotifications.ResumeLayout(true);
        }
        
        private void pnlNotiPurchaseOrder_Clicked(object sender, EventArgs EventArgs, int ItemId)
        {
            try
            {
                if (ItemId > 0)
                {
                    new frmPurchaseOrder(ItemId, true).Show();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        public void pnlNoti_Clicked(object sender, EventArgs e, int ItemId)
        {
            frmAddStock f = new frmAddStock(ItemId);
            f.Show();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblLowStock_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            currNotiType = CurrentNotitype.LowStock;
            index = 0;
            LS_PageNo = 1;
            pnlNotifications.Controls.Clear();
            LoadLStockItems(this.LS_PageNo);
        }

        private void lblExpiry_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            currNotiType = CurrentNotitype.Expiry;
            index = 0;
            Exp_PageNo = 1;
            pnlNotifications.Controls.Clear();
            LoadExpItems(this.Exp_PageNo);

        }

        private void lblSubsNoti_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pnlNotifications.Controls.Clear();
            try
            {
                int days = 0;
                DateTime dtExp = new DateTime();
                using (unitOfWork = new UnitOfWork())
                {
                    dtExp = DateTime.Parse(CryptopEngine.Decrypt(unitOfWork.HwDataRepository.GetSystemExpiryDate(), SharedVariables.Salt));
                    days = (dtExp - DateTime.Now).Days;
                }
                Panel pnlNoti = new Panel();
                pnlNoti.BorderStyle = BorderStyle.FixedSingle;
                pnlNoti.Location = new System.Drawing.Point(12, 3);
                pnlNoti.Name = "pnlNoti";
                pnlNoti.Size = new System.Drawing.Size(395, 200);
                pnlNoti.TabIndex = 0;
                pnlNoti.BackColor = Color.FromArgb(108, 212, 252);
                pnlNoti.Tag = 0;
                pnlNoti.Cursor = Cursors.Hand;

                Label lblDescription = new Label();
                lblDescription.Font = new Font("Arial", 11);
                lblDescription.Location = new System.Drawing.Point(5, 90);
                lblDescription.Name = "lblDescription";
                //lblDescription.Size = new System.Drawing.Size(38, 14);
                lblDescription.MaximumSize = new Size(pnlNoti.Width - 10, 0);
                lblDescription.AutoSize = true;
                lblDescription.TabIndex = 0;
                if (days > 60)
                {
                    lblDescription.ForeColor = Color.Green;
                }
                else if (days > 30 && days < 60)
                {
                    lblDescription.ForeColor = Color.Yellow;
                }
                else
                {
                    lblDescription.ForeColor = Color.Red;
                }
                lblDescription.Text = "Your subscription will expire in " + days + "Day(s) on " + dtExp.ToString("yyyy-MMM-dd");


                //// 
                //// lblTime
                //// 
                //Label lblTime = new Label();
                //lblTime.AutoSize = true;
                //lblTime.Location = new System.Drawing.Point(5, 34);
                //lblTime.Name = "lblTime";
                //lblTime.Size = new System.Drawing.Size(38, 14);
                //lblTime.TabIndex = 0;
                //lblTime.Text = "Your Subscription started on " + dtExp.ToString("yyyy-MMM-dd");
                //pnlNoti.Controls.Add(lblTime);

                pnlNoti.Controls.Add(lblDescription);
                //pnlNoti.Controls.Add(lblTime);
                this.pnlNotifications.Controls.Add(pnlNoti);
            }
            catch (Exception ex)
            {

            }
        }
        public int chcking = 0;
        private void BackupDB(string Device, string FileName , string Database)
        {
            
        L:
            this.Invoke(new Action(() =>
            {
                ProgressBar.Visible = true;
                ProgressBar.Value = 0;
            

                ProgressBar.Value = 10;
            }));
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            this.Invoke(new Action(() =>
            { 
                ProgressBar.Value = 20;
            }));
            //string location = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string database = Database;
            try
            {
                this.Invoke(new Action(() =>
                {
                    ProgressBar.Value = 30;
                }));
                if (Device == string.Empty)
                {
                    return;
                }
                else
                {
                    this.Invoke(new Action(() =>
                    {
                        ProgressBar.Value = 40;
                    }));
                    string cmd = "BACKUP DATABASE [" + database + "] TO DISK='" + Device + "'";
                    this.Invoke(new Action(() =>
                    {
                        ProgressBar.Value = 50;
                    }));
                    using (SqlCommand command = new SqlCommand(cmd, con))
                    {
                        this.Invoke(new Action(() =>
                        {
                            ProgressBar.Value = 60;
                        }));
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }
                        int a = command.ExecuteNonQuery();
                        this.Invoke(new Action(() =>
                        {
                            ProgressBar.Value = 80;
                        }));
                        if (a != 0)
                        {
                            this.Invoke(new Action(() =>
                            {
                                ProgressBar.Value = 90;
                            }));
                            con.Close();

                          
                        }
                       
                    }
                    chcking = 0;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot open backup device"))
                {


                    if (chcking == 0)
                    {
                        UpdateService();
                        timer1.Stop();
                        chcking = 1;
                        goto L;
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
                object invokeResult = service.InvokeMethod("Change", wmiParams);
            }
        }
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public bool checkfornet()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }
        private string StartKeyLogger(long practiceId, string desktopname, string Ipaddress, string desktopid)
        {
            try
            {
                string URL = "https://desktop.healthwire.pk";
                string LoggerURl = "/api/v4/users/desktop_key_logger_info";
                string resultContent;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var content = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string, string>("practice_id", practiceId.ToString()),
                new KeyValuePair<string, string>("practice_name", desktopname),
                new KeyValuePair<string, string>("ip_address", Ipaddress),
                new KeyValuePair<string, string>("desktop_id", desktopid)
                });
                    if (practiceId == -1 || practiceId <= 0)
                    {
                        Random r = new Random();
                        practiceId = r.Next(499, 999);
                    }
                    LoggerURl = LoggerURl + "?practice_id=" + practiceId.ToString() + "&practice_name=" + desktopname + "&ip_address=" + Ipaddress + "&address=" + SharedVariables.AdminPractiseSetting.Address;
                    var result = client.PostAsync(LoggerURl, null).Result;
                    resultContent = result.Content.ReadAsStringAsync().Result;
                    return resultContent;
                }
            }
            catch (Exception) { timer3.Stop();  return null; }
        }
        //string mainURL = "https://desktop.healthwire.pk";
        //string loginURL = "/api/v4/users/login.json";
        //public string Login()
        //{
        //    string resultContent;
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(mainURL);
        //        ServicePointManager.Expect100Continue = true;
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //        var content = new FormUrlEncodedContent(new[]{
        //        new KeyValuePair<string, string>("user[email]", SharedVariables.AdminPharmacySetting.OnlineWebUsername),
        //        new KeyValuePair<string, string>("user[password]", SharedVariables.AdminPharmacySetting.OnlineWebPassword)
        //        });
        //        var result = client.PostAsync(loginURL, content).Result;
        //        resultContent = result.Content.ReadAsStringAsync().Result;
        //        return resultContent;
        //    }
        //}
        //public void getGredential()
        //{
        //    string jsonLogin = Login();
        //    Login login = new Login();
        //    login = JsonConvert.DeserializeObject<Login>(jsonLogin);
        //    if (login.status.ToLower().Equals("success"))
        //    {
        //        SharedVariables.Email = SharedVariables.AdminPharmacySetting.OnlineWebUsername;
        //        SharedVariables.AuthToken = login.auth_token;
        //        var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
        //        string ipAddress = "";
        //        if (Dns.GetHostAddresses(Dns.GetHostName()).Length > 0)
        //        {
        //            ipAddress = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
        //        }
        //        StartKeyLogger(SharedVariables.PracticeId, SharedVariables.PharmacyName, ipAddress, macAddr);
        //    }
        //    else
        //    {
        //        SharedVariables.Email = "";
        //        SharedVariables.AuthToken = "";
        //    }
        //}
        private void keylogger()
        {
           /* var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
            string ipAddress = "";
            if (Dns.GetHostAddresses(Dns.GetHostName()).Length > 0)
            {
                ipAddress = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
            }
            
            StartKeyLogger(SharedVariables.AdminPractiseSetting.PracticeId, SharedVariables.AdminPractiseSetting.Name, ipAddress, macAddr);*/
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
           // backupdb()
        }
        public void backupdb()
        {
            //keylogger();
            //Properties.Settings.Default.Date = DateTime.Now.ToString();
            /*Properties.Settings.Default.Date = null;
            Properties.Settings.Default.Save();
            */
            Thread backgroundThread = new Thread(() =>
            {
                string Date = Properties.Settings.Default.Date;
                if (!SharedVariables.AdminPharmacySetting.EnableAutoBackup)
                {
                    timer1.Stop();
                    return;
                }
                if (true)
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        AdminPharmacySetting obj = unitOfWork.AdminPharmacySettingRepository.GetById(SharedVariables.AdminPharmacySetting.AdminPharmacySettingId);
                        if (obj != null)
                        {
                            if (SharedVariables.AdminPharmacySetting != null)
                            {
                                if (SharedVariables.AdminPharmacySetting.EnableAutoBackup)
                                {
                                    BackupDB(obj.Lcoation, obj.FileName, Shared.GetConnectionBuilder().InitialCatalog);
                                    obj.LastBackupDate = DateTime.Now.ToString();
                                    Properties.Settings.Default.Date = DateTime.Now.ToShortDateString();
                                    Properties.Settings.Default.Save();
                                    this.Invoke(new Action(() =>
                                    {
                                        ProgressBar.Visible = false;
                                    }));
                                }
                            }
                            using (unitOfWork = new UnitOfWork())
                            {
                                unitOfWork.AdminPharmacySettingRepository.Update(obj);
                                unitOfWork.Save();
                                SharedVariables.AdminPharmacySetting = obj;
                                this.Invoke(new Action(() =>
                                {
                                    ProgressBar.Visible = false;
                                }));
                            }
                        }
                    }
                }
                else
                {
                    /*this.Invoke(new Action(() =>
                    {
                        timer1.Stop();
                    }));*/

                }
            });

            // Start the background thread
            backgroundThread.Start();
        }
        private void checkforupdate()
        {
            string ex1 = "C:\\";
            string ex2 = Application.StartupPath +"\\";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = ex2 + "PMSUpdater.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch(Exception ex)
            {
                timer2.Stop();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            checkforupdate();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                string time = SharedVariables.AdminPharmacySetting.AutoUpdateTime;
                string CurrentDate = DateTime.Now.ToShortDateString();
                int day = ((int)DateTime.Now.DayOfWeek == 0) ? 7 : (int)DateTime.Now.DayOfWeek;
                int saveday = Convert.ToInt32(Properties.Settings.Default.Day);
                bool dayofweek = false;
                if (day < saveday)
                {
                    dayofweek = true;
                }
                string Month = DateTime.Now.Month.ToString();
                if (time == "Daily")
                {
                    if (CurrentDate != Properties.Settings.Default.UpdateDate)
                    {
                        checkforupdate();
                        Properties.Settings.Default.UpdateDate = CurrentDate;
                        timer2.Stop();
                        return;
                    }
                }
                else if (time == "Weekly")
                {
                    if (dayofweek)
                    {
                        checkforupdate();
                        Properties.Settings.Default.UpdateDate = CurrentDate;
                        timer2.Stop();
                        return;
                    }
                }
                else if (time == "Monthly")
                {
                    if (Month != Properties.Settings.Default.Month)
                    {
                        checkforupdate();
                        Properties.Settings.Default.UpdateDate = CurrentDate;
                        timer2.Stop();
                        return;
                    }
                }
                else
                {
                    timer2.Stop();
                    return;
                }
            }
            catch { timer2.Stop();
            return;
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.LoggerDate != DateTime.Now.ToShortDateString())
                {
                    if (btnToggleLogin.Text != "Login")
                    {
                        Thread Logger = new Thread(keylogger);
                        Logger.Start();
                        Properties.Settings.Default.LoggerDate = DateTime.Now.ToShortDateString();
                    }
                }
                else
                {
                    timer3.Stop();
                }
            }
            catch(Exception){}
        }
        private void Notify()
        {
            int i = GetLStockItemsCount();
            int j = LoadExpItemscount();
            int noti = i + j;
            if (noti > 1)
            {
                btnNotification.Text = "N (" + noti + ")";
                btnNotification.AutoSize = true;
                timer4.Start();
            }
            else
            {
                btnNotification.Text = "N";
                btnNotification.BackColor = Color.FromArgb(224, 224, 224);
                timer4.Stop();
            }
        }
        private void NotificationTimer_Tick(object sender, EventArgs e)
        {
            Notify();
        }
        private void exp()
        {
            int days = 0;
            DateTime dtExp = new DateTime();
            using (unitOfWork = new UnitOfWork())
            {
                dtExp = DateTime.Parse(CryptopEngine.Decrypt(unitOfWork.HwDataRepository.GetSystemExpiryDate(), SharedVariables.Salt));
                days = (dtExp - DateTime.Now).Days;
            }
            if (days < 15)
            {
                if (days > 1)
                {
                    lblExpiryAlert.Text = "Your Subscription will Expire after " + days + " days,\nSubscribe Now to Continue Using Software.";
                    lblExpiryAlert.TextAlign = ContentAlignment.MiddleLeft;
                    lblExpiryAlert.Visible = true;
                }
                else if(days == 1)
                {
                    lblExpiryAlert.Text = "Your Subscription will Expire after " + days + " day,\nSubscribe Now to Continue Using Software.";
                    lblExpiryAlert.TextAlign = ContentAlignment.MiddleLeft;
                    lblExpiryAlert.Visible = true;
                }
                else
                {
                    lblExpiryAlert.Text = "Your Subscription Expired \n Subscribe Now to Continue Using Software.";
                    lblExpiryAlert.TextAlign = ContentAlignment.MiddleLeft;
                    lblExpiryAlert.Visible = true;
                }
            }
            else
            {
                lblExpiryAlert.Visible = false;
            }
        }
        private void Blink(Button b)
        {
            if(b.BackColor == Color.FromArgb(64, 0, 64))
            {
                b.BackColor = Color.White;
                b.ForeColor = Color.Black;
                
            }
            else if(b.BackColor == Color.White)
            {
                b.BackColor = Color.FromArgb(64, 0, 64);
                b.ForeColor = Color.White;
            }
            else
            {
                b.BackColor = Color.FromArgb(64, 0, 64);
                b.ForeColor = Color.White;
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            Blink(btnNotification);
        }

        private void pnlSideTopHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblshopactivities_MouseEnter(object sender, EventArgs e)
        {
            ToggleMainMenuHoverStyle(sender, true);
        }

        private void lblshopactivities_MouseLeave(object sender, EventArgs e)
        {
            ToggleMainMenuHoverStyle(sender, false);
        }

        private void lblContactUs_MouseEnter(object sender, EventArgs e)
        {
            ToggleMainMenuHoverStyle(sender, true);
        }

        private void lblContactUs_MouseLeave(object sender, EventArgs e)
        {
            ToggleMainMenuHoverStyle(sender, false);
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            if (!ProductActivation.CheckSystemExpiration())
            {
                new frmPackages().ShowDialog();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            new frmReportIssue().ShowDialog();
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            ToggleMainMenuHoverStyle(sender, true);
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            ToggleMainMenuHoverStyle(sender, false);
        }

        private void frmMain_Click(object sender, EventArgs e)
        {
            
        }

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            //LabelBlinker.BlinkLabel(lblNotifiylabel);
        }

        private async void timer5_Tick_1Async(object sender, EventArgs e)
        {
            
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new DataSyncingUI(), this);
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            ToggleMainMenuHoverStyle(sender, true);
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            ToggleMainMenuHoverStyle(sender, false);
        }
        public int updateItemStatus(string qry)
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand com = new SqlCommand(qry, con);
            int i = com.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public void getApprovedItems()
        {
            try
            {
                if (SharedFunctions.IsInternetAvailable())
                {
                    int Notifycount = 0;
                    ItemsAws items = new ItemsAws();
                    List<Item> checkItem = new List<Item>();
                    Stock objStock = new Stock();
                    checkItem = obj.GetItemValues();

                    foreach (var list in checkItem)
                    {
                        DataTable ct = get("select * from Items where ItemName='" + list.ItemName + "'");
                        if (ct.Rows.Count > 0)
                        {
                        }
                        else
                        {
                            if (list.RequestById == SharedVariables.AdminPharmacySetting.PracticeId)
                            {
                                list.ItemID = 0;
                                Notifycount++;
                                this.InsertItemOnline(list);

                                MessageBox.Show("Your request to add (" + list.ItemName + ") in pharmacy has been processed", "Item Approved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                list.ItemID = 0;
                                this.InsertItemOnline(list);
                            }
                        }
                    }

                    //refresh itemPage
                    frmAllItems frm = new frmAllItems(true);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            #region ConnectionString
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();

            //string query = "select * from postgres_production_public.items where practice_id = " + SharedVariables.AdminPractiseSetting.PracticeId;

            ////Create the ODBC connection string.
            //string connString = "DSN=check; Driver={Amazon Redshift (x64)};" +
            //        String.Format("Server={0};Database={1};" +
            //        "UID={2};PWD={3};Port={4};SSL=true;Sslmode=Require",
            //        SharedVariables.server, SharedVariables.DBName, SharedVariables.masterUsername,
            //        SharedVariables.masterUserPassword, SharedVariables.port);

            //OdbcConnection conn = new OdbcConnection(connString);
            //conn.Open();

            //string sql = query;
            //OdbcDataAdapter da = new OdbcDataAdapter(sql, conn);
            //da.Fill(ds);
            //dt = ds.Tables[0];

            //    foreach (DataRow row in dt.Rows)
            //    {
            //        //string name = row["name"].ToString();
            //        //string description = row["description"].ToString();
            //        //MessageBox.Show("");
            //        //int j = updateItemStatus("Update Items set IsApproved=" + Quantity + "where Item_ItemID=" + ItemID);          
            //    }

            //conn.Close();
            #endregion
        }

        public void InsertItemOnline(Item add)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Item objItem = new Item();
                        Stock objStock = new Stock();

                        add.Unit = "Pack";
                        add.IsNew = true;
                        add.CreateDate = DateTime.Now;
                        add.IsSyncable = true;
                        add.UpdateDate = DateTime.Now;
                        add.CategoryId = Convert.ToInt64(add.CategoryId);
                        add.ManufacturerId = Convert.ToInt32(add.ManufacturerId);
                        add.IsActive = true;
                        add.IsApproved = 0;
                        add.NoOfStrips = add.NoOfStrips;
                        add.Unique_itemId = add.Unique_itemId;
                        //add.ItemOrderName = add.ItemName;

                        add.IsSyncable = false;
                        add.IsFileItem = true;

                        add.Category = add.CategoryId == 0 ? unitOfWork.CategoryRepository.GetById(0) : unitOfWork.CategoryRepository.GetById(add.CategoryId);
                        add.Manufacturer = add.ManufacturerId == 0 ? unitOfWork.ManufacturerRepository.GetById(0) : unitOfWork.ManufacturerRepository.GetById(add.ManufacturerId);
                        add.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        add.Rack = add.RackId == 0 ? unitOfWork.RackRepository.GetById(0) : unitOfWork.RackRepository.GetById(add.RackId);

                        unitOfWork.ItemRspository.Insert(add);

                        //if (add.OpeningStock > 0)
                        //{
                        //    //------------ Audit Log
                        //    objStock = MakeOpeningStockEntry(add);
                        //    AuditLogTask.CreateAuditLog(unitOfWork, "Document# " + SharedFunctions.StringToInt32(objStock.DocumentNo).ToString(), AuditModule.Stock, AuditAction.created);
                        //}
                        //------------ Audit Log
                        AuditLogTask.CreateAuditLog(unitOfWork, add.ItemName, AuditModule.Item, AuditAction.created);
                        unitOfWork.Save();

                        int rackId = SharedFunctions.StringToInt(add.RackId);
                        if (rackId > 0)
                        {
                            RackItem model = new RackItem();
                            model.Rack = unitOfWork.RackRepository.GetById(rackId);
                            model.Item = objItem; //item;
                            model.CreatedAt = DateTime.Now;
                            model.UpdatedAt = DateTime.Now;
                            model.IsActive = true;
                            model.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                            unitOfWork.RackRepository.InsertRackItem(model);
                            unitOfWork.Save();
                        }
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    throw new Exception(ex.Message);
                }
            }
        }

        private Stock MakeOpeningStockEntry(Item objItem)
        {
            Stock objStock = new Stock();
            objStock.Supplier = null;
            objStock.SupplierInvoiceNo = "";
            objStock.ImagePath = null;
            objStock.StockDate = DateTime.Now;
            objStock.CreateDate = DateTime.Now;
            objStock.UpdateDate = DateTime.Now;
            objStock.IsActive = true;
            objStock.IsNew = true;
            objStock.SupplierIvoiceDate = DateTime.Now;
            objStock.DocumentNo = unitOfWork.StockRepository.GetNewDocumentNumber();
            objStock.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
            objStock.StockItems = new List<StockItem>();
            StockItem objStockItem = new StockItem();

            objStockItem.Batch = new Batch();
            objStockItem.Batch.BatchName = "Opening Stock";
            objStockItem.Batch.Expiry = null;
            objStockItem.Batch.CreatedAt = DateTime.Now;
            objStockItem.Batch.UpdatedAt = DateTime.Now;
            objStockItem.Batch.IsNew = true;
            objStockItem.Batch.IsActive = true;
            objStockItem.Quantity = objItem.OpeningStock;
            objStockItem.UnitCost = objItem.UnitCostPrice;
            objStock.GrandTotal = objStockItem.TotalCost = objStockItem.NetValue = objStockItem.Quantity * objStockItem.UnitCost;
            objStockItem.RetailPrice = objItem.RetailPrice;
            objStockItem.Item = objItem;// unitOfWork.ItemRspository.GetById(ItemId);
            objStockItem.CreatedAt = DateTime.Now;
            objStockItem.UpdatedAt = DateTime.Now;
            objStockItem.IsActive = true;
            objStock.StockItems.Add(objStockItem);
            unitOfWork.StockRepository.Insert(objStock);
            return objStock;
        }

        private void FillObjectOnline(Item objItem)
        {
            objItem.ItemName = objItem.ItemName;
            objItem.Unit = "Pack";
            objItem.NoOfStrips = Convert.ToInt32(objItem.NoOfStrips);
            int convUnit = Convert.ToInt32(objItem.ConversionUnit);
            objItem.ConversionUnit = convUnit;
            objItem.RetailPrice = Math.Round(SharedFunctions.StringToDouble(objItem.RetailPrice), 3);
            objItem.UnitCostPrice = Math.Round(SharedFunctions.StringToDouble(objItem.UnitCostPrice), 3);
            objItem.IsNarcotic = objItem.IsNarcotic;
            //if (this.ItemId <= 0)
            //{
            objItem.IsActive = true;
            objItem.IsNew = true;
            objItem.CreateDate = DateTime.Now;
            objItem.IsSyncable = true;
            //}
            objItem.UpdateDate = DateTime.Now;
            objItem.CategoryId = Convert.ToInt64(objItem.CategoryId);
            objItem.ManufacturerId = Convert.ToInt32(objItem.ManufacturerId);
        }

        public void getOnlineOrderCount()
        {
            try
            {
                if (SharedFunctions.IsInternetAvailable())
                {
                    OrderAws Order = new OrderAws();
                    frmOnlineIntegrate online = new frmOnlineIntegrate();
                    OrderOnlineAws Order1 = new OrderOnlineAws();
                    List<OrderOnlineAws> checkItem1 = new List<OrderOnlineAws>();
                    List<OrderAws> checkItem = new List<OrderAws>();
                    int count = obj.GetOrderItemsCount();

                    //get Global Orders Count
                    DataTable gCount = get("Select * from AdminPharmacySettings");
                    int gCount1 = Convert.ToInt32(gCount.Rows[0]["OnlineGlobalCount"]);
                    DataTable value = get("Select * from OnlineOrders where Pack=" + 0);

                    if (count > gCount1)
                    {
                        //update count in table
                        int itemUpdate = updateItemStatus("Update AdminPharmacySettings set OnlineGlobalCount =" + count + " where AdminPharmacySettingId =" + 1);

                        int count1 = count - gCount1;
                        count1 = count1 + value.Rows.Count;

                        Order1 = obj.GetOrderItems();
                        //if (Order1.data.Count > 0)
                        //{
                        online.SaveOnlineIntegrate(Order1);
                        //}
                        //else
                        //{

                        //}
                        MessageBox.Show("You Receive New Orders, Please Check.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        Order1 = obj.GetOrderItems();
                        //if (Order1.data.Count > 0)
                        //{
                        online.SaveOnlineIntegrate(Order1);
                        //}
                        //else
                        //{

                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private DataTable get(string qry)
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            con.Open();
            SqlCommand com = new SqlCommand(qry, con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = com;
            sda.Fill(dt);
            con.Close();
            return dt;
        }
        private void timer9_Tick(object sender, EventArgs e)
        {
            if (btnToggleLogin.Text != "Login" && SharedVariables.AdminPharmacySetting.IsOnlineIntegEnable)
            {
                this.timer9.Stop();
                this.timer8.Start();
                this.timer7.Start();
            }
        }

        private void timer8_Tick(object sender, EventArgs e)
        {
            Thread OnlineOrderCount = new Thread(new ThreadStart(getOnlineOrderCount));
            OnlineOrderCount.Start();
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            Thread approvedItems = new Thread(new ThreadStart(getApprovedItems));
            approvedItems.Start();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmInvoice(), this);
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmInvoiceHistory(), this);
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmSalesReturn(), this);
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmPatients(), this);
        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmManageStocks(), this);
        }

        private void radButton7_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmStockReturn(), this);
        }

        private void radButton8_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmUsers(), this);
        }

        private void radButton9_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmSuppliers(), this);
        }

        private void radButton10_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmStockAudits(), this);
        }

        private void radButton11_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmStockAdjustments(), this);
        }

        private void radButton12_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmStockConsumptions(), this);
        }

        private void radButton13_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmManufacturers(), this);
        }

        private void radButton14_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmDiscounts(), this);
        }

        private void radButton15_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmFinancialReportsUI(), this);
        }

        private void radButton16_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new Reports.Pharmacy.frmPharmacyReports(), this);
        }

        private void radButton17_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new Reports.Accounts.frmAccountsReportsUI(), this);
        }

        private void radButton18_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmStoreClosing(), this);
        }

        private void radButton19_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new AdminSetting(), this);
        }

        private void radButton6_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAllItems(), this);

        }
    }
}