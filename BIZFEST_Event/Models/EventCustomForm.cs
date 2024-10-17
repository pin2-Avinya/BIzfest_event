using System.ComponentModel.DataAnnotations;

namespace BIZFEST_Event.Models
{
    public class EventCustomForm
    {
        [Key]
        public int EventId { get; set; }
        public string? LabelName { get; set; }
        public string? Type { get; set; }
        public string? value { get; set; }
        public bool IsMandatory { get; set; }

    }
}
