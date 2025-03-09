using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core.ViewModels;
using System.Data.Entity;
namespace Pharmacy.Infrastructure.Repository
{
    public class GeneralRepository
    {
        private PharmacyContext cxt = null;
        public GeneralRepository(PharmacyContext cxt)
        {
            this.cxt = cxt;
        }

        public IncomeStatementVM GetIncomeStatement(DateTime DateFrom, DateTime DateTo)
        {
            IncomeStatementVM Result = new IncomeStatementVM
            {
                TotalStockPayments = cxt.Stocks
                                      .Where(s => s.IsActive
                                      && DbFunctions.TruncateTime(s.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                      && DbFunctions.TruncateTime(s.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                      )
                                     .Sum(inv => (double?)inv.InvoicePayments.Where(p => p.IsActive)
                                                        .Sum(p => (double?)p.Amount ?? 0)) ?? 0,

                TotalRevenue = (cxt.Invoices
                                               .Where(i => i.IsActive && !i.IsQInvoice
                                                   && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom) &&
                                                   DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                                                    .Sum(inv => (double?)inv.InvoicePayments.Where(p => p.IsActive)
                                                        .Sum(p => (double?)p.Payment ?? 0)) ?? 0)
            };
            return Result;
        }
        public TodaysReportVM GetTodyasReportForDashboard()
        {
            return new TodaysReportVM
            {
                //SalesTotal = cxt.Invoices.Where(i => i.IsActive && DbFunctions.TruncateTime(i.CreateDate) == DbFunctions.TruncateTime(DateTime.Now))
                //            .Sum(i => (double?)i.GrandTotal) ?? 0,

                SalesTotal = cxt.Invoices.Where(i => i.IsActive)
                                .Sum(i => i.InvoicePayments.Where(ip => ip.IsActive
                                 && DbFunctions.TruncateTime(ip.PaymentDate) == DbFunctions.TruncateTime(DateTime.Now)
                                 && ip.PaymentType == 1).Sum(ip => (double?)ip.Payment)) ?? 0,


                TotalRefund = cxt.Invoices.Where(i => i.IsActive).Sum(i => i.InvoicePayments.Where(ip => ip.IsActive
                && DbFunctions.TruncateTime(ip.PaymentDate) == DbFunctions.TruncateTime(DateTime.Now)
                && ip.PaymentType == 1 && ip.Payment < 0).Sum(ip => (double?)ip.Payment)) ?? 0,


                PurchaseOrdersTotal = cxt.PurchaseOrders.Where(po => po.IsActive && DbFunctions.TruncateTime(po.CreateDate) == DbFunctions.TruncateTime(DateTime.Now))
                                      .Sum(po => (double?)po.TotalAmount) ?? 0,

                //CashReceivedTotal = cxt.Invoices.Where(i => i.IsActive && DbFunctions.TruncateTime(i.CreateDate) == DbFunctions.TruncateTime(DateTime.Now))
                //                    .Sum(i => i.InvoicePayments.Where(ip => ip.IsActive
                //                                                    && DbFunctions.TruncateTime(ip.PaymentDate) == DbFunctions.TruncateTime(DateTime.Now)
                //                                                    && ip.PaymentType == 1
                //                                                    ).Sum(ip => (double?)ip.Payment)) ?? 0,

                ChecquePaymentsTotal = cxt.Invoices.Where(i => i.IsActive && DbFunctions.TruncateTime(i.CreateDate) == DbFunctions.TruncateTime(DateTime.Now))
                                    .Sum(i => i.InvoicePayments.Where(ip => ip.IsActive
                                                                    && DbFunctions.TruncateTime(ip.PaymentDate) == DbFunctions.TruncateTime(DateTime.Now)
                                                                    && ip.PaymentType == 5
                                                                    ).Sum(ip => (double?)ip.Payment)) ?? 0,

                DebitCreditTotal = cxt.Invoices.Where(i => i.IsActive && DbFunctions.TruncateTime(i.CreateDate) == DbFunctions.TruncateTime(DateTime.Now))
                                    .Sum(i => i.InvoicePayments.Where(ip => ip.IsActive
                                                                    && DbFunctions.TruncateTime(ip.PaymentDate) == DbFunctions.TruncateTime(DateTime.Now)
                                                                    && ip.PaymentType == 4
                                                                    ).Sum(ip => (double?)ip.Payment)) ?? 0,

                OnlinePaymentsTotal = cxt.Invoices.Where(i => i.IsActive && DbFunctions.TruncateTime(i.CreateDate) == DbFunctions.TruncateTime(DateTime.Now))
                                    .Sum(i => i.InvoicePayments.Where(ip => ip.IsActive
                                                                    && DbFunctions.TruncateTime(ip.PaymentDate) == DbFunctions.TruncateTime(DateTime.Now)
                                                                    && ip.PaymentType == 3
                                                                    ).Sum(ip => (double?)ip.Payment)) ?? 0,
                TotalChargePayment = cxt.Invoices.Where(i => i.IsActive && DbFunctions.TruncateTime(i.CreateDate) == DbFunctions.TruncateTime(DateTime.Now))
                                    .Sum(i => i.InvoicePayments.Where(ip => ip.IsActive
                                                                    && DbFunctions.TruncateTime(ip.PaymentDate) == DbFunctions.TruncateTime(DateTime.Now)
                                                                    && ip.PaymentType == 2
                                                                    ).Sum(ip => (double?)ip.Payment)) ?? 0
            };
        }

        public bool ClearDefaultData()
        {
            try
            {
                cxt.Database.ExecuteSqlCommand(@"
                                                    begin transaction
                                                    go
                                                    truncate table stockitems
                                                    DELETE FROM Stocks
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.Stocks',RESEED, 0)

                                                    --delete invoicepayments
                                                    truncate table invoicepayments

                                                    --delete invoices
                                                    DELETE FROM InvoiceItems
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.InvoiceItems',RESEED, 0)
                                                    DELETE FROM invoices
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.invoices',RESEED, 0)


                                                    --delete stock consumptions
                                                    DELETE FROM StockConsumptionItems
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.StockConsumptionItems',RESEED, 0)
                                                    DELETE FROM StockConsumptions
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.StockConsumptions',RESEED, 0)


                                                    --delete stock adjustments
                                                    DELETE FROM AdjustmentItems
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.AdjustmentItems',RESEED, 0)
                                                    DELETE FROM Adjustments
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.Adjustments',RESEED, 0)

                                                    --delete discounts data related to items
                                                    DELETE FROM DiscountItems
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.DiscountItems',RESEED, 0)
                                                    DELETE FROM FlatDiscounts
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.FlatDiscounts',RESEED, 0)

                                                    --delete purchase orders
                                                    DELETE FROM PurchaseOrderItems
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.PurchaseOrderItems',RESEED, 0)
                                                    DELETE FROM PurchaseOrders
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.PurchaseOrders',RESEED, 0)

                                                    --delete suppliers
                                                    DELETE FROM Suppliers
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.suppliers',RESEED, 0)


                                                    --delete missed sales
                                                    DELETE FROM MissedSales
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.MissedSales',RESEED, 0)

                                                    --delete templateitems
                                                    DELETE FROM TemplateItems
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.TemplateItems',RESEED, 0)

                                                    --delete items
                                                    DELETE FROM Items
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.items',RESEED, 0)

                                                    --delete categories : must be dropped after items are dropped
                                                    DELETE FROM Categories
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.Categories',RESEED, 0)

                                                    --delete manufacturers : must be dropped after items are dropped
                                                    DELETE FROM Manufacturers
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.Manufacturers',RESEED, 0)


                                                    --drop expenses
                                                    DELETE FROM expenses
                                                    DBCC CHECKIDENT ('Pharmacy.dbo.expenses',RESEED, 0)

                                                    commit

                                                ");
                cxt.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<AccTransactionsVM> GetAccountTransactions()
        {
            return (from p in cxt.invoicePayments
                    where p.IsActive
                    orderby p.InvoicePaymentId descending
                    select new AccTransactionsVM { TrDate = p.PaymentDate, Type = p.PaymentType, Amount = p.Payment, TotalAmount = p.Invoice.GrandTotal, Comments = "Sale Trans" })
                    .Union( // supplier payments
                            (from e in cxt.Expenses
                             where e.IsActive
                             orderby e.ExpenseId descending
                             select new AccTransactionsVM { TrDate = e.Date, Type = e.PaymentMode, Amount = e.Amount, TotalAmount = e.Stock.GrandTotal, Comments = "Supplier Payments" })
                             )
                    .Union(
                            (from a in cxt.AccTransactions
                             where a.IsActive
                             orderby a.AccTransactionId descending
                             select new AccTransactionsVM { TrDate = a.TrDate, Type = 0, Amount = a.Amount, TotalAmount = a.Amount, Comments = a.Description })
                    ).ToList();
        }
    }
}