using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Note.BLL;
using Note.MVC6.Models;

namespace Note.MVC6.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserBll _userBll;
        public HomeController(UserBll userBll)
        {
            _userBll = userBll;
        }

        public IActionResult Index()
        {
            var user = _userBll.GetUser(1);

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
