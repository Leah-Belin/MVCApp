using System.Web.Mvc;
using MVCApp.Controllers;
using System.Web.Routing;
using System.Security.Principal;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using MVCApp.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCApp.Business;
using System;

namespace MVCApp.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController _controller;
        private InMemoryStudentRepository _repository;

        private University _university1 = new University { UniversityName = "University1", Id = 1 };
        private University _university2 = new University { UniversityName = "University2", Id = 2 };
        private University _university3 = new University { UniversityName = "University3", Id = 3 };

        private Course _course1 = new Course { CourseName = "Course1", UniversityId = 1, Id = 1 };
        private Course _course2 = new Course { CourseName = "Course2", UniversityId = 1, Id = 2 };
        private Course _course3 = new Course { CourseName = "Course3", UniversityId = 1, Id = 3 };
        private Course _course4 = new Course { CourseName = "Course4", UniversityId = 2, Id = 4 };

        private Student _student1 = new Student { FirstName = "FirstName1", LastName = "LastName4", Id = 1 };
        private Student _student2 = new Student { FirstName = "FirstName2", LastName = "LastName3", Id = 2 };
        private Student _student3 = new Student { FirstName = "FirstName3", LastName = "LastName2", Id = 3 };
        private Student _student4 = new Student { FirstName = "FirstName4", LastName = "LastName1", Id = 4 };


        private Registration _registration1 = new Registration { StudentId = 1, CourseId = 1, Id = 1};
        private Registration _registration2 = new Registration { StudentId = 2, CourseId = 1, Id = 2};
        private Registration _registration3 = new Registration { StudentId = 3, CourseId = 1, Id = 3};
        private Registration _registration4 = new Registration { StudentId = 4, CourseId = 2, Id = 4};
        private Registration _registration5 = new Registration { StudentId = 1, CourseId = 2, Id = 4};

        private void Setup()
        {
            _repository = new InMemoryStudentRepository();
            IBusinessServices business = new BusinessServices(_repository);
            _controller = GetHomeController(business);
        }
        
        [TestMethod]
        public void Index_Get_AsksForIndexView()
        {
            Setup();
            ViewResult result = _controller.Index("universityName");
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Index_Get_retrievesAllUniversitiesFromRepository()
        {
            Setup();

            _repository.AddUniversity(_university1);
            _repository.AddUniversity(_university2);

            var result = _controller.Index("universityName");

            var model = (IEnumerable<University>)result.ViewData.Model;
            Assert.AreEqual(2, model.ToList().Count());
            CollectionAssert.Contains(model.ToList(), _university1);
            CollectionAssert.Contains(model.ToList(), _university2);
        }

        [TestMethod]
        public void Index_Get_RetrievesAllUniversitiesFromRepository_Ascending()
        {
            Setup();

            _repository.AddUniversity(_university1);
            _repository.AddUniversity(_university2);
            _repository.AddUniversity(_university3);

            var result = _controller.Index("universityName");

            var model = (IEnumerable<University>)result.ViewData.Model;
            Assert.AreEqual(_university1.Id, model.ToList().ElementAt(0).Id);
            Assert.AreEqual(_university2.Id, model.ToList().ElementAt(1).Id);
            Assert.AreEqual(_university3.Id, model.ToList().ElementAt(2).Id);
        }

        [TestMethod]
        public void Index_Get_RetrievesAllUniversitiesFromRepository_Descending()
        {
            Setup();

            _repository.AddUniversity(_university1);
            _repository.AddUniversity(_university2);
            _repository.AddUniversity(_university3);

            var result = _controller.Index("universityName_desc");

            var model = (IEnumerable<University>)result.ViewData.Model;
            Assert.AreEqual(_university1.Id, model.ToList().ElementAt(2).Id);
            Assert.AreEqual(_university2.Id, model.ToList().ElementAt(1).Id);
            Assert.AreEqual(_university3.Id, model.ToList().ElementAt(0).Id);
        }

        [TestMethod]
        public void Courses_Get_AsksForCoursesView()
        {
            Setup();
            ViewResult result = _controller.Courses(1, "courseName");
            Assert.AreEqual("Courses", result.ViewName);
        }

        [TestMethod]
        public void Courses_Get_RetrievesCoursesByUniversityId()
        {
            Setup();

            _repository.AddCourse(_course1);
            _repository.AddCourse(_course2);
            _repository.AddCourse(_course3);
            _repository.AddCourse(_course4);

            var result = _controller.Courses(1, "courseName");

            var model = (IEnumerable<Course>)result.ViewData.Model;
            CollectionAssert.Contains(model.ToList(), _course1);
            CollectionAssert.Contains(model.ToList(), _course2);
            CollectionAssert.Contains(model.ToList(), _course3);
            CollectionAssert.DoesNotContain(model.ToList(), _course4);

            result = _controller.Courses(2, "courseName");

            model = (IEnumerable<Course>)result.ViewData.Model;
            CollectionAssert.Contains(model.ToList(), _course4);
            CollectionAssert.DoesNotContain(model.ToList(), _course1);
        }

        [TestMethod]
        public void Courses_Get_RetrievesCoursesByUniversityId_Asc()
        {
            Setup();

            _repository.AddCourse(_course1);
            _repository.AddCourse(_course2);
            _repository.AddCourse(_course3);

            var result = _controller.Courses(1, "courseName");

            var model = (IEnumerable<Course>)result.ViewData.Model;
            Assert.AreEqual(_course1.Id, model.ToList().ElementAt(0).Id);
            Assert.AreEqual(_course2.Id, model.ToList().ElementAt(1).Id);
            Assert.AreEqual(_course3.Id, model.ToList().ElementAt(2).Id);
        }

        [TestMethod]
        public void Courses_Get_RetrievesCoursesByUniversityId_Desc()
        {
            Setup();

            _repository.AddCourse(_course1);
            _repository.AddCourse(_course2);
            _repository.AddCourse(_course3);

            var result = _controller.Courses(1, "courseName_desc");

            var model = (IEnumerable<Course>)result.ViewData.Model;
            Assert.AreEqual(_course1.Id, model.ToList().ElementAt(2).Id);
            Assert.AreEqual(_course2.Id, model.ToList().ElementAt(1).Id);
            Assert.AreEqual(_course3.Id, model.ToList().ElementAt(0).Id);
        }

        [TestMethod]
        public void Students_Get_AsksForStudentsView()
        {
            Setup();
            ViewResult result = _controller.Students(1, "firstName");
            Assert.AreEqual("Students", result.ViewName);
        }

        [TestMethod]
        public void Students_Get_RetrievesStudentsByCourseId()
        {
            Setup();

            _repository.AddStudent(_student1);
            _repository.AddStudent(_student2);
            _repository.AddStudent(_student3);
            _repository.AddStudent(_student4);

            _repository.AddRegistration(_registration1);
            _repository.AddRegistration(_registration2);
            _repository.AddRegistration(_registration3);
            _repository.AddRegistration(_registration4);
            _repository.AddRegistration(_registration5);

            var result = _controller.Students(1, "firstName");

            var model = (IEnumerable<Student>)result.ViewData.Model;
            CollectionAssert.Contains(model.ToList(), _student1);
            CollectionAssert.Contains(model.ToList(), _student2);
            CollectionAssert.Contains(model.ToList(), _student3);
            CollectionAssert.DoesNotContain(model.ToList(), _student4);

            result = _controller.Students(2, "firstName");

            model = (IEnumerable<Student>)result.ViewData.Model;
            CollectionAssert.Contains(model.ToList(), _student4);
            CollectionAssert.Contains(model.ToList(), _student1);
            CollectionAssert.DoesNotContain(model.ToList(), _student2);
        }

        [TestMethod]
        public void Students_Get_RetrievesStudentsByCourseId_FirstNameAsc()
        {
            Setup();

            _repository.AddStudent(_student1);
            _repository.AddStudent(_student2);
            _repository.AddStudent(_student3);
            _repository.AddStudent(_student4);

            _repository.AddRegistration(_registration1);
            _repository.AddRegistration(_registration2);
            _repository.AddRegistration(_registration3);
            _repository.AddRegistration(_registration4);
            _repository.AddRegistration(_registration5);

            var result = _controller.Students(1, "firstName");

            var model = (IEnumerable<Student>)result.ViewData.Model;
            Assert.AreEqual(_student1.Id, model.ToList().ElementAt(0).Id);
            Assert.AreEqual(_student2.Id, model.ToList().ElementAt(1).Id);
            Assert.AreEqual(_student3.Id, model.ToList().ElementAt(2).Id);
        }

        [TestMethod]
        public void Students_Get_RetrievesStudentsByCourseId_FirstNameDesc()
        {
            Setup();

            _repository.AddStudent(_student1);
            _repository.AddStudent(_student2);
            _repository.AddStudent(_student3);
            _repository.AddStudent(_student4);

            _repository.AddRegistration(_registration1);
            _repository.AddRegistration(_registration2);
            _repository.AddRegistration(_registration3);
            _repository.AddRegistration(_registration4);
            _repository.AddRegistration(_registration5);

            var result = _controller.Students(1, "firstName_desc");

            var model = (IEnumerable<Student>)result.ViewData.Model;
            Assert.AreEqual(_student1.Id, model.ToList().ElementAt(2).Id);
            Assert.AreEqual(_student2.Id, model.ToList().ElementAt(1).Id);
            Assert.AreEqual(_student3.Id, model.ToList().ElementAt(0).Id);
        }

        [TestMethod]
        public void Students_Get_RetrievesStudentsByCourseId_LastNameAsc()
        {
            Setup();

            _repository.AddStudent(_student1);
            _repository.AddStudent(_student2);
            _repository.AddStudent(_student3);
            _repository.AddStudent(_student4);

            _repository.AddRegistration(_registration1);
            _repository.AddRegistration(_registration2);
            _repository.AddRegistration(_registration3);
            _repository.AddRegistration(_registration4);
            _repository.AddRegistration(_registration5);

            var result = _controller.Students(1, "lastName");

            var model = (IEnumerable<Student>)result.ViewData.Model;
            Assert.AreEqual(_student1.Id, model.ToList().ElementAt(2).Id);
            Assert.AreEqual(_student2.Id, model.ToList().ElementAt(1).Id);
            Assert.AreEqual(_student3.Id, model.ToList().ElementAt(0).Id);
        }

        [TestMethod]
        public void Students_Get_RetrievesStudentsByCourseId_LastNameDesc()
        {
            Setup();

            _repository.AddStudent(_student1);
            _repository.AddStudent(_student2);
            _repository.AddStudent(_student3);
            _repository.AddStudent(_student4);

            _repository.AddRegistration(_registration1);
            _repository.AddRegistration(_registration2);
            _repository.AddRegistration(_registration3);
            _repository.AddRegistration(_registration4);
            _repository.AddRegistration(_registration5);

            var result = _controller.Students(1, "lastName_desc");

            var model = (IEnumerable<Student>)result.ViewData.Model;
            Assert.AreEqual(_student1.Id, model.ToList().ElementAt(0).Id);
            Assert.AreEqual(_student2.Id, model.ToList().ElementAt(1).Id);
            Assert.AreEqual(_student3.Id, model.ToList().ElementAt(2).Id);
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
