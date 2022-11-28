using System.Diagnostics;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FileStream_SclParesr.Controller
{
    using Microsoft.AspNetCore.Mvc;
    using System.IO;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/Home/upload")]
        public async Task<IActionResult> upload(IFormFile testFile)
        {
            //1.  stringBuilder 사용해서 filestream 사용
            //var result = new StringBuilder();
            //using (var reader = new StreamReader(testFile.OpenReadStream()))
            //{
            //    while (reader.Peek() >= 0)
            //        result.AppendLine(await reader.ReadLineAsync());
            //}
            //Debug.WriteLine(result.ToString());


            //2. IFormFile -> XML 즉시변환
            using (MemoryStream str = new MemoryStream())
            {
                await testFile.CopyToAsync(str);
                str.Position = 0;
                var xml = XDocument.Load(str);

                XElement root = xml.Document.Root;
                XNamespace ns = root.GetDefaultNamespace();

                var connectedAps = root
                    .Descendants(ns + "ConnectedAP")
                    .Select(x => new ConnectedAp()
                    {
                        iedName = x.Attribute("iedName").Value,
                        ip = x.Descendants(ns + "P").First().Value
                    }).ToList();


                foreach (var ca in connectedAps)
                {
                    Debug.WriteLine($"{ca.iedName}, {ca.ip}");
                }
            }
           
            return Ok();
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
