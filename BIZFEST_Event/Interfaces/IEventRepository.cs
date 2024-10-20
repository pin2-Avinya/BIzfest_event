﻿using BIZFEST_Event.Models;

namespace BIZFEST_Event.Interfaces
{
    public interface IEventRepository
    {
        Task<int> CreateEvent(UserEvent Event);
        IEnumerable<UserEvent> GetAllEvents();

        Task<int> DeleteUser(int id);
        Task<int> AddCustom(EventCustomForm model);
    }
}
