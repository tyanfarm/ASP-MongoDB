using MongoDB.Bson;

namespace ASP_MongoDB.Models {
    public class Employee {
        public ObjectId Id {get; set;}
        
        public int EmployeeID {get; set;}

        public string? EmployeeName {get; set;}

        public string? Department {get; set;}

        public string DateOfJoining {get; set;}

        public string PhotoFileName {get; set;}
    }
}