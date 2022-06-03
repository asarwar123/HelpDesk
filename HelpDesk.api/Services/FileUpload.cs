using Azure.Storage.Blobs;

namespace HelpDesk.api.Services
{
    public class FileUpload : IFileUpload
    {
        private readonly BlobServiceClient _blobServiceClient;

        public FileUpload(BlobServiceClient blobServiceClient )
        {
            _blobServiceClient = blobServiceClient 
                    ?? throw new ArgumentNullException(nameof(blobServiceClient));
        }

        public async Task UploadFile(IFormFile file)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("upload-files");
            var blobClient = blobContainer.GetBlobClient(file.FileName);

            await blobClient.UploadAsync(file.OpenReadStream());
        }
    }
}
