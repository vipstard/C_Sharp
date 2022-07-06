using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hello.MVC6.Models;
using Hello.BLL;

namespace Hello.MVC6.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserBLL _userBLL;

        public HomeController(UserBLL userBLL)
        {
            _userBLL = userBLL;
        }

        public IActionResult Index()
        {
            var userListr = _userBLL.GetUserLitst();

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
