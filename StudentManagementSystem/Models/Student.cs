namespace StudentManagementSystem.Models
{
    public class Student
    {
        public string StudentId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
