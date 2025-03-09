using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using Pharmacy.UI.ManageOfflineTask;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zen.Barcode;

namespace Pharmacy.UI.MenuBar.Pharmacy
{
    public partial class frmPrintStockBarcode : Form
    {
        long StockId;
        UnitOfWork unitOfWork;
        public frmPrintStockBarcode(long StockId)
        {
            InitializeComponent();
            this.StockId = StockId;
            LoadItems();
            SharedFunctions.SetGridStyle(grdItems);
        }

        private void LoadItems()
        {
            try
            {
                grdItems.Rows.Clear();
                using (unitOfWork = new UnitOfWork())
                {
                    List<StockItem> items = unitOfWork.StockRepository.GetItemsByStockId(this.StockId);
                    foreach (var item in items)
                    {
                        item.Quantity = item.Unit == 0 ? item.Quantity / item.Item.ConversionUnit : item.Quantity;
                        grdItems.Rows.Add(item.Item.ItemID, item.Item.ItemName, item.Quantity);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colNoOfCopy"].Index)
            {
                e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress); // to allow onyl integer input here in the datagridview
            }
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }
        private async void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    using (new CursorWait())
                    {
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            int ItemId = SharedFunctions.StringToInt(r.Cells["colItemId"].Value);
                            int NoOfCopy = SharedFunctions.StringToInt(r.Cells["colNoOfCopy"].Value);
                            if (NoOfCopy <= 0)
                                continue;
                            Item item = unitOfWork.ItemRspository.GetItemData_ByITemId(ItemId);
                            if (item == null)
                                continue;
                            if (string.IsNullOrEmpty(item.Barcode))
                            {
                                item.Barcode = SharedFunctions.GetTimestamp2(DateTime.Now);
                                unitOfWork.ItemRspository.Update(item);
                                unitOfWork.Save();
                                if (!item.IsFileItem)
                                    OfflineJsonTask.SaveOfflineTask(unitOfWork, item, ModuleName.Item, ModuleAction.Update, item.ItemID);
                                await Task.Delay(2000);
                            }
                            Code128BarcodeDraw barcode = BarcodeDrawFactory.Code128WithChecksum;
                            var barcodeImage = barcode.Draw(item.Barcode, 20);
                            double RetailPrice = (double)(item.RetailPrice * item.ConversionUnit);
                            var resultImage = SharedFunctions.BarcodeSetting(barcodeImage, item.Barcode, item.ItemName, RetailPrice);
                            using (PrintDocument pd = new PrintDocument())
                            {
                                pd.PrintPage += new PrintPageEventHandler((obj, arg) => PrintPage(obj, arg, resultImage));
                                pd.PrinterSettings = new PrinterSettings
                                {
                                    Copies = (short)NoOfCopy,
                                    PrinterName = SharedVariables.AdminInvoiceSetting.BarcodePrinterName == null ||
                                        SharedVariables.AdminInvoiceSetting.BarcodePrinterName == string.Empty ?
                                        "Microsoft Print to PDF" : SharedVariables.AdminInvoiceSetting.BarcodePrinterName
                                };
                                pd.Print();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        private void PrintPage(object sender, PrintPageEventArgs e, Bitmap bitmap)
        {
            Rectangle m = e.MarginBounds;
            e.Graphics.DrawImage(bitmap,
                     (e.PageBounds.Width - bitmap.Width) / 2,
                     (e.PageBounds.Height - bitmap.Height) / 2,
                     bitmap.Width,
                     bitmap.Height);
        }

        private void frmPrintBarcode_Load(object sender, EventArgs e)
        {

        }
    }
}
