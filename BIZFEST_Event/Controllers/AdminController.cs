using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Helper;
using BIZFEST_Event.Interfaces;
using BIZFEST_Event.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;


namespace BIZFEST_Event.Controllers
{
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class AdminController : Controller
    {
        private readonly IEventRepository _IEventRepository;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        public AdminController(ApplicationDbContext db, IEventRepository repository, IConfiguration configuration)
        {
            _IEventRepository = repository;
            _db = db;
            _configuration = configuration;
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

        public async Task<IActionResult> CreateEvent()
        {
            string? userName = HttpContext.Session.GetString("UserName");
            int? userid = await GetUserId(userName);
            var custFields = _db.CustomFields.Where(u => u.UserID == userid).ToList();
            ViewBag.Custom = custFields;
            return View();
        }

        public async Task<IActionResult> DynamicCreate()
        {
            string? userName = HttpContext.Session.GetString("UserName");
            int? userid = await GetUserId(userName);
            var fields = _db.DynamicFields.ToList();
            var custFields = _db.CustomFields.Where(u => u.UserID == userid).ToList();
            ViewBag.Custom = custFields;
            return PartialView("_DynamicPartial", fields);
        }

        public async Task<IActionResult> ViewCustomFields()
        {
            string? userName = HttpContext.Session.GetString("UserName");
            int? userid = await GetUserId(userName);

            if (userid.HasValue)
            {
                var fields = _db.CustomFields
                                .Where(f => f.UserID == userid.Value) 
                                .ToList();

                return View(fields);
            }
            else
            {
                return View(new List<CustomFields>()); 
            }
        }


        public IActionResult CustomFields()
        {
            var fields = _db.CustomFields.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomField(CustomFields model)
        {
            string? userName = HttpContext.Session.GetString("UserName");
            int userid = await GetUserId(userName);
            bool isMandatory = model.IsMandatory;

            string generatedHtml;
            string? options = null;

            if (model.TypeName == "dropdown")
            {
                var optionList = model.Options?.Split(',').Select(o => o.Trim()).ToList();

                generatedHtml = $"<label>{model.LabelName}</label><select name='{model.LabelName}' {(isMandatory ? "required" : "")}>";

                if (optionList != null && optionList.Any())
                {
                    foreach (var option in optionList)
                    {
                        generatedHtml += $"<option value='{option}'>{option}</option>";
                    }

                    options = string.Join(",", optionList);
                }

                generatedHtml += "</select>";
            }
            else
            {
                generatedHtml = $"<div class='form-group'>" +
                                $"<label class='form-label'>{model.LabelName}</label>" +
                                $"<input type='{model.TypeName}' class='form-control' name='{model.LabelName}' {(isMandatory ? "required" : "")} />" +
                                "</div>";
            }

            var formField = new CustomFields
            {
                LabelName = model.LabelName,
                TypeName = model.TypeName,
                IsMandatory = isMandatory,
                Options = options ,
                UserID = userid,
                FieldType = "Custom"
            };

            _db.CustomFields.Add(formField);
            await _db.SaveChangesAsync();

            return RedirectToAction("CustomFields");
        }

        public async Task<int> GetUserId(string? username)
        {
            var user = await _db.AdminLogin.Where(u => u.UserName == username).Select(u => u.AdminLoginId).FirstOrDefaultAsync();

            return user;
        }

        [HttpPost]
        public IActionResult EditUser(int Id)
        {
            UserEvent objEvent = new UserEvent();
            objEvent =_db.UserEvent.Where(x => x.Id == Id).FirstOrDefault();
            return PartialView("_EventCreatePartial", objEvent);
        }

        [HttpPost]
        public IActionResult AddEvent(EventViewModel formData, IFormFile logo,
                                          IFormFile sponsorFile,
                                          IFormFile banner,
                                          IFormFile footerimg)
        {
            UserEvent userevent = formData.userEvent;
            if (logo != null && logo.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                     logo.CopyToAsync(memoryStream);
                    userevent.Logo = memoryStream.ToArray();
                }
            }

            if (sponsorFile != null && sponsorFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                     sponsorFile.CopyToAsync(memoryStream);
                    userevent.SponsoredBy = memoryStream.ToArray();
                }
            }

            if (banner != null && banner.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                     banner.CopyToAsync(memoryStream);
                    userevent.Banner = memoryStream.ToArray();
                }
            }

            if (footerimg != null && footerimg.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    footerimg.CopyToAsync(memoryStream);
                    userevent.Footer = memoryStream.ToArray();
                }
            }
            _IEventRepository.CreateEvent(userevent);
            int eventId = userevent.Id;

            if (formData.CustomForm != null && formData.CustomForm.Any())
            {
                foreach (var customField in formData.CustomForm)
                {
                    // Only process if IsChecked is true
                    if (customField.IsChecked)
                    {
                        if (!string.IsNullOrEmpty(customField.LabelName))
                        {
                            var customForm = new EventCustomForm
                            {
                                EventId = eventId,
                                LabelName = customField.LabelName,
                                Type = customField.Type,
                                value = customField.value,
                                IsMandatory = customField.IsMandatory,
                                IsChecked = customField.IsChecked
                            };
                            _IEventRepository.AddCustom(customForm);
                        }
                    }
                }
            }
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
            await _IEventRepository.DeleteUser(Id);
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
               // City = x.City,
                //State = x.State,
                BusinessCategory = x.BusinessCategory,
                RegistrationNumber = x.RegistrationNumber,
                WhereDoYouKnow= x.WhereDoYouKnow,
                EventId = x.EventId,
                RegistereDate = x.RegistereDate.HasValue ? Convert.ToDateTime(x.RegistereDate).ToString("dd/MM/yyyy HH:mm:ss") : "",
                IsBNIMember = x.IsBNIMember,
                IsStudent = x.IsStudent,
                //IsInvitedByBNIMember = x.IsInvitedByBNIMember,
                ChapterName = x.ChapterName,
                //InvitedByChapter = x.InvitedByChapter,
                InvitedMemberName = x.InvitedMemberName,
                Fees = x.Fees,
                PaymentStatus = x.PaymentStatus,
                PaymentScreenShot=x.PaymentScreenShot,
                PaymentMode = x.PaymentMode == "No" ? "Paytm" : "RazorPay"
            });
            return View(response);
        }



        public IActionResult PaymentHistory(int eventId, int userId)
        {
            var response = _db.ResponsePayment.Where(x => x.EvenId == eventId && x.UserId == userId).OrderByDescending(x => x.ResponseDate);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> UserSentMSg(int Id, int EventId)
        {
            Helper.Helper _helper = new Helper.Helper();
            var user = _db.UserRegistration.Where(x => x.Id == Id && x.EventId == EventId).FirstOrDefault();
            var events = _db.UserEvent.Where(x => x.Id == EventId).FirstOrDefault();
           
            await _helper.SendSms(_configuration["baseUrl"].ToString(), user.ContactNo, events.StartDate?.ToString("dd-MMM-yyyy"), user, events.EventName, events.Location, events.City);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePaymentStatus(int Id, int EventId, string paymentStatus)
        {
            Helper.Helper _helper = new Helper.Helper();
            var user = _db.UserRegistration.Where(x => x.Id == Id && x.EventId == EventId).FirstOrDefault();
            if(user != null)
            {
                user.PaymentStatus = paymentStatus;                
                _db.UserRegistration.Update(user);
                _db.SaveChanges();
                return Ok();
            }
            return BadRequest("User does not exists!");
        }

        public IActionResult CustomFieldList()
        {
            var customFieldsList = _db.CustomFields.ToList();
            return View(customFieldsList);
        }
    }
}
