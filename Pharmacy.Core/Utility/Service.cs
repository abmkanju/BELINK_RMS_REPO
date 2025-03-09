using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Pharmacy.Core;
using System.IO;
using System.Net.Http.Headers;
using System.Net;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Pharmacy.Core.Entities;
using Pharmacy.Core.Interfaces;
using Pharmacy.Core.Utility;
using Pharmacy.Core.ServiceModel;
using Pharmacy.Core.Wrapper;
using Newtonsoft.Json;
using Pharmacy.Core.ViewModels;
using System.Data.SqlClient;
using Serilog;

namespace Pharmacy.Core.Utility
{
    public class Service
    {
        public string mainURL = "";
        public string awsURL = "";
        Serilog.Core.Logger logger;

        public Service()
        {
            if (SharedVariables.IsTesting)
            {
                mainURL = "https://api.healthwire.pk"; //https://api.healthwire.pk
                awsURL = "https://okd9cexpmh.execute-api.us-east-1.amazonaws.com";
            }
            else
            {
                mainURL = "https://api.healthwire.pk";// "https://elastic.healthwire.pk";// "https://clientfixes.healthwire.pk";//"https://staging.healthwire.pk"; //"https://design.healthwire.pk"; // "https://desktop.healthwire.pk/"; //"https://api.healthwire.pk"; //"https://lookup.healthwire.pk";
                awsURL = "https://okd9cexpmh.execute-api.us-east-1.amazonaws.com";
            }
        }

        public string loginURL = "/api/v4/users/login.json";
        public string GetItemsData = "/default/";
        public string SendVoucherMail = "/api/v4/orders/sent-voucher-payment-email";
        public string SendEmailUri = "/api/v2/users/update-results/";
        public string SendPatientInfo = "/api/v4/patients/send-message";
        public string AllowItemApproval = "/api/v4/items/franchise-requested-items";
        public string pandaGo = "/api/v4/orders/verify-token";
        public string SendDispatch = "/api/v4/orders/dispatch-message";
        public string checkDuplicteItem = "/api/v4/items/check-duplicate-item";
        public string GetOrderDataApi = "/api/v4/pharmacy/orders/get-orders.json";
        public string GetOrderCountApi = "/api/v4/pharmacy/orders/check-orders-count";
        public string GetItemsDataApi = "/api/v4/pharmacy/items/check-requested-items";
        public string CancelOrder = "/api/v4/orders/cancel-food-panda-order";

        public string Login()
        {
            string resultContent;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(mainURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var content = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string, string>("user[email]", SharedVariables.AdminPharmacySetting.OnlineWebUsername),
                new KeyValuePair<string, string>("user[password]", SharedVariables.AdminPharmacySetting.OnlineWebPassword)
                });
                var result = client.PostAsync(loginURL, content).Result;
                resultContent = result.Content.ReadAsStringAsync().Result;
                return resultContent;
            }
        }

        public void getCredential()
        {
            try
            {
                string jsonLogin = Login();
                Login login = new Login();
                login = JsonConvert.DeserializeObject<Login>(jsonLogin);
                if (login.status.ToLower().Equals("success"))
                {
                    SharedVariables.Email = SharedVariables.AdminPharmacySetting.OnlineWebUsername;
                    SharedVariables.AuthToken = login.auth_token;
                }
                else
                {
                    SharedVariables.Email = "";
                    SharedVariables.AuthToken = "";
                }
            }
            catch (Exception ex)
            {
                logger.Error("Login issue Token: " + ex.ToString());
            }
        }

        public void startService()
        {
            //logger start
            logger = new LoggerConfiguration()
               .WriteTo
               .File(SharedVariables.FilePath)
               .CreateLogger();
            logger.Information("Service started");
        }

        public Response hitPandaGo(int orderID)
        {
            startService();
            logger.Information("Pandago checking");
            HttpResponseMessage result = new HttpResponseMessage();
            Response checkPanda = new Response();
            string response = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(mainURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.AdminPharmacySetting.OnlineWebUsername);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.AuthToken);

                    result = client.PostAsJsonAsync(pandaGo + "?practice_id=" + SharedVariables.AdminPharmacySetting.PracticeId + "&order_id=" + orderID, "").Result;
                    response = result.Content.ReadAsStringAsync().Result;

                    checkPanda = JsonConvert.DeserializeObject<Response>(response);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Panda Go issue: " + ex.ToString());
            }
            return checkPanda;
        }

        public Response SendDispatchMsg(string custName, string Address, string Phone, string orderValue)
        {
            //logger.Information("Send Dispatch message");
            HttpResponseMessage result = new HttpResponseMessage();
            Response checkdisptch = new Response();
            string response = "";
            try
            {
                using (var client = new HttpClient())
                {
                    DispatchMsg msg = new DispatchMsg();
                    msg.order_id = orderValue;
                    msg.user_name = custName;
                    msg.phone = Phone;

                    client.BaseAddress = new Uri(mainURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.AdminPharmacySetting.OnlineWebUsername);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.AuthToken);

                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
                    HttpContent content = new StringContent(data);

                    result = client.PostAsJsonAsync(SendDispatch, msg).Result;
                    response = result.Content.ReadAsStringAsync().Result;

                    checkdisptch = JsonConvert.DeserializeObject<Response>(response);
                }
            }
            catch (Exception ex)
            {
                //logger.Error("Send Message issue: " + ex.ToString());                
            }
            return checkdisptch;
        }

        public Response SendVoucherEmail(string voucher)
        {
            getCredential();
            startService();
            logger.Information("Send voucher message");
            HttpResponseMessage result = new HttpResponseMessage();
            Response checkdisptch = new Response();
            string response = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(mainURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.AdminPharmacySetting.OnlineWebUsername);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.AuthToken);

                    result = client.PostAsJsonAsync(SendVoucherMail + "?voucher_email=" + voucher, "").Result;
                    response = result.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Send Voucher message: " + ex.ToString());
            }
            return checkdisptch;
        }

        public Response SendCancelMsg(int OrderId, string cancelReason)
        {
            getCredential();
            HttpResponseMessage result = new HttpResponseMessage();
            Response checkCancel = new Response();
            string response = "";
            try
            {
                using (var client = new HttpClient())
                {
                    DispatchMsg msg = new DispatchMsg();
                    msg.order_id = OrderId.ToString();

                    client.BaseAddress = new Uri(mainURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.AdminPharmacySetting.OnlineWebUsername);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.AuthToken);

                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
                    HttpContent content = new StringContent(data);

                    result = client.PostAsJsonAsync(CancelOrder + "?order_id=" + OrderId + "&practice_id=" + SharedVariables.AdminPharmacySetting.PracticeId + "&cancel_reason=" + cancelReason, "").Result;
                    response = result.Content.ReadAsStringAsync().Result;

                    checkCancel = JsonConvert.DeserializeObject<Response>(response);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Cancel Issue: " + ex.ToString());
            }
            return checkCancel;
        }

        public string PostDataInvoice(string PhoneNumber, string PatientName)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            string response = "";
            try
            {
                using (var client = new HttpClient())
                {
                    Patient pat = new Patient();
                    pat.Name = PatientName;
                    pat.Phone = PhoneNumber;

                    client.BaseAddress = new Uri(mainURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.AdminPharmacySetting.OnlineWebUsername);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.AuthToken);

                    PatientWrapper service = new PatientWrapper();
                    m_patient obj = ObjectMapper.MapSavePatient(pat);
                    PatientWrapper wrapper = new PatientWrapper();
                    wrapper.patients = obj;

                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(wrapper);
                    HttpContent content = new StringContent(data);

                    result = client.PostAsJsonAsync(SendPatientInfo, wrapper).Result;
                    response = result.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Invoice Message issue: " + ex.ToString());
            }
            return response;
        }


        public int GetOrderItemsCount()
        {
            getCredential();
            startService();
            logger.Information("Get Order Item Count");
            HttpResponseMessage result = new HttpResponseMessage();
            OrderAws checkOrder1 = new OrderAws();
            string response = "";
            int Count = 0;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(mainURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.AdminPharmacySetting.OnlineWebUsername);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.AuthToken);

                    result = client.PostAsJsonAsync(GetOrderCountApi + "?practice_id=" + SharedVariables.AdminPharmacySetting.PracticeId, "").Result;
                    response = result.Content.ReadAsStringAsync().Result;

                    int numericValue;
                    bool isNumber = int.TryParse(response, out numericValue);

                    if (isNumber == true)
                    {
                        Count = Convert.ToInt32(response);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Order Count Issue: " + ex.ToString());
            }
            return Count;
        }

        //public List<OrderOnlineAws> GetOrderItems()
        //{
        //    startService();
        //    logger.Information("Get Order Items");
        //    HttpResponseMessage result = new HttpResponseMessage();
        //    //List<OrderOnlineAws> checkOrder = new List<OrderOnlineAws>();
        //    List<OrderOnlineAws> checkOrder1 = new List<OrderOnlineAws>();

        //    string response = "";
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            AwsQuery aws = new AwsQuery();
        //            aws.query = "select staging.orders.id as order_id, orders.inventory_supplier_id,orders.order_number,orders.created_at order_created_at,orders.updated_at order_updated_at,orders.order_grand_total,orders.which_type,orders.order_product_total,orders.product_discount,orders.payment_method,order_items.id as order_item_id,order_items.item_id,order_items.order_id,order_items.quantity,order_items.unit_cost,order_items.total_amount,order_items.created_at as order_item_created_at,order_items.updated_at as order_item_updated_at,order_items.sale_price,order_items.unit,order_items.order_item_type,order_item_status,items.item_name as variant,locations.name as customer_name,locations.address as customer_address, locations.phone as customer_phone, items.conversion_unit from staging.orders inner join staging.order_items ON staging.order_items.order_id = staging.orders.id left outer join staging.item_variants ON staging.item_variants.id = staging.order_items.item_variant_id left outer join staging.items ON staging.items.id = staging.order_items.item_id inner join staging.order_practices ON staging.orders.id = order_practices.order_id left outer join staging.locations on locations.locationable_type = 'Order' and locations.locationable_id = orders.id where staging.order_practices.practice_id=" + SharedVariables.AdminPharmacySetting.PracticeId + ";";
        //            //aws.query = "select staging.orders.id as order_id,orders.inventory_supplier_id,orders.order_number,orders.created_at,"
        //            //            + "order_created_at,orders.updated_at order_updated_at,orders.order_grand_total,orders.which_type,orders.order_product_total,"
        //            //            + "orders.product_discount,orders.payment_method,order_items.id as order_item_id,order_items.item_id,"
        //            //            + "order_items.order_id,order_items.quantity,order_items.unit_cost,order_items.total_amount,order_items.created_at as "
        //            //            + " order_item_created_at,order_items.updated_at as order_item_updated_at,order_items.sale_price,order_items.unit,"
        //            //            + "order_items.order_item_type,order_item_status,items.item_name as variant,locations.name as customer_name,"
        //            //            + "locations.address as customer_address, locations.phone as customer_phone, items.conversion_unit from staging.orders inner join "
        //            //            + " staging.order_items ON staging.order_items.order_id = staging.orders.id left outer  join staging.item_variants ON "
        //            //            + "staging.item_variants.id = staging.order_items.item_variant_id left outer  join staging.items ON staging.items.id = "
        //            //            + " staging.item_variants.item_id inner join staging.order_practices ON staging.orders.id = order_practices.order_id left "
        //            //            + " outer join staging.locations on locations.locationable_type = 'Order' and locations.locationable_id = orders.id where" 
        //            //            + " staging.order_practices.practice_id=1390"
        //            //            + " and trunc(staging.orders.updated_at) = current_date;";
        //            aws.query_name = "orders";
        //            aws.formatted = true;

        //            client.BaseAddress = new Uri(awsURL);
        //            ServicePointManager.Expect100Continue = true;
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //            result = client.PostAsJsonAsync(GetItemsData, aws).Result;
        //            response = result.Content.ReadAsStringAsync().Result;

        //            checkOrder1 = JsonConvert.DeserializeObject<List<OrderOnlineAws>>(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.InnerException.ToString());      
        //        throw ex;
        //    }

        //    return checkOrder1;
        //}

        public OrderOnlineAws GetOrderItems()
        {
            startService();
            logger.Information("Get Order Items");
            HttpResponseMessage result = new HttpResponseMessage();
            OrderOnlineAws checkOrder1 = new OrderOnlineAws();
            string response = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(mainURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.AdminPharmacySetting.OnlineWebUsername);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.AuthToken);

                    result = client.PostAsJsonAsync(GetOrderDataApi + "?practice_id=" + SharedVariables.AdminPharmacySetting.PracticeId, "").Result;
                    response = result.Content.ReadAsStringAsync().Result;

                    checkOrder1 = JsonConvert.DeserializeObject<OrderOnlineAws>(response);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Get Order Items Issue: " + ex.ToString());
            }
            return checkOrder1;
        }

        //public List<Item> GetItemValues()
        //{
        //    startService();
        //    logger.Information("Get Approved Items");
        //    HttpResponseMessage result = new HttpResponseMessage();
        //    ItemsAws set = new ItemsAws();
        //    List<Item> checkItem = new List<Item>();
        //    string response = "";
        //    try
        //    {
        //            using (var client = new HttpClient())
        //            {
        //                AwsQuery aws = new AwsQuery();
        //                aws.query = "select * from staging.items where trunc(staging.items.created_at) = current_date and staging.items.requested_by_id != 0;";
        //                aws.query_name = "items";

        //                client.BaseAddress = new Uri(awsURL);
        //                ServicePointManager.Expect100Continue = true;
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //                result = client.PostAsJsonAsync(GetItemsData, aws).Result;
        //                response = result.Content.ReadAsStringAsync().Result;

        //                set = JsonConvert.DeserializeObject<ItemsAws>(response);

        //                foreach (m_items1 set1 in set.data)
        //                {
        //                    if (set1.status == 1)
        //                    {
        //                        Item checkItem1 = new Item();
        //                        if (set1.status == 2) //approved
        //                        {
        //                            checkItem1.IsApproved = 0;
        //                        }
        //                        else if (set1.status == 0) //pending
        //                        {
        //                            checkItem1.IsApproved = 1;
        //                        }
        //                        else if (set1.status == 1) //rejected
        //                        {
        //                            checkItem1.IsApproved = 2;
        //                        }
        //                        checkItem1.ItemID = Convert.ToInt32(set1.desktop_id);
        //                        checkItem1.ItemName = set1.item_name;
        //                        checkItem1.ConversionUnit = Convert.ToInt32(set1.conversion_unit);
        //                        checkItem1.UnitCostPrice = Convert.ToInt32(set1.unit_cost);
        //                        checkItem1.RetailPrice = Convert.ToInt32(set1.retail_price);
        //                        checkItem1.Unit = "Pack";
        //                        checkItem1.ManufacturerId = Convert.ToInt32(set1.manufacturer_id);
        //                        checkItem1.CategoryId = Convert.ToInt32(set1.item_classification_id);
        //                        checkItem1.IsNarcotic = set1.narcotic;
        //                        checkItem1.NoOfStrips = Convert.ToInt32(set1.strips);
        //                        checkItem1.RackId = Convert.ToInt32(set1.rack);
        //                        checkItem1.RequestById = Convert.ToInt32(set1.requested_by_id);
        //                        checkItem.Add(checkItem1);
        //                    }
        //                }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.InnerException.ToString());
        //        throw ex;
        //    }
        //    return checkItem;
        //}

        public List<Item> GetItemValues()
        {
            getCredential();
            startService();
            logger.Information("Get Approved Items");
            HttpResponseMessage result = new HttpResponseMessage();
            ItemsAws set = new ItemsAws();
            List<Item> checkItem = new List<Item>();
            string response = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(mainURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.AdminPharmacySetting.OnlineWebUsername);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.AuthToken);

                    result = client.PostAsJsonAsync(GetItemsDataApi, "").Result;
                    response = result.Content.ReadAsStringAsync().Result;

                    set = JsonConvert.DeserializeObject<ItemsAws>(response);

                    foreach (m_items1 set1 in set.data)
                    {
                        if (set1.status == "approved")
                        {
                            Item checkItem1 = new Item();
                            if (set1.status == "approved") //approved
                            {
                                checkItem1.IsApproved = 0;
                            }
                            else if (set1.status == "0") //pending
                            {
                                checkItem1.IsApproved = 1;
                            }
                            else if (set1.status == "1") //rejected
                            {
                                checkItem1.IsApproved = 2;
                            }
                            checkItem1.ItemID = Convert.ToInt32(set1.desktop_id);
                            checkItem1.ItemName = set1.item_name;
                            checkItem1.ConversionUnit = Convert.ToInt32(set1.conversion_unit);
                            checkItem1.UnitCostPrice = Convert.ToInt32(set1.unit_cost);
                            checkItem1.RetailPrice = Convert.ToInt32(set1.retail_price);
                            checkItem1.Unit = "Pack";
                            checkItem1.ManufacturerId = Convert.ToInt32(set1.manufacturer_id);
                            checkItem1.CategoryId = Convert.ToInt32(set1.item_classification_id);
                            checkItem1.IsNarcotic = set1.narcotic;
                            checkItem1.NoOfStrips = Convert.ToInt32(set1.strips);
                            checkItem1.RackId = Convert.ToInt32(set1.rack);
                            checkItem1.Unique_itemId = Convert.ToInt32(set1.id);
                            checkItem1.RequestById = Convert.ToInt32(set1.requested_by_id);
                            checkItem.Add(checkItem1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Approved Items Issue: " + ex.ToString());
            }
            return checkItem;
        }

        public Response GetItemWithSameName(string itemName)
        {
            startService();
            HttpResponseMessage result = new HttpResponseMessage();
            Response checkOrder1 = new Response();
            string response = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(mainURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.AdminPharmacySetting.OnlineWebUsername);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.AuthToken);

                    result = client.PostAsJsonAsync(checkDuplicteItem + "?item_name=" + itemName, "").Result;
                    response = result.Content.ReadAsStringAsync().Result;

                    checkOrder1 = JsonConvert.DeserializeObject<Response>(response);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            return checkOrder1;
        }

        public string PostApproveItems(Item objItem)
        {
            startService();
            HttpResponseMessage result = new HttpResponseMessage();
            string response = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(mainURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    client.DefaultRequestHeaders.Add("x-user-email", SharedVariables.AdminPharmacySetting.OnlineWebUsername);
                    client.DefaultRequestHeaders.Add("x-user-token", SharedVariables.AuthToken);

                    ItemsWrapper service = new ItemsWrapper();
                    m_Item obj = ObjectMapper.MapSaveItem(objItem);
                    obj.desktop_id = 0;
                    obj.is_pharmaguide = true;
                    ItemsWrapper wrapper = new ItemsWrapper();
                    wrapper.items = new List<m_Item>() { obj };

                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(wrapper);
                    HttpContent content = new StringContent(data);

                    result = client.PostAsJsonAsync(AllowItemApproval, wrapper).Result;
                    response = result.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Post Approve Items Issue:" + ex.ToString());
            }
            return response;
        }

        public string SendLowStocckEmail(List<string> Receivers, string Token, string FilePath)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(FilePath));
                    fileContent.Headers.ContentDisposition =
                            new ContentDispositionHeaderValue("form-data") //<- 'form-data' instead of 'attachment'
                            {
                                Name = "attachment", // <- included line...
                                FileName = "Foo.txt",
                            };
                    content.Add(fileContent);
                    var resp = client.PostAsync(SendEmailUri, content).Result;
                    var res = resp.Content.ReadAsStringAsync().Result;
                    return res;
                }
            }

        }


    }
}