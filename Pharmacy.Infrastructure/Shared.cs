using Pharmacy.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure
{
    public class Shared
    {
        //public static string ConnectionString = ConfigurationManager.ConnectionStrings["PharmacyCon"].ToString();
        public static string GetConnectionString()
        {

            string connection = @"Data Source=(LocalDb)\v11.0;AttachDbFilename=E:\RAW DATA\Project\BELINK RMS\Pharmacy.UI\DBFile\Pharmacy.mdf;Integrated Security=True";
            if(SharedVariables.UseLocalDb)
            {
                return ConfigurationManager.ConnectionStrings["PharmacyCon_Local"].ConnectionString;
            }
            //string EncryptedConStr = ConfigurationManager.ConnectionStrings["PharmacyCon"].ConnectionString;            
            //EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(EncryptedConStr);
            //string EncryptedConStr = System.Environment.GetEnvironmentVariable("PharmacyConnString");
            //SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(EncryptedConStr);
            
            //string InitialCatalog = CryptopEngine.Decrypt(sqlBuilder.InitialCatalog, GlobalSharing.Salt);
            //string UserId = CryptopEngine.Decrypt(sqlBuilder.UserID, GlobalSharing.Salt);
            //string Password = CryptopEngine.Decrypt(sqlBuilder.Password, GlobalSharing.Salt);
            ////string DataSource = Utilities.CryptopEngine.Decrypt(sqlBuilder.DataSource, Shared.Salt);
            ////string DataSource = "192.168.10.4,1433"; // use this connection string in a build where remote sql access is required.
            ////string DataSource = Environment.MachineName.ToString() + @"\SQLEXPRESS";
            //string DataSource =  sqlBuilder.DataSource;
            //sqlBuilder.InitialCatalog = InitialCatalog;
            //sqlBuilder.UserID = UserId;
            //sqlBuilder.Password = Password;
            //sqlBuilder.DataSource = DataSource;
            //string Decrypted = sqlBuilder.ConnectionString.ToString();



            //string EncryptedConStr = System.Environment.GetEnvironmentVariable("PharmacyConnString");
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(connection);

            string DataSourc = sqlBuilder.DataSource;
            //string UserId = CryptopEngine.Decrypt(sqlBuilder.UserID, GlobalSharing.Salt);
            //string Password = CryptopEngine.Decrypt(sqlBuilder.Password, GlobalSharing.Salt);
            //string DataSource = Utilities.CryptopEngine.Decrypt(sqlBuilder.DataSource, Shared.Salt);
            //string DataSource = "192.168.10.4,1433"; // use this connection string in a build where remote sql access is required.
            //string DataSource = Environment.MachineName.ToString() + @"\SQLEXPRESS";
            string AttachDBFilename = sqlBuilder.AttachDBFilename;
            sqlBuilder.DataSource = DataSourc;
            sqlBuilder.IntegratedSecurity = sqlBuilder.IntegratedSecurity;
            //sqlBuilder.Password = Password;
            sqlBuilder.AttachDBFilename = AttachDBFilename;
            
            string Decrypted = sqlBuilder.ConnectionString.ToString();



            // entityBuilder.ProviderConnectionString = sqlBuilder.ConnectsionString;
            //return entityBuilder.ToString();
            return sqlBuilder.ConnectionString;
        }
        public static string GetConnectionString1()
        {
            if (SharedVariables.UseLocalDb)
            {
                return ConfigurationManager.ConnectionStrings["PharmacyCon_Local"].ConnectionString;
            }
            //string EncryptedConStr = ConfigurationManager.ConnectionStrings["PharmacyCon"].ConnectionString;            
            //EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(EncryptedConStr);
            string EncryptedConStr = System.Environment.GetEnvironmentVariable("PharmacyConnString1");
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(EncryptedConStr);
            string InitialCatalog = CryptopEngine.Decrypt(sqlBuilder.InitialCatalog, GlobalSharing.Salt);
            string UserId = CryptopEngine.Decrypt(sqlBuilder.UserID, GlobalSharing.Salt);
            string Password = CryptopEngine.Decrypt(sqlBuilder.Password, GlobalSharing.Salt);
            //string DataSource = Utilities.CryptopEngine.Decrypt(sqlBuilder.DataSource, Shared.Salt);
            //string DataSource = "192.168.10.4,1433"; // use this connection string in a build where remote sql access is required.
            //string DataSource = Environment.MachineName.ToString() + @"\SQLEXPRESS";
            string DataSource = sqlBuilder.DataSource;
            sqlBuilder.InitialCatalog = InitialCatalog;
            sqlBuilder.UserID = UserId;
            sqlBuilder.Password = Password;
            sqlBuilder.DataSource = DataSource;
            string Decrypted = sqlBuilder.ConnectionString.ToString();
            // entityBuilder.ProviderConnectionString = sqlBuilder.ConnectsionString;
            //return entityBuilder.ToString();
            return sqlBuilder.ConnectionString;
        }
        public static string GetConnectionStringEncrypted()
        {
            if (SharedVariables.UseLocalDb)
            {
                return ConfigurationManager.ConnectionStrings["PharmacyCon_Local"].ConnectionString;
            }
            //string EncryptedConStr = ConfigurationManager.ConnectionStrings["PharmacyCon"].ConnectionString;            
            //EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(EncryptedConStr);
            string EncryptedConStr = System.Environment.GetEnvironmentVariable("PharmacyConnString");
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(EncryptedConStr);
            string InitialCatalog = sqlBuilder.InitialCatalog;
            string UserId = sqlBuilder.UserID;
            string Password = sqlBuilder.Password;
            //string DataSource = Utilities.CryptopEngine.Decrypt(sqlBuilder.DataSource, Shared.Salt);
            //string DataSource = "192.168.10.4,1433"; // use this connection string in a build where remote sql access is required.
            //string DataSource = Environment.MachineName.ToString() + @"\SQLEXPRESS";
            string DataSource = sqlBuilder.DataSource;
            sqlBuilder.InitialCatalog = InitialCatalog;
            sqlBuilder.UserID = UserId;
            sqlBuilder.Password = Password;
            sqlBuilder.DataSource = DataSource;
            string Decrypted = sqlBuilder.ConnectionString.ToString();
            // entityBuilder.ProviderConnectionString = sqlBuilder.ConnectsionString;
            //return entityBuilder.ToString();
            return sqlBuilder.ConnectionString;
        }
        public static SqlConnectionStringBuilder GetConnectionBuilder()
        {
            if (SharedVariables.UseLocalDb)
            {
                //return ConfigurationManager.ConnectionStrings["PharmacyCon_Local"].ConnectionString;
            }
            //string EncryptedConStr = ConfigurationManager.ConnectionStrings["PharmacyCon"].ConnectionString;            
            //EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(EncryptedConStr);
            string EncryptedConStr = System.Environment.GetEnvironmentVariable("PharmacyConnString");
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(EncryptedConStr);
            string InitialCatalog = CryptopEngine.Decrypt(sqlBuilder.InitialCatalog, GlobalSharing.Salt);
            string UserId = CryptopEngine.Decrypt(sqlBuilder.UserID, GlobalSharing.Salt);
            string Password = CryptopEngine.Decrypt(sqlBuilder.Password, GlobalSharing.Salt);
            //string DataSource = Utilities.CryptopEngine.Decrypt(sqlBuilder.DataSource, Shared.Salt);
            //string DataSource = "192.168.10.4,1433"; // use this connection string in a build where remote sql access is required.
            //string DataSource = Environment.MachineName.ToString() + @"\SQLEXPRESS";
            string DataSource = sqlBuilder.DataSource;
            sqlBuilder.InitialCatalog = InitialCatalog;
            sqlBuilder.UserID = UserId;
            sqlBuilder.Password = Password;
            sqlBuilder.DataSource = DataSource;
            string Decrypted = sqlBuilder.ConnectionString.ToString();
            // entityBuilder.ProviderConnectionString = sqlBuilder.ConnectsionString;
            //return entityBuilder.ToString();
            return sqlBuilder;
        }
        //Fatima Clinic & Maternity connection string special case     
        public void MakeItemSyncable(UnitOfWork unitOfWork, int ItemId)
        {
            unitOfWork.ItemRspository.GetById(ItemId);
        }
    }

}

