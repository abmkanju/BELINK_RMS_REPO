using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Microsoft.Win32;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;

namespace Pharmacy.UI
{
    public class ProductActivation
    {
        private static string EncryptionKey = GlobalSharing.Salt;
        private static string SqlConnectionString = Shared.GetConnectionString();
        private static string TableName = "HwFileDatas";
        private static string ColumnName = "JsonKey";
        private static UnitOfWork unitOfWork;
        public static bool CheckSystemExpiration()
        {
            try
            {
                // Retrieve the saved data from SQL table
                string jsonData = RetrieveJsonFromSql();

                if (!string.IsNullOrEmpty(jsonData))
                {
                    // Convert the JSON string to DataTable
                    jsonData = CryptopEngine.Decrypt(jsonData, GlobalSharing.Salt);
                    DataTable dataTable = JsonToDataTable(jsonData);

                    if (dataTable == null)
                    {
                        return false;
                    }

                    // Decrypt the field names and data
                    Dictionary<string, object> decryptedFields = new Dictionary<string, object>();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        string decryptedField = DecryptData(column.ColumnName);
                        string decryptedData = DecryptData(dataTable.Rows[0][column].ToString());
                        decryptedFields.Add(decryptedField, decryptedData);
                    }

                    // Extract the decrypted values
                    string activationKey = decryptedFields["ActivationKey"].ToString();
                    DateTime activationDate = DateTime.Parse(decryptedFields["ActivationDate"].ToString());
                    DateTime expiryDate = DateTime.Parse(decryptedFields["ExpiryDate"].ToString());
                    bool isNetworkSystem = bool.Parse(decryptedFields["IsNetworkSystem"].ToString());
                    string networkIp = decryptedFields["NetworkIp"].ToString();
                    DateTime mostRecentRecordedate = DateTime.Parse(decryptedFields["MostRecentRecordedate"].ToString());
                    int usageDaysCounter = int.Parse(decryptedFields["UsageDaysCounter"].ToString());

                    // Calculate the usage limit based on activation date and expiry date
                    TimeSpan usageLimit = expiryDate - activationDate;

                    // Check if the system has expired based on usage days
                    if (usageDaysCounter >= usageLimit.TotalDays)
                    {
                        return false; // System has exceeded usage days limit
                    }

                    // Check if the system has expired based on expiry date
                    if (DateTime.Now > expiryDate)
                    {
                        return false; // System has expired
                    }

                    // Update the most recent date and usage days counter
                    DateTime currentDate = DateTime.Now;

                    if ((currentDate.Date >= activationDate.Date && currentDate.Date >= mostRecentRecordedate.Date && currentDate.Date <= expiryDate.Date) || (usageDaysCounter <= usageLimit.TotalDays && currentDate.Date >= activationDate.Date && currentDate.Date <= expiryDate.Date))
                    {
                        mostRecentRecordedate = currentDate;
                        if (Properties.Settings.Default.DaysCounterDate.Date != currentDate.Date)
                        {
                            Properties.Settings.Default.DaysCounterDate = currentDate;
                            Properties.Settings.Default.Save();
                            usageDaysCounter++;
                        }
                        UpdateMostRecentDateAndCounter(mostRecentRecordedate, usageDaysCounter);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                //return true; // System is not expired
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static async Task<double> GetExpiryDateAsync()
        {
            try
            {
                // Retrieve the saved data from SQL table asynchronously
                string jsonData = await Task.Run(() => RetrieveJsonFromSql());

                if (!string.IsNullOrEmpty(jsonData))
                {
                    // Convert the JSON string to DataTable
                    jsonData = CryptopEngine.Decrypt(jsonData, GlobalSharing.Salt);
                    DataTable dataTable = JsonToDataTable(jsonData);

                    if (dataTable == null)
                    {
                        return 0;
                    }

                    // Decrypt the field names and data
                    Dictionary<string, object> decryptedFields = new Dictionary<string, object>();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        string decryptedField = await Task.Run(() => DecryptData(column.ColumnName));
                        string decryptedData = await Task.Run(() => DecryptData(dataTable.Rows[0][column].ToString()));
                        decryptedFields.Add(decryptedField, decryptedData);
                    }

                    // Extract the decrypted values
                    string activationKey = decryptedFields["ActivationKey"].ToString();
                    DateTime activationDate = DateTime.Parse(decryptedFields["ActivationDate"].ToString());
                    DateTime expiryDate = DateTime.Parse(decryptedFields["ExpiryDate"].ToString());
                    bool isNetworkSystem = bool.Parse(decryptedFields["IsNetworkSystem"].ToString());
                    string networkIp = decryptedFields["NetworkIp"].ToString();
                    DateTime mostRecentRecordedate = DateTime.Parse(decryptedFields["MostRecentRecordedate"].ToString());
                    int usageDaysCounter = int.Parse(decryptedFields["UsageDaysCounter"].ToString());

                    // Calculate the usage limit based on activation date and expiry date
                    TimeSpan usageLimit = expiryDate - activationDate;

                    // Check if the system has expired based on usage days
                    if (usageDaysCounter >= usageLimit.TotalDays)
                    {
                        return 0; // System has exceeded usage days limit
                    }

                    // Check if the system has expired based on expiry date
                    if (DateTime.Now > expiryDate)
                    {
                        return 0; // System has expired
                    }

                    // Update the most recent date and usage days counter
                    DateTime currentDate = DateTime.Now;

                    if ((currentDate.Date >= activationDate.Date && currentDate.Date >= mostRecentRecordedate.Date && currentDate.Date <= expiryDate.Date) || (usageDaysCounter <= usageLimit.TotalDays && currentDate.Date >= activationDate.Date && currentDate.Date <= expiryDate.Date))
                    {
                        double timeSpan = (expiryDate.Date - currentDate.Date).TotalDays;
                        return timeSpan;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }



        private const string SpreadsheetId = "1kJSmkHYUQ-C5AyMzsCVIssG1y0MJiNSO65fdMi9gE6U";
        private const string SheetName = "Sheet1";

        private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        public static void UpdateRecordInSheet(string activationKey, string pharmacyName, DateTime? activationDate, DateTime? expiryDate, string newActivationKey, int years, int months, int days, bool isnetwork, int isActivated)
        {
            /*var clientSecrets = new ClientSecrets
            {
                ClientId = "821796772196-blnu0g1bo715cc678p8m0fr2u3ffrb2q.apps.googleusercontent.com",
                ClientSecret = "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"
            };

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets,
                Scopes,
                "user",
                System.Threading.CancellationToken.None,
                new FileDataStore("MyAppsToken"))
                .Result;

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "MyTestApp"
            });

            string activated = "";
            if (isActivated == 0)
            {
                activated = "yes";
            }
            else if (isActivated == 1)
            {
                activated = "no";
            }
            else
            {
                activated = "Trial User";
            }

            var range = $"{SheetName}!A:K";
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            var values = response.Values;
            
            string user = SharedVariables.LoggedInUser.UserName;
                

            if (values != null && values.Count > 0)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var row = values[i];
                    if (row.Count > 4 && row[4].ToString() == CryptopEngine.Decrypt(activationKey, GlobalSharing.Salt)) // Assuming ActivationKey is in the fifth column (index 4)
                    {
                        // Update the values in the matching row
                        row[0] = pharmacyName;
                        row[2] = activationDate?.ToShortDateString();
                        row[3] = expiryDate?.ToShortDateString();
                        row[4] = CryptopEngine.Decrypt(activationKey, GlobalSharing.Salt);
                        row[5] = years;
                        row[6] = months;
                        row[7] = days;
                        row[8] = isnetwork ? "yes" : "no";
                        row[9] = activated;
                        row[10] = user;

                        var updateRange = $"{SheetName}!A{i + 1}:K{i + 1}"; // Assuming rowIndex starts from 0
                        var updateValueRange = new ValueRange
                        {
                            Values = new List<IList<object>> { row }
                        };

                        var updateRequest = service.Spreadsheets.Values.Update(updateValueRange, SpreadsheetId, updateRange);
                        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                        var updateResponse = updateRequest.Execute();
                        break; // Break out of the loop since the record has been updated
                    }
                }
            }*/
        }
        public static void WriteStringsToSheet(string PharmacyName, string CPUID, DateTime? ActivationDate, DateTime? ExpiryDate, string ActivationKey, int years, int months, int days, bool isnetwork, int isActivated)
        {
          /*  var clientSecrets = new ClientSecrets
            {
                ClientId = "821796772196-blnu0g1bo715cc678p8m0fr2u3ffrb2q.apps.googleusercontent.com",
                ClientSecret = "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"
            };

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets,
                Scopes,
                "user",
                System.Threading.CancellationToken.None,
                new FileDataStore("MyAppsToken"))
                .Result;

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "MyTestApp"
            });
            string activated = "";
            if (isActivated == 0)
            {
                activated = "yes";
            }
            else if (isActivated == 1)
            {
                activated = "no";
            }
            else
            {
                activated = "Trial User";
            }
            string user = "SQA Testing  User";
            DataTable dt = get("select Deployed from HwFileDatas");
            if (dt.Rows.Count > 0)
            {
                if (!Convert.ToBoolean(dt.Rows[0][0].ToString()))
                {
                    user = "SQA Testing  User";
                    
                }
                else
                {
                    user = SharedVariables.LoggedInUser.UserName;
                }
            }
            
            var range = $"{SheetName}!A:K";
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>> { new List<object> { PharmacyName, CPUID, ActivationDate.Value.ToShortDateString(), ExpiryDate.Value.ToShortDateString(), ActivationKey, years, months, days, isnetwork ? "yes" : "no", activated , user } }
            };

            var request = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
            var response = request.Execute();*/
        }
        private static void UpdateMostRecentDateAndCounter(DateTime mostRecentRecordedate, int usageDaysCounter)
        {
            // Retrieve the JSON data from the SQL table
            string jsonData = RetrieveJsonFromSql();

            if (!string.IsNullOrEmpty(jsonData))
            {
                jsonData = CryptopEngine.Decrypt(jsonData, GlobalSharing.Salt);
                // Convert the JSON string to DataTable
                DataTable dataTable = JsonToDataTable(jsonData);

                // Update the values in the DataTable
                string MostRecentRecordedate = CryptopEngine.Encrypt("MostRecentRecordedate", GlobalSharing.Salt);
                string UsageDaysCounter = CryptopEngine.Encrypt("UsageDaysCounter", GlobalSharing.Salt);
                dataTable.Rows[0][MostRecentRecordedate] = CryptopEngine.Encrypt(mostRecentRecordedate.ToString(), GlobalSharing.Salt);
                /*CryptopEngine.Encrypt(.ToString(),GlobalSharing.Salt);*/
                dataTable.Rows[0][UsageDaysCounter] = CryptopEngine.Encrypt(usageDaysCounter.ToString(), GlobalSharing.Salt);
                /*CryptopEngine.Decrypt(.ToString(), GlobalSharing.Salt)*/

                // Convert the DataTable back to JSON string
                string updatedJsonData = DataTableToJson(dataTable);

                // Update the complete JSON string in the SQL table
                using (SqlConnection connection = new SqlConnection(SqlConnectionString))
                {
                    connection.Open();

                    string query = $"UPDATE {TableName} SET {ColumnName} = @JsonData";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@JsonData", CryptopEngine.Encrypt(updatedJsonData, GlobalSharing.Salt));
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        private static string RetrieveJsonFromSql()
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();

                string query = $"SELECT TOP 1 {ColumnName} FROM {TableName} ORDER BY id DESC"; // Replace "SomeColumn" with the appropriate column for ordering the records
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return /*CryptopEngine.Decrypt(*/reader.GetString(0)/* ,  GlobalSharing.Salt)*/;

                        }
                    }
                }
            }

            return null;
        }
        private static DataTable JsonToDataTable(string json)
        {
            try
            {
                DataTable dataTable = new DataTable();
                //json = CryptopEngine.Decrypt(json, GlobalSharing.Salt);
                // Assuming the JSON represents an array of records
                List<Dictionary<string, object>> records = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);

                if (records.Count > 0)
                {
                    // Add columns to the DataTable based on the first record
                    foreach (string field in records[0].Keys)
                    {
                        dataTable.Columns.Add(field);
                    }

                    // Add records to the DataTable
                    foreach (var record in records)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        foreach (KeyValuePair<string, object> entry in record)
                        {
                            dataRow[entry.Key] = entry.Value;
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static bool IsIPAddress(string input)
        {
            IPAddress ipAddress;
            return IPAddress.TryParse(input, out ipAddress);
        }
        public static DateTime GetCurrentDateTimeInPakistanTimeZone()
        {
            string ntpServer = "time.windows.com";

            IPAddress[] addresses = Dns.GetHostAddresses(ntpServer);
            if (addresses.Length == 0)
            {
                throw new ArgumentException("Could not resolve NTP server hostname.");
            }

            byte[] ntpData = new byte[48];
            ntpData[0] = 0x1B;

            using (var udpClient = new UdpClient())
            {
                udpClient.Connect(new IPEndPoint(addresses[0], 123));
                udpClient.Send(ntpData, ntpData.Length);

                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                ntpData = udpClient.Receive(ref remoteEndPoint);
            }

            ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | ntpData[43];
            ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | ntpData[47];

            ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000UL);

            DateTime utcTime = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(milliseconds);
            DateTime pakistanTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"));

            return pakistanTime;
        }
        public static DataTable get(string qry)
        {
            try
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
            catch (Exception ex)
            {
                return new DataTable();
            }

        }

        public static void ActivateSystem(string key , DateTime ActivationDate , DateTime Expirydate , bool IsNetwork , string NetworkIp , DateTime MostRecentDate , bool istrial )
        {
            // Step 1: Assign values to variables
            string activationKey = CryptopEngine.Encrypt(key ,  GlobalSharing.Salt);
            DateTime activationDate = ActivationDate;
            DateTime expiryDate = Expirydate;
            bool isNetworkSystem = IsNetwork;
            string networkIp = NetworkIp;
            DateTime mostRecentRecordedate = MostRecentDate;
            int usageDaysCounter = 1;
           
            // Step 2: Encrypt field names and data
            Dictionary<string, object> encryptedFields = new Dictionary<string, object>
        {
            { EncryptField("ActivationKey"), EncryptData(activationKey) },
            { EncryptField("ActivationDate"), EncryptData(activationDate.ToString()) },
            { EncryptField("ExpiryDate"), EncryptData(expiryDate.ToString()) },
            { EncryptField("IsNetworkSystem"), EncryptData(isNetworkSystem.ToString()) },
            { EncryptField("NetworkIp"), EncryptData(networkIp) },
            { EncryptField("MostRecentRecordedate"), EncryptData(mostRecentRecordedate.ToString()) },
            { EncryptField("UsageDaysCounter"), EncryptData(usageDaysCounter.ToString()) }
        };

            // Step 3: Create DataTable with encrypted column names and data
            DataTable dataTable = new DataTable();
            foreach (var encryptedField in encryptedFields)
            {
                dataTable.Columns.Add(encryptedField.Key);
            }

            DataRow dataRow = dataTable.NewRow();
            foreach (var encryptedField in encryptedFields)
            {
                dataRow[encryptedField.Key] = encryptedField.Value;
            }
            dataTable.Rows.Add(dataRow);

            // Step 4: Convert DataTable to JSON string
            string jsonString = DataTableToJson(dataTable);
            
            UpdateRecordInSheet(activationKey , SharedVariables.AdminPractiseSetting.Name , activationDate , expiryDate , activationKey , istrial?0:Properties.Settings.Default.years , istrial ? 0 : Properties.Settings.Default.months , istrial ? 15 : Properties.Settings.Default.days , IsNetwork , istrial ? 2 :0);
            // Step 5: Save JSON string in SQL table
            jsonString = CryptopEngine.Encrypt(jsonString, GlobalSharing.Salt);
            string str = CryptopEngine.Encrypt("true", GlobalSharing.Salt);
            SaveJsonToSql(jsonString , str);
        }

        private static string EncryptField(string field)
        {
            return CryptopEngine.Encrypt(field, EncryptionKey);
        }

        private static string EncryptData(string data)
        {
            return CryptopEngine.Encrypt(data, EncryptionKey);
        }

        private static string DecryptData(string encryptedData)
        {
            return CryptopEngine.Decrypt(encryptedData, EncryptionKey);
        }

        private static string DataTableToJson(DataTable dataTable)
        {
            List<Dictionary<string, object>> records = new List<Dictionary<string, object>>();

            foreach (DataRow row in dataTable.Rows)
            {
                Dictionary<string, object> record = new Dictionary<string, object>();
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    string columnName = dataTable.Columns[i].ColumnName;
                    string columnValue = row[i].ToString();
                    record[columnName] = columnValue;
                }
                records.Add(record);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(records);
        }

        private static void SaveJsonToSql(string jsonString , string str)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                
                string query = $"delete from HwFileDatas";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
                query = $"INSERT INTO {TableName} ({ColumnName} , xijuekhaumi , Deployed) VALUES (@JsonData ,  @str , 'True')";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@JsonData", jsonString);
                    command.Parameters.AddWithValue("@str", str);
                    command.ExecuteNonQuery();
                }
            }
        }
        private static void SaveToSql(string str)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();

                string query = $"delete from HwFileDatas";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
                query = $"INSERT INTO {TableName} (xijuekhaumi) VALUES (@Data)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Data", str);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static String GenerateActivationKey(string years , string months , string days)
        {
            string key = "";
            string DeviceID = getCPUId();
            String CurrentDate = DateTime.Now.ToString("ddMMyy");
            string UniqueNumber = new Random().Next(1000 , 9999).ToString();
            key = UniqueNumber + "-" + CurrentDate + "-" + DeviceID + "-" + years + "-" + months + "-" + days + "-" + new Random().Next(1000, 9999).ToString();
            return key;
        }
        
        public static string getCPUId()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return cpuInfo;
        }
        
        public static bool SendEmail(string subject, string body)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("abdulbasitkanju@gmail.com", "Product Activation");
                    mail.To.Add("abmkanju@gmail.com");
                    //mail.To.Add("hassan.rasool@healthwire.pk");
                    //mail.To.Add("hamza.yaseen@healthwire.pk");
                    //mail.To.Add("muhammad.inam@healthwire.pk");
                    //mail.To.Add("Healthwire15@gmail.com");
                    mail.Subject = subject;
                    mail.Body = "<h5>" + body + "</h5>";
                    mail.BodyEncoding = Encoding.GetEncoding("utf-8");
                    mail.SubjectEncoding = System.Text.Encoding.Default;
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("abdulbasitkanju@gmail.com", "zjhc sdvq ctot ealj");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
