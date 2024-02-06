namespace Authentication.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Category { get; set; }
    }
}
