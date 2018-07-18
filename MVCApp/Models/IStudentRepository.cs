using System.Collections.Generic;

namespace MVCApp.Models
{
    public interface IStudentRepository
    {
        IEnumerable<University> GetUniversities();
        IEnumerable<Course> GetCoursesByUniversityId(int UniversityId);
        IEnumerable<Student> GetStudentsByCourseId(int courseId);
    }
}