using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core.ViewModels;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using Pharmacy.Core.Utility;
using Newtonsoft.Json;
using Pharmacy.Core.Entities;
using System.Text.RegularExpressions;
using System.Reflection;
using Pharmacy.UI;

namespace Pharmacy.Core
{
    public static class SharedFunctions
    {
        public static Form IsFormOpen(string Name)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm.Name == Name)
                {
                    
                    return frm;
                }
                
            }
            return null;
        }
        public static Form IsFormOpen(string Name , bool search)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm.Name == Name && frm.ContainsFocus)
                {

                    return frm;
                }

            }
            return null;
        }
        public static Form OpenForm(Form objForm, Form ParentForm)
        {
            
            Form f = SharedFunctions.IsFormOpen(objForm.Name);
            if (f == null)
            {
                objForm.MdiParent = ParentForm;
                objForm.BringToFront();
                objForm.Show();
                if (SharedVariables.AdminPharmacySetting.DarkMode)
                {
                    DarkMode.EnableDarkMode(objForm);
                }
                SharedVariables.IsStockModule = f!= null && f.Name == "frmAddStock" ? true : false;
                objForm.WindowState = FormWindowState.Maximized;
                //formClose(ParentForm, objForm.Text);
                return objForm;
            }
            else
            {
                f.Activate();
                f.BringToFront();
                SharedVariables.IsStockModule = f != null && f.Name == "frmAddStock" ? true : false;
                f.WindowState = FormWindowState.Maximized;
                return f;
            }
        }
        public static void formClose(Form ParentForm , string txt)
        {
            foreach(Form frm in ParentForm.MdiChildren)
            {
                if (frm.Text != txt)
                {
                    frm.Visible = false;
                    frm.Dispose();
                }
            }
            return;
        }
        public static void isValidNumericKey(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        public static void IsValidDoubleNumerickey(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int cursorPosLeft = tb.SelectionStart;
            int cursorPosRight = tb.SelectionStart + tb.SelectionLength;
            string result = tb.Text.Substring(0, cursorPosLeft) + e.KeyChar + tb.Text.Substring(cursorPosRight);
            string[] parts = result.Split('.');
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
            if (Char.IsNumber(e.KeyChar) || e.KeyChar == '.')
            {
                if (parts.Length > 1)
                {
                    if (parts[1].Length > 3 || parts.Length > 2)
                    {
                        e.Handled = true;
                    }
                }
                if (parts.Length > 0)
                {
                    if (parts[0].Length > 10 || parts.Length > 2)
                    {
                        e.Handled = true;
                    }
                }
            }
            if (parts != null && parts.Length > 1 && result.Contains(".") && result.Contains(".\b"))
            {
                int StartIndex = parts[0].Replace("\b", "").Length;
                int EndIndex = parts[1].Replace("\b", "").Length;
                if (StartIndex + EndIndex > 10)
                {
                    e.Handled = true;
                }
            }
        }
        public static void IsValidDoubleNumericDatagridview(object sender, KeyPressEventArgs e, TextBox tb)
        {
            try
            {
                if(tb == null)
                {
                    return;
                }
                int cursorPosLeft = tb.SelectionStart;
                int cursorPosRight = tb.SelectionStart + tb.SelectionLength;
                string result = tb.Text.Substring(0, cursorPosLeft) + e.KeyChar + tb.Text.Substring(cursorPosRight);
                string[] parts = result.Split('.');
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
                if ((e.KeyChar == '.') && (tb.Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
                if (Char.IsNumber(e.KeyChar) || e.KeyChar == '.')
                {
                    if (parts.Length > 1)
                    {
                        if (parts[1].Length > 3 || parts.Length > 2)
                        {
                            e.Handled = true;
                        }
                    }
                    if (parts.Length > 0)
                    {
                        if (parts[0].Length > 10 || parts.Length > 2)
                        {
                            e.Handled = true;
                        }
                    }
                }
                if (parts != null && parts.Length > 1 && result.Contains(".") && result.Contains(".\b"))
                {
                    int StartIndex = parts[0].Replace("\b", "").Length;
                    int EndIndex = parts[1].Replace("\b", "").Length;
                    if (StartIndex + EndIndex > 10)
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception) { }
        }

        public static void isValidIntegerKey(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        public static void isValidAlphabaticKey(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
        }

        public static void ShowErrorMessage(string Message, string ExceptionMessage, string MessageTitle)
        {
            if (SharedVariables.ShowActualExceptionMessages)
            {
                MessageBox.Show(ExceptionMessage, MessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(Message, MessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static void ShowSuccessMessage(string Message, string MessageTitle)
        {
            MessageBox.Show(Message, MessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowWarningMessage(string Message, string MessageTitle)
        {
            MessageBox.Show(Message, MessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        public static String GetTimestamp2(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }

        public static string CopyFileToLocal(string SourceFilePath)
        {
            string DestFileName = SharedFunctions.GetTimestamp(DateTime.Now) + Path.GetExtension(SourceFilePath);
            string destDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            string DestPath = Path.Combine(destDir, DestFileName);
            string sourchPath = SourceFilePath;
            if (File.Exists(sourchPath))
            {
                System.IO.Directory.CreateDirectory(destDir);
                File.Copy(sourchPath, DestPath);
            }
            return DestPath;
        }

        public static void GenerateStockConsumptionTable(DataTable dtStockConsumption)
        {
            dtStockConsumption.Columns.Add("Quantity", typeof(int));
            dtStockConsumption.Columns.Add("ConsumptionType", typeof(int));
            dtStockConsumption.Columns.Add("Comment", typeof(string));
            dtStockConsumption.Columns.Add("Batch_BatchId", typeof(string));
            dtStockConsumption.Columns.Add("Item_ItemID", typeof(int));
            dtStockConsumption.Columns.Add("Invoice_InvoiceId", typeof(string));
            dtStockConsumption.Columns.Add("CreateDate", typeof(DateTime));
            dtStockConsumption.Columns.Add("IsActive", typeof(bool));
            dtStockConsumption.Columns.Add("IsNew", typeof(bool));
        }

        public static void ShowPageNo(Control ShowingControl, int CurrentPage, int TotalPages)
        {
            if (ShowingControl is Label || ShowingControl is TextBox)
            {
                ShowingControl.Text = CurrentPage + " / " + TotalPages;
            }
            else
            {
                MessageBox.Show("You Can Speicify only Label OR TextBox For Showing Page no.", "Invalid Control", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void SetApplicationStartupValues()
        {
            SharedVariables.PharmacyName = "Desktop Pharmacy Practices";
            SharedVariables.PharmacyPhone = "0302xxxxxxx";
            SharedVariables.PharmacyAddress = "N/A";
        }

        public static List<RightsVM> CreateRights()
        {
            List<RightsVM> Rights = new List<RightsVM>();

            Rights.Add(new RightsVM { RightId = 1, RightDescription = "Create Users" });
            Rights.Add(new RightsVM { RightId = 2, RightDescription = "Edit Users" });
            Rights.Add(new RightsVM { RightId = 3, RightDescription = "Delete Users" });
            Rights.Add(new RightsVM { RightId = 4, RightDescription = "Edit Payment/Invoice Date" });
            Rights.Add(new RightsVM { RightId = 5, RightDescription = "View Financial Reports" });
            Rights.Add(new RightsVM { RightId = 6, RightDescription = "Delete Patients" });
            Rights.Add(new RightsVM { RightId = 7, RightDescription = "Edit Invoice" });
            Rights.Add(new RightsVM { RightId = 8, RightDescription = "Refund Payment" });
            Rights.Add(new RightsVM { RightId = 9, RightDescription = "Delete Invoice" });
            Rights.Add(new RightsVM { RightId = 10, RightDescription = "Add Stock" });
            Rights.Add(new RightsVM { RightId = 11, RightDescription = "Edit Stock" });
            Rights.Add(new RightsVM { RightId = 12, RightDescription = "View Pharmacy Report" });
            Rights.Add(new RightsVM { RightId = 13, RightDescription = "Edit Item" });
            Rights.Add(new RightsVM { RightId = 14, RightDescription = "Add Item" });
            Rights.Add(new RightsVM { RightId = 15, RightDescription = "Delete Item" });
            Rights.Add(new RightsVM { RightId = 16, RightDescription = "Edit Invoice Retail Price" });
            return Rights;
        }


        public static byte[] ImageToByteArray(Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }

        public static byte[] ImageToByteArrays(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                Bitmap bmp = new Bitmap(imageIn);
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static DataTable GetImageTable(string ImagePath)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add(new DataColumn("LogoImage", typeof(byte[])));
                Image img = Image.FromFile(ImagePath);
                byte[] ImageBytes = SharedFunctions.ImageToByteArray(img);
                DataRow dr = dt.NewRow();
                dr[0] = ImageBytes;
                dt.Rows.Add(dr);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("You pharmacy logo is not found, image will not show on invoices and prints." + Environment.NewLine + "You can fix this by adding you logo under admin setting", "Logo not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return dt;
        }
        public static DataTable GetImageTable(Bitmap ImagePath)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add(new DataColumn("FBRBrCode", typeof(byte[])));
                byte[] ImageBytes = SharedFunctions.ImageToByteArray(ImagePath);
                DataRow dr = dt.NewRow();
                dr[0] = ImageBytes;
                dt.Rows.Add(dr);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("You pharmacy logo is not found, image will not show on invoices and prints." + Environment.NewLine + "You can fix this by adding you logo under admin setting", "Logo not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return dt;
        }

        public static DataTable EmptyImageTable()
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add(new DataColumn("LogoImage", typeof(byte[])));
                byte[] ImageBytes = new byte[0];
                DataRow dr = dt.NewRow();
                dr[0] = ImageBytes;
                dt.Rows.Add(dr);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("You pharmacy logo is not found, image will not show on invoices and prints." + Environment.NewLine + "You can fix this by adding you logo under admin setting", "Logo not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return dt;
        }

        public static void SetLarggeButtonsStyle(Button[] Buttons)
        {
            foreach (Button b in Buttons)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 152, 210);
                //42, 196, 244
                b.BackColor = Color.AliceBlue;
                b.ForeColor = Color.Black;
                b.FlatAppearance.BorderSize = 1;
                if (SharedVariables.AdminPharmacySetting.DarkMode)
                {
                    b.FlatAppearance.BorderSize = 2;
                    ///b.FlatAppearance.BorderColor = Color.White;
                }
                //b.Height = 30; // setting this property disturbs controls location in all forms <(O_O)>
                b.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            }
        }


        public static void SetControlForeColor(Control[] Controls)
        {
            foreach (Control c in Controls)
            {
                c.ForeColor = Color.AliceBlue; ;
            }
        }





        public static void SetSmallButtonsStyle(Button[] Buttons)
        {
            foreach (Button b in Buttons)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 152, 210);
                b.BackColor = Color.AliceBlue;
                b.ForeColor = Color.Black;
                b.FlatAppearance.BorderSize = 1;
                if (SharedVariables.AdminPharmacySetting.DarkMode)
                {
                    b.FlatAppearance.BorderSize = 2;
                    ///b.FlatAppearance.BorderColor = Color.White;
                }
                //b.Height = 23;
                b.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            }
        }
        public static void SetGridStyle(DataGridView grid, bool IsInvoiceGrid = false)
        {
            if (!SharedVariables.AdminPharmacySetting.DarkMode)
            {
                grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Cornsilk;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12.25f, FontStyle.Bold, GraphicsUnit.Point);
                grid.RowTemplate.DefaultCellStyle.Font = new Font("Times New Roman", 12.25f, FontStyle.Regular, GraphicsUnit.Point);
                if (!IsInvoiceGrid)
                {
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
                    grid.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
                }
                grid.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.AliceBlue; //Color.FromArgb(231, 249, 255); //Color.FromArgb(225, 248, 255);
                grid.RowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
                grid.RowHeadersVisible = false;
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersHeight = 45;
                grid.ScrollBars = ScrollBars.Both;
                grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                grid.RowTemplate.Height = 32;
                grid.BackgroundColor = Color.White;
                //grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            }
            else
            {
                //DataGridView dataGridView = control as DataGridView;
                grid.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
                grid.DefaultCellStyle.ForeColor = Color.White;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point);
                grid.RowHeadersVisible = false;
                grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                grid.RowTemplate.Height = 23;
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersHeight = 35;
                grid.ScrollBars = ScrollBars.Both;
                grid.BackgroundColor = Color.FromArgb(45, 45, 48);
                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
                grid.RowsDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
                grid.ForeColor = Color.White;
               

            }



        }


        //public static void UpdateCommonStyle(DataGridView Grid)
        //{
        //    Grid.DefaultCellStyle.BackColor = SharedVariables.RowSelectionColor;
        //}

        // code for sending emails through API
        //public static void Email(string filePath, List<string> Receivers)

        //method signature for sending email using healthwire email using credentials
        public static void Email(string sender, string senderPswd, List<string> Receivers, List<string> AttachedDocs)
        {
            try
            {
                // code for sending emails through API
                //Service service = new Service();
                //string jsonLogin = service.Login();
                //if (IsJson(jsonLogin))
                //{
                //    Login login = new Login();
                //    login = JsonConvert.DeserializeObject<Login>(jsonLogin);
                //    service.Token = login.auth_token;
                //    if (login.success)
                //    {
                //        string dataSentBYAPI = "";
                //        dataSentBYAPI = service.SendLowStocckEmail(Receivers,login.auth_token).ToString();
                //        //foreach (FinalReport e in UnSyncedReports)
                //        //{
                //        //    dataSentBYAPI = service.SyncFinalReport(e, AppData.Email, AppData.Token, login.practice_id).ToString();
                //        //    if (Shared.IsJson(dataSentBYAPI))
                //        //    {
                //        //        SyncResponse result = JsonConvert.DeserializeObject<SyncResponse>(dataSentBYAPI);
                //        //        if (result.status == "success" || result.status.Contains("present"))
                //        //        {
                //        //            using (cxt = new DAL.AnalyzerContext())
                //        //            {
                //        //                FinalReport rpt = cxt.Reports.Where(r => r.FinalReportId == e.FinalReportId).FirstOrDefault();
                //        //                rpt.IsSynced = true;
                //        //                cxt.SaveChanges();
                //        //            }
                //        //        }        
                //        //    }
                //        //}
                //    }

                //code for sending email using healthwire email using credentials
                //MailMessage message = new MailMessage();
                //SmtpClient smtp = new SmtpClient();
                //message.From = new MailAddress(sender);
                //foreach (string receiver in Receivers)
                //{
                //    message.To.Add(new MailAddress(receiver));
                //}
                //message.Subject = "Low Stocks";
                //message.IsBodyHtml = true; //to make message body as html  
                //if (AttachedDocs != null)
                //{
                //    foreach (string doc in AttachedDocs)
                //    {
                //        message.Attachments.Add(new Attachment(doc));
                //    }
                //}
                //message.Body = "Please find the attached doc for low stock";
                //smtp.Port = 587;
                //smtp.Host = "smtp.gmail.com"; //for gmail host  
                //smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential(Sender, SenderPassword);
                //smtp.Send(message);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void EmailWithIp(string Subject, string MessageBody, string[] AttachedDocs, string Ip, string[] Receivers)
        {
            try
            {
                string to = "faani1786@gmail.com";
                string from = "umar.waqas.se@gmail.com";
                MailMessage message = new MailMessage(from, to);
                message.Subject = "Using the new SMTP client.";
                message.Body = @"Using this new feature, you can send an email message from an application very easily.";
                SmtpClient client = new SmtpClient(Ip);
                // Credentials are necessary if the server requires the client
                // to authenticate before it will send email on the client's behalf.
                client.UseDefaultCredentials = true;
                client.Send(message);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool IsInternetAvailable()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool IsJson(String jsonData)
        {
            return jsonData.Trim().Substring(0, 1).IndexOfAny(new[] { '[', '{' }) == 0;
        }

        public static void MessageBoxShow(string text, MessageType type)
        {
            switch (type)
            {
                case MessageType.Error:
                    MessageBox.Show(text, type.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case MessageType.Info:
                    MessageBox.Show(text, type.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case MessageType.Success:
                    MessageBox.Show(text, type.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case MessageType.Warning:
                    MessageBox.Show(text, type.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                default:
                    break;
            }
        }

        public static bool IsAssignUserRole(string RoleName)
        {
            bool IsAssign = false;
            try
            {
                if (SharedVariables.UserRights == null && SharedVariables.UserRights.Count() <= 0)
                    IsAssign = false;
                if (SharedVariables.UserRights.Where(x => x.Description.ToLower() == RoleName.ToLower()).Count() > 0)
                    IsAssign = true;
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
            return IsAssign;
        }

        public static bool IsActionallowed(string Right)
        {
            foreach (UserRole r in SharedVariables.LoggedInUser.UserRoles)
            {
                if (r.Description == "super Admin")
                {
                    return true;
                }
            }

            long RightId = SharedVariables.LoggedInUser.Rights.Where(r => r.Description == Right).Select(r => r.RightId).FirstOrDefault();
            if (RightId > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void AccessDeniedMsg()
        {
            MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static int StringToInt(object value)
        {
            int result = 0;
            try
            {
                if (value != null)
                    int.TryParse(value.ToString(), out result);
            }
            catch (Exception)
            {

            }
            return result;
        }

        public static Int32 StringToInt32(object value)
        {
            Int32 result = 0;
            try
            {
                if (value != null)
                    Int32.TryParse(value.ToString(), out result);
            }
            catch (Exception)
            {

            }
            return result;
        }
        public static long StringToLong(object value)
        {
            long result = 0;
            try
            {
                if (value != null)
                    long.TryParse(value.ToString(), out result);
            }
            catch (Exception)
            {

            }
            return result;
        }

        public static string ObjectToString(object value)
        {
            string result = "";
            try
            {
                if (value != null)
                    result = value.ToString();
            }
            catch (Exception)
            {

            }
            return result;
        }

        public static double StringToDouble(object value)
        {
            double result = 0;
            try
            {
                if (value != null)
                    double.TryParse(value.ToString(), out result);
            }
            catch (Exception)
            {

            }
            return result;
        }

        public static void CalculateInvoiceItemQty(BatchStockVM i)
        {
            i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock) - i.StockReturn - i.BonusStock;
            if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
            {
                i.AvailableStock += i.HoldStock;
            }
        }
        
        public static void CalculateInvoiceItemQty_1(BatchStockVM i)
        {
            if (i.ConsumedStock < 0)
            {
                i.ConsumedStock = System.Math.Abs(i.ConsumedStock);
            }
            else 
            {
                i.ConsumedStock = i.ConsumedStock;
            }
            i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock) - i.StockReturn - i.BonusStock;
            if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
            {
                i.AvailableStock += i.HoldStock;
            }
        }
        public static void CalculateItemAvailableQty(ItemsVM item)
        {
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                item.AvailableStock = item.TotalStock - (item.ExpiredStock - item.ExpiredConsumedStock) - item.ConsumedStock + item.AdjustedStock - item.StockReturn - item.BonusStock;
            }
            else
            {
                item.AvailableStock = item.TotalStock - (item.ExpiredStock - item.ExpiredConsumedStock) - item.ConsumedStock + item.AdjustedStock + item.HoldStock - item.StockReturn - item.BonusStock;
            }
        }
        public static void CalculateItemAvailableQtywithoutexpirty(ItemsVM item)
        {
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                item.AvailableStock = item.TotalStock  - item.ConsumedStock + item.AdjustedStock - item.StockReturn - item.BonusStock;
            }
            else
            {
                item.AvailableStock = item.TotalStock - item.ConsumedStock + item.AdjustedStock + item.HoldStock - item.StockReturn - item.BonusStock;
            }
        }
        public static void CalculateItemAvailableQty_New(StockStaticsVM item)
        {
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                item.AvailableStock = item.totalstock - (item.ExpiredStock - item.ExpiredConsumedStock) - item.TotalConsumed + item.TotalAdjustment - item.StockReturn - item.BonusStock;
                item.CostofAvailableQty = Convert.ToInt32(item.UnitCost * item.AvailableStock);
                item.RetailofAvailableQty = Convert.ToInt32(item.RetailPrice * item.AvailableStock);
            }
            else
            {
                item.AvailableStock = item.totalstock - (item.ExpiredStock-item.ExpiredConsumedStock) - item.TotalConsumed + item.TotalAdjustment + item.HoldStock - item.StockReturn - item.BonusStock;
                item.CostofAvailableQty = Convert.ToInt32(item.UnitCost * item.AvailableStock);
                item.RetailofAvailableQty = Convert.ToInt32(item.RetailPrice * item.AvailableStock);
            }
        }
        public static void CalculateItemAvailableQtyOnSearch(BulkItemsVM item)
        {
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                item.AvailableStock = item.TotalStock - (item.ExpiredStock - item.ExpiredConsumedStock) - item.ConsumedStock + item.AdjustedStock - item.StockReturn - item.BonusStock;
            }
            else
            {
                item.AvailableStock = item.TotalStock - (item.ExpiredStock - item.ExpiredConsumedStock) - item.ConsumedStock + item.AdjustedStock + item.HoldStock - item.StockReturn - item.BonusStock;
            }
        }
        public static void CalculateStockBatchAvailableQty(StockReturnItemVM i)
        {
            i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock) - i.StockReturn - i.BonusStock;
            if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                i.AvailableStock += i.HoldStock;
            }
        }
        public static void CalculateStockBatchAvailableQtyReturn(StockReturnItemVM i)
        {
            i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock) - i.ReturnQuantity - i.BonusStock;
            if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                i.AvailableStock += i.HoldStock;
            }
        }
        public static void CalculateStockFilterQty(StockAddFilterVM i)
        {
            i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock) - i.StockReturn - i.BonusStock;
            if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                i.AvailableStock += i.HoldStock;
            }
        }
        public static void CalculateStockFilterQty1(StockAddFilterVM i)
        {
            i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock1 - i.ExpiredStock) - i.StockReturn - i.BonusStock;
            if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                i.AvailableStock += i.HoldStock;
            }
        }
        public static int CalculatePurchaseQty(ItemsVM AvStock)
        {
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                return AvStock.TotalStock + AvStock.AdjustedStock - AvStock.ExpiredStock - AvStock.ConsumedStock - AvStock.StockReturn - AvStock.BonusStock;
            }
            else
            {
                return AvStock.TotalStock + AvStock.AdjustedStock - AvStock.ExpiredStock - AvStock.ConsumedStock + AvStock.HoldStock - AvStock.StockReturn - AvStock.BonusStock;
            }
        }
        public static void CalculateItemwisePurchaseReport(ItemWisePurchaseVM i)
        {
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.StockReturn - i.BonusStock;
            }
            else
            {
                i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock + i.HoldStock - i.StockReturn - i.BonusStock;
            }
        }
        public static void CalculateDeadStock(DeadStockVM i)
        {
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.StockReturn - i.BonusStock;
            }
            else
            {
                i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock + i.HoldStock - i.StockReturn - i.BonusStock;
            }
        }
        public static void CalculateItemwiseSaleReport(ItemWiseSaleVM i)
        {
            i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock) - i.StockReturn - i.BonusStock;
            if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
            {
                i.AvailableStock += i.HoldStock;
            }
        }
        public static void CalculateItemwiseReport(ItemWiseRevenueVM obj, AvailableQtyVM objQty)
        {
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock - objQty.StockReturn - objQty.BonusStock;
            }
            else
            {
                obj.AvailableStock = objQty.TotalStock + objQty.AdjustedStock - objQty.ExpiredStock - objQty.ConsumedStock + objQty.HoldStock - objQty.StockReturn - objQty.BonusStock;
            }
        }


        public static void SearchableCombobox(ComboBox cmb)
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDown;
            cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static bool ValidationEmail(string email)
        {
            return Regex.IsMatch(email, @"\A([^@\s]+)@((?:[-a-z 0-9]+\.)+[a-z]{2,})\z");
            //return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }
        public static bool ValidatePassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
        }

        public static byte[] GetBytesFromImage(string imageFile)
        {
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromFile(imageFile);
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        public static Image ByteArrayToImage(byte[] bytesArr)
        {
            using (MemoryStream memstr = new MemoryStream(bytesArr))
            {
                Image img = Image.FromStream(memstr);
                return img;
            }
        }

        public static List<T> DataTableToModel<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList();
        }

        public static Bitmap BarcodeSetting(Image barcodeImage, string Barcode, string Name, double RetailPrice)
        {
            var resultImage = new Bitmap(barcodeImage.Width, barcodeImage.Height + 55); // 20 is bottom padding, adjust to your text
            using (var graphics = Graphics.FromImage(resultImage))
            using (var font = new Font("Microsoft Sans Serif", 7))
            using (var brush = new SolidBrush(Color.Black))
            using (var format = new StringFormat()
            {
                Alignment = StringAlignment.Near, // Also, horizontally centered text, as in your example of the expected output
                LineAlignment = StringAlignment.Far
            })
            {
                graphics.Clear(Color.White);
                graphics.DrawImage(barcodeImage, 0, 14);
                var bar = Barcode + "\n" + Name + "\n" + "Price:" + RetailPrice.ToString("0.00");
                graphics.DrawString(bar, font, brush, 1, resultImage.Height, format);
                graphics.DrawString(SharedVariables.AdminPractiseSetting.Name, font, brush, 1, 12, format);
            }
            return resultImage;
        }

        public static bool IsDoubleFormat(double value, string price)
        {
            double integerPart = Math.Truncate(value);
            if (integerPart.ToString().Length > 10)
            {
                SharedFunctions.MessageBoxShow(price + " Integer value greater then 10 digits.", MessageType.Warning);
                return true;
            }
            else
            {
                return false;
            }
        }

      

        public static string GetHWID()
        {
            return "";
            //var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            //ManagementObjectCollection mbsList = mbs.Get();
            //string id = "";
            //foreach (ManagementObject mo in mbsList)
            //{
            //    id = mo["ProcessorId"].ToString();
            //    break;
            //}
        }
    }
    public enum MessageType
    {
        Error,
        Info,
        Success,
        Warning
    }
}













