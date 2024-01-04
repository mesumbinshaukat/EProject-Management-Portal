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
                    return RedirectToAction("Index", "Home");
                }
            
            return View(obj);
        }


    }
}
