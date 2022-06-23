using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetNote.MVC6.DataContext;
using AspnetNote.MVC6.Models;
using AspnetNote.MVC6.ViewModel;
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

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // ID, 비밀번호 -필수
            if (ModelState.IsValid)
            {
                // DB 오픈 커넥션 후 자동으로 닫음
                using (var db = new AspnetNoteDbContext())
                {
                    // Linq - 메서드 체이닝
                    // => : A Go to B 
                    // 입력받은 아이디 와 Db 값 비교
                    // FirstOrDefault() : 첫 번째 입력값 출력하겠다.  ()안에 원하는값 넣어서 찾기
                    // 비교할때 == 를 사용하면 메모리누수가 발생하기 때문에 Equals를 사용한다.
                    //var user = db.Users.FirstOrDefault(u=> u.UserId == model.UserId && u.UserPassword == model.UserPassword); 
                    var user = db.Users.FirstOrDefault(u => u.UserId.Equals(model.UserId) && u.UserPassword.Equals(model.UserPassword)); // 아이디 비밀번호 매칭

                    // 로그인 실패
                    if (user != null)
                        return RedirectToAction("LoginSuccess", "Home");
              
                    // 로그인 성공, 사용자 ID 자체가 회원가입 X 경우
                    ModelState.AddModelError(string.Empty, "사용자 ID 혹은 비밀번호가 올바르지 않습니다.");
                }
            }
            return View(model);
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
