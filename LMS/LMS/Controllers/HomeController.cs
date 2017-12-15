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

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Admin");
            }

            return View();
        }
        
        [Authorize(Roles ="Admin")]
        public ActionResult Admin()
        {
            var repo = new AdminRepository();
            var model = new
            {
                Users = repo.GetUsers().Select(u => new AdminUserListViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Role = u.Role,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                }),
                Roles = repo.GetRoles().Select(r => new AdminRoleListViewModel
                {
                    Id = r.Id,
                    Name = r.Name
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
