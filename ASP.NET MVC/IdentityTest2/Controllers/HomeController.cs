using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityTest2.Models;
using Microsoft.AspNetCore.Authorization;

namespace IdentityTest2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        /* 특별한 인증이 필요하다 . 인증된 사용자만 사용이 가능하다. */

        /// <summary>
        /// Index 페이지
        /// </summary>
        /// <returns></returns>
        /// [AllowAnonymous]  : 클래스단위에 Authorize를 사용하여 인증된 사용자만 사용할 수 있지만 
        /// 특정 페이지를 익명누구나 사용 할 수 있게 허용한다. 위의 경우 Index페이지는 
        /// 누구나 접근할 수 있다.
        [AllowAnonymous] 
        public IActionResult Index()
        {
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
