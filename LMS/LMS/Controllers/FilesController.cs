using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LMS.DataAccess;
using LMS.Models;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;



using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;


using System.Web.Http.ModelBinding;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using LMS.Providers;
using LMS.Results;
using LMS.Repositories;

namespace LMS.Controllers
{
    public class FilesController : LMSApiController
    {
        private LMSContext db = new LMSContext();

        // GET: api/Files
        public IQueryable<File> GetFiles()
        {
            return db.Files;
        }

        // GET: api/Files/5
        [ResponseType(typeof(File))]
        public IHttpActionResult GetFile(int id)
        {
            File file = db.Files.Find(id);
            if (file == null)
            {
                return NotFound();
            }

            return Ok(file);
        }

        // PUT: api/Files/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFile(int id, File file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != file.FileId)
            {
                return BadRequest();
            }

            db.Entry(file).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Files
        public IHttpActionResult PostFile()
        {
            var upload = System.Web.HttpContext.Current.Request.Files;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            for (int i = 0; i < upload.Count; i++)
            {
                if (upload[i] != null && upload[i].ContentLength > 0)
                {
                    var file = new File
                    {
                        FileName = System.IO.Path.GetFileName(upload[i].FileName),
                        ContentType = upload[i].ContentType,
                        PublisherId = null //System.Web.HttpContext.Current.Session["IDString"].ToString()
                    };
                    using (var reader = new System.IO.BinaryReader(upload[i].InputStream))
                    {
                        file.Content = reader.ReadBytes(upload[i].ContentLength);
                    }
                    db.Files.Add(file);
                    db.SaveChanges();
                }
            }

            return StatusCode(HttpStatusCode.Created);
        }

        // DELETE: api/Files/5
        [ResponseType(typeof(File))]
        public IHttpActionResult DeleteFile(int id)
        {
            File file = db.Files.Find(id);
            if (file == null)
            {
                return NotFound();
            }

            db.Files.Remove(file);
            db.SaveChanges();

            return Ok(file);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FileExists(int id)
        {
            return db.Files.Count(e => e.FileId == id) > 0;
        }
    }
}