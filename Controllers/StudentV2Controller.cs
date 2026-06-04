using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace StudentApi.Controllers
{
    [ApiController]
    //[ApiVersion("2.0")]
    [Route("api/v2/student")]
    public class StudentV2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Student API V2");
        }
    }
}
