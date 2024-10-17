namespace BIZFEST_Event.Models
{
    public class RazorPayOptionModel
    {
        public string Key { get; set; }
        public decimal? AmountInSubUnites { get; set; }
        public string currency { get; set; }
        public string Name { get; set; }
        public string Dec { get; set; }
        public string ImageLogURL { get; set; }
        public string OrderId { get; set; }
        public string PofileName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string EventName { get; set; }
        public Dictionary<string,string> Note { get; set; }
    }
}
