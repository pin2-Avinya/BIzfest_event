using BIZFEST_Event.Models;

namespace BIZFEST_Event.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CreateUser(UsersRegistration Event);
        IEnumerable<UsersRegistration> GetAllUser();
        //Task<int> SoftDeleteUser(int id);
    }
}
