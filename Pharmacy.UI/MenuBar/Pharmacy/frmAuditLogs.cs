using PagedList;
using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using Pharmacy.Infrastructure;
using Pharmacy.UI.ManageAuditLog;
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
    public partial class frmAuditLogs : Form
    {
        UnitOfWork unitOfWork;
        int pageNo = 1;
        IPagedList<AuditLogVM> audits;

        #region LoadDetails
        public frmAuditLogs()
        {
            InitializeComponent();
        }
        private void frmStockAudits_Load(object sender, System.EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdItems);
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnSearch });
            LoadList();
        }

        private void LoadList()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {

                    Dictionary<int, string> dict = new Dictionary<int, string>();
                    foreach (var name in Enum.GetNames(typeof(AuditModule)))
                    {
                        dict.Add((int)Enum.Parse(typeof(AuditModule), name), name.Replace("_"," "));
                    }
                    lstAuditActions.DataSource = new BindingSource(dict, null);
                    lstAuditActions.ValueMember = "Key";
                    lstAuditActions.DisplayMember = "Value";

                    List<UserSelectVM> userLists = unitOfWork.UserRepository.GetAllActiveUsers().ToList();
                    lstUsers.DataSource = userLists;
                    lstUsers.ValueMember = "UserId";
                    lstUsers.DisplayMember = "UserName";
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region PaginationButtons
        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadAuditLogs();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo += 1;
            LoadAuditLogs();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = audits.PageCount;
            LoadAuditLogs();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo -= 1;
            LoadAuditLogs();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadAuditLogs();
        }
        #endregion

        #region SearchRecord
        private void btnSearch_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadAuditLogs();
        }
        private void LoadAuditLogs()
        {
            try
            {
                grdItems.Rows.Clear();
                DateTime? dtFrom = null;
                DateTime? dtTo = null;
                List<int> UserIds = new List<int>();
                List<int> AuditLogIds = new List<int>();
                AssignValue(ref dtFrom, ref dtTo, ref UserIds, ref AuditLogIds);
                using (unitOfWork = new UnitOfWork())
                {
                    audits = unitOfWork.AuditLogsRepository.GetAuditLogs(dtFrom, dtTo, UserIds, AuditLogIds, pageNo, SharedVariables.PageSize);
                }
                if (audits != null && audits.Count > 0)
                {
                    foreach (var a in audits)
                    {
                        grdItems.Rows.Add(a.CreatedOn, a.UserName, a.Description);
                    }

                    btnPrevious.Enabled = btnFirstPage.Enabled = audits.HasPreviousPage;
                    btnNext.Enabled = btnLastPage.Enabled = audits.HasNextPage;
                    SharedFunctions.ShowPageNo(lblPageNo, pageNo, audits.PageCount);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.MessageBoxShow(ex.Message, MessageType.Error);
            }

        }

        private void AssignValue(ref DateTime? dtFrom, ref DateTime? dtTo, ref List<int> UserIds, ref List<int> AuditLogIds)
        {
            if (dtpFromDate.Checked)
                dtFrom = dtpFromDate.Value;
            if (dtpToDate.Checked)
                dtTo = dtpToDate.Value;
            UserIds = lstUsers.SelectedItems.OfType<UserSelectVM>().Select(x => x.UserId).ToList();
            foreach (KeyValuePair<int, string> action in lstAuditActions.SelectedItems)
            {
                AuditLogIds.Add(action.Key);
            }
        }
        #endregion

    }
}
