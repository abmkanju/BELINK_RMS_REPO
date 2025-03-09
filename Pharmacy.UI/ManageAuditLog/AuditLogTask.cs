using Pharmacy.Core;
using Pharmacy.Core.Entities;
using Pharmacy.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.UI.ManageAuditLog
{
    public class AuditLogTask
    {
        //AL
        public static void CreateAuditLog(UnitOfWork unitOfWork, string name, AuditModule module, AuditAction action)
        {
            AuditLog audit = new AuditLog();
            audit.ActionId = (int)module;
            audit.Description = module.ToString().Replace("_", " ") + " '" + name + "' " + action.ToString().Replace("_", " ") + ".";
            audit.UserId = SharedVariables.LoggedInUser.UserId;
            audit.CreatedOn = DateTime.Now;
            unitOfWork.AuditLogsRepository.Insert(audit);
        }

        public static void CreateAuditLogInOut(UnitOfWork unitOfWork, string name, AuditModule module, AuditAction action)
        {
            AuditLog audit = new AuditLog();
            audit.ActionId = (int)module;
            audit.Description = "User '" + name + "' " + action.ToString().Replace("_", " ") + ".";
            audit.UserId = SharedVariables.LoggedInUser.UserId;
            audit.CreatedOn = DateTime.Now;
            unitOfWork.AuditLogsRepository.Insert(audit);
        }
        
        public static void CreateAuditLog(UnitOfWork unitOfWork, AuditModule module, AuditAction action)
        {
            AuditLog audit = new AuditLog();
            audit.ActionId = (int)module;
            audit.Description = module.ToString().Replace("_", " ") + " " + action.ToString().Replace("_", " ") + ".";
            audit.UserId = SharedVariables.LoggedInUser.UserId;
            audit.CreatedOn = DateTime.Now;
            unitOfWork.AuditLogsRepository.Insert(audit);
        }

        public static void CreateAuditLog(UnitOfWork unitOfWork, List<string> names, AuditModule module, AuditAction action)
        {
            foreach (var name in names)
            {
                AuditLog audit = new AuditLog();
                audit.ActionId = (int)module;
                audit.Description = module.ToString().Replace("_", " ") + " '" + name + "' " + action.ToString().Replace("_", " ") + ".";
                audit.UserId = SharedVariables.LoggedInUser.UserId;
                audit.CreatedOn = DateTime.Now;
                unitOfWork.AuditLogsRepository.Insert(audit);
            }
        }
    }

    public enum AuditModule
    {
        User = 1,
        Patient = 2,
        Category = 3,
        Supplier = 4,
        Item = 5,
        Purchase_Order = 6,
        Stock = 7,
        Stock_Adjustment = 8,
        Stock_Consumption = 9,
        Expense = 10,
        Invoice = 11,
        Store = 12,
        Stock_Return = 13,
        Rack = 14,
        Manufacturer = 15,
        Store_Closing = 16,
        POS_Closing = 17,
        Logged_In = 18,
        Logged_Out = 19,
    }
    public enum AuditAction
    {
        created,
        updated,
        deleted,
        refunded,
        paid,
        logged_in,
        logged_out,
        Restore_invoice,
    }

}
