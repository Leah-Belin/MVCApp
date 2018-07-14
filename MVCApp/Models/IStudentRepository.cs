using System.Collections.Generic;

namespace MVCApp.Models
{
    public interface IStudentRepository
    {
        IEnumerable<University> GetUniversities(string sortOrder);
        IEnumerable<Course> GetCoursesByUniversityId(int UniversityId, string sortOrder);
        IEnumerable<Student> GetStudentsByCourseId(int courseId, string sortOrder);
    }
}