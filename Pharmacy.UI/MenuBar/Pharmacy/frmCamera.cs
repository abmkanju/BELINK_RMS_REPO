using AForge.Video;
using AForge.Video.DirectShow;
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
    public partial class frmCamera : Form
    {
        frmAddPatient _frm = new frmAddPatient();
        public frmCamera(frmAddPatient frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private FilterInfoCollection videoDevices;
        private string capturedImagePath;
        private VideoCaptureDevice videoSource;
        private void frmCamera_Load(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                videoSource.Start();
            }
            else
            {
                MessageBox.Show("No video devices found.");
            }
        }
        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bitmap;
        }

        private void frmCamera_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                string filePath = "captured_image.jpg"; // Specify the file path and extension
                pictureBox1.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                capturedImagePath = filePath;
                _frm.lblFilePath.Text = capturedImagePath;
                this.Hide();
            }
            else
            {
                MessageBox.Show("Camera is not running.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
