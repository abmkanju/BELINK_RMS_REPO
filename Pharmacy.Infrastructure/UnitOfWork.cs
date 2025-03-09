using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core.Interfaces;
using System.Runtime.CompilerServices;

namespace Pharmacy.Infrastructure
{
    public class UnitOfWork : IDisposable
    {
        private bool disposed = false;
        PharmacyContext cxt = new PharmacyContext();
        //PharmacyContext cxt;

        //public UnitOfWork()
        //{

        //}

        private SalesReturnRepository _SalesReturnRepository;
        private ItemsRepository _ItemRspository;
        private IItemRepository _IItemRspository;
        private OnlineIntegrateRepository _OnlineIntegrateRepository;
        private VoucherRepository _VoucherRepository;
        private SupplierRepository _SupplierRepository;
        private PurchaseOrderRepository _PurchaseOrderRepository;
        private PurchaseOrderItemsRepository _PurchaseOrderItemsRepository;
        private BatchRepository _BatchRepository;
        private StoreRepository _StoreRepository;
        private StockRepository _StockRepository;
        private PatientRepository _PatientRepository;
        private TagsRepository _TagsRepository;
        private InvoiceRepository _InvoiceRepository;
        private ChequeInfoRepository _ChequeInfoRepository;
        private ChargeInfoRepository _ChargeInfoRepository;
        private StockConsumptionRepository _StockConsumptionRepository;
        private AdjustmentRepository _AdjustmentRepository;
        private StockReturnRepository _StockReturnRepository;
        private UserRepository _UserRepository;
        private OfflineTaskRepository _OfflineTaskRepository;
        private UserRoleRepository _UserRoleRepository;
        private RightRepository _RightRepository;
        private GeneralRepository _GeneralRepository;
        private ProcedureRepository _ProcedureRepository;
        private AdminInvoiceSettingRepository _AdminInvoiceSettingRepository;
        private AdminProcedureInvoiceSettingRepository _AdminProcedureInvoiceSettingRepository;
        private AdminPractiseSettingRepository _AdminPractiseSettingRepository;
        private AdminPrintFormatSettingRepository _AdminPrintFormatSettingRepository;
        private DepratmentRepository _DepratmentRepository;
        private SubDepartmentRepository _SubDepartmentRepository;
        private LowStockMailRepository _LowStockMailRepository;
        private WardRepository _WardRepository;
        private BedRepository _BedRepository;
        private AccountExpenseCategoryRepository _AccountExpenseCategoryRepository;
        private AccountExpenseRepository _AccountExpenseRepository;
        private AdmitPatientsRepository _AdmitPatientsRepository;
        private VitalRepository _VitalRepository;
        private CategoryRepository _CategoryRepository;
        private ExpenseRepository _ExpenseRepository;
        private ManufacturerRepository _ManufacturerRepository;
        private HwDataRepository _HwDataRepository;
        private TemplateRepository _TemplateRepository;
        private StockAuditRepository _StockAuditRepository;
        private RackRepository _RackRepository;
        private AdminPharmacySettingRepository _AdminPharmacySettingRepository;
        private FlatDiscountRepository _FlatDiscountRepository;
        private MissedsalesRepository _MissedsalesRepository;
        private AdminShiftMasterSettingRepository _AdminShiftMasterSettingRepository;
        private AdminShiftsSettingRepository _AdminShiftsSettingRepository;
        private POSCashReceiveRepository _POSCashReceiveRepository;
        private POSCashSkimmedRepository _POSCashSkimmedRepository;
        private LoginHistoryRepository _LoginHistoryRepository;
        private POSClosingRepository _POSClosingRepository;
        private AccTransactionsRepository _AccTransactionsRepository;
        private StoreClosingRepository _StoreClosingRepository;
        private AuditLogsRepository _AuditLogRepository;
        private AdminFBRSettingRepository _AdminFBRSettingRepository;
        private AdminPatientSettings _AdminPatientSettings;
        private ProfitStatementTransactionsRepository _ProfitStatementTransactions;
        private HwFileDataRepository _HwFileDataRepository;
        private InvoiceSettingRepository _invoiceSettingRepository;
        public ItemsRepository ItemRspository
        {
            get
            {
                if (_ItemRspository == null)
                {
                    _ItemRspository = new ItemsRepository(cxt);
                }
                return _ItemRspository;
            }
        }
        public VoucherRepository VoucherRepository
        {
            get
            {
                if (_VoucherRepository == null)
                {
                    _VoucherRepository = new VoucherRepository(cxt);
                }
                return _VoucherRepository;
            }
        }
        public OnlineIntegrateRepository OnlineIntegrateRepository
        {
            get
            {
                if(_OnlineIntegrateRepository == null)
                {
                    _OnlineIntegrateRepository = new OnlineIntegrateRepository(cxt);
                }
                return _OnlineIntegrateRepository;
            }
        }
        public InvoiceSettingRepository InvoiceSettingRepository
        {
            get
            {
                if (_invoiceSettingRepository == null)
                {
                    _invoiceSettingRepository = new InvoiceSettingRepository(cxt);
                }
                return _invoiceSettingRepository;
            }
        }
        public HwFileDataRepository HwFileDataRepository
        {
            get
            {
                if (_HwFileDataRepository == null)
                {
                    _HwFileDataRepository = new HwFileDataRepository(cxt);
                }
                return _HwFileDataRepository;
            }
        }

        public IItemRepository IItemRspository
        {
            get
            {
                if (_IItemRspository == null)
                {
                    _IItemRspository = new ItemsRepository(cxt);
                }
                return _IItemRspository;
            }
        }
        public SupplierRepository SupplierRepository
        {
            get
            {
                if (_SupplierRepository == null)
                {
                    _SupplierRepository = new SupplierRepository(cxt);
                }
                return _SupplierRepository;
            }
        }
        public ProfitStatementTransactionsRepository ProfitStatementTransactionsRepository
        {
            get
            {
                if (_ProfitStatementTransactions == null)
                {
                    _ProfitStatementTransactions = new ProfitStatementTransactionsRepository(cxt);
                }
                return _ProfitStatementTransactions;
            }
        }
        public AdminPatientSettings AdminPatientSettings
        {
            get
            {
                if (_AdminPatientSettings == null)
                {
                    _AdminPatientSettings = new AdminPatientSettings(cxt);
                }
                return _AdminPatientSettings;
            }
        }
        public PurchaseOrderRepository PurchaseOrderRepository
        {
            get
            {
                if (_PurchaseOrderRepository == null)
                {
                    _PurchaseOrderRepository = new PurchaseOrderRepository(cxt);
                }
                return _PurchaseOrderRepository;
            }
        }
        public PurchaseOrderItemsRepository PurchaseOrderItemsRepository
        {
            get
            {
                if (_PurchaseOrderItemsRepository == null)
                {
                    _PurchaseOrderItemsRepository = new PurchaseOrderItemsRepository(cxt);
                }
                return _PurchaseOrderItemsRepository;
            }
        }
        public BatchRepository BatchRepository
        {
            get
            {
                if (_BatchRepository == null)
                {
                    _BatchRepository = new BatchRepository(cxt);
                }
                return _BatchRepository;
            }
        }

        public StoreRepository StoreRepository
        {
            get
            {
                if (_StoreRepository == null)
                {
                    _StoreRepository = new StoreRepository(cxt);
                }
                return _StoreRepository;
            }
        }

        public StockRepository StockRepository
        {
            get
            {
                if (_StockRepository == null)
                {
                    _StockRepository = new StockRepository(cxt);
                }
                return _StockRepository;
            }
        }
        public PatientRepository PatientRepository
        {
            get
            {
                if (_PatientRepository == null)
                {
                    _PatientRepository = new PatientRepository(cxt);
                }
                return _PatientRepository;
            }
        }
        public TagsRepository TagsRepository
        {
            get
            {
                if (_TagsRepository == null)
                {
                    _TagsRepository = new TagsRepository(cxt);
                }
                return _TagsRepository;
            }
        }

        public InvoiceRepository InvoiceRepository
        {
            get
            {
                if (_InvoiceRepository == null)
                {
                    _InvoiceRepository = new InvoiceRepository(cxt);
                }
                return _InvoiceRepository;
            }
        }
        public SalesReturnRepository SalesReturnRepository
        {
            get
            {
                if (_SalesReturnRepository == null)
                {
                    _SalesReturnRepository = new SalesReturnRepository(cxt);
                }
                return _SalesReturnRepository;
            }
        }
        public ChequeInfoRepository ChequeInfoRepository
        {
            get
            {
                if (_ChequeInfoRepository == null)
                {
                    _ChequeInfoRepository = new ChequeInfoRepository(cxt);
                }
                return _ChequeInfoRepository;
            }
        }
        public ChargeInfoRepository ChargeInfoRepository
        {
            get
            {
                if (_ChargeInfoRepository == null)
                {
                    _ChargeInfoRepository = new ChargeInfoRepository(cxt);
                }
                return _ChargeInfoRepository;
            }
        }
        public AdjustmentRepository AdjustmentRepository
        {
            get
            {
                if (_AdjustmentRepository == null)
                {
                    _AdjustmentRepository = new AdjustmentRepository(cxt);
                }
                return _AdjustmentRepository;
            }
        }

        public StockReturnRepository StockReturnRepository
        {
            get
            {
                if (_StockReturnRepository == null)
                {
                    _StockReturnRepository = new StockReturnRepository(cxt);
                }
                return _StockReturnRepository;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                if (_UserRepository == null)
                {
                    _UserRepository = new UserRepository(cxt);
                }
                return _UserRepository;
            }
        }

        public OfflineTaskRepository OfflineTaskRepository
        {
            get
            {
                if (_OfflineTaskRepository == null)
                {
                    _OfflineTaskRepository = new OfflineTaskRepository(cxt);
                }
                return _OfflineTaskRepository;
            }
        }

        public UserRoleRepository UserRoleRepository
        {
            get
            {
                if (_UserRoleRepository == null)
                {
                    _UserRoleRepository = new UserRoleRepository(cxt);
                }
                return _UserRoleRepository;
            }
        }
        public RightRepository RightRepository
        {
            get
            {
                if (_RightRepository == null)
                {
                    _RightRepository = new RightRepository(cxt);
                }
                return _RightRepository;
            }
        }

        public GeneralRepository GeneralRepository
        {
            get
            {
                if (_GeneralRepository == null)
                {
                    _GeneralRepository = new GeneralRepository(cxt);
                }
                return _GeneralRepository;
            }
        }


        public ProcedureRepository ProcedureRepository
        {
            get
            {
                if (_ProcedureRepository == null)
                {
                    _ProcedureRepository = new ProcedureRepository(cxt);
                }
                return _ProcedureRepository;
            }
        }


        public AdminInvoiceSettingRepository AdminInvoiceSettingRepository
        {
            get
            {
                if (_AdminInvoiceSettingRepository == null)
                {
                    _AdminInvoiceSettingRepository = new AdminInvoiceSettingRepository(cxt);
                }
                return _AdminInvoiceSettingRepository;
            }
        }


        public StockConsumptionRepository StockConsumptionRepository
        {
            get
            {
                if (_StockConsumptionRepository == null)
                {
                    _StockConsumptionRepository = new StockConsumptionRepository(cxt);
                }
                return _StockConsumptionRepository;
            }
        }

        public AdminPractiseSettingRepository AdminPractiseSettingRepository
        {
            get
            {
                if (_AdminPractiseSettingRepository == null)
                {
                    _AdminPractiseSettingRepository = new AdminPractiseSettingRepository(cxt);
                }
                return _AdminPractiseSettingRepository;
            }
        }
        public AdminProcedureInvoiceSettingRepository AdminProcedureInvoiceSettingRepository
        {
            get
            {
                if (_AdminProcedureInvoiceSettingRepository == null)
                {
                    _AdminProcedureInvoiceSettingRepository = new AdminProcedureInvoiceSettingRepository(cxt);
                }
                return _AdminProcedureInvoiceSettingRepository;
            }
        }

        public AdminPrintFormatSettingRepository AdminPrintFormatSettingRepository
        {
            get
            {
                if (_AdminPrintFormatSettingRepository == null)
                {
                    _AdminPrintFormatSettingRepository = new AdminPrintFormatSettingRepository(cxt);
                }
                return _AdminPrintFormatSettingRepository;
            }
        }



        public DepratmentRepository DepratmentRepository
        {
            get
            {
                if (_DepratmentRepository == null)
                {
                    _DepratmentRepository = new DepratmentRepository(cxt);
                }
                return _DepratmentRepository;
            }
        }

        public SubDepartmentRepository SubDepartmentRepository
        {
            get
            {
                if (_SubDepartmentRepository == null)
                {
                    _SubDepartmentRepository = new SubDepartmentRepository(cxt);
                }
                return _SubDepartmentRepository;
            }
        }

        public LowStockMailRepository LowStockMailRepository
        {
            get
            {
                if (_LowStockMailRepository == null)
                {
                    _LowStockMailRepository = new LowStockMailRepository(cxt);
                }
                return _LowStockMailRepository;
            }
        }

        public WardRepository WardRepository
        {
            get
            {
                if (_WardRepository == null)
                {
                    _WardRepository = new WardRepository(cxt);
                }
                return _WardRepository;
            }
        }

        public BedRepository BedRepository
        {
            get
            {
                if (_BedRepository == null)
                {
                    _BedRepository = new BedRepository(cxt);
                }
                return _BedRepository;
            }
        }

        public AccountExpenseCategoryRepository AccountExpenseCategoryRepository
        {
            get
            {
                if (_AccountExpenseCategoryRepository == null)
                {
                    _AccountExpenseCategoryRepository = new AccountExpenseCategoryRepository(cxt);
                }
                return _AccountExpenseCategoryRepository;
            }
        }

        public AccountExpenseRepository AccountExpenseRepository
        {
            get
            {
                if (_AccountExpenseRepository == null)
                {
                    _AccountExpenseRepository = new AccountExpenseRepository(cxt);
                }
                return _AccountExpenseRepository;
            }
        }

        public AdmitPatientsRepository AdmitPatientsRepository
        {
            get
            {
                if (_AdmitPatientsRepository == null)
                {
                    _AdmitPatientsRepository = new AdmitPatientsRepository(cxt);
                }
                return _AdmitPatientsRepository;
            }
        }


        public VitalRepository VitalRepository
        {
            get
            {
                if (_VitalRepository == null)
                {
                    _VitalRepository = new VitalRepository(cxt);
                }
                return _VitalRepository;
            }
        }

        public CategoryRepository CategoryRepository
        {
            get
            {
                if (_CategoryRepository == null)
                {
                    _CategoryRepository = new CategoryRepository(cxt);
                }
                return _CategoryRepository;
            }
        }

        public ExpenseRepository ExpenseRepository
        {
            get
            {
                if (_ExpenseRepository == null)
                {
                    _ExpenseRepository = new ExpenseRepository(cxt);
                }
                return _ExpenseRepository;
            }
        }

        public ManufacturerRepository ManufacturerRepository
        {
            get
            {
                if (_ManufacturerRepository == null)
                {
                    _ManufacturerRepository = new ManufacturerRepository(cxt);
                }
                return _ManufacturerRepository;
            }
        }


        public HwDataRepository HwDataRepository
        {
            get
            {
                if (_HwDataRepository == null)
                {
                    _HwDataRepository = new HwDataRepository(cxt);
                }
                return _HwDataRepository;
            }
        }

        public TemplateRepository TemplateRepository
        {
            get
            {
                if (_TemplateRepository == null)
                {
                    _TemplateRepository = new TemplateRepository(cxt);
                }
                return _TemplateRepository;
            }
        }

        public StockAuditRepository StockAuditRepository
        {
            get
            {
                if (_StockAuditRepository == null)
                {
                    _StockAuditRepository = new StockAuditRepository(cxt);
                }
                return _StockAuditRepository;
            }
        }

        public RackRepository RackRepository
        {
            get
            {
                if (_RackRepository == null)
                {
                    _RackRepository = new RackRepository(cxt);
                }
                return _RackRepository;
            }
        }


        public AdminPharmacySettingRepository AdminPharmacySettingRepository
        {
            get
            {
                if (_AdminPharmacySettingRepository == null)
                {
                    _AdminPharmacySettingRepository = new AdminPharmacySettingRepository(cxt);
                }
                return _AdminPharmacySettingRepository;
            }
        }
        public void SetItemSyncable(int ItemId)
        {
            var r = cxt.Items.Where(i => i.ItemID == ItemId).FirstOrDefault();
            if (r != null)
            {
                r.IsSyncable = true;
            }
        }

        public FlatDiscountRepository FlatDiscountRepository
        {
            get
            {
                if (_FlatDiscountRepository == null)
                {
                    _FlatDiscountRepository = new FlatDiscountRepository(cxt);
                }
                return _FlatDiscountRepository;
            }
        }

        public MissedsalesRepository MissedsalesRepository
        {
            get
            {
                if (_MissedsalesRepository == null)
                {
                    _MissedsalesRepository = new MissedsalesRepository(cxt);
                }
                return _MissedsalesRepository;
            }
        }


        public AdminShiftMasterSettingRepository AdminShiftMasterSettingRepository
        {
            get
            {
                if (_AdminShiftMasterSettingRepository == null)
                {
                    _AdminShiftMasterSettingRepository = new AdminShiftMasterSettingRepository(cxt);
                }
                return _AdminShiftMasterSettingRepository;
            }
        }


        public AdminShiftsSettingRepository AdminShiftsSettingRepository
        {
            get
            {
                if (_AdminShiftsSettingRepository == null)
                {
                    _AdminShiftsSettingRepository = new AdminShiftsSettingRepository(cxt);
                }
                return _AdminShiftsSettingRepository;
            }
        }


        public POSCashReceiveRepository POSCashReceiveRepository
        {
            get
            {
                if (_POSCashReceiveRepository == null)
                {
                    _POSCashReceiveRepository = new POSCashReceiveRepository(cxt);
                }
                return _POSCashReceiveRepository;
            }
        }


        public LoginHistoryRepository LoginHistoryRepository
        {
            get
            {
                if (_LoginHistoryRepository == null)
                {
                    _LoginHistoryRepository = new LoginHistoryRepository(cxt);
                }
                return _LoginHistoryRepository;
            }
        }

        public POSCashSkimmedRepository POSCashSkimmedRepository
        {
            get
            {
                if (_POSCashSkimmedRepository == null)
                {
                    _POSCashSkimmedRepository = new POSCashSkimmedRepository(cxt);
                }
                return _POSCashSkimmedRepository;
            }
        }


        public POSClosingRepository POSClosingRepository
        {
            get
            {
                if (_POSClosingRepository == null)
                {
                    _POSClosingRepository = new POSClosingRepository(cxt);
                }
                return _POSClosingRepository;
            }
        }


        public AccTransactionsRepository AccTransactionsRepository
        {
            get
            {
                if (_AccTransactionsRepository == null)
                {
                    _AccTransactionsRepository = new AccTransactionsRepository(cxt);
                }
                return _AccTransactionsRepository;
            }
        }


        public StoreClosingRepository StoreClosingRepository
        {
            get
            {
                if (_StoreClosingRepository == null)
                {
                    _StoreClosingRepository = new StoreClosingRepository(cxt);
                }
                return _StoreClosingRepository;
            }
        }


        public AuditLogsRepository AuditLogsRepository
        {
            get
            {
                if (_AuditLogRepository == null)
                {
                    _AuditLogRepository = new AuditLogsRepository(cxt);
                }
                return _AuditLogRepository;
            }
        }


        public AdminFBRSettingRepository AdminFBRSettingRepository
        {
            get
            {
                if (_AdminFBRSettingRepository == null)
                {
                    _AdminFBRSettingRepository = new AdminFBRSettingRepository(cxt);
                }
                return _AdminFBRSettingRepository;
            }
        }




        public void Save()
        {
            cxt.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    cxt.Dispose();
                }
            }
            this.disposed = true;
        }

        public PharmacyContext GetDbContext()
        {
            return this.cxt;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
