namespace Storefront.UserService.Entities
{
    public class EntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
