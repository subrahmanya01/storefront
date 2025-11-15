namespace Storefront.CartService.Behaviours
{
    public class UnidentifiedUserException : Exception
    {
        public UnidentifiedUserException()
            : base("User identity could not be determined from the request.")
        {
        }
    }
}
