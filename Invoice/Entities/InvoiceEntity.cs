using Invoice.Models;

namespace Invoice.Entities
{
    public class InvoiceEntity
    {
        public int InvoiceId { get; set; }

        public string InvoiceNumber { get; set; } = null!;

        public int AdminId { get; set; }
        public UserEntity Admin { get; set; } = null!;

        public int CreatedByUserId { get; set; }
        public UserEntity CreatedBy { get; set; } = null!;

        public int CustomerId { get; set; }
        public CustomerEntity Customer { get; set; } = null!;

        public string? Status { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<InvoiceItemsEntity> Items { get; set; } = new List<InvoiceItemsEntity>();
    }
}
