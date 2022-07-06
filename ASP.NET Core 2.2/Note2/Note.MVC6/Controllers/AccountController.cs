using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Note.Model;
using Note.ViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Note.MVC6.Controllers
{
    public class AccountController : Controller
    {
        // GET: /<controller>/
        public IActionResult  Login()
        {
            return View();
        }

        /// <summary>
        /// 로그인 전송
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IActionResult Login(LoginViewModel model) // User클래스 자체를 넘기면 [Required] 이기 때문에 IsValid를 넘어 갈 수 없다. 
        {
            if (ModelState.IsValid)
            {
                // 모두 입력 받음
                var id = model.Id;
                var password = model.Password;
            }
            return View();

        }
    }
}
