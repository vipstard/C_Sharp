using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspnetCoreStudy.Models;

namespace AspnetCoreStudy.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 액션 메서드
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            // Controller -> View Data 넘기는 방법
            // [약한 형식의 데이터]
            // 1. ViewBag -> dynamic type(List, User, 숫자, true/false, 문자열)
            // Controller에 정의가 되지 않아도 View에 표시가 될 경우 오류가 발생하지 않는다.
            // ViewBag.Test "Hello World, Test";

            // [약한 형식의 데이터]
            // 2. ViewData[""] 
            // ViewData["T2"] = "Hello World, T2"; 
            //ViewData["T2"] = new User
            //{
            //    UserNo = 1,
            //    UserName = "홍길동"
            //};


            // [강력한 형식의 데이터]
            // ViewModel

            var vm = new UserViewModel
            {
                UserName = "홍길순",
                UserAge = 100
            };

            return View(vm);
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
    }
}
