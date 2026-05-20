using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Interfaces;
using StudentApi.Models;
using StudentApi.Repositories;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        //private readonly AppDbContext _context;

        //public StudentController(AppDbContext context)
        //{
        //    _context = context;
        //}

        private readonly IStudentService _service;
        public StudentController(IStudentService service)
        {
            _service = service;
        }


        // GET ALL
        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            return await _service.GetAllAsync();
        }

        // POST
        [HttpPost]
        public async Task<ActionResult> AddStudent(Student student)
        {
            //_repository.AddAsync(student);
            //_context.Students.Add(student);
            
            //await _context.SaveChangesAsync();

            await _service.AddAsync(student);
            return Ok("Student added successfully");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            //var student = await _context.Students.FindAsync(id);
            //var student = await _repository.GetByIdAsync(id);
            //if (student == null)
            //{
            //    return NotFound("Student not found");
            //}

            await _service.DeleteAsync(id);

            //_context.Students.Remove(student);
            //_repository.DeleteAsync(student);

            //await _context.SaveChangesAsync();

            return Ok("Deleted successfully");
        }

        //// PUT
        //[HttpPut("{id}")]
        //public async Task<ActionResult> UpdateStudent(int id , Student student)
        //{
        //    //var existingstudent = await _repository.GetByIdAsync(id);
        //    //if(existingstudent == null)
        //    //{
        //    //    return null;
        //    //}
        //    //existingstudent.Name = student.Name;
        //    //existingstudent.Major = student.Major;
        //    //existingstudent.Age = student.Age;
        //    //return await _repository.PutAsync(existingstudent);

        //    //var student = await _context.Students.FindAsync(id);
        //    //if (student != null)
        //    //{
        //    //    student.Name = NewName;
        //    //    student.Major = NewMajor;
        //    //    student.Age = NewAge;
        //    //    await _repository.SaveChangesAsync();

        //    //    return Ok("Update successfully");
        //    //}

        //    //return NotFound("Student not found");

        //}
    }
}