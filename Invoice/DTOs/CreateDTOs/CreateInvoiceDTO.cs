using Invoice.DTOs.BaseDTOs;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace Invoice.DTOs.CreateDTOs
{
    public class CreateInvoiceDTO
    {
        public Guid UserId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public DateTime InvoiceDate { get; set; }
        public List<CreateInvoiceItemDTO> InvoiceItems { get; set; } = new();
        public string? InvoiceStatus {  get; set; }
        public string? Description { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GstAmount { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
