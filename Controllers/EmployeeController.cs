using ASP_MongoDB.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

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

        [HttpGet]
        public JsonResult Get() {
            // Khởi tạo kết nối
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("DefaultConnection"));

            var dbList = dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Employee employee) {
            // Khởi tạo kết nối
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("DefaultConnection"));

            int lastEmployeeID = dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").AsQueryable().Count();

            employee.EmployeeID = lastEmployeeID + 1;

            dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").InsertOne(employee);

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee employee) {
            // Kiểm tra xem ID có được đưa vào không
            if (employee.EmployeeID == 0) {
                return new JsonResult("Updated Failed");
            }

            // Khởi tạo kết nối
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("DefaultConnection"));

            // Tìm EmployeeID trong DB theo ID được đưa vào
            var filter = Builders<Employee>.Filter.Eq("EmployeeID", employee.EmployeeID);

            // Update Employee
            var update = Builders<Employee>.Update.Set("EmployeeName", employee.EmployeeName)
                                                    .Set("Department", employee.Department)
                                                    .Set("DateOfJoining", employee.DateOfJoining)
                                                    .Set("PhotoFileName", employee.PhotoFileName);

            dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").UpdateOne(filter, update);

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id) {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("DefaultConnection"));
            
            // Tìm EmployeeID trong DB theo ID được đưa vào
            var filter = Builders<Employee>.Filter.Eq("EmployeeID", id);

            dbClient.GetDatabase("testdb").GetCollection<Employee>("Employee").DeleteOne(filter);

            return new JsonResult("Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile() {
            try {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _environment.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create)) {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);

            }
            catch (Exception) {
                return new JsonResult("anonymous.png");
            }
        }
    }

}