using Microsoft.AspNetCore.Mvc;
using MSIT153Site.Models;
using MSIT153Site.Models.ViewModel;
using System.Runtime.CompilerServices;

namespace MSIT153Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly IWebHostEnvironment _host;
        private readonly DemoContext _context;
        public ApiController(IWebHostEnvironment host, DemoContext context)
        {
            _host = host;
            _context = context;
        }


        //  public IActionResult Index(string name, int age=30)
        public IActionResult Index(UserViewModel user)
        {
            //System.Threading.Thread.Sleep(5000);//停五秒鐘
            if (string.IsNullOrEmpty(user.name))
            {
                user.name = "guest";
            }
            //return Content("<h2>Ajax 你好 !!</h2>","text/html", System.Text.Encoding.UTF8);
            return Content($"Hello {user.name}， You are {user.age} years old.");
        }

        public IActionResult register(Members member, IFormFile formFile) //需等於Register.cshtml 的 23到26行的 name欄位要一致。
        {
            //實際路徑
            //C:\Users\User\Documents\workspace\MSIT153Site\wwwroot\uploads\abc.jpg
            //專案根目錄的實際路徑
            //string strPath = _host.ContentRootPath;//C:\Users\User\Documents\workspace\MSIT153Site
            //wwwroot的實際路徑
            //string strPath = _host.WebRootPath;//C:\Users\User\Documents\workspace\MSIT153Site\wwwroot
            string strPath = Path.Combine(_host.WebRootPath, "uploads",formFile.FileName);//C:\Users\User\Downloads\MSIT153Site-master\wwwroot\uploads\boy_5.png

            //將檔案存到uploads資料夾中
            using (var fileStream = new FileStream(strPath, FileMode.Create))
            {
                formFile.CopyTo(fileStream);
            }
            //資料庫目前還缺少FileName與FileData
            member.FileName = formFile.FileName;
            //將上傳的圖轉成二進位
            byte[]? imgByte = null;

            using (var MemoryStream = new MemoryStream())
            {
                formFile.CopyTo(MemoryStream);
                imgByte = MemoryStream.ToArray();
            }
            member.FileData = imgByte;

            //將資料寫進資料庫中
            _context.Members.Add(member);
            _context.SaveChanges();

            return Content("新增成功");

            //檔案名稱、檔案大小、檔案類型
            //string fileInfo = $"{formFile?.FileName} - {formFile?.Length} - {formFile?.ContentType}";
            //return Content(fileInfo);


            //return Content("<h2>Ajax 你好 !!</h2>","text/html", System.Text.Encoding.UTF8);
            //return Content($"Hello {member.name}，{member.email},  You are {member.age} years old.");
        }
        public IActionResult cities()
        {
            var cities = _context.Address.Select(x => x.City).Distinct();
            return Json(cities);
        }
        public IActionResult districts(string city)
        
        {
            var districts = _context.Address.Where(c=>c.City == city).Select(a=>a.SiteId).Distinct();
            return Json(districts);
            //https://localhost:7202/api/districts?city=xxx(縣市)
        }
        public IActionResult roads(string SiteId)
        {
        var road = _context.Address.Where(c=>c.SiteId==SiteId)
            .Select(a=>a.Road).Distinct();
            return Json(road);
            //https://localhost:7202/API/road?SiteId=%E9%87%91%E9%96%80%E7%B8%A3%E9%87%91%E6%B9%96%E9%8E%AE
        }
        //讀取資料庫中二進位的圖片
        public IActionResult GetImageByte(int id = 1)
        {
            Members? member = _context.Members.Find(id);
            byte[]? img = member?.FileData;

            if(img != null)
            {
                return File(img, "image/jpeg");
            }
            return NotFound();
                

        }

    }
}
