using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;

namespace BIZFEST_Event.Controllers
{
    public class PaymentController : Controller
    {
        string key = "<Enter your Api Key here>";
        string secret = "<Enter your Api Secret here>";

        public IActionResult Index()
        {
            string orderId = "";
            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", 100); // this amount should be same as transaction amount
            input.Add("currency", "INR");
            input.Add("receipt", "12121");

     
            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(input);
            orderId = order["id"].ToString();





            return View();
        }
    }
}
