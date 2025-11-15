namespace Storefront.OrderAndShippingService.Services
{
    public static class HelperService
    {
        public static string GenerateOrderNumber()
        {
            var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            var randomPart = GenerateRandomAlphaNumeric(6);
            return $"ORD-{datePart}-{randomPart}";
        }

        private static string GenerateRandomAlphaNumeric(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Range(0, length)
                .Select(_ => chars[random.Next(chars.Length)]).ToArray());
        }
    }
}
