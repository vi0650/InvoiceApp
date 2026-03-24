using System.ComponentModel.DataAnnotations;

namespace Invoice.DTOs.AuthDTO
{
    public class LogReqDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
