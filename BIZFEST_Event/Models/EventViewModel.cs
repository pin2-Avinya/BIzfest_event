namespace BIZFEST_Event.Models
{
    public class EventViewModel
    {
        public UserEvent userEvent { get; set; }
        public List<EventCustomForm> CustomForm { get; set; } // Change from single object to List
    }
}
