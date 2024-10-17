using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Models;
using Microsoft.AspNetCore.Mvc;

namespace BIZFEST_Event.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TicketController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            int? EventId = Convert.ToInt32(HttpContext.Request.Query["EventId"]);
            int UserId = Convert.ToInt32(HttpContext.Request.Query["UserId"]);

            var User = _db.UserRegistration.Where(x => x.Id == UserId).FirstOrDefault();
            var Event = _db.UserEvent.Where(y => y.Id.Equals(EventId)).FirstOrDefault();

            UserEventDetail objUserDetail = new UserEventDetail();

            if (User != null && Event != null)
            {
                objUserDetail.Name = User.Name;
                objUserDetail.StartDate = Event.StartDate;
                objUserDetail.Location = Event.Location;
                objUserDetail.ContactNo = User.ContactNo;
                objUserDetail.BrCodeURL = User.BrCodeURL;
                objUserDetail.EventName = Event.EventName;
                objUserDetail.Email = User.EmailId;
                objUserDetail.City = User.City;
            }

            return View(objUserDetail);
        }
    }
}
