namespace Storefront.ProductService.Services
{
    public interface IBlobService
    {
        Task<string> UploadAsync(IFormFile file, string fileName);
        Task DeleteAsync(string fileName);
        Task<List<string>> GetAllAsync(string? prefix = null);
    }

}
