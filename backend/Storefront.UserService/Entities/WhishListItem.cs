namespace Storefront.UserService.Entities
{
    public class WhishListItem : EntityBase
    {
        public Guid WhishListId { get; set; }

        public string? ProductId { get; set; }

        public WhishList? WhishList { get; set; }
    }
}
