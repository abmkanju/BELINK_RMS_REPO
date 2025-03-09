using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Pharmacy.Core.ViewModels;
using PagedList;
namespace Pharmacy.Infrastructure.Repository
{
    public class AdmitPatientsRepository : Repository<AdmittedPatient>
    {
        PharmacyContext cxt;
        public AdmitPatientsRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public IPagedList<IGrouping<Ward, AdmittedPatient>> GetAdmittedPatients(int pageNo, int PageSize)
        {

            // this query got groups accurately, but could not include reference entities data
            //var res2 = cxt.AdmitPatients
            //            .Where(a=>!a.IsDischarged)
            //            .GroupBy(a=>a.Ward)
            //            .Select(group=> new {
            //                Ward = group.Key,
            //                AdmittedPatients = group.ToList()
            //            }).ToList();

            var result =
                (from ap in cxt.AdmitPatients
                 join p in cxt.Patients on ap.Patient equals p
                 group ap by ap.Ward into g
                 select g).OrderByDescending(r => r.Key.WardId).ToPagedList(pageNo, PageSize);

            foreach (var res in result)
            {
                foreach (AdmittedPatient p in res)
                {
                    p.Patient = cxt.AdmitPatients.Where(ap => ap.AdmittedPatientId == p.AdmittedPatientId).Select(r => r.Patient).FirstOrDefault();
                    p.Doctors = cxt.AdmitPatients.Where(ap => ap.AdmittedPatientId == p.AdmittedPatientId).FirstOrDefault().Doctors == null ? null : cxt.AdmitPatients.Where(ap => ap.AdmittedPatientId == p.AdmittedPatientId).FirstOrDefault().Doctors.ToList();
                }
                res.Key.Beds = cxt.Beds.Where(b => b.Ward.WardId == res.Key.WardId).ToList();
            }
            //{
            //    Ward = g.Key.w,
            //    Patient = g.Key.ap.Patient,
            //}
            //).ToList();
            return result;
        }

        public bool IsPatientAlreadyAdmitted(long PatientId)
        {
            return cxt.AdmitPatients.Any(ap => !ap.IsDischarged && ap.Patient.PatientId == PatientId);
        }

        public decimal GetNewAdmissionNoi()
        {
            return cxt.AdmitPatients.Select(ap => ap.AdmissionNo).FirstOrDefault() + 1;
        }

        public AdmittedPatient GetAdmittedPatientDataById(long AdmittedPatientId)
        {
            return cxt.AdmitPatients.Where(ap => ap.AdmittedPatientId == AdmittedPatientId)
                .Include(ap => ap.Patient)
                .Include(ap => ap.Ward)                
                .Include(ap=>ap.Bed)
                .Include(ap=>ap.Doctors).ToList()
                .FirstOrDefault();
        }

        public DateTime GetAdmissionDate(long AdmittedPatientId)
        {
            return cxt.AdmitPatients.Where(a => a.AdmittedPatientId == AdmittedPatientId).Select(a => a.CreateDate).FirstOrDefault();
        }
    }
}