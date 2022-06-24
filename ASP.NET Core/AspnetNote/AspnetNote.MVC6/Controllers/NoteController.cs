using System.Linq;
using AspnetNote.MVC6.DataContext;
using AspnetNote.MVC6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspnetNote.MVC6.Controllers
{
    public class NoteController : Controller
    {
        /// <summary>
        /// 게시판 리스트
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            { //로그인 안된 상태
                return RedirectToAction("Login", "Account");
            }

            using (var db = new AspnetNoteDbContext())
            {
                // 테이블 안에있는 모든 리스트를 출력
                var list = db.Notes.ToList();
                return View(list);
            }
              
        }

        public IActionResult Detail(int noteNo)
        {
            if(HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            {
                //로그인이 안된상태
                return RedirectToAction("Login", "Account");
            }
            using (var db = new AspnetNoteDbContext())
            {
                var note = db.Notes.FirstOrDefault(n => n.NoteNo.Equals(noteNo));
                return View(note);
            }

        }

        /// <summary>
        /// 게시물 추가
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Note model)
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            { //로그인 안된 상태
                return RedirectToAction("Login", "Account");
            }

            model.UserNo =int.Parse(HttpContext.Session.GetInt32("USER_LOGIN_KEY").ToString());

            if (ModelState.IsValid) // 제목과 내용이 입력되었다면 
            {
                using (var db = new AspnetNoteDbContext())
                {
                    db.Notes.Add(model);

                    if (db.SaveChanges() > 0)  // 추가가 안됐다면 false가 나온다.
                    {
                        return Redirect("Index");
                    }

                    /* if문에서 Redirect 안되고 빠져나왔을 때 전역적인 에러 메세지 출력한다. */
                    ModelState.AddModelError(string.Empty, "게시물을 저장 할 수 없습니다.");
                }
            }
            return View(model);
        }
        /// <summary>
        /// 게시물 수정
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit()
        {
            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            { //로그인 안된 상태
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        /// <summary>
        ///  게시물 삭제
        /// </summary>
        /// <returns></returns>
        public IActionResult Delete(){

            if (HttpContext.Session.GetInt32("USER_LOGIN_KEY") == null)
            { //로그인 안된 상태
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}
