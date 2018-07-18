using MVCApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MVCApp.Business
{
    public class BusinessServices : IBusinessServices
    {
        IStudentRepository _repository;
        public BusinessServices() :  this(new StudentRepository()) { }
        public BusinessServices(IStudentRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<Course> GetCoursesByUniversityId(int universityId, string sortOrder)
        {
            var courses = _repository.GetCoursesByUniversityId(universityId).ToList();

            switch (sortOrder)
            {
                case "courseName_desc":
                    return courses.OrderByDescending(x => x.CourseName);
                default:
                    return courses.OrderBy(x => x.CourseName);
            }
        }

        public IEnumerable<Student> GetStudentsByCourseId(int id, string sortOrder)
        {
            var students = _repository.GetStudentsByCourseId(id).ToList();

            switch (sortOrder)
            {
                case "firstName":
                    return students.OrderBy(x => x.FirstName);
                case "firstName_desc":
                    return students.OrderByDescending(x => x.FirstName);
                case "lastName_desc":
                    return students.OrderByDescending(x => x.LastName);
                default:
                    return students.OrderBy(x => x.LastName);
            }
        }

        public IEnumerable<University> GetUniversities(string sortOrder)
        {
            var universities = _repository.GetUniversities().ToList();

            switch (sortOrder)
            {
                case "universityName_desc":
                    return universities.OrderByDescending(x => x.UniversityName);
                default:
                    return universities.OrderBy(x => x.UniversityName);
            }
        }
    }
}