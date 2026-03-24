namespace Invoice.Entities
{
    public class RoleEntity
    {
        public int RoleId { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}
