using ASP_MongoDB.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ASP_MongoDB.Controllers 
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DepartmentController : ControllerBase {
        private readonly IConfiguration _configuration;
        
        public DepartmentController(IConfiguration configuration) {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get() {
            // Khởi tạo kết nối
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("DefaultConnection"));

            var dbList = dbClient.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Department department) {
            // Khởi tạo kết nối
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("DefaultConnection"));

            int lastDepartmentID = dbClient.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable().Count();

            department.DepartmentID = lastDepartmentID + 1;

            dbClient.GetDatabase("testdb").GetCollection<Department>("Department").InsertOne(department);

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Department department) {
            // Khởi tạo kết nối
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("DefaultConnection"));

            // Tìm DepartmentID trong DB theo ID được đưa vào
            var filter = Builders<Department>.Filter.Eq("DepartmentID", department.DepartmentID);

            // Update DepartmentName
            var update = Builders<Department>.Update.Set("DepartmentName", department.DepartmentName);

            dbClient.GetDatabase("testdb").GetCollection<Department>("Department").UpdateOne(filter, update);

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id) {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("DefaultConnection"));
            
            // Tìm DepartmentID trong DB theo ID được đưa vào
            var filter = Builders<Department>.Filter.Eq("DepartmentID", id);

            dbClient.GetDatabase("testdb").GetCollection<Department>("Department").DeleteOne(filter);

            return new JsonResult("Deleted Successfully");
        }
    }

}