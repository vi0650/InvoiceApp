using Invoice.Models;
using System.ComponentModel.DataAnnotations;

namespace Invoice.DTOs.BaseDTOs
{
    public class InvoiceDTO
    {
        public Guid InvoiceId { get; set; }
        public int InvoiceNo { get; set; }
        public string? Description { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GstAmount { get; set; }
        public decimal GrandTotal { get; set; }

        //public ICollection<InvoiceItemsModel> InvoiceItems { get; set; } = new List<InvoiceItemsModel>();
    }
}
