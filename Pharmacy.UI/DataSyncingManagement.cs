using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Pharmacy.UI
{

    public static class TreeViewExtensions
    {
        public static void AddJsonToTreeView(this TreeView treeView, string jsonString)
        {
            try
            {
                JToken token = JToken.Parse(jsonString);

                if (token != null)
                {
                    CreateTreeNode(token, treeView.Nodes);
                }
            }
            catch (Exception ex)
            {
                // Handle JSON parsing exception here
                Console.WriteLine("An error occurred while parsing the JSON string: " + ex.Message);
            }

            
        }

        private static void CreateTreeNode(JToken token, TreeNodeCollection parentNodes)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    TreeNode objectNode = new TreeNode("Object");
                    parentNodes.Add(objectNode);

                    foreach (JProperty property in token.Children<JProperty>())
                    {
                        TreeNode propertyNode = new TreeNode(property.Name);
                        objectNode.Nodes.Add(propertyNode);
                        CreateTreeNode(property.Value, propertyNode.Nodes);
                    }
                    break;

                case JTokenType.Array:
                    TreeNode arrayNode = new TreeNode("Array");
                    parentNodes.Add(arrayNode);

                    int index = 0;
                    foreach (JToken childToken in token.Children())
                    {
                        TreeNode arrayItemNode = new TreeNode("[" + index + "]");
                        arrayNode.Nodes.Add(arrayItemNode);
                        CreateTreeNode(childToken, arrayItemNode.Nodes);
                        index++;
                    }
                    break;

                case JTokenType.Property:
                    TreeNode propertyValueNode = new TreeNode(((JProperty)token).Name);
                    parentNodes.Add(propertyValueNode);
                    CreateTreeNode(((JProperty)token).Value, propertyValueNode.Nodes);
                    break;

                case JTokenType.Null:
                    parentNodes.Add(new TreeNode("null"));
                    break;

                default:
                    parentNodes.Add(new TreeNode(token.ToString()));
                    break;
            }
        }
    }

    public static class DataSyncingManagement
    {
        public static void StartPharmacySyncingService()
        {
            string serviceName = "PharmacySyncingService";

            // Check if the service is already running
            ServiceController service = new ServiceController(serviceName);
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                // Check if the current user has administrative privileges
                if (!IsUserAdministrator())
                {
                    MessageBox.Show("This operation requires administrative privileges.Please Run Application as Administrator to Perform this Action");
                    return;
                }

                // Start the service
                try
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                    MessageBox.Show("Service started successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error starting the service: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Service is already running.");
            }
        }
        public static void StopPharmacySyncingService()
        {
            string serviceName = "PharmacySyncingService";

            // Check if the service is already running
            ServiceController service = new ServiceController(serviceName);
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                // Check if the current user has administrative privileges
                if (!IsUserAdministrator())
                {
                    MessageBox.Show("This operation requires administrative privileges.Please Run Application as Administrator to Perform this Action");
                    return;
                }

                // Start the service
                try
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                    MessageBox.Show("Service started successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error starting the service: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Service is already running.");
            }
        }
        public static bool GetStatusPharmacySyncingService()
        {
            string serviceName = "PharmacySyncingService";

            // Check if the service is already running
            ServiceController service = new ServiceController(serviceName);
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        

        private static bool IsUserAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        private static async Task<DataTable> GetDataAsync(bool st)
        {
            if (st)
            {
                return await Task.Run(() =>
                {
                    DataTable dt = ProductActivation.get("select OfflineTaskId ,ModuleJson as Data, CreatedOn as Date , ErrorResponse , ErrorSensitivity , IsActive as Synced from OfflineTasks");
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                });
            }
            else
            {
                return await Task.Run(() =>
                {
                    DataTable dt = ProductActivation.get("select OfflineTaskId ,ModuleJson as Data, CreatedOn as Date , ErrorResponse , ErrorSensitivity , IsActive as Synced from OfflineTasks where IsActive=1");
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                });
            }
        }

        public static async void SetDataSource(DataGridView dgv ,  bool st)
        {
            DataTable data = await GetDataAsync(st);
            if(data == null)
            {
                return;
            }
            dgv.DataSource = data;

            dgv.Columns[0].Visible = false;
            //dgv.Columns[2].Visible = false;
            dgv.Columns[1].MinimumWidth = 600;
            dgv.Columns[3].ReadOnly = false;

            DataGridViewButtonColumn buttonView = new DataGridViewButtonColumn();
            buttonView.Name = "ActionView";
            buttonView.HeaderText = "Action";
            buttonView.Text = "View Data";
            buttonView.UseColumnTextForButtonValue = true;

            dgv.Columns.Add(buttonView);

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Set full row selection
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Disable multi-select
            dgv.MultiSelect = false;
        }
    }

}
