using System.ComponentModel.DataAnnotations;

namespace AspnetNote.MVC6.ViewModel
{
    public class LoginViewModel
    {
        // 로그인 할때만 사용할 View  모델
        [Required(ErrorMessage ="사용자 ID를 입력해주세요")]
        public string UserId{get; set;}

        [Required(ErrorMessage = "사용자 비밀번호를 입력해주세요")]
        public string UserPassword { get; set; }
    }
}
