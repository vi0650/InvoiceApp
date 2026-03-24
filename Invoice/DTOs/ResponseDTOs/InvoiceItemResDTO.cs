using Invoice.DTOs.ResponseDTOs;
using Invoice.Models;

namespace Invoice.DTOs.BaseDTOs
{
    public class InvoiceItemResDTO
    {
        public Guid InvoiceItemId { get; set; }
        //public Guid InvoiceId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        //public ProductResDTO Products { get; set; } = null!;
        //public int InvoiceItemNo { get; set; }

        public decimal Rate { get; set; }
        public int Qty { get; set; }

        public Guid RateId { get; set; }
        public decimal Gst { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
