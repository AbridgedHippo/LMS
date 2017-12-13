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

namespace LMS.Controllers
{
    public class ManageAccountController : ApiController
    {
        // GET: Admin
        public IHttpActionResult Index()
        {
            return Ok();
        }
    }
}