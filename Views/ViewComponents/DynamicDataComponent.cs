using Microsoft.AspNetCore.Mvc;
using Symphony_LTD.Data;
using Symphony_LTD.Models;

namespace Symphony_LTD.Views.ViewComponents
{
    public class DynamicDataComponent : ViewComponent
    {

        private readonly DbSymphonyContext _db;

        public DynamicDataComponent (DbSymphonyContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Fetch your dynamic data here
            IEnumerable<Contact> data = _db._Contact.ToList();
            ViewBag.NotificationCount = _db._Contact.Count(s => s.Read == false);

            return View("/Views/Components/DynamicDataComponent/Default.cshtml", data);

            //return View(data);
        }
    }
}
