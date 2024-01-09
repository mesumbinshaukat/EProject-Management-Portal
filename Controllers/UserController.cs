using Microsoft.AspNetCore.Mvc;
using Symphony_LTD.Data;
using Symphony_LTD.Models;

namespace Symphony_LTD.Controllers
{
    public class UserController : Controller
    {
        private readonly DbSymphonyContext _db;

        public UserController (DbSymphonyContext db)
        {
            _db = db;
        }

        public IActionResult Contact()
        {
            ViewBag.Branches = _db.Branches.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Contact obj)
        {
            
                if(ModelState.IsValid)
                {
                    _db._Contact.Add(obj);
                    _db.SaveChanges();
                    TempData["success"] = "Submitted!";
                    return RedirectToAction("Contact");
                }
            TempData["failed"] = "Error!";
            return View(obj);
        }

        public IActionResult Courses ()
        {
            IEnumerable<Course> data = _db.Courses.ToList();
            return View(data);
        }

        public IActionResult AboutUs()
        {
            IEnumerable<About> data = _db._AboutUs.ToList();
            return View(data);
        }
    }
}
