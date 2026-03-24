using System.ComponentModel.DataAnnotations;

namespace Invoice.DTOs.CreateDTOs
{
    public class CreateCustomerDTO
    {
        public string? CustomerName { get; set; }
        public Guid UserId { get; set; }
        public string? MobileNo { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
    }
}
