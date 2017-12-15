using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using LMS.Models;
using LMS.Providers;
using LMS.Results;
using LMS.Repositories;
using System.Linq;

namespace LMS.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/Admin")]
    public class AdminController : LMSApiController
    {
        private AdminRepository repo;
        public AdminController()
        {
            repo = new AdminRepository();
        }

        [HttpPost, Route("GetUsers")]
        public IHttpActionResult GetUsers()
        {
            var users = repo.GetUsers();
            return Ok(users.Select(u => new AdminUserListViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Role = u.Role,
                FirstName = u.FirstName,
                LastName = u.LastName
            }));
        }

        [HttpPost, Route("GetRoles")]
        public IHttpActionResult GetRoles()
        {
            var roles = repo.GetRoles();
            return Ok(roles.Select(r => new AdminRoleListViewModel
            {
                Id = r.Id,
                Name = r.Name
            }));
        }

        [Route("CreateUser")]
        public async Task<IHttpActionResult> CreateUser(AdminCreateUserBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = CreateUsername(model.FirstName, model.LastName);
            var password = CreateRandomPassword(username);
            var user = new User
            {
                UserName = username,
                Email = username + "@LMS.com",
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = model.Role
            };

            var result = await repo.CreateUser(user, password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok($"User {username} created successfully! Password: {password}");
        }
        [HttpPost, Route("EditUser")]
        public async Task<IHttpActionResult> EditUser(AdminUserListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await repo.EditUser(model);

            if(result == null)
            {
                return BadRequest("Cannot edit username or role of Admin!");
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok($"User {model.UserName} edited successfully!");
        }
        [HttpPost, Route("DeleteUser")]
        public async Task<IHttpActionResult> DeleteUser(AdminUserListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = model.Id;

            if(id == User.Identity.GetUserId())
            {
                return BadRequest("Cannot delete own account!");
            }
            if (model.UserName == "Admin")
            {
                return BadRequest("Cannot delete Admin!");
            }
            var result = await repo.DeleteUser(id);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok($"User {model.UserName} deleted successfully!");
        }

        [HttpPost, Route("CreateRole")]
        public async Task<IHttpActionResult> CreateRole(AdminCreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await repo.CreateRole(model.Name);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok($"Role {model.Name} created successfully!");
        }

        [HttpPost, Route("DeleteRole")]
        public async Task<IHttpActionResult> DeleteRole(AdminCreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Name == "Admin")
            {
                return BadRequest("Cannot remove role Admin");
            }
            var result = await repo.DeleteRole(model.Name);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok($"Role {model.Name} deleted successfully!");
        }

        // Helpers
        private string CreateUsername(string fName, string lName)
        {
            var date = DateTime.Now.Year.ToString();
            return fName.Substring(0, 3).ToLower() + lName.Substring(0, 3).ToLower() + "-" + (char)date[date.Length - 1];
        }
        private string CreateRandomPassword(string username)
        {
            username = username.Substring(0, username.Length - 2);
            var rng = new Random();
            var n = rng.Next(username.Length);
            var index = username.Length;
            var password = username.ToCharArray();

            while(index != 0)
            {
                n = rng.Next(index);
                index--;
                var tmp = password[index];
                password[index] = password[n];
                password[n] = tmp;
                var s = new string(password);

                if(index == 0 && new string(password) == username)
                {
                    index = password.Length;
                }
            }
            password[0] = password[0].ToString().ToUpper().ToCharArray()[0];
            var result = new string(password);

            for (index = 3; index > 0; index--)
            {
                result += rng.Next(1, 9);
            }
            return result;
        }

    }
}