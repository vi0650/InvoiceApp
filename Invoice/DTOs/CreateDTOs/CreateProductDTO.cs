namespace Invoice.DTOs.CreateDTOs
{
    public class CreateProductDTO
    {
        public string? ProductName { get; set; }
        public Guid UserId { get; set; }
        public decimal? Rate { get; set; }
        public bool? IsActive { get; set; }
    }
}
