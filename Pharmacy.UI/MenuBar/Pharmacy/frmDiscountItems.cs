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

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmDiscountItems : Form
    {
        private UnitOfWork unitOfWork;
        public List<int> DiscountedItemsList{ get; set; }
        public List<int> DeletedItemsList { get; set; }
        public double Discount { get; set; }
        public int DiscountType { get; set; } // 0 : %, 1:value
        public frmDiscountItems()
        {
            InitializeComponent();
        }
        private void frmDiscountItems_Load(object sender, EventArgs e)
        {        
        }

        private void btnSearchItems_Click(object sender, EventArgs e)
        {
            frmSearchItem f = new frmSearchItem();
            f.OnItemSelected += frmSearchItem_OnItemSelected;
            f.ShowDialog();
        }

        private void frmSearchItem_OnItemSelected(int ItemId, int Quantity)
        {
            try
            {
                ItemsDataVM d = this.getItemData(ItemId);
                double discountedVal = 0;
                if (this.DiscountType == 0) // percent
                {
                    discountedVal = d.RetailPrice - (this.Discount / 100) * d.RetailPrice;

                }
                else
                {
                    discountedVal = d.RetailPrice - this.Discount;
                }
                grdItems.Rows.Add(0, ItemId, d.ItemName, d.RetailPrice, Discount, this.DiscountType == 0 ? "%" : "Value", discountedVal);
            }
            catch (Exception ex)
            {
                MessageBox.Show("an error occurred while loading item, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private ItemsDataVM getItemData(int itemId)
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    return unitOfWork.ItemRspository.getItemData(itemId);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
                {
                    int DiscItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colDiscountItemId"].Value);
                    if(DiscItemId > 0)
                    {
                        DeletedItemsList.Add(DiscItemId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Operation failed, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddItems_Click(object sender, EventArgs e)
        {
            try
            {
                DiscountedItemsList = new List<int>();
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    DiscountedItemsList.Add(Convert.ToInt32(r.Cells["colItemId"].Value));
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding items to the discount please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}