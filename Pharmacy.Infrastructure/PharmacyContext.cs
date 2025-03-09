using Pharmacy.Core;
using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure
{
    public class PharmacyContext : DbContext
    {
        //static string ConString = @"Server=DESKTOP-TNQ3SDJ\SQLEXPRESS;Database=Pharmacy;User Id=sa;Password=healthwire1122;Integrated Security=false;Persist Security Info=true;";
        public PharmacyContext()
            : base(Shared.GetConnectionString())
        {
            //if (!SharedVariables.IsClientPC)
            //{
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PharmacyContext, Pharmacy.Infrastructure.Migrations.Configuration>());
            //Database.SetInitializer<PharmacyContext>(new Pharmacy.Infrastructure.Migrations.InitializerDB());
            //}
            //Database.SetInitializer(new CreateDatabaseIfNotExists<PharmacyContext>());
            // Database.SetInitializer(new DropCreateDatabaseAlways<PharmacyContext>());
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<StockConsumptionItem> StockConsumptionItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> invoiceItems { get; set; }
        public DbSet<ChequeInfo> Chequeinfos { get; set; }
        public DbSet<ChargeInfo> Chargeinfos { get; set; }
        public DbSet<InvoicePayment> invoicePayments { get; set; }
        public DbSet<Adjustment> Adjustments { get; set; }
        public DbSet<AdjustmentItem> AdjustmentItems { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<StockReturn> StockReturns { get; set; }
        public DbSet<StockReturnItem> StockReturnItems { get; set; }
        public DbSet<SalesReturn> SalesReturn { get; set; }
        public DbSet<SalesReturnItem> SalesReturnItem { get; set; }
        //public DbSet<ReturnStockItem> ReturnStockItems { get; set; }
        public DbSet<Right> Rights { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<AdminInvoiceSetting> AdminInvoiceSetting { get; set; }
        public DbSet<StockConsumption> StockConsumptions { get; set; }
        public DbSet<AdminPractiseSetting> AdminPractiseSetting { get; set; }
        public DbSet<AdminProcedureInvoiceSetting> AdminProcedureInvoiceSetting { get; set; }
        public DbSet<AdminPrintFormatSetting> AdminPrintFormatSetting { get; set; }
        public DbSet<AdminPharmacySetting> AdminPharmacySetting { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<SubDepartment> SubDepartments { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<LowStockMail> LowStockMails { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<AdmittedPatient> AdmitPatients { get; set; }
        public DbSet<Vital> Vitals { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<HwData> HwData { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<StockAudit> StockAudits { get; set; }
        public DbSet<StockAuditDetail> StockAuditDetails { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<RackItem> RackItems { get; set; }
        public DbSet<FlatDiscount> FlatDiscount { get; set; }
        public DbSet<DiscountItem> DiscountItem { get; set; }
        public DbSet<MissedSale> MissedSales { get; set; }
        public DbSet<AdminShiftMasterSetting> ShiftsMaster { get; set; }
        public DbSet<AdminShiftSetting> Shifts { get; set; }
        public DbSet<POSReceivedCash> POSReceivedCash { get; set; }
        public DbSet<POSSkimmedCash> POSSkimmedCash { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
        public DbSet<POSClosing> POSClosings { get; set; }
        public DbSet<AccTransaction> AccTransactions { get; set; }
        public DbSet<StoreClosing> StoreClosings { get; set; }
        public DbSet<OfflineTask> OfflineTask { get; set; }
        public DbSet<AccountExpenseCategory> AccountExpenseCategory { get; set; }
        public DbSet<AccountExpense> AccountExpenses { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AdminFBRSetting> AdminFBRSettings { get; set; }
        public DbSet<PatientFormFeilds> PatientFormFeilds { get; set; }
        public DbSet<AdminSystemSlotSetting> AdminSystemSlotSetting { get; set; }
        public DbSet<ProfitStatementTransactions> ProfitStatementTransactions { get; set; }
        public DbSet<HwFileData> HwFileData { get; set; }
        public DbSet<InvoiceSetting> InvoiceSettings { get; set; }
        public DbSet<OnlineOrder> OnlineOrder { get; set; }
        public DbSet<OnlineOrderItem> OnlineOrderItem { get; set; }
        public DbSet<OnlineCustomer> OnlineCustomer { get; set; }
        public DbSet<OrderVoucher> OrderVoucher { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
       {
            modelBuilder.Entity<PurchaseOrder>().Property(p => p.PurchaseOrderNo).HasPrecision(18, 0);
            modelBuilder.Entity<Patient>().Property(p => p.MRNo).HasPrecision(18, 0);
            modelBuilder.Entity<Patient>().HasIndex(p => p.MRNo).IsUnique(true);
            //modelBuilder.Entity<Item>().HasIndex(i => i.Barcode).IsUnique(true);
            //modelBuilder.Entity<Item>().HasIndex(i=>i.ItemName).IsUnique(true);
            modelBuilder.Entity<Supplier>().HasIndex(s => s.Name).IsUnique(true);
            modelBuilder.Entity<Supplier>().HasIndex(s => s.Name).IsUnique(true);
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique(true);
            //modelBuilder.Entity<Invoice>().HasIndex(c => c.InvoiceRefNo).IsUnique(true);
            modelBuilder.Entity<Ward>().HasIndex(w => w.Name).IsUnique(true);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique(true);
            modelBuilder.Entity<PatientFormFeilds>().HasIndex(u => u.Id).IsUnique(true);
            base.OnModelCreating(modelBuilder);
        }
    }
    
}