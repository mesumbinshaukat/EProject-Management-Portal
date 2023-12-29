using Microsoft.AspNetCore.Mvc;
using Symphony_LTD.Data;
using Symphony_LTD.Models;
using System.Diagnostics;

namespace Symphony_LTD.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbSymphonyContext _db;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DbSymphonyContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
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

        public IActionResult Register ()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register (Student obj, IFormFile Std_Image)
        {
            if (Std_Image != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                string filepath = Path.Combine(path, Std_Image.FileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var stream = new FileStream(filepath, FileMode.Create);
                Std_Image.CopyTo(stream);
                string? filename = Std_Image.FileName;
                obj.Picture = filename;

            }

            if (ModelState.IsValid)
            {
                _db.Students.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("LogIn");
            }
            return View(obj);
            
        }
    }
}