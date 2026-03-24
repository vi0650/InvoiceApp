using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Invoice.Models
{
    public class ProductModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid productid { get; set; }

        [Required]
        public Guid userid { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(userid))]
        public UserModel users { get; set; } = null!;

        public string? productname { get; set; }
        public decimal? rate { get; set; }
        public bool? isactive { get; set; } = true;
        public DateTime createdat { get; set; } = DateTime.UtcNow;
        public DateTime updatedat { get; set; }

        public ICollection<InvoiceItemsModel> invoiceitems { get; set; } = new List<InvoiceItemsModel>();
    }
}
