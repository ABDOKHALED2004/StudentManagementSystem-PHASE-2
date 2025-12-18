using System.Collections.Generic;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAll();
        Student? GetById(string studentId);

        /// <summary>Throws InvalidOperationException if the ID already exists.</summary>
        void Add(Student student);

        /// <summary>Returns true if updated, false if not found.</summary>
        bool Update(Student student);

        /// <summary>Returns true if deleted, false if not found.</summary>
        bool Delete(string studentId);
    }
}
