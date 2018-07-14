/*  This application is based on the Asp.Net MVC template provided by visual studio
 *  
 *  This tutorial helped with sorting logic:
 *  https://www.itworld.com/article/2956575/development/how-to-sort-search-and-paginate-tables-in-asp-net-mvc-5.html
 *  
 *  I also found this tutorial really helpful:
 *  https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
 *  
 */

using MVCApp.Business;
using MVCApp.Models;
using System.Web.Mvc;

namespace MVCApp.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        IBusinessServices _business;
        public HomeController() :  this(new BusinessServices()) { }
        public HomeController(IBusinessServices business)
        {
            _business = business;
        }

        public ViewResult Index(string sortOrder)
        {
            ViewBag.UniversityNameSortParam = sortOrder == "universityName" ? "universityName_desc" : "universityName";
            ViewBag.CurrentSort = sortOrder;

            return View("Index", _business.GetUniversities(sortOrder));
        }   
        
        public ViewResult Courses(int id, string sortOrder)
        {
            ViewBag.CourseNameSortParam = sortOrder == "courseName" ? "coursName_desc" : "coursName";
            ViewBag.CurrentSort = sortOrder;

            return View("Courses", _business.GetCoursesByUniversityId(id, sortOrder));
        }

        public ViewResult Students(int id, string sortOrder)
        {
            ViewBag.FirstNameSortParam = sortOrder == "firstName" ? "firstName_desc" : "firstName";
            ViewBag.LastNameSortParam = sortOrder == "lastName" ? "lastName_desc" : "lastName";
            ViewBag.CurrentSort = sortOrder;

            return View("Students", _business.GetStudentsByCourseId(id, sortOrder));
        }
    }
}