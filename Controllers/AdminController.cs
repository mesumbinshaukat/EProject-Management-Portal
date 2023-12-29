using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Symphony_LTD.Data;
using Symphony_LTD.Models;

namespace Symphony_LTD.Controllers
{
    public class AdminController : Controller
    {
        private readonly DbSymphonyContext _db;

        public AdminController(DbSymphonyContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                IEnumerable<Admin> objAdmin = _db._Admin;
                return View("Index");

            }
            return View("LogIn");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {

                return RedirectToAction("Index", "Admin");
            }
            IEnumerable<Admin> objAdmin = _db._Admin;

            return View();

        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult LogIn(Admin data, string emailaddress, string password)
        {
            var check_user = _db._Admin.Where(i => i.Email == emailaddress).FirstOrDefault();

            var test_email = check_user ?? null;
            if (test_email != null)
            {
                var check_user_password = _db._Admin.Where(i => i.Password == password).FirstOrDefault();

                var test_password = check_user_password ?? null;

                var verify = check_user.Password == password;

                if (verify && test_password != null)
                {

                    var verify_pass = verify.ToString();

                    HttpContext.Session.SetString("s_email", emailaddress); // Set Email In Session
                    HttpContext.Session.SetString("s_pass_verify", verify_pass); // Set Password In Session



                    TempData["success"] = "Logged In Successfully";
                    return RedirectToAction("Index", "Admin");

                }
                TempData["failed"] = "Invalid Password";
                return RedirectToAction("LogIn");


            }
            TempData["failed"] = "Invalid Email";
            return RedirectToAction("LogIn");

        }

        
        public IActionResult LogOut()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                HttpContext.Session.Remove("s_email"); // Remove email from session
                HttpContext.Session.Remove("s_pass_verify"); // Remove password verification from session

                TempData["success"] = "Logged Out Successfully";
                return RedirectToAction("LogIn", "Admin");
            }
            return View("LogIn");
        }


        public IActionResult Courses() {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                IEnumerable<Course> courses = _db.Courses;
                return View(courses);

            }
            return View("LogIn");
        }

        public IActionResult AddCourse()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                return View();

            }
            return View("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCourse(Course obj)
        {
            if (ModelState.IsValid)
            {
                _db.Courses.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Courses");
            }
            else
            {
                return View(obj);
            }
            //return View("LogIn");
        }

        public IActionResult EditCourse(int? id)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var studentFromDb = _db.Courses.Find(id);

                if (studentFromDb == null)
                {
                    return NotFound();
                }
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                ViewBag.Course = _db.Courses.ToList();
                return View(studentFromDb);



            }
            return View("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCourse(Course obj)
        {
            if (ModelState.IsValid)
            {
                _db.Courses.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Courses");
            }

            return View();
        }

        public IActionResult DeleteCourse(int? id)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                var obj = _db.Courses.Find(id);
                if (id != null)
                {
                    _db.Courses.Remove(obj);
                    _db.SaveChanges();
                    return RedirectToAction("Courses");
                }
            }

            return BadRequest();
        }

        public IActionResult Exam()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Course = _db.Courses.ToList();
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
            
                return View();
            }
            return View("LogIn");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
         public IActionResult Exam(Exam obj)
        {
            if(ModelState.IsValid)
            {
                _db.Exams.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
               
            
                return View();
            
            
        }

        public IActionResult AddStudent ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                return View();
            }

                return View("LogIn");
        }

    }
}
