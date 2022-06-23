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
        //http:www.example.com/Home/Index
        public IActionResult Index()
        {

            //오브젝트 이니셜라이저
            var firstUser = new User
            {
                UserNo = 1,
                UserName = "홍길동"
            };
            // 1번째 방법 @Model
            // return View(firstUser);

            // 2번째 방법 ViewBag
            //ViewBag.User = firstUser;

            // 3번째 방법 ViewData
            ViewData["UserNo"] = firstUser.UserNo;
            ViewData["UserName"] = firstUser.UserName;

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
    }
}
