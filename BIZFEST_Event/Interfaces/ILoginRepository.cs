using BIZFEST_Event.Models;

namespace BIZFEST_Event.Interfaces
{
    public interface ILoginRepository
    {
        Task<bool> LoginUser(string UserName , string Password);
    }
}
