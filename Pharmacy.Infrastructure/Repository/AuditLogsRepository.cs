using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Repository
{
    public class AuditLogsRepository : Repository<AuditLog>
    {
        PharmacyContext cxt;
        public AuditLogsRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public bool CheckAuditLogReachLimit()
        {
            return cxt.AuditLogs.Count() > 50000;
        }

        public void DeleteBulkAuditLog()
        {
            cxt.Database.ExecuteSqlCommand("delete top(30000) from AuditLogs where (select COUNT(*) from AuditLogs ) >= 50000");
        }
        public IPagedList<AuditLogVM> GetAuditLogs(DateTime? fromDate, DateTime? toDate, List<int> UserIds, List<int> ActionIds, int pageNo, int PageSize)
        {
            //IQueryable<AuditLog> audit;
            IPagedList<AuditLogVM> result = null;
            var audit = cxt.AuditLogs.AsQueryable().Join(cxt.Users,
                auditlog => auditlog.UserId,
                user => user.UserId,
                (auditlog, user) => new { auditlog, user });
            if (fromDate.HasValue)
                audit = audit.Where(i => DbFunctions.TruncateTime(i.auditlog.CreatedOn) >= DbFunctions.TruncateTime(fromDate));
            if (toDate.HasValue)
                audit = audit.Where(i => DbFunctions.TruncateTime(i.auditlog.CreatedOn) <= DbFunctions.TruncateTime(toDate));
            if (ActionIds.Count() > 0)
                audit = audit.Where(x => ActionIds.Contains(x.auditlog.ActionId));
            if (UserIds.Count() > 0)
                audit = audit.Where(x => UserIds.Contains(x.auditlog.UserId));
            result = audit.Select(x => new AuditLogVM 
            { 
                AuditLogId = x.auditlog.AuditLogId, 
                ActionId = x.auditlog.ActionId, 
                Description = x.auditlog.Description, 
                CreatedOn = x.auditlog.CreatedOn, 
                UserName = x.user != null ? x.user.UserName : "",
            }).OrderByDescending(x => x.AuditLogId).ToPagedList(pageNo, PageSize);
            return result;
        }
    }
}
