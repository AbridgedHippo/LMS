using LMS.Models;
using LMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Title = "Home Page";

            return View();
    }

        //public ActionResult Schedules()
        //{
        //    var schedules = schedulesRepo.GetAll();
        //    return View(schedules);
        //}
    }
}
