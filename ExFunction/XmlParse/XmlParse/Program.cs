using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using static XmlParse.Program;

namespace XmlParse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // XDocument 형식의 파일을 로드
            XDocument xmlScl = XDocument.Load("C:\\Users\\Yoon DongIl\\Desktop\\Yoon\\NasCopy\\NMS\\개정규격관련파일\\개정규격관련파일\\E000_C457.cid");

            // AddIed 메서드 호출
            Device device = new Device();
          
            ParseXml( xmlScl, device);


        }
        public static void ParseXml(XDocument xmlScl, Device paramDevice)
        {
            try
            {
                XElement root = xmlScl.Document.Root;
                XNamespace ns = root.GetDefaultNamespace();

                List<XElement> ConnectAp = root.Descendants(ns + "ConnectedAP").ToList();


                foreach (XElement ca in ConnectAp)
                {
                    // SCL파일 속 <ConnectedAP> 에서 iedName 과 IP를 Parsing 
                    // GW 는 이름 설정 가능 IED 는 이름 설정 불가
                    paramDevice.Name = ca.Attribute("iedName").Value;
                    paramDevice.Ip = ca.Descendants(ns + "P").Where(e => e.Attribute("type").Value == "IP").First().Value;
                    paramDevice.Mac = ca.Descendants(ns + "P").Where(e => e.Attribute("type").Value == "MAC-Address").First().Value;

                    Console.WriteLine($"Name : {paramDevice.Name}, IP : {paramDevice.Ip}, MAC : {paramDevice.Mac}");

                }
            }
            catch (IOException e)
            {
                throw new IOException(e.Message);
            }
        }

        public class Device
        {
            public String Name { get; set; }
            public String Ip { get; set; }
            public String Mac { get; set; }
        }
    }
}