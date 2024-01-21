using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            ViewBag.Courses = _db.Courses.ToList();
            ViewBag.EntranceExam = _db._EntranceExamList.FirstOrDefault(x => x.Availablity == true);
            ViewBag.Course = _db.Courses.FirstOrDefault(x => x.CourseFee <= 800);
            ViewBag.FAQ = _db.FAQS.FirstOrDefault();
            var existing_content = _db._HomeSectionOne.ToList();
            if(existing_content != null)
            {
            foreach(var i in existing_content)
                {
                    ViewBag.H5 = i.H5;
                    ViewBag.H2 = i.H2;
                    ViewBag.Paragraph = i.Paragraph;
                    ViewBag.BtnAction = i.BtnAction;
                    ViewBag.BtnVal = i.BtnVal;
                    ViewBag.Img = i.Img;
                }

            }

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

    }
}