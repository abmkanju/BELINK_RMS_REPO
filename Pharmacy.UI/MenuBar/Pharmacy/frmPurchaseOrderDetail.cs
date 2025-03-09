using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Infrastructure;
using Pharmacy.Core.Entities;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core;
namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmPurchaseOrderDetail : Form
    {
        UnitOfWork unitOfWork;
        private long PurchaseOrderId = 0;
        int PageSize = SharedVariables.PageSize;
        public frmPurchaseOrderDetail()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        public frmPurchaseOrderDetail(long PurchaseOrderId)
        {
            InitializeComponent();
            this.PurchaseOrderId = PurchaseOrderId;
            unitOfWork = new UnitOfWork();
        }
        private void frmPurchaseOrderDetail_Load(object sender, EventArgs e)
        {
            GetPurchaseOrder();
            SharedFunctions.SetGridStyle(grdOrderItems);
        }

        private void GetPurchaseOrder()
        {
            if (PurchaseOrderId > 0)
            {
                PurchaseOrder PurchaseOrder = unitOfWork.PurchaseOrderRepository.GetPurchaseOrderById_Inc_Stock(this.PurchaseOrderId);
                //if (PurchaseOrder.Stock == null)
                //{
                //    btnAddStock.Visible = true;
                //    btnEdit.Visible = true;
                //    btnDelete.Visible = true;
                //}
                lblPONo.Text = PurchaseOrder.PurchaseOrderNo.ToString();
                if (PurchaseOrder.Supplier != null)
                {
                    lblSupplierName.Text = PurchaseOrder.Supplier.Name;
                }
               
                lblPODate.Text = PurchaseOrder.OrderDate.ToString("dddd, dd MMMM yyyy");
                lblDeliveryDate.Text = PurchaseOrder.DeliveryDate.ToString("dddd, dd MMMM yyyy");

                lblTotalAmount.Text = PurchaseOrder.TotalAmount.ToString();
                if (PurchaseOrder.Manufecturers != null)
                {
                    lblManufecturers.Text = "";
                    foreach (Manufacturer manu in PurchaseOrder.Manufecturers)
                    {
                        lblManufecturers.Text += manu.Name + ",\n";
                    }
                }
                else
                {
                    lblManufecturers.Text = "";
                }
                this.Text += "(" + PurchaseOrder.SupplierName + ")";
                foreach (PurchaseOrderItem i in PurchaseOrder.PurchaseOrderItems)
                {
                    if (i.IsActive)
                    {
                        
                        i.Quantity = i.Unit == 0 ? i.Quantity / i.Item.ConversionUnit : i.Quantity;
                        i.UnitCost = i.Unit == 0 ? i.UnitCost * i.Item.ConversionUnit : i.UnitCost;
                        i.UnitString = i.Unit == 0 ? i.Item.Unit : "Units";
                        grdOrderItems.Rows.Add(i.Item.ItemID, i.Item.ItemName, i.Manufecturer == null ? "" : i.Manufecturer.ManufacturerId.ToString(), i.Manufecturer == null ? "" : i.Manufecturer.Name, i.UnitString, i.Quantity, i.UnitCost, i.GrossAmount , i.Discount , i.DiscountType == 0 ? "%" : "Value" , i.Bonus , i.TotalAmount);
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddStock_Click(object sender, EventArgs e)
        {
            frmAddStock2 f = new frmAddStock2(this.PurchaseOrderId);
            f.Show();
            return;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            frmPurchaseOrder f = new frmPurchaseOrder(this.PurchaseOrderId);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
            f.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            unitOfWork.PurchaseOrderRepository.SetInActive(this.PurchaseOrderId);
            SharedFunctions.ShowSuccessMessage("Order Deleted Successfully", "Success");
            this.Close();
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            unitOfWork = new UnitOfWork();
            RefreshForm();
        }

        private void RefreshForm()
        {
            grdOrderItems.Rows.Clear();
            GetPurchaseOrder();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Reports.PurchaseOrderViewer v = new Reports.PurchaseOrderViewer(this.PurchaseOrderId);
            v.Show();
            return;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}