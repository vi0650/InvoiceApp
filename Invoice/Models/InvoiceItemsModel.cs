using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Invoice.Models
{
    [Index(nameof(invoiceid))]
    [Index(nameof(productid))]
    public class InvoiceItemsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid invoiceitemid { get; set; }

        [Required]
        public Guid invoiceid { get; set; }
        
        [JsonIgnore]
        [ForeignKey(nameof(invoiceid))]
        public InvoiceModel invoices { get; set; } = null!;

        [Required]
        public Guid productid { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(productid))]
        public ProductModel products { get; set; } = null!;

        //public int InvoiceItemNo { get; set; }
        public string? productname { get; set; }

        [Required]
        public Guid rateid { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(rateid))]
        public GstModel gstrate { get; set; } = null!;

        public decimal rate { get; set; }

        public int qty { get; set; }
        public decimal gst { get; set; }
        public decimal amount { get; set; }
        public DateTime createdat { get; set; } = DateTime.UtcNow;
        public DateTime updatedat { get; set; }
    }
}
