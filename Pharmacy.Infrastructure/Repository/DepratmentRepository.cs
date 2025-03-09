using Pharmacy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Pharmacy.Core.ViewModels;

namespace Pharmacy.Infrastructure.Repository
{
    public class DepratmentRepository : Repository<Department>
    {
        PharmacyContext cxt;
        public DepratmentRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public DepartmentVM GetDepWithActiveSubDeps(long DepartmentId)
        {
            return cxt.Departments
                .Where(dep => dep.DepartmentId == DepartmentId)
                .Select(d => new DepartmentVM
                {
                    DepartmentId = d.DepartmentId,
                    Name = d.Name,
                    SubDepartments = d.SubDepartments.Where(sd => sd.IsActive).ToList()
                }).FirstOrDefault();
        }

        public Department GetDepWithAllSubDeps(long DepartmentId)
        {
            return cxt.Departments
                .Where(dep => dep.DepartmentId == DepartmentId)
                .Include(d => d.SubDepartments).ToList()
                .FirstOrDefault();
        }

        public List<DepartmentVM> GetAllDepsWithActiveSubDeps()
        {
            return cxt.Departments
                .Where(dep => dep.IsActive)
                .Select(d => new DepartmentVM
                {
                    DepartmentId = d.DepartmentId,
                    Name = d.Name,
                    SubDepartments = d.SubDepartments.Where(sd => sd.IsActive).ToList()
                }).ToList();
        }

        public void SetDepartmentInActive(long DepartmentId)
        {
            cxt.Departments.Where(dep => dep.DepartmentId == DepartmentId).FirstOrDefault().IsActive = false;
            cxt.SaveChanges();
        }

        public List<SubDepartment> GetActiveIPDSubDepartments()
        {
            return cxt.SubDepartments.Where(sd => sd.Department.Name == "IPD" && sd.Department.IsActive && sd.IsActive).ToList();
        }
    }
}