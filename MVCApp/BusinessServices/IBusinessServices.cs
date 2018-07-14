using System.Collections.Generic;

namespace MVCApp.Business
{
    public interface IBusinessServices
    {
        IEnumerable<Course> GetCoursesByUniversityId(int universityId, string sortOrder);
        IEnumerable<University> GetUniversities(string sortOrder);
        IEnumerable<Student> GetStudentsByCourseId(int id, string sortOrder);
    }
}