namespace Invoice.DTOs.ResponseDTOs
{
    public class UserResDTO
    {
        public Guid UserId { get; set; }
        public string? ShopName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
