namespace Storefront.UserService.Entities
{
    public class WhishList : EntityBase
    {
        public Guid UserId { get; set; }

        public User? User { get; set; }
    }
}
