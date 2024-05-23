
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Crawler
{
    class RequestTransceivers
    {
        static void Main(string[] args)
        {
            // 접속할 H101 IP만 바꿔주면 됌
            string ip = "";
            string mainUrl = $"http://{ip}:10103/list";
            string userName = "";
            string passWord = "";

            List<string> transceiversList =  GetTransceiversList(mainUrl, userName, passWord);

            if (transceiversList.Count > 0)
            {
                foreach (string transceiver in transceiversList)
                {
                    Console.WriteLine(transceiver);
                }
            }
            else
            {
                Console.WriteLine("Transceivers List를 찾을 수 없음");
            }
        }

        /// <summary>
        /// 메인 Url에 요청 후 transceivers url을 받고
        /// 다시 transceivers url에 요청해서 현재 transceivers 목록을 가져온다.
        /// </summary>
        /// <param name="mainUrl"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        static List<string> GetTransceiversList(string mainUrl, string username, string password)
        {
            List<string> transceiversList = new List<string>();
            string transceiversUrl = string.Empty;

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                //Http 기본인증 id, pw로 인증한다.
                handler.Credentials = new System.Net.NetworkCredential(username, password);

                //client 접속
                using (HttpClient client = new HttpClient(handler))
                {
                    HttpResponseMessage response = client.GetAsync(mainUrl).Result;

                    //H101 Main 응답 성공 
                    if (response.IsSuccessStatusCode)
                    {
                        string pageContents = response.Content.ReadAsStringAsync().Result;

                        #region 메인페이지에 요청 후 받아온 xsl의 Resource id에서 transceivers url 파싱
                        HtmlParser parser = new HtmlParser();
                        IHtmlDocument document = parser.ParseDocument(pageContents);
                        IEnumerable<string> transceiversLinks = document.QuerySelectorAll("Resource").Attr("id");

                        foreach (string link in transceiversLinks)
                        {
                            if (link.Contains("transceivers"))
                            {
                                transceiversUrl = string.Join("", mainUrl.Replace("/list", ""), link);

                                Console.WriteLine($"Transceivers Url : {transceiversUrl}");
                                break;
                            }
                        }

                        if (string.IsNullOrEmpty(transceiversUrl))
                        {
                            Console.WriteLine("Transceivers Url 찾을 수 없음");
                        }
                        #endregion

                        response =  client.GetAsync(transceiversUrl).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            pageContents =  response.Content.ReadAsStringAsync().Result;

                            #region transceiversUrl에 요청 후 받아온 xsl의 <a> 태그에서 transceivers 값들 파싱
                            document =  parser.ParseDocument(pageContents);
                            IHtmlCollection<IElement> elements = document.QuerySelectorAll("a");

                            foreach (IElement aTagValue in elements)
                            {
                                transceiversList.Add(aTagValue.TextContent);
                            }
                            #endregion

                        }
                        else
                        {
                            Console.WriteLine("Transceivers 요청 응답이 200이 아닙니다.");
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