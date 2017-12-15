using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using LMS.Models;
using LMS.Repositories;

namespace LMS.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : LMSApiController
    {
        private AccountRepository repo;
        
        public AccountController()
        {
            repo = new AccountRepository();
        }
        
        // POST api/Account/Logout
        [AllowAnonymous]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            repo.LogOut();
            return Ok();

        }
        
        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await repo.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok("Password changed successfully!");
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await repo.SetPassword(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok("Password set successfully!");
        }     

        // POST api/Account/Register
        //[Authorize(Roles="Admin")]
        //[Route("Register")]
        //public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = new User() { UserName = model.Email, Email = model.Email };

        //    IdentityResult result = await UserManager.CreateAsync(user, model.Password);

        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    return Ok();
        //}
    }
}
