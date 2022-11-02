using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.LibPcap;

namespace PracticeCSharp
{
    public class Class1
    {
        public static void Main()
        {

            var devices = CaptureDeviceList.Instance;
            var deviceIp = LibPcapLiveDeviceList.Instance;
            //foreach (var dev in devices)
            //    Console.WriteLine("{0}\n", dev);

            Console.WriteLine("=================================");
            foreach (var dev in deviceIp)
            {
                Console.WriteLine("{0}\n", dev);
            }
            Console.WriteLine("=================================");

            NetworkInterface[] nicArray = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var nic in nicArray)
            {
                if(nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || nic.NetworkInterfaceType ==NetworkInterfaceType.Ethernet)
                {
                    foreach (var ip in nic.GetIPProperties().UnicastAddresses)
                    {
                        Console.WriteLine("{0} / {1} / {2} / {3}", nic.Name, nic.Description, nic.NetworkInterfaceType.ToString(), nic.Id, ip.Address.ToString());
                    }
               

                }

            }
            //foreach (var dev in deviceIp)
            //{
            //    if (dev.Addresses.Count != 0)
            //    {
            //        var ip = dev.Addresses[1].Addr.ipAddress;
            //        Console.WriteLine("ip : {0}\n", ip);
            //    }

            //}


        }
    }
}
