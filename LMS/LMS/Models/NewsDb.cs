using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class NewsDb : DbContext
    {
        public NewsDb() : base("name=DefaultConnection")
        {}

        public DbSet<Newsfeed> Feeds { get; set; }
    }
}