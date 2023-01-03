using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SCL_Parser
{
    public class LInqParser
    {
        public static void Main()
        {

            String path = "C:\\nms4sa\\database\\KEPCO_preconfigured cid file_Ver 2.2.cid";
            XDocument xd = XDocument.Load(path);
            XElement root = xd.Document.Root;

            XNamespace ns = XDocument.Load(path).Root.GetDefaultNamespace();

            var connectedAps = root
                .Descendants(ns+ "ConnectedAP")
                .Select(x => new ConnectedAp()
                {
                    iedName = x.Attribute("iedName").Value,
                    ip = x.Descendants(ns +"P").Where(e => e.Attribute("type").Value == "IP").First().Value
                }).ToList();


            foreach (var ca in connectedAps)
            {
             Console.WriteLine($"{ca.iedName}, {ca.ip}");   
            }


            //DATASET MEMBER 구하기 위해 <DataSet > ...</DataSet> 값 구하기

            var dm = root.Descendants(ns + "DataSet").ToList();

            var dm2 = dm.Descendants(ns + "FCDA").Select(x => new dm()
            {
                address = x.Attribute("lnClass").Value + "$" +  x.Attribute("fc").Value + "$" + x.Attribute("doName").Value + (x.Attribute("daName") != null ? "$" + x.Attribute("daName").Value : "")
            }).ToList();

            foreach (var d in dm)
            {
                Console.WriteLine(d.ToString());
            }

            foreach (var d2 in dm2)
            {
                Console.WriteLine(d2.address);
            }
        }

        public class dm
        {
            public String address { get; set; }


            public dm()
            {
            }

            public dm(string address)
            {
                this.address = address;

            }
        }
        public class ConnectedAp
        { 
            public String iedName { get; set; }
            public String ip { get; set; }

            public ConnectedAp()
            {
            }
          
            public ConnectedAp(String iedName, String ip)
            {
                this.iedName = iedName;
                this.ip = ip;
            }
        }
    }
}

// TEST.cid
//<? xml version = "1.0" encoding = "utf-8" ?>
//    < SCL xmlns = "http://www.iec.ch/61850/2003/SCL" xmlns: xsi = "http://www.w3.org/2001/XMLSchema-instance" xsi: schemaLocation = "http://www.iec.ch/61850/2003/SCL SCL.xsd" >
//    < Header id = "NMS_IEC61850_SERVER" nameStructure = "IEDName" />
//    < Communication >
//    < SubNetwork name = "SubNetwork1" type = "8-MMS" >
//    < ConnectedAP apName = "S1" iedName = "D000_NMS3" >
//    < Address >
//    < P type = "IP" > 192.168.56.1 </ P >
//    < P type = "IP-SUBNET" > 255.255.255.0 </ P >
//    < P type = "IP-GATEWAY" > 192.168.111.1 </ P >
//    < P type = "OSI-AP-Title" > 1,1,9999,1 </ P >
//    < P type = "OSI-AE-Qualifier" > 6 </ P >
//    < P type = "OSI-PSEL" > 00000001 </ P >
//    < P type = "OSI-SSEL" > 0001 </ P >
//    < P type = "OSI-TSEL" > 0001 </ P >
//    </ Address >
//    </ ConnectedAP >
//    </ SubNetwork >
//    </ Communication >
//    </ SCL >
