using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Invoice.Models
{
    [Index(nameof(userid))]
    [Index(nameof(customerid))]
    public class InvoiceModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid invoiceid { get; set; }

        [Required]
        public Guid userid { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(userid))]
        public UserModel users { get; set; } = null!;

        [Required]
        public Guid customerid { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(customerid))]
        public CustomerModel customer { get; set; } = null!;

        public string customername { get; set; }
        public string email { get; set; }
        public string mobileno { get; set; }
        public string address { get; set; }
        public DateTime invoicedate { get; set; }

        public ICollection<InvoiceItemsModel> invoiceitems { get; set; } = new List<InvoiceItemsModel>();

        public string? description { get; set; }
        public string? invoicestatus { get; set; }
        public decimal subtotal { get; set; }
        public decimal gstamount { get; set; }
        public decimal grandtotal { get; set; }
        public DateTime createdat { get; set; } = DateTime.UtcNow;
        public DateTime updatedat { get; set; }
    }
}