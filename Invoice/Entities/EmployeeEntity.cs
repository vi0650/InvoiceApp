namespace Invoice.Entities
{
    public class EmployeeEntity
    {
        public int EmployeeId { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;

        public int AdminId { get; set; }
        public UserEntity Admin { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
