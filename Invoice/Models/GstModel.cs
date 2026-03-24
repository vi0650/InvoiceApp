using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Invoice.Models
{
    public class GstModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid rateid { get; set; }

        [Required]
        public Guid userid { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(userid))]
        public UserModel users { get; set; } = null!;

        public decimal gst { get; set; }
        public bool isactive { get; set; } = false;
        public DateTime createdat { get; set; } = DateTime.UtcNow;
        public DateTime updatedat { get; set; }
    }
}
