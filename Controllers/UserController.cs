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

        public IActionResult EntranceExam ()
        {            
            ViewBag.EntranceExam = _db._EntranceExamList.ToList();
            ViewBag.TotalCourses = _db.Courses.ToList();
            ViewBag.CurrentDate = DateTime.Now;
            return View();
        }

        public IActionResult AboutUs()
        {
            IEnumerable<About> data = _db._AboutUs.ToList();
            ViewBag.Faculty = _db._Faculty.ToList();
            ViewBag.FacultyCount = _db._Faculty.Count();
            ViewBag.TotalCourses = _db.Courses.Count();
            ViewBag.TotalStudents = _db.Students.Count();
            ViewBag.Branches = _db.Branches.Count();
            
            return View(data);
        }

        public IActionResult Courses()
        {
            ViewBag.Courses = _db.Courses.ToList();


            return View();
        }

        [HttpGet]
        public IActionResult GetResult(int? roll)
        {
            if(roll != null)
            {
                var fetch_student = _db.Students.FirstOrDefault(x => x.RollNumber == roll);

                if(fetch_student != null)
                {
                var fetch_result = _db._EntranceExamResult.FirstOrDefault(x => x.StudentId == fetch_student.StudentId);

                    var fetch_entrance_exam = _db._EntranceExam.FirstOrDefault(x => x.StudentId == fetch_student.StudentId);

                    if(fetch_entrance_exam != null && fetch_entrance_exam.Pending == true)
                    {
                        TempData["invalid_roll"] = "Exam is scheduled but it is still not conducted yet, please contact the examination department for further details or wait at least 24 hours.";
                        return RedirectToAction("EntranceExam");
                    }
                    if(fetch_result != null)
                    {
                        var fetch_course = _db.Courses.FirstOrDefault(x => x.Id == fetch_result.Course);
                        if(fetch_course == null)
                        {
                            TempData["course"] = "No Course Found";
                            TempData["course_fee"] = "No Course Found";
                            TempData["course_detail"] = "No Course Found";
                            TempData["course_topics"] = "No Course Found";
                        }
                        TempData["course"] = fetch_course.CourseName;
                        TempData["course_fee"] = fetch_course.CourseFee.ToString();
                        TempData["course_detail"] = fetch_course.CourseDetails;
                        TempData["course_topics"] = fetch_course.TopicsCovered;
                        if (fetch_student.MiddleName != null)
                        {
                            TempData["student"] = fetch_student.FirstName + " " + fetch_student.MiddleName + " " + fetch_student.LastName;
                        }
                        TempData["student"] = fetch_student.FirstName + " " + fetch_student.LastName;
                        TempData["roll"] = roll.ToString();
                        TempData["class"] = fetch_student.Class.ToString();

                        if (fetch_entrance_exam == null || fetch_entrance_exam.TotalMarks == null)
                        {
                            TempData["total_marks"] = "Total marks not defined.";
                        }
                        TempData["exam"] = fetch_entrance_exam.ExamName;
                        TempData["exam_detail"] = fetch_entrance_exam.Description;
                        TempData["total_marks"] = fetch_entrance_exam.TotalMarks.ToString();
                        TempData["date"] = fetch_entrance_exam.Date.ToString();

                        TempData["marks_obtained"] = fetch_result.MarksObtained.ToString();
                        TempData["comment"] = fetch_result.Comments;
                        TempData["validate"] = "true";
                        return RedirectToAction("EntranceExam");
                    }
                    TempData["invalid_roll"] = "Result Not Found.";
                    return RedirectToAction("EntranceExam");

                }
                TempData["invalid_roll"] = "Student Not Found.";
                return RedirectToAction("EntranceExam");
            }
            TempData["invalid_roll"] = "Invalid Roll Number.";
            return RedirectToAction("EntranceExam");
        }
    }
}
