namespace BIZFEST_Event.Models
{
    public class ResponsePayment
    {
        public int Id { get; set; }
        public string? OrderId { get; set; }
        public string? ResponsePaymentId { get; set; }
        public int? EvenId { get; set; }
        public int? UserId { get; set; }
        public string? MobileNo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public string? Status { get; set; }
        public string? SignatureId { get; set; }
        public DateTime? ResponseDate { get; set; }
    }
}
