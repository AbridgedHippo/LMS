using LMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LMS.DataAccess
{
    public class LMSContext : IdentityDbContext<User>
    {
        //public DbSet<Schedule> Schedules { get; set; }

        public LMSContext() : base("DefaultConnection", throwIfV1Schema: false) { }

        public static LMSContext Create()
        {
            return new LMSContext();
        }

        public DbSet<Newsfeed> Feeds { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<SubmittedAssignment> SubmittedAssignments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
    }

}