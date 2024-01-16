﻿using Microsoft.AspNetCore.Authorization;
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
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

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
                ViewBag.Classes = _db._Class.ToList();
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
                ViewBag.Classes = _db._Class.ToList();
                return View(studentData);
            }
            TempData["failed"] = "Please Log In!";
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
                existingStudent.RollNumber = updatedStudent.RollNumber ?? existingStudent.RollNumber;
            existingStudent.FirstName = updatedStudent.FirstName ?? existingStudent.FirstName;
            existingStudent.MiddleName = updatedStudent.MiddleName ?? existingStudent.MiddleName;
            existingStudent.LastName = updatedStudent.LastName ?? existingStudent.LastName;
            existingStudent.DateOfBirth = updatedStudent.DateOfBirth ?? existingStudent.DateOfBirth;
            existingStudent.Address = updatedStudent.Address ?? existingStudent.Address;
            existingStudent.Email = updatedStudent.Email ?? existingStudent.Email;
            existingStudent.PhoneNumber = updatedStudent.PhoneNumber ?? existingStudent.PhoneNumber;
            existingStudent.Picture = updatedStudent.Picture ?? existingStudent.Picture;
            existingStudent.Password = updatedStudent.Password ?? existingStudent.Password;
            existingStudent.Accept = updatedStudent.Accept ?? existingStudent.Accept;
            existingStudent.Class = updatedStudent.Class ?? existingStudent.Class;

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
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        public IActionResult DeleteStudent (int? id)
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

        
        public IActionResult MsgRead (int? id)
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

        public IActionResult Contact ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                IEnumerable<Contact> data = _db._Contact.ToList();
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

                if(contact != null)
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

        public IActionResult _Branches ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                
                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult _Branches (Branch data)
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

                if(existingBranch != null)
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

       
        public IActionResult DeleteBranch (int? id)
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
                if(_db._AboutUs.FirstOrDefault() == null)
                {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
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

        public IActionResult EditAbout ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                var fetch = _db._AboutUs.FirstOrDefault();

                if(fetch == null)
                {
                    TempData["failed"] = "Can't edit because there's no content. Consider adding the content.";
                    return RedirectToAction("AboutUs");
                }

                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                
                ViewBag.Content = _db._AboutUs.FirstOrDefault(); 
                ViewBag.ImageOne = _db._AboutUs.FirstOrDefault().ImageOne; 
                ViewBag.ImageTwo = _db._AboutUs.FirstOrDefault().ImageTwo; 
                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAbout (About data) 
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
        public IActionResult UpdateAboutImage (IFormFile img_one, IFormFile img_two)
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

        public IActionResult DeleteAbout ()
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

        public IActionResult Faculty ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {           
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

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
        public IActionResult FacultyImage (int _id , IFormFile img)
        {
            if(HttpContext.Session.GetString("s_email") != null)
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

        public IActionResult DeleteFaculty (int? id)
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

        public IActionResult AddResult ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                var fetch_individual_exam = _db.Exams.ToList();

                ViewBag.Exam = fetch_individual_exam;


                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddResult (Result data)
        {
            if (ModelState.IsValid)
            {
                _db.Results.Add(data);
                _db.SaveChanges();
                TempData["success"] = "Result Has Been Created!";
                return RedirectToAction("AddResult");
            }
            
            TempData["failed"] = "Database Error";
            return RedirectToAction("AddResult");
        }

        public IActionResult CheckResults()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                var resultDetails = _db.Results.ToList();

                if (resultDetails != null && resultDetails.Any())
                {
                    var studentDetailsList = new List<Student>();

                    foreach (var i in resultDetails)
                    {
                        var studentDetails = _db.Students.FirstOrDefault(x => x.StudentId == i.StudentId);

                        if (studentDetails != null)
                        {
                            studentDetailsList.Add(studentDetails);
                        }
                        else
                        {
                            TempData["failed"] = "No Student";
                        }
                    }

                    ViewBag.StudentDetailsList = studentDetailsList;
                }
                else
                {
                    TempData["failed"] = "No Results";
                    // Add logging to help identify the issue
                    Console.WriteLine("No results found or resultDetails is null.");
                }

                return View();
            }

            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        public IActionResult Classes ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                ViewBag.Courses = _db.Courses.ToList();
                ViewBag.Classes = _db._Class.ToList();
                return View();
            }
            TempData["failed"] = "Please Log In!";
            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Classes(Class obj)
        {
            if(ModelState.IsValid)
            {
                _db._Class.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Class Created Successfully!";
                return RedirectToAction("Classes");
            }
            TempData["failed"] = "Database Issue!";
            return RedirectToAction("Classes");
        }

    }
}
