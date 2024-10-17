namespace BIZFEST_Event.Models
{
    public class UserEvent
    {
        public int Id { get; set; }
        public string? EventName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Location { get; set; }
        public string? OrganizerName { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public bool? IsDeleted { get; set; } = false;
        public string? BrCodeURL { get; set; }
        public string? City { get; set; }
        public decimal? Fees { get; set; }
        public string? FeesDescription { get; set; }
        public string? IsHiddenField { get; set; }  
        public int EventId { get; set; }
        
    }
   
}
