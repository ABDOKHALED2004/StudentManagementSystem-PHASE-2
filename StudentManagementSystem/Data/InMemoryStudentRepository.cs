using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    /// <summary>
    /// Simple in-memory repository. Your friend can swap this with an ADO.NET SQL repo.
    /// </summary>
    public sealed class InMemoryStudentRepository : IStudentRepository
    {
        private readonly List<Student> _students = new();

        public InMemoryStudentRepository(bool seedDemoData = false)
        {
            if (seedDemoData)
            {
                _students.AddRange(new[]
                {
                    new Student { StudentId = "1001", Name = "Ahmed Ali", Age = 20, Department = "CS", Phone = "01000000000" },
                    new Student { StudentId = "1002", Name = "Sara Mohamed", Age = 19, Department = "IT", Phone = "01111111111" },
                });
            }
        }

        public IEnumerable<Student> GetAll() => _students
            .OrderBy(s => s.StudentId, StringComparer.OrdinalIgnoreCase)
            .ToList();

        public Student? GetById(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId)) return null;

            return _students.FirstOrDefault(s =>
                string.Equals(s.StudentId.Trim(), studentId.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void Add(Student student)
        {
            if (student is null) throw new ArgumentNullException(nameof(student));
            if (string.IsNullOrWhiteSpace(student.StudentId))
                throw new InvalidOperationException("Student ID is required.");

            if (GetById(student.StudentId) != null)
                throw new InvalidOperationException("Student ID already exists.");

            _students.Add(Clone(student));
        }

        public bool Update(Student student)
        {
            if (student is null) throw new ArgumentNullException(nameof(student));
            if (string.IsNullOrWhiteSpace(student.StudentId))
                throw new InvalidOperationException("Student ID is required.");

            var existing = GetById(student.StudentId);
            if (existing is null) return false;

            existing.Name = student.Name;
            existing.Age = student.Age;
            existing.Department = student.Department;
            existing.Phone = student.Phone;

            return true;
        }

        public bool Delete(string studentId)
        {
            var existing = GetById(studentId);
            if (existing is null) return false;

            _students.Remove(existing);
            return true;
        }

        private static Student Clone(Student s) => new Student
        {
            StudentId = s.StudentId.Trim(),
            Name = s.Name.Trim(),
            Age = s.Age,
            Department = s.Department.Trim(),
            Phone = s.Phone.Trim()
        };
    }
}
