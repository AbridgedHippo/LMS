using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using LMS.Models;
using LMS.Services;

namespace LMS.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/Admin")]
    public class AdminController : LMSApiController
    {
        private AdminService service;
        public AdminController()
        {
            service = new AdminService();
        }

        #region Users

        [HttpPost, Route("GetUsers")]
        public async Task<IHttpActionResult> GetUsers()
        {
            var users = await service.GetUsers();
            List<UserListVM> list = new List<UserListVM>();
            foreach (var user in users)
            {
                list.Add(user);
            }
            return Ok(list);
        }

        [HttpPost, Route("GetUser")]
        public async Task<IHttpActionResult> GetUsers(UserListVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await service.GetUserById(model.Id);
            if(user == null)
            {
                return BadRequest("No User with the given Id!");
            }

            UserDetailsVM result = user;
            return Ok(result);
;        }

        [Route("CreateUser")]
        public async Task<IHttpActionResult> CreateUser(AdminCreateUserBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await service.CreateUser(model);

            if (!result.Item1.Succeeded)
            {
                return GetErrorResult(result.Item1);
            }

            return Ok($"User created successfully! Password: {result.Item2}");
        }

        [HttpPost, Route("EditUser")]
        public async Task<IHttpActionResult> EditUser(UserDetailsVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.EditUser(model);

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
        public async Task<IHttpActionResult> DeleteUser(UserListVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(model.Id == User.Identity.GetUserId())
            {
                return BadRequest("Cannot delete own account!");
            }
            if (model.UserName == "Admin")
            {
                return BadRequest("Cannot delete Admin!");
            }

            var result = await service.DeleteUser(model.Id);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok($"User {model.UserName} deleted successfully!");
        }

        [HttpPost, Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SeUsertPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.SetPassword(model.User.Id, model.Passwords.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok($"Password set successfully!");
        }

        #endregion

        #region Roles

        [HttpPost, Route("GetRoles")]
        public async Task<IHttpActionResult> GetRoles()
        {
            var roles = await service.GetRoles();
            List<RoleListVM> list = new List<RoleListVM>();
            foreach(var role in roles)
            {
                list.Add(role);
            }

            return Ok(list);
        }

        [HttpPost, Route("GetRole")]
        public async Task<IHttpActionResult> GetRole(RoleListVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await service.GetRoleById(model.Id);
            if (role == null)
            {
                return BadRequest("No Role with the given Id!");
            }

            RoleDetailsVM result = role;
            return Ok(result);
        }

        [HttpPost, Route("CreateRole")]
        public async Task<IHttpActionResult> CreateRole(AdminCreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.CreateRole(model.Name);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok($"Role {model.Name} created successfully!");
        }

        [HttpPost, Route("EditRole")]
        public async Task<IHttpActionResult> EditRole(RoleDetailsVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var result = await service.EditRole(model);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok($"Role {model.Name} created successfully!");
        }

        [HttpPost, Route("DeleteRole")]
        public async Task<IHttpActionResult> DeleteRole(RoleListVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Name == "Admin" || model.Name == "Student" || model.Name == "Teacher")
            {
                return BadRequest("Cannot delete this Role!");
            }

            var result = await service.DeleteRole(model.Name);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok($"Role {model.Name} deleted successfully!");
        }

        #endregion

        #region Courses

        [HttpPost, Route("GetCourses")]
        public async Task<IHttpActionResult> GetCourses()
        {
            var courses = await service.GetCourses();
            List<CourseListVM> list = new List<CourseListVM>();
            foreach (var course in courses)
            {
                list.Add(course);
            }

            return Ok(list);
        }

        [HttpPost, Route("GetCourse")]
        public async Task<IHttpActionResult> GetCourse(CourseListVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await service.GetCourseById(model.Id);
            if (course == null)
            {
                return BadRequest("No Course with the given Id!");
            }

            CourseDetailsVM result = course;
            return Ok(result);
        }

        [HttpPost, Route("CreateCourse")]
        public async Task<IHttpActionResult> CreateCourse(CourseListVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await service.CreateCourse(model);

            return Ok("Course created successfully!");
        }

        [HttpPost, Route("EditCourse")]
        public async Task<IHttpActionResult> EditCourse(CourseDetailsVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await service.EditCourse(model);

            return Ok("Course edited successfully!");
        }

        [HttpPost, Route("DeleteCourse")]
        public async Task<IHttpActionResult> DeleteCourse(CourseListVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await service.DeleteCourse(model);

            return Ok("Course deleted successfully!");
        }

        #endregion

        #region Course Users

        [HttpPost, Route("GetTeachersForCourse")]
        public async Task<IHttpActionResult> AddTeacherToCourse(CourseListVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teachers = await service.GetTeachersForCourse(model.Id);
            var list = new List<TeacherListVM>();
            foreach (var teacher in teachers)
            {
                list.Add(teacher);
            }

            return Ok(list);
        }
        [HttpPost, Route("GetStudentsForCourse")]
        public async Task<IHttpActionResult> GetStudentsForCourse(CourseListVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var students = await service.GetStudentsForCourse(model.Id);
            var list = new List<StudentListVM>();
            foreach (var student in students)
            {
                list.Add(student);
            }

            return Ok(list);
        }

        [HttpPost, Route("AddUserToCourse")]
        public async Task<IHttpActionResult> AddUserToCourse(UserCourseVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await service.AddUserToCourse(model.User, model.Course.Id);

            return Ok($"User {model.User.UserName} added successfully to the course {model.Course.Name}!");
        }
        [HttpPost, Route("RemoveUserFromCourse")]
        public async Task<IHttpActionResult> RemoveUserFromCourse(UserCourseVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await service.RemoveUserFromCourse(model.User, model.Course.Id);

            return Ok($"User {model.User.UserName} removed successfully from the course {model.Course.Name}!");
        }

        #endregion

        #region Course Assignments

        [HttpPost, Route("GetAssignment")]
        public async Task<IHttpActionResult> GetAssignment(AssignmentListVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AssignmentDetailsVM assignment = await service.GetAssignment(model.Id);
            if(assignment == null)
            {

            }

            return Ok(assignment);
        }

        [HttpPost, Route("GetAssignmentsForCourse")]
        public async Task<IHttpActionResult> GetAssignmentsForCourse(CourseListVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignments = await service.GetAssignmentsForCourse(model.Id);
            var list = new List<AssignmentListVM>();
            foreach(var assignment in assignments)
            {
                list.Add(new AssignmentListVM
                {
                    Id = assignment.Id,
                    Name = assignment.Name,
                    CourseId = assignment.CourseId
                });
            }

            return Ok(list);
        }

        [HttpPost, Route("CreateAssignmentForCourse")]
        public async Task<IHttpActionResult> CreateAssignmentForCourse(Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await service.CreateAssignmentForCourse(assignment);

            return Ok("Assignment Created successfully!");
        }
        
        [HttpPost, Route("DeleteAssignmentFromCourse")]
        public async Task<IHttpActionResult> DeleteAssignmentFromCourse(Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await service.DeleteAssignmentFromCourse(assignment);

            return Ok("Assignment deleted successfully!");
        }
        
        [HttpPost, Route("EditAssignmentForCourse")]
        public async Task<IHttpActionResult> EditAssignmentForCourse(Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await service.EditAssignmentForCourse(assignment);

            return Ok("Assignment Updated successfully!");
        }

        #endregion

    }
}