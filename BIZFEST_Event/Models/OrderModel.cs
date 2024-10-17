namespace BIZFEST_Event.Models
{
    public class OrderModel
    {
        public decimal? OrderAmount { get; set; }
        public string? Currency { get; set; }
        public int? Payment_Capture { get; set; }
        public string[] Notes { get; set; } 
    }
}
