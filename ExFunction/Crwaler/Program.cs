using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;

namespace Crwaler
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> transceiversList = new List<string>();
            string currentPath = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\";
            string currentDirectory = Path.GetDirectoryName(currentPath);
            Console.WriteLine("현재 실행 파일의 경로: " + currentPath);
            Console.WriteLine("현재 실행 파일이 있는 디렉토리 경로: " + currentDirectory);


       

            string ip = ""; //접속할 H101 IP만 바꿔주면 됌
            string mainUrl = $"";
            string url = SearchTransceiversUrl(currentDirectory, mainUrl);

            if (!string.IsNullOrEmpty(url))
            {
                ExecuteChrome(currentDirectory, url, transceiversList);
                Console.WriteLine($"\n{url}");
                foreach (string transceiver in transceiversList)
                {
                    Console.WriteLine(transceiver);
                }

                //foreach (string transceiver in transceiversList)
                //{
                //    if (transceiver.Contains("K1"))
                //    {
                //        Console.WriteLine(transceiver);
                //    }
                //}

            }
            else
            {
                Console.WriteLine("Transceivers URL을 찾을 수 없음");
            }
        }

        static string SearchTransceiversUrl(string currentDirectory, string url)
        {
            string href = string.Empty;
            using (var chromeDriver = InitializeChromeDriver(currentDirectory))
            {
                chromeDriver.Navigate().GoToUrl(url);
                int responseCode = GetResponseCode(chromeDriver);

                if (responseCode == 200)
                {
                    IReadOnlyCollection<IWebElement> elements = chromeDriver.FindElements(By.XPath("//a[contains(@href, 'transceivers')]"));
                    foreach (var element in elements)
                    {
                        href = element.GetAttribute("href");
                    }
                }
                else // 404 : 찾을 수 없음, 401 : 권한 거부
                {
                    Console.WriteLine("페이지 요청 응답이 200이 아닙니다.");
                }
            }
            return href;
        }

        static void ExecuteChrome(string currentDirectory, string url, List<string> list)
        {
            using (var chromeDriver = InitializeChromeDriver(currentDirectory))
            {
                chromeDriver.Navigate().GoToUrl(url);
                int responseCode = GetResponseCode(chromeDriver);

                if (responseCode == 200)
                {
                    var elements = chromeDriver.FindElements(By.TagName("a"));
                    foreach (var element in elements)
                    {
                        list.Add(element.Text);
                    }
                }
                else
                {
                    Console.WriteLine("페이지 요청 응답이 200이 아닙니다.");
                }
            }
        }

        /// <summary>
        /// 현재 디렉토리에서 크롬드라이버 찾아서 사용
        /// </summary>
        /// <param name="currentDirectory"></param>
        /// <returns></returns>
        static ChromeDriver InitializeChromeDriver(string currentDirectory)
        {
            ChromeOptions options = new ChromeOptions();

            // 크롬창 최소화
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");

            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(currentDirectory);
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
