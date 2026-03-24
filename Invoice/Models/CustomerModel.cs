using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Invoice.Models
{
    public class CustomerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid customerid { get; set; }

        [Required]
        public Guid userid { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(userid))]
        public UserModel users { get; set; } = null!;

        public string customername { get; set; }
        public string mobileno { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public bool isactive { get; set; } = true;
        public DateTime createdat { get; set; } = DateTime.UtcNow;
        public DateTime updatedat { get; set; }

        public ICollection<InvoiceModel> invoices { get; set; } = new List<InvoiceModel>();
    }
}
