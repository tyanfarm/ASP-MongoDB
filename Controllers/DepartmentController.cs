using ASP_MongoDB.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ASP_MongoDB.Controllers 
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DepartmentController : ControllerBase {
        private readonly IConfiguration _configuration;
        
        // Thông tin thư mục gốc, tên môi trường, ...
        private readonly IWebHostEnvironment _environment;

        public DepartmentController(IConfiguration configuration, IWebHostEnvironment environment) {
            _configuration = configuration;
            _environment = environment;
        }

        [HttpGet]
        public JsonResult Get() {
            // khi sử dụng MongoClient thì không có xác minh IP
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("DefaultConnection"));

            var dbList = dbClient.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable();

            return new JsonResult(dbList);
        }
    }

}