using HelpDesk.api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.api.Controllers
{
    [Route("v1/FileUpload")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly ILogger<FileUploadController> _logger;
        private readonly IFileUpload _fileUpload;

        public FileUploadController(ILogger<FileUploadController> logger,IFileUpload fileUpload)
        {
            _logger = logger 
                ?? throw new ArgumentNullException(nameof(logger));
            _fileUpload = fileUpload 
                ?? throw new ArgumentNullException(nameof(fileUpload));
        }

        [HttpPost]
        public ActionResult UploadFile([FromForm]IFormFile file)
        {
            _fileUpload.UploadFile(file);
            return Ok();
        }
    }
}
