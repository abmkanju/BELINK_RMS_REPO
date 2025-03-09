using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Pharmacy.Infrastructure.Repository
{
    public class ProcedureRepository : Repository<Procedure>
    {
        PharmacyContext cxt;
        public ProcedureRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<Procedure> GetActiveProcedures(string NameFilter, int PageNo, int PageSize)
        {
            if (string.IsNullOrEmpty(NameFilter))
            {
                return cxt.Procedures
                    .Where(p => p.IsActive)
                    .OrderByDescending(p => p.ProcedureId).ToPagedList(PageNo, PageSize);
            }
            else
            {
                return cxt.Procedures
                    .Where(p => p.IsActive)
                    .Where(p => p.ProcedureName.Contains(NameFilter))
                    .OrderByDescending(p => p.ProcedureId).ToPagedList(PageNo, PageSize);
            }
        }
        public List<Procedure> GetActiveProceduresList()
        {
            return cxt.Procedures
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.ProcedureId).ToList();
        }

        public Procedure GetDefaultProcedures()
        {
            return cxt.Procedures
                .Where(p => p.IsDefault).FirstOrDefault();                
        }

        public double GetProcedurePrice(long ProcedureId)
        {
            return cxt.Procedures.Where(p => p.ProcedureId == ProcedureId).Select(p => p.Price).FirstOrDefault();
        }
    }
}