using StudentApi.Models;

namespace StudentApi.Interfaces
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllAsync();
        Task AddAsync(Student student);
        Task DeleteAsync(int id);
    }
}
