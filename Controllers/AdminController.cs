using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Symphony_LTD.Data;
using Symphony_LTD.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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
                //IEnumerable<Admin> objAdmin = _db._Admin;
                //IEnumerable<Contact> objContact= _db._Contact.ToList();

                var adminData = _db._Admin.ToList();
                var contactData = _db._Contact.ToList();

                var viewModel = new AdminAndContact
                {
                    Admin = adminData,
                    Contact = contactData
                };

                var studentPropertiesCount = _db.Students.Count();
                ViewBag.TotalColumns = studentPropertiesCount;

                var totalContacts = _db._Contact.Count();
                ViewBag.TotalContacts = totalContacts;

                var totalRevenue = _db.Courses.ToList();
                decimal total = 0;

                foreach (var i in totalRevenue)
                {
                    if (i.CourseFee.HasValue)
                    {
                        total += i.CourseFee.Value;
                    }
                    else
                    {
                        ViewBag.TotalRevenue = "0";
                    }
                    // Optionally, handle cases where CourseFee is null if necessary
                }

                // Set ViewBag.TotalRevenue after the loop finishes to avoid overwriting
                ViewBag.TotalRevenue = total;


                return View("Index", viewModel);

            }
            return RedirectToAction("LogIn");
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
            return RedirectToAction("LogIn");
        }


        public IActionResult Courses()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                IEnumerable<Course> courses = _db.Courses;
                return View(courses);

            }
            return RedirectToAction("LogIn");
        }

        public IActionResult AddCourse()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                return View();

            }
            return RedirectToAction("LogIn");
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
            return RedirectToAction("LogIn");
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
            return RedirectToAction("LogIn");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Exam(Exam obj)
        {
            if (ModelState.IsValid)
            {
                _db.Exams.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View();


        }

        public IActionResult AddStudent()
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
        public IActionResult AddStudent(Student obj, IFormFile stdImage)
        {
            if (stdImage != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                string filepath = Path.Combine(path, stdImage.FileName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                
                var stream = new FileStream(filepath, FileMode.Create);
                stdImage.CopyTo(stream);
                string? filename = stdImage.FileName;
                obj.Picture = filename;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    

                    _db.Students.Add(obj);
                    _db.SaveChanges();

                    return RedirectToAction("Index", "Admin");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the student details.");
                    
                    return View(ex);
                }
            }

            
            return View(obj);
        }

        public IActionResult ChangeStudent(int? id)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var studentData = _db.Students.Find(id);

                if (studentData == null)
                {
                    return NotFound();
                }

                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                ViewBag.Student = _db.Students.ToList();
                return View(studentData);
            }

            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStudent(Student updatedStudent, IFormFile stdImage)
        {
            if (ModelState.IsValid)
            {
                var existingStudent = _db.Students.FirstOrDefault(s => s.StudentId == updatedStudent.StudentId);

            if (existingStudent == null)
            {
                return NotFound(); // Handle if the student is not found
            }

            _db.Entry(existingStudent).State = EntityState.Modified;

                // Update the existing student entity with the changes from updatedStudent
                existingStudent.RollNumber = updatedStudent.RollNumber;
            existingStudent.FirstName = updatedStudent.FirstName;
            existingStudent.MiddleName = updatedStudent.MiddleName;
            existingStudent.LastName = updatedStudent.LastName;
            existingStudent.DateOfBirth = updatedStudent.DateOfBirth;
            existingStudent.Address = updatedStudent.Address;
            existingStudent.Email = updatedStudent.Email;
            existingStudent.PhoneNumber = updatedStudent.PhoneNumber;
            existingStudent.Picture = updatedStudent.Picture ?? existingStudent.Picture;
            existingStudent.Password = updatedStudent.Password;
            existingStudent.Accept = updatedStudent.Accept;

            if (stdImage != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                string filepath = Path.Combine(path, stdImage.FileName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var stream = new FileStream(filepath, FileMode.Create);
                stdImage.CopyTo(stream);
                string? filename = stdImage.FileName;
                    existingStudent.Picture = filename;
            }

            
                _db.Students.Update(existingStudent);
                _db.SaveChanges();
                return RedirectToAction("Student");
            }

            return RedirectToAction ("Index");
            
        }

    
        public IActionResult Student()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                IEnumerable<Student> obj = _db.Students;
                return View(obj);
            }

            return RedirectToAction("LogIn");
        }

        public IActionResult DeleteStudent (int? id)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                var _id = _db.Students.Find(id);

                if (id != null)
                {
                    _db.Students.Remove(_id);
                    _db.SaveChanges();
                    return RedirectToAction("Student");
                }
            }

            return BadRequest();
        }

        
        public IActionResult MsgRead (int? id)
        {
            var userContact = _db._Contact.FirstOrDefault(s => s.ContactId == id);

            _db.Entry(userContact).State = EntityState.Modified;

            userContact.Read = true;
            _db._Contact.Update(userContact);
            _db.SaveChanges();
            return RedirectToAction("Index", "Admin");

        }

        public IActionResult Contact ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                IEnumerable<Contact> data = _db._Contact.ToList();
                return View(data);
            }

            return RedirectToAction("LogIn");
        }
    }
}
