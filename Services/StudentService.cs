using StudentApi.Models;
using StudentApi.Interfaces;

namespace StudentApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _Repository;
        public StudentService(IStudentRepository repository) 
        { 
            _Repository = repository; 
        }
        public async Task<List<Student>> GetAllAsync()
        {
            return await _Repository.GetAllAsync();
        }

        public async Task AddAsync(Student student)
        {
            // logic
            if (student.Age < 18)
            {
                throw new Exception("Student must be adult");
            }
            await _Repository.AddAsync(student);
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _Repository.GetByIdAsync(id);
            if (student == null)
            {
                throw new Exception("Student not found");
            }
            await _Repository.DeleteAsync(student);
        }
    }
}
