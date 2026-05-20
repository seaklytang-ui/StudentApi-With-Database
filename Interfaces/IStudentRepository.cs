using StudentApi.Models;

namespace StudentApi.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task AddAsync(Student student);
        Task<Student> GetByIdAsync(int id);
        Task<Student> PutAsync(Student student);
        Task DeleteAsync(Student student);
    }
}
