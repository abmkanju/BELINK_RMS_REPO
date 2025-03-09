using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.Core;
using Pharmacy.Infrastructure;
using Pharmacy.Core.Entities;
using System.Data.Entity;
using PagedList;
namespace Pharmacy.Infrastructure.Repository
{
    public class UserRoleRepository : Repository<UserRole>
    {
        PharmacyContext cxt;
        public UserRoleRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
        public List<UserRole> GetAllRolesWithUsersAndRights(bool IsSuperAdminLoggedIn)
        {
            if (IsSuperAdminLoggedIn)
            {
                return cxt.UserRoles
                    .Where(r => r.IsActive)
                    .Include(r => r.Users.Select(u => u.Rights))
                    .OrderByDescending(r => r.UserRoleId).Where(r => r.Users.Any(u => u.IsActive == true)).ToList();
            }
            else
            {
                return cxt.UserRoles
                    .Where(r => r.IsActive)
                    .Where(r => !r.IsAdmin)
                    .Include(r => r.Users.Select(u => u.Rights))
                    .OrderByDescending(r => r.UserRoleId).Where(r => r.Users.Any(u => u.IsActive == true)).ToList();
            }
            //if (IsSuperAdminLoggedIn)
            //{
            //    return cxt.UserRoles
            //        .Where(r => r.IsActive)
            //        .Include(r => r.Users.Select(u => u.Rights))
            //        .OrderByDescending(r => r.UserRoleId).Where(r => r.Users.Any(u => u.IsActive)).ToList();
            //}
            //else
            //{
            //    return cxt.UserRoles
            //        .Where(r => r.IsActive)
            //        .Where(r => !r.IsAdmin)
            //        .Include(r => r.Users.Select(u => u.Rights))
            //        .OrderByDescending(r => r.UserRoleId).Where(r => r.Users.Any(u => u.IsActive)).ToList();
            //}
        }

        public List<UserRole> GetNonAdminUserRoles()
        {
            return cxt.UserRoles.Where(r => !r.IsAdmin).ToList();
        }

        public List<UserRole> GetAdminOnlyUserRole()
        {
            return cxt.UserRoles.Where(r => r.IsAdmin).ToList();
        }

        public List<UserRole> GetActiveUserRoles()
        {
            return cxt.UserRoles.Where(r => r.IsActive).ToList();
        }

        public UserRole GetUserRoleId(int UserId)
        {
            return cxt.UserRoles.Where(r => r.IsActive && r.UserRoleId == UserId).FirstOrDefault();
        }
        public bool CheckAdminUserRole(int UserId)
        {
            return cxt.Users.Where(r => r.UserId == UserId && r.UserRoles.Any(ur => ur.IsAdmin)).ToList().Count() > 0 ? true : false;
        }

    }
}