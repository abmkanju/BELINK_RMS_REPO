using Pharmacy.Core;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
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
using Pharmacy.Core.Entities;
using Pharmacy.Core.Interfaces;
using Pharmacy.Infrastructure;
using Pharmacy.Infrastructure.Repository;

namespace Pharmacy.UI.MenuBar
{
    public partial class DlgAddStockFilter : Form
    {
        UnitOfWork unitOfWork;
        public DateTime? DateFrom;
        public DateTime? DateTo;
        List<StockAddFilterVM> LoadedItems = new List<StockAddFilterVM>();
        public List<StockAddFilterVM> SelectedItems = new List<StockAddFilterVM>();

        public DlgAddStockFilter()
        {
            InitializeComponent();
            LoadSupplier();
        }

        private void LoadSupplier()
        {
            try
            {
                using (UnitOfWork unitOfWork = new Infrastructure.UnitOfWork())
                {
                    List<Supplier> suppliers = unitOfWork.SupplierRepository.GetAll().ToList();
                    Supplier Select = new Supplier();
                    Select.Name = "Select";
                    Select.SupplierID = 0;
                    suppliers.Insert(0, Select);
                    cmbSuppliers.DataSource = suppliers;
                    cmbSuppliers.ValueMember = "SupplierId";
                    cmbSuppliers.DisplayMember = "Name";
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            grdItems.Rows.Clear();
            if (dtpFrom.Value > DateTime.Now
                || dtpTo.Value > DateTime.Now)
            {
                MessageBox.Show("Date seletion can't be greater than today.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpFrom.Value > dtpTo.Value)
            {
                MessageBox.Show("Date from filter can't be greater than Date to filter.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!dtpFrom.Checked || !dtpTo.Checked)
            {
                MessageBox.Show("You must select both dates to apply this filter", "Invalid Date Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            using (unitOfWork = new UnitOfWork())
            {
                if (cmbSuppliers.SelectedIndex > 0)
                {
                    LoadedItems = unitOfWork.InvoiceRepository.LoadPreviouslySoldItems(Convert.ToInt64(cmbSuppliers.SelectedValue), dtpFrom.Value, dtpTo.Value);
                }
                else
                {
                    LoadedItems = unitOfWork.InvoiceRepository.LoadPreviouslySoldItems(dtpFrom.Value, dtpTo.Value);
                }
            }
            foreach (var i in LoadedItems)
            {
                SharedFunctions.CalculateStockFilterQty1(i);
                grdItems.Rows.Add(i.ItemId, i.ItemName, i.AvailableStock, i.ConsumedStock, false);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            dtpFrom.Checked = false;
            dtpTo.Checked = false;
        }


        private void DlgAddStockFilter_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnApply, btnCancel, btnCancelAll, btnLoad });
            SharedFunctions.SetGridStyle(grdItems);
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            bool chkd = chkAll.Checked;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                r.Cells["colLoad"].Value = chkd;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            //if (chkAll.Checked)
            //{
            //    SelectedItems = LoadedItems;
            //}
            //else
            //{
            int itemId, Qty = 0;//, AvQty = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                bool chkd = Convert.ToBoolean(r.Cells["colLoad"].Value);
                if (chkd)
                {
                    itemId = 0; Qty = 0;// AvQty = 0; itemName = "";
                    itemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                    var item = LoadedItems.Where(i => i.ItemId == itemId).FirstOrDefault();
                    if (item != null)
                    {
                        Qty = Convert.ToInt32(r.Cells["colQuantity"].Value);
                        item.Quantity = Qty;
                        SelectedItems.Add(item);
                    }
                }
            }
            //}
            this.Close();
        }
    }
}