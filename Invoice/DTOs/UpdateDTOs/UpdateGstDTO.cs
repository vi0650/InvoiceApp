using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice.DTOs.UpdateDTOs
{
    public class UpdateGstDTO
    {
        public Guid RateId { get; set; }
        public Guid UserId { get; set; }
        public decimal Gst { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
