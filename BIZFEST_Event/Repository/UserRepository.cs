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
            User.IsStudent = "Yes";
            Helper.Helper _helper = new Helper.Helper();
            User.RegistereDate = DateTime.Now;
            //#region QR Code
            //string Path = string.Empty;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    QRCodeGenerator qrGenerator = new QRCodeGenerator();

            //    string userJson = JsonConvert.SerializeObject(User);

            //    //string QrCodeName = User.ContactNo+","+User.EmailId+","+User.BusinessName+","+User.City+","+User.State+","+User.BusinessCategory+","
            //    //                + User.BrCodeURL+","+User.RegistereDate+","+User.IsBNIMember+","+User.IsInvitedByBNIMember+","+User.InvitedByChapter;



            //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(userJson, QRCodeGenerator.ECCLevel.Q);
            //    QRCode qrCode = new QRCode(qrCodeData);
            //    using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
            //    {
            //        qrCodeImage.Save(ms, ImageFormat.Png);
            //        //Path = "Image;base64" + Convert.ToBase64String(ms.ToArray());
            //        Path = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray()));
            //    }
            //}
            //#endregion


                #region QR Code
                string Path = string.Empty;
            //string link = $"http://bizfest.itfuturz.com/User/UserView?EventId={0}";
            string QrCodeName = User.ContactNo + "," + User.EmailId + "," + User.BusinessName + "," + User.City + "," + User.State + "," + User.BusinessCategory + ","
                           + User.BrCodeURL+","+User.RegistereDate+","+User.IsBNIMember+","+User.IsStudent+","+User.IsInvitedByBNIMember+","+User.InvitedByChapter;
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(QrCodeName, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                {
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    Path = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray()));
                }
            }
            #endregion
            User.BrCodeURL = Path;
            _db.UserRegistration.Add(User);
            _db.SaveChanges();
                      
          
            return 0;
        }
        public async Task<int> SoftDeleteUser(int Id)
        {
            var objEvent = new UserEvent();
            objEvent = _db.UserEvent.Where(x => x.Id == Id).First();
            objEvent.IsDeleted = true;
            _db.UserEvent.Update(objEvent);
            _db.SaveChanges();
            return (0);
            //using (var connection = _DB.CreateConnection())
            //{
            //  var response=  await connection.ExecuteScalarAsync<int>(@"UPDATE UserEvent SET IsDelete =1 Where Id = @Id", new {Id = Id});
            //}
        }

        public List<UserEvent> Getevent(int id)
        {
            return _db.UserEvent.Where(x => x.IsDeleted == false && x.Id == id).ToList();
        }

        public List<EventCustomForm> GetEventCustomFields(int id)
        {
            return _db.EventCustomForm.Where(x => x.IsChecked == true && x.EventId == id).ToList();
        }
    }
}
