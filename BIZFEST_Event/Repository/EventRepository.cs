using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Interfaces;
using BIZFEST_Event.Models;
using Microsoft.Extensions.Logging;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BIZFEST_Event.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _db;
        // private readonly IDbRepository _DB;

        public EventRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public Task<int> CreateEvent(UserEvent Event)
        {
               
            if (Event.Id > 0)
            {
                _db.UserEvent.Update(Event);
                _db.SaveChanges();
            }
            else
            {
                _db.UserEvent.Add(Event);
                _db.SaveChanges();

                #region QR Code
                string Path = string.Empty;
                string link = $"http://bizfest.itfuturz.com/User/UserView?EventId={Event.Id}";
                using (MemoryStream ms = new MemoryStream())
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                    {
                        qrCodeImage.Save(ms, ImageFormat.Png);
                        Path = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray()));
                    }
                }
                #endregion

                //Update QR code
                Event.BrCodeURL = Path;
                _db.UserEvent.Update(Event);
                _db.SaveChanges();
            }

            return Task.FromResult(0);
        }

        public Task<int> updateEvent(UserEvent Event)
        {
            var eventdata = _db.UserEvent.Find(Event.Id);
            if (eventdata != null)
            {
                eventdata.EventName = Event.EventName;
                eventdata.Description = Event.Description;
                eventdata.StartDate = Event.StartDate;
                eventdata.EndDate = Event.EndDate;
                eventdata.Location = Event.Location;
                eventdata.City = Event.City;
                eventdata.OrganizerName = Event.OrganizerName;
                eventdata.Fees = Event.Fees;
                eventdata.FeesDescription = Event.FeesDescription;

                _db.UserEvent.Update(eventdata);
            }
            _db.SaveChanges();
            return Task.FromResult(0);
        }

        public Task<int> AddCustom(EventCustomForm model)
        {
            if (model != null)
            {
                _db.EventCustomForm.Add(model);
                _db.SaveChanges();
            }
            return Task.FromResult(0);
        }
        public IEnumerable<UserEvent> GetAllEvents()
        {
            List<UserEvent> response = _db.UserEvent.Where(x => x.IsDeleted == false).ToList();
            return response;
        }

        public List<UserEvent> GetEventById(int id) 
        {
            var Event = _db.UserEvent.Where(x => x.Id == id).ToList();
            return Event;
        }

        public async Task<int> DeleteUser(int Id)
        {
           var  objuser = _db.UserRegistration.Where(x => x.Id == Id).First();
            if (objuser != null) {
                _db.UserRegistration.Remove(objuser);
                _db.SaveChanges();
            }
          
            return (0);
            //using (var connection = _DB.CreateConnection())
            //{
            //  var response=  await connection.ExecuteScalarAsync<int>(@"UPDATE UserEvent SET IsDelete =1 Where Id = @Id", new {Id = Id});
            //}
        }

        public async Task<int> DeleteEvent(int id)
        {
            var Event = _db.UserEvent.Find(id);
            
            if (Event != null)
            {
                Event.IsDeleted = true;
                _db.UserEvent.Update(Event);
            };
            await _db.SaveChangesAsync();
            return 0;
        }

        public async Task<List<UserEvent>> GetRegisteredUsers()
        {
            var userEvents = await _db.UserEvent
                .GroupBy(x => x.EventName)
                .Select(g => g.FirstOrDefault())
                .ToListAsync();

            var registeredUsers = new List<UserEvent>(userEvents);

            return registeredUsers;
        }


        public async Task<List<UserRegistrationCustomForm>> GetRegisteredCustFields(int eventId)
        {
            var custFields = await _db.UserRegistrationCustomForm
                                       .Where(x => x.EventId == eventId)
                                       .ToListAsync();
            return custFields;
        }

        public async Task<List<CustomFields>> GetCustomFields()
        {
            var fields = await _db.CustomFields.ToListAsync();
            return fields;
        }

        public async Task<int> DeleteCustomField(int id)
        {
            var field = await _db.CustomFields.FindAsync(id);
            if (field != null)
            {
                _db.CustomFields.Remove(field);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<CustomFields> GetCustomFieldById(int id)
        {
            var field = await _db.CustomFields.FindAsync(id);
            return field;
        }

        public async Task<bool> UpdateCustomField(CustomFields model)
        {
            var existingField = await _db.CustomFields.FindAsync(model.Id);
            if (existingField != null)
            {
                existingField.LabelName = model.LabelName;
                existingField.TypeName = model.TypeName;
                existingField.Options = model.Options;
                existingField.IsMandatory = model.IsMandatory;

                _db.CustomFields.Update(existingField);
            }
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<CustomFields>> GetCustomFields(int start, int length, string searchValue)
        {
            IQueryable<CustomFields> query = _db.CustomFields;

            // Apply search filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(cf => cf.LabelName.Contains(searchValue) || cf.TypeName.Contains(searchValue));
            }

            // Apply pagination
            return await query.Skip(start).Take(length).ToListAsync();
        }

        public async Task<int> GetFilteredCustomFieldCount(string searchValue)
        {
            IQueryable<CustomFields> query = _db.CustomFields;

            // Apply search filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(cf => cf.LabelName.Contains(searchValue) || cf.TypeName.Contains(searchValue));
            }

            return await query.CountAsync();
        }
    }
}

