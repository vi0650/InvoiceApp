namespace Invoice.DTOs.BaseDTOs
{
    public class CustomerResDTO
    {
        public Guid CustomerId { get; set; }
        public Guid UserId { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
