using StudentApi.DTOs;
using StudentApi.Interfaces;

namespace StudentApi.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<FileUploadResponseDto> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("No file Uploaded");
            }
            var uploadsFolder = Path.Combine(
                _environment.ContentRootPath,
                "Uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid().ToString() +
                Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using(var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new FileUploadResponseDto
            {
                FileName = fileName,
                filePath = filePath
            };

        }

    }
}
