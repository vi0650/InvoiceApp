using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Invoice.Models
{
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid userid { get; set; }
        public string? username { get; set; }
        public string? shopname { get; set; }
        public string? password { get; set; }
        public string? email { get; set; }
        public string? mobileno { get; set; }
        public string? address { get; set; }
        public string? role { get; set; }
        public bool isactive { get; set; } = true;
        public DateTime createdat { get; set; } = DateTime.UtcNow;
        public DateTime updatedat { get; set; }

        [JsonIgnore]
        public ICollection<CustomerModel> customers { get; set; } = new List<CustomerModel>();
        [JsonIgnore]
        public ICollection<ProductModel> products { get; set; } = new List<ProductModel>();
        [JsonIgnore]
        public ICollection<InvoiceModel> invoices { get; set; } = new List<InvoiceModel>();
    }
}
