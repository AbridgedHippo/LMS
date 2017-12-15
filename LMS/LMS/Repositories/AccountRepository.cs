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
using LMS.DataAccess;

namespace LMS.Repositories
{
    public class AccountRepository : IDisposable
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? new ApplicationUserManager(new UserStore<User>(new LMSContext()));
                //return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        public AccountRepository() {}

        public AccountRepository(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public void LogOut()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);

            var Session = HttpContext.Current.Session;
            if (Session != null)
            {
                Session.Clear();
                Session.RemoveAll();
            }
        }

        public async Task<IdentityResult> SetPassword(string userID, string password)
        {
            using (var manager = UserManager)
            {
                return await manager.AddPasswordAsync(userID, password);
            }
        }
        public async Task<IdentityResult> ChangePassword(string userID, string oldPassword, string newPassword)
        {

            using (var manager = UserManager)
            {
                return await manager.ChangePasswordAsync(userID, oldPassword,
                   newPassword);
            }
            //return await UserManager.ChangePasswordAsync(userID, oldPassword,
            //       newPassword);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
        }
        
        private IAuthenticationManager Authentication
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }
    }
}