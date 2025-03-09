using Pharmacy.Core;
using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace Pharmacy.Infrastructure.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Pharmacy.Infrastructure.PharmacyContext>
    {
        UserRole AdminRole;
        private readonly bool _pendingMigrations;
        public Configuration()
        {

            //if (!SharedVariables.IsClientPC)
            //{
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

            //// Check if there are migrations pending to run, this can happen if database doesn't exists or if there was any
            ////  change in the schema
            //var migrator = new DbMigrator(this);
            //_pendingMigrations = migrator.GetPendingMigrations().Any();

            //// If there are pending migrations run migrator.Update() to create/update the database then run the Seed() method to populate
            ////  the data if necessary
            //if (_pendingMigrations)
            //{
            //    migrator.Update();
            //    Seed(new Pharmacy.Infrastructure.PharmacyContext());
            //}
            //}
        }

        protected override void Seed(Pharmacy.Infrastructure.PharmacyContext context)
        {
            try
            {
                //if (!SharedVariables.IsClientPC)
                //{
                seedDefaultData(context);
                //}
            }
            catch (Exception ex)
            {

            }
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.            
        }

        private void seedDefaultData(Pharmacy.Infrastructure.PharmacyContext context)
        {
            ///
            /// as per new changes
            /// must set default unit to Pack
            /// and
            /// set default conversionUnit to 1
            ///
            try
            {
                List<long> InvoiceRefNos = context.Database.SqlQuery<long>("select InvoiceRefNo from Invoices group by InvoiceRefNo having COUNT(InvoiceRefNo) > 1").ToList();
                if (InvoiceRefNos.Count() > 0)
                {
                    foreach (var RefNo in InvoiceRefNos)
                    {
                        List<Invoice> invoices = context.Invoices.Where(x => x.InvoiceRefNo == RefNo).ToList();
                        foreach (var invoice in invoices)
                        {
                            invoice.InvoiceRefNo = context.Invoices.Max(n => n.InvoiceRefNo) + 1;
                            context.Entry(invoice).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }
                List<decimal> StockRefNos = context.Database.SqlQuery<decimal>("select DocumentNo from Stocks group by DocumentNo having COUNT(DocumentNo) > 1").ToList();
                if (StockRefNos.Count() > 0)
                {
                    foreach (var RefNo in StockRefNos)
                    {
                        List<Stock> stocks = context.Stocks.Where(x => x.DocumentNo == RefNo).ToList();
                        foreach (var stock in stocks)
                        {
                            stock.DocumentNo = context.Stocks.Max(n => n.DocumentNo) + 1;
                            context.Entry(stock).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }

                if (!context.HwData.Any())
                {
                    HwData d = new HwData();
                    d.SystemExpiry = CryptopEngine.Encrypt(DateTime.Now.AddMonths(12).Date.ToString(), GlobalSharing.Salt);
                    context.HwData.Add(d);
                }
                List<UserRole> UserRole = new List<UserRole>();
                if (!context.UserRoles.Any())
                {
                    AdminRole = new UserRole { Description = "Admin", IsActive = true, IsNew = true, IsSynced = false, IsUpdate = false, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsAdmin = true };
                    UserRole.Add(AdminRole);
                    UserRole.Add(new UserRole { Description = "Pharmacist", IsActive = true, IsNew = false, IsSynced = false, IsUpdate = false, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsAdmin = false });
                    UserRole.Add(new UserRole { Description = "Accountant", IsActive = true, IsNew = false, IsSynced = false, IsUpdate = false, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsAdmin = false });
                    UserRole.Add(new UserRole { Description = "Doctor", IsActive = true, IsNew = false, IsSynced = false, IsUpdate = false, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsAdmin = false });
                    UserRole.Add(new UserRole { Description = "Pharmacy Cashier", IsActive = true, IsNew = false, IsSynced = false, IsUpdate = false, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsAdmin = false });
                    UserRole.Add(new UserRole { Description = "Pharmacy Manager", IsActive = true, IsNew = false, IsSynced = false, IsUpdate = false, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsAdmin = false });
                }
                context.UserRoles.AddRange(UserRole);
                List<Right> right = new List<Right>();
                if (!context.Rights.Any())
                {
                    right.Add(new Right { Description = "Create Users" });
                    right.Add(new Right { Description = "Edit Users" });
                    right.Add(new Right { Description = "Delete Users" });
                    right.Add(new Right { Description = "Edit Payment/Invoice Date" });
                    right.Add(new Right { Description = "View Financial Reports" });
                    right.Add(new Right { Description = "Delete Patients" });
                    right.Add(new Right { Description = "Edit Invoice" });
                    right.Add(new Right { Description = "Refund Payment" });
                    right.Add(new Right { Description = "Delete Invoice" });
                    right.Add(new Right { Description = "Add Stock" });
                    right.Add(new Right { Description = "Edit Stock" });
                    right.Add(new Right { Description = "View Pharmacy Report" });
                    right.Add(new Right { Description = "Edit Item" });
                    right.Add(new Right { Description = "Add Item" });
                    right.Add(new Right { Description = "Delete Item" });
                    right.Add(new Right { Description = "Edit Invoice Retail Price" });
                }
                if (!context.Rights.Any(x => x.Description == "Adjust Stock"))
                {
                    right.Add(new Right { Description = "Adjust Stock" });
                }
                context.Rights.AddRange(right);
                if (!context.Users.Any())
                {
                    List<User> ul = new List<User>();
                    List<UserRole> roles = new List<UserRole>();
                    roles.Add(AdminRole);
                    User u = new User { UserId = 1, UserName = "Admin", Password = "Admin@123", Phone = "03082944770", Email = "admin@Belinkwire.com", IsActive = true, IsNew = true, IsUpdate = false, IsSynced = false, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, UserRoles = roles, CanGiveDiscount = true, DiscLimit = 100 };
                    ul.Add(u);
                    context.Users.AddRange(ul);
                    //SOT
                    OfflineTask task = new OfflineTask();
                    task.ModuleName = "User";
                    task.ModuleAction = "Create";
                    task.ModuleJson = "{\r\n  \"$id\": \"1\",\r\n  \"UserId\": 1,\r\n  \"UserName\": \"Admin\",\r\n  \"Password\": \"Admin@123\",\r\n  \"Phone\": \"03082944770\",\r\n  \"Email\": \"admin@Belinkwire.com\",\r\n  \"CreatedById\": 1,\r\n  \"CreateDate\": \"2021-11-03T15:53:52.487\",\r\n  \"UpdateDate\": \"2021-11-03T16:11:00.2228436+05:00\",\r\n  \"SyncedAt\": null,\r\n  \"ShiftId\": null,\r\n  \"Shift\": null,\r\n  \"UserRoles\": [\r\n    {\r\n      \"$id\": \"2\",\r\n      \"UserRoleId\": 1,\r\n      \"Description\": \"Admin\",\r\n      \"CreateDate\": \"2021-11-03T15:53:52.287\",\r\n      \"UpdateDate\": \"2021-11-03T15:53:52.287\",\r\n      \"IsAdmin\": true,\r\n      \"Users\": [\r\n        {\r\n          \"$ref\": \"1\"\r\n        }\r\n      ],\r\n      \"IsActive\": true,\r\n      \"IsNew\": true,\r\n      \"IsUpdate\": false,\r\n      \"IsSynced\": false\r\n    }\r\n  ],\r\n  \"Rights\": [],\r\n  \"AdmittedPatients\": null,\r\n  \"CanGiveDiscount\": true,\r\n  \"DiscLimit\": 100.0,\r\n  \"IsActive\": true,\r\n  \"IsNew\": true,\r\n  \"IsUpdate\": false,\r\n  \"IsSynced\": false\r\n}";
                    task.ModuleId = 1;
                    task.CreatedOn = DateTime.Now;
                    task.IsActive = true;
                    context.OfflineTask.Add(task);
                }

                if (!context.Procedures.Any())
                {

                    Procedure p = new Procedure { ProcedureId = 1, ProcedureName = "default", Price = 0, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsNew = false, IsUpdate = false, IsSynced = true, IsActive = true, IsDefault = true };
                    context.Procedures.AddOrUpdate(p);
                }

                if (!context.Items.Any())
                {
                    Item i = new Item { ItemID = 1, ItemName = "default", Barcode = "default", Unit = "", ReOrderingLevel = 0, OpeningStock = 0, ChecmicalName = "", UnitCostPrice = 0, IsNarcotic = false, IsActive = true, IsNew = false, IsUpdate = false, IsSynced = false, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsDefault = true };
                    context.Items.AddOrUpdate(i);
                }

                if (!context.AdminInvoiceSetting.Any())
                {
                    AdminInvoiceSetting a = new AdminInvoiceSetting { AdminInvoiceSettingId = 1, PrintPageSize = 2, MarginTop = 0, MarginRight = 0, MarginBottom = 0, MarginLeft = 0, FontSize = 11, PageType = 1, PageOrientation = 1, ShowUserName = false, ShowLogoWaterMark = false, HideRateAndQuantityInPrintFormat = false, ShowGrandtotalsInWords = false, GrandTotalsOfInvoiceAsPaymentByDefault = false, HideDuesAndAdvanceFromInvoiceAndPmntReceipts = false, PrintPractiseName = false, InvoiceNote = "" };
                    context.AdminInvoiceSetting.AddOrUpdate(a);
                }

                if (!context.AdminProcedureInvoiceSetting.Any())
                {
                    AdminProcedureInvoiceSetting a = new AdminProcedureInvoiceSetting { AdminProcedureInvoiceSettingId = 1, PrintPageSize = 1, MarginTop = 0, MarginRight = 0, MarginBottom = 0, MarginLeft = 0, FontSize = 11, PageType = 1, PageOrientation = 1, ShowUserName = false, ShowLogoWaterMark = false, HideRateAndQuantityInPrintFormat = false, ShowGrandtotalsInWords = false, GrandTotalsOfInvoiceAsPaymentByDefault = false, HidePaymentsAndDuesFromInvoiceAndPmntReceipts = false, PrintPractiseName = false, InvoiceNote = "" };
                    context.AdminProcedureInvoiceSetting.AddOrUpdate(a);
                }

                if (!context.AdminPractiseSetting.Any())
                {
                    AdminPractiseSetting p = new AdminPractiseSetting { AdminPractiseSettingId = 1, Name = "", Address = "", LogoPath = "", Phone = "", PracticeId = -1, UserDeletedDefaultData = false };
                    context.AdminPractiseSetting.AddOrUpdate(p);
                }
                if (!context.AdminPrintFormatSetting.Any())
                {
                    AdminPrintFormatSetting p = new AdminPrintFormatSetting { AdminPrintFormatSettingId = 1, HeaderFooterPref = 1, PatientDetailsPref = 1 };
                    context.AdminPrintFormatSetting.AddOrUpdate(p);
                }
                if (!context.AdminPharmacySetting.Any())
                {
                    AdminPharmacySetting ps = new AdminPharmacySetting { AdminPharmacySettingId = 1, IsItemConumptionFifo = true, AllowNegCons = false, IsPharmacyTemps = false, IsHolsStockOnInvoiceHold = false, IsItemDefUnitOnPOS = true, IsStockEditableUnitRetailPrice = false };
                    context.AdminPharmacySetting.AddOrUpdate(ps);
                }

                if (!context.Departments.Any())
                {
                    context.Departments.AddOrUpdate(new Core.Entities.Department { DepartmentId = 1, Name = "IPD", CreateDate = DateTime.Now, UpdateDate = DateTime.Now, IsActive = true });
                }
                if (!context.LowStockMails.Any())
                {
                    context.LowStockMails.AddOrUpdate(new LowStockMail { LowStockMailId = 1, LastSent = DateTime.Now, CreatedAt = DateTime.Now });
                }

                if (!context.ShiftsMaster.Any())
                {
                    context.ShiftsMaster.AddOrUpdate(new AdminShiftMasterSetting { AdminShiftMasterSettingId = 1, ShiftsEnabled = false, EnforceLogout = false });
                }
                if (!context.Shifts.Any())
                {
                    context.Shifts.AddOrUpdate(new AdminShiftSetting { AdminShiftSettingId = 1, Name = "Morning", Code = 1, StartTime = new DateTime(2021, 01, 01, 12, 00, 00), EndTime = new DateTime(2021, 01, 01, 13, 00, 00), IsActive = false });
                    context.Shifts.AddOrUpdate(new AdminShiftSetting { AdminShiftSettingId = 2, Name = "Afternoon", Code = 2, StartTime = new DateTime(2021, 01, 01, 12, 00, 00), EndTime = new DateTime(2021, 01, 01, 13, 00, 00), IsActive = false });
                    context.Shifts.AddOrUpdate(new AdminShiftSetting { AdminShiftSettingId = 3, Name = "Evening", Code = 3, StartTime = new DateTime(2021, 01, 01, 12, 00, 00), EndTime = new DateTime(2021, 01, 01, 13, 00, 00), IsActive = false });
                    context.Shifts.AddOrUpdate(new AdminShiftSetting { AdminShiftSettingId = 4, Name = "Night", Code = 4, StartTime = new DateTime(2021, 01, 01, 12, 00, 00), EndTime = new DateTime(2021, 01, 01, 13, 00, 00), IsActive = false });
                }



            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while initializing default data, please try closing and running application again.");
            }
        }
    }
    public class InitializerDB : IDatabaseInitializer<PharmacyContext>
    {
        public void InitializeDatabase(PharmacyContext context)
        {
            //Check Invoice DuplicateRef Number
            List<long> InvoiceRefNos = context.Database.SqlQuery<long>("select InvoiceRefNo from Invoices group by InvoiceRefNo having COUNT(InvoiceRefNo) > 1").ToList();
            if (InvoiceRefNos.Count() > 0)
            {
                foreach (var RefNo in InvoiceRefNos)
                {
                    Invoice invoice = context.Invoices.Where(x => x.InvoiceRefNo == RefNo).FirstOrDefault();
                    invoice.InvoiceRefNo = context.Invoices.Max(n => n.InvoiceRefNo) + 1;
                    context.Entry(invoice).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }
    }

    public class PharmacyDBInitializer
    {
        private DbContext dbContext;

        public PharmacyDBInitializer(PharmacyContext context)
        {
            List<long> InvoiceRefNos = context.Database.SqlQuery<long>("select InvoiceRefNo from Invoices group by InvoiceRefNo having COUNT(InvoiceRefNo) > 1").ToList();
            if (InvoiceRefNos.Count() > 0)
            {
                foreach (var RefNo in InvoiceRefNos)
                {
                    Invoice invoice = context.Invoices.Where(x => x.InvoiceRefNo == RefNo).FirstOrDefault();
                    invoice.InvoiceRefNo = context.Invoices.Max(n => n.InvoiceRefNo) + 1;
                    context.Entry(invoice).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }
    }
}