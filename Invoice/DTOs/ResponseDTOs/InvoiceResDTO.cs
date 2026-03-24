namespace Invoice.DTOs.BaseDTOs
{
    public class InvoiceResDTO
    {
        public Guid InvoiceId { get; set; }
        public Guid UserId { get; set; }
        //public Guid CustomerId { get; set; }
        public CustomerResDTO Customer { get; set; }
        public DateTime InvoiceDate { get; set; }
        public List<InvoiceItemResDTO> InvoiceItems { get; set; } = new List<InvoiceItemResDTO>();
        public string Description { get; set; }
        public string InvoiceStatus { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GstAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }

}
