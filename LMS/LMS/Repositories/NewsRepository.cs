using LMS.DataAccess;
using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Repositories
{
    public class NewsRepository
    {
        NewsDb nb = new NewsDb();

        public IEnumerable<Models.Newsfeed> ShowFeed()
        {
            return nb.Feeds;
        }

        public void AddToFeed(Newsfeed newsfeed)
        {
            nb.Feeds.Add(newsfeed);
            //nb.SaveChanges();
        }

        public void AddToFeed(string title, string breadText)
        {
            Newsfeed newsfeed = new Newsfeed();
            newsfeed.Title = title;
            newsfeed.BreadText = breadText;
            newsfeed.PubDate = DateTime.Today;

            nb.Feeds.Add(newsfeed);
            nb.SaveChanges();
        }
    }
}