using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Pharmacy.Sync.MappedEntities;
using Pharmacy.Sync.Wrapper;
using Pharmacy.Core;

namespace Pharmacy.Sync.Utility
{
    public class Service<T>
    {
        public string mainURL = "";
        public Service(int urlType)
        {
            //if (Shared.IsTesting)
            //{
            //    mainURL = "https://api.healthwire.pk";                
            //    //mainURL = "http://192.168.40.193:3000/";

            //    //mainURL = "https://healthwire.pk";
            //}
            //else
            //{
            //    mainURL = "https://api.healthwire.pk/";
            //}
            mainURL = SharedVariables.API_serverPath;
            switch (urlType)
            {
                case 1: Url = CreateCategoriesURL; break;
                case 11: Url = UpdateCategoriesURL; break;
                case 111: Url = DeleteCategoriesURL; break;

                case 2: Url = CreateSuppliersURL; break;
                case 22: Url = UpdateSuppliersURL; break;
                case 222: Url = DeleteSuppliersURL; break;

                case 3: Url = CreateItemsURL; break;
                case 33: Url = UpdateItemsURL; break;
                case 333: Url = DeleteItemsURL; break;

                case 4: Url = CreateBatchesURL; break;
                case 44: Url = UpdateBatchesURL; break;

                case 5: Url = CreatePatientsURL; break;
                case 55: Url = UpdatePatientsURL; break;
                case 555: Url = DeletePatientsURL; break;

                case 6: Url = CreateInvoicesURL; break;
                case 66: Url = UpdateInvoicesURL; break;
                case 666: Url = DeleteInvoicesURL; break;

                case 7: Url = CreateInvoicePaymentsURL; break;
                case 77: Url = UpdateInvoicePaymentsURL; break;
                case 777: Url = DeleteInvoicePaymentsURL; break;

                case 8: Url = CreateStocksURL; break;
                case 88: Url = UpdateStocksURL; break;
                case 888: Url = DeleteStocksURL; break;

                case 9: Url = CreateExpensesURL; break;
                case 99: Url = UpdateExpensesURL; break;
                case 999: Url = DeleteExpensesURL; break;

                case 1001: Url = CreateAdjustmentsURL; break;
                case 1002: Url = UpdateAdjustmentsURL; break;
                case 1003: Url = DeleteAdjustmentsURL; break;

                case 1004: Url = CreatePurchaseOrdersURL; break;
                case 1005: Url = UpdatePurchaseOrdersURL; break;
                case 1006: Url = DeletePurchaseOrdersURL; break;

                case 1007: Url = CreateStockConsumptionsURL; break;
                case 1008: Url = UpdateStockConsumptionsURL; break;
                case 1009: Url = DeleteStockConsumptionsURL; break;

                case 1010: Url = CreateUsersURL; break;
                case 1011: Url = UpdateUsersURL; break;
                case 1012: Url = DeleteUsersURL; break;
            }
        }

        public string loginURL = "/api/v4/users/login/";
        public string AddReportURL = "/api/v2/users/update-results";

        private string CreateCategoriesURL = "api/v4/item_classifications.json";
        private string UpdateCategoriesURL = "api/v4/item_classifications/update-classifications.json";
        private string DeleteCategoriesURL = "api/v4/item_classifications/delete-classifications.json";

        private string CreateSuppliersURL = "api/v4/inventory_suppliers.json";
        private string UpdateSuppliersURL = "api/v4/inventory_suppliers/update-suppliers.json";
        private string DeleteSuppliersURL = "api/v4/inventory_suppliers/delete-suppliers";

        private string CreateItemsURL = "api/v4/items.json";
        private string UpdateItemsURL = "api/v4/items/update-items.json";
        private string DeleteItemsURL = "api/v4/items/delete-items.json";

        private string CreateBatchesURL = "api/v4/batches.json";
        private string UpdateBatchesURL = "api/v4/batches/update-batches.json";

        private string CreatePatientsURL = "api/v4/patients.json";
        private string UpdatePatientsURL = "api/v4/patients/update-patients.json";
        private string DeletePatientsURL = "api/v4/patients/delete-patients.json";

        private string CreateStocksURL = "api/v4/stocks.json";
        private string UpdateStocksURL = "api/v4/stocks/update-stocks.json";
        private string DeleteStocksURL = "api/v4/stocks/delete-stocks.json";

        private string CreateExpensesURL = "api/v4/expenses.json";
        private string UpdateExpensesURL = "api/v4/expenses/update-expenses.json";
        private string DeleteExpensesURL = "api/v4/expenses/delete-expenses.json";

        private string CreateInvoicesURL = "api/v4/invoices.json";
        private string UpdateInvoicesURL = "api/v4/invoices/update-invoices.json";
        private string DeleteInvoicesURL = "api/v4/invoices/delete-invoices.json";

        private string CreateInvoicePaymentsURL = "api/v4/invoice_payments.json";
        private string UpdateInvoicePaymentsURL = "api/v4/invoice_payments/update-payments.json";
        private string DeleteInvoicePaymentsURL = "api/v4/invoice_payments/delete-payments.json";

        private string CreateAdjustmentsURL = "api/v4/adjustments.json";
        private string UpdateAdjustmentsURL = "api/v4/adjustments/update-adjustments.json";
        private string DeleteAdjustmentsURL = "api/v4/adjustments/delete-adjustments.json";

        private string CreatePurchaseOrdersURL = "api/v4/orders.json";
        private string UpdatePurchaseOrdersURL = "api/v4/orders/update-orders.json";
        private string DeletePurchaseOrdersURL = "api/v4/orders/delete-orders.json";

        private string CreateStockConsumptionsURL = "api/v4/consume_stocks.json";
        private string UpdateStockConsumptionsURL = "api/v4/consume_stocks/update-consume-stocks.json";
        private string DeleteStockConsumptionsURL = "api/v4/consume_stocks/delete-consume-stocks.json";

        private string CreateUsersURL = "api/v4/users.json";
        private string UpdateUsersURL = "api/v4/users/update-users.json";
        private string DeleteUsersURL = "api/v4/users/delete-users.json";

        private string Url = "";

        public string Login(string email, string password)
        {
            try
            {
                string resultContent;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(mainURL);
                    var content = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string, string>("user[email]", email),
                new KeyValuePair<string, string>("user[password]", password)
                });
                    var result = client.PostAsync(loginURL, content).Result;
                    resultContent = result.Content.ReadAsStringAsync().Result;
                    return resultContent;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed");
            }
        }

        //public string Create_Categories(CategoriesWrapper obj)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(mainURL);
        //            client.DefaultRequestHeaders.Add("x-user-email", Shared.LoggedInEmail);
        //            client.DefaultRequestHeaders.Add("x-user-token", Shared.LoggedInToken);
        //            //HttpContent content = new FormUrlEncodedContent(new[]{
        //            // new KeyValuePair<string, string>("practice_id", vm.practice_id),
        //            // new KeyValuePair<string, laboratory_results>("laboratory_results", lr),
        //            // new KeyValuePair<string, string>("employee[machine_serial_no]", emp.Machine_Serial_No),
        //            // //new KeyValuePair<string, string>("employee[gender]", emp.Gender.ToString()),
        //            // //new KeyValuePair<string, string>("employee[cnic]", emp.CNIC),
        //            // });

        //            string data = Newtonsoft.Json.JsonConvert.SerializeObject(Categories);
        //            HttpContent content = new StringContent(data);

        //            HttpResponseMessage result = new HttpResponseMessage();
        //            result = client.PostAsJsonAsync(CreateCategoriesURL, Categories).Result;
        //            string res = result.Content.ReadAsStringAsync().Result;
        //            return res;
        //        }
        //    }
        //    catch { return null; }
        //}
        //public string Update_Categories(CategoriesWrapper obj)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(mainURL);
        //            client.DefaultRequestHeaders.Add("x-user-email", Shared.LoggedInEmail);
        //            client.DefaultRequestHeaders.Add("x-user-token", Shared.LoggedInToken);

        //            string data = Newtonsoft.Json.JsonConvert.SerializeObject(Categories);
        //            HttpContent content = new StringContent(data);

        //            HttpResponseMessage result = new HttpResponseMessage();
        //            result = client.PutAsJsonAsync(UpdateCategoriesURL, Categories).Result;
        //            string res = result.Content.ReadAsStringAsync().Result;
        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
        //public string Delete_Categories(CategoriesWrapper obj)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(mainURL);
        //            client.DefaultRequestHeaders.Add("x-user-email", Shared.LoggedInEmail);
        //            client.DefaultRequestHeaders.Add("x-user-token", Shared.LoggedInToken);

        //            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //            HttpContent content = new StringContent(data);

        //            HttpResponseMessage result = new HttpResponseMessage();
        //            result = client.PostAsJsonAsync(DeleteCategoriesURL, obj).Result;
        //            string res = result.Content.ReadAsStringAsync().Result;
        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}



        //public string Create_Suppliers(SupplierWrapper obj)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(mainURL);
        //            client.DefaultRequestHeaders.Add("x-user-email", Shared.LoggedInEmail);
        //            client.DefaultRequestHeaders.Add("x-user-token", Shared.LoggedInToken);
        //            //HttpContent content = new FormUrlEncodedContent(new[]{
        //            // new KeyValuePair<string, string>("practice_id", vm.practice_id),
        //            // new KeyValuePair<string, laboratory_results>("laboratory_results", lr),
        //            // new KeyValuePair<string, string>("employee[machine_serial_no]", emp.Machine_Serial_No),
        //            // //new KeyValuePair<string, string>("employee[gender]", emp.Gender.ToString()),
        //            // //new KeyValuePair<string, string>("employee[cnic]", emp.CNIC),
        //            // });

        //            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //            HttpContent content = new StringContent(data);

        //            HttpResponseMessage result = new HttpResponseMessage();
        //            result = client.PostAsJsonAsync(CreateSuppliersURL, obj).Result;
        //            string res = result.Content.ReadAsStringAsync().Result;
        //            return res;
        //        }
        //    }
        //    catch { return null; }
        //}
        //public string Update_Suppliers(SupplierWrapper obj)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(mainURL);
        //            client.DefaultRequestHeaders.Add("x-user-email", Shared.LoggedInEmail);
        //            client.DefaultRequestHeaders.Add("x-user-token", Shared.LoggedInToken);

        //            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //            HttpContent content = new StringContent(data);

        //            HttpResponseMessage result = new HttpResponseMessage();
        //            result = client.PutAsJsonAsync(UpdateSuppliersURL, obj).Result;
        //            string res = result.Content.ReadAsStringAsync().Result;
        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
        //public string Delete_Suppliers(DeletionWrapper obj)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(mainURL);
        //            client.DefaultRequestHeaders.Add("x-user-email", Shared.LoggedInEmail);
        //            client.DefaultRequestHeaders.Add("x-user-token", Shared.LoggedInToken);

        //            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //            HttpContent content = new StringContent(data);

        //            HttpResponseMessage result = new HttpResponseMessage();
        //            result = client.PostAsJsonAsync(DeleteCategoriesURL, obj).Result;
        //            string res = result.Content.ReadAsStringAsync().Result;
        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}



        //public string Create_Items(ItemsWrapper obj)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(mainURL);
        //            client.DefaultRequestHeaders.Add("x-user-email", Shared.LoggedInEmail);
        //            client.DefaultRequestHeaders.Add("x-user-token", Shared.LoggedInToken);                  

        //            //string data = Newtonsoft.Json.JsonConvert.SerializeObject(Item);
        //            //HttpContent content = new StringContent(data);

        //            HttpResponseMessage result = new HttpResponseMessage();
        //            result = client.PostAsJsonAsync(CreateItemsURL, Item).Result;
        //            return result.Content.ReadAsStringAsync().Result;
        //        }
        //    }
        //    catch { return null; }
        //}
        //public string Update_Items(ItemsWrapper obj)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(mainURL);
        //            client.DefaultRequestHeaders.Add("x-user-email", Shared.LoggedInEmail);
        //            client.DefaultRequestHeaders.Add("x-user-token", Shared.LoggedInToken);

        //            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //            HttpContent content = new StringContent(data);

        //            HttpResponseMessage result = new HttpResponseMessage();
        //            result = client.PutAsJsonAsync(UpdateItemsURL, obj).Result;
        //            return result.Content.ReadAsStringAsync().Result;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
        //public string Delete_Items(DeletionWrapper obj)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(mainURL);
        //            client.DefaultRequestHeaders.Add("x-user-email", Shared.LoggedInEmail);
        //            client.DefaultRequestHeaders.Add("x-user-token", Shared.LoggedInToken);

        //            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //            HttpContent content = new StringContent(data);

        //            HttpResponseMessage result = new HttpResponseMessage();
        //            result = client.PostAsJsonAsync(UpdateItemsURL, obj).Result;
        //            return result.Content.ReadAsStringAsync().Result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        //public string Create_Items(ItemsWrapper obj)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(mainURL);
        //            client.DefaultRequestHeaders.Add("x-user-email", Shared.LoggedInEmail);
        //            client.DefaultRequestHeaders.Add("x-user-token", Shared.LoggedInToken);

        //            //string data = Newtonsoft.Json.JsonConvert.SerializeObject(Item);
        //            //HttpContent content = new StringContent(data);

        //            HttpResponseMessage result = new HttpResponseMessage();
        //            result = client.PostAsJsonAsync(CreateItemsURL, Item).Result;
        //            return result.Content.ReadAsStringAsync().Result;
        //        }
        //    }
        //    catch { return null; }
        //}
        //public string Update_Items(ItemsWrapper obj)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(mainURL);
        //            client.DefaultRequestHeaders.Add("x-user-email", Shared.LoggedInEmail);
        //            client.DefaultRequestHeaders.Add("x-user-token", Shared.LoggedInToken);

        //            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //            HttpContent content = new StringContent(data);

        //            HttpResponseMessage result = new HttpResponseMessage();
        //            result = client.PutAsJsonAsync(UpdateItemsURL, obj).Result;
        //            return result.Content.ReadAsStringAsync().Result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
        //public string Delete_Items(DeletionWrapper obj)
        //{
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(mainURL);
        //            client.DefaultRequestHeaders.Add("x-user-email", Shared.LoggedInEmail);
        //            client.DefaultRequestHeaders.Add("x-user-token", Shared.LoggedInToken);

        //            string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //            HttpContent content = new StringContent(data);

        //            HttpResponseMessage result = new HttpResponseMessage();
        //            result = client.PostAsJsonAsync(UpdateItemsURL, obj).Result;
        //            return result.Content.ReadAsStringAsync().Result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        public string Create_Data(T obj)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(mainURL);
                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.API_Email);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.API_Token);

                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    HttpContent content = new StringContent(data);

                    HttpResponseMessage result = new HttpResponseMessage();
                    result = client.PostAsJsonAsync(Url, obj).Result;
                    return result.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Update_Data(T obj)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(mainURL);
                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.API_Email);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.API_Token);

                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    HttpContent content = new StringContent(data);

                    HttpResponseMessage result = new HttpResponseMessage();
                    result = client.PutAsJsonAsync(Url, obj).Result;
                    return result.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Delete_Data(T obj)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(mainURL);
                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.API_Email);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.API_Token);

                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    HttpContent content = new StringContent(data);

                    HttpResponseMessage result = new HttpResponseMessage();
                    result = client.PostAsJsonAsync(Url, obj).Result;
                    return result.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}