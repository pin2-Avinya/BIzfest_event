using BIZFEST_Event.Models;
using System.Threading.Tasks;

namespace BIZFEST_Event.Interfaces
{
    public interface IEventRepository
    {
        Task<int> CreateEvent(UserEvent Event);
        IEnumerable<UserEvent> GetAllEvents();

        Task<int> DeleteUser(int id);
        Task<int> AddCustom(EventCustomForm model);
        Task<int> DeleteEvent(int id);
        List<UserEvent> GetEventById(int id);
        Task<int> updateEvent(UserEvent Event);
    }
}
