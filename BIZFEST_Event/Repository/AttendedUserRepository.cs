using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Helper;
using BIZFEST_Event.Interfaces;
using BIZFEST_Event.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BIZFEST_Event.Repository
{
    public class AttendedUserRepository : IAttendedUserRepository
    {
        private readonly ApplicationDbContext _db;

        public AttendedUserRepository(ApplicationDbContext db)
        {
            _db = db;

        }
        public async Task<int> EditUser(UserAttended User)
        {
            int result = 0;
            try
            {
                Helper.Helper _helper = new Helper.Helper();
                var Userdetail = await _db.UserRegistration.Where(endp => endp.EventId == User.EventId && endp.Id == User.UserId).FirstOrDefaultAsync();
                var events = _db.UserEvent.Where(x => x.Id == User.EventId).FirstOrDefault();
              
                if(Userdetail == null)
                {
                    return -1;
                }
                else if (Convert.ToBoolean(Userdetail.IsAttended) == false)
                {
                    Userdetail.IsAttended = true;
                    //Userdetail.IsAttended = true;
                    _db.UserRegistration.Update(Userdetail);
                    result = await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {               
            }
            return result;
        }
    }
}
