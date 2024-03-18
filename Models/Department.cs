using MongoDB.Bson;

namespace ASP_MongoDB.Models {
    public class Department {
        public ObjectId Id {get; set;}
        
        public int DepartmentID {get; set;}

        public string DepartmentName {get; set;}
    }
}