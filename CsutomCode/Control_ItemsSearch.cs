using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core;
using Pharmacy.Infrastructure;
using PagedList;
using Pharmacy.Core.Entities;

namespace CsutomCode
{
    public partial class Control_ItemsSearch : UserControl
    {
        public UnitOfWork UnitOfWork;
        public int PageNo { get; set; }
        public int SelectedItemId { get; set; }

        System.Timers.Timer ItemsSearchTimer = new System.Timers.Timer();
        List<Item> ItemsGlobal = new List<Item>();
        public delegate void GetSelectedItem(int ItemId);
        public GetSelectedItem  _delegate;
        public Control_ItemsSearch()
        {
            InitializeComponent();
            ItemsSearchTimer.Enabled = true;
            ItemsSearchTimer.Interval = SharedVariables.AsyncDataLoadDelay;
            ItemsSearchTimer.AutoReset = true;
            ItemsSearchTimer.Elapsed += new System.Timers.ElapsedEventHandler(ItemsSearchTimer_Elapsed);
            ItemsSearchTimer.Stop();
        }     

        private void Control_SearchItems_Load(object sender, EventArgs e)
        {

        }

        private void SearchItems()
        {
            string s = txtSearchItems.Text;
            if (s == "")
            {
                txtSearchItems.Text = "";
                this.SelectedItemId = 0;
                ItemsSearchTimer.Stop();
                grdSearchItems.Visible = false;
                return;
            }
            //if (s != "")
            //{
            IPagedList<Item> ItemsLocal = null;
            using (UnitOfWork = new UnitOfWork())
            {
                ItemsLocal = UnitOfWork.ItemRspository.GetMatchingItems(s.ToLower(), PageNo, 20);
            }
            if (ItemsLocal.Count <= 0) ItemsSearchTimer.Stop();
            ItemsGlobal.AddRange(ItemsLocal);
            foreach (Item i in ItemsLocal)
            {
                this.Invoke(new Action(() =>
                {
                    grdSearchItems.Rows.Add(i.ItemID, i.ItemName);
                }));
            }
            if (ItemsGlobal != null && ItemsGlobal.Count > 0)
            {
                this.Invoke(new Action(() =>
                {
                    //grdSearchItems.Rows[0].Selected = false;
                    grdSearchItems.Visible = true;
                }));
            }
            //else
            //{
            //    this.Invoke(new Action(() =>
            //    {
            //        grdSearchItems.Visible = false;
            //    }));
            //}
        }
        private void LoadSearchedItem()
        {
            ItemsSearchTimer.Stop();
            this.SelectedItemId = 0;
            if (grdSearchItems.SelectedRows.Count > 0)
            {
                txtSearchItems.TextChanged -= txtSearchItems_TextChanged;
                this.SelectedItemId = Convert.ToInt32(grdSearchItems.SelectedRows[0].Cells["colSItemId"].Value);
                txtSearchItems.Text = grdSearchItems.SelectedRows[0].Cells["colSItemName"].Value.ToString();
                grdSearchItems.Visible = false;
                txtSearchItems.TextChanged += txtSearchItems_TextChanged;
                this._delegate(this.SelectedItemId);
                //LoadItemBatches(this.SelectedItemId);
                //btnAddItem.PerformClick();
            }
        }
        private void ItemsSearchTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            PageNo += 1;
            SearchItems();
        }
        private void txtSearchItems_Enter(object sender, EventArgs e)
        {
            grdSearchItems.Visible = false;
            grdSearchItems.BringToFront();
        }
        private void txtSearchItems_Leave(object sender, EventArgs e)
        {
            if (grdSearchItems.Rows.Count > 0)
            {
                grdSearchItems.Rows[0].Selected = true;
            }
            //grdSearchItems.Visible = false;
        }
        private void txtSearchItems_KeyDown(object sender, KeyEventArgs e)
        {
            //if(e.KeyCode == Keys.Enter)
            //{
            //    btnAddItem.PerformClick();
            //}
            //else
            if (e.KeyCode == Keys.Down)
            {
                ShiftCtrlToItemSearchGrid();
            }
        }
        private void txtSearchItems_TextChanged(object sender, EventArgs e)
        {
            ItemsSearchTimer.Stop();
            PageNo = 1;
            ItemsGlobal = new List<Item>();
            //grdSearchItems.Location = new Point(txtSearchItems.Location.X, txtSearchItems.Location.Y + txtSearchItems.Height + 4);
            grdSearchItems.BringToFront();
            grdSearchItems.Rows.Clear();
            //if (txtSearchByPhone.Text != "") { txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged; txtSearchByPhone.Text = ""; txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged; }            
            ItemsSearchTimer.Start();
            SearchItems();
        }
        private void grdSearchItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            LoadSearchedItem();
        }
        private void grdSearchItems_KeyPress(object sender, KeyPressEventArgs e)
        {
            // CHECKING FOR ALPHABET 
            TextBox TargetText = new TextBox();
            TargetText = txtSearchItems;
            if ((e.KeyChar >= 65 && e.KeyChar <= 90)
                || (e.KeyChar >= 97 && e.KeyChar <= 122) || e.KeyChar >= 48 && e.KeyChar <= 57)
            {
                TargetText.Focus();
                TargetText.Text += (char)e.KeyChar;
                TargetText.SelectionStart = TargetText.Text.Length;
            }

            if (e.KeyChar == (char)Keys.Back)
            {
                TargetText.Focus();
                TargetText.SelectionStart = TargetText.Text.Length;
                //txtSearchByName.Text = txtSearchByName.Text.Substring(txtSearchByName.Text.Length, 1);
                //txtSearchByName.SelectionStart = 0;
                //txtSearchByName.SelectionLength = 1;
            }
        }
        private void grdSearchItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadSearchedItem();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                grdSearchItems.Visible = false;
            }
        }
        private void ShiftCtrlToItemSearchGrid()
        {
            if (grdSearchItems.Rows.Count > 0)
            {
                this.Size = new System.Drawing.Size(396, 181);
                grdSearchItems.BringToFront();
                grdSearchItems.Focus();
                if (grdSearchItems.Rows.Count > 1)
                {
                    grdSearchItems.Rows[0].Selected = false;
                    grdSearchItems.Rows[1].Selected = true;
                }
                else
                {
                    grdSearchItems.Rows[0].Selected = true;
                }
            }
        }
    }
}
