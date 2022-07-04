using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using AsyncAwaitTest.Models;
using System.Threading.Tasks;

namespace AsyncAwaitTest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {

        // # 테스트 방법  - 전통적인 방식
        Stopwatch watch = new Stopwatch();
            watch.Start();

        // # 새로운 테스트 방식? - > VS Diagonostics Tools
            Test1();
            Test2();
            Test3();
            //watch.Stop();
            //var result = watch.ElapsedMilliseconds;
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var test1 =  Test1Async();
            var test2 = Test2Async();
            var test3 = Test3Async(); 

            var result1 = await test1; // var result1 = await Test1Async() 처럼 적으면 여기서 걸려버림
            var result2 = await test2;
            var result3 = await test3;
            //var result = watch.ElapsedMilliseconds;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public int Test1()
        {
            Thread.Sleep(3000);
            return 0;
        }

        public int Test2()
        {
            Thread.Sleep(3000);
            return 0;
        }

        public int Test3()
        {
            Thread.Sleep(4000);
            return 0;
        }

        public async Task<int>  Test1Async()
        {
            await Task.Delay(3000);
            return 0;
        }

        public async Task<int> Test2Async()
        {
            await Task.Delay(3000);
            return 0;

        }

        public async Task<int> Test3Async()
        {
            await Task.Delay(4000);
            return 0;
        }
    }
}
