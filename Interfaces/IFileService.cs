using StudentApi.DTOs;

namespace StudentApi.Interfaces
{
    public interface IFileService
    {
        Task<FileUploadResponseDto> UploadAsync(IFormFile file);
    }
}
