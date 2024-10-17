using BIZFEST_Event.Models;

namespace BIZFEST_Event.Interfaces
{
    public interface IAttendedUserRepository
    {
        Task<int> EditUser(UserAttended User);
    }
}
