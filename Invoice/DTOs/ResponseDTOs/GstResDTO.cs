using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice.DTOs.ResponseDTOs
{
    public class GstResDTO
    {
        public Guid RateId { get; set; }
        public Guid UserId { get; set; }
        public decimal Gst { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}
