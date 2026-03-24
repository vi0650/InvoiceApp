using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice.DTOs.CreateDTOs
{
    public class CreateGstDTO
    {
        public decimal Gst { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
