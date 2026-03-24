namespace Invoice.DTOs.BaseDTOs
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? Rate { get; set; }
        public bool? IsActive { get; set; }
    }
}
