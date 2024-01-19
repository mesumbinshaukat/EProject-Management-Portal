using Microsoft.AspNetCore.Mvc;
using Symphony_LTD.Data;
using Symphony_LTD.Models;

namespace Symphony_LTD.Controllers
{
    public class FAQController : Controller
    {
        private readonly DbSymphonyContext _db;

        public FAQController (DbSymphonyContext db)
        {
            _db = db;
        }

        public IActionResult Index()
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

                IEnumerable<FAQ> faq = _db.FAQS.ToList();

                return View(faq);
            }

            return RedirectToAction("LogIn", "Admin");
        }

        public IActionResult AddFAQ ()
        {
            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();
                
                return View();
            }
            TempData["failed"] = "Please login!";
            return RedirectToAction("LogIn", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddFAQ (FAQ obj)
        {
            if (ModelState.IsValid)
            {
                _db.FAQS.Add(obj);
                _db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            return View(obj);
        }


        public IActionResult FAQS ()
        {

            IEnumerable<FAQ> data = _db.FAQS.ToList();
            if(data != null)
            {
               return View(data);           

            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["failed"] = "Null Id selected. Select any FAQ.";
                return RedirectToAction("Index");
            }

            if (HttpContext.Session.GetString("s_email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("s_email").ToString();
                ViewBag.Pass = HttpContext.Session.GetString("s_pass_verify").ToString();

                var faq = _db.FAQS.FirstOrDefault(x => x.Id == id);

                if (faq != null)
                {
                    ViewBag.Id = id;
                    return View(faq);
                }
                TempData["failed"] = "No FAQ available on the selected Id: " + id;
                return RedirectToAction("Index");
            }

            TempData["failed"] = "Please login!";
            return RedirectToAction("LogIn", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FAQ data)
        {
            if (ModelState.IsValid)
            {
                var existing = _db.FAQS.FirstOrDefault(x => x.Id == data.Id);

                if (existing != null)
                {
                    existing.Question = data.Question ?? existing.Question;
                    existing.Answer = data.Answer ?? existing.Answer;

                    _db.FAQS.Update(existing);
                    _db.SaveChanges();
                    TempData["success"] = "FAQ edited successfully!";
                    return RedirectToAction("Index");
                }

                TempData["failed"] = "No FAQ available on the selected Id: " + data.Id;
                return RedirectToAction("Index");
            }

            TempData["failed"] = "Model is not valid.";
            return RedirectToAction("Index");
        }

        public IActionResult Delete (int? id)
        {
            if(id != null)
            {
                var deleteFAQ = _db.FAQS.FirstOrDefault(x => x.Id == id);
                if (deleteFAQ != null)
                {
                    _db.FAQS.Remove(deleteFAQ);
                    _db.SaveChanges();
                    TempData["success"] = "Successfully deleted a FAQ!";
                    return RedirectToAction("Index");
                }
                TempData["failed"] = "No FAQ available on the specified ID: " + id + ". Can't delete any FAQ.";
                return RedirectToAction("Index");
            }
            TempData["failed"] = "No Id Selected. Can't delete any FAQ.";
            return RedirectToAction("Index");
        }
    }
}
