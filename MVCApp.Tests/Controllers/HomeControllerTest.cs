using System.Web.Mvc;
using MVCApp.Controllers;
using MVCApp.Models;
using System.Web.Routing;
using System.Security.Principal;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using MVCApp.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCApp.Business;

namespace MVCApp.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_Get_AsksForIndexView()
        {
            var controller = GetHomeController(new BusinessServices(new InMemoryStudentRepository()));
            ViewResult result = controller.Index("universityName");
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Index_Get_retrievesAllUniversitiesFromRepository()
        {
            InMemoryStudentRepository repository = new InMemoryStudentRepository();
            IBusinessServices business = new BusinessServices(repository);
            var controller = GetHomeController(business);

            University university1 = new University { UniversityName = "University1", Id = 1 };
            University university2 = new University { UniversityName = "University2", Id = 2 };

            repository.AddUniversity(university1);
            repository.AddUniversity(university2);

            var result = controller.Index("universityName");

            var model = (IEnumerable<University>)result.ViewData.Model;
            CollectionAssert.Contains(model.ToList(), university1);
            CollectionAssert.Contains(model.ToList(), university2);

        }

        
        IEnumerable<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            for (int i = 0; i < 4; i++)
            {
                students.Add(new Student { FirstName = "FirstName" + i, LastName = "LastName" + i });
            }
            return students;
        }


        IEnumerable<University> GetUniversities()
        {
            List<University> universities = new List<University>();
            for (int i = 0; i < 4; i++)
            {
                EntitySet<Course> courses = new EntitySet<Course>();
                courses.AddRange(GetCourses());
                universities.Add(new University { UniversityName = "UniversityName" + i, Courses = courses });
            }
            return universities;
        }

        IEnumerable<Course> GetCourses()
        {
            List<Course> courses = new List<Course>();
            for (int i = 0; i < 4; i++)
            {
                EntitySet<Registration> registration = new EntitySet<Registration>();
                courses.Add(new Course { CourseName = "Course" + i, University = GetUniversities().ElementAt(i) });
            }
            return courses;
        }

        private static HomeController GetHomeController(IBusinessServices business)
        {
            HomeController controller = new HomeController(business);

            controller.ControllerContext = new ControllerContext()
            {
                Controller = controller,
                RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
            };
            return controller;
        }

        private class MockHttpContext : HttpContextBase
        {
            private readonly IPrincipal _user = new GenericPrincipal(
                new GenericIdentity("someUser"), null);

            public override IPrincipal User
            {
                get
                {
                    return _user;
                }
                set
                {
                    base.User = value;
                }
            }
        }
        
    }
}
