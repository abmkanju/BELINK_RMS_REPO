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
    public class UserRepository : Repository<User>
    {
        PharmacyContext cxt;
        public UserRepository(PharmacyContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public User GetUserDetailsWithRolesAndRights(int UserId)
        {
            return cxt.Users
                .Where(u => u.UserId == UserId)
                .Include(u => u.UserRoles)
                .Include(u => u.Rights).FirstOrDefault();
        }

        public User IsLoggedIn(string UserName, string Password)
        {
            return cxt.Users
                .Where(u => u.Email == UserName && u.Password == Password && u.IsActive)
                .Include(u => u.Rights)
                .Include(u => u.Shift)
                .Include(u => u.UserRoles)
                .FirstOrDefault();
        }
        public User IsLoggedInbyPIN(string UserName, int pin)
        {
            return cxt.Users
                .Where(u => u.Email == UserName && u.Pin == pin && u.IsActive)
                .Include(u => u.Rights)
                .Include(u => u.Shift)
                .Include(u => u.UserRoles)
                .FirstOrDefault();
        }
        public User IsInvalidPIN(int PIN)
        {
            return cxt.Users
                .Where(u => u.Pin == PIN && u.IsActive)
                .Include(u => u.Rights)
                .Include(u => u.Shift)
                .Include(u => u.UserRoles)
                .FirstOrDefault();
        }

        public List<string> GetAdminUsersEmails()
        {
            //return cxt.Users
            //            .Where(u => u.IsActive)
            //            .Where(u => u.UserRole.Description.ToLower().Equals("admin"))
            //            .Select(u => u.Email).ToList();
            return null;
        }

        public List<User> GetActiveDoctors()
        {
            return cxt.Users
                .Where(u => u.IsActive && u.UserRoles.Any(r => r.Description == "Doctor"))
                .ToList();
        }
        public List<UserSelectVM> GetAllActiveUsers()
        {
            return cxt.Users
                .Where(u => u.IsActive)
                .Select(u => new UserSelectVM
                {
                    UserId = u.UserId,
                    UserName = u.UserName
                }).ToList();
        }
        
        public List<UserSelectVM> GetStaffUsers()
        {
            List<int> roleList = new List<int>(){ 2, 3, 5, 6};
            return cxt.Users
                .Where(u => u.IsActive && u.UserRoles.Any(x=> roleList.Contains(x.UserRoleId)))
                .Select(u => new UserSelectVM
                {
                    UserId = u.UserId,
                    UserName = u.UserName
                }).ToList();
        }
    }
}