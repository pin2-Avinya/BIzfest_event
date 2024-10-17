using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Models;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;
using System.Net.Sockets;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BIZFEST_Event.Helper
{
    public class Helper
    {
        private readonly ApplicationDbContext _db;
        public async Task<int> SendSms(string baseUrl, string ContactNo, string? StartDate, UsersRegistration User, string eventName, string eventAddress, string city)
        {
            #region Send SMS

            string link = $"{baseUrl}Ticket?EventId%3D{User.EventId}%26UserId%3D{User.Id}";
            //https://api.arihantsms.com/api/v2/SendSMS?SenderId=BIZFST&Is_Unicode=false&Is_Flash=false&Message=Thank%20you%20suraj%20for%20registering%20to%20attend%20DOCBIZCON%20on%2006-Aug-2023%20at%20Avadh%20Utopiya%2C%20Surat.%20Please%20click%20on%20the%20following%20link%20for%20your%20entry%20ticket%20%3A%20http%3A%2F%2Fbizfest.itfuturz.com%2FTicket%3FEventId%3D1022%26UserId%3D1024%20BUSINESS%20SUPPORT%20SERVICES&MobileNumbers=918320535895&ApiKey=DRtCYtkDh%2FapkjACSBPSiO1%2FVSO5jLG1Jhvvk46hHyo%3D&ClientId=2471f9a7-383f-46e8-ae29-69f2b009a76a

            string message = $"Thank%20you%20{User.Name}%20for%20registering%20to%20attend%20BNI%20DOCBIZCON%20on%2006%20Aug%202023%20at%20Avadh%20Utopia%2CSurat.%20Please%20click%20on%20the%20following%20link%20for%20your%20entry%20ticket%20%3A%20{link}%20BUSINESS%20SUPPORT%20SERVICES";            
            string url = $"api/v2/SendSMS?SenderId=BIZFST&Is_Unicode=false&Is_Flash=false&Message={message}&MobileNumbers=91{User.ContactNo}&ApiKey=DRtCYtkDh/apkjACSBPSiO1/VSO5jLG1Jhvvk46hHyo=&ClientId=2471f9a7-383f-46e8-ae29-69f2b009a76a";
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.arihantsms.com/");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress+url);
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

    //https://api.arihantsms.com/api/v2/SendSMS?SenderId=BIZFST&Is_Unicode=false&Is_Flash=false&Message=Thank you {Suraj} for registering to attend DOCBIZCON on {date} at Amore, Surat. Please click on the following link for your entry ticket: http://bizfest.itfuturz.com/Ticket?EventId=
    //      //{eventId}&UserId={userId} BUSINESS SUPPORT SERVICES&MobileNumbers=919586011195&ApiKey=DRtCYtkDh/apkjACSBPSiO1/VSO5jLG1Jhvvk46hHyo=&ClientId=2471f9a7-383f-46e8-ae29-69f2b009a76a
}
