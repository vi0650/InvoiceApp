using System.ComponentModel.DataAnnotations;

namespace Invoice.DTOs.CreateDTOs
{
    public class UpdateCustomerDTO
    {
        public Guid CustomerId { get; set; }
        public Guid UserId { get; set; }
        public string? CustomerName { get; set; }
        public string? MobileNo { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
    }
}
