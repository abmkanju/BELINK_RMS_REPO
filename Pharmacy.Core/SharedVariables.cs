using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Pharmacy.Core.Entities;
using System.Drawing;
using Pharmacy.Core.ViewModels;
using System.Data;
using System.IO;

namespace Pharmacy.Core
{
    public static class SharedVariables
    {
        //public static int Menu_MinWidth { get; set; }
        //public static int Menu_MaxWidth { get; set; }
        public static string ApplicationVersion = "(1.00.3v-2.12b)";
        public static bool IsTesting = false;
        public static bool ShowActualExceptionMessages = true;
        public static string GeneralSuccMsg = "Record Saved Successfully";
        public static string GeneralInsertMsg = "Record Inserted Successfully";
        public static string GeneralUpdateMsg = "Record Updated Successfully";
        public static string GeneralErrMsg = "Faild to Perform Required Action, Please Try Again";
        public static User LoggedInUser;

        public static bool IsClientPC = true;
        //API variables
        public static int API_PerBatchCount = 5;
        public static string Salt = "hw05grewqwas21w3";
        public static string API_serverPath = "";
        public static string API_Email = "";
        public static string API_Password = "";
        public static string API_Token = "";
        public static string Email = "";
        public static string AuthToken = "";

        public static bool manageStockRefr = false;
        public static bool manageStockAdjsRef = false;
        public static bool manageStockConsumRef = false;
        public static bool manageStockReturnRef = false;
        public static string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PharmacyLog.txt");
        //public static int DeadStockDaysLimit = 30;
        public enum ConsumptionType { Services = 1, Sales = 2, Damages = 3, Returned = 4 };
        public enum PatientStatus { Active = 1, Inactive = 2, Inactive_Deceased = 3 }
        public static int PageSize = 15; //13
        public static string PharmacyName { get; set; }
        public static string PharmacyAddress { get; set; }
        public static string PharmacyPhone { get; set; }

        public static Color DisabledColor { get { return Color.LightGray; } }
        public static Color EditingColor { get { return Color.Coral; } }
        public static AdminInvoiceSetting AdminInvoiceSetting { get; set; }
        public static HwFileData HwFileData { get; set; }
        public static AdminProcedureInvoiceSetting AdminProcedureInvoiceSetting { get; set; }
        public static PatientFormFeilds PatientFormFeilds { get; set; }
        public static AdminPractiseSetting AdminPractiseSetting { get; set; }
        public static AdminPrintFormatSetting AdminPrintFormatSetting { get; set; }
        public static AdminPharmacySetting AdminPharmacySetting { get; set; }
        public static AdminSystemSlotSetting AdminSystemSlotSetting { get; set; }
        public static List<Right> UserRights { get; set; }
        public static AdminShiftMasterSetting AdminShiftMasterSetting { get; set; }
        public static List<AdminShiftSetting> AdminShiftSettings { get; set; }
        public static AdminFBRSetting AdminFBRSettings { get; set; }
        public static InvoiceSetting invoiceSetting { get; set; }
        public enum PrintPageSize { A4 = 1, A8 = 2, A5 = 3 };
        public enum PageType { PlainPaper = 1, LetterHead = 2 };
        public enum PageOrientation { Portrait = 1, LandScape = 2 };
        public enum ExpiryPeriodUnit { Days = 0, Months = 2 };
        //public static Color RowSelectionColor = Color.FromArgb(247, 247, 247);
        public static Color RowSelectionBackColor = Color.FromArgb(225, 248, 255);
        public static Color MenuSelectionBackColor = Color.FromArgb(42, 196, 244);
        public static Color MenuSelectionForeColor = Color.White;
        //public static List<Item> POSItems = null;
        public static int LowStockMailDaysInterval = 15; // days
        public static long LowStockMailRetryInterval = 1; // minute
        public static bool UseLocalDb = false;
        public static int PracticeId { get; set; }
        //public static int InitialItemsLoadSize = 50;
        public static int AsyncDataLoadDelay = 500; //milliseconds

        public static DataView BulkLoadedItemsDataView;
        public static List<BulkItemsVM> BulkLoadedItemsList;

        public static int SideBarWidth { get; set; }
        public static int TopBarHeight { get; set; }
        public static int TopMenueBarHeight { get; set; }
        public static List<FlatDiscount> TodayDiscounts { get; set; }



        //variables being used to set and read from diference forms
        public static double TotalNotesAmount { get; set; }
        //---- Batch For Syncing Purpose
        public static readonly int ItemBatch = 50;
        public static readonly int ModuleBatch = 50;
        public static bool IsStockModule { get; set; }
    }
}