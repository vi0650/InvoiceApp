using System.ComponentModel.DataAnnotations;

namespace Invoice.DTOs.CreateDTOs
{
    public class CreateInvoiceItemDTO
    {
        //public int InvoiceItemNo { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }

        public decimal Rate { get; set; }
        public int Qty { get; set; }

        public Guid RateId { get; set; }
        public decimal Gst { get; set; }
        public decimal Amount { get; set; }
    }
}
