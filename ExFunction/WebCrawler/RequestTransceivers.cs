using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crawler
{
    class RequestTransceivers
    {
        static async Task Main(string[] args)
        {
            // 접속할 H101 IP만 바꿔주면 됌
            string ip = "10.10.156.206";
            string mainUrl = $"http://{ip}:10103/list";
            string userName = "paranoid";
            string passWord = "survive";

            List<string> transceiversList = await GetTransceiversList(mainUrl, userName, passWord);

            if (transceiversList.Count > 0)
            {
                foreach (string transceiver in transceiversList)
                {
                    Console.WriteLine(transceiver);
                }
            }
            else
            {
                Console.WriteLine("Transceivers URL을 찾을 수 없음");
            }
        }

        static async Task<List<string>> GetTransceiversList(string mainUrl, string username, string password)
        {
            List<string> transceiversList = new List<string>();
            string transceiversUrl = string.Empty;

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.Credentials = new System.Net.NetworkCredential(username, password);

                using (HttpClient client = new HttpClient(handler))
                {
                    HttpResponseMessage response = await client.GetAsync(mainUrl);

                    //H101 Main 응답 성공 
                    if (response.IsSuccessStatusCode)
                    {
                        string pageContents = await response.Content.ReadAsStringAsync();

                        // 메인페이지에 요청 후 받아온 xsl의 Resource id에서 transceivers url 파싱
                        HtmlParser parser = new HtmlParser();
                        IHtmlDocument document = await parser.ParseDocumentAsync(pageContents);
                        IEnumerable<string> transceiversLinks = document.QuerySelectorAll("Resource").Attr("id");

                        foreach (string link in transceiversLinks)
                        {
                            if (link.Contains("transceivers"))
                            {
                                transceiversUrl = string.Join("", mainUrl.Replace("/list", ""), link);

                                Console.WriteLine(transceiversUrl);
                                break;
                            }
                        }
                        response = await client.GetAsync(transceiversUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            pageContents = await response.Content.ReadAsStringAsync();
                            // 메인페이지에 요청 후 받아온 xsl의 Resource id에서 transceivers url 파싱
                            parser = new HtmlParser();
                            document = await parser.ParseDocumentAsync(pageContents);
                            var dd = document.QuerySelectorAll("a");

                            foreach (var li in dd)
                            {
                                Console.WriteLine(li.TextContent);
                            }

                        }


                    }
                    // 응답실패 (401 : 권한 부족, 404 : 페이지 찾을 수 없음)
                    else
                    {
                        Console.WriteLine("메인 페이지 요청 응답이 200이 아닙니다.");
                    }




                }
            }

            return transceiversList;
        }
    }
}
