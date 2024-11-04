using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIZFEST_Event.Models
{
    public class UsersRegistration
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContactNo { get; set; }
        public string? EmailId { get; set; }
        public string? BusinessName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? BusinessCategory { get; set; }
        public string? BrCodeURL { get; set; }
        public int EventId { get; set; }
        public DateTime? RegistereDate { get; set; }
        public string? IsBNIMember { get; set; }
        public string? IsInvitedByBNIMember { get; set; }
        public string? IsStudent { get; set; }
        public string? CollegeName { get; set; }
        public Guid UserId { get; set; }


        [NotMapped]
        public IFormFile? ScreenShot {get;set;}
        public string? PaymentMode { get; set; }
        public string? PaymentScreenShot {get;set;}
        public string? ChapterName { get; set; }
        public string? InvitedByChapter { get; set; }
        public string? InvitedMemberName { get; set; }
        public decimal? Fees { get; set; }
        public bool? IsAttended { get; set; }        
        public DateTime? EntryDate { get; set; }
        public string? PaymentStatus { get; set; } = "initiated";
        public bool? IsActive { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? WhereDoYouKnow { get; set; }
        
    }

    public class UsersRegistrationView
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContactNo { get; set; }
        public string? EmailId { get; set; }
        public string? BusinessName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? BusinessCategory { get; set; }  
        public string? RegistrationNumber { get; set; }
        public string? WhereDoYouKnow { get; set; }
        public int? EventId { get; set; }
        public string RegistereDate { get; set; }
        public string? IsStudent { get; set; }
        public string? CollegeName { get; set; }
        public string? IsBNIMember { get; set; }
        public string? IsInvitedByBNIMember { get; set; }
        public string? ChapterName { get; set; }
        public string? InvitedByChapter { get; set; }
        public string? InvitedMemberName { get; set; }
        public decimal? Fees { get; set; }
        public string? PaymentStatus { get; set; }
        public string? PaymentScreenShot { get; set; }
        public string? PaymentMode { get; set; }

    }
}
