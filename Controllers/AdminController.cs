using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Symphony_LTD.Data;
using Symphony_LTD.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
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

                var adminData = _db._Admin.ToList();
                var contactData = _db._Contact.ToList();
                
                var viewModel = new AdminAndContact
                {
                    Admin = adminData,
                    Contact = contactData
                };

                var totalCourses = _db.Courses.Count();
                ViewBag.TotalCourses = totalCourses;

                var studentPropertiesCount = _db.Students.Count();
                ViewBag.TotalColumns = studentPropertiesCount;

                var totalContacts = _db._Contact.Count();
                ViewBag.TotalContacts = totalContacts;

                var allFaculties = _db._Faculty.ToList();
                ViewBag.TotalFaculties = allFaculties;

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

                }

                ViewBag.TotalRevenue = total;

                var user_details = _db._Admin.FirstOrDefault();

                if(user_details != null)
                {
                ViewBag.Username = user_details.Name;

                }

                return View("Index", viewModel);

            }
            TempData["failed"] = "Please Log In!";
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
        [ValidateAntiForgeryToken]
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
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }


        public IActionResult Courses()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                IEnumerable<Course> courses = _db.Courses;
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View(courses);

            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        public IActionResult AddCourse()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();

            }
            TempData["failed"] = "Please Log In!";
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
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View(studentFromDb);



            }
            TempData["failed"] = "Please Log In!";
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
                ViewBag.Student = _db.Students.ToList();
                ViewBag.Exam = _db.Exams.ToList();
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();
            }
            TempData["failed"] = "Please Log In!";
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
                TempData["success"] = "Exam Scheduled!";
                return RedirectToAction("Exam");
            }

            return View();

        }

        public IActionResult AddStudent()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                ViewBag.Classes = _db._Class.ToList();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();
            }
            TempData["failed"] = "Please Log In!";
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
                    TempData["success"] = "Student Added!";
                    return RedirectToAction("Student");
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
                ViewBag.Password = _db.Students.FirstOrDefault(x => x.StudentId == id).Password;
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                ViewBag.Classes = _db._Class.ToList();
                return View(studentData);
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStudent(Student updatedStudent)
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
                existingStudent.RollNumber = updatedStudent.RollNumber ?? existingStudent.RollNumber;
                existingStudent.FirstName = updatedStudent.FirstName ?? existingStudent.FirstName;
                existingStudent.MiddleName = updatedStudent.MiddleName ?? existingStudent.MiddleName;
                existingStudent.LastName = updatedStudent.LastName ?? existingStudent.LastName;
                existingStudent.DateOfBirth = updatedStudent.DateOfBirth ?? existingStudent.DateOfBirth;
                existingStudent.Address = updatedStudent.Address ?? existingStudent.Address;
                existingStudent.Email = updatedStudent.Email ?? existingStudent.Email;
                existingStudent.PhoneNumber = updatedStudent.PhoneNumber ?? existingStudent.PhoneNumber;
                existingStudent.Password = updatedStudent.Password ?? existingStudent.Password;
                existingStudent.Accept = updatedStudent.Accept ?? existingStudent.Accept;
                existingStudent.Class = updatedStudent.Class ?? existingStudent.Class;




                _db.Students.Update(existingStudent);
                _db.SaveChanges();
                TempData["success"] = "Edited!";
                return RedirectToAction("Student");
            }
            TempData["failed"] = "Database issue!";
            return RedirectToAction("Student");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStudentImage(int? _id, IFormFile stdImage)
        {

            if (HttpContext.Session.GetString("s_email") != null)
            {
                var existingStudent = _db.Students.FirstOrDefault(x => x.StudentId == _id);
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

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
                    existingStudent.Picture = filename ?? existingStudent.Picture;
                    _db.Students.Update(existingStudent);
                    _db.SaveChanges();
                    TempData["success"] = "Image Updated!";
                    return RedirectToAction("Student");
                }
            }
            if (stdImage == null)
            {
                TempData["success"] = "Invalid Image or No Image Selected.";
                return RedirectToAction("Student");
            }

            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        public IActionResult Student()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                ViewBag.Student = _db.Students.ToList();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        public IActionResult DeleteStudent(int? id)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                var _id = _db.Students.Find(id);

                if (id != null && _id != null)
                {
                    _db.Students.Remove(_id);
                    _db.SaveChanges();
                    TempData["success"] = "Student Deleted!";
                    return RedirectToAction("Student");
                }
                TempData["failed"] = "Student can't be deleted due to invalid null or invalid ID.";
                return RedirectToAction("Student");
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }


        public IActionResult MsgRead(int? id)
        {
            var userContact = _db._Contact.FirstOrDefault(s => s.ContactId == id);

            if (userContact != null)
            {
                _db.Entry(userContact).State = EntityState.Modified;
                userContact.Read = true;
                _db.SaveChanges();
                TempData["success"] = "Msg Seen";

            }

            return RedirectToAction("Contact");

        }

        public IActionResult Contact()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                IEnumerable<Contact> data = _db._Contact.ToList();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View(data);
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        public IActionResult DeleteContact(int? id)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {

                var contact = _db._Contact.FirstOrDefault(con => con.ContactId == id);

                if (contact != null)
                {
                    _db._Contact.Remove(contact);
                    _db.SaveChanges();
                    TempData["success"] = "Successfuly Deleted!";
                    return RedirectToAction("Contact");

                }
                TempData["failed"] = "Null ID Error!";
                return RedirectToAction("Contact");
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        public IActionResult _Branches()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Branches(Branch data)
        {
            if (ModelState.IsValid)
            {
                _db.Branches.Add(data);
                _db.SaveChanges();
                TempData["success"] = "Branch Added " + data.Branches;
                return RedirectToAction("ViewBranches");
            }
            TempData["failed"] = "Database error. Please contact the developer or support team.";
            return RedirectToAction("Index");
        }


        public IActionResult ViewBranches()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                ViewBag.Branch = _db.Branches.ToList();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ViewBranches(Branch obj, int? id)
        {
            if (ModelState.IsValid)
            {

                var existingBranch = _db.Branches.FirstOrDefault(b => b.Id == id);

                if (existingBranch != null)
                {
                    existingBranch.Branches = obj.Branches ?? existingBranch.Branches;
                    existingBranch.Address = obj.Address ?? existingBranch.Address;
                    existingBranch.Code = obj.Code ?? existingBranch.Code;
                    existingBranch.Postal = obj.Postal ?? existingBranch.Postal;
                    existingBranch.City = obj.City ?? existingBranch.City;
                    existingBranch.Country = obj.Country ?? existingBranch.Country;

                    _db.Branches.Update(existingBranch);
                    _db.SaveChanges();

                    TempData["success"] = "Branch Updated";

                    return RedirectToAction("ViewBranches");
                }

                TempData["failed"] = "Unexpected Error Occured";


                return RedirectToAction("ViewBranches");
            }
            TempData["failed"] = "Database Error";
            return RedirectToAction("ViewBranches");
        }


        public IActionResult DeleteBranch(int? id)
        {
            var branch = _db.Branches.FirstOrDefault(a => a.Id == id);
            if (branch != null)
            {
                _db.Branches.Remove(branch);
                _db.SaveChanges();
                TempData["success"] = "Branch Deleted";
                return RedirectToAction("ViewBranches");
            }
            TempData["failed"] = "Branch not deleted. Error: Trying to delete null value.";
            return RedirectToAction("ViewBranches");
        }

        public IActionResult AboutUs()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                if (_db._AboutUs.FirstOrDefault() == null)
                {
                    ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                    ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                    var user_details = _db._Admin.FirstOrDefault();

                    if (user_details != null)
                    {
                        ViewBag.Username = user_details.Name;

                    }
                    return View();

                }
                TempData["failed"] = "Main About page content is already in the database. You can only modify it, for adding new content again, consider deleting current content.";
                return RedirectToAction("EditAbout");
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AboutUs(About data, IFormFile img_one, IFormFile img_two)
        {
            if (img_one != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/media/about");
                string filepath = Path.Combine(path, img_one.FileName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                var stream = new FileStream(filepath, FileMode.Create);
                img_one.CopyTo(stream);
                string? filename = img_one.FileName;
                data.ImageOne = filename;
            }
            if (img_two != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/media/about");
                string filepath = Path.Combine(path, img_two.FileName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                var stream = new FileStream(filepath, FileMode.Create);
                img_two.CopyTo(stream);
                string? filename = img_two.FileName;
                data.ImageTwo = filename;
            }

            if (ModelState.IsValid)
            {
                _db._AboutUs.Add(data);
                _db.SaveChanges();
                TempData["success"] = "Content Added To About Us page.";
                return RedirectToAction("Index");
            }
            TempData["failed"] = "Unexpected Error Occurred.";
            return View();
        }

        public IActionResult EditAbout()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                var fetch = _db._AboutUs.FirstOrDefault();

                if (fetch == null)
                {
                    TempData["failed"] = "Can't edit because there's no content. Consider adding the content.";
                    return RedirectToAction("AboutUs");
                }

                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                ViewBag.Content = _db._AboutUs.FirstOrDefault();
                ViewBag.ImageOne = _db._AboutUs.FirstOrDefault().ImageOne;
                ViewBag.ImageTwo = _db._AboutUs.FirstOrDefault().ImageTwo;
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAbout(About data)
        {

            if (ModelState.IsValid)
            {
                var existing = _db._AboutUs.FirstOrDefault();

                if (existing != null)
                {
                    existing.HeadingOne = data.HeadingOne ?? existing.HeadingOne;
                    existing.HeadingTwo = data.HeadingTwo ?? existing.HeadingTwo;
                    existing.ParagraphOne = data.ParagraphOne ?? existing.ParagraphOne;
                    existing.ParagraphTwo = data.ParagraphTwo ?? existing.ParagraphTwo;
                    existing.ParagraphThree = data.ParagraphThree ?? existing.ParagraphThree;
                    existing.ParagraphFour = data.ParagraphFour ?? existing.ParagraphFour;

                    _db._AboutUs.Update(existing);
                    _db.SaveChanges();
                    TempData["success"] = "About Us Page Edited Successfully.";
                    return RedirectToAction("EditAbout");
                }
                TempData["failed"] = "Error: There's no existing row selected.";
                return RedirectToAction("EditAbout");

            }
            TempData["failed"] = "Unexpected Error Occurred.";
            return RedirectToAction("EditAbout");
        }

        [HttpPost]
        public IActionResult UpdateAboutImage(IFormFile img_one, IFormFile img_two)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                var x = _db._AboutUs.FirstOrDefault();
                if (img_one != null && img_one.FileName != x.ImageOne)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/media/about");
                    string filepath = Path.Combine(path, img_one.FileName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var stream = new FileStream(filepath, FileMode.Create);
                    img_one.CopyTo(stream);
                    string? filename = img_one.FileName;
                    x.ImageOne = filename;

                    _db._AboutUs.Update(x);
                    _db.SaveChanges();
                    TempData["success"] = "Image Edited Successfully.";
                    return RedirectToAction("EditAbout");
                }

                if (img_two != null && img_two.FileName != x.ImageTwo)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/media/about");
                    string filepath = Path.Combine(path, img_two.FileName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var stream = new FileStream(filepath, FileMode.Create);
                    img_two.CopyTo(stream);
                    string? filename = img_two.FileName;
                    x.ImageTwo = filename;

                    _db._AboutUs.Update(x);
                    _db.SaveChanges();
                    TempData["success"] = "Image Edited Successfully.";
                    return RedirectToAction("EditAbout");
                }

                TempData["failed"] = "Image update failed!";
                return RedirectToAction("EditAbout");
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        public IActionResult DeleteAbout()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                var fetch = _db._AboutUs.FirstOrDefault();

                if (fetch != null)
                {
                    _db._AboutUs.Remove(fetch);
                    _db.SaveChanges();
                    TempData["success"] = "Successfully deleted!";
                    return RedirectToAction("AboutUs");
                }
                TempData["failed"] = "Can't delete due to null or inavlid value.";
                return RedirectToAction("EditAbout");
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        public IActionResult Faculty()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                ViewBag.Faculty = _db._Faculty.ToList();

                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Faculty(Faculty obj, IFormFile img)
        {
            if (img != null)
            {
                try
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/media/faculty");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + img.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        img.CopyTo(stream);
                    }

                    obj.Image = uniqueFileName;
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("Image", "Error uploading the image" + ex);
                }
            }

            if (ModelState.IsValid)
            {
                _db._Faculty.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Faculty Data Successfully Added.";
                return RedirectToAction("Faculty");
            }

            TempData["failed"] = "Error, while inserting data to database.";
            return RedirectToAction("Faculty");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditFaculty(Faculty data)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                var existing_faculty = _db._Faculty.Where(x => x.Id == data.Id).FirstOrDefault();

                if (existing_faculty != null)
                {
                    if (ModelState.IsValid)
                    {
                        existing_faculty.Name = data.Name ?? existing_faculty.Name;
                        existing_faculty.Role = data.Role ?? existing_faculty.Role;
                        //existing_faculty.Image = data.Image ?? existing_faculty.Image;

                        _db._Faculty.Update(existing_faculty);
                        _db.SaveChanges();
                        TempData["success"] = "Updated!";
                        return RedirectToAction("Faculty");
                    }

                    TempData["failed"] = "Error!";
                    return RedirectToAction("Faculty");
                }
                TempData["failed"] = "There's no faculty.";
                return RedirectToAction("Faculty");
            }

            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FacultyImage(int _id, IFormFile img)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                var existing_path = _db._Faculty.FirstOrDefault(x => x.Id == _id);

                if (existing_path != null)
                {
                    try
                    {
                        if (img != null)
                        {
                            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/media/faculty");
                            string filepath = Path.Combine(path, img.FileName);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            var stream = new FileStream(filepath, FileMode.Create);
                            img.CopyTo(stream);
                            string? filename = img.FileName;
                            existing_path.Image = filename;

                            _db._Faculty.Update(existing_path);
                            _db.SaveChanges();

                            TempData["success"] = "Image Update!";
                            return RedirectToAction("Faculty");
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["failed"] = ex.Message;
                        ModelState.AddModelError("Image", "Error uploading the image: " + ex.Message);
                    }
                }


                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        public IActionResult DeleteFaculty(int? id)
        {

            if (HttpContext.Session.GetString("s_email") != null)
            {
                var existing_faculty = _db._Faculty.FirstOrDefault(f => f.Id == id);
                if (id != null && existing_faculty != null)
                {
                    _db._Faculty.Remove(existing_faculty);
                    _db.SaveChanges();
                    TempData["success"] = "Successfully Deleted!";
                    return RedirectToAction("Faculty");
                }

                TempData["failed"] = "Id is invalid or null!";
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("Faculty");
        }

        public IActionResult AddResult()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                ViewBag.IndividualStudentExam = _db.Exams.ToList();

                ViewBag.Student = _db.Students.ToList();

                var user_details = _db._Admin.FirstOrDefault();

                var result = _db.Results.ToList();
                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddResult(Result data)
        {
            var existing_student_result = _db.Results.FirstOrDefault(x => x.StudentId == data.StudentId);

            if(existing_student_result != null)
            {
                TempData["failed"] = "Result For This Student Is Already Created.";
                return RedirectToAction("AddResult");
            }

            if (data.SubjectId == null)
            {
                var student = _db.Exams.FirstOrDefault(x => x.StudentId == data.StudentId);

                var course = _db.Courses.FirstOrDefault(x => x.Id == student.CourseId);

                if(course != null)
                {
                    data.SubjectId = course.Id;

                    if (data != null)
                    {
                        _db.Results.Add(data);
                        _db.SaveChanges();
                        TempData["success"] = "Result Has Been Created!";
                        return RedirectToAction("AddResult");
                    }
                    TempData["failed"] = "Database Error!";
                    return RedirectToAction("AddResult");
                }
                TempData["failed"] = "No Course Found For This Individual Student!";
                return RedirectToAction("AddResult");
            }
            TempData["failed"] = "Course Id Error!";
            return RedirectToAction("AddResult");
        }

        public IActionResult CheckResults()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                ViewBag.IndividualExamResult = _db.Results.ToList();

                ViewBag.IndividualExam = _db.Exams.ToList();

                ViewBag.Course = _db.Courses.ToList();

                ViewBag.Student = _db.Students.ToList();

                ViewBag.EntranceExamResult = _db._EntranceExamResult.ToList();

                ViewBag.EntranceExam = _db._EntranceExam.ToList();

                ViewBag.CourseExamResult = _db._CourseExamResult.ToList();

                ViewBag.CourseExam = _db._CourseExam.ToList();

                ViewBag.Class = _db._Class.ToList();

                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;
                }     
                return View();
            }

            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        public IActionResult Classes()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                ViewBag.Courses = _db.Courses.ToList();
                ViewBag.Classes = _db._Class.ToList();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Classes(Class obj)
        {
            if (ModelState.IsValid)
            {
                _db._Class.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Class Created Successfully!";
                return RedirectToAction("Classes");
            }
            TempData["failed"] = "Database Issue!";
            return RedirectToAction("Classes");
        }

        public IActionResult CourseExam()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                ViewBag.Courses = _db.Courses.ToList();
                ViewBag.Classes = _db._Class.ToList();
                ViewBag.ScheduledExams = _db._CourseExam.ToList();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CourseExam(CourseExam data)
        {
            if (ModelState.IsValid)
            {
                _db._CourseExam.Add(data);
                _db.SaveChanges();
                TempData["success"] = "Exam Scheduled!";
                return RedirectToAction("CourseExam");
            }
            TempData["failed"] = "Can't schedule exam due to unexpected error.";
            return View("CourseExam");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCourseExam (int? id)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                if (id != null)
                {
                    var fetchDetails = _db._CourseExam.FirstOrDefault(x => x.Id == id);
                    if(fetchDetails != null)
                    {
                        _db._CourseExam.Remove(fetchDetails);
                        _db.SaveChanges();
                        TempData["success"] = "Successfully Deleted!";
                        return RedirectToAction("CourseExam");
                    }
                    TempData["failed"] = "Can't delete because can't find the specified field.";
                    return RedirectToAction("CourseExam");
                }
                TempData["failed"] = "Invalid Id or Null Id error.";
                return RedirectToAction("CourseExam");
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
           
        }

        public IActionResult EditCourseExam (int? id)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }

                if (id != null)
                {
                    var existing_details = _db._CourseExam.FirstOrDefault(x => x.Id == id);
                    if(existing_details != null)
                    {
                        var exisitng_class = _db._Class.FirstOrDefault(x => x._Class == existing_details.Class);
                        if(exisitng_class != null)
                        {
                            var existing_course = _db.Courses.FirstOrDefault(x => x.Id == exisitng_class.Course);
                            if (existing_details != null)
                            {
                                ViewBag.Id = existing_details.Id;
                                ViewBag.ExamName = existing_details.ExamName;
                                ViewBag.Class = existing_details.Class;
                                ViewBag.TotalScore = existing_details.TotalScore;
                                ViewBag.Description = existing_details.Description;
                                ViewBag.Date = existing_details.Date;
                                ViewBag.Course = existing_course.CourseName;
                                ViewBag.Courses = _db.Courses.ToList();
                                ViewBag.Classes = _db._Class.ToList();
                                ViewBag.ScheduledExams = _db._CourseExam.ToList();

                                return View();
                            }
                            TempData["failed"] = "Can't find the course.";
                            return RedirectToAction("CourseExam");

                        }
                        TempData["failed"] = "Can't find the course exam.";
                        return RedirectToAction("CourseExam");
                    }
                    TempData["failed"] = "Can't edit because there's no matching id.";
                    return RedirectToAction("CourseExam");

                }
                TempData["failed"] = "Invalid Id or Null Id error.";
                return RedirectToAction("CourseExam");
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCourseExam (CourseExam data)
        {
            if(ModelState.IsValid)
            {
                var existing_exam = _db._CourseExam.FirstOrDefault(x => x.Id == data.Id);

                if(existing_exam != null)
                {
                    existing_exam.ExamName = data.ExamName ?? existing_exam.ExamName;
                    existing_exam.Class = data.Class ?? existing_exam.Class;
                    existing_exam.Description = data.Description ?? existing_exam.Description;
                    existing_exam.Date = data.Date ?? existing_exam.Date;
                    existing_exam.TotalScore = data.TotalScore ?? existing_exam.TotalScore;

                    _db._CourseExam.Update(existing_exam);
                    _db.SaveChanges();
                    TempData["success"] = "Scheduled Exam Modified/Updated!";
                    return RedirectToAction("CourseExam");
                }
                TempData["failed"] = "No Scheduled Exam Found On The Specified Id: " + data.Id;
                return RedirectToAction("CourseExam");
            }
            TempData["failed"] = "Database error.";
            return RedirectToAction("CourseExam");
        }

        public IActionResult EntranceExam ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                ViewBag.Courses = _db.Courses.ToList();
                ViewBag.EntranceExam = _db._EntranceExam.ToList();
                ViewBag.Student = _db.Students.ToList();
                ViewBag.EntranceExamList = _db._EntranceExamList.ToList();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EntranceExam (EntranceExam data)
        {
            var convertToInt = int.Parse(data.ExamName);

            var fetch_entrance_exam_list = _db._EntranceExamList.FirstOrDefault(x => x.Id == convertToInt);

            if (fetch_entrance_exam_list != null)
            {
                data.ExamName = fetch_entrance_exam_list.Name;
                data.TotalMarks = fetch_entrance_exam_list.Marks;
                data.Description = fetch_entrance_exam_list.Description;
                data.Date = fetch_entrance_exam_list.Date;
            }
            else
            {
                TempData["failed"] = "No Entrance Exam Found";
                return RedirectToAction("EntranceExam");
            }

            if (data != null)
            {                
                    _db._EntranceExam.Add(data);
                    _db.SaveChanges();
                    TempData["success"] = "Entrance Exam Scheduled Successfully.";
                    return RedirectToAction("EntranceExam"); 
            }
            TempData["failed"] = "Can't schedule Exam Due To Database Error!";
            return RedirectToAction("EntranceExam");
        }

        public IActionResult Home ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                var existing_data = _db._HomeSectionOne.FirstOrDefault();
                if (existing_data != null)
                {
                    ViewBag.H5 = existing_data.H5;
                    ViewBag.H2 = existing_data.H2;
                    ViewBag.Paragraph = existing_data.Paragraph;
                    ViewBag.BtnVal = existing_data.BtnVal;
                    ViewBag.BtnAction = existing_data.BtnAction;
                    ViewBag.Img = existing_data.Img;
                }

                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Home (HomeSectionOne data, IFormFile _img)
        {
            var existing_data = _db._HomeSectionOne.FirstOrDefault();
            if (_img != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/media/home");
                string filepath = Path.Combine(path, _img.FileName);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var stream = new FileStream(filepath, FileMode.Create);
                _img.CopyTo(stream);
                string? filename = _img.FileName;
                data.Img = filename;
            }
            else
            {
                string defaultFilename = existing_data.Img;
                
                string extension = Path.GetExtension(defaultFilename)?.ToLowerInvariant();
               
                string contentType;
                switch (extension)
                {
                    case ".svg":
                        contentType = "image/svg+xml";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".jpg":
                    case ".jpeg":
                        contentType = "image/jpeg";
                        break;
                    default:
                        contentType = "application/octet-stream";
                        break;
                }

                _img = new FormFile(null, 0, 0, "Img", defaultFilename)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType,
                };

            }

            if (existing_data != null)
            {
                existing_data.H5 = data.H5 ?? existing_data.H5;
                existing_data.H2 = data.H2 ?? existing_data.H2;
                existing_data.Paragraph = data.Paragraph ?? existing_data.Paragraph;
                existing_data.BtnVal = data.BtnVal ?? existing_data.BtnVal;
                existing_data.BtnAction = data.BtnAction ?? existing_data.BtnAction;
                existing_data.Img = data.Img ?? existing_data.Img;

                    _db._HomeSectionOne.Update(existing_data);
                    _db.SaveChanges();
                    TempData["success"] = "Successfully Modified Home Page.";
                    return RedirectToAction("Home");
                

            }

            if (ModelState.IsValid)
            {               
                    if (data != null)
                    {
                        _db._HomeSectionOne.Add(data);
                        _db.SaveChanges();
                        TempData["success"] = "Successfully Modified Home Page.";
                        return RedirectToAction("Home");
                    }                    
               
                TempData["failed"] = "Values are invalid, or you've have left some fields null.";
                return RedirectToAction("Home");
                
            }
            TempData["failed"] = "Database Error!";
            return RedirectToAction("Home");
        }

        public IActionResult ExamEdit(int? id)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                var exam_details = _db.Exams.FirstOrDefault(x => x.Id == id);
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;
                }

                if (exam_details != null)
                {
                    ViewBag.Id = exam_details.Id;                    
                    ViewBag.ExamName = exam_details.ExamName;                    
                    ViewBag.Score = exam_details.Score;
                    ViewBag.StudentId = exam_details.StudentId;
                    ViewBag.CourseId = exam_details.CourseId;
                    ViewBag.Details = exam_details.Detail;

                    var student = _db.Students.FirstOrDefault(x => x.StudentId == exam_details.StudentId);

                    ViewBag.StudentFName = student.FirstName;
                    ViewBag.StudentMName = student.MiddleName;
                    ViewBag.StudentLName = student.LastName;
                    ViewBag.RollNo = student.RollNumber;

                    var course = _db.Courses.FirstOrDefault(x => x.Id == exam_details.CourseId);

                    ViewBag.CourseId = course.Id;
                    ViewBag.CourseName = course.CourseName;

                    ViewBag.Course = _db.Courses.ToList();
                    ViewBag.Student = _db.Students.ToList();
                    return View();
                }
                TempData["failed"] = "Invalid Id, can't find the the scheduled exam.";
                return RedirectToAction("Exam");
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExamEdit(Exam obj)
        {
            
            if (ModelState.IsValid)
            {
                var existing_exam = _db.Exams.FirstOrDefault(x => x.Id == obj.Id);

                if(existing_exam != null)
                {
                    existing_exam.ExamName = obj.ExamName ?? existing_exam.ExamName;
                    existing_exam.Score = obj.Score ?? existing_exam.Score;
                    existing_exam.StudentId = obj.StudentId ?? existing_exam.StudentId;
                    existing_exam.CourseId = obj.CourseId ?? existing_exam.CourseId;
                    existing_exam.Detail = obj.Detail ?? existing_exam.Detail;

                    _db.Exams.Update(existing_exam);
                    _db.SaveChanges();

                    TempData["success"] = "Updated Successfully!";
                    return RedirectToAction("Exam");
                }
                TempData["failed"] = "Can't find the scheduled exam.";
                return RedirectToAction("Exam");
            }
            TempData["failed"] = "Database Error! Model Is Not Valid.";
            return RedirectToAction("Exam");
        }
        
        public IActionResult DeleteExam(int? id)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }

                var existing_data = _db.Exams.FirstOrDefault(x => x.Id == id);

                if(existing_data != null)
                {
                    _db.Exams.Remove(existing_data);
                    _db.SaveChanges();
                    TempData["success"] = "Successfully deleted.";
                    return RedirectToAction("Exam");
                }

                TempData["failed"] = "Can't delete, because scheduled exam can't be found.";
                return RedirectToAction("Exam");

            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateExamSchedule(Exam data)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();                
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;
                }

                if (ModelState.IsValid)
                {
                    var existing_exam = _db.Exams.FirstOrDefault(x => x.Id == data.Id);

                    if(existing_exam != null)
                    {
                        existing_exam.ExamDate = data.ExamDate ?? existing_exam.ExamDate;

                        _db.Exams.Update(existing_exam);
                        _db.SaveChanges();
                        TempData["success"] = "Scheduled Date Updated!";
                        return RedirectToAction("Exam");
                    }
                    TempData["failed"] = "No Scheduled Exam Found.";
                    return RedirectToAction("Exam");
                }
                TempData["failed"] = "Database Error!";
                return RedirectToAction("Exam");

            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");

                 
        }

        public IActionResult IndividualStudentExamSchedule()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                ViewBag.Exams = _db.Exams.ToList();
                ViewBag.Student = _db.Students.ToList();
                ViewBag.Course = _db.Courses.ToList();

                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();

            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
         public IActionResult IndividualStudentExamSchedule(Exam data)
        {
            if (ModelState.IsValid)
            {
                var existing_student_exam = _db.Exams.FirstOrDefault(x => x.Id == data.Id);

                if(existing_student_exam != null)
                {
                    existing_student_exam.ExamName = data.ExamName ?? existing_student_exam.ExamName;
                    existing_student_exam.ExamDate = data.ExamDate ?? existing_student_exam.ExamDate;
                    existing_student_exam.StudentId = data.StudentId ?? existing_student_exam.StudentId;
                    existing_student_exam.CourseId = data.CourseId ?? existing_student_exam.CourseId;
                    existing_student_exam.Score = data.Score ?? existing_student_exam.Score;
                    existing_student_exam.Detail = data.Detail ?? existing_student_exam.Detail;
                    existing_student_exam.Pending = data.Pending ?? existing_student_exam.Pending;

                    _db.Exams.Update(existing_student_exam);
                    _db.SaveChanges();
                    TempData["success"] = "Successfully Updated!";
                    return RedirectToAction("IndividualStudentExamSchedule");
                }
                TempData["failed"] = "No Scheduled Exam Found.";
                return RedirectToAction("IndividualStudentExamSchedule");
            }
            TempData["failed"] = "Database Error!";
            return RedirectToAction("IndividualStudentExamSchedule");
        }

        public IActionResult EntranceExamSchedule()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                ViewBag._EntranceExam = _db._EntranceExam.ToList();                
                ViewBag.Courses = _db.Courses.ToList();

                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();

            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EntranceExamSchedule (EntranceExam data)
        {
            if (ModelState.IsValid)
            {
                var existing_entrance_exam = _db._EntranceExam.FirstOrDefault(x => x.Id == data.Id);

                if (existing_entrance_exam != null)
                {
                    existing_entrance_exam.ExamName = data.ExamName ?? existing_entrance_exam.ExamName;
                    existing_entrance_exam.StudentId = data.StudentId ?? existing_entrance_exam.StudentId;
                    existing_entrance_exam.TotalMarks = data.TotalMarks ?? existing_entrance_exam.TotalMarks;
                    existing_entrance_exam.Course = data.Course ?? existing_entrance_exam.Course;
                    existing_entrance_exam.Description = data.Description ?? existing_entrance_exam.Description;
                    existing_entrance_exam.Date = data.Date ?? existing_entrance_exam.Date;
                    existing_entrance_exam.Pending = data.Pending ?? existing_entrance_exam.Pending;                    

                    _db._EntranceExam.Update(existing_entrance_exam);
                    _db.SaveChanges();
                    TempData["success"] = "Successfully Updated!";
                    return RedirectToAction("EntranceExamSchedule");
                }
                TempData["failed"] = "No Scheduled Exam Found.";
                return RedirectToAction("EntranceExamSchedule");
            }
            TempData["failed"] = "Database Error!";
            return RedirectToAction("EntranceExamSchedule");
        }

        public IActionResult CourseExamSchedule ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                ViewBag._CourseExam = _db._CourseExam.ToList();
                ViewBag.Class = _db._Class.ToList();

                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();

            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CourseExamSchedule (CourseExam data)
        {
            if (ModelState.IsValid)
            {
                var existing_course_exam = _db._CourseExam.FirstOrDefault(x => x.Id == data.Id);

                if (existing_course_exam != null)
                {
                    existing_course_exam.ExamName = data.ExamName ?? existing_course_exam.ExamName;
                    existing_course_exam.Class = data.Class ?? existing_course_exam.Class;
                    existing_course_exam.TotalScore = data.TotalScore ?? existing_course_exam.TotalScore;
                    existing_course_exam.Description = data.Description ?? existing_course_exam.Description;
                    existing_course_exam.Date = data.Date ?? existing_course_exam.Date;
                    existing_course_exam.Pending = data.Pending ?? existing_course_exam.Pending;

                    _db._CourseExam.Update(existing_course_exam);
                    _db.SaveChanges();
                    TempData["success"] = "Successfully Updated!";
                    return RedirectToAction("CourseExamSchedule");
                }
                TempData["failed"] = "No Scheduled Exam Found.";
                return RedirectToAction("CourseExamSchedule");
            }
            TempData["failed"] = "Database Error!";
            return RedirectToAction("CourseExamSchedule");
        }

        public IActionResult EntranceResult()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                ViewBag.EntranceExam = _db._EntranceExam.ToList();

                ViewBag.Courses = _db.Courses.ToList();

                ViewBag.Student = _db.Students.ToList();

                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();

            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EntranceResult (EntranceExamResult data)
        {
            var existing_data = _db._EntranceExam.FirstOrDefault(i => i.StudentId == data.StudentId);

            if (existing_data != null)
            {
                data.Course = existing_data.Course;

                var already_existed_result = _db._EntranceExamResult.FirstOrDefault(x => x.StudentId == data.StudentId) ?? null;

                if(already_existed_result != null)
                {
                    TempData["failed"] = "Result Has Already Created. Please Select Other Student.";
                    return RedirectToAction("EntranceResult");
                }

                if (data != null)
                {
                    _db._EntranceExamResult.Add(data);
                    _db.SaveChanges();
                    TempData["success"] = "Result Created Successfully";
                    return RedirectToAction("EntranceResult");
                }
                TempData["failed"] = "No Existing Course Data Found.";
                return RedirectToAction("EntranceResult");
            }
            TempData["failed"] = "No Existing Data Found.";
            return RedirectToAction("EntranceResult");
        }

        public IActionResult CourseResult ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                ViewBag.CourseExam = _db._CourseExam.ToList();

                ViewBag.Courses = _db.Courses.ToList();

                ViewBag.Class = _db._Class.ToList();

                ViewBag.Student = _db.Students.ToList();

                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();

            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CourseResult (CourseExamResult data)
        {
            var student = _db.Students.FirstOrDefault(x => x.StudentId == data.StudentId);

                var existing_data = _db._CourseExam.FirstOrDefault(x => x.Class == student.Class);

                if (existing_data != null)
                {
                    var existing_student = _db.Students.FirstOrDefault(x => x.Class == existing_data.Class && x.StudentId == data.StudentId);
                    if (existing_student != null)
                    {
                        var course_exam_result = _db._CourseExamResult.FirstOrDefault(x => x.StudentId == existing_student.StudentId);

                        if (course_exam_result != null)
                        {
                            TempData["failed"] = "Please select different student because result for this student, " + existing_student.FirstName + " is already created.";
                            return RedirectToAction("CourseResult");
                        }

                        var existing_class = _db._Class.FirstOrDefault(x => x._Class == existing_student.Class);

                        if (existing_class != null)
                        {
                            data.Course = existing_class.Course;
                            data.Class = existing_class._Class;

                            _db._CourseExamResult.Add(data);
                            _db.SaveChanges();
                            TempData["success"] = "Result Successfully Created.";
                            return RedirectToAction("CourseResult");
                        }
                        TempData["failed"] = "No Class Found.";
                        return RedirectToAction("CourseResult");
                    }
                    TempData["failed"] = "No Existing Student Found.";
                    return RedirectToAction("CourseResult");
                }
                TempData["failed"] = "No Existing Course/Cass Exam Found.";
                return RedirectToAction("CourseResult");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditEntranceExam(EntranceExam data)
        {
            var convertToInt = int.Parse(data.ExamName);

            var fetch_entrance_exam_list = _db._EntranceExamList.FirstOrDefault(x => x.Id == convertToInt);

            if (fetch_entrance_exam_list != null)
            {
                data.ExamName = fetch_entrance_exam_list.Name;
                data.TotalMarks = fetch_entrance_exam_list.Marks;
                data.Description = fetch_entrance_exam_list.Description;
                data.Date = fetch_entrance_exam_list.Date;
            }
            else
            {
                TempData["failed"] = "No Entrance Exam Found";
                return RedirectToAction("EntranceExam");
            }

            var existing_exam = _db._EntranceExam.FirstOrDefault(x => x.Id == data.Id);
            if (existing_exam != null)
            {
                if (data.Date == null)
                {
                    data.Date = existing_exam.Date;
                }

                if (data != null)
                {
                    existing_exam.ExamName = data.ExamName ?? existing_exam.ExamName;
                    existing_exam.TotalMarks = data.TotalMarks ?? existing_exam.TotalMarks;
                    existing_exam.StudentId = data.StudentId ?? existing_exam.StudentId;
                    existing_exam.Course = data.Course ?? existing_exam.Course;
                    existing_exam.Date = data.Date ?? existing_exam.Date;
                    existing_exam.Description = data.Description ?? existing_exam.Description;

                    _db._EntranceExam.Update(existing_exam);
                    _db.SaveChanges();
                    TempData["success"] = "Modified!";
                    return RedirectToAction("EntranceExam");
                }
                TempData["failed"] = "Can't modify because no data found.";
                return RedirectToAction("EntranceExam");
            }
            
            TempData["failed"] = "Can't modify because no existing exam found.";
            return RedirectToAction("EntranceExam");
        }

        public IActionResult EntranceExamList()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                ViewBag.Exam = _db._EntranceExamList.ToList();

                ViewBag.CurrentDate = DateTime.Now;

                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                return View();

            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _EntranceExamList (EntranceExamList data)
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                var user_details = _db._Admin.FirstOrDefault();

                if (user_details != null)
                {
                    ViewBag.Username = user_details.Name;

                }
                if (ModelState.IsValid)
                {
                    _db._EntranceExamList.Add(data);
                    _db.SaveChanges();
                    TempData["success"] = "Entrance Exam Created!";
                    return RedirectToAction("EntranceExamList");
                }
                TempData["failed"] = "Database Error! Model Not Valid";
                return RedirectToAction("EntranceExamList");

            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");

            
        }

    }
}
