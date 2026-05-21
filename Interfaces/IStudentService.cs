using StudentApi.DTOs;
using StudentApi.Models;

namespace StudentApi.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentDto>> GetPagedAsync(int page, int pageSize , string? search);
        Task<List<StudentDto>> GetAllAsync();
        Task AddAsync(Student student);
        Task DeleteAsync(int id);
    }
}
