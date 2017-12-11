using LMS.Models;
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
        // GET: News
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Index(string RSSURL)
        //{
        //    WebClient wclient = new WebClient();
        //    string RSSData = wclient.DownloadString(RSSURL);

        //    XDocument xml = XDocument.Parse(RSSData);
        //    var RSSFeedData = (from x in xml.Descendants("item")
        //                       select new RSSFeed
        //                       {
        //                           Title = ((string)x.Element("title")),
                                   
        //                       });
        //}
    }
}