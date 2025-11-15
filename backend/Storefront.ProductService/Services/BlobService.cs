using Azure.Storage.Blobs;

namespace Storefront.ProductService.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(IConfiguration config)
        {
            var connectionString = config["AzureBlob:ConnectionString"];
            var containerName = config["AzureBlob:ContainerName"];
            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadAsync(IFormFile file, string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);
            return blobClient.Uri.ToString();
        }

        public async Task DeleteAsync(string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllAsync(string? prefix = null)
        {
            var urls = new List<string>();

            await foreach (var blobItem in _containerClient.GetBlobsAsync(prefix: prefix))
            {
                var blobClient = _containerClient.GetBlobClient(blobItem.Name);
                urls.Add(blobClient.Uri.ToString());
            }

            return urls;
        }
    }

}
