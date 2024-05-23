using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<string> transceiversList = new List<string>();

            ///* 접속할 H101 IP만 바꿔주면 됌 */
            //string ip = "";

            //string mainUrl = $"";

            //NavigateAndExtract(mainUrl, transceiversList);

            //if (transceiversList.Count > 0)
            //{
            //    foreach (string transceiver in transceiversList)
            //    {
            //        Console.WriteLine(transceiver);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Transceivers URL을 찾을 수 없음");
            //}   //List<string> transceiversList = new List<string>();

            ///* 접속할 H101 IP만 바꿔주면 됌 */
            //string ip = "";

            //string mainUrl = $"";

            //NavigateAndExtract(mainUrl, transceiversList);

            //if (transceiversList.Count > 0)
            //{
            //    foreach (string transceiver in transceiversList)
            //    {
            //        Console.WriteLine(transceiver);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Transceivers URL을 찾을 수 없음");
            //}
        }

        static void NavigateAndExtract(string mainUrl, List<string> list)
        {
            using (var chromeDriver = InitializeChromeDriver())
            {
                chromeDriver.Navigate().GoToUrl(mainUrl);
                int responseCode = GetResponseCode(chromeDriver);

                if (responseCode == 200) // 200 : 정상
                {
                    // H101 접속 후 Transceivers 요청 URL 파싱
                    IReadOnlyCollection<IWebElement> elements = chromeDriver.FindElements(By.XPath("//a[contains(@href, 'transceivers')]"));
                    string transceiversUrl = string.Empty;

                    foreach (var element in elements)
                    {
                        transceiversUrl = element.GetAttribute("href");
                        break;
                    }

                    // Transceivers 주소에 목록 요청
                    if (!string.IsNullOrEmpty(transceiversUrl))
                    {
                        chromeDriver.Navigate().GoToUrl(transceiversUrl);
                        responseCode = GetResponseCode(chromeDriver);

                        if (responseCode == 200)
                        {
                            // <a> 태그 값 파싱
                            elements = chromeDriver.FindElements(By.TagName("a"));
                            foreach (var element in elements)
                            {
                                list.Add(element.Text);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Transceivers 페이지 요청 응답이 200이 아닙니다.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Transceivers URL을 찾을 수 없음");
                    }
                }
                else // 404 : 찾을 수 없음, 401 : 권한 거부
                {
                    Console.WriteLine("메인 페이지 요청 응답이 200이 아닙니다.");
                }
            }
        }

        /// <summary>
        /// 크롬드라이버 사용하여 크롬접속 (본인 Chrome에 맞는 버전 ChromeDriver 필요함)
        /// </summary>
        /// <param name="currentDirectory"></param>
        /// <returns></returns>
        static ChromeDriver InitializeChromeDriver()
        {
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            ChromeOptions options = new ChromeOptions();

            // 크롬창 최소화 옵션 
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");

            chromeDriverService.HideCommandPromptWindow = true;

            return new ChromeDriver(chromeDriverService, options);
        }

        /// <summary>
        /// 웹서버 응답 코드 가져오기
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        static int GetResponseCode(ChromeDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            string script = @"return fetch(window.location.href)
                .then(response => response.status)
                .catch(error => {
                    console.error('Error fetching response code:', error);
                    return -1;
                });";

            return Convert.ToInt32(js.ExecuteScript(script));
        }
    }
}