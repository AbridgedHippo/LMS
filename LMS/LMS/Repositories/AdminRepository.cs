using LMS.DataAccess;
using LMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LMS.Repositories
{
    public class AdminRepository
    {
        private LMSContext context;
        private ApplicationUserManager userManager;
        private RoleManager<IdentityRole> roleManager;

        public AdminRepository()
        {

        }

        public IEnumerable<User> GetUsers()
        {
            using (var context = new LMSContext())
            {
                using (var store = new UserStore<User>(context))
                {
                    using (var manager = new ApplicationUserManager(store))
                    {
                        return manager.Users.ToList();
                    }
                }
            }
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            using (var context = new LMSContext())
            {
                using (var store = new RoleStore<IdentityRole>(context))
                {
                    using (var manager = new RoleManager<IdentityRole>(store))
                    {
                        return manager.Roles.ToList();
                    }
                }
            }
        }

        public async Task<IdentityResult> CreateUser(User user, string password)
        {
            using (var context = new LMSContext())
            {
                using(var store = new UserStore<User>(context))
                {
                    using (var manager = new ApplicationUserManager(store))
                    {
                        var result = await manager.CreateAsync(user, password);
                        await manager.AddToRoleAsync(user.Id, user.Role);
                        return result;
                    }
                }
            }
        }
        public async Task<IdentityResult> EditUser(AdminUserListViewModel model)
        {
            using (var context = new LMSContext())
            {
                using (var store = new UserStore<User>(context))
                {
                    using (var manager = new ApplicationUserManager(store))
                    {
                        var user = manager.Users.FirstOrDefault(u => u.Id == model.Id);
                        if(user.UserName == "Admin" && (model.UserName != user.UserName || model.Role != user.Role))
                        {
                            return null;
                        }
                        user.UserName = model.UserName;
                        user.Email = model.Email;
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        await manager.RemoveFromRoleAsync(user.Id, user.Role);
                        user.Role = model.Role;
                        await manager.AddToRoleAsync(user.Id, user.Role);
                        return await manager.UpdateAsync(user);
                    }
                }
            }
        }
        public async Task<IdentityResult> DeleteUser(string id)
        {
            using (var context = new LMSContext())
            {
                using (var store = new UserStore<User>(context))
                {
                    using (var manager = new ApplicationUserManager(store))
                    {
                        var user = await manager.FindByIdAsync(id);
                        return await manager.DeleteAsync(user);
                    }
                }
            }
        }
        public async Task<IdentityResult> CreateRole(string name)
        {
            using (var context = new LMSContext())
            {
                using (var store = new RoleStore<IdentityRole>(context))
                {
                    using (var manager = new RoleManager<IdentityRole>(store))
                    {
                        return await manager.CreateAsync(new IdentityRole(name));
                    }
                }
            }
        }
        public async Task<IdentityResult> DeleteRole(string name)
        {
            using (var context = new LMSContext())
            {
                using (var store = new RoleStore<IdentityRole>(context))
                {
                    using (var manager = new RoleManager<IdentityRole>(store))
                    {
                        var role = await manager.FindByNameAsync(name);
                        return await manager.DeleteAsync(role);
                    }
                }
            }
        }
    }
}