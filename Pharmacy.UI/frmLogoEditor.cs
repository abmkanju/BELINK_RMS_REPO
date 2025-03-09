using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI
{
    public partial class frmLogoEditor : Form
    {
        public frmLogoEditor()
        {
            InitializeComponent();
            
        }
        public frmLogoEditor(string LogoLoc)
        {
            InitializeComponent();
            radImageEditor1.BitmapPath = LogoLoc;
            Image img = Bitmap.FromFile(LogoLoc);
            Bitmap bitmap = new Bitmap(img);
            radImageEditor1.CurrentBitmap = bitmap;
            
        }
    }
}
