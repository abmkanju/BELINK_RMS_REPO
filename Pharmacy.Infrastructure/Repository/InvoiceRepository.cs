using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core.Entities;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.ViewModels;
using Pharmacy.Core;
using System.Data.Entity;
using PagedList;
using System.Data.SqlClient;
using System.Data.Entity.SqlServer;
namespace Pharmacy.Infrastructure.Repository
{
    public class InvoiceRepository : Repository<Invoice>
    {
        UnitOfWork unitOfWork;
        PharmacyContext cxt = null;
        public InvoiceRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
        public long InsertInvoice(Invoice objInvoice)
        {
            cxt.Invoices.Add(objInvoice);
            cxt.SaveChanges();
            return objInvoice.InvoiceId;
        }

        public long GetInvoiceRefNo()
        {
            long invRefNo = 0;
            if (cxt.Invoices.FirstOrDefault() == null || cxt.Invoices.OrderByDescending(i => i.InvoiceId).FirstOrDefault().InvoiceRefNo == 0)
            {
                invRefNo = 100001;
            }
            else
            {
                invRefNo = cxt.Invoices.Max(n => n.InvoiceRefNo) + 1;
            }
            return invRefNo;
        }
        public InvoiceVM GetPatient_LastInvoiceDetail(long PatientId)
        {
            //return cxt.Invoices.Include("InvoiceItems").Where(i => i.Patient.PatientId == PatientId).OrderByDescending(i => i.InvoiceId).FirstOrDefault();
            InvoiceVM result =
                cxt.Invoices
                .Where(i => i.Patient.PatientId == PatientId)
           .Select(invoice => new InvoiceVM
           {
               InvoiceId = invoice.InvoiceId,
               CreateDate = invoice.CreateDate,
               //subt = order.Supplier.Name,
               SubTotal = invoice.SubTotal,
               GrandTotal = invoice.GrandTotal,
               TotalDiscount = invoice.TotalDiscount,
               InvoiceItems = (invoice.InvoiceItems.Select(i => new InvoiceItemVM
               {
                   ItemName = i.Item.ItemName,
                   Amount = i.Amount
               }))
           }).OrderByDescending(i => i.InvoiceId).FirstOrDefault();
            return result;
        }
        public List<StockAddFilterVM> LoadSaleReturn(int ItemId, int BatchId)
        {
            try
            {
                return cxt.invoiceItems
                     .Where(i => i.Item.ItemID == ItemId && i.Batch.BatchId == BatchId)
                    .GroupBy(i => new { i.Item })
                    .Select(i => new StockAddFilterVM
                    {
                        ItemId = i.Key.Item.ItemID,
                        ItemName = i.Key.Item.ItemName,
                        Unit = i.Key.Item.Unit,
                        ConversionUnit = i.Key.Item.ConversionUnit,
                        InvoiceItemId = (from ii1 in cxt.invoiceItems join iv1 in cxt.Invoices on ii1.Invoice.InvoiceId equals iv1.InvoiceId where ii1.IsActive && iv1.IsActive && ii1.Item.ItemID == i.Key.Item.ItemID select ii1.InvoiceItemId).FirstOrDefault(),
                        PerUnitCostPrice = (from ii1 in cxt.invoiceItems join iv1 in cxt.Invoices on ii1.Invoice.InvoiceId equals iv1.InvoiceId where ii1.IsActive && iv1.IsActive && ii1.Item.ItemID == i.Key.Item.ItemID select ii1.PerUnitCostPrice).FirstOrDefault(),
                        InvoiceItemQty = (from ii1 in cxt.invoiceItems join iv1 in cxt.Invoices on ii1.Invoice.InvoiceId equals iv1.InvoiceId where ii1.IsActive && iv1.IsActive && ii1.Item.ItemID == i.Key.Item.ItemID select ii1.Quantity).FirstOrDefault(),
                        UnitCost =
                        SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                            (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId ascending select si.UnitCost).FirstOrDefault()
                            :
                            (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId descending select si.UnitCost).FirstOrDefault(),
                        RetailPrice =
                        SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                        (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId ascending select si.RetailPrice).FirstOrDefault()
                        :
                        (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault(),

                        Quantity = i.Sum(gp => gp.Quantity - gp.ReturnedQuantity),
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.Key.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.Key.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        //BatchId = (from bi in cxt.StockItems join bii in cxt.Batches on bi.Batch.BatchId equals bii.BatchId where bi.IsActive && bi.Item.ItemID == i.Key.Item.ItemID select new { BatchId = bi.Batch.BatchId }),
                        BatchId = (from si in cxt.StockItems where si.IsActive && si.Item.ItemID == i.Key.Item.ItemID select si.Batch.BatchId).ToList(),
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.Key.Item.ItemID && s.Batch.BatchId == BatchId select new { qty = s.Quantity }).Sum(s => (int?)s.qty) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.Key.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { qty = s.Quantity, BQty = s.BonusQuantity }).Sum(s => (int?)(s.qty + s.BQty)) ?? 0,
                        ExpiredConsStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.Key.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == i.Key.Item.ItemID select new { qty = ai.Quantity }).Sum(ai => (int?)ai.qty) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn =
                          (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.Key.Item.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.Key.Item.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<InvoiceVM> GetInvoiceByInvoiceIdVoucher(string VoucherName)
        {
            List<InvoiceVM> result =
                cxt.Invoices.Where(i => i.VoucherName == VoucherName && i.IsVoucherPaid == false)
                .Select(i => new InvoiceVM
                {
                    InvoiceId = i.InvoiceId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreateDate = i.CreateDate,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    ModifiedDiscount = i.ModifiedDiscount,
                    DiscountType = i.DiscountType,
                    SubTotal = i.SubTotal,
                    GrandTotal = i.GrandTotal,
                    Store = i.Store,
                    TotalPaidAmount = i.TotalPaid,
                    Services = i.Services,
                    ServiceType = i.ServiceType,
                    TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive).Sum(p => p.Payment) ?? 0,
                    Due = i.Due,
                    Note = i.Note,
                    Patient = new PatientVM
                    {
                        PatientId = i.Patient == null ? 0 : i.Patient.PatientId,
                        Name = i.Patient == null ? "" : i.Patient.Name,
                        Phone = i.Patient == null ? "" : i.Patient.Phone,
                        Email = i.Patient == null ? "" : i.Patient.Email,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        RegistrationDate = DateTime.Now
                    },
                    InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                    {
                        IsActive = item.IsActive,
                        InvoiceItemId = item.InvoiceItemId,
                        Item = item.Item,
                        ItemId = item.Item.ItemID,
                        ItemName = item.Item.ItemName,
                        Unit = item.Unit,
                        UnitCost = item.PerUnitCostPrice,
                        Rate = item.Rate,
                        Rack = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == item.Item.ItemID select ri.Rack.Name).FirstOrDefault(),
                        Quantity = item.Quantity,
                        BonusQuantity = item.BonusQuantity,
                        Discount = item.Discount,
                        DiscountType = item.DiscountType,
                        DiscountTypeString = item.DiscountType == 1 ? "%" : "Value",
                        DiscountedPrice = item.DiscountPrice,
                        Amount = item.Amount,
                        NetAmount = item.NetAmount,
                        Batch = item.Batch,
                        BatchId = item.Batch == null ? 0 : item.Batch.BatchId,
                        BatchName = item.Batch.BatchName,
                        ReturnedQuantity = item.ReturnedQuantity,
                        AffectStock = item.AffectStock,
                        SaleTax = item.saletax.ToString(),
                        SaleTaxType = item.saletaxType
                        //UnitCost = item.unit
                    })),
                    InvoicePayments = (i.InvoicePayments.Where(p => p.IsActive).Select(p => new InvoicePaymentVM
                    {
                        InvoicePaymentId = p.InvoicePaymentId,
                        PaymentType = p.PaymentType,
                        Payment = p.Payment,
                        ChequeNumber = p.ChequeNumber,
                        BankName = p.BankName,
                        ChequeStatus = p.ChequeStatus,
                        PaymentDate = p.PaymentDate,
                    }))
                }).ToList();
            return result;
        }
        public PatientVM GetPatienInvoices(long PatientId, int PageNo, int Pagesize)
        {
            PatientVM result =
                cxt.Patients.Where(p => p.PatientId == PatientId)
                .Select(p => new PatientVM
                {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    MRNo = p.MRNo,
                    Invoices = (p.Invoices.Where(i => i.IsActive && !i.IsQInvoice).Select(i => new InvoiceVM
                    {
                        InvoiceId = i.InvoiceId,
                        InvoiceRefNo = i.InvoiceRefNo,
                        SubTotal = i.SubTotal,
                        InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(iItem => new InvoiceItemVM
                        {
                            InvoiceItemId = iItem.InvoiceItemId,

                            ItemName = iItem.Item.ItemName
                        })).ToList(),
                        InvoicePayments = (i.InvoicePayments.Where(ip => ip.IsActive).Select(ip => new InvoicePaymentVM
                        {
                            Payment = ip.Payment,
                            PaymentDate = ip.PaymentDate
                        })).ToList(),
                        Due = i.Due,
                    })).ToList(),
                    Tags = p.Tags.ToList()
                }).FirstOrDefault();
            return result;
        }

        public PatientVM GetPatienInvoices_Rpt(long PatientId)
        {
            PatientVM result =
                cxt.Patients.Where(p => p.PatientId == PatientId)
                .Select(p => new PatientVM
                {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    MRNo = p.MRNo,
                    Invoices = (p.Invoices.Where(i => i.IsActive && !i.IsQInvoice).Select(i => new InvoiceVM
                    {
                        InvoiceId = i.InvoiceId,
                        InvoiceRefNo = i.InvoiceRefNo,
                        SubTotal = i.SubTotal,
                        InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(iItem => new InvoiceItemVM
                        {
                            InvoiceItemId = iItem.InvoiceItemId,
                            ItemName = iItem.Item.ItemName
                        })).ToList(),
                        InvoicePayments = (i.InvoicePayments.Where(ip => ip.IsActive).Select(ip => new InvoicePaymentVM
                        {
                            Payment = ip.Payment,
                            PaymentDate = ip.PaymentDate
                        })).ToList(),
                        Due = i.Due,
                    })).ToList(),
                    Tags = p.Tags.ToList()
                }).FirstOrDefault();
            return result;
        }

        public int GetHoldingInvoicesCount()
        {
            return cxt.Invoices.Where(i => i.IsHoldInvoice && i.IsActive && !i.IsQInvoice).Count();
        }

        public int LagInvoiceId(int invoiceId)
        {
            //string qry = "select lag(invoiceid) from invoices where invoiceid = " + invoiceId;
            //int id = cxt.Database.SqlQuery<int>(qry);
            return 1;
        }
        public int LeadInvoiceId(int invoiceId)
        {
            return 0;
        }
        public Invoice GetInvoicceByIdWithDetails(long InvoiceId)
        {
            return cxt.Invoices
                .Where(i => i.InvoiceId == InvoiceId)
                .Include(i => i.InvoicePayments)
                .Include(i => i.Patient)
                .Where(i => i.InvoicePayments.Any(p => p.IsActive))
                .Include(i => i.InvoiceItems)
                .Where(i => i.InvoiceItems.Any(r => r.IsActive))
                .Include(i => i.StockConsumption)
                .Include(i => i.StockConsumption.StockConsumptions)
                .Include(i => i.StockConsumption.StockConsumptions.Select(sc => sc.Item))
                .Include(i => i.StockConsumption.StockConsumptions.Select(sc => sc.Batch))
                //.Include(i => i.StockConsumptions)
                //.Where(i => i.StockConsumptions.Any(c => c.IsActive))
                //.Include(i => i.StockConsumptions.Select(c => c.Item))
                //.Include(i => i.StockConsumptions.Select(c => c.Batch))
                .FirstOrDefault();
        }
        // alternate to above method, but it will require changes on POs screens as well
        public InvoiceVM GetInvoiceDetailById_Edit(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                FirstPayment = i.InvoicePayments.OrderBy(p => p.InvoicePaymentId).Select(p => p.Payment).FirstOrDefault(),
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                ObjPatient = i.Patient,
                InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                {
                    Batch = item.Batch,
                    Item = item.Item,
                    ItemName = item.Item.ItemName,
                    Rate = item.Rate,
                    Quantity = item.Quantity,
                    Amount = item.Amount
                })).ToList(),
                InvoicePayments = (i.InvoicePayments.Where(p => p.IsActive && p.Payment >= 0).OrderBy(p => p.InvoicePaymentId)
                .Select(p => new InvoicePaymentVM
                {
                    InvoicePaymentId = p.InvoicePaymentId,
                    PaymentType = p.PaymentType,
                    Payment = p.Payment,
                    PaymentDate = p.PaymentDate,
                    RefundReason = p.RefundReason,
                }))
            }).FirstOrDefault();
        }
        public InvoiceVM GetInvoiceById_IncludeDetails(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                TotalPaid = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == InvoiceId).Sum(p => p.Payment)),
                InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                {
                    ItemName = item.Item.ItemName,
                    Rate = item.Rate,
                    Quantity = item.Quantity,
                    Amount = item.Amount
                })).ToList()
            }).FirstOrDefault();
        }
        public InvoiceVM GetInvoiceByIdDetails_IncPatient(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                PaymentType = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentType).FirstOrDefault()),
                TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                Patient = (new PatientVM
                {
                    MRNo = i.Patient.MRNo,
                    Name = i.Patient.Name,
                    Phone = i.Patient.Phone,
                    DateOfBirth = i.Patient.DateOfBirth,
                    Gender = i.Patient.Gender,
                    AgeYears = i.Patient.AgeYears ?? 0,
                    AgeMonths = i.Patient.AgeMonths ?? 0,
                    AgeDays = i.Patient.AgeDays ?? 0
                }),
                InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                {
                    ItemName = item.Item.ItemName,
                    Rate = item.Rate,
                    Quantity = item.Quantity,
                    Amount = item.Amount
                })).ToList()
            }).FirstOrDefault();
        }
        public InvoiceVM GetProc_InvoiceDetailsById(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                CreateDate = i.CreateDate,
                //TotalAmount = i.TotalAmount,
                //TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                // SubTotal = i.SubTotal,
                //Due = i.Due,
                //Advance = i.Advance,
                //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                PaymentType = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentType).FirstOrDefault()),
                // TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                Patient = (new PatientVM
                {
                    MRNo = i.Patient.MRNo,
                    Name = i.Patient.Name,
                    Phone = i.Patient.Phone,
                    DateOfBirth = i.Patient.DateOfBirth,
                    Gender = i.Patient.Gender,
                    AgeYears = i.Patient.AgeYears ?? 0,
                    AgeMonths = i.Patient.AgeMonths ?? 0,
                    AgeDays = i.Patient.AgeDays ?? 0
                }),
                InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                {
                    ProcedureName = item.Procedure.ProcedureName,
                })).ToList()
            }).FirstOrDefault();
        }
        public InvoiceVM GetPOSInvoiceDetailById(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.IsActive && i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                FBRCode = i.FBRCode == null ? "" : i.FBRCode,
                FBRInvoiceNo = i.FBRInvoiceNo == null ? "" : i.FBRInvoiceNo,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                Services = i.Services,
                InvoiceType = i.IsQInvoice ? "Estimate Bill Invoice" : "Sale Invoice",
                ServiceType = i.ServiceType,
                CashBack = i.TotalPaid - (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                FirstPayment = i.InvoicePayments.OrderBy(p => p.InvoicePaymentId).Select(p => p.Payment).FirstOrDefault(),
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                ObjPatient = i.Patient,
                Note = i.Note,
                InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                {
                    ItemName = item.Item.ItemName,
                    BatchName = item.Batch.BatchName,
                    ConUnit = item.Item.ConversionUnit,
                    Unit = item.Unit,
                    UnitString = item.Unit == 1 ? "Unit" : item.Item.Unit,//"Pack",
                    Discount = item.Discount,
                    DiscountType = item.DiscountType,
                    Description = item.ReturnedQuantity > 0 ? item.ReturnedQuantity.ToString() : "0", //For Reporting Purpose
                    ProcedureName = item.Discount > 0 ? item.Discount.ToString() + (item.DiscountType == 1 ? "%" : "") : "0", //For Reporting Purpose
                    ReturnedQuantity = item.ReturnedQuantity,
                    SaleTax = item.saletax.ToString(),
                    SaleTaxType = item.saletaxType,
                    BatchExpiry =
                    item.Batch.Expiry.HasValue == true ? SqlFunctions.DateName("day", item.Batch.Expiry.Value) + "-"
                    + DbFunctions.Right("00" + item.Batch.Expiry.Value.Month, 2) + "-"
                    + SqlFunctions.DateName("year", item.Batch.Expiry.Value) :
                    "",
                    Rate = item.Rate,
                    Quantity = item.Quantity,
                    InvoiceType = item.IsQInvoice ? "Estimate Bill Invoice" : "Sale Invoice",
                    BonusQuantity = item.BonusQuantity,
                    Amount = item.Amount,
                    NetAmount = item.NetAmount,
                })).ToList(),
                InvoicePayments = (i.InvoicePayments.Where(p => p.IsActive && p.Payment >= 0).OrderBy(p => p.InvoicePaymentId)
                .Select(p => new InvoicePaymentVM
                {
                    PaymentDate = p.PaymentDate,
                    PaymentType = p.PaymentType,
                    PaymentString = "Rs. " + Math.Round(p.Payment, 0) + "/-",
                    Payment = Math.Round(p.Payment, 0)
                }))
            }).FirstOrDefault();
        }
        public InvoiceVM GetDeletedInvoiceDetailById(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => !i.IsActive && i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                FBRCode = i.FBRCode == null ? "" : i.FBRCode,
                FBRInvoiceNo = i.FBRInvoiceNo == null ? "" : i.FBRInvoiceNo,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                Services = i.Services,
                InvoiceType = i.IsQInvoice ? "Estimate Bill Invoice" : "Sale Invoice",
                ServiceType = i.ServiceType,
                CashBack = i.TotalPaid - (i.InvoicePayments.Where(p => !p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                FirstPayment = i.InvoicePayments.OrderBy(p => p.InvoicePaymentId).Select(p => p.Payment).FirstOrDefault(),
                PaymentDate = (i.InvoicePayments.Where(p => !p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                RefundDate = (i.InvoicePayments.Where(p => !p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => !p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                TotalRefund = -1 * (i.InvoicePayments.Where(p => !p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                ObjPatient = i.Patient,
                Note = i.Note,
                InvoiceItems = (i.InvoiceItems.Where(ii => !ii.IsActive).Select(item => new InvoiceItemVM
                {
                    ItemName = item.Item.ItemName,
                    BatchName = item.Batch.BatchName,
                    ConUnit = item.Item.ConversionUnit,
                    Unit = item.Unit,
                    UnitString = item.Unit == 1 ? "Unit" : item.Item.Unit,//"Pack",
                    Discount = item.Discount,
                    DiscountType = item.DiscountType,
                    Description = item.ReturnedQuantity > 0 ? item.ReturnedQuantity.ToString() : "0", //For Reporting Purpose
                    ProcedureName = item.Discount > 0 ? item.Discount.ToString() + (item.DiscountType == 1 ? "%" : "") : "0", //For Reporting Purpose
                    ReturnedQuantity = item.ReturnedQuantity,
                    SaleTax = item.saletax.ToString(),
                    SaleTaxType = item.saletaxType,
                    BatchExpiry =
                    item.Batch.Expiry.HasValue == true ? SqlFunctions.DateName("day", item.Batch.Expiry.Value) + "-"
                    + DbFunctions.Right("00" + item.Batch.Expiry.Value.Month, 2) + "-"
                    + SqlFunctions.DateName("year", item.Batch.Expiry.Value) :
                    "",
                    Rate = item.Rate,
                    Quantity = item.Quantity,
                    InvoiceType = item.IsQInvoice ? "Estimate Bill Invoice" : "Sale Invoice",
                    BonusQuantity = item.BonusQuantity,
                    Amount = item.Amount,
                    NetAmount = item.NetAmount,
                })).ToList(),
                InvoicePayments = (i.InvoicePayments.Where(p => !p.IsActive && p.Payment >= 0).OrderBy(p => p.InvoicePaymentId)
                .Select(p => new InvoicePaymentVM
                {
                    PaymentDate = p.PaymentDate,
                    PaymentType = p.PaymentType,
                    PaymentString = "Rs. " + Math.Round(p.Payment, 0) + "/-",
                    Payment = Math.Round(p.Payment, 0)
                }))
            }).FirstOrDefault();
        }
        public InvoiceVM GetPOSInvoiceById(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.IsActive && i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                CreateDate = i.CreateDate,
                //TotalAmount = i.TotalAmount,
                //TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                //SubTotal = i.SubTotal,
                Due = i.Due,
                //FirstPayment = i.InvoicePayments.OrderBy(p => p.InvoicePaymentId).Select(p => p.Payment).FirstOrDefault(),
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                PaymentType = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentType).FirstOrDefault()),
                ObjPatient = i.Patient,
                Note = i.Note,
                InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                {
                    ItemName = item.Item.ItemName,
                    //Rate = item.Rate,
                    //Quantity = item.Quantity,
                    //Amount = item.Amount
                })).ToList()
                //InvoicePayments = (i.InvoicePayments.Where(p => p.IsActive && p.Payment >= 0).OrderBy(p => p.InvoicePaymentId)
                //.Select(p => new InvoicePaymentVM
                //{
                //    Date = p.Date,
                //    PaymentType = p.PaymentType,
                //    PaymentString = "Rs. " + p.Payment + "/-"
                //}))
            }).FirstOrDefault();
        }
        public InvoiceVM GetProcedureInvoiceDetailById(long InvoiceId)
        {
            return cxt.Invoices
            .Where(i => i.InvoiceId == InvoiceId)
            .Select(i => new InvoiceVM
            {
                InvoiceId = i.InvoiceId,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                FirstPayment = i.InvoicePayments.OrderBy(p => p.InvoicePaymentId).Select(p => p.Payment).FirstOrDefault(),
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                ObjPatient = i.Patient,
                InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                {
                    ProcedureName = item.Procedure.ProcedureName,
                    Rate = item.Rate,
                    Quantity = item.Quantity,
                    Amount = item.Amount
                })).ToList(),
                InvoicePayments = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderBy(p => p.InvoicePaymentId)
                .Select(p => new InvoicePaymentVM
                {
                    PaymentDate = p.PaymentDate,
                    PaymentType = p.PaymentType,
                    PaymentString = "Rs. " + p.Payment + "/-"
                }))
            }).FirstOrDefault();
        }



        public IPagedList<InvoiceTransactionsVM> GetTransactions_Rpt(int PageNo, int PageSize)
        {
            //return cxt.Invoices
            //.Where(i => i.IsActive)
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
            //    && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
            //)
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice,
            //    //InvoicePayments = i.InvoicePayments.ToList(),
            //}).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);  

            return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where i.IsActive && !i.IsQInvoice
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     PaymentCreateDate = p.CreatedAt,
                     TotalPaid = p.Payment,
                     Services = i.Services,
                     ServiceType = i.ServiceType,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(p => p.PaymentDate).ToPagedList(PageNo, PageSize);

            // this query was being used to show data when multiple refund payments were being inserted and it was required to show single transaction for refund while multiple transactions for customer payments
            //return
            //    ((from i in cxt.Invoices
            //     join p in cxt.invoicePayments on i equals p.Invoice
            //     where p.IsActive
            //     && DbFunctions.TruncateTime(p.Date) >= DbFunctions.TruncateTime(DateFrom)
            //     && (DbFunctions.TruncateTime(p.Date) <= DbFunctions.TruncateTime(DateTo))
            //     && p.Payment > 0
            //     select new InvoiceTransactionsVM
            //     {
            //         InvoiceId = i.InvoiceId,
            //         CreateDate = i.CreateDate,
            //         TotalAmount = i.TotalAmount,
            //         TotalDiscount = i.TotalDiscount,
            //         DiscountType = i.DiscountType,
            //         GrandTotal = i.GrandTotal,
            //         SubTotal = i.SubTotal,
            //         Due = i.Due,
            //         Advance = i.Advance,
            //         SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //         ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //         PaymentDate = p.Date,
            //         TotalPaid = p.Payment,
            //         //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //         //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //         //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
            //         Patient = i.Patient,
            //         PatientName = i.Patient.Name,
            //         IsProcedureInvoice = i.IsProcedureInvoice,
            //         //InvoicePayments = i.InvoicePayments.ToList(),
            //     })
            //     .Union
            //    (from i in cxt.Invoices
            //     join p in cxt.invoicePayments on i equals p.Invoice
            //     where p.IsActive
            //     && DbFunctions.TruncateTime(p.Date) >= DbFunctions.TruncateTime(DateFrom)
            //     && (DbFunctions.TruncateTime(p.Date) <= DbFunctions.TruncateTime(DateTo))
            //     && p.Payment < 0
            //     group i by i into g
            //     select new InvoiceTransactionsVM
            //     {
            //         InvoiceId = g.Key.InvoiceId,
            //         CreateDate = g.Key.CreateDate,
            //         TotalAmount = g.Key.TotalAmount,
            //         TotalDiscount = g.Key.TotalDiscount,
            //         DiscountType = g.Key.DiscountType,
            //         GrandTotal = g.Key.GrandTotal,
            //         SubTotal = g.Key.SubTotal,
            //         Due = g.Key.Due,
            //         Advance = g.Key.Advance,
            //         SaleQuantity = g.Key.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //         ReturnedQty = g.Key.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //         PaymentDate = g.Key.InvoicePayments.Where(ip => ip.Payment < 0).Select(r => r.Date).FirstOrDefault(),
            //         TotalPaid = (double?)g.Key.InvoicePayments.Where(ip=>ip.Payment<0 && ip.IsActive).Sum(ip => ip.Payment) ?? 0,
            //         //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //         //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //         //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
            //         Patient = g.Key.Patient,
            //         PatientName = g.Key.Patient.Name,
            //         IsProcedureInvoice = g.Key.IsProcedureInvoice,
            //         //InvoicePayments = i.InvoicePayments.ToList(),
            //     })).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            ////return null;
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions_Rpt(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize, double InvoiceRef = 0, long Patientid = 0)
        {

            //return cxt.Invoices
            //.Where(i => i.IsActive)
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
            //    && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
            //)
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice,
            //    //InvoicePayments = i.InvoicePayments.ToList(),
            //}).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);  
            if (InvoiceRef > 0)
            {
                if (Patientid > 0)
                {
                    return
           (from i in cxt.Invoices
            join p in cxt.invoicePayments on i equals p.Invoice
            where p.IsActive && !i.IsQInvoice && InvoiceRef == i.InvoiceRefNo && Patientid == i.Patient.PatientId
            && (DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom))
            && (DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo))
            select new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                PaymentDate = p.PaymentDate,
                PaymentCreateDate = p.CreatedAt,
                TotalPaid = p.Payment,
                Services = i.Services,
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                //InvoicePayments = i.InvoicePayments.ToList(),
            }).OrderByDescending(p => p.PaymentDate).ToPagedList(PageNo, PageSize);

                }
                else
                {
                    return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && !i.IsQInvoice && InvoiceRef == i.InvoiceRefNo
                 && (DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom))
                 && (DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo))
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     PaymentCreateDate = p.CreatedAt,
                     TotalPaid = p.Payment,
                     Services = i.Services,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(p => p.PaymentDate).ToPagedList(PageNo, PageSize);

                }
            }
            else
            {
                if (Patientid > 0)
                {
                    return
           (from i in cxt.Invoices
            join p in cxt.invoicePayments on i equals p.Invoice
            where p.IsActive && !i.IsQInvoice && Patientid == i.Patient.PatientId
            && (DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom))
            && (DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo))
            select new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                PaymentDate = p.PaymentDate,
                PaymentCreateDate = p.CreatedAt,
                TotalPaid = p.Payment,
                Services = i.Services,
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                //InvoicePayments = i.InvoicePayments.ToList(),
            }).OrderByDescending(p => p.PaymentDate).ToPagedList(PageNo, PageSize);

                }
                else
                {
                    return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && !i.IsQInvoice
                 && (DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom))
                 && (DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo))
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     PaymentCreateDate = p.CreatedAt,
                     TotalPaid = p.Payment,
                     Services = i.Services,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(p => p.PaymentDate).ToPagedList(PageNo, PageSize);

                }
            }

            // this query was being used to show data when multiple refund payments were being inserted and it was required to show single transaction for refund while multiple transactions for customer payments
            //return
            //    ((from i in cxt.Invoices
            //     join p in cxt.invoicePayments on i equals p.Invoice
            //     where p.IsActive
            //     && DbFunctions.TruncateTime(p.Date) >= DbFunctions.TruncateTime(DateFrom)
            //     && (DbFunctions.TruncateTime(p.Date) <= DbFunctions.TruncateTime(DateTo))
            //     && p.Payment > 0
            //     select new InvoiceTransactionsVM
            //     {
            //         InvoiceId = i.InvoiceId,
            //         CreateDate = i.CreateDate,
            //         TotalAmount = i.TotalAmount,
            //         TotalDiscount = i.TotalDiscount,
            //         DiscountType = i.DiscountType,
            //         GrandTotal = i.GrandTotal,
            //         SubTotal = i.SubTotal,
            //         Due = i.Due,
            //         Advance = i.Advance,
            //         SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //         ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //         PaymentDate = p.Date,
            //         TotalPaid = p.Payment,
            //         //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //         //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //         //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
            //         Patient = i.Patient,
            //         PatientName = i.Patient.Name,
            //         IsProcedureInvoice = i.IsProcedureInvoice,
            //         //InvoicePayments = i.InvoicePayments.ToList(),
            //     })
            //     .Union
            //    (from i in cxt.Invoices
            //     join p in cxt.invoicePayments on i equals p.Invoice
            //     where p.IsActive
            //     && DbFunctions.TruncateTime(p.Date) >= DbFunctions.TruncateTime(DateFrom)
            //     && (DbFunctions.TruncateTime(p.Date) <= DbFunctions.TruncateTime(DateTo))
            //     && p.Payment < 0
            //     group i by i into g
            //     select new InvoiceTransactionsVM
            //     {
            //         InvoiceId = g.Key.InvoiceId,
            //         CreateDate = g.Key.CreateDate,
            //         TotalAmount = g.Key.TotalAmount,
            //         TotalDiscount = g.Key.TotalDiscount,
            //         DiscountType = g.Key.DiscountType,
            //         GrandTotal = g.Key.GrandTotal,
            //         SubTotal = g.Key.SubTotal,
            //         Due = g.Key.Due,
            //         Advance = g.Key.Advance,
            //         SaleQuantity = g.Key.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //         ReturnedQty = g.Key.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //         PaymentDate = g.Key.InvoicePayments.Where(ip => ip.Payment < 0).Select(r => r.Date).FirstOrDefault(),
            //         TotalPaid = (double?)g.Key.InvoicePayments.Where(ip=>ip.Payment<0 && ip.IsActive).Sum(ip => ip.Payment) ?? 0,
            //         //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //         //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //         //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
            //         Patient = g.Key.Patient,
            //         PatientName = g.Key.Patient.Name,
            //         IsProcedureInvoice = g.Key.IsProcedureInvoice,
            //         //InvoicePayments = i.InvoicePayments.ToList(),
            //     })).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            ////return null;
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions_Rpt(DateTime DateFrom, DateTime DateTo, long PatientId, int PageNo, int PageSize)
        {
            //return cxt.Invoices
            //.Where(i => i.IsActive)
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
            //    && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
            //)
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice,
            //    //InvoicePayments = i.InvoicePayments.ToList(),
            //}).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);  

            return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && i.IsActive && !i.IsQInvoice && i.Patient != null && i.Patient.PatientId == PatientId
                 && p.PaymentDate >= DateFrom
                 && p.PaymentDate <= DateTo
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     PaymentCreateDate = p.CreatedAt,
                     TotalPaid = p.Payment,
                     Services = i.Services,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, PageSize);

            // this query was being used to show data when multiple refund payments were being inserted and it was required to show single transaction for refund while multiple transactions for customer payments
            //return
            //    ((from i in cxt.Invoices
            //     join p in cxt.invoicePayments on i equals p.Invoice
            //     where p.IsActive
            //     && DbFunctions.TruncateTime(p.Date) >= DbFunctions.TruncateTime(DateFrom)
            //     && (DbFunctions.TruncateTime(p.Date) <= DbFunctions.TruncateTime(DateTo))
            //     && p.Payment > 0
            //     select new InvoiceTransactionsVM
            //     {
            //         InvoiceId = i.InvoiceId,
            //         CreateDate = i.CreateDate,
            //         TotalAmount = i.TotalAmount,
            //         TotalDiscount = i.TotalDiscount,
            //         DiscountType = i.DiscountType,
            //         GrandTotal = i.GrandTotal,
            //         SubTotal = i.SubTotal,
            //         Due = i.Due,
            //         Advance = i.Advance,
            //         SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //         ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //         PaymentDate = p.Date,
            //         TotalPaid = p.Payment,
            //         //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //         //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //         //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
            //         Patient = i.Patient,
            //         PatientName = i.Patient.Name,
            //         IsProcedureInvoice = i.IsProcedureInvoice,
            //         //InvoicePayments = i.InvoicePayments.ToList(),
            //     })
            //     .Union
            //    (from i in cxt.Invoices
            //     join p in cxt.invoicePayments on i equals p.Invoice
            //     where p.IsActive
            //     && DbFunctions.TruncateTime(p.Date) >= DbFunctions.TruncateTime(DateFrom)
            //     && (DbFunctions.TruncateTime(p.Date) <= DbFunctions.TruncateTime(DateTo))
            //     && p.Payment < 0
            //     group i by i into g
            //     select new InvoiceTransactionsVM
            //     {
            //         InvoiceId = g.Key.InvoiceId,
            //         CreateDate = g.Key.CreateDate,
            //         TotalAmount = g.Key.TotalAmount,
            //         TotalDiscount = g.Key.TotalDiscount,
            //         DiscountType = g.Key.DiscountType,
            //         GrandTotal = g.Key.GrandTotal,
            //         SubTotal = g.Key.SubTotal,
            //         Due = g.Key.Due,
            //         Advance = g.Key.Advance,
            //         SaleQuantity = g.Key.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //         ReturnedQty = g.Key.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //         PaymentDate = g.Key.InvoicePayments.Where(ip => ip.Payment < 0).Select(r => r.Date).FirstOrDefault(),
            //         TotalPaid = (double?)g.Key.InvoicePayments.Where(ip=>ip.Payment<0 && ip.IsActive).Sum(ip => ip.Payment) ?? 0,
            //         //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //         //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //         //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
            //         Patient = g.Key.Patient,
            //         PatientName = g.Key.Patient.Name,
            //         IsProcedureInvoice = g.Key.IsProcedureInvoice,
            //         //InvoicePayments = i.InvoicePayments.ToList(),
            //     })).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            ////return null;
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactionsStore_Rpt(DateTime DateFrom, DateTime DateTo, int StoreId, int PageNo, int PageSize)
        {
            return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && i.IsActive && i.Store != null && i.Store.IsActive && i.Store.StoreId == StoreId
                 && p.PaymentDate >= DateFrom
                 && p.PaymentDate <= DateTo
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     PaymentCreateDate = p.CreatedAt,
                     TotalPaid = p.Payment,
                     Services = i.Services,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, PageSize);
        }
        public List<InvoiceTransactionsVM> GetTransactions(long InvoiceId)
        {
            //return cxt.Invoices
            //.Where(i => i.IsActive)
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
            //    && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
            //)
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = (i.DiscountType == 2 ? (i.TotalDiscount / 100) * i.SubTotal : i.TotalDiscount),
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice,
            //}).OrderByDescending(i => i.InvoiceId).ToList();
            return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && !i.IsQInvoice
               && i.InvoiceId == InvoiceId
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Due = i.Due,
                     Services = i.Services,
                     InvoiceNoString = SqlFunctions.StringConvert(i.Services).Trim(),
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.InvoiceId).ToList();
        }

        public List<InvoiceTransactionsVM> GetTransactions(DateTime DateFrom, DateTime DateTo, double InvoiceRef = 0, long Patientid = 0)
        {
            //return cxt.Invoices
            //.Where(i => i.IsActive)
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
            //    && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
            //)
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = (i.DiscountType == 2 ? (i.TotalDiscount / 100) * i.SubTotal : i.TotalDiscount),
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice,
            //}).OrderByDescending(i => i.InvoiceId).ToList();
            if (InvoiceRef > 0)
            {
                if (Patientid > 0)
                {
                    return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && !i.IsQInvoice && InvoiceRef == i.InvoiceRefNo && Patientid == i.Patient.PatientId
                 && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                 && (DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo))
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Services = i.Services,
                     InvoiceNoString = SqlFunctions.StringConvert(i.Services).Trim(),
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.InvoiceId).ToList();

                }
                else
                {
                    return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && !i.IsQInvoice && InvoiceRef == i.InvoiceRefNo
                 && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                 && (DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo))
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Services = i.Services,
                     InvoiceNoString = SqlFunctions.StringConvert(i.Services).Trim(),
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.InvoiceId).ToList();

                }
            }
            else
            {
                if (Patientid > 0)
                {
                    return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && !i.IsQInvoice && Patientid == i.Patient.PatientId
                 && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                 && (DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo))
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Services = i.Services,
                     InvoiceNoString = SqlFunctions.StringConvert(i.Services).Trim(),
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.InvoiceId).ToList();

                }
                else
                {
                    return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && !i.IsQInvoice
                 && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                 && (DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo))
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Services = i.Services,
                     InvoiceNoString = SqlFunctions.StringConvert(i.Services).Trim(),
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.InvoiceId).ToList();

                }
            }
        }
        public List<InvoiceTransactionsVM> GetTransactionsRPT(long PatientId)
        {

            return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && i.IsActive && !i.IsQInvoice && i.Patient != null && i.Patient.PatientId == PatientId
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Services = i.Services,
                     InvoiceNoString = SqlFunctions.StringConvert(i.Services).Trim(),
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.InvoiceId).ToList();

        }
        public List<InvoiceTransactionsVM> GetTransactionsStoreRPT(int StoreId)
        {
            return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && i.IsActive && i.Store != null && i.Store.IsActive && i.Store.StoreId == StoreId
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Services = i.Services,
                     InvoiceNoString = SqlFunctions.StringConvert(i.Services).Trim(),
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.InvoiceId).ToList();

        }
        public List<InvoiceTransactionsVM> GetTransactionsStoreRPT(int StoreId, DateTime fromDate, DateTime toDate)
        {
            return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && i.IsActive && i.Store != null && i.Store.IsActive && i.Store.StoreId == StoreId && p.PaymentDate >= fromDate
                 && p.PaymentDate <= toDate
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Services = i.Services,
                     InvoiceNoString = SqlFunctions.StringConvert(i.Services).Trim(),
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.InvoiceId).ToList();

        }
        public List<InvoiceTransactionsVM> GetTransactions()
        {
            //return cxt.Invoices
            //.Where(i => i.IsActive)
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
            //    && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
            //)
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = (i.DiscountType == 2 ? (i.TotalDiscount / 100) * i.SubTotal : i.TotalDiscount),
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice,
            //}).OrderByDescending(i => i.InvoiceId).ToList();
            return
                (from i in cxt.Invoices
                 join p in cxt.invoicePayments on i equals p.Invoice
                 where p.IsActive && !i.IsQInvoice
                 select new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Services = i.Services,
                     InvoiceNoString = SqlFunctions.StringConvert(i.Services).Trim(),
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     PaymentDate = p.PaymentDate,
                     TotalPaid = p.Payment,
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                     //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                     Patient = i.Patient,
                     PatientName = i.Patient.Name,
                     IsProcedureInvoice = i.IsProcedureInvoice,
                     //InvoicePayments = i.InvoicePayments.ToList(),
                 }).OrderByDescending(i => i.InvoiceId).ToList();
        }



        public IPagedList<InvoiceTransactionsVM> GetTransactions_rpt(long InvoiceNo, int PageNo, int PageSize)
        {
            return
               (from i in cxt.Invoices
                join p in cxt.invoicePayments on i equals p.Invoice
                where p.IsActive && !i.IsQInvoice
                && i.IsActive && i.InvoiceRefNo == InvoiceNo
                select new InvoiceTransactionsVM
                {
                    InvoiceId = i.InvoiceId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreateDate = i.CreateDate,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    DiscountType = i.DiscountType,
                    GrandTotal = i.GrandTotal,
                    SubTotal = i.SubTotal,
                    Due = i.Due,
                    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                    PaymentDate = p.PaymentDate,
                    PaymentCreateDate = p.CreatedAt,
                    TotalPaid = p.Payment,
                    Services = i.Services,
                    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                    //TotalRefund = (double?)(cxt.invoicePayments.Where(ip => ip.Invoice.InvoiceId == i.InvoiceId && ip.IsActive && ip.Payment < 0).Sum(pp => pp.Payment)) ?? 0,
                    Patient = i.Patient,
                    PatientName = i.Patient.Name,
                    IsProcedureInvoice = i.IsProcedureInvoice,
                    //InvoicePayments = i.InvoicePayments.ToList(),
                }).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, PageSize);
            //.Select(i => new InvoiceTransactionsVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    TotalAmount = i.TotalAmount,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    Due = i.Due,
            //    Advance = i.Advance,
            //    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
            //    TotalRefund = (double?)(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
            //    Patient = i.Patient,
            //    PatientName = i.Patient.Name,
            //    IsProcedureInvoice = i.IsProcedureInvoice

            //}).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }


        public IPagedList<InvoiceTransactionsVM> GetTransactions_Financial(DateTime DateFrom, DateTime DateTo, int PaymentMode, int PageNo, int PageSize)
        {
            if (PaymentMode == 0)
            {
                return
                 (from i in cxt.Invoices
                  join ip in cxt.invoicePayments on i equals ip.Invoice
                  where i.IsActive && !i.IsQInvoice
                   && (DbFunctions.TruncateTime(ip.PaymentDate) >= DbFunctions.TruncateTime(DateFrom))
                   && (DbFunctions.TruncateTime(ip.PaymentDate) <= DbFunctions.TruncateTime(DateTo))
                  select new InvoiceTransactionsVM
                  {
                      InvoiceId = i.InvoiceId,
                      InvoiceRefNo = i.InvoiceRefNo,
                      CreateDate = i.CreateDate,
                      PaymentDate = ip.PaymentDate,
                      TotalAmount = i.TotalAmount,
                      TotalDiscount = i.TotalDiscount,
                      DiscountType = i.DiscountType,
                      GrandTotal = i.GrandTotal,
                      SubTotal = i.SubTotal,
                      Due = i.Due,
                      SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                      ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                      IsProcedureInvoice = i.IsProcedureInvoice,
                      Payment = ip.Payment,
                      PaymentType = ip.PaymentType.ToString(),
                      InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                      {
                          //IsActive = item.IsActive,
                          //InvoiceItemId = item.InvoiceItemId,
                          //ItemId = item.Item.ItemID,
                          ProcedureName = item.Procedure.ProcedureName,
                          ItemName = item.Item.ItemName,
                          //Rate = item.Rate,
                          //Quantity = item.Quantity,
                          //Discount = item.Discount,
                          //DiscountType = item.DiscountType,
                          //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                          //Amount = item.Amount,
                          //BatchId = item.Batch.BatchId,
                          //BatchName = item.Batch.BatchName,
                          //ReturnedQuantity = item.ReturnedQuantity
                      })).ToList(),
                      //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                      //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                      //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                      //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                      //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && (DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)) && (DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo))).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                      Patient = i.Patient,
                      PatientName = i.Patient.Name
                  })
                .OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, PageSize);
                //.OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            }
            else
            {

                return cxt.Invoices
                 .Where(i => i.IsActive)
                 .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                     && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                 )
                 .Select(i => new InvoiceTransactionsVM
                 {
                     InvoiceId = i.InvoiceId,
                     InvoiceRefNo = i.InvoiceRefNo,
                     CreateDate = i.CreateDate,
                     TotalAmount = i.TotalAmount,
                     TotalDiscount = i.TotalDiscount,
                     DiscountType = i.DiscountType,
                     GrandTotal = i.GrandTotal,
                     SubTotal = i.SubTotal,
                     Due = i.Due,
                     SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                     ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                     InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                     {
                         //IsActive = item.IsActive,
                         //InvoiceItemId = item.InvoiceItemId,
                         //ItemId = item.Item.ItemID,
                         ItemName = item.Item.ItemName,
                         //Rate = item.Rate,
                         //Quantity = item.Quantity,
                         //Discount = item.Discount,
                         //DiscountType = item.DiscountType,
                         //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                         //Amount = item.Amount,
                         //BatchId = item.Batch.BatchId,
                         //BatchName = item.Batch.BatchName,
                         //ReturnedQuantity = item.ReturnedQuantity
                     })).ToList(),
                     //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                     //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                     //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                     InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && p.PaymentType == PaymentMode).ToList(),
                     Patient = i.Patient,
                     PatientName = i.Patient.Name
                 }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            }
        }

        public List<InvoiceTransactionsVM> GetTransactions_FinancialPay(DateTime DateFrom, DateTime DateTo,int PaymentMode, double MRno = 0.0, long InvoiceRefno = 0)
        {

            /*DbFunctions.TruncateTime(i.UpdateDate) >= DbFunctions.TruncateTime(DateFrom)
                            && (DbFunctions.TruncateTime(i.UpdateDate) <= DbFunctions.TruncateTime(DateTo) &&*/
            //Implementation of Search By MR# Filter In Reports>Financial>Transaction
            if (InvoiceRefno > 0)
            {
                if (MRno != 0.0)
                {
                    var convtodec = Convert.ToDecimal(MRno);

                    PatientVM pa = cxt.Patients.Where(p => p.MRNo == convtodec)
                    .Select(p => new PatientVM
                    {
                        PatientId = p.PatientId
                    }).FirstOrDefault();
                    if (PaymentMode == 0)
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        .Where(i => pa.PatientId == i.Patient.PatientId && i.InvoiceRefNo == InvoiceRefno)
                        
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            IsProcedureInvoice = i.IsProcedureInvoice,
                            InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ProcedureName = item.Procedure.ProcedureName,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                    else
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        .Where(i => pa.PatientId == i.Patient.PatientId && i.InvoiceRefNo == InvoiceRefno)
                        
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && p.PaymentType == PaymentMode).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                }
                else
                {

                    if (PaymentMode == 0)
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        .Where(i => i.InvoiceRefNo == InvoiceRefno)
                        
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            IsProcedureInvoice = i.IsProcedureInvoice,
                            InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ProcedureName = item.Procedure.ProcedureName,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                    else
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        .Where(i => i.InvoiceRefNo == InvoiceRefno)
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && p.PaymentType == PaymentMode).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                }
            }
            else
            {
                if (MRno != 0.0)
                {
                    var convtodec = Convert.ToDecimal(MRno);

                    PatientVM pa = cxt.Patients.Where(p => p.MRNo == convtodec)
                    .Select(p => new PatientVM
                    {
                        PatientId = p.PatientId
                    }).FirstOrDefault();
                    if (PaymentMode == 0)
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        .Where(i =>  pa.PatientId == i.Patient.PatientId)
                        
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            IsProcedureInvoice = i.IsProcedureInvoice,
                            InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ProcedureName = item.Procedure.ProcedureName,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                    else
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        .Where(i => pa.PatientId == i.Patient.PatientId)
                        
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && p.PaymentType == PaymentMode).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                }
                else
                {

                    if (PaymentMode == 0)
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                  
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            IsProcedureInvoice = i.IsProcedureInvoice,
                            InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ProcedureName = item.Procedure.ProcedureName,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                    else
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && p.PaymentType == PaymentMode).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                }
            }

        }
        public IPagedList<InvoiceTransactionsVM> GetTransactionsByInvNo_Financial(long InvoiceNo, int PaymentMode, int PageNo, int PageSize)
        {
            if (PaymentMode == 0)
            {
                return
                   (from i in cxt.Invoices
                    join ip in cxt.invoicePayments on i equals ip.Invoice
                    where i.IsActive && i.InvoiceRefNo == InvoiceNo
                    select new InvoiceTransactionsVM
                    {
                        InvoiceId = i.InvoiceId,
                        InvoiceRefNo = i.InvoiceRefNo,
                        CreateDate = i.CreateDate,
                        PaymentDate = ip.PaymentDate,
                        TotalAmount = i.TotalAmount,
                        TotalDiscount = i.TotalDiscount,
                        DiscountType = i.DiscountType,
                        GrandTotal = i.GrandTotal,
                        SubTotal = i.SubTotal,
                        Due = i.Due,
                        SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                        ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                        IsProcedureInvoice = i.IsProcedureInvoice,
                        Payment = ip.Payment,
                        PaymentType = ip.PaymentType.ToString(),
                        InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                        {
                            //IsActive = item.IsActive,
                            //InvoiceItemId = item.InvoiceItemId,
                            //ItemId = item.Item.ItemID,
                            ProcedureName = item.Procedure.ProcedureName,
                            ItemName = item.Item.ItemName,
                            //Rate = item.Rate,
                            //Quantity = item.Quantity,
                            //Discount = item.Discount,
                            //DiscountType = item.DiscountType,
                            //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                            //Amount = item.Amount,
                            //BatchId = item.Batch.BatchId,
                            //BatchName = item.Batch.BatchName,
                            //ReturnedQuantity = item.ReturnedQuantity
                        })).ToList(),
                        //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                        //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                        //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                        //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                        //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && (DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)) && (DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo))).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                        Patient = i.Patient,
                        PatientName = i.Patient.Name
                    })
                  .OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, PageSize);
                //.OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            }
            else
            {
                return cxt.Invoices
                .Where(i => i.IsActive && i.InvoiceRefNo == InvoiceNo)
                .Select(i => new InvoiceTransactionsVM
                {
                    InvoiceId = i.InvoiceId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreateDate = i.CreateDate,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    DiscountType = i.DiscountType,
                    GrandTotal = i.GrandTotal,
                    SubTotal = i.SubTotal,
                    Due = i.Due,
                    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                    InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                    {
                        //IsActive = item.IsActive,
                        //InvoiceItemId = item.InvoiceItemId,
                        //ItemId = item.Item.ItemID,
                        ItemName = item.Item.ItemName,
                        //Rate = item.Rate,
                        //Quantity = item.Quantity,
                        //Discount = item.Discount,
                        //DiscountType = item.DiscountType,
                        //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                        //Amount = item.Amount,
                        //BatchId = item.Batch.BatchId,
                        //BatchName = item.Batch.BatchName,
                        //ReturnedQuantity = item.ReturnedQuantity
                    })).ToList(),
                    //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                    //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                    InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && p.PaymentType == PaymentMode).ToList(),
                    Patient = i.Patient,
                    PatientName = i.Patient.Name
                }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            }
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactionsByMrNo_Financial(long MrNo, int PaymentMode, int PageNo, int PageSize)
        {
            return (from i in cxt.Invoices
                    join ip in cxt.invoicePayments on i equals ip.Invoice
                    where i.IsActive && i.Patient.MRNo == MrNo
                    select new InvoiceTransactionsVM
                    {
                        InvoiceId = i.InvoiceId,
                        InvoiceRefNo = i.InvoiceRefNo,
                        CreateDate = i.CreateDate,
                        PaymentDate = ip.PaymentDate,
                        TotalAmount = i.TotalAmount,
                        TotalDiscount = i.TotalDiscount,
                        DiscountType = i.DiscountType,
                        GrandTotal = i.GrandTotal,
                        SubTotal = i.SubTotal,
                        Due = i.Due,
                        SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                        ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                        IsProcedureInvoice = i.IsProcedureInvoice,
                        Payment = ip.Payment,
                        PaymentType = ip.PaymentType.ToString(),
                        InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                        {
                            //IsActive = item.IsActive,
                            //InvoiceItemId = item.InvoiceItemId,
                            //ItemId = item.Item.ItemID,
                            ProcedureName = item.Procedure.ProcedureName,
                            ItemName = item.Item.ItemName,
                            //Rate = item.Rate,
                            //Quantity = item.Quantity,
                            //Discount = item.Discount,
                            //DiscountType = item.DiscountType,
                            //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                            //Amount = item.Amount,
                            //BatchId = item.Batch.BatchId,
                            //BatchName = item.Batch.BatchName,
                            //ReturnedQuantity = item.ReturnedQuantity
                        })).ToList(),
                        //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                        //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                        //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                        //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                        //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && (DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)) && (DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo))).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                        Patient = i.Patient,
                        PatientName = i.Patient.Name
                    })
             .OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, PageSize);
            //.OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public List<SummationsVM> GetTransactions_Financial_Graph(DateTime DateFrom, DateTime DateTo, int PaymentMode)
        {
            List<SummationsVM> Result = new List<SummationsVM>();
            if (PaymentMode == 0)
            {
                Result = (from i in cxt.Invoices
                          join ip in cxt.invoicePayments on i equals ip.Invoice
                          where i.IsActive
                          && DbFunctions.TruncateTime(ip.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                          && DbFunctions.TruncateTime(ip.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                          group i by new { Date = DbFunctions.TruncateTime(i.CreateDate) } into g
                          select new SummationsVM
                          {
                              Date = g.Key.Date,
                              MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                          }
                              ).ToList();
            }
            else
            {
                Result = (from i in cxt.Invoices
                          join ip in cxt.invoicePayments on i equals ip.Invoice
                          where i.IsActive
                          && DbFunctions.TruncateTime(ip.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                          && DbFunctions.TruncateTime(ip.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                          group i by new { Date = DbFunctions.TruncateTime(i.CreateDate) } into g
                          select new SummationsVM
                          {
                              Date = g.Key.Date,
                              MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                          }
                              ).ToList();
            }
            return Result;
        }
        public List<SummationsVM> GetTransactionsByInvNo_Financial_Graph(long InvoiceNo, int PaymentMode)
        {
            List<SummationsVM> Result = new List<SummationsVM>();
            if (PaymentMode == 0)
            {
                Result = (from i in cxt.Invoices
                          where i.IsActive
                          && i.InvoiceId == InvoiceNo
                          group i by new { Date = DbFunctions.TruncateTime(i.CreateDate) } into g
                          select new SummationsVM
                          {
                              Date = g.Key.Date,
                              MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                          }
                              ).ToList();
            }
            else
            {
                Result = (from i in cxt.Invoices
                          where i.IsActive
                          && i.InvoiceId == InvoiceNo
                          group i by new { Date = DbFunctions.TruncateTime(i.CreateDate) } into g
                          select new SummationsVM
                          {
                              Date = g.Key.Date,
                              MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                          }
                              ).ToList();
            }
            return Result;
        }
        public List<SummationsVM> GetTransactionsByMrNo_Financial_Graph(long MrNo, int PaymentMode)
        {
            List<SummationsVM> Result = new List<SummationsVM>();
            if (PaymentMode == 0)
            {
                Result = (from i in cxt.Invoices
                          where i.IsActive
                          && i.Patient.MRNo == MrNo
                          group i by new { Date = DbFunctions.TruncateTime(i.CreateDate) } into g
                          select new SummationsVM
                          {
                              Date = g.Key.Date,
                              MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                          }
                              ).ToList();
            }
            else
            {
                Result = (from i in cxt.Invoices
                          where i.IsActive
                          && i.Patient.MRNo == MrNo
                          group i by new { Date = DbFunctions.TruncateTime(i.CreateDate) } into g
                          select new SummationsVM
                          {
                              Date = g.Key.Date,
                              MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                          }
                              ).ToList();
            }
            return Result;
        }
        public List<InvoiceTransactionsVM> GetTransactions_Financial_Reports(DateTime DateFrom, DateTime DateTo, int PaymentMode, double MRno = 0.0, long InvoiceRefno = 0)
        {
            if (InvoiceRefno > 0)
            {
                if (MRno > 0)
                {
                    var convtodec = Convert.ToDecimal(MRno);

                    PatientVM pa = cxt.Patients.Where(p => p.MRNo == convtodec)
                    .Select(p => new PatientVM
                    {
                        PatientId = p.PatientId
                    }).FirstOrDefault();

                    if (PaymentMode == 0)
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        .Where(i => i.InvoiceRefNo == InvoiceRefno && i.Patient.PatientId == pa.PatientId)
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            IsProcedureInvoice = i.IsProcedureInvoice,
                            InvoiceItems = (i.InvoiceItems.Where(ii => !ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ProcedureName = item.Procedure.ProcedureName,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                    else
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        .Where(i => i.InvoiceRefNo == InvoiceRefno && i.Patient.PatientId == pa.PatientId)
                        
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && !p.IsQInvoice && p.PaymentType == PaymentMode).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                }
                else
                {
                    if (PaymentMode == 0)
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        .Where(i =>  i.InvoiceRefNo == InvoiceRefno)
                        
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            IsProcedureInvoice = i.IsProcedureInvoice,
                            InvoiceItems = (i.InvoiceItems.Where(ii => !ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ProcedureName = item.Procedure.ProcedureName,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                    else
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        .Where(i =>  i.InvoiceRefNo == InvoiceRefno)
                        
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && !p.IsQInvoice && p.PaymentType == PaymentMode).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                }
            }
            else
            {
                if (MRno > 0)
                {
                    var convtodec = Convert.ToDecimal(MRno);

                    PatientVM pa = cxt.Patients.Where(p => p.MRNo == convtodec)
                    .Select(p => new PatientVM
                    {
                        PatientId = p.PatientId
                    }).FirstOrDefault();

                    if (PaymentMode == 0)
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        .Where(i =>  i.Patient.PatientId == pa.PatientId)
                        
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            IsProcedureInvoice = i.IsProcedureInvoice,
                            InvoiceItems = (i.InvoiceItems.Where(ii => !ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ProcedureName = item.Procedure.ProcedureName,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                    else
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                        .Where(i =>  i.Patient.PatientId == pa.PatientId)
                        
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && !p.IsQInvoice && p.PaymentType == PaymentMode).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                }
                else
                {
                    if (PaymentMode == 0)
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
                       /* .Where(i => DbFunctions.TruncateTime(i.UpdateDate) >= DbFunctions.TruncateTime(DateFrom)
                            && (DbFunctions.TruncateTime(i.UpdateDate) <= DbFunctions.TruncateTime(DateTo))*/
                        
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            IsProcedureInvoice = i.IsProcedureInvoice,
                            InvoiceItems = (i.InvoiceItems.Where(ii => !ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ProcedureName = item.Procedure.ProcedureName,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                    else
                    {
                        return cxt.Invoices
                        .Where(i => i.IsActive && !i.IsQInvoice)
 /*                       .Where(i => DbFunctions.TruncateTime(i.UpdateDate) >= DbFunctions.TruncateTime(DateFrom)
                            && (DbFunctions.TruncateTime(i.UpdateDate) <= DbFunctions.TruncateTime(DateTo))
                        )*/
                        .Select(i => new InvoiceTransactionsVM
                        {
                            InvoiceId = i.InvoiceId,
                            InvoiceRefNo = i.InvoiceRefNo,
                            CreateDate = i.CreateDate,
                            TotalAmount = i.TotalAmount,
                            TotalDiscount = i.TotalDiscount,
                            DiscountType = i.DiscountType,
                            GrandTotal = i.GrandTotal,
                            SubTotal = i.SubTotal,
                            Due = i.Due,
                            SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                            ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                            InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                            {
                                //IsActive = item.IsActive,
                                //InvoiceItemId = item.InvoiceItemId,
                                //ItemId = item.Item.ItemID,
                                ItemName = item.Item.ItemName,
                                //Rate = item.Rate,
                                //Quantity = item.Quantity,
                                //Discount = item.Discount,
                                //DiscountType = item.DiscountType,
                                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                                //Amount = item.Amount,
                                //BatchId = item.Batch.BatchId,
                                //BatchName = item.Batch.BatchName,
                                //ReturnedQuantity = item.ReturnedQuantity
                            })).ToList(),
                            //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                            //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                            //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                            InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && !p.IsQInvoice && p.PaymentType == PaymentMode).ToList(),
                            Patient = i.Patient,
                            PatientName = i.Patient.Name
                        }).OrderByDescending(i => i.InvoiceId).ToList();
                    }
                }
            }
            //var convtodec = Convert.ToDecimal(MRno);

            //PatientVM pa = cxt.Patients.Where(p => p.MRNo == convtodec)
            //.Select(p => new PatientVM
            //{
            //    PatientId = p.PatientId
            //}).FirstOrDefault();

            //if (PaymentMode == 0)
            //{
            //    return cxt.Invoices
            //    .Where(i => i.IsActive && !i.IsQInvoice)
            //    .Where(i => DbFunctions.TruncateTime(i.UpdateDate) >= DbFunctions.TruncateTime(DateFrom)
            //        && (DbFunctions.TruncateTime(i.UpdateDate) <= DbFunctions.TruncateTime(DateTo))
            //    )
            //    .Select(i => new InvoiceTransactionsVM
            //    {
            //        InvoiceId = i.InvoiceId,
            //        InvoiceRefNo = i.InvoiceRefNo,
            //        CreateDate = i.CreateDate,
            //        TotalAmount = i.TotalAmount,
            //        TotalDiscount = i.TotalDiscount,
            //        DiscountType = i.DiscountType,
            //        GrandTotal = i.GrandTotal,
            //        SubTotal = i.SubTotal,
            //        Due = i.Due,
            //        SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //        ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //        IsProcedureInvoice = i.IsProcedureInvoice,
            //        InvoiceItems = (i.InvoiceItems.Where(ii => !ii.IsActive).Select(item => new InvoiceItemVM
            //        {
            //            //IsActive = item.IsActive,
            //            //InvoiceItemId = item.InvoiceItemId,
            //            //ItemId = item.Item.ItemID,
            //            ProcedureName = item.Procedure.ProcedureName,
            //            ItemName = item.Item.ItemName,
            //            //Rate = item.Rate,
            //            //Quantity = item.Quantity,
            //            //Discount = item.Discount,
            //            //DiscountType = item.DiscountType,
            //            //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
            //            //Amount = item.Amount,
            //            //BatchId = item.Batch.BatchId,
            //            //BatchName = item.Batch.BatchName,
            //            //ReturnedQuantity = item.ReturnedQuantity
            //        })).ToList(),
            //        //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //        //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //        //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
            //        //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
            //        InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).ToList(),
            //        Patient = i.Patient,
            //        PatientName = i.Patient.Name
            //    }).OrderByDescending(i => i.InvoiceId).ToList();
            //}
            //else
            //{
            //    return cxt.Invoices
            //    .Where(i => i.IsActive && !i.IsQInvoice)
            //    .Where(i => DbFunctions.TruncateTime(i.UpdateDate) >= DbFunctions.TruncateTime(DateFrom)
            //        && (DbFunctions.TruncateTime(i.UpdateDate) <= DbFunctions.TruncateTime(DateTo))
            //    )
            //    .Select(i => new InvoiceTransactionsVM
            //    {
            //        InvoiceId = i.InvoiceId,
            //        InvoiceRefNo = i.InvoiceRefNo,
            //        CreateDate = i.CreateDate,
            //        TotalAmount = i.TotalAmount,
            //        TotalDiscount = i.TotalDiscount,
            //        DiscountType = i.DiscountType,
            //        GrandTotal = i.GrandTotal,
            //        SubTotal = i.SubTotal,
            //        Due = i.Due,
            //        SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
            //        ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
            //        InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
            //        {
            //            //IsActive = item.IsActive,
            //            //InvoiceItemId = item.InvoiceItemId,
            //            //ItemId = item.Item.ItemID,
            //            ItemName = item.Item.ItemName,
            //            //Rate = item.Rate,
            //            //Quantity = item.Quantity,
            //            //Discount = item.Discount,
            //            //DiscountType = item.DiscountType,
            //            //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
            //            //Amount = item.Amount,
            //            //BatchId = item.Batch.BatchId,
            //            //BatchName = item.Batch.BatchName,
            //            //ReturnedQuantity = item.ReturnedQuantity
            //        })).ToList(),
            //        //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //        //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //        //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
            //        //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
            //        InvoicePayments = i.InvoicePayments.Where(p => p.IsActive && !p.IsQInvoice && p.PaymentType == PaymentMode).ToList(),
            //        Patient = i.Patient,
            //        PatientName = i.Patient.Name
            //    }).OrderByDescending(i => i.InvoiceId).ToList();
            //}
        }
        public SummationsVM GetSums_Financial(DateTime DateFrom, DateTime DateTo, double MRno = 0.0, long InvoiceRef = 0)
        {
            SummationsVM Result = new SummationsVM();
            //Implementation of Search By MR# Filter In Reports>Financial>Transaction
            if (InvoiceRef > 0)
            {
                if (MRno != 0.0)
                {
                    var convtodec = Convert.ToDecimal(MRno);

                    PatientVM pa = cxt.Patients.Where(p => p.MRNo == convtodec)
                    .Select(p => new PatientVM
                    {
                        PatientId = p.PatientId
                    }).FirstOrDefault();

                    Result = (from i in cxt.Invoices
                              join p in cxt.invoicePayments on i equals p.Invoice
                              where i.IsActive && !i.IsQInvoice
                              && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                              && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                              && pa.PatientId == i.Patient.PatientId && i.InvoiceRefNo == InvoiceRef
                              group i by new { Year = i.UpdateDate.Year, Month = i.UpdateDate.Month } into g
                              select new SummationsVM
                              {
                                  Year = g.Key.Year,
                                  Month = g.Key.Month,
                                  MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                              }
                              ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();
                    var Res = cxt.Invoices
                           .Where(i => i.IsActive && !i.IsQInvoice
                            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                            && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && pa.PatientId == i.Patient.PatientId && i.InvoiceRefNo == InvoiceRef
                              ).ToList();
                    if (Res.Any())
                    {
                        Result.TotalRevenue = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && pa.PatientId == i.Patient.PatientId && i.InvoiceRefNo == InvoiceRef)
                                .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive
                                    && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)).Sum(p => p.Payment));
                        Result.GrossSale = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.InvoiceRefNo == InvoiceRef && pa.PatientId == i.Patient.PatientId)
                                .Sum(r => (double?)r.SubTotal);
                    }
                }
                else
                {
                    Result = (from i in cxt.Invoices
                              join p in cxt.invoicePayments on i equals p.Invoice
                              where i.IsActive && !i.IsQInvoice
                              && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                              && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo) && i.InvoiceRefNo == InvoiceRef
                              group i by new { Year = i.UpdateDate.Year, Month = i.UpdateDate.Month } into g
                              select new SummationsVM
                              {
                                  Year = g.Key.Year,
                                  Month = g.Key.Month,
                                  MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                              }
                              ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();
                    var Res = cxt.Invoices
                           .Where(i => i.IsActive && !i.IsQInvoice
                            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                            && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.InvoiceRefNo == InvoiceRef).ToList();
                    if (Res.Any())
                    {
                        Result.TotalRevenue = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && i.InvoiceRefNo == InvoiceRef)
                                .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive
                                    && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                                    ).Sum(p => p.Payment));
                        Result.GrossSale = (double?)cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.InvoiceRefNo == InvoiceRef)
                                .Sum(r => r.SubTotal);
                    }
                }
            }
            else
            {
                if (MRno != 0.0)
                {
                    var convtodec = Convert.ToDecimal(MRno);

                    PatientVM pa = cxt.Patients.Where(p => p.MRNo == convtodec)
                    .Select(p => new PatientVM
                    {
                        PatientId = p.PatientId
                    }).FirstOrDefault();

                    Result = (from i in cxt.Invoices
                              join p in cxt.invoicePayments on i equals p.Invoice
                              where i.IsActive && !i.IsQInvoice
                              && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                              && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                              && pa.PatientId == i.Patient.PatientId
                              group i by new { Year = i.UpdateDate.Year, Month = i.UpdateDate.Month } into g
                              select new SummationsVM
                              {
                                  Year = g.Key.Year,
                                  Month = g.Key.Month,
                                  MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                              }
                              ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();
                    var Res = cxt.Invoices
                           .Where(i => i.IsActive && !i.IsQInvoice
                            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                            && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && pa.PatientId == i.Patient.PatientId
                              ).ToList();
                    if (Res.Any())
                    {
                        Result.TotalRevenue = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && pa.PatientId == i.Patient.PatientId)
                                .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive
                                    && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)).Sum(p => p.Payment));
                        Result.GrossSale = (double?)cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && pa.PatientId == i.Patient.PatientId)
                                .Sum(r => r.SubTotal);
                    }
                }
                else
                {
                    Result = (from i in cxt.Invoices
                              join p in cxt.invoicePayments on i equals p.Invoice
                              where i.IsActive && !i.IsQInvoice
                              && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                              && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                              group i by new { Year = i.UpdateDate.Year, Month = i.UpdateDate.Month } into g
                              select new SummationsVM
                              {
                                  Year = g.Key.Year,
                                  Month = g.Key.Month,
                                  MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))

                              }
                              ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();
                    var Res = cxt.Invoices
                           .Where(i => i.IsActive && !i.IsQInvoice
                            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                            && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)).ToList();
                    if (Res.Any())
                    {
                        Result.TotalRevenue = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice)
                                .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive
                                    && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                                    ).Sum(p => p.Payment));
                        Result.GrossSale = (double?)cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                                .Sum(r => r.SubTotal);
                    }
                }
            }



            return Result;
        }
        public SummationsVM GetSums_Financial(DateTime DateFrom, DateTime DateTo, int PaymentMode, double MRNo = 0.0, long InvoiceRef = 0)
        {
            SummationsVM Result = new SummationsVM();
            //Implementation of Search By MR# Filter In Reports>Financial>Transaction
            if (InvoiceRef > 0)
            {
                if (MRNo != 0.0)
                {
                    var convtodec = Convert.ToDecimal(MRNo);

                    PatientVM pa = cxt.Patients.Where(p => p.MRNo == convtodec)
                    .Select(p => new PatientVM
                    {
                        PatientId = p.PatientId
                    }).FirstOrDefault();

                    Result = (from i in cxt.Invoices
                              join p in cxt.invoicePayments on i equals p.Invoice
                              where i.IsActive && !i.IsQInvoice
                              && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                              && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                              && i.Patient.PatientId == pa.PatientId && i.InvoiceRefNo == InvoiceRef
                              group i by new { Year = i.UpdateDate.Year, Month = i.UpdateDate.Month } into g
                              select new SummationsVM
                              {
                                  Year = g.Key.Year,
                                  Month = g.Key.Month,
                                  MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                              }
                              ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();
                    var Res = cxt.Invoices
                           .Where(i => i.IsActive && !i.IsQInvoice
                            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                            && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.Patient.PatientId == pa.PatientId && i.InvoiceRefNo == InvoiceRef).ToList();
                    if (Res.Any())
                    {
                        Result.TotalRevenue = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && i.Patient.PatientId == pa.PatientId && i.InvoiceRefNo == InvoiceRef)
                                .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive
                                 && p.PaymentType == PaymentMode
                                 && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                              ).Sum(p => (double?)p.Payment) ?? 0);
                        Result.GrossSale = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.InvoicePayments.Where(p => p.PaymentType == PaymentMode && p.IsActive && !p.IsQInvoice).ToList().Count() > 0 && i.InvoiceRefNo == InvoiceRef)
                                .Sum(r => (double?)r.SubTotal);
                    }

                }
                else
                {
                    Result = (from i in cxt.Invoices
                              join p in cxt.invoicePayments on i equals p.Invoice
                              where i.IsActive && !i.IsQInvoice
                              && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                              && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo) && i.InvoiceRefNo == InvoiceRef
                              group i by new { Year = i.UpdateDate.Year, Month = i.UpdateDate.Month } into g
                              select new SummationsVM
                              {
                                  Year = g.Key.Year,
                                  Month = g.Key.Month,
                                  MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                              }
                              ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();
                    var Res = cxt.Invoices
                           .Where(i => i.IsActive && !i.IsQInvoice
                            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                            && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.InvoiceRefNo == InvoiceRef).ToList();
                    if (Res.Any())
                    {
                        Result.TotalRevenue = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && i.InvoiceRefNo == InvoiceRef
                                )
                                .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive
                                 && p.PaymentType == PaymentMode
                                 && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                                 ).Sum(p => (double?)p.Payment) ?? 0);
                        Result.GrossSale = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.InvoicePayments.Where(p => p.PaymentType == PaymentMode && p.IsActive && !p.IsQInvoice).ToList().Count() > 0 && i.InvoiceRefNo == InvoiceRef)
                                .Sum(r => (double?)r.SubTotal);
                    }

                }
            }
            else
            {
                if (MRNo != 0.0)
                {
                    var convtodec = Convert.ToDecimal(MRNo);

                    PatientVM pa = cxt.Patients.Where(p => p.MRNo == convtodec)
                    .Select(p => new PatientVM
                    {
                        PatientId = p.PatientId
                    }).FirstOrDefault();

                    Result = (from i in cxt.Invoices
                              join p in cxt.invoicePayments on i equals p.Invoice
                              where i.IsActive && !i.IsQInvoice
                              && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                              && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                              && i.Patient.PatientId == pa.PatientId
                              group i by new { Year = i.UpdateDate.Year, Month = i.UpdateDate.Month } into g
                              select new SummationsVM
                              {
                                  Year = g.Key.Year,
                                  Month = g.Key.Month,
                                  MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                              }
                              ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();
                    var Res = cxt.Invoices
                           .Where(i => i.IsActive && !i.IsQInvoice
                            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                            && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.Patient.PatientId == pa.PatientId).ToList();
                    if (Res.Any())
                    {
                        Result.TotalRevenue = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && i.Patient.PatientId == pa.PatientId)
                                .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive
                                 && p.PaymentType == PaymentMode
                                 && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                              ).Sum(p => (double?)p.Payment) ?? 0);
                        Result.GrossSale = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.InvoicePayments.Where(p => p.PaymentType == PaymentMode && p.IsActive && !p.IsQInvoice).ToList().Count() > 0)
                                .Sum(r => (double?)r.SubTotal);

                    }

                }
                else
                {
                    Result = (from i in cxt.Invoices
                              join p in cxt.invoicePayments on i equals p.Invoice
                              where i.IsActive && !i.IsQInvoice
                              && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                              && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                              group i by new { Year = i.UpdateDate.Year, Month = i.UpdateDate.Month } into g
                              select new SummationsVM
                              {
                                  Year = g.Key.Year,
                                  Month = g.Key.Month,
                                  MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                              }
                              ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();
                    var Res = cxt.Invoices
                           .Where(i => i.IsActive && !i.IsQInvoice
                            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                            && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)).ToList();
                    if (Res.Any())
                    {
                        Result.TotalRevenue = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice
                                )
                                .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive
                                 && p.PaymentType == PaymentMode
                                 && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                                 ).Sum(p => (double?)p.Payment) ?? 0);
                        Result.GrossSale = cxt.Invoices
                                .Where(i => i.IsActive && !i.IsQInvoice && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                 && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.InvoicePayments.Where(p => p.PaymentType == PaymentMode && p.IsActive && !p.IsQInvoice).ToList().Count() > 0)
                                .Sum(r => (double?)r.SubTotal);
                    }

                }
            }

            return Result;
        }

        public SummationsVM GetSums_FinancialTransaction(DateTime DateFrom, DateTime DateTo)
        {
            SummationsVM Result = new SummationsVM();
            Result = (from i in cxt.Invoices
                      join ip in cxt.invoicePayments on i equals ip.Invoice
                      where i.IsActive
                      && DbFunctions.TruncateTime(ip.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                      && DbFunctions.TruncateTime(ip.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                      group i by new { Year = i.CreateDate.Year, Month = i.CreateDate.Month } into g
                      select new SummationsVM
                      {
                          Year = g.Key.Year,
                          Month = g.Key.Month,
                          MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                      }
                      ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();

            var Res = cxt.Invoices
                       .Where(i => i.IsActive
                        && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                        && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)).ToList();
            if (Res.Any())
            {
                Result.TotalRevenue = cxt.Invoices
                        .Where(i => i.IsActive
                        && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                         && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                        .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive
                         ).Sum(p => (double?)p.Payment) ?? 0);
            }
            return Result;
        }
        public SummationsVM GetSums_FinancialTransaction(DateTime DateFrom, DateTime DateTo, int PaymentMode)
        {
            SummationsVM Result = new SummationsVM();
            Result = (from i in cxt.Invoices
                      join ip in cxt.invoicePayments on i equals ip.Invoice
                      where i.IsActive
                      && DbFunctions.TruncateTime(ip.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                      && DbFunctions.TruncateTime(ip.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                      group i by new { Year = i.CreateDate.Year, Month = i.CreateDate.Month } into g
                      select new SummationsVM
                      {
                          Year = g.Key.Year,
                          Month = g.Key.Month,
                          MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                      }
                      ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();

            var Res = cxt.Invoices
                       .Where(i => i.IsActive
                        && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                        && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)).ToList();
            if (Res.Any())
            {
                Result.TotalRevenue = cxt.Invoices
                        .Where(i => i.IsActive
                        && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                         && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                        .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive && p.PaymentType == PaymentMode
                         ).Sum(p => (double?)p.Payment) ?? 0);
            }
            return Result;
        }
        public SummationsVM GetSums_Financial(long InvoiceNo)
        {
            SummationsVM Result = new SummationsVM();
            Result = (from i in cxt.Invoices
                      where i.IsActive && i.InvoiceRefNo == InvoiceNo
                      group i by new { Year = i.CreateDate.Year, Month = i.CreateDate.Month } into g
                      select new SummationsVM
                      {
                          Year = g.Key.Year,
                          Month = g.Key.Month,
                          MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                      }
                          ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();

            var Res = cxt.Invoices
                       .Where(i => i.IsActive && i.InvoiceRefNo == InvoiceNo).ToList();
            if (Res.Any())
            {
                Result.TotalRevenue = cxt.Invoices
                        .Where(i => i.IsActive && i.InvoiceRefNo == InvoiceNo)
                        .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive).Sum(p => p.Payment));
            }
            return Result;
        }
        public SummationsVM GetSums_FinancialMrNum(long MrNum)
        {
            SummationsVM Result = new SummationsVM();
            Result = (from i in cxt.Invoices
                      where i.IsActive && i.Patient.MRNo == MrNum
                      group i by new { Year = i.CreateDate.Year, Month = i.CreateDate.Month } into g
                      select new SummationsVM
                      {
                          Year = g.Key.Year,
                          Month = g.Key.Month,
                          MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                      }
                          ).OrderByDescending(r => r.MaxMonthRevenue).FirstOrDefault();

            var Res = cxt.Invoices
                       .Where(i => i.IsActive && i.Patient.MRNo == MrNum).ToList();
            if (Res.Any())
            {
                Result.TotalRevenue = cxt.Invoices
                        .Where(i => i.IsActive && i.Patient.MRNo == MrNum)
                        .Sum(r => r.InvoicePayments.DefaultIfEmpty().Where(p => p.IsActive).Sum(p => p.Payment));
            }
            return Result;
        }

        public List<SummationsVM> GetPayment_Financial_Graph(DateTime DateFrom, DateTime DateTo, int PaymentMode, double MRno = 0.0, long InvoiceRefNo = 0)
        {
            List<SummationsVM> Result = new List<SummationsVM>();
            //Implementation of Search By MR# Filter In Reports>Financial>Transaction
            if (InvoiceRefNo > 0)
            {
                if (MRno != 0.0)
                {
                    var convtodec = Convert.ToDecimal(MRno);

                    PatientVM pa = cxt.Patients.Where(p => p.MRNo == convtodec)
                    .Select(p => new PatientVM
                    {
                        PatientId = p.PatientId
                    }).FirstOrDefault();

                    if (PaymentMode == 0)
                    {
                        Result = (from i in cxt.Invoices
                                  join p in cxt.invoicePayments on i equals p.Invoice
                                  where i.IsActive && !i.IsQInvoice
                                  && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                  && i.Patient.PatientId == pa.PatientId && i.InvoiceRefNo == InvoiceRefNo
                                  group p by new { PaymentType = p.PaymentType } into g
                                  select new SummationsVM
                                  {
                                      PaymentType = g.Key.PaymentType,
                                      TotalRevenue = g.Sum(p => p.Payment)
                                  }
                                      ).ToList();
                    }
                    else
                    {
                        Result = (from i in cxt.Invoices
                                  where i.IsActive && !i.IsQInvoice
                                  && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.Patient.PatientId == pa.PatientId && i.InvoiceRefNo == InvoiceRefNo
                                  group i by new { Date = DbFunctions.TruncateTime(i.CreateDate) } into g
                                  select new SummationsVM
                                  {
                                      Date = g.Key.Date,
                                      MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                                  }
                                      ).ToList();
                    }
                }
                else
                {
                    if (PaymentMode == 0)
                    {
                        Result = (from i in cxt.Invoices
                                  join p in cxt.invoicePayments on i equals p.Invoice
                                  where i.IsActive && !i.IsQInvoice
                                  && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.InvoiceRefNo == InvoiceRefNo
                                  group p by new { PaymentType = p.PaymentType } into g
                                  select new SummationsVM
                                  {
                                      PaymentType = g.Key.PaymentType,
                                      TotalRevenue = g.Sum(p => p.Payment)
                                  }
                                      ).ToList();
                    }
                    else
                    {
                        Result = (from i in cxt.Invoices
                                  where i.IsActive && !i.IsQInvoice
                                  && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo) && i.InvoiceRefNo == InvoiceRefNo
                                  group i by new { Date = DbFunctions.TruncateTime(i.CreateDate) } into g
                                  select new SummationsVM
                                  {
                                      Date = g.Key.Date,
                                      MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                                  }
                                      ).ToList();
                    }
                }
            }
            else
            {
                if (MRno != 0.0)
                {
                    var convtodec = Convert.ToDecimal(MRno);

                    PatientVM pa = cxt.Patients.Where(p => p.MRNo == convtodec)
                    .Select(p => new PatientVM
                    {
                        PatientId = p.PatientId
                    }).FirstOrDefault();

                    if (PaymentMode == 0)
                    {
                        Result = (from i in cxt.Invoices
                                  join p in cxt.invoicePayments on i equals p.Invoice
                                  where i.IsActive && !i.IsQInvoice
                                  && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                  && i.Patient.PatientId == pa.PatientId
                                  group p by new { PaymentType = p.PaymentType } into g
                                  select new SummationsVM
                                  {
                                      PaymentType = g.Key.PaymentType,
                                      TotalRevenue = g.Sum(p => p.Payment),
                                      
                                  }
                                      ).ToList();
                    }
                    else
                    {
                        Result = (from i in cxt.Invoices
                                  where i.IsActive && !i.IsQInvoice
                                  && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                  group i by new { Date = DbFunctions.TruncateTime(i.CreateDate) } into g
                                  select new SummationsVM
                                  {
                                      Date = g.Key.Date,
                                      MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                                  }
                                      ).ToList();
                    }
                }
                else
                {
                    if (PaymentMode == 0)
                    {
                        Result = (from i in cxt.Invoices
                                  join p in cxt.invoicePayments on i equals p.Invoice
                                  where i.IsActive && !i.IsQInvoice
                                  && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                  group p by new { PaymentType = p.PaymentType } into g
                                  select new SummationsVM
                                  {
                                      PaymentType = g.Key.PaymentType,
                                      TotalRevenue = g.Sum(p => p.Payment)
                                  }
                                      ).ToList();
                    }
                    else
                    {
                        Result = (from i in cxt.Invoices
                                  where i.IsActive && !i.IsQInvoice
                                  && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                  group i by new { Date = DbFunctions.TruncateTime(i.CreateDate) } into g
                                  select new SummationsVM
                                  {
                                      Date = g.Key.Date,
                                      MaxMonthRevenue = g.Sum(p => p.InvoicePayments.Where(pmt => pmt.IsActive).Sum(pmt => pmt.Payment))
                                  }
                                      ).ToList();
                    }
                }
            }

            

            return Result;

        }
        public SummationsVM GetSums()
        {
            SummationsVM res = new SummationsVM
            {
                //RV_T_Stock = (cxt.StockItems.Where(si => si.IsActive).Sum(si => ((int?)si.Quantity + si.BonusQuantity) * si.RetailPrice) ?? 0),
                //RV_T_Stock = (from si in cxt.StockItems where si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, RetailPrice = (from im in cxt.StockItems where im.IsActive && im.Item.ItemID == si.Item.ItemID && im.Batch.BatchId == si.Batch.BatchId orderby im.StockItemId descending select im.RetailPrice).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.RetailPrice)) ?? 0,
                //RV_T_C_Stock = (from sc in cxt.StockConsumptionItems where sc.IsActive select new { Quantity = sc.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == sc.Item.ItemID && si.Batch.BatchId == sc.Batch.BatchId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                //RV_T_A_Stock = (from ai in cxt.AdjustmentItems where ai.IsActive select new { Quantity = ai.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == ai.Item.ItemID && si.Batch.BatchId == ai.Batch.BatchId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                //RV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == s.Item.ItemID && s.Batch.BatchId == si.Batch.BatchId orderby s.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                //RV_T_C_E_Stock =  (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) orderby s.StockConsumptionItemId descending select new { Quantity = s.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == s.Item.ItemID && s.Batch.BatchId == si.Batch.BatchId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,

                ////CV_T_Stock = (cxt.StockItems.Where(si => si.IsActive).Sum(si => ((int?)si.Quantity + si.BonusQuantity) * si.UnitCost) ?? 0),
                //CV_T_Stock = (from si in cxt.StockItems where si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, CostPrice = (from im in cxt.StockItems where im.IsActive && im.Item.ItemID == si.Item.ItemID && im.Batch.BatchId == si.Batch.BatchId orderby im.StockItemId descending select im.UnitCost).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.CostPrice)) ?? 0,
                //CV_T_C_Stock = (from sc in cxt.StockConsumptionItems where sc.IsActive select new { Quantity = sc.Quantity, CostPrice = (from si in cxt.StockItems where si.Item.ItemID == sc.Item.ItemID && si.Batch.BatchId == sc.Batch.BatchId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.CostPrice)) ?? 0,
                //CV_T_A_Stock = (from ai in cxt.AdjustmentItems where ai.IsActive select new { Quantity = ai.Quantity, CostPrice = (from si in cxt.StockItems where si.Item.ItemID == ai.Item.ItemID && si.Batch.BatchId == ai.Batch.BatchId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.CostPrice)) ?? 0,
                //CV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, UnitCost = (from si in cxt.StockItems where si.Item.ItemID == s.Item.ItemID && s.Batch.BatchId == si.Batch.BatchId orderby s.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
                //CV_T_C_E_Stock =(from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) orderby s.StockConsumptionItemId descending select new { Quantity = s.Quantity, UnitCost = (from si in cxt.StockItems where si.Item.ItemID == s.Item.ItemID && s.Batch.BatchId == si.Batch.BatchId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
                //TotalRevenue = (cxt.invoicePayments.Where(p => p.IsActive)).Sum(r => (double?)r.Payment) ?? 0
                RV_T_Stock = (from si in cxt.StockItems where si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, RetailPrice = (from rp in cxt.StockItems where rp.Item.ItemID == si.Item.ItemID where rp.IsActive orderby rp.StockItemId descending select rp.RetailPrice).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.RetailPrice)) ?? 0,
                RV_T_C_Stock = (from sc in cxt.StockConsumptionItems where sc.IsActive select new { Quantity = sc.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == sc.Item.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                RV_T_A_Stock = (from ai in cxt.AdjustmentItems where ai.IsActive select new { Quantity = ai.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == ai.Item.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                RV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == s.Item.ItemID && si.IsActive orderby s.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,
                RV_T_C_E_Stock = (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) orderby s.StockConsumptionItemId descending select new { Quantity = s.Quantity, RetailPrice = (from si in cxt.StockItems where si.Item.ItemID == s.Item.ItemID && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.RetailPrice)) ?? 0,

                //CV_T_Stock = (cxt.StockItems.Where(si => si.IsActive).Sum(si => ((int?)si.Quantity + si.BonusQuantity) * si.UnitCost) ?? 0),
                CV_T_Stock = (from si in cxt.StockItems where si.IsActive select new { Quantity = si.Quantity + si.BonusQuantity, CostPrice = (from im in cxt.StockItems where im.IsActive && im.Item.ItemID == si.Item.ItemID orderby im.StockItemId descending select im.UnitCost).FirstOrDefault() }).Sum(si => (double?)(si.Quantity * si.CostPrice)) ?? 0,
                CV_T_C_Stock = (from sc in cxt.StockConsumptionItems where sc.IsActive select new { Quantity = sc.Quantity, CostPrice = (from si in cxt.StockItems where si.IsActive && si.Item.ItemID == sc.Item.ItemID orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.CostPrice)) ?? 0,
                CV_T_A_Stock = (from ai in cxt.AdjustmentItems where ai.IsActive select new { Quantity = ai.Quantity, CostPrice = (from si in cxt.StockItems where si.IsActive && si.Item.ItemID == ai.Item.ItemID orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.CostPrice)) ?? 0,
                CV_T_E_Stock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) orderby s.StockItemId descending select new { Quantity = s.Quantity + s.BonusQuantity, UnitCost = (from si in cxt.StockItems where si.IsActive && si.Item.ItemID == s.Item.ItemID orderby s.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
                CV_T_C_E_Stock = (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && b.Expiry != null && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) orderby s.StockConsumptionItemId descending select new { Quantity = s.Quantity, UnitCost = (from si in cxt.StockItems where si.IsActive && si.Item.ItemID == s.Item.ItemID orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() }).Sum(r => (double?)(r.Quantity * r.UnitCost)) ?? 0,
                TotalRevenue = (cxt.invoicePayments.Where(p => p.IsActive && !p.IsQInvoice)).Sum(r => (double?)r.Payment) ?? 0
            };
            res.RetailValueOfAvailableStock = res.RV_T_Stock - res.RV_T_C_Stock + res.RV_T_A_Stock - (res.RV_T_E_Stock - res.RV_T_C_E_Stock);
            res.CostOfAvailableStock = res.CV_T_Stock - res.CV_T_C_Stock + res.CV_T_A_Stock - (res.CV_T_E_Stock - res.CV_T_C_E_Stock);
            return res;
        }
        public SummationsVM GetSums(DateTime DateFrom, DateTime DateTo, double InvoiceRef = 0, long Patientid = 0)
        {
            SummationsVM res = new SummationsVM();

            if (InvoiceRef > 0)
            {
                if (Patientid > 0)
                {
                    res.TotalRevenue = (cxt.invoicePayments.Where(p => p.Invoice.IsActive && !p.IsQInvoice && !p.Invoice.IsHoldInvoice && p.IsActive && p.PaymentDate >= DateFrom && p.PaymentDate <= DateTo && InvoiceRef == p.Invoice.InvoiceRefNo && Patientid == p.Invoice.Patient.PatientId)).Sum(r => (double?)r.Payment) ?? 0;
                }
                else
                {
                    res.TotalRevenue = (cxt.invoicePayments.Where(p => p.Invoice.IsActive && !p.IsQInvoice && !p.Invoice.IsHoldInvoice && p.IsActive && p.PaymentDate >= DateFrom && p.PaymentDate <= DateTo && InvoiceRef == p.Invoice.InvoiceRefNo)).Sum(r => (double?)r.Payment) ?? 0;
                }
            }
            else
            {
                if (Patientid > 0)
                {
                    res.TotalRevenue = (cxt.invoicePayments.Where(p => p.Invoice.IsActive && !p.IsQInvoice && !p.Invoice.IsHoldInvoice && p.IsActive && p.PaymentDate >= DateFrom && p.PaymentDate <= DateTo && Patientid == p.Invoice.Patient.PatientId)).Sum(r => (double?)r.Payment) ?? 0;
                }
                else
                {
                    res.TotalRevenue = (cxt.invoicePayments.Where(p => p.Invoice.IsActive && !p.IsQInvoice && !p.Invoice.IsHoldInvoice && p.IsActive && p.PaymentDate >= DateFrom && p.PaymentDate <= DateTo)).Sum(r => (double?)r.Payment) ?? 0;
                }
            }
            return res;
        }

        public SummationsVM GetSums_1(DateTime DateFrom, DateTime DateTo)
        {
            SummationsVM res = new SummationsVM
            {
                TotalRevenue = (cxt.invoicePayments.Where(p => p.Invoice.IsActive && !p.Invoice.IsQInvoice && !p.Invoice.IsHoldInvoice && p.IsActive &&
                    DbFunctions.TruncateTime(p.Invoice.UpdateDate) >= DbFunctions.TruncateTime(DateFrom)
                    && DbFunctions.TruncateTime(p.Invoice.UpdateDate) <= DbFunctions.TruncateTime(DateTo))).Sum(r => (double?)r.Payment) ?? 0
            };
            return res;
        }
        public SummationsVM GetSums(long InvoiceNo)
        {
            SummationsVM res = new SummationsVM
            {
                TotalRevenue = (cxt.invoicePayments.Where(p => p.Invoice.IsActive && !p.Invoice.IsQInvoice && !p.Invoice.IsHoldInvoice && p.IsActive && p.Invoice.InvoiceRefNo == InvoiceNo)).Sum(r => (double?)r.Payment) ?? 0
            };
            return res;
        }
        public IPagedList<InvoiceTransactionsVM> GetPendingPayments_Financial(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => i.IsActive && !i.IsQInvoice)
            .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                && i.Due > 0
            )
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                IsProcedureInvoice = i.IsProcedureInvoice,
                //InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                //{
                //IsActive = item.IsActive,
                //InvoiceItemId = item.InvoiceItemId,
                //ItemId = item.Item.ItemID,
                //ItemName = item.Item.ItemName,
                //Rate = item.Rate,
                //Quantity = item.Quantity,
                //Discount = item.Discount,
                //DiscountType = item.DiscountType,
                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                //Amount = item.Amount,
                //BatchId = item.Batch.BatchId,
                //BatchName = item.Batch.BatchName,
                //ReturnedQuantity = item.ReturnedQuantity
                //})).ToList(),
                //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                Patient = i.Patient,
                PatientName = i.Patient.Name
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public List<InvoiceTransactionsVM> GetPendingPayments_Financial_Reports(DateTime DateFrom, DateTime DateTo)
        {
            return cxt.Invoices
            .Where(i => i.IsActive && !i.IsQInvoice)
            .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                && i.Due > 0
            )
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceRefNo,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                IsProcedureInvoice = i.IsProcedureInvoice,
                //InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                //{
                //IsActive = item.IsActive,
                //InvoiceItemId = item.InvoiceItemId,
                //ItemId = item.Item.ItemID,
                //ItemName = item.Item.ItemName,
                //Rate = item.Rate,
                //Quantity = item.Quantity,
                //Discount = item.Discount,
                //DiscountType = item.DiscountType,
                //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                //Amount = item.Amount,
                //BatchId = item.Batch.BatchId,
                //BatchName = item.Batch.BatchName,
                //ReturnedQuantity = item.ReturnedQuantity
                //})).ToList(),
                //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                Patient = i.Patient,
                PatientName = i.Patient.Name
            }).OrderByDescending(i => i.InvoiceId).ToList();
        }
        public SummationsVM GetSums_PendingPayments_Financial(DateTime DateFrom, DateTime DateTo)
        {
            SummationsVM Result = new SummationsVM();
            Result.TotalDues = (from i in cxt.Invoices
                                where i.IsActive && !i.IsQInvoice
                                && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                select i.Due).DefaultIfEmpty(0).Sum();
            return Result;
        }
        public IPagedList<InvoiceTransactionsVM> GetRefundInvoices_Financial(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return
             (from i in cxt.Invoices
              join ip in cxt.invoicePayments on i equals ip.Invoice
              where ip.Payment < 0 && i.IsActive && !i.IsQInvoice && i.InvoiceItems.Any(ii => ii.ReturnedQuantity > 0)
              && (DbFunctions.TruncateTime(ip.PaymentDate) >= DbFunctions.TruncateTime(DateFrom))
              && (DbFunctions.TruncateTime(ip.PaymentDate) <= DbFunctions.TruncateTime(DateTo))
              select new InvoiceTransactionsVM
              {
                  InvoiceId = i.InvoiceId,
                  InvoiceRefNo = i.InvoiceRefNo,
                  CreateDate = i.CreateDate,
                  TotalAmount = i.TotalAmount,
                  TotalDiscount = i.TotalDiscount,
                  DiscountType = i.DiscountType,
                  GrandTotal = i.GrandTotal,
                  SubTotal = i.SubTotal,
                  Due = i.Due,
                  SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                  ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                  RefundDate = ip.PaymentDate,
                  TotalRefund = (double?)-1 * ip.Payment ?? 0,
                  //TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
                  ReasonForRefund = ip.RefundReason,
                  User = i.User,
                  DeletedAt = i.UpdateDate,
                  IsProcedureInvoice = i.IsProcedureInvoice,
                  InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                  {
                      //IsActive = item.IsActive,
                      //InvoiceItemId = item.InvoiceItemId,
                      //ItemId = item.Item.ItemID,
                      ProcedureName = item.Procedure.ProcedureName,
                      ItemName = item.Item.ItemName,
                      //Rate = item.Rate,
                      //Quantity = item.Quantity,
                      //Discount = item.Discount,
                      //DiscountType = item.DiscountType,
                      //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                      //Amount = item.Amount,
                      //BatchId = item.Batch.BatchId,
                      //BatchName = item.Batch.BatchName,
                      //ReturnedQuantity = item.ReturnedQuantity
                  })).ToList(),
                  //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                  //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                  //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                  //TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
                  //ReasonForRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(r => r.InvoicePaymentId).Select(r => r.RefundReason).FirstOrDefault()),
                  //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                  Patient = i.Patient,
                  PatientName = i.Patient.Name
              }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<InvoiceTransactionsVM> GetRefundInvoicesMrNum_Financial(long MrNum, int PageNo, int PageSize)
        {
            return
             (from i in cxt.Invoices
              join ip in cxt.invoicePayments on i equals ip.Invoice
              where ip.Payment < 0 && i.Patient.MRNo == MrNum
              where i.IsActive && i.InvoiceItems.Any(ii => ii.ReturnedQuantity > 0)
              select new InvoiceTransactionsVM
              {
                  InvoiceId = i.InvoiceId,
                  InvoiceRefNo = i.InvoiceRefNo,
                  CreateDate = i.CreateDate,
                  TotalAmount = i.TotalAmount,
                  TotalDiscount = i.TotalDiscount,
                  DiscountType = i.DiscountType,
                  GrandTotal = i.GrandTotal,
                  SubTotal = i.SubTotal,
                  Due = i.Due,
                  SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                  ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                  User = i.User,
                  DeletedAt = i.UpdateDate,
                  IsProcedureInvoice = i.IsProcedureInvoice,
                  InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                  {
                      //IsActive = item.IsActive,
                      //InvoiceItemId = item.InvoiceItemId,
                      //ItemId = item.Item.ItemID,
                      ProcedureName = item.Procedure.ProcedureName,
                      ItemName = item.Item.ItemName,
                      //Rate = item.Rate,
                      //Quantity = item.Quantity,
                      //Discount = item.Discount,
                      //DiscountType = item.DiscountType,
                      //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                      //Amount = item.Amount,
                      //BatchId = item.Batch.BatchId,
                      //BatchName = item.Batch.BatchName,
                      //ReturnedQuantity = item.ReturnedQuantity
                  })).ToList(),
                  //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                  RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                  //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                  TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
                  ReasonForRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(r => r.InvoicePaymentId).Select(r => r.RefundReason).FirstOrDefault()),
                  //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                  Patient = i.Patient,
                  PatientName = i.Patient.Name
              }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public double GetSumRefundInvoices_Financial(DateTime DateFrom, DateTime DateTo)
        {
            double RefundAmount = 0;
            var value =
            (from i in cxt.Invoices
             join ip in cxt.invoicePayments on i equals ip.Invoice
             where ip.Payment < 0
             where i.IsActive && i.InvoiceItems.Any(ii => ii.ReturnedQuantity > 0)
                 && DbFunctions.TruncateTime(ip.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                 && (DbFunctions.TruncateTime(ip.PaymentDate) <= DbFunctions.TruncateTime(DateTo))

             select new InvoiceTransactionsVM
             {
                 InvoiceId = i.InvoiceId,
                 TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
             }).OrderByDescending(i => i.InvoiceId).ToList();
            RefundAmount = value.Sum(x => x.TotalRefund);
            return RefundAmount;
        }
        public double GetSumRefundInvoicesByMrNo_Financial(long MrNum)
        {
            double RefundAmount = 0;
            var value =
            (from i in cxt.Invoices
             join ip in cxt.invoicePayments on i equals ip.Invoice
             where ip.Payment < 0 && i.Patient.MRNo == MrNum
             where i.IsActive && i.InvoiceItems.Any(ii => ii.ReturnedQuantity > 0)
             select new InvoiceTransactionsVM
             {
                 InvoiceId = i.InvoiceId,
                 TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
             }).OrderByDescending(i => i.InvoiceId).ToList();
            RefundAmount = value.Sum(x => x.TotalRefund);
            return RefundAmount;
        }

        public List<InvoiceTransactionsVM> GetRefundInvoices_Financial_Report(DateTime DateFrom, DateTime DateTo)
        {
            return cxt.Invoices
            .Where(i => i.IsActive)
            .Where(i => i.InvoiceItems.Any(ii => ii.ReturnedQuantity > 0))
            .Where(i => DbFunctions.TruncateTime(i.UpdateDate) >= DbFunctions.TruncateTime(DateFrom)
                && (DbFunctions.TruncateTime(i.UpdateDate) <= DbFunctions.TruncateTime(DateTo))
            )
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                User = i.User,
                DeletedAt = i.UpdateDate,
                IsProcedureInvoice = i.IsProcedureInvoice,
                InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(item => new InvoiceItemVM
                {
                    //IsActive = item.IsActive,
                    //InvoiceItemId = item.InvoiceItemId,
                    //ItemId = item.Item.ItemID,
                    ProcedureName = item.Procedure.ProcedureName,
                    ItemName = item.Item.ItemName,
                    //Rate = item.Rate,
                    //Quantity = item.Quantity,
                    //Discount = item.Discount,
                    //DiscountType = item.DiscountType,
                    //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                    //Amount = item.Amount,
                    //BatchId = item.Batch.BatchId,
                    //BatchName = item.Batch.BatchName,
                    //ReturnedQuantity = item.ReturnedQuantity
                })).ToList(),
                //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                //TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
                ReasonForRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(r => r.InvoicePaymentId).Select(r => r.RefundReason).FirstOrDefault()),
                //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                Patient = i.Patient,
                PatientName = i.Patient.Name
            }).OrderByDescending(i => i.InvoiceId)
            .ToList();
        }
        public Invoice GetInvoiceById_Inc_Pmnts_cons(long InvoiceId)
        {
            //return cxt.Invoices
            //    .Where(i => i.InvoiceId == InvoiceId)
            //    .Include(i => i.InvoicePayments)
            //    .Include(i => i.InvoiceItems.Select(it => it.Item))
            //    .Include(i => i.InvoiceItems.Select(it => it.Batch))
            //    .Include(i => i.InvoiceItems.Select(it => it.Procedure))
            //    .Include(i => i.StockConsumptions.Select(sc => sc.Item)).FirstOrDefault();

            return cxt.Invoices
                .Where(i => i.InvoiceId == InvoiceId && !i.IsQInvoice)
                .Include(i => i.InvoicePayments)
                .Include(i => i.Patient)
                .Include(i => i.InvoiceItems.Select(it => it.Item))
                .Include(i => i.InvoiceItems.Select(it => it.Batch))
                .Include(i => i.InvoiceItems.Select(it => it.Procedure))
                .Include(i => i.StockConsumption)
                .Include(i => i.StockConsumption.StockConsumptions)
                .Include(i => i.StockConsumption.StockConsumptions.Select(sc => sc.Item))
                .Include(i => i.StockConsumption.StockConsumptions.Select(sc => sc.Batch))
                .FirstOrDefault();
        }
        public InvoiceVM GetInvoiceByInvoiceId(long InvoiceId)
        {
            InvoiceVM result =
                cxt.Invoices.Where(i => i.InvoiceId == InvoiceId)
                .Select(i => new InvoiceVM
                {
                    InvoiceId = i.InvoiceId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreateDate = i.CreateDate,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    ModifiedDiscount = i.ModifiedDiscount,
                    DiscountType = i.DiscountType,
                    SubTotal = i.SubTotal,
                    GrandTotal = i.GrandTotal,
                    Store = i.Store,
                    TotalPaidAmount = i.TotalPaid,
                    Services = i.Services,
                    ServiceType = i.ServiceType,
                    PaymentType = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentType).FirstOrDefault()),
                    TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive).Sum(p => p.Payment) ?? 0,
                    Due = i.Due,
                    Note = i.Note,
                    Patient = new PatientVM
                    {
                        PatientId = i.Patient == null ? 0 : i.Patient.PatientId,
                        Name = i.Patient == null ? "" : i.Patient.Name,
                        Phone = i.Patient == null ? "" : i.Patient.Phone
                    },
                    InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                    {
                        IsActive = item.IsActive,
                        InvoiceItemId = item.InvoiceItemId,
                        Item = item.Item,
                        ItemId = item.Item.ItemID,
                        ItemName = item.Item.ItemName,
                        Unit = item.Unit,
                        UnitCost = item.PerUnitCostPrice,
                        Rate = item.Rate,
                        Rack = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == item.Item.ItemID select ri.Rack.Name).FirstOrDefault(),
                        Quantity = item.Quantity,
                        BonusQuantity = item.BonusQuantity,
                        Discount = item.Discount,
                        DiscountType = item.DiscountType,
                        DiscountTypeString = item.DiscountType == 1 ? "%" : "Value",
                        DiscountedPrice = item.DiscountPrice,
                        Amount = item.Amount,
                        NetAmount = item.NetAmount,
                        BatchId = item.Batch == null ? 0 : item.Batch.BatchId,
                        BatchName = item.Batch.BatchName,
                        ReturnedQuantity = item.ReturnedQuantity,
                        AffectStock = item.AffectStock,
                        SaleTax = item.saletax.ToString(),
                        SaleTaxType = item.saletaxType,
                        InvoiceID = i.InvoiceRefNo
                        //UnitCost = item.unit
                    })),
                    InvoicePayments = (i.InvoicePayments.Where(p => p.IsActive).Select(p => new InvoicePaymentVM
                    {
                        InvoicePaymentId = p.InvoicePaymentId,
                        PaymentType = p.PaymentType,
                        Payment = p.Payment,
                        ChequeNumber = p.ChequeNumber,
                        BankName = p.BankName,
                        ChequeStatus = p.ChequeStatus,
                        PaymentDate = p.PaymentDate,
                        P_InvoiceNo = p.P_InvoiceNo,
                        P_RiderName = p.P_RiderName,
                        P_VendorName = p.P_VendorName,
                    }))
                }).FirstOrDefault();
            return result;
        }
        public IPagedList<InvoiceVM> GetHoldingInvoices(int pageNo, int PageSize)
        {
            IPagedList<InvoiceVM> result =
                cxt.Invoices.Where(i => i.IsHoldInvoice && i.IsActive && !i.IsQInvoice)
                .Select(i => new InvoiceVM
                {
                    InvoiceId = i.InvoiceId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreateDate = i.CreateDate,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    ModifiedDiscount = i.ModifiedDiscount,
                    DiscountType = i.DiscountType,
                    SubTotal = i.SubTotal,
                    GrandTotal = i.GrandTotal,
                    TotalPaid = (double?)i.InvoicePayments.Where(p => p.IsActive).Sum(p => p.Payment) ?? 0,
                    Due = i.Due,
                    Note = i.Note,
                    Patient = new PatientVM
                    {
                        PatientId = i.Patient == null ? 0 : i.Patient.PatientId,
                        Name = i.Patient == null ? "" : i.Patient.Name
                    },
                    InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                    {
                        IsActive = item.IsActive,
                        InvoiceItemId = item.InvoiceItemId,
                        Item = item.Item,
                        ItemId = item.Item.ItemID,
                        ItemName = item.Item.ItemName,
                        Unit = item.Unit,
                        UnitCost = item.PerUnitCostPrice,
                        Rate = item.Rate,
                        Rack = (from ri in cxt.RackItems where ri.IsActive && ri.Rack.IsActive && ri.Item.ItemID == item.Item.ItemID select ri.Rack.Name).FirstOrDefault(),
                        Quantity = item.Quantity,
                        BonusQuantity = item.BonusQuantity,
                        DiscountedPrice = item.DiscountPrice,
                        Discount = item.Discount,
                        DiscountType = item.DiscountType,
                        SaleTax = item.saletax.ToString(),
                        SaleTaxType = item.saletaxType,
                        DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                        Amount = item.Amount,
                        NetAmount = item.NetAmount,
                        BatchId = item.Batch.BatchId,
                        BatchName = item.Batch.BatchName,
                        ReturnedQuantity = item.ReturnedQuantity,
                        //UnitCost = item.unit
                    })),
                    InvoicePayments = (i.InvoicePayments.Where(p => p.IsActive).Select(p => new InvoicePaymentVM
                    {
                        InvoicePaymentId = p.InvoicePaymentId,
                        PaymentType = p.PaymentType,
                        Payment = p.Payment,
                        ChequeNumber = p.ChequeNumber,
                        BankName = p.BankName,
                        ChequeStatus = p.ChequeStatus,
                        PaymentDate = p.PaymentDate,
                    }))
                })
                .OrderByDescending(i => i.InvoiceId)
                .ToPagedList(pageNo, PageSize);
            return result;
        }
        public string RackName(int ItemId)
        {
            return cxt.RackItems.Where(x => x.Rack.IsActive && x.IsActive && x.Item.ItemID == ItemId).Select(x => x.Rack.Name).FirstOrDefault();
        }
        public InvoiceVM GetProcedureInvoiceByInvoiceId(long InvoiceId)
        {
            InvoiceVM result =
                cxt.Invoices.Where(i => i.InvoiceId == InvoiceId && !i.IsQInvoice)
                .Select(i => new InvoiceVM
                {
                    InvoiceId = i.InvoiceId,
                    CreateDate = i.CreateDate,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    DiscountType = i.DiscountType,
                    SubTotal = i.SubTotal,
                    GrandTotal = i.GrandTotal,
                    Due = i.Due,
                    Patient = new PatientVM
                    {
                        PatientId = i.Patient == null ? 0 : i.Patient.PatientId,
                        Name = i.Patient == null ? "" : i.Patient.Name
                    },
                    InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                    {
                        IsActive = item.IsActive,
                        InvoiceItemId = item.InvoiceItemId,
                        ProcedureId = item.Procedure.ProcedureId,
                        ProcedureName = item.Procedure.ProcedureName,
                        ProcedureDescription = item.ProcedureDescription,
                        Rate = item.Rate,
                        Quantity = item.Quantity,
                        Discount = item.Discount,
                        DiscountType = item.DiscountType,
                        DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                        Amount = item.Amount,
                        ReturnedQuantity = item.ReturnedQuantity
                    })),
                    InvoicePayments = (i.InvoicePayments.Where(p => p.IsActive).Select(p => new InvoicePaymentVM
                    {
                        InvoicePaymentId = p.InvoicePaymentId,
                        PaymentType = p.PaymentType,
                        Payment = p.Payment,
                        PaymentDate = p.PaymentDate,
                        ChequeNumber = p.ChequeNumber,
                        BankName = p.BankName,
                        ChequeStatus = p.ChequeStatus
                    }))
                }).FirstOrDefault();
            return result;
        }

        public IPagedList<InvoiceReturnVM> GetInvoiceReturns(DateTime FromDate, DateTime Dateto, int PageNo, int Pagesize)
        {
            return cxt.Invoices
                .Where(i => i.IsActive && !i.IsQInvoice)
                .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                    && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto))
                    .Where(i => i.InvoiceItems.Any(it => it.ReturnedQuantity > 0))
                    .Select(ir => new InvoiceReturnVM
                    {
                        InvoiceId = ir.InvoiceId,
                        InvoiceRefNo = ir.InvoiceRefNo,
                        IsProcedureInvoice = ir.IsProcedureInvoice,
                        Patient = ir.Patient,
                        Total = ir.SubTotal,
                        Paid = (ir.InvoicePayments.Where(p => p.Invoice.InvoiceId == ir.InvoiceId)).Sum(p => p.Payment),
                        PaymentDate = (ir.InvoicePayments.OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                        CreateDate = ir.CreateDate,
                        UserName = ir.User.UserName
                    }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, Pagesize);
        }
        public IPagedList<InvoiceReturnVM> GetInvoiceReturns(int PageNo, int Pagesize)
        {
            return cxt.Invoices
                .Where(i => i.IsActive && !i.IsQInvoice)
                    .Where(i => i.InvoiceItems.Any(it => it.ReturnedQuantity > 0))
                    .Select(ir => new InvoiceReturnVM
                    {
                        InvoiceId = ir.InvoiceId,
                        InvoiceRefNo = ir.InvoiceRefNo,
                        IsProcedureInvoice = ir.IsProcedureInvoice,
                        Patient = ir.Patient,
                        Total = ir.SubTotal,
                        Paid = (ir.InvoicePayments.Where(p => p.Invoice.InvoiceId == ir.InvoiceId)).Sum(p => p.Payment),
                        PaymentDate = (ir.InvoicePayments.OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                        CreateDate = ir.CreateDate,
                        UserName = ir.User.UserName

                    }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, Pagesize);
        }
        public IPagedList<InvoiceReturnVM> GetInvoiceReturns(DateTime FromDate, DateTime Dateto, long invoiceRefNo, int PageNo, int Pagesize)
        {
            return cxt.Invoices
                    .Where(i => i.InvoiceRefNo == invoiceRefNo && !i.IsQInvoice)
                    .Where(i => i.InvoiceItems.Any(it => it.ReturnedQuantity > 0))
                    .Select(ir => new InvoiceReturnVM
                    {
                        InvoiceId = ir.InvoiceId,
                        InvoiceRefNo = ir.InvoiceRefNo,
                        IsProcedureInvoice = ir.IsProcedureInvoice,
                        Patient = ir.Patient,
                        Total = ir.SubTotal,
                        Paid = (ir.InvoicePayments.Where(p => p.Invoice.InvoiceId == ir.InvoiceId)).Sum(p => p.Payment),
                        PaymentDate = (ir.InvoicePayments.OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                        CreateDate = ir.CreateDate,
                        UserName = ir.User.UserName
                    }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, Pagesize);
        }
        public IPagedList<InvoiceReturnVM> GetInvoiceReturns(DateTime FromDate, DateTime Dateto, int UserID, int PageNo, int Pagesize)
        {
            return cxt.Invoices
                    .Where(i => i.IsActive && i.User != null && i.User.UserId == UserID && !i.IsQInvoice)
                    .Where(i => i.InvoiceItems.Any(it => it.ReturnedQuantity > 0))
                      .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                    && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto))
                    .Select(ir => new InvoiceReturnVM
                    {
                        InvoiceId = ir.InvoiceId,
                        InvoiceRefNo = ir.InvoiceRefNo,
                        IsProcedureInvoice = ir.IsProcedureInvoice,
                        Patient = ir.Patient,
                        Total = ir.SubTotal,
                        Paid = (ir.InvoicePayments.Where(p => p.Invoice.InvoiceId == ir.InvoiceId)).Sum(p => p.Payment),
                        PaymentDate = (ir.InvoicePayments.OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                        CreateDate = ir.CreateDate,
                        UserName = ir.User.UserName
                    }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, Pagesize);
        }
        public IPagedList<InvoiceReturnVM> GetInvoiceReturns(DateTime FromDate, DateTime Dateto, string SearchString, int PageNo, int Pagesize)
        {
            return cxt.Invoices
                .Where(i => i.IsActive && !i.IsQInvoice)
                .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                    && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto))
                    .Where(i => i.InvoiceItems.Any(it => it.ReturnedQuantity > 0))
                    .Where(i => i.Patient.Name.ToLower().Contains(SearchString) || i.Patient.MRNo.ToString().Contains(SearchString))
                    .Select(ir => new InvoiceReturnVM
                    {
                        InvoiceId = ir.InvoiceId,
                        InvoiceRefNo = ir.InvoiceRefNo,
                        IsProcedureInvoice = ir.IsProcedureInvoice,
                        Patient = ir.Patient,
                        Total = ir.SubTotal,
                        Paid = (ir.InvoicePayments.Where(p => p.Invoice.InvoiceId == ir.InvoiceId)).Sum(p => p.Payment),
                        PaymentDate = (ir.InvoicePayments.OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                        CreateDate = ir.CreateDate,
                        UserName = ir.User.UserName
                    }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, Pagesize);
        }

        // this is correct method
        public IPagedList<NarcoticsDrugInvoiceVM> GetInvoicesWithNarcoticDrugs(DateTime FromDate, DateTime Dateto, int PageNo, int Pagesize)
        {
            //IPagedList<NarcoticsDrugInvoiceVM> nds = 
            //    return
            //        cxt.Invoices
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
            //        && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
            //        && (i.InvoiceItems.Any(item => item.Item.IsNarcotic)))
            //.Select(i => new NarcoticsDrugInvoiceVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
            //    PatientName = i.Patient == null ? "" : i.Patient.Name,
            //    //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
            //    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
            //    TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
            //   InvoicePayments = (i.InvoicePayments.Where(ip=>ip.IsActive)).ToList(),
            //    InvoiceItems = (i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
            //    {
            //        ItemName = item.Item.ItemName,
            //        Rate = item.Rate,
            //        Quantity = item.Quantity,
            //        ReturnedQuantity = item.ReturnedQuantity,
            //        Amount = item.Amount,
            //        NetAmount = item.NetAmount
            //    })).ToList()
            //}).OrderByDescending(I => I.InvoiceId).ToPagedList(PageNo, Pagesize);

            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && ii.Item.IsNarcotic && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,// (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                      Payment = InvGroup.Key.p.Payment,// (i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      //TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          BatchId = item.Batch.BatchId,
                          ItemName = item.Item.ItemName,
                          Rate = item.Rate,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount
                      })).ToList()
                  })).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, Pagesize);

            //IPagedList<NarcoticsDrugInvoiceVM> items;
            //bool Revenue = 0;
            ////bool itemFound = false;
            //foreach (NarcoticsDrugInvoiceVM i in nds)
            //{
            //    foreach (InvoiceItemVM ii in i.InvoiceItems)
            //    {
            //        Revenue = (ii.Amount / i.GrandTotal) * i.TotalPaid;
            //        //foreach (var item in items)
            //        //{
            //        //    if (item.ItemId == ii.ItemId)
            //        //    {
            //        //        itemFound = true;
            //        //        item.Revenue += ii.Revenue;
            //        //        break;// exit if found to avoid extra iterations
            //        //    }
            //        //}
            //        //if (!itemFound)
            //        //{
            //        //    items.Add(ii);
            //        //}
            //    }
            //}
            ////return items;
            //return nds;
        }


        public List<string> LoadSupplierName(long ItemId, long BatchId)
        {
            List<string> SupplierName = new List<string>();
            try
            {
                using (PharmacyContext cxt = new PharmacyContext())
                {
                    SupplierName = cxt.Database.SqlQuery<string>("select su.Name from Stocks as s inner join Suppliers as su on s.Supplier_SupplierID = su.SupplierID where StockId IN (select Stock_StockId from StockItems where Item_ItemID = @ItemId and Batch_BatchId = @BatchId)",
                        new SqlParameter("@ItemId", ItemId),
                        new SqlParameter("@BatchId", BatchId))
                    .ToList();
                }
            }
            catch (Exception)
            {

            }
            return SupplierName;
        }
        public IPagedList<NarcoticsDrugInvoiceVM> GetInvoicesWithNarcoticDrugs(long InvoiceNo, int PageNo, int Pagesize)
        {
            //IPagedList<NarcoticsDrugInvoiceVM> nds = 
            //    return
            //        cxt.Invoices
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
            //        && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
            //        && (i.InvoiceItems.Any(item => item.Item.IsNarcotic)))
            //.Select(i => new NarcoticsDrugInvoiceVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
            //    PatientName = i.Patient == null ? "" : i.Patient.Name,
            //    //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
            //    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
            //    TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
            //   InvoicePayments = (i.InvoicePayments.Where(ip=>ip.IsActive)).ToList(),
            //    InvoiceItems = (i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
            //    {
            //        ItemName = item.Item.ItemName,
            //        Rate = item.Rate,
            //        Quantity = item.Quantity,
            //        ReturnedQuantity = item.ReturnedQuantity,
            //        Amount = item.Amount,
            //        NetAmount = item.NetAmount
            //    })).ToList()
            //}).OrderByDescending(I => I.InvoiceId).ToPagedList(PageNo, Pagesize);


            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where i.InvoiceId == InvoiceNo && !i.IsQInvoice
                  && ii.Item.IsNarcotic
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,// (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                      Payment = InvGroup.Key.p.Payment,// (i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      //TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          ItemName = item.Item.ItemName,
                          BatchId = item.Batch.BatchId,
                          Rate = item.Rate,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount
                      })).ToList()
                  })).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, Pagesize);

            //IPagedList<NarcoticsDrugInvoiceVM> items;
            //bool Revenue = 0;
            ////bool itemFound = false;
            //foreach (NarcoticsDrugInvoiceVM i in nds)
            //{
            //    foreach (InvoiceItemVM ii in i.InvoiceItems)
            //    {
            //        Revenue = (ii.Amount / i.GrandTotal) * i.TotalPaid;
            //        //foreach (var item in items)
            //        //{
            //        //    if (item.ItemId == ii.ItemId)
            //        //    {
            //        //        itemFound = true;
            //        //        item.Revenue += ii.Revenue;
            //        //        break;// exit if found to avoid extra iterations
            //        //    }
            //        //}
            //        //if (!itemFound)
            //        //{
            //        //    items.Add(ii);
            //        //}
            //    }
            //}
            ////return items;
            //return nds;
        }
        public IPagedList<NarcoticsDrugInvoiceVM> GetInvoicesWithNarcoticDrugs(DateTime FromDate, DateTime Dateto, string PatientName, int PageNo, int Pagesize)
        {
            //IPagedList<NarcoticsDrugInvoiceVM> nds = 
            //    return
            //        cxt.Invoices
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
            //        && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
            //        && (i.InvoiceItems.Any(item => item.Item.IsNarcotic)))
            //.Select(i => new NarcoticsDrugInvoiceVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
            //    PatientName = i.Patient == null ? "" : i.Patient.Name,
            //    //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
            //    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
            //    TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
            //   InvoicePayments = (i.InvoicePayments.Where(ip=>ip.IsActive)).ToList(),
            //    InvoiceItems = (i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
            //    {
            //        ItemName = item.Item.ItemName,
            //        Rate = item.Rate,
            //        Quantity = item.Quantity,
            //        ReturnedQuantity = item.ReturnedQuantity,
            //        Amount = item.Amount,
            //        NetAmount = item.NetAmount
            //    })).ToList()
            //}).OrderByDescending(I => I.InvoiceId).ToPagedList(PageNo, Pagesize);


            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && i.Patient != null && i.Patient.Name.ToLower().Contains(PatientName)
                  && ii.Item.IsNarcotic && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,// (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                      Payment = InvGroup.Key.p.Payment,// (i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      //TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          ItemName = item.Item.ItemName,
                          Rate = item.Rate,
                          BatchId = item.Batch.BatchId,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount
                      })).ToList()
                  })).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, Pagesize);

            //IPagedList<NarcoticsDrugInvoiceVM> items;
            //bool Revenue = 0;
            ////bool itemFound = false;
            //foreach (NarcoticsDrugInvoiceVM i in nds)
            //{
            //    foreach (InvoiceItemVM ii in i.InvoiceItems)
            //    {
            //        Revenue = (ii.Amount / i.GrandTotal) * i.TotalPaid;
            //        //foreach (var item in items)
            //        //{
            //        //    if (item.ItemId == ii.ItemId)
            //        //    {
            //        //        itemFound = true;
            //        //        item.Revenue += ii.Revenue;
            //        //        break;// exit if found to avoid extra iterations
            //        //    }
            //        //}
            //        //if (!itemFound)
            //        //{
            //        //    items.Add(ii);
            //        //}
            //    }
            //}
            ////return items;
            //return nds;
        }
        public IPagedList<NarcoticsDrugInvoiceVM> GetInvoicesWithNarcoticDrugs(DateTime FromDate, DateTime Dateto, string DrugName, int PageNo, int Pagesize, bool isDrugSearch)
        {
            //IPagedList<NarcoticsDrugInvoiceVM> nds = 
            //    return
            //        cxt.Invoices
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
            //        && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
            //        && (i.InvoiceItems.Any(item => item.Item.IsNarcotic)))
            //.Select(i => new NarcoticsDrugInvoiceVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
            //    PatientName = i.Patient == null ? "" : i.Patient.Name,
            //    //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
            //    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
            //    TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
            //   InvoicePayments = (i.InvoicePayments.Where(ip=>ip.IsActive)).ToList(),
            //    InvoiceItems = (i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
            //    {
            //        ItemName = item.Item.ItemName,
            //        Rate = item.Rate,
            //        Quantity = item.Quantity,
            //        ReturnedQuantity = item.ReturnedQuantity,
            //        Amount = item.Amount,
            //        NetAmount = item.NetAmount
            //    })).ToList()
            //}).OrderByDescending(I => I.InvoiceId).ToPagedList(PageNo, Pagesize);


            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && ii.Item.IsNarcotic && ii.Item.ItemName.ToLower().Contains(DrugName) && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,// (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                      Payment = InvGroup.Key.p.Payment,// (i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      //TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          ItemName = item.Item.ItemName,
                          BatchId = item.Batch.BatchId,
                          Rate = item.Rate,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount
                      })).ToList()
                  })).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, Pagesize);

            //IPagedList<NarcoticsDrugInvoiceVM> items;
            //bool Revenue = 0;
            ////bool itemFound = false;
            //foreach (NarcoticsDrugInvoiceVM i in nds)
            //{
            //    foreach (InvoiceItemVM ii in i.InvoiceItems)
            //    {
            //        Revenue = (ii.Amount / i.GrandTotal) * i.TotalPaid;
            //        //foreach (var item in items)
            //        //{
            //        //    if (item.ItemId == ii.ItemId)
            //        //    {
            //        //        itemFound = true;
            //        //        item.Revenue += ii.Revenue;
            //        //        break;// exit if found to avoid extra iterations
            //        //    }
            //        //}
            //        //if (!itemFound)
            //        //{
            //        //    items.Add(ii);
            //        //}
            //    }
            //}
            ////return items;
            //return nds;
        }
        public List<NarcoticsDrugInvoiceVM> GetInvoicesWithNarcoticDrugs(DateTime FromDate, DateTime Dateto)
        {

            //IPagedList<NarcoticsDrugInvoiceVM> nds = 
            //    return
            //        cxt.Invoices
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
            //        && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
            //        && (i.InvoiceItems.Any(item => item.Item.IsNarcotic)))
            //.Select(i => new NarcoticsDrugInvoiceVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    PatientName = i.Patient == null ? "" : i.Patient.Name,
            //    //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
            //    TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
            //   InvoicePayments = (i.InvoicePayments.Where(ip=>ip.IsActive)).ToList(),
            //    InvoiceItems = (i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
            //    {
            //        ItemName = item.Item.ItemName,
            //        Rate = item.Rate,
            //        Quantity = item.Quantity,
            //        ReturnedQuantity = item.ReturnedQuantity,
            //        Amount = item.Amount,
            //        NetAmount = item.NetAmount
            //    })).ToList()
            //}).OrderByDescending(I => I.InvoiceId).ToPagedList(PageNo, Pagesize);


            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && ii.Item.IsNarcotic && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,// (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      CreateBy = InvGroup.Key.i.User == null ? "" : InvGroup.Key.i.User.UserName,
                      Payment = InvGroup.Key.p.Payment,// (i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                      //TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          BatchId = item.Batch.BatchId,
                          ItemName = item.Item.ItemName,
                          Rate = item.Rate,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount
                      })).ToList()
                  })).OrderByDescending(I => I.PaymentDate).ToList();

            //IPagedList<NarcoticsDrugInvoiceVM> items;
            //bool Revenue = 0;
            ////bool itemFound = false;
            //foreach (NarcoticsDrugInvoiceVM i in nds)
            //{
            //    foreach (InvoiceItemVM ii in i.InvoiceItems)
            //    {
            //        Revenue = (ii.Amount / i.GrandTotal) * i.TotalPaid;
            //        //foreach (var item in items)
            //        //{
            //        //    if (item.ItemId == ii.ItemId)
            //        //    {
            //        //        itemFound = true;
            //        //        item.Revenue += ii.Revenue;
            //        //        break;// exit if found to avoid extra iterations
            //        //    }
            //        //}
            //        //if (!itemFound)
            //        //{
            //        //    items.Add(ii);
            //        //}
            //    }
            //}
            ////return items;
            //return nds;
        }
        public List<NarcoticsDrugInvoiceVM> GetInvoicesWithNarcoticDrugs(long InvoiceNo)
        {

            //IPagedList<NarcoticsDrugInvoiceVM> nds = 
            //    return
            //        cxt.Invoices
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
            //        && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
            //        && (i.InvoiceItems.Any(item => item.Item.IsNarcotic)))
            //.Select(i => new NarcoticsDrugInvoiceVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    PatientName = i.Patient == null ? "" : i.Patient.Name,
            //    //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
            //    TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
            //   InvoicePayments = (i.InvoicePayments.Where(ip=>ip.IsActive)).ToList(),
            //    InvoiceItems = (i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
            //    {
            //        ItemName = item.Item.ItemName,
            //        Rate = item.Rate,
            //        Quantity = item.Quantity,
            //        ReturnedQuantity = item.ReturnedQuantity,
            //        Amount = item.Amount,
            //        NetAmount = item.NetAmount
            //    })).ToList()
            //}).OrderByDescending(I => I.InvoiceId).ToPagedList(PageNo, Pagesize);


            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where i.InvoiceId == InvoiceNo
                  && ii.Item.IsNarcotic && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,// (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                      Payment = InvGroup.Key.p.Payment,// (i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      //TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemName = item.Item.ItemName,
                          ItemId = item.Item.ItemID,
                          BatchId = item.Batch.BatchId,
                          Rate = item.Rate,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount
                      })).ToList()
                  })).OrderByDescending(I => I.PaymentDate).ToList();

            //IPagedList<NarcoticsDrugInvoiceVM> items;
            //bool Revenue = 0;
            ////bool itemFound = false;
            //foreach (NarcoticsDrugInvoiceVM i in nds)
            //{
            //    foreach (InvoiceItemVM ii in i.InvoiceItems)
            //    {
            //        Revenue = (ii.Amount / i.GrandTotal) * i.TotalPaid;
            //        //foreach (var item in items)
            //        //{
            //        //    if (item.ItemId == ii.ItemId)
            //        //    {
            //        //        itemFound = true;
            //        //        item.Revenue += ii.Revenue;
            //        //        break;// exit if found to avoid extra iterations
            //        //    }
            //        //}
            //        //if (!itemFound)
            //        //{
            //        //    items.Add(ii);
            //        //}
            //    }
            //}
            ////return items;
            //return nds;
        }
        public List<NarcoticsDrugInvoiceVM> GetInvoicesWithNarcoticDrugs(DateTime FromDate, DateTime Dateto, string PatientName)
        {

            //IPagedList<NarcoticsDrugInvoiceVM> nds = 
            //    return
            //        cxt.Invoices
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
            //        && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
            //        && (i.InvoiceItems.Any(item => item.Item.IsNarcotic)))
            //.Select(i => new NarcoticsDrugInvoiceVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    PatientName = i.Patient == null ? "" : i.Patient.Name,
            //    //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
            //    TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
            //   InvoicePayments = (i.InvoicePayments.Where(ip=>ip.IsActive)).ToList(),
            //    InvoiceItems = (i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
            //    {
            //        ItemName = item.Item.ItemName,
            //        Rate = item.Rate,
            //        Quantity = item.Quantity,
            //        ReturnedQuantity = item.ReturnedQuantity,
            //        Amount = item.Amount,
            //        NetAmount = item.NetAmount
            //    })).ToList()
            //}).OrderByDescending(I => I.InvoiceId).ToPagedList(PageNo, Pagesize);


            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && i.Patient != null && i.Patient.Name.ToLower().Contains(PatientName)
                  && ii.Item.IsNarcotic && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,// (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                      Payment = InvGroup.Key.p.Payment,// (i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      //TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          ItemName = item.Item.ItemName,
                          Rate = item.Rate,
                          BatchId = item.Batch.BatchId,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount
                      })).ToList()
                  })).OrderByDescending(I => I.PaymentDate).ToList();

            //IPagedList<NarcoticsDrugInvoiceVM> items;
            //bool Revenue = 0;
            ////bool itemFound = false;
            //foreach (NarcoticsDrugInvoiceVM i in nds)
            //{
            //    foreach (InvoiceItemVM ii in i.InvoiceItems)
            //    {
            //        Revenue = (ii.Amount / i.GrandTotal) * i.TotalPaid;
            //        //foreach (var item in items)
            //        //{
            //        //    if (item.ItemId == ii.ItemId)
            //        //    {
            //        //        itemFound = true;
            //        //        item.Revenue += ii.Revenue;
            //        //        break;// exit if found to avoid extra iterations
            //        //    }
            //        //}
            //        //if (!itemFound)
            //        //{
            //        //    items.Add(ii);
            //        //}
            //    }
            //}
            ////return items;
            //return nds;
        }
        public List<NarcoticsDrugInvoiceVM> GetInvoicesWithNarcoticDrugs(DateTime FromDate, DateTime Dateto, string DrugName, bool IsDrugSearch)
        {

            //IPagedList<NarcoticsDrugInvoiceVM> nds = 
            //    return
            //        cxt.Invoices
            //.Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
            //        && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
            //        && (i.InvoiceItems.Any(item => item.Item.IsNarcotic)))
            //.Select(i => new NarcoticsDrugInvoiceVM
            //{
            //    InvoiceId = i.InvoiceId,
            //    CreateDate = i.CreateDate,
            //    GrandTotal = i.GrandTotal,
            //    SubTotal = i.SubTotal,
            //    TotalDiscount = i.TotalDiscount,
            //    DiscountType = i.DiscountType,
            //    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    PatientName = i.Patient == null ? "" : i.Patient.Name,
            //    //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
            //    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
            //    TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
            //   InvoicePayments = (i.InvoicePayments.Where(ip=>ip.IsActive)).ToList(),
            //    InvoiceItems = (i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
            //    {
            //        ItemName = item.Item.ItemName,
            //        Rate = item.Rate,
            //        Quantity = item.Quantity,
            //        ReturnedQuantity = item.ReturnedQuantity,
            //        Amount = item.Amount,
            //        NetAmount = item.NetAmount
            //    })).ToList()
            //}).OrderByDescending(I => I.InvoiceId).ToPagedList(PageNo, Pagesize);


            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && ii.Item.IsNarcotic && ii.Item.ItemName.ToLower().Contains(DrugName)
                  && ii.Item.IsNarcotic && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,// (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      //RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                      Payment = InvGroup.Key.p.Payment,// (i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      //TotalRefund = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemName = item.Item.ItemName,
                          ItemId = item.Item.ItemID,
                          Rate = item.Rate,
                          BatchId = item.Batch.BatchId,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount
                      })).ToList()
                  })).OrderByDescending(I => I.PaymentDate).ToList();

            //IPagedList<NarcoticsDrugInvoiceVM> items;
            //bool Revenue = 0;
            ////bool itemFound = false;
            //foreach (NarcoticsDrugInvoiceVM i in nds)
            //{
            //    foreach (InvoiceItemVM ii in i.InvoiceItems)
            //    {
            //        Revenue = (ii.Amount / i.GrandTotal) * i.TotalPaid;
            //        //foreach (var item in items)
            //        //{
            //        //    if (item.ItemId == ii.ItemId)
            //        //    {
            //        //        itemFound = true;
            //        //        item.Revenue += ii.Revenue;
            //        //        break;// exit if found to avoid extra iterations
            //        //    }
            //        //}
            //        //if (!itemFound)
            //        //{
            //        //    items.Add(ii);
            //        //}
            //    }
            //}
            ////return items;
            //return nds;
        }

        // this is date range method
        public DiscountsMasterVM GetDiscounts(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            int count = 0;
            count = cxt.Invoices
            .Where(i => i.IsActive && !i.IsQInvoice)
            .Where(i => i.ModifiedDiscount > 0
            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                   && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                   .Count();
            if (count <= 0)
            {
                return new DiscountsMasterVM { Discounts = new List<DiscountVM>().ToPagedList(1, 1) };
            }
            IPagedList<DiscountVM> Discounts = cxt.Invoices
            .Where(i => i.IsActive)
            .Where(i => i.ModifiedDiscount > 0
            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                   && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
            .Select(i => new DiscountVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                SubTotal = i.SubTotal,
                GrandTotal = i.GrandTotal,
                DiscountType = i.DiscountType,
                //ModifiedDiscount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal),
                ModifiedDiscount = i.ModifiedDiscount,
                //ModifiedDiscount = i
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0) ?? 0,
                Patient = i.Patient,
                InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                {
                    ItemName = item.Item.ItemName
                })).ToList()
            }).OrderByDescending(r => r.InvoiceId).ToPagedList(PageNo, PageSize);


            double totalDiscount = cxt.Invoices
            .Where(i => i.IsActive && !i.IsQInvoice
            && i.ModifiedDiscount > 0
            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
            && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
            .Sum(i => (double?)i.ModifiedDiscount ?? 0);
            //.Select(i => new DiscountVM
            //{
            //    ModifiedDiscount = (double?)i.ModifiedDiscount ?? 0,

            //}).Sum(r => (double?)r.ModifiedDiscount ?? 0) ?? 0;

            //double TotalDiscount = 0;
            //if (PageNo == 1)
            //{
            //    TotalDiscount = cxt.Invoices
            //                              .Where(i => i.IsActive && i.TotalDiscount > 0)
            //                              .Sum(i => (double?)i.TotalDiscount) ?? 0;
            //}
            return new DiscountsMasterVM
            {
                TotalDiscount = totalDiscount,
                Discounts = Discounts
            };
        }
        public DiscountsMasterVM GetDiscountByInvNo(long InvNo, int PageNo, int PageSize)
        {
            IPagedList<DiscountVM> Discounts = cxt.Invoices
            .Where(i => i.InvoiceRefNo == InvNo)
            .Where(i => i.ModifiedDiscount > 0 && !i.IsQInvoice)
            .Select(i => new DiscountVM
            {
                InvoiceId = i.InvoiceId,
                SubTotal = i.SubTotal,
                GrandTotal = i.GrandTotal,
                InvoiceRefNo = i.InvoiceRefNo,
                DiscountType = i.DiscountType,
                //ModifiedDiscount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal),
                ModifiedDiscount = i.ModifiedDiscount,
                //ModifiedDiscount = i
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment)) ?? 0,
                Patient = i.Patient,
                InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                {
                    ItemName = item.Item.ItemName
                })).ToList()
            }).OrderByDescending(r => r.InvoiceId).ToPagedList(PageNo, PageSize);


            double totalDiscount = (double?)(cxt.Invoices
            .Where(i => i.InvoiceRefNo == InvNo)
            .Where(i => i.ModifiedDiscount > 0 && !i.IsQInvoice)
            .Select(i => i.ModifiedDiscount)
            .DefaultIfEmpty(0)
            .Sum()) ?? 0;
            //.Select(i => new DiscountVM
            //{
            //    ModifiedDiscount = (double?)i.ModifiedDiscount ?? 0,

            //}).Sum(r => (double?)r.ModifiedDiscount ?? 0)) ?? 0;
            //double TotalDiscount = 0;
            //if (PageNo == 1)
            //{
            //    TotalDiscount = cxt.Invoices
            //                              .Where(i => i.IsActive && i.TotalDiscount > 0)
            //                              .Sum(i => (double?)i.TotalDiscount) ?? 0;
            //}
            //return new DiscountsMasterVM
            //{
            //    TotalDiscount = totalDiscount,
            //    Discounts = Discounts
            //};

            return new DiscountsMasterVM
            {
                TotalDiscount = 0,
                Discounts = Discounts
            };
        }
        public DiscountsMasterVM GetDiscountByPatient(long PatientId, int PageNo, int PageSize)
        {
            IPagedList<DiscountVM> Discounts = cxt.Invoices
                .Where(i => i.Patient != null && i.Patient.PatientId == PatientId)
            .Where(i => i.ModifiedDiscount > 0 && !i.IsQInvoice)
            .Select(i => new DiscountVM
            {
                InvoiceId = i.InvoiceId,
                SubTotal = i.SubTotal,
                GrandTotal = i.GrandTotal,
                DiscountType = i.DiscountType,
                InvoiceRefNo = i.InvoiceRefNo,
                //ModifiedDiscount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal),
                ModifiedDiscount = i.ModifiedDiscount,
                //ModifiedDiscount = i
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment)) ?? 0,
                Patient = i.Patient,
                InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                {
                    ItemName = item.Item.ItemName
                })).ToList()
            }).OrderByDescending(r => r.InvoiceId).ToPagedList(PageNo, PageSize);


            double totalDiscount = (double?)(cxt.Invoices
            .Where(i => i.Patient != null && i.Patient.PatientId == PatientId)
            .Where(i => i.ModifiedDiscount > 0 && !i.IsQInvoice)
            .Select(i => i.ModifiedDiscount)
            .DefaultIfEmpty(0)
            .Sum()) ?? 0;
            //.Select(i => new DiscountVM
            //{
            //    ModifiedDiscount = (double?)i.ModifiedDiscount ?? 0,

            //}).Sum(r => (double?)r.ModifiedDiscount ?? 0)) ?? 0;
            //double TotalDiscount = 0;
            //if (PageNo == 1)
            //{
            //    TotalDiscount = cxt.Invoices
            //                              .Where(i => i.IsActive && i.TotalDiscount > 0)
            //                              .Sum(i => (double?)i.TotalDiscount) ?? 0;
            //}
            //return new DiscountsMasterVM
            //{
            //    TotalDiscount = totalDiscount,
            //    Discounts = Discounts
            //};

            return new DiscountsMasterVM
            {
                TotalDiscount = 0,
                Discounts = Discounts
            };
        }
        public DiscountsMasterVM GetDiscounts(DateTime DateFrom, DateTime DateTo, bool GetFullDiscountSales, int PageNo, int PageSize)
        {
            IPagedList<DiscountVM> Discounts = cxt.Invoices
            .Where(i => i.IsActive && !i.IsQInvoice)
            .Where(i => i.ModifiedDiscount > 0)
            .Where(i => (i.DiscountType == 2 && i.TotalDiscount == 100) || (i.DiscountType == 1 && i.TotalDiscount == i.SubTotal) && !i.IsQInvoice)
            .Select(i => new DiscountVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                SubTotal = i.SubTotal,
                GrandTotal = i.GrandTotal,
                DiscountType = i.DiscountType,
                //ModifiedDiscount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal),
                ModifiedDiscount = i.ModifiedDiscount,
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                Patient = i.Patient,
                InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                {
                    ItemName = item.Item.ItemName
                })).ToList()
            }).OrderByDescending(r => r.InvoiceId).ToPagedList(PageNo, PageSize);
            double TotalDiscount = 0;
            if (PageNo == 1) // this will avoid getting total while each new page is fetched.
            {
                TotalDiscount = cxt.Invoices
                                          .Where(i => i.IsActive && i.TotalDiscount > 0 && !i.IsQInvoice)
                                          .Sum(i => (double?)i.ModifiedDiscount) ?? 0;
            }
            return new DiscountsMasterVM
            {
                //TotalDiscount = TotalDiscount,
                Discounts = Discounts
            };
        }
        // this is report method
        public List<DiscountVM> GetDiscounts_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            List<DiscountVM> discounts = cxt.Invoices
            .Where(i => i.IsActive && !i.IsQInvoice)
            .Where(i => i.ModifiedDiscount > 0
                && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                   && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                   )
            .Select(i => new DiscountVM
            {
                InvoiceId = i.InvoiceId,
                SubTotal = i.SubTotal,
                GrandTotal = i.GrandTotal,
                DiscountType = i.DiscountType,
                //Discount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal), // this field is being use din reports
                Discount = i.ModifiedDiscount, // this field is being use din reports
                //ModifiedDiscount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal),
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0),
                Patient = i.Patient,
                InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                {
                    ItemName = item.Item.ItemName
                })).ToList()
            }).OrderByDescending(r => r.InvoiceId).ToList();
            foreach (DiscountVM d in discounts)
            {
                if (d.Patient == null)
                {
                    d.PatientName = "";
                    d.MrNo = 0;
                }
                else
                {
                    d.PatientName = d.Patient.Name;
                    d.MrNo = (decimal)d.Patient.MRNo;
                }
                foreach (InvoiceItemVM i in d.InvoiceItems)
                {
                    d.Description += i.ItemName + ", ";
                }
                d.Description = string.IsNullOrEmpty(d.Description) ? "" : d.Description.Trim().TrimEnd(',');
            }
            return discounts;
        }
        public List<DiscountVM> GetDiscounts_Rpt(bool GetFullDiscountSales, DateTime DateFrom, DateTime DateTo)
        {
            List<DiscountVM> discounts = cxt.Invoices
            .Where(i => i.IsActive && !i.IsQInvoice)
            .Where(i => i.ModifiedDiscount > 0)
            .Where(i => (i.DiscountType == 2 && i.ModifiedDiscount == 100) || (i.DiscountType == 1 && i.TotalDiscount == i.SubTotal)
            && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                   && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
            .Select(i => new DiscountVM
            {
                InvoiceId = i.InvoiceId,
                SubTotal = i.SubTotal,
                GrandTotal = i.GrandTotal,
                DiscountType = i.DiscountType,
                //Discount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal), // this property is being used in reports
                Discount = i.ModifiedDiscount, // this property is being used in reports
                //ModifiedDiscount = i.DiscountType == 1 ? i.ModifiedDiscount : ((i.ModifiedDiscount / 100) * i.SubTotal),
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                Patient = i.Patient,
                InvoiceItems = (i.InvoiceItems.Select(item => new InvoiceItemVM
                {
                    ItemName = item.Item.ItemName
                })).ToList()
            }).OrderByDescending(r => r.InvoiceId).ToList();
            //foreach (DiscountVM d in discounts)
            //{
            //    if (d.Patient == null)
            //    {
            //        d.PatientName = "";
            //        d.MrNo = 0;
            //    }
            //    else
            //    {
            //        d.PatientName = d.Patient.Name;
            //        d.MrNo = (decimal)d.Patient.MRNo;
            //    }
            //    foreach (InvoiceItemVM i in d.InvoiceItems)
            //    {
            //        d.Description += i.ItemName + ", ";
            //    }
            //    d.Description = string.IsNullOrEmpty(d.Description) ? "" : d.Description.Trim().TrimEnd(',');
            //}
            return discounts;
        }

        public ProfitStatementMasterVM GetProfitStatements(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            ProfitStatementMasterVM vm = new ProfitStatementMasterVM();
            vm.ProfitStatementPaged = (from i in cxt.Invoices

                                       where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                       where DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                       where i.IsActive
                                       group i by DbFunctions.TruncateTime(i.CreateDate) into g
                                       select new ProfitStatementVM
                                       {
                                           Date = g.Key.ToString(),
                                           TotalSales = g.Sum(r => (double?)r.SubTotal) ?? 0,
                                           NetTotalSale = (double?)g.Sum(r => r.InvoiceItems.Where(p => p.IsActive).Sum(ii => ii.Amount - (ii.Amount / ii.Quantity) * ii.ReturnedQuantity)) ?? 0,
                                           TotalRevenue = (double?)g.Sum(r => r.InvoicePayments.Where(p => p.IsActive).Sum(p => p.Payment)) ?? 0,
                                           GrandTotals = g.Sum(r => (double?)r.GrandTotal) ?? 0,
                                           CostOfSales = g.Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.TotalCostPrice)) ?? 0,
                                           DiscountedCostofSales = g.Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.TotalDiscPrice > 0 ? ii.TotalDiscPrice : ii.TotalCostPrice)) ?? 0,
                                           Discount = g.Sum(r => (double?)r.ModifiedDiscount) ?? 0,
                                           //Expense = cxt.AccountExpenses.Where(x => x.IsActive && DbFunctions.TruncateTime(x.CreatedAt) == g.Key).Sum(x => (double?)x.Amount) ?? 0,
                                           //Discount = g.Sum(r => r.DiscountType == 1 ? r.ModifiedDiscount : ((r.ModifiedDiscount / 100) * r.SubTotal))
                                       }).OrderByDescending(r => r.Date).ToPagedList(PageNo, PageSize);
            vm.TotalSales = (from i in cxt.Invoices

                             where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                             where DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                             where i.IsActive
                             select i.SubTotal
                                      ).Sum(r => (double?)r) ?? 0;

            vm.NetTotalSales = (double?)(from i in cxt.Invoices

                                         where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                         where DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                         where i.IsActive
                                         select i
                                      ).Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.Amount - ((ii.Amount / ii.Quantity) * ii.ReturnedQuantity))) ?? 0;

            vm.TotalRevenue = (double?)(from i in cxt.Invoices

                                        where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                        where DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                        where i.IsActive
                                        select i
                     ).Sum(r => (double?)r.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment)) ?? 0;

            vm.TotalCostOfSales = (double?)(from i in cxt.Invoices

                                            where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                            where DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                            where i.IsActive
                                            select i
                     ).Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.TotalCostPrice)) ?? 0;
            vm.DiscountedCostofSales = (double?)(from i in cxt.Invoices

                                                 where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                                 where DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                                 where i.IsActive && !i.IsQInvoice
                                                 select i
                                            ).Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.TotalDiscPrice > 0 ? ii.TotalDiscPrice : ii.TotalCostPrice)) ?? 0;

            vm.TotalDiscount = (double?)(from i in cxt.Invoices

                                         where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                                         where DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                                         where i.IsActive
                                         select i
                     ).Sum(r => (double?)r.ModifiedDiscount) ?? 0;
            //Sum(r => r.DiscountType == 1 ? r.ModifiedDiscount : ((r.ModifiedDiscount / 100) * r.SubTotal)) ?? 0;

            return vm;
        }
        //public ProfitStatementMasterVM GetProfitStatements1(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        //{
        //    ProfitStatementMasterVM vm = new ProfitStatementMasterVM();
        //    vm.ProfitStatementPaged = (from i in cxt.Invoices
        //                               join p in cxt.invoicePayments on i equals p.Invoice

        //                               where DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
        //                               where DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
        //                               where i.IsActive && !i.IsQInvoice

        //                               select new ProfitStatementVM
        //                               {
        //                                   Date = p.PaymentDate.ToString(),
        //                                   TotalSales = (double)i.InvoiceItems.Where(ii=>ii.IsActive).Sum((ii=>ii.Quantity*ii.Rate - ii.ReturnedQuantity * ii.Rate),
        //                                   NetTotalSale = (double?)g.Sum(r => r.InvoiceItems.Where(p => p.IsActive).Sum(ii => ii.Amount - (ii.Amount / ii.Quantity) * ii.ReturnedQuantity)) ?? 0,
        //                                   TotalRevenue = (double?)g.Sum(r => r.InvoicePayments.Where(pp => pp.IsActive).Sum(pp => pp.Payment)) ?? 0,
        //                                   GrandTotals = g.Sum(r => (double?)r.GrandTotal) ?? 0,
        //                                   CostOfSales = g.Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.TotalCostPrice)) ?? 0,
        //                                   DiscountedCostofSales = g.Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.TotalDiscPrice > 0 ? ii.TotalDiscPrice : ii.TotalCostPrice)) ?? 0,
        //                                   Discount = g.Sum(r => (double?)r.ModifiedDiscount) ?? 0,
        //                                   //Expense = cxt.AccountExpenses.Where(x => x.IsActive && DbFunctions.TruncateTime(x.CreatedAt) == g.Key).Sum(x => (double?)x.Amount) ?? 0,
        //                                   //Discount = g.Sum(r => r.DiscountType == 1 ? r.ModifiedDiscount : ((r.ModifiedDiscount / 100) * r.SubTotal))
        //                               }).OrderByDescending(r => r.Date).ToPagedList(PageNo, PageSize);
        //    vm.TotalSales = (from i in cxt.Invoices
        //                     join p in cxt.invoicePayments on i equals p.Invoice

        //                     where DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
        //                     where DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
        //                     where i.IsActive && !i.IsQInvoice
        //                     select i.SubTotal
        //                              ).Sum(r => (double?)r) ?? 0;

        //    vm.NetTotalSales = (double?)(from i in cxt.Invoices
        //                                 join p in cxt.invoicePayments on i equals p.Invoice

        //                                 where DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
        //                                 where DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
        //                                 where i.IsActive && !i.IsQInvoice
        //                                 select i
        //                              ).Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.Amount - ((ii.Amount / ii.Quantity) * ii.ReturnedQuantity))) ?? 0;

        //    vm.TotalRevenue = (double?)(from i in cxt.Invoices
        //                                join p in cxt.invoicePayments on i equals p.Invoice

        //                                where DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
        //                                where DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
        //                                where i.IsActive && !i.IsQInvoice
        //                                select i
        //             ).Sum(r => (double?)r.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment)) ?? 0;

        //    vm.TotalCostOfSales = (double?)(from i in cxt.Invoices
        //                                    join p in cxt.invoicePayments on i equals p.Invoice

        //                                    where DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
        //                                    where DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
        //                                    where i.IsActive && !i.IsQInvoice
        //                                    select i
        //             ).Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.TotalCostPrice)) ?? 0;
        //    vm.DiscountedCostofSales = (double?)(from i in cxt.Invoices
        //                                         join p in cxt.invoicePayments on i equals p.Invoice

        //                                         where DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
        //                                         where DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
        //                                         where i.IsActive && !i.IsQInvoice
        //                                         select i
        //                                    ).Sum(r => (double?)r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => (double?)ii.TotalDiscPrice > 0 ? ii.TotalDiscPrice : ii.TotalCostPrice)) ?? 0;

        //    vm.TotalDiscount = (double?)(from i in cxt.Invoices
        //                                 join p in cxt.invoicePayments on i equals p.Invoice

        //                                 where DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
        //                                 where DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
        //                                 where i.IsActive && !i.IsQInvoice
        //                                 select i
        //             ).Sum(r => (double?)r.ModifiedDiscount) ?? 0;
        //    //Sum(r => r.DiscountType == 1 ? r.ModifiedDiscount : ((r.ModifiedDiscount / 100) * r.SubTotal)) ?? 0;

        //    return vm;
        //}

        public List<ProfitStatementVM> GetProfitStatements_Reports(DateTime DateFrom, DateTime DateTo)
        {
            return (from i in cxt.Invoices
                    where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                    where DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                    where i.IsActive
                    group i by DbFunctions.TruncateTime(i.UpdateDate) into g
                    select new ProfitStatementVM
                    {
                        Date = g.Key.ToString(),
                        TotalSales = g.Sum(r => r.SubTotal),
                        NetTotalSale = (double?)g.Sum(r => r.InvoiceItems.Where(p => p.IsActive).Sum(ii => ii.Amount - (ii.Amount / ii.Quantity) * ii.ReturnedQuantity)) ?? 0,
                        TotalRevenue = (double?)g.Sum(r => r.InvoicePayments.Sum(p => p.Payment)) ?? 0,
                        GrandTotals = g.Sum(r => r.GrandTotal),
                        CostOfSales = g.Sum(r => r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => ii.TotalCostPrice)),
                        DiscountedCostofSales = g.Sum(r => r.InvoiceItems.Where(ii => ii.IsActive).Sum(ii => ii.TotalDiscPrice > 0 ? ii.TotalDiscPrice : ii.TotalCostPrice)),
                        //Discount = g.Sum(r => r.DiscountType == 1 ? r.ModifiedDiscount : ((r.ModifiedDiscount / 100) * r.SubTotal))
                        Discount = g.Sum(r => r.ModifiedDiscount)

                    }).OrderByDescending(r => r.Date).ToList();
        }
        public List<ProfitStatementTransactionVM> GetGrossProfitStatements_Reports(DateTime DateFrom, DateTime DateTo)
        {
            return (from i in cxt.ProfitStatementTransactions
                    where DbFunctions.TruncateTime(i.TransactionDate) >= DbFunctions.TruncateTime(DateFrom)
                    where DbFunctions.TruncateTime(i.TransactionDate) <= DbFunctions.TruncateTime(DateTo)
                    group i by DbFunctions.TruncateTime(i.TransactionDate) into g
                    select new ProfitStatementTransactionVM
                    {
                        PaymentDate = (DateTime)g.Key,
                        DiscountedCostOfSales = g.Sum(r => r.TransactionDiscountedCostOfSale),
                        Expense = (double?)g.Sum(r => r.TransactionExpense) ?? 0,
                        TotalRevenue = (double?)g.Sum(r => r.TransactionRevenue) ?? 0,
                        TotalRefund = g.Sum(r => r.TransactionReturn),
                        CostOfSales = g.Sum(r => r.TransactionCostOfSales),
                        //Discount = g.Sum(r => r.DiscountType == 1 ? r.ModifiedDiscount : ((r.ModifiedDiscount / 100) * r.SubTotal))
                        Discount = g.Sum(r => r.TransactionDiscount)

                    }).OrderByDescending(r => r.PaymentDate).ToList();
        }

        public IPagedList<FinancialTransactionsVM> GetFinancialTrancation(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => i.IsActive && !i.IsQInvoice)
            .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                   && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
            .Select(i => new FinancialTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                RefundDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                TotalRefund = -1 * (i.InvoicePayments.Where(p => p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                Patient = i.Patient,
                InvoiceItems = (i.InvoiceItems.Where(ii => ii.IsActive).Select(ii => new InvoiceItemVM
                {
                    Item = ii.Item
                })).ToList()
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }

        public List<ShiftCollectionVM> ShiftCollection(DateTime DateFrom, DateTime DateTo, int Shift)
        {
            bool GetAllShiftsData = false;
            if (Shift == 0)
            {
                GetAllShiftsData = true;
            }
            //shift 1 : morning
            //shift 2 : afternoon
            //shift 3 : evening
            DateTime ShiftStart;
            DateTime ShiftEnd;
            List<ShiftCollectionVM> CollectionList = new List<ShiftCollectionVM>();
            if (Shift == 1 || (GetAllShiftsData && Shift == 0))
            {
                ShiftStart = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 0, 0, 0);
                ShiftEnd = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 7, 59, 59);
                ShiftCollectionVM Collection =
                    new ShiftCollectionVM
                    {
                        Shift = "Morning(12am - 8am)",
                        Collection = cxt.invoicePayments
                        .Where(p => p.IsActive && !p.IsQInvoice
                            && p.Payment > 0
                            && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                            && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                            && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                               >=
                               DbFunctions.CreateTime(ShiftStart.Hour, ShiftStart.Minute, ShiftStart.Second)
                               && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                               <=
                               DbFunctions.CreateTime(ShiftEnd.Hour, ShiftEnd.Minute, ShiftEnd.Second)
                            )//where ends
                            .Sum(p => (double?)p.Payment) ?? 0
                    };
                CollectionList.Add(Collection);
            }
            if (Shift == 2 || (GetAllShiftsData) && Shift == 0)
            {
                ShiftStart = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 8, 0, 0);
                ShiftEnd = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 15, 59, 59);
                ShiftCollectionVM Collection =
                    new ShiftCollectionVM
                    {
                        Shift = "Afternoon(8am - 4pm)",
                        Collection = cxt.invoicePayments
                        .Where(p => p.IsActive && !p.IsQInvoice
                                && p.Payment > 0
                                && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                                && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                                >=
                                DbFunctions.CreateTime(ShiftStart.Hour, ShiftStart.Minute, ShiftStart.Second)
                                && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                                <=
                                DbFunctions.CreateTime(ShiftEnd.Hour, ShiftEnd.Minute, ShiftEnd.Second)
                            )// where ends
                            .Sum(p => (double?)p.Payment) ?? 0
                        //Collection = (double?)cxt.invoicePayments.Where(p => p.IsActive && p.Payment > 0 && DbFunctions.TruncateTime(p.Date) >= DbFunctions.TruncateTime(ShiftStart) && DbFunctions.TruncateTime(p.Date) <= DbFunctions.TruncateTime(ShiftEnd)).Sum(p => p.Payment) ?? 0
                        //Collection = (double?)c.InvoicePayments.Sum(p => p.Payment) ?? 0
                    };
                CollectionList.Add(Collection);
            }
            if (Shift == 3 || (GetAllShiftsData) && Shift == 0)
            {
                ShiftStart = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 16, 0, 0);
                ShiftEnd = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 23, 59, 59);
                ShiftCollectionVM Collection =
                    new ShiftCollectionVM
                    {
                        Shift = "Evening(4pm - 12am)",
                        Collection = cxt.invoicePayments
                        .Where(p => p.IsActive && !p.IsQInvoice
                                && p.Payment > 0
                                && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                                && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                                >=
                                DbFunctions.CreateTime(ShiftStart.Hour, ShiftStart.Minute, ShiftStart.Second)
                                && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                                <=
                                DbFunctions.CreateTime(ShiftEnd.Hour, ShiftEnd.Minute, ShiftEnd.Second)

                            )// where ends
                            .Sum(p => (double?)p.Payment) ?? 0
                    };
                CollectionList.Add(Collection);
            }
            return CollectionList;
        }
        public List<AdminShiftSetting> LoadAdminShiftsSetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    List<AdminShiftSetting> setting = unitOfWork.AdminShiftsSettingRepository.GetShifts();
                    return setting;
                }
            }
            catch (Exception ex)
            {
                //SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
                return null;
            }
        }
        public List<ShiftCollectionVM> ShiftCollection1(DateTime DateFrom, DateTime DateTo, int Shift)
        {
            List<AdminShiftSetting> shifts = new List<AdminShiftSetting>();
            shifts = LoadAdminShiftsSetting();
            var shift = shifts.FirstOrDefault();
            bool GetAllShiftsData = false;
            if (Shift == 0)
            {
                GetAllShiftsData = true;
            }
            //shift 1 : morning
            //shift 2 : afternoon
            //shift 3 : evening
            DateTime ShiftStart = DateTime.Now;
            DateTime ShiftEnd = DateTime.Now;
            string shft = "";
            List<ShiftCollectionVM> CollectionList = new List<ShiftCollectionVM>();
            if (Shift == 1 || (GetAllShiftsData && Shift == 0))
            {
                foreach (var s in shifts)
                {
                    if (s.Code == 1)
                    {
                        ShiftStart = Convert.ToDateTime(s.StartTime.ToShortTimeString());
                        ShiftEnd = Convert.ToDateTime(s.EndTime.ToShortTimeString());
                        shft = s.Name + "(" + ShiftStart.ToShortTimeString() + " To " + ShiftEnd.ToShortTimeString() + ")";
                    }
                }

                ShiftCollectionVM Collection =
                    new ShiftCollectionVM
                    {
                        Shift = shft,
                        Collection = cxt.invoicePayments
                        .Where(p => p.IsActive && !p.IsQInvoice
                            && p.Payment > 0
                            && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                            && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                            && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                               >=
                               DbFunctions.CreateTime(ShiftStart.Hour, ShiftStart.Minute, ShiftStart.Second)
                               && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                               <=
                               DbFunctions.CreateTime(ShiftEnd.Hour, ShiftEnd.Minute, ShiftEnd.Second)
                            )//where ends
                            .Sum(p => (double?)p.Payment) ?? 0
                    };
                CollectionList.Add(Collection);
            }
            if (Shift == 2 || (GetAllShiftsData) && Shift == 0)
            {
                //ShiftStart = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 8, 0, 0);
                //ShiftEnd = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 15, 59, 59);
                foreach (var s in shifts)
                {
                    if (s.Code == 2)
                    {
                        ShiftStart = Convert.ToDateTime(s.StartTime.ToShortTimeString());
                        ShiftEnd = Convert.ToDateTime(s.EndTime.ToShortTimeString());
                        shft = s.Name + "(" + ShiftStart.ToShortTimeString() + " To " + ShiftEnd.ToShortTimeString() + ")";
                    }
                }

                ShiftCollectionVM Collection =
                    new ShiftCollectionVM
                    {
                        Shift = shft,
                        Collection = cxt.invoicePayments
                        .Where(p => p.IsActive && !p.IsQInvoice
                                && p.Payment > 0
                                && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                                && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                                >=
                                DbFunctions.CreateTime(ShiftStart.Hour, ShiftStart.Minute, ShiftStart.Second)
                                && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                                <=
                                DbFunctions.CreateTime(ShiftEnd.Hour, ShiftEnd.Minute, ShiftEnd.Second)
                            )// where ends
                            .Sum(p => (double?)p.Payment) ?? 0
                        //Collection = (double?)cxt.invoicePayments.Where(p => p.IsActive && p.Payment > 0 && DbFunctions.TruncateTime(p.Date) >= DbFunctions.TruncateTime(ShiftStart) && DbFunctions.TruncateTime(p.Date) <= DbFunctions.TruncateTime(ShiftEnd)).Sum(p => p.Payment) ?? 0
                        //Collection = (double?)c.InvoicePayments.Sum(p => p.Payment) ?? 0
                    };
                CollectionList.Add(Collection);
            }
            if (Shift == 3 || (GetAllShiftsData) && Shift == 0)
            {
                //ShiftStart = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 16, 0, 0);
                //ShiftEnd = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 23, 59, 59);
                foreach (var s in shifts)
                {
                    if (s.Code == 3)
                    {
                        ShiftStart = Convert.ToDateTime(s.StartTime.ToShortTimeString());
                        ShiftEnd = Convert.ToDateTime(s.EndTime.ToShortTimeString());
                        shft = s.Name + "(" + ShiftStart.ToShortTimeString() + " To " + ShiftEnd.ToShortTimeString() + ")";
                    }
                }

                ShiftCollectionVM Collection =
                    new ShiftCollectionVM
                    {
                        Shift = shft,
                        Collection = cxt.invoicePayments
                        .Where(p => p.IsActive && !p.IsQInvoice
                                && p.Payment > 0
                                && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                                && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                                >=
                                DbFunctions.CreateTime(ShiftStart.Hour, ShiftStart.Minute, ShiftStart.Second)
                                && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                                <=
                                DbFunctions.CreateTime(ShiftEnd.Hour, ShiftEnd.Minute, ShiftEnd.Second)

                            )// where ends
                            .Sum(p => (double?)p.Payment) ?? 0
                    };
                CollectionList.Add(Collection);
            }
            if (Shift == 4 || (GetAllShiftsData) && Shift == 0)
            {
                //ShiftStart = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 16, 0, 0);
                //ShiftEnd = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 23, 59, 59);
                foreach (var s in shifts)
                {
                    if (s.Code == 4)
                    {
                        ShiftStart = Convert.ToDateTime(s.StartTime.ToShortTimeString());
                        ShiftEnd = Convert.ToDateTime(s.EndTime.ToShortTimeString());
                        shft = s.Name + "(" + ShiftStart.ToShortTimeString() + " To " + ShiftEnd.ToShortTimeString() + ")";
                    }
                }

                ShiftCollectionVM Collection =
                    new ShiftCollectionVM
                    {
                        Shift = shft,
                        Collection = cxt.invoicePayments
                        .Where(p => p.IsActive && !p.IsQInvoice
                                && p.Payment > 0
                                && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                                && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                                && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                                >=
                                DbFunctions.CreateTime(ShiftStart.Hour, ShiftStart.Minute, ShiftStart.Second)
                                && DbFunctions.CreateTime(p.PaymentDate.Hour, p.PaymentDate.Minute, p.PaymentDate.Second)
                                <=
                                DbFunctions.CreateTime(ShiftEnd.Hour, ShiftEnd.Minute, ShiftEnd.Second)

                            )// where ends
                            .Sum(p => (double?)p.Payment) ?? 0
                    };
                CollectionList.Add(Collection);
            }
            return CollectionList;
        }

        public int GetTotalInvoiceCount()
        {
            return cxt.Invoices.Where(i => i.IsActive).Count();
        }

        public int GetCurrentDateInvoiceCount()
        {
            return cxt.Invoices.Where(i => i.IsActive && i.CreateDate.Date == DateTime.Now.Date).Count();
        }

        public List<SaleGraphVM> GetMonthlySalesGraph(DateTime DateFrom, DateTime DateTo)
        {
            List<SaleGraphVM> Result = new List<SaleGraphVM>();
            Result = (from i in cxt.Invoices
                      where i.IsActive && !i.IsQInvoice
                      && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                      && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                      group i by DbFunctions.TruncateTime(i.CreateDate) into g
                      select new SaleGraphVM
                      {
                          MonthDay = g.FirstOrDefault().CreateDate.Day,
                          GrandTotal = g.Sum(r => r.GrandTotal),
                      }).ToList();
            return Result;
        }

        public List<SaleGraphVM> GetMonthlyRevenueGraph(DateTime DateFrom, DateTime DateTo)
        {
            List<SaleGraphVM> Result = new List<SaleGraphVM>();
            Result = (from i in cxt.Invoices
                      where i.IsActive && !i.IsQInvoice
                      && DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                      && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)
                      group i by DbFunctions.TruncateTime(i.CreateDate) into g
                      select new SaleGraphVM
                      {
                          MonthDay = g.FirstOrDefault().CreateDate.Day,
                          GrandTotal = g.Sum(r => r.InvoicePayments.Sum(ip => ip.Payment)),
                      }).ToList();
            return Result;
        }

        public Invoice GetInvoiceToDelete(long invoiceId)
        {
            return cxt.Invoices
                .Where(i => i.InvoiceId == invoiceId && !i.IsQInvoice)
                .Include(i => i.InvoicePayments)
                .Include(i => i.StockConsumption)
                .Include(i => i.StockConsumption.StockConsumptions)
                .Include(i => i.InvoiceItems)
                .FirstOrDefault();
        }

        public int getInvoiceIdByRefNo(int invoiceRefNo)
        {
            Invoice inv = cxt.Invoices.Where(i => i.InvoiceRefNo == invoiceRefNo && !i.IsQInvoice)
                .FirstOrDefault();
            if (inv != null)
            {
                return (int)inv.InvoiceId;
            }
            else
            {
                return 0;
            }
        }
        public int getMaxInvoiceId()
        {
            return (int)cxt.Invoices.Max(i => i.InvoiceId);
        }
        public double GetInvoicesBalanceByPatientId(long patientId)
        {
            double getPatientBalance = 0;
            getPatientBalance = cxt.Invoices
                .Where(i => i.IsActive && i.Patient.PatientId == patientId)
                .Sum(i => (double?)i.Due) ?? 0;
            return getPatientBalance;
        }

        public List<XRptVM> getXRpt(int UserId)
        {
            return cxt.invoicePayments
                .Where(r => r.User.UserId == UserId && !r.IsQInvoice
                    && r.PaymentDate > (from c in cxt.POSClosings where c.IsActive orderby c.POSClosingId descending select c.ClosingDate).FirstOrDefault()
                    )
                    .Select(p => new XRptVM
                    {
                        InvoiceId = p.Invoice.InvoiceId,
                        InvoiceRef = p.Invoice.InvoiceRefNo,
                        TrTime = p.PaymentDate,
                        Amount = p.PaymentType == 1 ? p.Payment : 0
                    }).ToList();
        }
        public List<StockAddFilterVM> LoadPreviouslySoldItems(DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                return cxt.invoiceItems
                    .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom))
                    .Where(i => DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                    .Where(i => !i.IsQInvoice)
                    .GroupBy(i => new { i.Item })
                    .Select(i => new StockAddFilterVM
                    {
                        ItemId = i.Key.Item.ItemID,
                        ItemName = i.Key.Item.ItemName,
                        Manufecturer = i.Key.Item.Manufacturer,
                        Unit = i.Key.Item.Unit,
                        ConversionUnit = i.Key.Item.ConversionUnit,
                        UnitCost =
                        SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                            (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId ascending select si.UnitCost).FirstOrDefault()
                            :
                            (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId descending select si.UnitCost).FirstOrDefault(),
                        RetailPrice =
                        SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                        (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId ascending select si.RetailPrice).FirstOrDefault()
                        :
                        (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault(),

                        Quantity = i.Sum(gp => gp.Quantity - gp.ReturnedQuantity),
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.Key.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.Key.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.Key.Item.ItemID && s.ConsumptionType == 2 select new { qty = s.Quantity }).Sum(s => (int?)s.qty) ?? 0,
                        ConsumedStock1 = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.Key.Item.ItemID select new { qty = s.Quantity }).Sum(s => (int?)s.qty) ?? 0,

                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.Key.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { qty = s.Quantity, BQty = s.BonusQuantity }).Sum(s => (int?)(s.qty + s.BQty)) ?? 0,
                        ExpiredConsStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.Key.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == i.Key.Item.ItemID select new { qty = ai.Quantity }).Sum(ai => (int?)ai.qty) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn =
                          (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.Key.Item.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.Key.Item.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    })
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<StockAddFilterVM> LoadPreviouslySoldItems(long SupplierId, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                List<StockAddFilterVM> result = new List<StockAddFilterVM>();

                var items = cxt.invoiceItems
                     .Where(i => DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                     .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom))
                     .Where(i => !i.IsQInvoice)
                     .GroupBy(i => new { i.Item })
                     .Select(i => new StockAddFilterVM
                     {
                         SupplierIds = i.Key.Item.Suppliers.Select(x => x.SupplierID).ToList(),
                         ItemId = i.Key.Item.ItemID,
                         ItemName = i.Key.Item.ItemName,
                         Manufecturer = i.Key.Item.Manufacturer,
                         Unit = i.Key.Item.Unit,
                         ConversionUnit = i.Key.Item.ConversionUnit,
                         UnitCost =
                         SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                             (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId ascending select si.UnitCost).FirstOrDefault()
                             :
                             (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId descending select si.UnitCost).FirstOrDefault(),
                         RetailPrice =
                         SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                         (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId ascending select si.RetailPrice).FirstOrDefault()
                         :
                         (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault(),

                         Quantity = i.Sum(gp => gp.Quantity - gp.ReturnedQuantity),
                         TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.Key.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                         HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.Key.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                         ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.Key.Item.ItemID && s.ConsumptionType == 2 select new { qty = s.Quantity }).Sum(s => (int?)s.qty) ?? 0,
                         ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.Key.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { qty = s.Quantity, BQty = s.BonusQuantity }).Sum(s => (int?)(s.qty + s.BQty)) ?? 0,
                         ExpiredConsStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.Key.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                         AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == i.Key.Item.ItemID select new { qty = ai.Quantity }).Sum(ai => (int?)ai.qty) ?? 0,
                         //--StockReturnQuantity--//
                         StockReturn =
                           (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.Key.Item.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                         //--BonusQunatity--//
                         BonusStock =
                         (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.Key.Item.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                     })
                     .ToList();

                foreach (var item in items)
                {
                    if (item.SupplierIds.IndexOf(SupplierId) >= 0)
                        result.Add(item);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<StockAddFilterVM> LoadSaleReturn(int ItemId, int BatchId, long INvoiceId)
        {
            try
            {
                return cxt.invoiceItems
                     .Where(i => i.Item.ItemID == ItemId && i.Batch.BatchId == BatchId && !i.IsQInvoice)
                    .GroupBy(i => new { i.Item })
                    .Select(i => new StockAddFilterVM
                    {

                        ItemId = i.Key.Item.ItemID,
                        ItemName = i.Key.Item.ItemName,
                        Unit = i.Key.Item.Unit,
                        ConversionUnit = i.Key.Item.ConversionUnit,
                        InvoiceItemId = (from ii1 in cxt.invoiceItems join iv1 in cxt.Invoices on ii1.Invoice.InvoiceId equals iv1.InvoiceId where ii1.IsActive && iv1.IsActive && ii1.Item.ItemID == i.Key.Item.ItemID select ii1.InvoiceItemId).FirstOrDefault(),
                        UnitCost =
                        SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                            (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId ascending select si.UnitCost).FirstOrDefault()
                            :
                            (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId descending select si.UnitCost).FirstOrDefault(),
                        RetailPrice =
                        SharedVariables.AdminPharmacySetting.IsItemConumptionFifo ?
                        (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId ascending select si.RetailPrice).FirstOrDefault()
                        :
                        (from si in cxt.StockItems join s in cxt.Stocks on si.Stock.StockId equals s.StockId where s.IsActive && si.IsActive && si.Item.ItemID == i.Key.Item.ItemID orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault(),

                        Quantity = i.Sum(gp => gp.Quantity - gp.ReturnedQuantity),
                        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == i.Key.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                        HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == i.Key.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                        //BatchId = (from bi in cxt.StockItems join bii in cxt.Batches on bi.Batch.BatchId equals bii.BatchId where bi.IsActive && bi.Item.ItemID == i.Key.Item.ItemID select new { BatchId = bi.Batch.BatchId }),
                        BatchId = (from si in cxt.StockItems where si.IsActive && si.Item.ItemID == i.Key.Item.ItemID select si.Batch.BatchId).ToList(),
                        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == i.Key.Item.ItemID && s.Batch.BatchId == BatchId select new { qty = s.Quantity }).Sum(s => (int?)s.qty) ?? 0,
                        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == i.Key.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { qty = s.Quantity, BQty = s.BonusQuantity }).Sum(s => (int?)(s.qty + s.BQty)) ?? 0,
                        ExpiredConsStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == i.Key.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == i.Key.Item.ItemID select new { qty = ai.Quantity }).Sum(ai => (int?)ai.qty) ?? 0,
                        //--StockReturnQuantity--//
                        StockReturn =
                          (from sri in cxt.StockReturnItems join sr in cxt.StockReturns on sri.StockReturn.StockReturnId equals sr.StockReturnId where sri.Item.ItemID == i.Key.Item.ItemID && sr.IsActive select new { Quantity = sri.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                        //--BonusQunatity--//
                        BonusStock =
                        (from ii in cxt.invoiceItems join iv in cxt.Invoices on ii.Invoice.InvoiceId equals iv.InvoiceId where ii.IsActive && iv.IsActive && ii.Item.ItemID == i.Key.Item.ItemID select ii).Sum(bq => (int?)bq.BonusQuantity) ?? 0,
                    }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region NarcoticReport
        public List<NarcoticsDrugInvoiceVM> GetInvoicesWithNarcoticDrugsPrintReport(DateTime FromDate, DateTime Dateto)
        {
            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && ii.Item.IsNarcotic
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      InvoiceNoString = InvGroup.Key.i.InvoiceRefNo.ToString(),
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      CreateBy = InvGroup.Key.i.User == null ? "" : InvGroup.Key.i.User.UserName,
                      Payment = InvGroup.Key.p.Payment,
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          BatchId = item.Batch.BatchId,
                          ItemName = item.Item.ItemName,
                          Rate = item.Rate,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount,

                          TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == item.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                          ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == item.Item.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                          ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                          AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == item.Item.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                          ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                          HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == item.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,

                      })).ToList()
                  })).OrderByDescending(I => I.PaymentDate).ToList();

        }

        public List<NarcoticsDrugInvoiceVM> GetInvoicesWithNarcoticDrugsPrintReport(DateTime FromDate, DateTime Dateto, string DrugName, bool IsDrugSearch)
        {

            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && ii.Item.IsNarcotic && ii.Item.ItemName.ToLower().Contains(DrugName)
                  && ii.Item.IsNarcotic
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      InvoiceNoString = InvGroup.Key.i.InvoiceRefNo.ToString(),
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      CreateBy = InvGroup.Key.i.User == null ? "" : InvGroup.Key.i.User.UserName,
                      Payment = InvGroup.Key.p.Payment,
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          BatchId = item.Batch.BatchId,
                          ItemName = item.Item.ItemName,
                          Rate = item.Rate,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount,

                          TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == item.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                          ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == item.Item.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                          ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                          AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == item.Item.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                          ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                          HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == item.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                      })).ToList()
                  })).OrderByDescending(I => I.PaymentDate).ToList();
        }

        public List<NarcoticsDrugInvoiceVM> GetInvoicesWithNarcoticDrugsPrintReport(DateTime FromDate, DateTime Dateto, string PatientName)
        {
            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && i.Patient != null && i.Patient.Name.ToLower().Contains(PatientName)
                  && ii.Item.IsNarcotic
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      InvoiceNoString = InvGroup.Key.i.InvoiceRefNo.ToString(),
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      CreateBy = InvGroup.Key.i.User == null ? "" : InvGroup.Key.i.User.UserName,
                      Payment = InvGroup.Key.p.Payment,
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          BatchId = item.Batch.BatchId,
                          ItemName = item.Item.ItemName,
                          Rate = item.Rate,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount,

                          TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == item.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                          ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == item.Item.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                          ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                          AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == item.Item.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                          ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                          HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == item.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,

                      })).ToList()
                  })).OrderByDescending(I => I.PaymentDate).ToList();

        }

        public List<NarcoticsDrugInvoiceVM> GetInvoicesWithNarcoticDrugsPrintReport(long InvoiceNo)
        {
            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where i.InvoiceId == InvoiceNo
                  && ii.Item.IsNarcotic
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      InvoiceNoString = InvGroup.Key.i.InvoiceRefNo.ToString(),
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      CreateBy = InvGroup.Key.i.User == null ? "" : InvGroup.Key.i.User.UserName,
                      Payment = InvGroup.Key.p.Payment,// (i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          BatchId = item.Batch.BatchId,
                          ItemName = item.Item.ItemName,
                          Rate = item.Rate,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount,

                          TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == item.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
                          ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == item.Item.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
                          ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
                          AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == item.Item.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
                          ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
                          HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == item.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                      })).ToList()
                  })).OrderByDescending(I => I.PaymentDate).ToList();
        }
        #endregion

        #region NarcoticItemswiseSales
        public IPagedList<NarcoticsDrugInvoiceVM> NarcoticItemwiseSalesByItemName(DateTime FromDate, DateTime Dateto, string DrugName, int PageNo, int Pagesize)
        {
            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && ii.Item.IsNarcotic && ii.Item.ItemName.ToLower().Contains(DrugName) && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      InvoiceRefNo = InvGroup.Key.i.InvoiceRefNo,
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      Payment = InvGroup.Key.p.Payment,
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          ItemName = item.Item.ItemName,
                          BatchId = item.Batch.BatchId,
                          Rate = item.Rate,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount
                      })).ToList()
                  })).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, Pagesize);
        }

        public IPagedList<NarcoticsDrugInvoiceVM> NarcoticItemwiseSalesByManufacturer(DateTime FromDate, DateTime Dateto, int ManufacturerId, int PageNo, int Pagesize)
        {
            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && ii.Item.IsNarcotic && ii.Item.Manufacturer.ManufacturerId == ManufacturerId && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      InvoiceRefNo = InvGroup.Key.i.InvoiceRefNo,
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      Payment = InvGroup.Key.p.Payment,
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive && !item.IsQInvoice).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          ItemName = item.Item.ItemName,
                          BatchId = item.Batch.BatchId,
                          Rate = item.Rate,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount
                      })).ToList()
                  })).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, Pagesize);
        }

        public IPagedList<NarcoticsDrugInvoiceVM> NarcoticItemwiseSales(DateTime FromDate, DateTime Dateto, int PageNo, int Pagesize)
        {
            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && ii.Item.IsNarcotic && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      InvoiceRefNo = InvGroup.Key.i.InvoiceRefNo,
                      CreateDate = InvGroup.Key.i.CreateDate,
                      GrandTotal = InvGroup.Key.i.GrandTotal,
                      SubTotal = InvGroup.Key.i.SubTotal,
                      TotalDiscount = InvGroup.Key.i.TotalDiscount,
                      DiscountType = InvGroup.Key.i.DiscountType,
                      PaymentDate = InvGroup.Key.p.PaymentDate,
                      PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
                      Payment = InvGroup.Key.p.Payment,
                      TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive && !item.IsQInvoice).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          BatchId = item.Batch.BatchId,
                          ItemName = item.Item.ItemName,
                          Rate = item.Rate,
                          Quantity = item.Quantity,
                          ReturnedQuantity = item.ReturnedQuantity,
                          Amount = item.Amount,
                          NetAmount = item.NetAmount
                      })).ToList()
                  })).OrderByDescending(i => i.PaymentDate).ToPagedList(PageNo, Pagesize);
        }

        #endregion

        #region NarcoticItemwiseSalesReport
        //public List<NarcoticsDrugInvoiceVM> NarcoticItemwiseSalesReport(DateTime FromDate, DateTime Dateto)
        //{
        //    return
        //         (from i in cxt.Invoices
        //          join ii in cxt.invoiceItems on i equals ii.Invoice
        //          join p in cxt.invoicePayments on i equals p.Invoice
        //          where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
        //          && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
        //          && ii.Item.IsNarcotic
        //          group new { i, p } by new { i, p } into InvGroup
        //          select (new NarcoticsDrugInvoiceVM
        //          {
        //              InvoiceId = InvGroup.Key.i.InvoiceId,
        //              InvoiceNoString = InvGroup.Key.i.InvoiceRefNo.ToString(),
        //              CreateDate = InvGroup.Key.i.CreateDate,
        //              GrandTotal = InvGroup.Key.i.GrandTotal,
        //              SubTotal = InvGroup.Key.i.SubTotal,
        //              TotalDiscount = InvGroup.Key.i.TotalDiscount,
        //              DiscountType = InvGroup.Key.i.DiscountType,
        //              PaymentDate = InvGroup.Key.p.PaymentDate,
        //              PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
        //              CreateBy = InvGroup.Key.i.User == null ? "" : InvGroup.Key.i.User.UserName,
        //              Payment = InvGroup.Key.p.Payment,
        //              TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
        //              InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
        //              {
        //                  ItemId = item.Item.ItemID,
        //                  BatchId = item.Batch.BatchId,
        //                  BatchName = item.Batch.BatchName,
        //                  BatchExpiry = item.Batch.Expiry.HasValue == true ?
        //                  SqlFunctions.DateName("day", item.Batch.Expiry.Value) + "-"
        //                  + DbFunctions.Right("00" + item.Batch.Expiry.Value.Month, 2) + "-"
        //                  + SqlFunctions.DateName("year", item.Batch.Expiry.Value) :
        //                  "",

        //                  ItemName = item.Item.ItemName,
        //                  Rate = item.Rate,
        //                  Quantity = item.Quantity,
        //                  ReturnedQuantity = item.ReturnedQuantity,
        //                  Amount = item.Amount,
        //                  NetAmount = item.NetAmount,

        //                  TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == item.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
        //                  ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == item.Item.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
        //                  ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
        //                  AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == item.Item.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
        //                  ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
        //                  HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == item.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,

        //              })).ToList()
        //          })).ToList();

        //}

        //public List<NarcoticsDrugInvoiceVM> NarcoticItemwiseSalesByItemNameReport(DateTime FromDate, DateTime Dateto, string DrugName)
        //{

        //    return
        //         (from i in cxt.Invoices
        //          join ii in cxt.invoiceItems on i equals ii.Invoice
        //          join p in cxt.invoicePayments on i equals p.Invoice
        //          where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
        //          && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
        //          && ii.Item.IsNarcotic && ii.Item.ItemName.ToLower().Contains(DrugName)
        //          && ii.Item.IsNarcotic
        //          group new { i, p } by new { i, p } into InvGroup
        //          select (new NarcoticsDrugInvoiceVM
        //          {
        //              InvoiceId = InvGroup.Key.i.InvoiceId,
        //              InvoiceNoString = InvGroup.Key.i.InvoiceRefNo.ToString(),
        //              CreateDate = InvGroup.Key.i.CreateDate,
        //              GrandTotal = InvGroup.Key.i.GrandTotal,
        //              SubTotal = InvGroup.Key.i.SubTotal,
        //              TotalDiscount = InvGroup.Key.i.TotalDiscount,
        //              DiscountType = InvGroup.Key.i.DiscountType,
        //              PaymentDate = InvGroup.Key.p.PaymentDate,
        //              PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
        //              CreateBy = InvGroup.Key.i.User == null ? "" : InvGroup.Key.i.User.UserName,
        //              Payment = InvGroup.Key.p.Payment,
        //              TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
        //              InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
        //              {
        //                  ItemId = item.Item.ItemID,
        //                  BatchId = item.Batch.BatchId,
        //                  ItemName = item.Item.ItemName,
        //                  BatchName = item.Batch.BatchName,
        //                  BatchExpiry = item.Batch.Expiry.HasValue == true ?
        //                  SqlFunctions.DateName("day", item.Batch.Expiry.Value) + "-"
        //                  + DbFunctions.Right("00" + item.Batch.Expiry.Value.Month, 2) + "-"
        //                  + SqlFunctions.DateName("year", item.Batch.Expiry.Value) :
        //                  "",
        //                  Rate = item.Rate,
        //                  Quantity = item.Quantity,
        //                  ReturnedQuantity = item.ReturnedQuantity,
        //                  Amount = item.Amount,
        //                  NetAmount = item.NetAmount,

        //                  TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == item.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
        //                  ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == item.Item.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
        //                  ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
        //                  AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == item.Item.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
        //                  ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
        //                  HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == item.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
        //              })).ToList()
        //          })).ToList();
        //}

        //public List<NarcoticsDrugInvoiceVM> NarcoticItemwiseSalesByManufacturerReport(DateTime FromDate, DateTime Dateto, int ManufacturerId)
        //{

        //    return
        //         (from i in cxt.Invoices
        //          join ii in cxt.invoiceItems on i equals ii.Invoice
        //          join p in cxt.invoicePayments on i equals p.Invoice
        //          where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
        //          && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
        //          && ii.Item.IsNarcotic && ii.Item.Manufacturer.ManufacturerId == ManufacturerId
        //          && ii.Item.IsNarcotic
        //          group new { i, p } by new { i, p } into InvGroup
        //          select (new NarcoticsDrugInvoiceVM
        //          {
        //              InvoiceId = InvGroup.Key.i.InvoiceId,
        //              InvoiceNoString = InvGroup.Key.i.InvoiceRefNo.ToString(),
        //              CreateDate = InvGroup.Key.i.CreateDate,
        //              GrandTotal = InvGroup.Key.i.GrandTotal,
        //              SubTotal = InvGroup.Key.i.SubTotal,
        //              TotalDiscount = InvGroup.Key.i.TotalDiscount,
        //              DiscountType = InvGroup.Key.i.DiscountType,
        //              PaymentDate = InvGroup.Key.p.PaymentDate,
        //              PatientName = InvGroup.Key.i.Patient == null ? "" : InvGroup.Key.i.Patient.Name,
        //              CreateBy = InvGroup.Key.i.User == null ? "" : InvGroup.Key.i.User.UserName,
        //              Payment = InvGroup.Key.p.Payment,
        //              TotalPaid = (InvGroup.Key.i.InvoicePayments.Where(ip => ip.IsActive && ip.Payment > 0).Sum(ip => (double?)ip.Payment) ?? 0),
        //              InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive).Select(item => new InvoiceItemVM
        //              {
        //                  ItemId = item.Item.ItemID,
        //                  BatchId = item.Batch.BatchId,
        //                  BatchName = item.Batch.BatchName,
        //                  BatchExpiry = item.Batch.Expiry.HasValue == true ?
        //                  SqlFunctions.DateName("day", item.Batch.Expiry.Value) + "-"
        //                  + DbFunctions.Right("00" + item.Batch.Expiry.Value.Month, 2) + "-"
        //                  + SqlFunctions.DateName("year", item.Batch.Expiry.Value) :
        //                  "",
        //                  ItemName = item.Item.ItemName,
        //                  Rate = item.Rate,
        //                  Quantity = item.Quantity,
        //                  ReturnedQuantity = item.ReturnedQuantity,
        //                  Amount = item.Amount,
        //                  NetAmount = item.NetAmount,

        //                  TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemID == item.Item.ItemID).Sum(si => (int?)si.Quantity + si.BonusQuantity) ?? 0),
        //                  ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemID == item.Item.ItemID select s).Sum(s => (int?)s.Quantity) ?? 0,
        //                  ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
        //                  AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemID == item.Item.ItemID select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
        //                  ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemID == item.Item.ItemID && DbFunctions.TruncateTime(b_.Expiry) < DbFunctions.TruncateTime(DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (int?)rs.Quantity) ?? 0,
        //                  HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemID == item.Item.ItemID select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
        //              })).ToList()
        //          })).ToList();
        //}

        #endregion

        #region NarcoticItemwiseSalesReport
        public List<NarcoticsDrugInvoiceVM> NarcoticItemwiseSalesReport(DateTime FromDate, DateTime Dateto)
        {
            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && ii.Item.IsNarcotic && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      InvoiceNoString = InvGroup.Key.i.InvoiceRefNo.ToString(),
                      CreateBy = InvGroup.Key.i.User == null ? "" : InvGroup.Key.i.User.UserName,
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive && !item.IsQInvoice).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          ItemName = item.Item.ItemName,
                          BatchId = item.Batch.BatchId,
                          BatchName = item.Batch.BatchName,
                          CreatedDate = item.CreateDate,
                          ManufacturerName = item.Item.Manufacturer.Name,
                          BatchExpiry = item.Batch.Expiry.HasValue == true ?
                          SqlFunctions.DateName("day", item.Batch.Expiry.Value) + "-"
                          + DbFunctions.Right("00" + item.Batch.Expiry.Value.Month, 2) + "-"
                          + SqlFunctions.DateName("year", item.Batch.Expiry.Value) :
                          "",
                          Quantity = item.Quantity,
                      })).ToList()
                  })).ToList();

        }

        public List<NarcoticsDrugInvoiceVM> NarcoticItemwiseSalesByItemNameReport(DateTime FromDate, DateTime Dateto, string DrugName)
        {

            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && ii.Item.IsNarcotic && ii.Item.ItemName.ToLower().Contains(DrugName)
                  && ii.Item.IsNarcotic && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      InvoiceNoString = InvGroup.Key.i.InvoiceRefNo.ToString(),
                      CreateBy = InvGroup.Key.i.User == null ? "" : InvGroup.Key.i.User.UserName,
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive && !item.IsQInvoice).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          ItemName = item.Item.ItemName,
                          BatchId = item.Batch.BatchId,
                          BatchName = item.Batch.BatchName,
                          CreatedDate = item.CreateDate,
                          ManufacturerName = item.Item.Manufacturer.Name,
                          BatchExpiry = item.Batch.Expiry.HasValue == true ?
                          SqlFunctions.DateName("day", item.Batch.Expiry.Value) + "-"
                          + DbFunctions.Right("00" + item.Batch.Expiry.Value.Month, 2) + "-"
                          + SqlFunctions.DateName("year", item.Batch.Expiry.Value) :
                          "",
                          Quantity = item.Quantity,
                      })).ToList()
                  })).ToList();
        }

        public List<NarcoticsDrugInvoiceVM> NarcoticItemwiseSalesByManufacturerReport(DateTime FromDate, DateTime Dateto, int ManufacturerId)
        {

            return
                 (from i in cxt.Invoices
                  join ii in cxt.invoiceItems on i equals ii.Invoice
                  join p in cxt.invoicePayments on i equals p.Invoice
                  where DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(FromDate)
                  && DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(Dateto)
                  && ii.Item.IsNarcotic && ii.Item.Manufacturer.ManufacturerId == ManufacturerId
                  && ii.Item.IsNarcotic && !i.IsQInvoice
                  group new { i, p } by new { i, p } into InvGroup
                  select (new NarcoticsDrugInvoiceVM
                  {
                      InvoiceId = InvGroup.Key.i.InvoiceId,
                      InvoiceNoString = InvGroup.Key.i.InvoiceRefNo.ToString(),
                      CreateBy = InvGroup.Key.i.User == null ? "" : InvGroup.Key.i.User.UserName,
                      InvoiceItems = (InvGroup.Key.i.InvoiceItems.Where(item => item.Item.IsNarcotic && item.IsActive && !item.IsQInvoice).Select(item => new InvoiceItemVM
                      {
                          ItemId = item.Item.ItemID,
                          ItemName = item.Item.ItemName,
                          BatchId = item.Batch.BatchId,
                          BatchName = item.Batch.BatchName,
                          CreatedDate = item.CreateDate,
                          ManufacturerName = item.Item.Manufacturer.Name,
                          BatchExpiry = item.Batch.Expiry.HasValue == true ?
                          SqlFunctions.DateName("day", item.Batch.Expiry.Value) + "-"
                          + DbFunctions.Right("00" + item.Batch.Expiry.Value.Month, 2) + "-"
                          + SqlFunctions.DateName("year", item.Batch.Expiry.Value) :
                          "",
                          Quantity = item.Quantity,
                      })).ToList()
                  })).ToList();
        }

        #endregion

        #region frmInvoiceHistory
        public IPagedList<InvoiceTransactionsVM> GetTransactions(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => (i.IsActive) && !i.IsQInvoice && !i.IsHoldInvoice)
            .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
            )
            .Select(i => new InvoiceTransactionsVM
            {
                IsOnlineIntegrate = i.IsOnlineIntegrate,
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreateDate = i.CreateDate,
                Services = i.Services,
                IsQInvoice = i.IsQInvoice,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
                //TotalRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                UserName = i.User.UserName
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions(DateTime DateFrom, DateTime DateTo, long PatientId, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => (i.IsActive) && !i.IsQInvoice && i.Patient.PatientId == PatientId && !i.IsHoldInvoice)
             .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                    && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)))
            .Select(i => new InvoiceTransactionsVM
            {
                IsOnlineIntegrate = i.IsOnlineIntegrate,
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                IsQInvoice = i.IsQInvoice,
                Services = i.Services,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
                //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                UserName = i.User.UserName
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions(DateTime DateFrom, DateTime DateTo, int UserId, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => (i.IsActive) && !i.IsQInvoice && i.User != null && i.User.UserId == UserId && !i.IsHoldInvoice)
             .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                    && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)))
            .Select(i => new InvoiceTransactionsVM
            {
                IsOnlineIntegrate = i.IsOnlineIntegrate,
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreateDate = i.CreateDate,
                IsQInvoice = i.IsQInvoice,
                Services = i.Services,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
                //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                UserName = i.User.UserName
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions(long InvoiceNo, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => (i.IsActive) && !i.IsQInvoice && i.InvoiceRefNo == InvoiceNo && !i.IsHoldInvoice)
            .Select(i => new InvoiceTransactionsVM
            {
                IsOnlineIntegrate = i.IsOnlineIntegrate,
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                IsQInvoice = i.IsQInvoice,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                Services = i.Services,
                SubTotal = i.SubTotal,
                Due = i.Due,
                SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = (double?)(i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => p.Payment)) ?? 0,
                TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
                //TotalRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                UserName = i.User.UserName

            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions(int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => (i.IsActive) && !i.IsHoldInvoice)
            .Select(i => new InvoiceTransactionsVM
            {
                IsOnlineIntegrate = i.IsOnlineIntegrate,
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                IsQInvoice = i.IsQInvoice,
                DiscountType = i.DiscountType,
                Services = i.Services,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
                Due = i.Due,
                SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                //TotalRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                IsProcedureInvoice = i.IsProcedureInvoice,
                UserName = i.User.UserName
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<InvoiceTransactionsVM> GetTransactions(int PageNo, int PageSize, int InvoiceType)
        {
            if (InvoiceType == 0)
            {
                return cxt.Invoices
                .Where(i => (i.IsActive) && !i.IsQInvoice && !i.IsHoldInvoice)
                .Select(i => new InvoiceTransactionsVM
                {
                    IsOnlineIntegrate = i.IsOnlineIntegrate,
                    InvoiceId = i.InvoiceId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreateDate = i.CreateDate,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    IsQInvoice = i.IsQInvoice,
                    DiscountType = i.DiscountType,
                    Services = i.Services,
                    GrandTotal = i.GrandTotal,
                    SubTotal = i.SubTotal,
                    Due = i.Due,
                    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                    TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0,
                    //TotalRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                    Patient = i.Patient,
                    PatientName = i.Patient.Name,
                    IsProcedureInvoice = i.IsProcedureInvoice,
                    UserName = i.User.UserName
                }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            }
            else if (InvoiceType == 1)
            {
                return cxt.Invoices
                .Where(i => (i.IsActive) && !i.IsHoldInvoice && !i.IsQInvoice)
                .Select(i => new InvoiceTransactionsVM
                {
                    IsOnlineIntegrate = i.IsOnlineIntegrate,
                    InvoiceId = i.InvoiceId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreateDate = i.CreateDate,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    IsQInvoice = i.IsQInvoice,
                    DiscountType = i.DiscountType,
                    Services = i.Services,
                    GrandTotal = i.GrandTotal,
                    SubTotal = i.SubTotal,
                    Due = i.Due,
                    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                    //TotalRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                    Patient = i.Patient,
                    PatientName = i.Patient.Name,
                    IsProcedureInvoice = i.IsProcedureInvoice,
                    UserName = i.User.UserName
                }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            }
            else
            {
                return cxt.Invoices
                .Where(i => (i.IsActive) && !i.IsHoldInvoice && i.IsQInvoice)
                .Select(i => new InvoiceTransactionsVM
                {
                    IsOnlineIntegrate = i.IsOnlineIntegrate,
                    InvoiceId = i.InvoiceId,
                    InvoiceRefNo = i.InvoiceRefNo,
                    CreateDate = i.CreateDate,
                    TotalAmount = i.TotalAmount,
                    TotalDiscount = i.TotalDiscount,
                    IsQInvoice = i.IsQInvoice,
                    DiscountType = i.DiscountType,
                    Services = i.Services,
                    GrandTotal = i.GrandTotal,
                    SubTotal = i.SubTotal,
                    Due = i.Due,
                    SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                    ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                    PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                    //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                    //TotalRefund = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                    Patient = i.Patient,
                    PatientName = i.Patient.Name,
                    IsProcedureInvoice = i.IsProcedureInvoice,
                    UserName = i.User.UserName
                }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
            }
        }
        public InvoiceHistorySumVM GetTransactionsSum(DateTime DateFrom, DateTime DateTo)
        {
            List<InvoiceHistorySumVM> model = cxt.Invoices
                .Where(i => i.IsActive && !i.IsHoldInvoice && !i.IsQInvoice)
                .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
                && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)))
            .Select(i => new InvoiceHistorySumVM
            {
                TotalQty = (i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0) - (i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0),
                TotalDue = ((double?)i.Due ?? 0),
                TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0
            }).ToList();
            return new InvoiceHistorySumVM()
            {
                TotalInvoice = model.Count(),
                TotalQty = model.Sum(x => (int?)x.TotalQty) ?? 0,
                TotalPaid = model.Sum(x => (double?)x.TotalPaid) ?? 0,
                TotalDue = model.Where(x => x.TotalDue > 0).Sum(x => (double?)x.TotalDue) ?? 0,
                TotalAdvance = model.Where(x => x.TotalDue < 0).Sum(x => (double?)x.TotalDue) ?? 0,
                TotalRefund = model.Sum(x => x.TotalRefund)
            };
        }
        public InvoiceHistorySumVM GetTransactionsSum(DateTime DateFrom, DateTime DateTo, long PatientId)
        {
            List<InvoiceHistorySumVM> model = cxt.Invoices
                .Where(i => i.IsActive && i.Patient.PatientId == PatientId && !i.IsHoldInvoice && !i.IsQInvoice)
                .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom) && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)))
            .Select(i => new InvoiceHistorySumVM
            {
                TotalQty = (i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0) - (i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0),
                TotalDue = ((double?)i.Due ?? 0),
                TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0
            }).ToList();
            return new InvoiceHistorySumVM()
            {
                TotalInvoice = model.Count(),
                TotalQty = model.Sum(x => (int?)x.TotalQty) ?? 0,
                TotalPaid = model.Sum(x => (double?)x.TotalPaid) ?? 0,
                TotalDue = model.Where(x => x.TotalDue > 0).Sum(x => (double?)x.TotalDue) ?? 0,
                TotalAdvance = model.Where(x => x.TotalDue < 0).Sum(x => (double?)x.TotalDue) ?? 0,
                TotalRefund = model.Sum(x => x.TotalRefund)
            };
        }
        public InvoiceHistorySumVM GetTransactionsSum(DateTime DateFrom, DateTime DateTo, int UserId)
        {
            List<InvoiceHistorySumVM> model = cxt.Invoices
                .Where(i => i.IsActive && i.User != null && i.User.UserId == UserId && !i.IsHoldInvoice && !i.IsQInvoice)
                .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom) && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo)))
            .Select(i => new InvoiceHistorySumVM
            {
                TotalQty = (i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0) - (i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0),
                TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0
            }).ToList();
            return new InvoiceHistorySumVM()
            {
                TotalInvoice = model.Count(),
                TotalQty = model.Sum(x => (int?)x.TotalQty) ?? 0,
                TotalPaid = model.Sum(x => (double?)x.TotalPaid) ?? 0,
                TotalRefund = model.Sum(x => x.TotalRefund)
            };
        }
        public InvoiceHistorySumVM GetTransactionsSum(long InvoiceRefNo)
        {
            List<InvoiceHistorySumVM> model = cxt.Invoices
            .Where(i => i.IsActive && !i.IsHoldInvoice && i.InvoiceRefNo == InvoiceRefNo && !i.IsQInvoice)
            .Select(i => new InvoiceHistorySumVM
            {
                TotalQty = (i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0) - (i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0),
                TotalDue = ((double?)i.Due ?? 0),
                TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0
            }).ToList();
            return new InvoiceHistorySumVM()
            {
                TotalInvoice = model.Count(),
                TotalQty = model.Sum(x => (int?)x.TotalQty) ?? 0,
                TotalPaid = model.Sum(x => (double?)x.TotalPaid) ?? 0,
                TotalDue = model.Where(x => x.TotalDue > 0).Sum(x => (double?)x.TotalDue) ?? 0,
                TotalAdvance = model.Where(x => x.TotalDue < 0).Sum(x => (double?)x.TotalDue) ?? 0,
                TotalRefund = model.Sum(x => x.TotalRefund)
            };
        }
        public InvoiceHistorySumVM GetTransactionsSum()
        {

            List<InvoiceHistorySumVM> model = cxt.Invoices
            .Where(i => i.IsActive && !i.IsHoldInvoice && !i.IsQInvoice)
            .Select(i => new InvoiceHistorySumVM
            {
                TotalQty = (i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0) - (i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0),
                TotalDue = ((double?)i.Due ?? 0),
                TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0
            }).ToList();
            return new InvoiceHistorySumVM()
            {
                TotalInvoice = model.Count(),
                TotalQty = model.Sum(x => (int?)x.TotalQty) ?? 0,
                TotalPaid = model.Sum(x => (double?)x.TotalPaid) ?? 0,
                TotalDue = model.Where(x => x.TotalDue > 0).Sum(x => (double?)x.TotalDue) ?? 0,
                TotalAdvance = model.Where(x => x.TotalDue < 0).Sum(x => (double?)x.TotalDue) ?? 0,
                TotalRefund = model.Sum(x => x.TotalRefund)
            };

        }
        public InvoiceHistorySumVM GetTransactionsSum(int InvoiceType)
        {

            if (InvoiceType == 0)
            {
                List<InvoiceHistorySumVM> model = cxt.Invoices
                .Where(i => i.IsActive && !i.IsHoldInvoice && !i.IsQInvoice)
                .Select(i => new InvoiceHistorySumVM
                {
                    TotalQty = (i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0) - (i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0),
                    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0),
                    TotalDue = ((double?)i.Due ?? 0),
                    TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0
                }).ToList();
                return new InvoiceHistorySumVM()
                {
                    TotalInvoice = model.Count(),
                    TotalQty = model.Sum(x => (int?)x.TotalQty) ?? 0,
                    TotalPaid = model.Sum(x => (double?)x.TotalPaid) ?? 0,
                    TotalDue = model.Where(x => x.TotalDue > 0).Sum(x => (double?)x.TotalDue) ?? 0,
                    TotalAdvance = model.Where(x => x.TotalDue < 0).Sum(x => (double?)x.TotalDue) ?? 0,
                    TotalRefund = model.Sum(x => x.TotalRefund)
                };
            }
            else if (InvoiceType == 1)
            {
                List<InvoiceHistorySumVM> model = cxt.Invoices
                .Where(i => i.IsActive && !i.IsHoldInvoice && !i.IsQInvoice)
                .Select(i => new InvoiceHistorySumVM
                {
                    TotalQty = (i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0) - (i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0),
                    TotalPaid = (i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0),
                    TotalDue = ((double?)i.Due ?? 0),
                    TotalRefund = (double?)-1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)) ?? 0
                }).ToList();
                return new InvoiceHistorySumVM()
                {
                    TotalInvoice = model.Count(),
                    TotalQty = model.Sum(x => (int?)x.TotalQty) ?? 0,
                    TotalPaid = model.Sum(x => (double?)x.TotalPaid) ?? 0,
                    TotalDue = model.Where(x => x.TotalDue > 0).Sum(x => (double?)x.TotalDue) ?? 0,
                    TotalAdvance = model.Where(x => x.TotalDue < 0).Sum(x => (double?)x.TotalDue) ?? 0,
                    TotalRefund = model.Sum(x => x.TotalRefund)
                };
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region DeleteInvoiceUI
        public IPagedList<InvoiceTransactionsVM> GenericSearchForDeleteInvoices(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => !i.IsActive && i.DeletedAt != null)
            .Where(i => DbFunctions.TruncateTime(i.DeletedAt) >= DbFunctions.TruncateTime(DateFrom)
                && (DbFunctions.TruncateTime(i.DeletedAt) <= DbFunctions.TruncateTime(DateTo))
            )
            .Select(i => new InvoiceTransactionsVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo,
                CreateDate = i.CreateDate,
                TotalAmount = i.TotalAmount,
                TotalDiscount = i.TotalDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                SubTotal = i.SubTotal,
                Due = i.Due,
                SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                User = i.User,
                DeletedAt = i.DeletedAt.Value,
                InvoiceItems = (i.InvoiceItems.Where(ii => !ii.IsActive).Select(item => new InvoiceItemVM
                {
                    ItemName = item.Item.ItemName,
                    //IsActive = item.IsActive,
                    //InvoiceItemId = item.InvoiceItemId,
                    //ItemId = item.Item.ItemID,
                    //Rate = item.Rate,
                    //Quantity = item.Quantity,
                    //Discount = item.Discount,
                    //DiscountType = item.DiscountType,
                    //DiscountTypeString = item.DiscountType == 1 ? "Value" : "%",
                    //Amount = item.Amount,
                    //BatchId = item.Batch.BatchId,
                    //BatchName = item.Batch.BatchName,
                    //ReturnedQuantity = item.ReturnedQuantity
                })).ToList(),
                TotalPaid = (i.InvoicePayments.Where(p => !p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                //PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                //TotalRefund = -1 * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => p.Payment)),
                //InvoicePayments = i.InvoicePayments.Where(p => p.IsActive).ToList(),
                Patient = i.Patient,
                PatientName = i.Patient.Name
            }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public List<DeleteInvoiceVM> GenericSearchForDeleteInvoicesRpt(DateTime DateFrom, DateTime DateTo)
        {
            return cxt.Invoices
            .Where(i => !i.IsActive && i.DeletedAt != null)
            .Where(i => DbFunctions.TruncateTime(i.DeletedAt) >= DbFunctions.TruncateTime(DateFrom)
                && (DbFunctions.TruncateTime(i.DeletedAt) <= DbFunctions.TruncateTime(DateTo))
            )
            .Select(i => new DeleteInvoiceVM
            {
                InvoiceId = i.InvoiceId,
                InvoiceRefNo = i.InvoiceRefNo.ToString(),
                CreateDate = i.CreateDate,
                PatientName = i.Patient != null ? i.Patient.Name : "",
                UserName = i.User != null ? i.User.UserName : "",
                DeletedAt = i.DeletedAt.Value,
                GrandTotal = i.GrandTotal,
                Due = i.Due,
                TotalPaid = (i.InvoicePayments.Where(p => !p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                ItemName = i.InvoiceItems.Where(ii => !ii.IsActive).Select(item => item.Item.ItemName).ToList(),
            }).OrderByDescending(i => i.InvoiceId).ToList();
        }
        #endregion

        #region StaffUI
        public IPagedList<InvoiceTransactionsVM> GenericSearchForStaffUI(DateTime DateFrom, DateTime DateTo, int UserId, int PageNo, int PageSize)
        {
            if (UserId > 0)
            {
                return (from i in cxt.Invoices
                        join p in cxt.invoicePayments on i equals p.Invoice
                        where i.IsActive && p.IsActive && i.User.UserId == UserId
                        && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                        && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                        group i by i.User.UserId into ug
                        select new InvoiceTransactionsVM
                        {
                            UserId = ug.Key,
                            UserName = cxt.Users.Where(u => u.UserId == ug.Key).Select(n => n.UserName).FirstOrDefault(),
                            TotalPaid = ug.Select(x => x.InvoicePayments.Sum(k => k.Payment)).ToList().Sum()
                        }).OrderByDescending(x => x.UserId).ToPagedList(PageNo, PageSize);
            }
            else
            {
                List<int> NotInclude = new List<int>() { 1, 4 };
                return (from i in cxt.Invoices
                        join p in cxt.invoicePayments on i equals p.Invoice
                        where i.IsActive && p.IsActive && i.User.UserRoles.All(r => !NotInclude.Contains(r.UserRoleId))
                        && DbFunctions.TruncateTime(p.PaymentDate) >= DbFunctions.TruncateTime(DateFrom)
                        && DbFunctions.TruncateTime(p.PaymentDate) <= DbFunctions.TruncateTime(DateTo)
                        group i by i.User.UserId into ug
                        select new InvoiceTransactionsVM
                        {
                            UserId = ug.Key,
                            UserName = cxt.Users.Where(u => u.UserId == ug.Key).Select(n => n.UserName).FirstOrDefault(),
                            TotalPaid = ug.Select(x => x.InvoicePayments.Sum(k => k.Payment)).ToList().Sum()
                        }).OrderByDescending(x => x.UserId).ToPagedList(PageNo, PageSize);
            }
        }
        #endregion

        #region StaffDetialsUI
        public IPagedList<InvoiceTransactionsVM> GenericSearchForStaffDetailsUI(DateTime DateFrom, DateTime DateTo, int UserId, int PageNo, int PageSize)
        {
            return cxt.Invoices
           .Where(i => i.IsActive && !i.IsHoldInvoice && i.User.UserId == UserId)
           .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
               && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
           )
           .Select(i => new InvoiceTransactionsVM
           {
               InvoiceId = i.InvoiceId,
               InvoiceRefNo = i.InvoiceRefNo,
               PatientName = i.Patient != null ? i.Patient.Name : "",
               TotalAmount = i.TotalAmount,
               TotalDiscount = i.TotalDiscount,
               DiscountType = i.DiscountType,
               GrandTotal = i.GrandTotal,
               SubTotal = i.SubTotal,
               Due = i.Due,
               SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
               ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
               PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
               TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
               UserName = i.User.UserName
           }).OrderByDescending(i => i.InvoiceId).ToPagedList(PageNo, PageSize);
        }
        public List<InvoiceTransactionsVM> GenericSearchForStaffDetailsUIRpt(DateTime DateFrom, DateTime DateTo, int UserId)
        {
            return cxt.Invoices
           .Where(i => i.IsActive && !i.IsHoldInvoice && i.User.UserId == UserId)
           .Where(i => DbFunctions.TruncateTime(i.CreateDate) >= DbFunctions.TruncateTime(DateFrom)
               && (DbFunctions.TruncateTime(i.CreateDate) <= DbFunctions.TruncateTime(DateTo))
           )
           .Select(i => new InvoiceTransactionsVM
           {
               InvoiceId = i.InvoiceId,
               InvoiceRefNo = i.InvoiceRefNo,
               PatientName = i.Patient != null ? i.Patient.Name : "",
               TotalAmount = i.TotalAmount,
               TotalDiscount = i.TotalDiscount,
               DiscountType = i.DiscountType,
               GrandTotal = i.GrandTotal,
               SubTotal = i.SubTotal,
               Due = i.Due,
               SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
               ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
               PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
               TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
               UserName = i.User.UserName
           }).OrderByDescending(i => i.InvoiceId).ToList();
        }
        #endregion

        #region PatientLedger

        public IPagedList<PatientLedgerVM> GetPatientLedger(long Patientid, int PageNo, int PageSize)
        {
            return cxt.Invoices
            .Where(i => (i.IsActive) && !i.IsHoldInvoice && i.Patient.PatientId == Patientid)
            .Select(i => new PatientLedgerVM
            {

                InvoiceRefNo = i.InvoiceRefNo,
                TotalDiscount = i.ModifiedDiscount,
                DiscountType = i.DiscountType,
                GrandTotal = i.GrandTotal,
                GrossTotal = i.SubTotal,
                Due = (i.Due > 0) ? i.Due : 0,
                SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                RemainingTotal = (i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0),
                Refund = (-1)*(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                Patient = i.Patient,
                PatientName = i.Patient.Name,
                InvoiceItems = i.InvoiceItems.ToList()
            }).OrderBy(i => i.Patient.PatientId).ToPagedList(PageNo, PageSize);
        }
        public List<PatientLedgerVM> GenericSearchForPatientLedgerRpt(long Patientid, DateTime Fromtime, DateTime Totime)
        {
            return (from i in cxt.Invoices
                    join p in cxt.invoicePayments on i equals p.Invoice

                    where ((i.IsActive) && !i.IsHoldInvoice && i.Patient.PatientId == Patientid && p.PaymentDate > Fromtime && p.PaymentDate < Totime)
                    select new PatientLedgerVM
                    {

                        InvoiceRefNo = i.InvoiceRefNo,
                        TotalDiscount = i.ModifiedDiscount,
                        DiscountType = i.DiscountType,
                        GrandTotal = i.GrandTotal,
                        GrossTotal = i.SubTotal,
                        Due = (i.Due > 0) ? i.Due : 0,
                        SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                        ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                        PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                        //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                        TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                        RemainingTotal = (i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0),
                        Refund = (-1)*(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                        Patient = i.Patient,
                        PatientName = i.Patient.Name,
                        InvoiceItemsV = (i.InvoiceItems.Select(inv => new InvoiceItemVM
                        {
                            ItemName = inv.Item.ItemName,
                            Amount = inv.Amount
                        }))
                    }).ToList();
        }
        public List<PatientLedgerVM> GenericSearchForPatientLedgerRpt(long Patientid)
        {
            return (from i in cxt.Invoices

                    where ((i.IsActive) && !i.IsHoldInvoice && i.Patient.PatientId == Patientid)
                    select new PatientLedgerVM
                    {

                        InvoiceRefNo = i.InvoiceRefNo,
                        TotalDiscount = i.ModifiedDiscount,
                        DiscountType = i.DiscountType,
                        GrandTotal = i.GrandTotal,
                        GrossTotal = i.SubTotal,
                        Due = (i.Due > 0) ? i.Due : 0,
                        SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                        ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                        PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                        //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                        TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                        Refund = (-1)*(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                        Patient = i.Patient,
                        PatientName = i.Patient.Name,
                        InvoiceItemsV = (i.InvoiceItems.Select(inv => new InvoiceItemVM
                        {
                            ItemName = inv.Item.ItemName,
                            Amount = inv.Amount
                        }))
                        //InvoiceItems = i.InvoiceItems.ToList() 
                    }).OrderBy(i => i.Patient.PatientId).ToList();
        }
        public IPagedList<PatientLedgerVM> GetPatientLedger(long Patientid, DateTime Fromtime, DateTime Totime, int PageNo, int PageSize)
        {
            return (from i in cxt.Invoices
                    join p in cxt.invoicePayments on i equals p.Invoice

                    where ((i.IsActive) && !i.IsHoldInvoice && i.Patient.PatientId == Patientid && p.PaymentDate > Fromtime && p.PaymentDate < Totime)
                    select new PatientLedgerVM
                    {

                        InvoiceRefNo = i.InvoiceRefNo,
                        TotalDiscount = i.ModifiedDiscount,
                        DiscountType = i.DiscountType,
                        GrandTotal = i.GrandTotal,
                        GrossTotal = i.SubTotal,
                        Due = (i.Due>0)? i.Due:0,
                        SaleQuantity = i.InvoiceItems.Sum(it => (int?)it.Quantity) ?? 0,
                        ReturnedQty = i.InvoiceItems.Sum(it => (int?)it.ReturnedQuantity) ?? 0,
                        PaymentDate = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.PaymentDate).FirstOrDefault()),
                        //RefundDate = (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).OrderByDescending(p => p.InvoicePaymentId).Select(p => p.Date).FirstOrDefault()),
                        TotalPaid = (i.InvoicePayments.Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0),
                        RemainingTotal = (i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0),
                        Refund = (-1)*(cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p => (double?)p.Payment) ?? 0),
                        Patient = i.Patient,
                        PatientName = i.Patient.Name,
                        InvoiceItems = i.InvoiceItems.ToList()
                    }).OrderBy(i => i.Patient.PatientId).ToPagedList(PageNo, PageSize);
        }
        public PatientLedgerSumVM GetPatientLedgerSum(long Patientid, DateTime Fromtime, DateTime Totime)
        {
            List<PatientLedgerSumVM> model = (from i in cxt.Invoices
                                              join p in cxt.invoicePayments on i equals p.Invoice

                                              where ((i.IsActive) && !i.IsHoldInvoice && i.Patient.PatientId == Patientid && p.PaymentDate > Fromtime && p.PaymentDate < Totime)
                                              select new PatientLedgerSumVM
                                              {
                                                  invoiceid = i.InvoiceId,
                                                  GrossTotal = i.SubTotal,
                                                  TotalPaid = (p.IsActive)? p.Payment:0,
                                                  TotalDue = ((double?)i.Due ?? 0),
                                                  TotalRefund = ((p.Payment<0 && p.IsActive)? p.Payment:0)
                                              }).ToList();
            List<double> invidlist = new List<double>();
            List<PatientLedgerSumVM> mod = new List<PatientLedgerSumVM>(); 
            foreach(var ii in model)
            {
                if (invidlist.Contains(ii.invoiceid))
                {
                    continue;
                }
                else
                {
                    invidlist.Add(ii.invoiceid);
                    mod.Add(ii);
                }
            }
            return new PatientLedgerSumVM()
            {
                TotalPaid = model.Sum(x => (double?)x.TotalPaid) ?? 0,
                TotalDue = model.Where(x => x.TotalDue > 0).Sum(x => (double?)x.TotalDue) ?? 0,
                GrossTotal = mod.Sum(x => (double?)x.GrossTotal) ?? 0,
                TotalRefund = (-1)*model.Sum(x => (double?)x.TotalRefund) ?? 0
            };
        }
        //public PatientLedgerSumVM GetPatientLedgerSum(long Patientid, DateTime Fromtime, DateTime Totime)
        //{
        //    List<PatientLedgerSumVM> model = (from i in cxt.Invoices
        //                                      join p in cxt.invoicePayments on i equals p.Invoice
        //                                      where ((i.IsActive) && !i.IsHoldInvoice && i.Patient.PatientId == Patientid && p.Payment < 0 && p.PaymentDate > Fromtime && p.PaymentDate < Totime)
        //                                      group new { i, p } by i.InvoiceId into g
        //                                      select new PatientLedgerSumVM
        //                                      {
        //                                          GrossTotal = g.Sum(x => x.i.SubTotal),
        //                                          TotalPaid = (g.Where(x => x.p.Payment > 0).Sum(x => (double?)x.p.Payment) ?? 0),
        //                                          TotalDue = (g.Sum(x => (double?)x.i.Due) ?? 0),
        //                                          TotalRefund = ((-1) * (g.Where(x => x.p.Payment < 0).Sum(x => (double?)x.p.Payment) ?? 0))
        //                                      }).ToList<PatientLedgerSumVM>();
        //    return new PatientLedgerSumVM()
        //    {
        //        TotalPaid = model.Sum(x => (double?)x.TotalPaid) ?? 0,
        //        TotalDue = model.Where(x => x.TotalDue > 0).Sum(x => (double?)x.TotalDue) ?? 0,
        //        GrossTotal = model.Sum(x => (double?)x.GrossTotal) ?? 0,
        //        TotalRefund = model.Sum(x => (double?)x.TotalRefund) ?? 0
        //    };
        //}

        public PatientLedgerSumVM GetPatientLedgerSum(long Patientid)
        {
            List<PatientLedgerSumVM> model = (from i in cxt.Invoices

                                              where ((i.IsActive) && !i.IsHoldInvoice && i.Patient.PatientId == Patientid)
                                              select new PatientLedgerSumVM
                                              {
                                                  GrossTotal = i.SubTotal,
                                                  TotalPaid = (i.InvoicePayments.Where(p => p.IsActive).Sum(p => (double?)p.Payment) ?? 0),
                                                  TotalDue = ((double?)i.Due ?? 0),
                                                  TotalRefund = ((-1) * (cxt.invoicePayments.Where(p => p.Invoice.InvoiceId == i.InvoiceId && p.IsActive && p.Payment < 0).Sum(p =>(double?) p.Payment)?? 0))
                                              }).ToList();
            return new PatientLedgerSumVM()
            {
                TotalPaid = model.Sum(x => (double?)x.TotalPaid) ?? 0,
                TotalDue = model.Where(x => x.TotalDue > 0).Sum(x => (double?)x.TotalDue) ?? 0,
                GrossTotal = model.Sum(x => (double?)x.GrossTotal) ?? 0,
                TotalRefund = model.Sum(x => (double?)x.TotalRefund) ?? 0
            };
        }


        #endregion

        public int GetPreviousDateItemStock(long ItemId, DateTime fromDate)
        {
            return cxt.StockItems.Where(x => x.Item.ItemID == ItemId && DbFunctions.TruncateTime(x.CreatedAt) < DbFunctions.TruncateTime(fromDate))
                .Sum(i => (int?)i.Quantity) ?? 0;
        }

        public List<NarcoticDrugsItemwiseReportVM> DateTimeAvailableItemStock(long ItemId, DateTime fromDate, DateTime toDate)
        {
            return cxt.StockItems
              .Where(i => i.Item.ItemID == ItemId &&
              DbFunctions.TruncateTime(i.CreatedAt) >= DbFunctions.TruncateTime(fromDate) &&
              DbFunctions.TruncateTime(i.CreatedAt) <= DbFunctions.TruncateTime(toDate))
              .Include(x => x.Batch).Include(x => x.Item).Include(x => x.Stock.User)
              .Select(vm => new NarcoticDrugsItemwiseReportVM
              {
                  ItemId = vm.Item.ItemID,
                  NarcoticDrugName = vm.Item.ItemName,
                  CreateByName = vm.Stock.User.UserName,
                  BatchName = vm.Batch.BatchName,
                  SupplierName = "",
                  InvoiceId = "",
                  ManufacturerName = "",
                  BatchExpiry = vm.Batch.Expiry.HasValue == true ?
                  SqlFunctions.DateName("day", vm.Batch.Expiry.Value) + "-"
                  + DbFunctions.Right("00" + vm.Batch.Expiry.Value.Month, 2) + "-"
                  + SqlFunctions.DateName("year", vm.Batch.Expiry.Value) :
                  "",
                  InQty = vm.Quantity,
                  CreatedOn = vm.CreatedAt,
                  OutQty = 0,
                  Description = "Add Stock"
              }).ToList();
        }


        public bool IsStoreItem(long InvoiceId)
        {
            return cxt.Invoices.Where(x => x.InvoiceId == InvoiceId).Any(s => s.Store != null);
        }
        public List<InvoiceItem> IsInvoiceHasReturn(long InvoiceId)
        {
            return cxt.invoiceItems.Where(x => x.Invoice.InvoiceId == InvoiceId && x.IsActive && x.ReturnedQuantity != 0).ToList();
        }

        public bool IsMultipleConsumptionItems(long InvoiceId)
        {
            StockConsumption stockConsumption = cxt.StockConsumptions.Where(x => x.InvoiceId == InvoiceId).Include(x => x.StockConsumptions).Include(x => x.StockConsumptions.Select(i => i.Item))
            .Include(x => x.StockConsumptions.Select(i => i.Batch)).FirstOrDefault();
            if (stockConsumption != null)
            {
                return stockConsumption.StockConsumptions.GroupBy(x => x.Item.ItemID).Any(c => c.Count() > 1);
            }
            return false;
        }
    }
}