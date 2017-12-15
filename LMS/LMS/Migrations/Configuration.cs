namespace LMS.Migrations
{
    using LMS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS.DataAccess.LMSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LMS.DataAccess.LMSContext context)
        {

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                roleManager.Create(new IdentityRole("Admin"));
            }
            if (!context.Users.Any(u => u.UserName == "Admin"))
            {
                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore);
                var user = new User
                {
                    UserName = "Admin",
                    Email = "admin@LMS.com",
                    FirstName = "admin",
                    LastName = "admin",
                    Role = "Admin"
                };
                userManager.Create(user, "Admin-1234");
                userManager.AddToRole(user.Id, "Admin");
            }
        }
    }
}
