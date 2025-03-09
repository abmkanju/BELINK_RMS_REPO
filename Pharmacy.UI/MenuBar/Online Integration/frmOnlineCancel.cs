using Pharmacy.Core;
using Pharmacy.Core.Utility;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.FranchiseModuels
{
    public partial class frmOnlineCancel : Form
    {
        int OrderId = 0;
        int OrderDeskId = 0;
        Service obj = new Service();
        public frmOnlineCancel()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public frmOnlineCancel(int OrderId, int OrderDeskId)
        {
            InitializeComponent();
            this.OrderId = OrderId;
            this.OrderDeskId = OrderDeskId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Response check = new Response();
            this.Cursor = Cursors.WaitCursor;
            if (textBox1.Text == "")
            {
                SharedFunctions.MessageBoxShow("Please Enter Cancel Reason!!", MessageType.Warning);
                return;
            }
            else
            {

                check = obj.SendCancelMsg(this.OrderId, textBox1.Text.ToString());
                string reason = textBox1.Text.ToString();

                this.Cursor = Cursors.Default;
                if (check.status == "Success")
                {
                    int up = updateDB("update OnlineOrders set Cancel= " + 1 + ",CancelReason='" + reason + "' where OrderId= " + this.OrderDeskId);
                    SharedFunctions.MessageBoxShow("Order is Cancelled", MessageType.Success);
                    this.Close();
                }
                else
                {
                    SharedFunctions.MessageBoxShow("Order is not Cancelled", MessageType.Success);
                    this.Close();
                }
            }
        }
        private int updateDB(string qry)
        {
            SqlConnection con = new SqlConnection(Shared.GetConnectionString());
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand com = new SqlCommand(qry, con);
            int i = com.ExecuteNonQuery();
            con.Close();
            return i;
        }
    }
}
