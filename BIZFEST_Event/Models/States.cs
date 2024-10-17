using System.ComponentModel.DataAnnotations;

namespace BIZFEST_Event.Models
{
    public class States
    {
        [Key]
        public int StateId { get; set; }
        public string? StateName { get; set; }
    }
}
