using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // POST
        [HttpPost]
        public async Task<ActionResult> AddStudent(Student student)
        {
            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            return Ok("Student added successfully");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound("Student not found");
            }

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            return Ok("Deleted successfully");
        }
    }
}