﻿using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace SCL_Parser
{
    public class Program
    {
        public static void Main()
        {

            String path = "C:\\nms4sa\\database\\D000_NMS.cid";

            // 1번
            XmlDocument xmlFile = new XmlDocument();
            xmlFile.Load(path);

            XmlNodeList XmlList = xmlFile.GetElementsByTagName("Communication");

            foreach (XmlNode item in XmlList)
            {
                Console.WriteLine($"{item["SubNetwork"]["ConnectedAP"]["Address"]["P"].InnerText}");
            }

            // 2번 LInq  (nameSpace 필요)
            XNamespace ns = XDocument.Load(path).Root.GetDefaultNamespace();
            var result = XDocument.Load(path)
                .Descendants(ns + "P")
                .Where(e => e.Attribute("type").Value == "IP")
                .FirstOrDefault()?.Value;

            Console.WriteLine(result);

            // 3번 만약 <ConnectedAp> 가 여러개 라면? => .ToList()

            var results = XDocument.Load(path)
                .Descendants(ns + "P")
                .Where(e => e.Attribute("type").Value == "IP")
                .ToList();

            Console.WriteLine("===========");

            foreach (var rs in results)
            {
                Console.WriteLine(rs.Value);
            }

            // 4번 <ConnectedAp> iedName들 찾기
            var iedName = XDocument.Load(path).Descendants(ns + "ConnectedAP").Attributes("iedName").ToList();

            foreach (var rs in iedName)
            {
                Console.WriteLine(rs.Value);
            }

            // 5번 <ConnectedAp> 에서 필요한 정보 들 빼오기 (iedName, ip)
            Console.WriteLine("\n\n");

            var ConnectAp = XDocument.Load(path).Descendants(ns + "ConnectedAP").ToList();
            foreach (var ca in ConnectAp)
            {
                
                Console.WriteLine(ca.Attribute("iedName").Value); 
                Console.WriteLine(ca.Descendants(ns+"P").Where(e=>e.Attribute("type").Value == "IP").FirstOrDefault()?.Value); 
            }

        }
    }

    // 아래 XML  에서 P type="IP"의 값을 얻고싶다.
    
    //<? xml version="1.0" encoding="utf-8"?>
    //<SCL xmlns = "http://www.iec.ch/61850/2003/SCL" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.iec.ch/61850/2003/SCL SCL.xsd">
    //<Header id = "NMS_IEC61850_SERVER" nameStructure="IEDName"/>
    //<Communication>
    //    <SubNetwork name = "SubNetwork1" type="8-MMS">
    //        <ConnectedAP apName = "S1" iedName="D000_NMS">
    //            <Address>
    //                <P type = "IP" > 192.168.111.50</P>
    //                <P type = "IP-SUBNET" > 255.255.255.0</P>
    //                <P type = "IP-GATEWAY" > 192.168.111.1</P>
    //                <P type = "OSI-AP-Title" > 1,1,9999,1</P>
    //                <P type = "OSI-AE-Qualifier" > 6 </ P >
    //                < P type="OSI-PSEL">00000001</P>
    //                <P type = "OSI-SSEL" > 0001 </ P >
    //                < P type="OSI-TSEL">0001</P>
    //            </Address>
    //            <!--<GSE cbName = "GocbA" desc="" ldInst="ALRM">
    //                <Address>
    //                    <P type = "MAC-Address" > 01 - 0C-CD-01-00-52</P>
    //                    <P type = "APPID" > 1 </ P >
    //                    < P type="VLAN-PRIORITY">4</P>
    //                    <P type = "VLAN-ID" > 0 </ P >
    //                </ Address >
    //                < MinTime multiplier="m" unit="s">10</MinTime>
    //                <MaxTime multiplier = "m" unit="s">2000</MaxTime>
    //            </GSE>-->
    //        </ConnectedAP>
    //    </SubNetwork>
    //</Communication>

    //</SCL>
}