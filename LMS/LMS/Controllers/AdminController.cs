using LMS.Models;
using LMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using LMS.DataAccess;
using System.Threading.Tasks;
using System.Net.Http;

namespace LMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private int i = 1;
        public UserManager<User> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }
        private LMSContext context;
        //private GenericRepository<User> userRepo;
        //private GenericRepository<IdentityRole> roleRepo;
        public AdminController()
        {
            //userRepo = new GenericRepository<User>();
            //roleRepo = new GenericRepository<IdentityRole>();
            context = new LMSContext();
            UserManager = new UserManager<User>(new UserStore<User>(context));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        private int GetI()
        {
            return i++;
        }
        // GET: Admin
        public ActionResult Index()
        {
            var users = UserManager.Users.ToList();
            var roles = RoleManager.Roles.ToList();
            var list = users.Select(u => new AdminUserListViewModel
            {
                ID = GetI(),
                UserName = u.UserName,
                Role = RoleManager.FindById(u.Roles.FirstOrDefault().RoleId).Name

            });

            return View(list);
        }

        public ActionResult Users()
        {
            var users = UserManager.Users.ToList();
            var roles = RoleManager.Roles.ToList();
            var list = users.Select(u => new AdminUserListViewModel
            {
                ID = GetI(),
                UserName = u.UserName,
                Role = RoleManager.FindById(u.Roles.FirstOrDefault().RoleId).Name

            });

            return View(list);
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser([Bind(Include = "Username, Email, Password, ConfirmPassword")]RegisterBindingModel vm)
        {
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync("/api/account/register", vm);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(vm);
        }
    }
}