using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI
{
    public static class DarkMode
    {
        public static void EnableDarkMode(Control container)
        {
            container.BackColor = Color.FromArgb(45, 45, 48);
            container.ForeColor = Color.White;

            foreach (Control control in container.Controls)
            {
                control.BackColor = Color.FromArgb(45, 45, 48);
                control.ForeColor = Color.White;

                if (control is Button)
                {
                    Button button = control as Button;
                    button.FlatAppearance.BorderColor = Color.FromArgb(50, 50, 50);
                    button.FlatAppearance.MouseDownBackColor = Color.FromArgb(63, 63, 70);
                    button.FlatAppearance.MouseOverBackColor = Color.FromArgb(63, 63, 70);
                    button.FlatAppearance.BorderColor = Color.WhiteSmoke;
                    button.FlatAppearance.BorderSize = 1;
                }
                else if (control is TextBox)
                {
                    TextBox textBox = control as TextBox;
                    textBox.BackColor = Color.FromArgb(69, 69, 74);
                    textBox.ForeColor = Color.White;
                }
                else if (control is ListBox)
                {
                    ListBox listBox = control as ListBox;
                    listBox.BackColor = Color.FromArgb(69, 69, 74);
                    listBox.ForeColor = Color.White;
                }
                else if (control is DataGridView)
                {
                    DataGridView dataGridView = control as DataGridView;
                    dataGridView.BackgroundColor = Color.FromArgb(45, 45, 48);
                    dataGridView.ForeColor = Color.White;
                    dataGridView.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
                    dataGridView.DefaultCellStyle.ForeColor = Color.White;
                }
                else if (control is Label)
                {
                    Label label = control as Label;
                    label.ForeColor = Color.White;
                }
                else if (control is Panel)
                {
                    Panel panel = control as Panel;
                    panel.BackColor = Color.FromArgb(28, 28, 28);
                    panel.ForeColor = Color.White;
                }
                else if (control is GroupBox)
                {
                    GroupBox groupBox = control as GroupBox;
                    groupBox.BackColor = Color.FromArgb(45, 45, 48);
                    groupBox.ForeColor = Color.White;
                }
                else if (control is CheckBox)
                {
                    
                    CheckBox checkBox = control as CheckBox;
                    checkBox.ForeColor = Color.White;
                }

                if (control.HasChildren)
                {
                    EnableDarkMode(control);
                }
            }
        }

    }
}
