using BIZFEST_Event.DataAccess;
using BIZFEST_Event.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace BIZFEST_Event.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _db;

        public LoginRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> LoginUser(string UserName, string Password)
        {
            var user = await _db.AdminLogin.FirstOrDefaultAsync(x => x.UserName == UserName && x.Pasword == Password);            
            return user !=null;
        }
        
    }
}
