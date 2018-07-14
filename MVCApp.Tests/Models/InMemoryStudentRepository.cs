using MVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

/* repository pattern and testing tutorial: https://msdn.microsoft.com/tr-tr/library/ff847525(v=vs.100).aspx
 * 
 */

namespace MVCApp.Tests.Models
{
    class InMemoryStudentRepository : IStudentRepository
    {
        private List<Student> _students = new List<Student>();
        private List<Course> _courses = new List<Course>();
        private List<University> _universities = new List<University>();
        private List<Registration> _registration = new List<Registration>();

        public Exception ExceptionToThrow { get; set; }

        public IEnumerable<Course> GetCoursesByUniversityId(int UniversityId, string sortOrder)
        {
            return _courses.Where(x => x.UniversityId == UniversityId);
        }

        public IEnumerable<Student> GetStudentsByCourseId(int courseId, string sortOrder)
        {
            return from s in _students
                   join r in _registration on s.Id equals r.StudentId
                   where r.CourseId == courseId
                   select s;
        }

        public IEnumerable<University> GetUniversities(string sortOrder)
        {
            return _universities;
        }   
        
        public void AddStudent(Student student)
        {
            _students.Add(student);
        }

        public void AddCourse(Course course)
        {
            _courses.Add(course);
        }

        public void AddUniversity(University university)
        {
            _universities.Add(university);
        }

        public void AddRegistration(Registration registration)
        {
            _registration.Add(registration);
        }
    }
}
