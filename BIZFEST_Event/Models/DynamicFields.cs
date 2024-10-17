namespace BIZFEST_Event.Models
{
    public class DynamicFields
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? Options { get; set; }
        public bool IsMandatory { get; set; }
    }

    public class DynamicFormModel
    {
        public List<DynamicFields> Fields { get; set; }
    }
}
