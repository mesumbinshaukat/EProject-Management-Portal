using Microsoft.AspNetCore.Mvc;
using Symphony_LTD.Data;

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


    }
}
