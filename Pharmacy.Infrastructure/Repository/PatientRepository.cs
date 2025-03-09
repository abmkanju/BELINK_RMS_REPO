using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Pharmacy.Core.Entities;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.ViewModels;
using Pharmacy.Core;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Pharmacy.Infrastructure.Repository
{
    public class PatientRepository : Repository<Patient>, IPatient
    {
        PharmacyContext cxt = null;
        public PatientRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public decimal GetNewMrNo()
        {
            decimal? MrNo = cxt.Patients.Max(p => p.MRNo);
            return (decimal)(MrNo == null ? 1 : MrNo + 1);
        }

        public List<Patient> GetActivePatients()
        {
            return cxt.Patients
                .Where(p => p.IsActive)
                .ToList();
        }
        public IPagedList<Patient> GetPatientsAsync(int pageNo, int pageSize)
        {
            return cxt.Patients.Where(p => p.IsActive).OrderByDescending(p => p.PatientId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<Patient> GetPatientsAsync_ByTag(int TagId, int pageNo, int pageSize)
        {

            return (from p in cxt.Patients
                    from t in p.Tags
                    where t.TagId == TagId && p.IsActive
                    orderby p.PatientId descending
                    select p).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<Patient> GetPatientsAsync_BySearch(string SearchString, int pageNo, int pageSize)
        {
            return (from p in cxt.Patients
                    where p.IsActive && (p.Name.Contains(SearchString) ||
                    p.Phone.Contains(SearchString) ||
                    p.Phone2.Contains(SearchString) ||
                    p.Phone3.Contains(SearchString) ||
                    p.Email.Contains(SearchString)) ||
                    SqlFunctions.StringConvert(p.MRNo).Contains(SearchString)
                    orderby p.PatientId descending
                    select p).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<Patient> GetPatientsAsync_ByAdvancedSearch(string SearchString, string SearchBy, int pageNo, int pageSize)
        {
            switch (SearchBy)
            {
                case "Name":
                    return (from p in cxt.Patients
                            where p.IsActive && p.Name.Contains(SearchString)
                            orderby p.PatientId descending
                            select p).ToPagedList(pageNo, pageSize);
                case "MR#":
                    return (from p in cxt.Patients
                            where p.IsActive && p.MRNo.ToString().Contains(SearchString)
                            orderby p.PatientId descending
                            select p).ToPagedList(pageNo, pageSize);
                case "Email":
                    return (from p in cxt.Patients
                            where p.IsActive && p.Email.Contains(SearchString)
                            orderby p.PatientId descending
                            select p).ToPagedList(pageNo, pageSize);

                case "Phone":
                    return (from p in cxt.Patients
                            where p.IsActive && p.Phone.Contains(SearchString)
                            orderby p.PatientId descending
                            select p).ToPagedList(pageNo, pageSize);

                case "Date Of Birth":
                    return (from p in cxt.Patients
                            where p.IsActive && p.DateOfBirth.ToString().Contains(SearchString)
                            orderby p.PatientId descending
                            select p).ToPagedList(pageNo, pageSize);

                case "Address":
                    return (from p in cxt.Patients
                            where p.IsActive && p.Address.Contains(SearchString)
                            orderby p.PatientId descending
                            select p).ToPagedList(pageNo, pageSize);

                case "Referred By":
                    return (from p in cxt.Patients
                            where p.IsActive && p.ReferredBy.Contains(SearchString)
                            orderby p.PatientId descending
                            select p).ToPagedList(pageNo, pageSize);

            }
            return null;
        }
        public IPagedList<PatientSearchVM> SearchPatient(bool IsByName, bool IsByPhone, bool IsByMrNo, string SearchString, int pageNo)
        {
            IPagedList<PatientSearchVM> Result = null;// ;//= new List<PatientSearchVM>();
            if (IsByName)
            {
                return Result = cxt.Patients
                     .Where(p => p.IsActive)
                     .Where(p => p.Name.ToLower().Contains(SearchString))
                     .Select(p => new PatientSearchVM()
                     {
                         PatientId = p.PatientId,
                         MrNo = (decimal)p.MRNo,
                         PatientName = p.Name,
                         Address = p.Address,
                         Phone = p.Phone,
                     }).OrderBy(p => p.PatientId).ToPagedList(pageNo, 20);
            }

            if (IsByPhone)
            {
                return Result = cxt.Patients
                   .Where(p => p.IsActive)
                   .Where(p => p.Phone.Contains(SearchString))
                   .Select(p => new PatientSearchVM()
                   {
                       PatientId = p.PatientId,
                       MrNo = (decimal)p.MRNo,
                       PatientName = p.Name,
                       Phone = p.Phone,
                   }).OrderBy(p => p.PatientId).ToPagedList(pageNo, 20);
            }

            if (IsByMrNo)
            {
                Result = cxt.Patients
                   .Where(p => p.IsActive)
                   .Where(p => p.MRNo.ToString().Contains(SearchString))
                   .Select(p => new PatientSearchVM()
                   {
                       PatientId = p.PatientId,
                       MrNo = (decimal)p.MRNo,
                       PatientName = p.Name,
                       Phone = p.Phone,
                   }).OrderBy(p => p.PatientId).ToPagedList(pageNo, 20);
            }
            return Result;
        }
        public List<PatientSearchVM> LoadPatientsCombo()
        {
            return cxt.Patients
                .Where(p => p.IsActive)
                .Select(p => new PatientSearchVM()
            {
                PatientId = p.PatientId,
                PatientName = p.MRNo + " =>     " + p.Name + "    | " + p.Phone
                //Phone = p.Phone,
                //MrNo = (decimal)p.MRNo
            }).ToList();
        }
        public Patient GetPatientWithTags_ByPatientId(long PatientId)
        {
            return cxt.Patients
                .Where(p => p.PatientId == PatientId && p.IsActive)
                .Include(p => p.Tags).FirstOrDefault();
        }
        public Patient GetPatientByPatientId(long PatientId)
        {
            return cxt.Patients
                .Where(p => p.PatientId == PatientId && p.IsActive)
                .FirstOrDefault();
        }
        public void UpdateStatus(long PatientId, int Status)
        {
            string qry = "update patients set Status = @Status where PatientId = @PatientID";
            cxt.Database.ExecuteSqlCommand(qry, new SqlParameter("@Status", 2), new SqlParameter("@PatientID", PatientId));
        }
        public bool PatientAlreadyHasTag(long TagId, long PatientId)
        {
            Patient objPatient = cxt.Patients.Where(p => p.PatientId == PatientId && p.Tags.Any(t => t.TagId == TagId)).FirstOrDefault();
            return objPatient == null ? false : true;
        }
        public void SetInActive(long PatientId)
        {
            cxt.Database.ExecuteSqlCommand("Update Patients Set IsActive = 0 WHERE PatientId = @PatientId", new SqlParameter("@PatientId", PatientId));
        }
        public double getDuesByPatientId(long patientId, long invoiceId)
        {
            var res = cxt.Invoices
                .Where(i => i.Patient.PatientId == patientId && i.InvoiceId != invoiceId)
                .Include(i => i.InvoicePayments).ToList();
            if (res != null)
            {
                return res.Sum(r => r.GrandTotal) - res.Sum(r => r.InvoicePayments.Where(ip => ip.IsActive).Sum(ip => ip.Payment));
            }
            else
            {
                return 0;
            }
        }
        public List<SelectListVM> GetSelectList()
        {
            return cxt.
                Patients
                .Where(p => p.IsActive)
                .Select(p => new SelectListVM
                {
                    Value = p.PatientId,
                    Text = p.MRNo + " " + p.Name
                }).ToList();
        }
        public bool CheckMrNoExists(long patientId, decimal? mrNo)
        {
           return cxt.Patients.Count(x => x.MRNo == mrNo && x.PatientId != patientId) > 0;
        }
    }
}