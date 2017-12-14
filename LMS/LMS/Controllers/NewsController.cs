using LMS.DataAccess;
using LMS.Models;
using LMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace LMS.Controllers
{
    public class NewsController : Controller
    {
        private GenericRepository<Newsfeed> repo = new GenericRepository<Newsfeed>();

        public ActionResult PartialFeed()
        {
            var model = repo.GetAll();
            return PartialView("_Newsfeed", model);
        }

        public ActionResult Feed()
        {
            var model = repo.GetAll();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [RedirectAuthorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(string title, string breadtext)
        {
            repo.Add(new Newsfeed { Title = title, BreadText = breadtext, PubDate = DateTime.UtcNow});
            
            return View();
        }
    }
}
