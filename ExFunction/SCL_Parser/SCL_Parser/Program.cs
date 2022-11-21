using System.Xml;
using System.Xml.Linq;

namespace SCL_Parser
{
    public class Program
    {
        public static void Main()
        {
            String path = "C:\\nms4sa\\database\\D000_NMS.cid";

            XmlDocument xmlFile = new XmlDocument();
            xmlFile.Load(path);

            XmlNodeList XmlList = xmlFile.GetElementsByTagName("SubNetwork");

            Console.WriteLine(XmlList.Item(0).InnerText);
            //foreach (XmlNode item in XmlList)
            //{
            //    Console.WriteLine(item.InnerText);
            //    Console.WriteLine($"{item["ConnectedAp"]["Address"]["P"].InnerText}");
            //}
        }
    }

    // 아래 XML  에서 P type="IP"의 값을 얻고싶다.

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
}
