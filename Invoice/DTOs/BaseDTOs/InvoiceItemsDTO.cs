using System.ComponentModel.DataAnnotations;

namespace Invoice.DTOs.BaseDTOs
{
    public class InvoiceItemsDTO
    {
        public Guid InvoiceItemId { get; set; }
        public int InvoiceItemNo { get; set; }
        public string? ProductName { get; set; }
        public decimal? Rate { get; set; }
        public int? Qty { get; set; }
        public int? Gst { get; set; }
        public decimal Amount { get; set; }
    }
}