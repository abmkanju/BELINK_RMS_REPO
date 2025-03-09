using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PagedList;
using Pharmacy.Core.ViewModels;
namespace Pharmacy.Infrastructure.Repository
{
    public class VitalRepository : Repository<Vital>
    {
        PharmacyContext cxt;
        public VitalRepository(PharmacyContext cxt) : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<Vital> GetVitals_Paginated(int pageNo, int PageSize, long AdmittedPatientId)
        {
            return cxt.Vitals
                .Where(v => v.AdmittedPatient.AdmittedPatientId == AdmittedPatientId && v.IsActive)
                .Include(v => v.User)
                .OrderByDescending(v => v.VitalId)
                .ToPagedList(pageNo, PageSize);
        }

        public List<Vital> GetVitals(DateTime DateFrom, DateTime DateTo, long AdmittedPatientId)
        {
            //VitalsGraphDataVM
            return cxt.Vitals
                .Where(v => v.IsActive)
                .Where(v=>v.AdmittedPatient.AdmittedPatientId == AdmittedPatientId)
                .Where(v => DbFunctions.TruncateTime(v.CreateDate) >= DbFunctions.TruncateTime(DateFrom) && DbFunctions.TruncateTime(v.CreateDate) <= DbFunctions.TruncateTime(DateTo))
                .OrderBy(v => v.CreateDate).ToList();
        }
    }
}