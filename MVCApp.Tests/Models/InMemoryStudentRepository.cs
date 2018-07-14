using MVCApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace MVCApp.Tests.Models
{
    class InMemoryStudentRepository : IStudentRepository
    {
        private List<Student> _students = new List<Student>();
        private List<Course> _courses = new List<Course>();
        private List<University> _universities = new List<University>();

        public Exception ExceptionToThrow { get; set; }

        public IEnumerable<Course> GetCoursesByUniversityId(int UniversityId, string sortOrder)
        {
            return _courses;
        }

        public IEnumerable<Student> GetStudentsByCourseId(int courseId, string sortOrder)
        {
            return _students;
        }

        public IEnumerable<University> GetUniversities(string sortOrder)
        {
            return _universities;
        }   
        
        public void AddStudent(string firstName, string lastName, int id)
        {
            _students.Add(new Student { FirstName = firstName,  LastName = lastName, Id = id});
        }

        public void AddCourse(string courseName, int universityId, int id)
        {
            _courses.Add(new Course { CourseName = courseName, UniversityId = universityId, Id = id});
        }

        public void AddUniversity(University university)
        {
            _universities.Add(university);
        }
    }
}
