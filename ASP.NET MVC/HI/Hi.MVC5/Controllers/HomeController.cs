using Hi.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hi.MVC5.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserBll _userBll;

        public HomeController(UserBll userBll)
        {
            _userBll = userBll;
        }

        public ActionResult Index()
        {
            var list = _userBll.GetUsersList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}