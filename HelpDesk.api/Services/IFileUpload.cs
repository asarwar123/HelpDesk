namespace HelpDesk.api.Services
{
    public interface IFileUpload
    {
        Task UploadFile(IFormFile file);
    }
}
