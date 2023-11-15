using Microsoft.AspNetCore.Mvc;
using MSIT153Site.Models.ViewModel;
using System.Runtime.CompilerServices;

namespace MSIT153Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly IWebHostEnvironment _host;
        public ApiController(IWebHostEnvironment host)
        {
            _host = host;
        }


        //  public IActionResult Index(string name, int age=30)
        public IActionResult Index(UserViewModel user)
        {
            System.Threading.Thread.Sleep(1000);
            if (string.IsNullOrEmpty(user.name))
            {
                user.name = "guest";
            }
            //return Content("<h2>Ajax 你好 !!</h2>","text/html", System.Text.Encoding.UTF8);
            return Content($"Hello {user.name}， You are {user.age} years old.");
        }

        public IActionResult register(MemberViewModel member ,IFormFile formFile) //需等於Register.cshtml 的 23到26行的 name欄位要一致。
        {
            //實際路徑
            //C:\Users\User\Documents\workspace\MSIT153Site\wwwroot\uploads\abc.jpg
            //string strPath = _host.ContentRootPath;//C:\Users\User\Documents\workspace\MSIT153Site
            //string strPath = _host.WebRootPath;//C:\Users\User\Documents\workspace\MSIT153Site\wwwroot
           

            string strPath = Path.Combine(_host.WebRootPath, "uploads", formFile.FileName);//C:\Users\User\Downloads\MSIT153Site-master\wwwroot\uploads\boy_5.png

           using(var fileStream = new FileStream(strPath, FileMode.Create))
            {
                formFile.CopyTo(fileStream);//將圖片上傳至wwwroot/uploads底下。
            }

            return Content(strPath);
            //return Content("<h2>Ajax 你好 !!</h2>","text/html", System.Text.Encoding.UTF8);
            //return Content($"Hello {member.name}，{member.email},  You are {member.age} years old.");
        }
    }
}
