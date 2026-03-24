namespace Invoice.DTOs.ResponseDTOs
{
    public class ProductResDTO
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public string? ProductName { get; set; }
        public decimal? Rate { get; set; }
        public bool? IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}
