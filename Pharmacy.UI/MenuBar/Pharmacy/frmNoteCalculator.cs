using Pharmacy.Core;
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
    public partial class frmNoteCalculator : Form
    {
        public frmNoteCalculator()
        {
            InitializeComponent();
        }

        private void grdNotes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdNotes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                if (e.ColumnIndex == grdNotes.Columns["colCount"].Index)
                {
                    double total = 0;
                    int count = 0; bool parsed = int.TryParse(grdNotes.Rows[e.RowIndex].Cells["colCount"].Value.ToString(), out count);
                    if (!parsed)
                    {
                        grdNotes.Rows[e.RowIndex].Cells["colCount"].Value = 0;
                    }
                    int Note = Convert.ToInt32(grdNotes.Rows[e.RowIndex].Cells["colNote"].Value);
                    grdNotes.Rows[e.RowIndex].Cells["colTotal"].Value = count * Note;
                    foreach (DataGridViewRow r in grdNotes.Rows)
                    {
                        total += Convert.ToDouble(r.Cells["colTotal"].Value);
                    }
                    SharedVariables.TotalNotesAmount = total;
                    txtTotal.Text = total.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while performing notes counting, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SharedVariables.TotalNotesAmount = 0;
            this.Close();
        }

        private void frmNoteCalculator_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdNotes);
            SharedFunctions.SetSmallButtonsStyle(new[] { btnOk, btnCancel });
            grdNotes.Rows.Add(1, 5000, 0, 0);
            grdNotes.Rows.Add(1, 1000, 0, 0);
            grdNotes.Rows.Add(1, 500, 0, 0);
            grdNotes.Rows.Add(1, 100, 0, 0);
            grdNotes.Rows.Add(1, 50, 0, 0);
            grdNotes.Rows.Add(1, 20, 0, 0);
            grdNotes.Rows.Add(1, 10, 0, 0);
            grdNotes.Rows.Add(1, 5, 0, 0);
            grdNotes.Rows.Add(1, 2, 0, 0);
            grdNotes.Rows.Add(1, 1, 0, 0);
        }
    }
}