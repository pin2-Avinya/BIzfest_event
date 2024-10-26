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
        Task<List<UserEvent>> GetRegisteredUsers();
        Task<List<UserRegistrationCustomForm>> GetRegisteredCustFields(int eventId);
        Task<List<CustomFields>> GetCustomFields();
        Task<int> DeleteCustomField(int id);
        Task<CustomFields> GetCustomFieldById(int id);
        Task<bool> UpdateCustomField(CustomFields model);
        Task<List<CustomFields>> GetCustomFields(int start, int length, string searchValue);
        Task<int> GetFilteredCustomFieldCount(string searchValue);
    }
}
