using System.ComponentModel.DataAnnotations;

namespace Invoice.DTOs.CreateDTOs
{
    public class UpdateInvoiceDTO
    {
        public Guid InvoiceId { get; set; }
        public Guid UserId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string InvoiceStatus { get; set; }
        public DateTime InvoiceDate { get; set; }
        public List<UpdateInvoiceItemDTO> InvoiceItems { get; set; } = new();
        public string? Description { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GstAmount { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
