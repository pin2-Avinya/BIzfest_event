﻿using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Helper;
using BIZFEST_Event.Interfaces;
using BIZFEST_Event.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using QRCoder;
using Razorpay.Api;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;

namespace BIZFEST_Event.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserRepository _userRepository;
		private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
       
        private readonly IWebHostEnvironment _HostEnvironment;
		public UserController(IUserRepository userRepository, ApplicationDbContext db, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
		{
			_userRepository = userRepository;
			_db = db;
			_HostEnvironment = hostEnvironment;
            _configuration = configuration;
        }
		public IActionResult UserView()
		{			
   
			ViewBag.State = _db.States.ToList();
			ViewBag.Category = _db.CategoryMaster.ToList();
            ViewBag.Event = _db.UserEvent.Where(x => x.IsDeleted == false).ToList();
            return View();
		}

		public JsonResult GetCityList(int StateId)
		{
			List<Cities> StateList = _db.Cities.Where(x => x.StateId == StateId).ToList();
			return Json(StateList);

		}
		public IActionResult Registration()
		{
			return View();
		}
		public IActionResult UserDetail()
		{
			int? EventId = Convert.ToInt32(HttpContext.Request.Query["EventId"]);
			List<UsersRegistration> response = _db.UserRegistration.Where(x => x.EventId== EventId).ToList();
			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> AddUser(UsersRegistration userModel)
		{
			Helper.Helper _helper = new Helper.Helper();
			var user = _db.UserRegistration.Where(x => x.ContactNo == userModel.ContactNo && x.EventId == userModel.EventId).FirstOrDefault();
			var events = _db.UserEvent.Where(x => x.Id == userModel.EventId).FirstOrDefault();

			if (user != null)
			{
                userModel.Id= user.Id;
                if (user.PaymentStatus != "success")
				{
                    TempData["Message"]="Your number is already registered with us but payment is pending, please click on the pay now button to complete the payment and confirm the registration.";
                    var razorPayOptionModel = await ProcessPayment(events, user);
                    return View("Payment", razorPayOptionModel);
                }
				else 
				{
                    TempData["Message"]="Your number is already registered with us, we have sent sms again. pleae check!";
                    await _helper.SendSms(_configuration["baseUrl"].ToString(), user.ContactNo, events.StartDate?.ToString("dd-MMM-yyyy"), user, events.EventName, events.Location, events.City);
                }

                ViewBag.State = _db.States.ToList();
				ViewBag.Category = _db.CategoryMaster.ToList();
                ViewBag.Event = _db.UserEvent.Where(x=> x.IsDeleted == false).ToList();

				return View("UserView", userModel);
			}
			else
			{
				TempData["Message"] = "";
                var amount = events.Fees + (events.Fees * 18 / 100);

                userModel.IsActive = true;
                userModel.PaymentStatus = "initiated";
				userModel.Fees = amount;
                await _userRepository.CreateUser(userModel);


                #region Payment
                if (events.Fees > 0)
				{
                  
                    var razorPayOptionModel = await ProcessPayment(events, userModel);                    
					return View("Payment", razorPayOptionModel);
					
				}
				else
				{
					TempData["Message"]="Something went wrong, please try again!";
                    userModel.Id= user.Id;
                    //await _helper.SendSms(userModel.ContactNo, events.StartDate?.ToString("dd-MMM-yyyy"), user);

                    ViewBag.State = _db.States.ToList();
                    ViewBag.Category = _db.CategoryMaster.ToList();
                    ViewBag.Event = _db.UserEvent.Where(x => x.IsDeleted == false).ToList();

                    return View("UserView", userModel);
				}
				#endregion
			}
		}

		private async Task<RazorPayOptionModel> ProcessPayment(UserEvent events, UsersRegistration userModel)
		{
            var amount = events.Fees + (events.Fees * 18 / 100);
            var key = _configuration["key"];
            var secret = _configuration["secret"];
            #region Payment 
            string orderId = "";
            int pay_amount = (int)(amount * 100);
            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", pay_amount); // this amount should be same as transaction amount
            input.Add("currency", "INR");
            input.Add("receipt", userModel.Id.ToString());
            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(input);
            orderId = order["id"].ToString();

            // insert in DB payment table
            OrderModel orderModel = new OrderModel()
            {
                OrderAmount = pay_amount,
                Currency ="INR",
                Payment_Capture =1,
            };
            RazorPayOptionModel razorPayOptionModel = new RazorPayOptionModel()
            {
                Key = key,
                AmountInSubUnites = orderModel.OrderAmount,
                currency = orderModel.Currency,
                Name = userModel.Name,
                Dec = events.Description,
                ImageLogURL ="",
                OrderId = orderId,
                ContactNo = userModel.ContactNo,
                Email = userModel.EmailId,
                Address = userModel.City,
                EventName = events.EventName
            };

            ResponsePayment Payment = new ResponsePayment()
            {
                OrderId =orderId,
                //ResponsePaymentId
                EvenId  = events.Id,
                UserId  = userModel.Id,
                MobileNo = userModel.ContactNo,
                PaymentDate = DateTime.Now,
                Amount  = amount,
                //Status 
            };
            _db.ResponsePayment.Add(Payment);
            _db.SaveChanges();

			return razorPayOptionModel;
            //return View("Payment", razorPayOptionModel);

            #endregion
        }

        public async Task<IActionResult> Paymentresponse()
		{

            Helper.Helper _helper = new Helper.Helper();

            var PaymentId = Request.Form["PaymentId"].ToString();
			var OrderId = Request.Form["OrderId"].ToString();
			var SignatureId = Request.Form["SignatureId"].ToString();

			var validSignature = CompareSignatures(OrderId, PaymentId, SignatureId);
            var objPayment = _db.ResponsePayment.Where(x => x.OrderId ==OrderId).FirstOrDefault();

            if (validSignature)
			{				
                objPayment.Status ="Success";
				objPayment.ResponsePaymentId = PaymentId;
				objPayment.SignatureId = SignatureId;
				objPayment.ResponseDate = DateTime.Now;

                _db.ResponsePayment.Update(objPayment);
				_db.SaveChanges();

                var user = _db.UserRegistration.Where(x => x.Id == objPayment.UserId && x.EventId == objPayment.EvenId).FirstOrDefault();
				if (user !=null)
				{
                    user.PaymentStatus = "success";                    
                    _db.UserRegistration.Update(user);
                    _db.SaveChanges();

                    var events = _db.UserEvent.Where(x => x.Id == objPayment.EvenId).FirstOrDefault();
					await _helper.SendSms(_configuration["baseUrl"].ToString(), user.ContactNo, events.StartDate?.ToString("dd-MMM-yyyy"), user, events.EventName, events.Location, events.City);

                }
                return View("Success");
			}
			else
			{
				objPayment.Status ="Fail";
				objPayment.ResponsePaymentId = PaymentId;
				objPayment.SignatureId = SignatureId;
                objPayment.ResponseDate = DateTime.Now;
                _db.ResponsePayment.Update(objPayment);
				_db.SaveChanges();

                var user = _db.UserRegistration.Where(x => x.Id == objPayment.UserId && x.EventId == objPayment.EvenId).FirstOrDefault();
                if (user !=null)
                {
                    user.PaymentStatus = "failed";
                    _db.UserRegistration.Update(user);
                    _db.SaveChanges();
                }
                return View("Fail");
			}


		}

		private bool CompareSignatures(string orderId, string paymentId, string razorPaySignature)
		{
			var text = orderId + "|" + paymentId;
			var secret = _configuration["secret"];
            var generatedSignature = CalculateSHA256(text, secret);
			if (generatedSignature == razorPaySignature)
				return true;
			else
				return false;
		}

		private string CalculateSHA256(string text, string secret)
		{
			string result = "";
			var enc = Encoding.Default;
			byte[]
			baText2BeHashed = enc.GetBytes(text),
			baSalt = enc.GetBytes(secret);
			System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baSalt);
			byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
			result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
			return result;
		}


	}
}