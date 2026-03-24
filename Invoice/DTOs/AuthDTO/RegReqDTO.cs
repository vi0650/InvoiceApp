using System.ComponentModel.DataAnnotations;

namespace Invoice.DTOs.AuthDTO
{
    public class RegReqDTO
    {
        [Required]
        public string? ShopName { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? MobileNo { get; set; }
        public string? Address { get; set; }
        [Required]
        public string? Role { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
