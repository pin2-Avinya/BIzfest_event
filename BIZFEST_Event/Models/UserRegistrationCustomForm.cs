using System.ComponentModel.DataAnnotations;

namespace BIZFEST_Event.Models
{
    public class UserRegistrationCustomForm
    {
        [Key]
        public int Id { get; set; }
        public string? LabelName { get; set; }
        public string? Type { get; set; }
        public string? Value { get; set; }
        public int EventId { get; set; }
    }
}
