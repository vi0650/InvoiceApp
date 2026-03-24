namespace Invoice.DTOs.CreateDTOs
{
    public class UpdateProductDTO
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public Guid UserId { get; set; }
        public decimal? Rate { get; set; }
        public bool? IsActive { get; set; }
    }
}
