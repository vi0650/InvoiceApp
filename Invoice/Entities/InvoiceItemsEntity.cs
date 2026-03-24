using Invoice.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice.Models
{
    public class InvoiceItemsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InvoiceItemId { get; set; }

        public int InvoiceId { get; set; }
        public InvoiceEntity Invoice { get; set; } = null!;

        public int ProductId { get; set; }
        public ProductEntity Product { get; set; } = null!;

        public string? ProductName { get; set; }
        public decimal? Rate { get; set; }
        public int? Qty { get; set; }
        public int? Gst { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
