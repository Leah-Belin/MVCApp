using System.Collections.Generic;
using System.Linq;

namespace MVCApp.Models
{
    public class StudentRepository : IStudentRepository
    {
        private StudentDBModelDataContext _db = new StudentDBModelDataContext();

        public IEnumerable<Course> GetCoursesByUniversityId(int UniversityId, string sortOrder)
        {
            return _db.Courses.Where(c => c.UniversityId == UniversityId);
        }
        
        public IEnumerable<Student> GetStudentsByCourseId(int courseId, string sortOrder)
        {
            return (from s in _db.Students
                    join r in _db.Registrations on s.Id equals r.StudentId
                    where r.CourseId == courseId
                    select s).ToList();            
        }

        public IEnumerable<University> GetUniversities(string sortOrder)
        {
            return _db.Universities.ToList();            
        }
    }
}