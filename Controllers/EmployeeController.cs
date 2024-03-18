using Microsoft.AspNetCore.Mvc;

namespace ASP_MongoDB.Controllers 
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeeController : ControllerBase {
        private readonly IConfiguration _configuration;
        
        // Thông tin thư mục gốc, tên môi trường, ...
        private readonly IWebHostEnvironment _environment;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment environment) {
            _configuration = configuration;
            _environment = environment;
        }
    }

}