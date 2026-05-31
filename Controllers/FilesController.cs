using Microsoft.AspNetCore.Mvc;
using StudentApi.Interfaces;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class FilesController : Controller
    {
       private readonly IFileService _Service;
        public FilesController(IFileService service)
        {
            _Service = service;
        }

        [HttpPost("Upload")]
        public async Task<ActionResult> Upload(IFormFile file)
        {
            try
            {
                var result = await _Service.UploadAsync(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
