using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Core.Entities;
using Pharmacy.Core;
using PagedList;
using Pharmacy.Infrastructure;
using System.Data.SqlClient;

namespace Pharmacy.UI
{
    public partial class UC_SearchRackItems : UserControl
    {
        public UnitOfWork UnitOfWork;
        private bool isSearchShown = false;
        public int PageNo { get; set; }
        public int SelectedItemId { get; set; }
        public int StoreId { get; set; }
        public string SelectedItemName { get; set; }
        public string SetText
        {
            //get { return this.txtSearchItems.Text; }
            set { this.txtSearchItems.Text = value; }
        }
        /// <summary>
        /// Thsi function sets focus to search field in this control.
        /// </summary>
        public void SetFocus()
        {
            txtSearchItems.Focus();
        }
        public delegate void ItemSelectionChanged();
        public delegate void EnterKeyDown();
        /// <summary>
        /// Updates properties SelectedItemId and SelectedItemName to be used in parent forms to load searched item.
        /// </summary>
        public ItemSelectionChanged OnItemSelectionChanged;
        /// <summary>
        /// This event will be fired when user presses enter key on txtSearchItems  
        /// This will tell the parent form about ENTER has been pressed.
        /// </summary>
        public EnterKeyDown OnEnterKeyPressed;
        public EnterKeyDown OnEnterKeyDown;

        System.Timers.Timer ItemsSearchTimer = new System.Timers.Timer();
        private List<Item> ItemsGlobal = new List<Item>();
        public UC_SearchRackItems()
        {
            InitializeComponent();
            StoreId = 0;
            ItemsSearchTimer.Enabled = true;
            ItemsSearchTimer.Interval = SharedVariables.AsyncDataLoadDelay;
            ItemsSearchTimer.AutoReset = true;
            ItemsSearchTimer.Elapsed += new System.Timers.ElapsedEventHandler(ItemsSearchTimer_Elapsed);
            ItemsSearchTimer.Stop();
        }
        
        bool isSearching = false;
        private void SearchItems()
        {
            isSearching = true;
            string s = txtSearchItems.Text;
            if (s == "")
            {
                txtSearchItems.Text = "";
                this.SelectedItemId = 0;
                ItemsSearchTimer.Stop();
                this.Invoke(new Action(() =>
                {
                    HideSearch();
                }));
                return;
            }
            IPagedList<Item> ItemsLocal = null;
            using (UnitOfWork = new UnitOfWork())
            {
                //Rack in Edit
                //-- Get Items that have rach and rack items 
                //Rack in Add 
                //-- Get Rack that not contains the other items
                ItemsLocal = UnitOfWork.ItemRspository.GetMatchingItems(s.ToLower(), 0, PageNo, 20);
            }
            if (ItemsLocal == null || ItemsLocal.Count <= 0)
                ItemsSearchTimer.Stop();
            if (ItemsLocal != null && ItemsLocal.Count() > 0)
            {
                ItemsGlobal.AddRange(ItemsLocal);
                foreach (Item i in ItemsLocal)
                {
                    this.Invoke(new Action(() =>
                    {
                        grdSearchItems.Rows.Add(i.ItemID, i.ItemName);
                    }));
                }
            }
            if (ItemsGlobal != null && ItemsGlobal.Count > 0)
            {
                this.Invoke(new Action(() =>
                {
                    if (!isSearchShown)
                    {
                        ShowSearch();
                    }
                }));
            }
            isSearching = false;
        }
        private void LoadSearchedItem()
        {
            ItemsSearchTimer.Stop();
            this.SelectedItemId = 0;
            if (grdSearchItems.SelectedRows.Count > 0)
            {
                txtSearchItems.TextChanged -= txtSearchItems_TextChanged;
                this.SelectedItemId = Convert.ToInt32(grdSearchItems.SelectedRows[0].Cells["colSItemId"].Value);
                this.SelectedItemName = txtSearchItems.Text = grdSearchItems.SelectedRows[0].Cells["colSItemName"].Value.ToString();
                HideSearch();
                txtSearchItems.TextChanged += txtSearchItems_TextChanged;

                this.OnItemSelectionChanged();
                //this.DelegateReturnSelectItem(this.SelectedItemId, this.SelectedItemName);                
                //LoadItemBatches(this.SelectedItemId);
                //btnAddItem.PerformClick();
            }
        }
        private void ItemsSearchTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!isSearching)
            {
                PageNo += 1;
                SearchItems();
            }
        }
        private void txtSearchItems_Enter(object sender, EventArgs e)
        {
            HideSearch();
        }
        private void txtSearchItems_Leave(object sender, EventArgs e)
        {
            //if (grdSearchItems.Rows.Count > 0)
            //{
            //    grdSearchItems.Rows[0].Selected = true;
            //}            
        }
        private void txtSearchItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadSearchedItem();
                //btnAddItem.PerformClick();
                if (this.OnEnterKeyDown != null)
                {
                    OnEnterKeyDown();
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                ShiftCtrlToItemSearchGrid();
            }
        }
        private void txtSearchItems_TextChanged(object sender, EventArgs e)
        {
            if (this.OnItemSelectionChanged == null)
            {
                return;
            }
            if (this.txtSearchItems.Text == "")
            {
                ItemsSearchTimer.Stop();
                HideSearch();
                this.SelectedItemId = 0;
                this.SelectedItemName = "";
                this.OnItemSelectionChanged();
                return;
            }
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
                HideSearch();
            }
        }
        private void ShiftCtrlToItemSearchGrid()
        {
            if (grdSearchItems.Rows.Count > 0)
            {
                grdSearchItems.BringToFront();
                grdSearchItems.Focus();
                if (grdSearchItems.Rows.Count > 0)
                {
                    grdSearchItems.Rows[0].Selected = true;
                }
            }
        }
        public void ShowSearch()
        {
            isSearchShown = true;
            this.Size = new Size(txtSearchItems.Size.Width + 1, this.grdSearchItems.Size.Height + txtSearchItems.Height + 10);
            this.BringToFront();
            this.grdSearchItems.BringToFront();
            this.grdSearchItems.ClearSelection();
        }
        public void HideSearch()
        {
            isSearchShown = false;
            this.Size = new Size(txtSearchItems.Width + 1, txtSearchItems.Height + 2);
        }
    }
}