using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.NetworkInformation;
namespace Pharmacy.UI
{
    public class Security
    {
        public static long GetSerailNo()
        {
            //ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            string mac = "";
            //ManagementObjectCollection moc = mc.GetInstances();
            //foreach (ManagementObject mo in moc)
            //{
            //    if (Convert.ToBoolean(mo["IpEnabled"]))
            //    {
            //        mac = mo["MacAddress"].ToString();
            //    }
            //}
            mac = NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Where(nic => (nic.OperationalStatus == OperationalStatus.Up || nic.OperationalStatus == OperationalStatus.Down) && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .Select(nic => nic.GetPhysicalAddress().ToString())
                    .FirstOrDefault();
            long sum = 0;
            int index = 1;
            foreach (char c in mac)
            {
                if (char.IsDigit(c))
                {
                    sum += (char)c * (index * 6);
                }
                else if (char.IsLetter(c))
                {
                    switch (((char)c).ToString().ToUpper())
                    {
                        case "A":
                            sum += 10;
                            break;
                        case "B":
                            sum += 11;
                            break;
                        case "C":
                            sum += 12;
                            break;
                        case "D":
                            sum += 13;
                            break;
                        case "E":
                            sum += 14;
                            break;
                        case "F":
                            sum += 15;
                            break;
                    }
                }
            }
            return sum;
        }

        public static bool CheckKey(long Key)
        {
            long Serial = GetSerailNo();
            long Result = ((Serial * 5) + 53 / 4) * (Serial / 53) - 2;
            if (Result == Key)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static long GenerateKey(long Serial)
        {
            return ((Serial * 5) + 53 / 4) * (Serial / 53) - 2;
        }
    }
}