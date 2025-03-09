using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI
{
    public class GridDesigner
    {
        public static void CustomizeDataGridView(DataGridView dataGridView)
        {
            // Set the background color of the DataGridView
            dataGridView.BackgroundColor = Color.White;

            // Set the default font for the cells
            dataGridView.DefaultCellStyle.Font = new Font("Arial", 9);

            // Set the text color for the header cells
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            // Set the background color for the header cells
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(36, 54, 82);

            // Set the alignment of the header text to center
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set the border style for the DataGridView
            dataGridView.BorderStyle = BorderStyle.FixedSingle;

            // Set the grid lines color
            dataGridView.GridColor = Color.LightGray;

            // Set the row header width and style
            dataGridView.RowHeadersWidth = 40;
   

            dataGridView.RowHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridView.RowHeadersDefaultCellStyle.ForeColor = Color.Black;

            // Set the alternating row style
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            // Set the selection color
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(36, 54, 82);
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;

            // Set the padding for the cells
            dataGridView.DefaultCellStyle.Padding = new Padding(8, 5, 8, 5);

            // Enable double buffering to reduce flickering
            EnableDoubleBuffering(dataGridView);

            // Enable auto-sizing of columns
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Set full row selection
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Disable multi-select
            dataGridView.MultiSelect = false;

            // Set the default row height and adjust cell wrap mode
            dataGridView.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.DefaultCellStyle.Padding = new Padding(8, 5, 8, 5);
            dataGridView.RowTemplate.Height = 40;
        }
        public static void CustomizeDataGridView_1(DataGridView dataGridView)
        {
            // Set the background color of the DataGridView
            dataGridView.BackgroundColor = Color.White;

            // Set the default font for the cells
            dataGridView.DefaultCellStyle.Font = new Font("Arial", 9);

            // Set the text color for the header cells
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            // Set the background color for the header cells
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(36, 54, 82);

            // Set the alignment of the header text to center
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set the border style for the DataGridView
            dataGridView.BorderStyle = BorderStyle.FixedSingle;

            // Set the grid lines color
            dataGridView.GridColor = Color.LightGray;

            // Set the row header width and style
            dataGridView.RowHeadersWidth = 40;


            dataGridView.RowHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridView.RowHeadersDefaultCellStyle.ForeColor = Color.Black;

            // Set the alternating row style
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            // Set the selection color
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(36, 54, 82);
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;

            // Set the padding for the cells
            dataGridView.DefaultCellStyle.Padding = new Padding(8, 5, 8, 5);

            // Enable double buffering to reduce flickering
            //EnableDoubleBuffering(dataGridView);

            // Enable auto-sizing of columns
            //dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Set full row selection
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Disable multi-select
            dataGridView.MultiSelect = false;

            // Set the default row height and adjust cell wrap mode
            dataGridView.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.DefaultCellStyle.Padding = new Padding(8, 5, 8, 5);
            dataGridView.RowTemplate.Height = 40;
        }

        private static void EnableDoubleBuffering(DataGridView dataGridView)
        {
            Type dgvType = dataGridView.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dataGridView, true, null);
        }
    }
}
