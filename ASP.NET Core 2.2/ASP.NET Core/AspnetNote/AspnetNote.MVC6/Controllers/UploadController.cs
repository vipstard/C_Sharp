using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspnetNote.MVC6.Controllers
{
    public class UploadController : Controller
    {
        private readonly IHostingEnvironment _environment;
        // GET: /<controller>/

        public UploadController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        //http://www.example.com/Upload/ImageUpload
        [HttpPost, Route("api/upload")]
        public async Task<IActionResult> ImageUpload(IFormFile file)  //비동기 처리 async Task<T>
        {
            // 이미지나 파일을 업로드 할때 필요한 구성
            //1. Path(경로) - 어디에다 저장할지 결정
            var path = Path.Combine(_environment.WebRootPath, @"images\upload"); 

            // 2. Name(이름) - DateTime 으로 많이쓰지만 사용자가 많으면 적합하지 않다. 충돌날 수 있음
           // GUID + GUID  (전역고유식별자)
            // 3. Extension(확장자) - jpg, png... txt
            // 파일 이름 image.jpg
             var fileFullName = file.FileName.Split('.');
            var fileName = $"{Guid.NewGuid()}.{fileFullName[1]}";

            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                /* async - await 한짝으로 같이 쓴다. */
                await file.CopyToAsync(fileStream); 
            }
                return Ok(new { file = "/images/upload/" + fileName, success = true });

        //URL 접근 방식
        // ASP.NET -호스트명/api/upload
        // JavaScript -호스트명 + api/upload => http://www.example.comapi/upload
        // JavaScript -호스트명 + / + api/upload => http://www.example.com/api/upload

        }
    }
}
