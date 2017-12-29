using System.Security.Policy;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using LMS.Models;
using LMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LMS.Controllers
{
    public class HomeController : Controller
    {
        // Example on how to use the generic repository
        //GenericRepository<Schedule> schedulesRepo;

        public HomeController()
        {
            //schedulesRepo = new GenericRepository<Schedule>();
        }
        
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var model = new DashboardVM();

            // Bootstrapping data for the Dashboard
            if (User.IsInRole("Admin"))
            {
                // For testing
                var submitted = new GenericRepository<SubmittedAssignment>().GetAll();
                model.UngradedAssignments = submitted.Where(a => a.Grade == Grade.U).ToList();
                model.GradedAssignments = submitted.Where(a => a.Grade != Grade.U && a.Show).ToList();
                var assignments = new GenericRepository<Assignment>().GetAll().ToList();
                assignments.RemoveAll(a => submitted.Any(s => s.AssignmentId == a.Id));
                model.AssignmentsToSubmit = assignments;
            }
            else if (User.IsInRole("Teacher"))
            {
                var repo = new GenericRepository<SubmittedAssignment>();
                var list = repo.GetAll().ToList().Where(a => a.Grade == Grade.U 
                && a.Assignment.Course.Teachers.Any(t => t.UserId == User.Identity.GetUserId()));
                model.UngradedAssignments = repo.GetAll().ToList().Where(a => a.Grade == Grade.U
                && a.Assignment.Course.Teachers.Any(t => t.UserId == User.Identity.GetUserId())).ToList();
            }
            else if (User.IsInRole("Student"))
            {
                var userId = User.Identity.GetUserId();
                var submitted = new GenericRepository<SubmittedAssignment>().GetAll();
                model.GradedAssignments = submitted.Where(a => a.Grade != Grade.U 
                    && a.UserId == User.Identity.GetUserId() && a.Show).ToList();
                var courses = new GenericRepository<Student>()
                    .Get(s => s.UserId == userId).Courses.ToList();
                var assignments = new GenericRepository<Assignment>().GetAll().ToList()
                    .Where(a => courses.Any(c => c.Id == a.CourseId)).ToList();
                assignments.RemoveAll(a => submitted.Any(s => s.AssignmentId == a.Id));
                model.AssignmentsToSubmit = assignments;
            }
            
            return View(model);
        }
        
        [Authorize(Roles ="Admin")]
        public ActionResult Admin()
        {
            var model = new
            {
                Users = new GenericRepository<User>().GetAll().Select(u => new UserListVM
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Role = u.Role
                }),
                Roles = new GenericRepository<IdentityRole>().GetAll().Select(r => new RoleListVM
                {
                    Id = r.Id,
                    Name = r.Name
                }),
                Courses = new GenericRepository<Course>().GetAll().Select(c => new CourseListVM
                {
                    Id = c.Id,
                    Name = c.Name
                })
            };

            return View("Admin", "", JsonConvert.SerializeObject(model));
        }

        //public ActionResult Schedules()
        //{
        //    var schedules = schedulesRepo.GetAll();
        //    return View(schedules);
        //}
    }
}
