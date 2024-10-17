using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Helper;
using BIZFEST_Event.Interfaces;
using BIZFEST_Event.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using ZXing.QrCode.Internal;
using static QRCoder.PayloadGenerator;
using QRCode = QRCoder.QRCode;


namespace BIZFEST_Event.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _HostEnvironment;
     
        public UserRepository(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db=db;
            _HostEnvironment=hostEnvironment;
        }
        public IEnumerable<UsersRegistration> GetAllUser()
        {
            var response = _db.UserRegistration.ToList();
            return response;
        }
        public async Task<int> CreateUser(UsersRegistration User)
        {
            Helper.Helper _helper = new Helper.Helper();
            User.RegistereDate = DateTime.Now;
            #region QR Code
            string Path = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();

                string userJson = JsonConvert.SerializeObject(User);

                //string QrCodeName = User.ContactNo+","+User.EmailId+","+User.BusinessName+","+User.City+","+User.State+","+User.BusinessCategory+","
                //                + User.BrCodeURL+","+User.RegistereDate+","+User.IsBNIMember+","+User.IsInvitedByBNIMember+","+User.InvitedByChapter;
                               
                   

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(userJson, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                {
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    //Path = "Image;base64" + Convert.ToBase64String(ms.ToArray());
                    Path = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray()));
                }
            }
            #endregion

            User.BrCodeURL = Path;
            _db.UserRegistration.Add(User);
            _db.SaveChanges();
                      
          
            return 0;
        }


    }
}
