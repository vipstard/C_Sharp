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

            //SharpPcap
            var deviceIp = LibPcapLiveDeviceList.Instance;
            
            //foreach (var dev in deviceIp)
            //{
            //    //Console.WriteLine(dev);
            //    Console.WriteLine($"{dev.Name} /  {dev.Description}  /  {(dev.Addresses.Count != 0 ? dev.Addresses[1].Addr:"NULL")}  / {dev.MacAddress}  \n");
            //}
            Console.WriteLine("=================================");

            //NetworkInterface
            NetworkInterface[] nicArray = NetworkInterface.GetAllNetworkInterfaces();
            //foreach (var nic in nicArray)
            //{

            //    Console.WriteLine($"{nic.GetPhysicalAddress()}");
            //            //Console.WriteLine("{0} / {1} / {2} ", nic.Name, nic.Description, nic.NetworkInterfaceType.ToString());



            //    }

            //CaptureDeviceList devices = CaptureDeviceList.Instance;
            //foreach (ICaptureDevice dev in devices)
            //    Console.WriteLine("{0} \n", dev.ToString());

            //////////////////////////
            String diretory = "C:\\nms4sa\\database\\";
            StringBuilder strBuilder = new StringBuilder(diretory);
            strBuilder.Append("iec61850_Server_Scl");
            Console.WriteLine(strBuilder);
        }

    }
}
