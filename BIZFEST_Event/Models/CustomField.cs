using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BIZFEST_Event.Models
{
    public class CustomFields
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string? LabelName { get; set; }
        public string? TypeName { get; set; }
        public bool IsMandatory { get; set; }
        public string? Options { get; set; }
        public int UserID { get; set; }
        public string? FieldType { get; set; }
        public bool IsHidden { get; set; }
    }
}
