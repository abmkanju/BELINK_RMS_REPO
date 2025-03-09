using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;
using Pharmacy.Core.ViewModels;

namespace Pharmacy.UI.Reports
{
    public partial class StocksViewer : Form
    {
        UnitOfWork unitOfWork;
        private DateTime DateFrom;
        private DateTime DateTo;
        List<StocksReportVM> StocksList;
        public StocksViewer()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }

        public StocksViewer(DateTime DateFrom, DateTime DateTo, List<StocksReportVM> StocksList)
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.StocksList = StocksList;
        }

        private void StocksViewer_Load(object sender, EventArgs e)
        {
            //List<Stock> stocks = unitOfWork.StockRepository.GetActiveStocksParentOnly();
            //foreach (var s in stocks)
            //{
            //    s.SupplierName = s.Supplier == null ? "N/A" : s.Supplier.Name;
            //}
            //List<StockItemVM> StockItems = unitOfWork.StockRepository.GetActiveStockItems();
            //foreach (var i in StockItems)
            //{
            //    i.StockId = i.StockId;
            //    i.ItemName = i.ItemName;
            //}
            ///List<StocksReportVM> stocks = unitOfWork.StockRepository.GetStocks_Print(this.DateFrom, this.DateTo);
           // List<StockItemVM> StockItems = new List<StockItemVM>();

            StocksRpt r = new StocksRpt();
            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
            {
                r.Database.Tables[0].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
            }
            r.Database.Tables[1].SetDataSource(this.StocksList);
            r.Database.Tables[2].SetDataSource(this.StocksList);
            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
            r.SetParameterValue("FromDate", this.DateFrom);
            r.SetParameterValue("ToDate", this.DateTo);
            
            crystalReportViewer1.ReportSource = r;
            crystalReportViewer1.Show();
        }
    }
}