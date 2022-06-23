using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetNote.MVC6.DataContext;
using AspnetNote.MVC6.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspnetNote.MVC6.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 로그인
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 회원가입
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid) // IsValid : 모든값 입력 받았는지 검증 [Required]와 연계
            {
                // Java try(SqlSession){} catch(){}

                //C# 데이터 커넥션
                using (var db = new AspnetNoteDbContext())
                {
                    db.Users.Add(model); // 메모리상에 올라감
                    db.SaveChanges(); // 메모리 -> DB에 저장
                }
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
