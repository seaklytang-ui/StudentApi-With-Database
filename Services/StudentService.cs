using Microsoft.EntityFrameworkCore;
using StudentApi.DTOs;
using StudentApi.Interfaces;
using StudentApi.Models;

namespace StudentApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _Repository;
        public StudentService(IStudentRepository repository) 
        { 
            _Repository = repository; 
        }
        public async Task<List<StudentDto>> GetAllAsync()
        {
            var students = await _Repository.GetAllAsync();
            return students.Select(s => new StudentDto
            {
                Name = s.Name,
                Major = s.Major
            }).ToList();
        
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

        public async Task<List<StudentDto>> GetPagedAsync(int page, int pageSize, string? search)
        {
            var students = await _Repository
                .GetPagedAsync(page, pageSize, search);
            return students.Select(s => new StudentDto
            {
                Name = s.Name,
                Major = s.Major
            }).ToList();
        }
    }
}
