using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Models;
using System.Net.Http.Headers;

namespace BIZFEST_Event.Helper
{
    public class Helper
    {
        private readonly ApplicationDbContext _db;
        public async Task<int> SendSms(string baseUrl, string ContactNo, string? StartDate, UsersRegistration User, string eventName, string eventAddress, string city)
        {
            #region Send SMS

            string link = $"{baseUrl}Ticket?EventId={User.EventId}%26UserId={User.Id}";
            string message = $"Thank you {User.Name} for registering to attend SURAT BIZ FEST on {StartDate} at {eventAddress}. Please click on the following link for your entry ticket : {link} BUSINESS SUPPORT SERVICES";
            string url = $"api/push.json?apikey=5bdc20250a594&sender=BIZFST&mobileno={User.ContactNo}&text={message}";


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://sms.mobileadz.in/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }
            return 1;
            #endregion

        }
    }
}
