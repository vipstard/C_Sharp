using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SCL_Parser
{
    //XML 파일의 모든 태그 값을 가져오기
    internal class program2
    {

        public static void Main()
        {
            String path = "C:\\nms4sa\\database\\TEST.cid";
            XNamespace ns = XDocument.Load(path).Root.GetDefaultNamespace();
            XDocument doc = XDocument.Load(path); //Loads the document by looking for the given path

            // 전체 출력

            List<XElement> elements = doc.Descendants().ToList(); //Extracts a list of every descendant node of the starting element.

            //foreach (XElement element in elements)
            //{
            //    Console.WriteLine(element);
            //}

            // element 출력
            XElement response = doc.Elements().FirstOrDefault();
            //Console.WriteLine(response);

            List<XElement> ConnectedAp = doc.Descendants("P").ToList();

            var ConnectAp = XDocument.Load(path).Descendants(ns + "ConnectedAP").ToList();
            foreach (var ca in ConnectAp)
            {

               

            }


            List<ConnectedAp> CAP = ConnectedAp.Select(x => Deserialize<ConnectedAp>(x.ToString())).ToList();
            foreach (var ca in ConnectedAp)
            {
                Console.WriteLine(ca);
            }
            
        }


        public static string Serialize<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
            catch
            {
                throw;
            }
        }

        public static T Deserialize<T>(string xmlText)
        {
            try
            {
                var stringReader = new System.IO.StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                throw;
            }
        }
    }

    [XmlRoot(ElementName = "SCL", Namespace = "http://www.iec.ch/61850/2003/SCL")]
    public class ConnectedAp
    {
        public String iedName { get; set; }
        public String ip { get; set; }

        public ConnectedAp(){}
        public ConnectedAp(String iedName, String ip)
        {
            this.iedName = iedName;
            this.ip = ip;
        }
    }

}

