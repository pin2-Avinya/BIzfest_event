using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Interfaces;
using BIZFEST_Event.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BIZFEST_Event.Controllers
{
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class AdminController : Controller
    {      
        private readonly IEventRepository _IEventRepository;
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db , IEventRepository repository)
        {          
            _IEventRepository = repository;
            _db = db;
        }
        public IActionResult Index()        
        {
            var response = _IEventRepository.GetAllEvents();
            return View(response);
        }

        public IActionResult Create()
        {
            UserEvent Event = new UserEvent();
            return PartialView("_EventCreatePartial", Event);
        }
        [HttpPost]
        public IActionResult EditUser(int Id)
        {
            UserEvent objEvent = new UserEvent();
            objEvent =_db.UserEvent.Where(x => x.Id == Id).FirstOrDefault();
            return PartialView("_EventCreatePartial", objEvent);
        }

        [HttpPost]
        public  IActionResult AddEvent(UserEvent Event)
        {
            _IEventRepository.CreateEvent(Event);  
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult EditEvent(UserEvent Event)
        {
            _IEventRepository.CreateEvent(Event);
            return RedirectToAction("Index");
        }

        [HttpDelete]       
        public async Task<IActionResult> DeleteCustomer(int Id)
        {
          await  _IEventRepository.SoftDeleteEvent(Id);
            return RedirectToAction("Index");
        }

        public IActionResult UserDetail()
        {
            int? EventId = Convert.ToInt32(HttpContext.Request.Query["EventId"]);
            var events = _db.UserEvent.FirstOrDefault(x => x.Id == EventId);
            string eventName = "";
            if (events != null)
            {
                eventName = events.EventName;
            }
   
            ViewBag.EventName = eventName;
            var response = _db.UserRegistration.Where(x => x.EventId == EventId).OrderByDescending(x => x.RegistereDate).Select(x => new UsersRegistrationView
            {
                Id = x.Id,
                Name = x.Name,
                ContactNo = x.ContactNo,
                EmailId = x.EmailId,
                BusinessName = x.BusinessName,
                City = x.City,
                State = x.State,
                BusinessCategory = x.BusinessCategory,
                EventId = x.EventId,
                RegistereDate = x.RegistereDate.HasValue ? Convert.ToDateTime(x.RegistereDate).ToString("dd/MM/yyyy HH:mm:ss") : "",
                IsBNIMember = x.IsBNIMember,
                IsInvitedByBNIMember = x.IsInvitedByBNIMember,
                ChapterName = x.ChapterName,
                InvitedByChapter = x.InvitedByChapter,
                InvitedMemberName = x.InvitedMemberName,
                Fees = x.Fees,
                PaymentStatus = x.PaymentStatus
            });
            return View(response);
        }


        public IActionResult PaymentHistory(int eventId, int userId)
        {
            var response = _db.ResponsePayment.Where(x => x.EvenId == eventId && x.UserId == userId).OrderByDescending(x => x.ResponseDate);
            return View(response);
        }
    }
}
