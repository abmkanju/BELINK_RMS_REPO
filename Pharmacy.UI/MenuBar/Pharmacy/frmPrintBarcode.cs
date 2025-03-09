using Pharmacy.Core;
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
    public partial class frmPrintBarcode : Form
    {
        string Barcode = "";
        string name = "";
        double RetailPrice = 0;
        public frmPrintBarcode(string barcode, string name, double RetailPrice)
        {
            InitializeComponent();
            this.Barcode = barcode;
            this.name = name;
            this.RetailPrice = RetailPrice;
            LoadPrinter();
            LoadBardcode();
        }

        private void LoadPrinter()
        {
            try
            {
                List<string> printers = new List<string>();
                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    printers.Add(printer);
                }
                cmbPrinter.DataSource = printers;
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }

        }

        private void LoadBardcode()
        {
            try
            {
                Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                var barcodeImage = barcode.Draw(Barcode, 20);
              
                var resultImage = SharedFunctions.BarcodeSetting(barcodeImage, Barcode, name, this.RetailPrice);
                pictureBox1.Image = resultImage;

                //Font newfont = new Font("IDAutomationHC39M", 13);
                //Size textSize = TextRenderer.MeasureText(Barcode, newfont);
                //Bitmap bitm = new Bitmap(textSize.Width + 20, 90);
                //using (Graphics graphic = Graphics.FromImage(bitm))
                //{
                //    PointF point = new PointF(2f, 2f);
                //    SolidBrush black = new SolidBrush(Color.Black);
                //    SolidBrush white = new SolidBrush(Color.White);
                //    graphic.FillRectangle(white, 0, 0, bitm.Width, bitm.Height);
                //    graphic.DrawString("*" + Barcode + "*", newfont, black, point);
                //}

                //using (MemoryStream Mmst = new MemoryStream())
                //{
                //    bitm.Save("ms", ImageFormat.Jpeg);
                //    pictureBox1.Image = bitm;
                //    pictureBox1.Width = bitm.Width;
                //    pictureBox1.Height = bitm.Height;
                //    pictureBox1.Location = new Point((this.Width - pictureBox1.Image.Width) / 2,
                //        ((this.Height - pictureBox1.Image.Height) / 2) - 50);
                //}
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }

       

        private void btnPrint_Click(object sender, EventArgs e)
        {
            using (PrintDocument pd = new PrintDocument())
            {
                pd.PrintPage += PrintPage;
                pd.PrinterSettings.PrinterName =
                    cmbPrinter.Text == null ||
                    cmbPrinter.Text == string.Empty ?
                    "Microsoft Print to PDF" : cmbPrinter.Text;
                pd.Print();
            }
        }
        private void PrintPage(object o, PrintPageEventArgs e)
        {
            Rectangle m = e.MarginBounds;
            e.Graphics.DrawImage(pictureBox1.Image,
                     (e.PageBounds.Width - pictureBox1.Image.Width) / 2,
                     (e.PageBounds.Height - pictureBox1.Image.Height) / 2,
                     pictureBox1.Image.Width,
                     pictureBox1.Image.Height);
        }

        private void frmPrintBarcode_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
