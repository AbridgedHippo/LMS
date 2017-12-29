using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using LMS.Models;
using LMS.DataAccess;

namespace LMS
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class LMSUserManager : UserManager<User>
    {
        public LMSUserManager(IUserStore<User> store) : base(store) {}

        public static LMSUserManager Create(IdentityFactoryOptions<LMSUserManager> options, IOwinContext context)
        {
            var manager = new LMSUserManager(new UserStore<User>(context.Get<LMSContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class LMSRoleManager : RoleManager<IdentityRole>
    {
        public LMSRoleManager(RoleStore<IdentityRole> store) : base(store) {}

        public static LMSRoleManager Create(IdentityFactoryOptions<LMSRoleManager> options, IOwinContext context)
        {
            var manager = new LMSRoleManager(new RoleStore<IdentityRole>(context.Get<LMSContext>()));
            // Configure validation logic for Roles
            manager.RoleValidator = new RoleValidator<IdentityRole>(manager)
            {
                //
            };
            return manager;
        }
    }
}
