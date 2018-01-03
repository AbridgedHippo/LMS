using System.Web.Http.Results;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using LMS.Repositories;

namespace LMS.Controllers
{
    [Authorize(Roles = "Admin, Teacher, Student")]
    public class AssignmentsController : Controller
    {
        GenericRepository<Assignment> repo;
        List<Course> courses;

        public AssignmentsController()
        {
            repo = new GenericRepository<Assignment>();

            // Students will only have the courses that are relevant to their assignments
            if (User != null && User.IsInRole("Student"))
            {
                courses = new GenericRepository<Course>().GetAll().Where(c => c.Students.Any(s => s.UserId == User.Identity.GetUserId())).ToList();
            }
            else
            {
                courses = new GenericRepository<Course>().GetAll().ToList();
            }
        }

        #region Default Stuff

        public ActionResult Index()
        {
            List<Assignment> assignments;

            if (User.IsInRole("Student"))
            {
                assignments = repo.GetAll().Where(a => courses.Any(c => c.Id == a.CourseId)).ToList();
            }
            else
            {
                assignments = repo.GetAll().ToList();
            }

            return View(assignments);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var assignment = repo.Get(id.Value);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(courses, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,CourseId")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                repo.Add(assignment);
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(courses, "Id", "Name", assignment.CourseId);
            return View(assignment);
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var assignment = repo.Get(id.Value);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(courses, "Id", "Name", assignment.CourseId);
            return View(assignment);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,CourseId")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                repo.Update(assignment);
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(courses, "Id", "Name", assignment.CourseId);
            return View(assignment);
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var assignment = repo.Get(id.Value);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            var assignment = repo.Get(id.Value);
            repo.Delete(assignment);
            return RedirectToAction("Index");
        }

        #endregion

        #region Extra Stuff

        public ActionResult SubmitAssignment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new SubmitAssignmentVM { AssignmentId = id.Value };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitAssignment(SubmitAssignmentVM model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var assignment = repo.Get(model.AssignmentId);
            if (assignment == null)
            {
                return HttpNotFound();
            }

            var tmp = new SubmittedAssignment
            {
                AssignmentId = assignment.Id,
                Date = DateTime.Now.ToString(),
                UserId = User.Identity.GetUserId(),
                Answer = model.Answer,
                Grade = Grade.U
            };

            var tmpRepo = new GenericRepository<SubmittedAssignment>();
            tmpRepo.Add(tmp);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ShowGradedAssignment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var repo = new GenericRepository<SubmittedAssignment>();
            var assignment = repo.Get(id.Value);
            if (assignment == null)
            {
                return HttpNotFound();
            }

            return View(assignment);
        }

        [HttpPost]
        [ActionName("ShowGradedAssignment")]
        [ValidateAntiForgeryToken]
        public ActionResult HideGradedAssignment(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var repo = new GenericRepository<SubmittedAssignment>();
            var assignment = repo.Get(id.Value);
            if(assignment == null)
            {
                return HttpNotFound();
            }

            assignment.Show = false;
            repo.Update(assignment);

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult GradeAssignment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var answer = new GenericRepository<SubmittedAssignment>().Get(id.Value).Answer;
            var model = new GradeAssignmentVM { Id = id.Value, Answer = answer };
            return View(model);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public ActionResult GradeAssignment(GradeAssignmentVM model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var tmpRepo = new GenericRepository<SubmittedAssignment>();
            var tmp = tmpRepo.Get(model.Id);
            if (tmp == null)
            {
                return HttpNotFound();
            }

            tmp.Grade = model.Grade;
            tmp.Show = true;
            tmp.Comment = model.Comment;
            tmpRepo.Update(tmp);

            return RedirectToAction("Index", "Home");
        }



        #endregion
    }
}
