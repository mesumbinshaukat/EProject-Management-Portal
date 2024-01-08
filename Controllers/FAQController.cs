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


    }
}
