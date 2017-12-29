using LMS.DataAccess;
using LMS.Models;
using LMS.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System;

namespace LMS.Services
{
    public class AdminService
    {
        private LMSUserManager _userManager;
        private LMSRoleManager _roleManager;
        private GenericRepository<Course> courseRepo;
        private GenericRepository<Student> studentRepo;
        private GenericRepository<Teacher> teacherRepo;

        public LMSUserManager UserManager
        {
            get { return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<LMSUserManager>(); }
            private set { _userManager = value; }
        }
        public LMSRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.Current.GetOwinContext().GetUserManager<LMSRoleManager>(); }
            private set { _roleManager = value; }
        }

        public AdminService()
        {
            courseRepo = new GenericRepository<Course>();
            studentRepo = new GenericRepository<Student>();
            teacherRepo = new GenericRepository<Teacher>();
        }
        public AdminService(LMSUserManager usermanager, LMSRoleManager rolemanager)
        {
            UserManager = usermanager;
            RoleManager = rolemanager;
        }

        #region Implementation

        #region Users

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await UserManager.Users.ToListAsync();
        }
        public async Task<User> GetUserById(string id)
        {
            return await UserManager.FindByIdAsync(id);
        }
        public async Task<System.Tuple<IdentityResult, string>> CreateUser(User user)
        {
            user.UserName = CreateUsername(user.FirstName, user.LastName); //  // user.FirstName.ToLower() + "." + user.LastName.ToLower();
            user.Email = user.UserName + "@LMS.com";

            var password = CreateRandomPassword(user.UserName);

            var result = await UserManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return new System.Tuple<IdentityResult, string>(result, null);
            }

            result = await UserManager.AddToRoleAsync(user.Id, user.Role);

            if (result.Succeeded)
            {
                switch (user.Role)
                {
                    case "Student":
                        await studentRepo.AddAsync( new Student
                        {
                            UserId = user.Id,
                            User = user
                        });
                        break;
                    case "Teacher":
                        await teacherRepo.AddAsync(new Teacher
                        {
                            UserId = user.Id,
                            User = user
                        });
                        break;
                }
            }

            return new System.Tuple<IdentityResult, string>(result, password);
        }
        public async Task<IdentityResult> EditUser(User updated)
        {

            var user = await UserManager.FindByIdAsync(updated.Id);

            if (user.UserName == "Admin" && (updated.UserName != user.UserName || updated.Role != user.Role))
            {
                return null;
            }

            user.UserName = updated.UserName;
            user.Email = updated.Email;
            user.FirstName = updated.FirstName;
            user.LastName = updated.LastName;

            if(user.Role != updated.Role)
            {
                var result = await UserManager.RemoveFromRoleAsync(user.Id, user.Role);
                if (!result.Succeeded)
                {
                    return result;
                }

                result = await UserManager.AddToRoleAsync(user.Id, updated.Role);
                if (!result.Succeeded)
                {
                    return result;
                }

                await RemoveFromeRole(user);
                await AddToRole(user, updated.Role);
            }


            return await UserManager.UpdateAsync(user);
        }
        public async Task<IdentityResult> DeleteUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if(user == null)
            {
                return null;
            }

            var result = await RemoveFromeRole(user);
            if (!result)
            {

            }

            return await UserManager.DeleteAsync(user);
        }
        public async Task<IdentityResult> SetPassword(string userId, string password)
        {
            var result = await UserManager.RemovePasswordAsync(userId);
            if (!result.Succeeded)
            {
                return result;
            }

            return await UserManager.AddPasswordAsync(userId, password);
        }

        #endregion

        #region Roles

        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await RoleManager.Roles.ToListAsync();
        }
        public async Task<IdentityRole> GetRoleById(string id)
        {
            return await RoleManager.FindByIdAsync(id);
        }
        public async Task<IdentityResult> CreateRole(string name)
        {
            return await RoleManager.CreateAsync(new IdentityRole(name));
        }
        public async Task<IdentityResult> EditRole(IdentityRole updated)
        {
            var role = await RoleManager.FindByIdAsync(updated.Id);

            if (role.Name == "Admin" || role.Name == "Student" || role.Name == "Teacher")
            {
                return new IdentityResult("Cannot edit this Role!");
            }

            if (role == null)
            {
                return new IdentityResult("No Role found with the given Id!");
            }

            var users = await UserManager.Users.Where(u => u.Role == role.Name).ToListAsync();
            foreach(var user in users)
            {
                user.Role = updated.Name;
            }

            role.Name = updated.Name;

            return await RoleManager.UpdateAsync(role);
        }
        public async Task<IdentityResult> DeleteRole(string name)
        {
            var role = await RoleManager.FindByNameAsync(name);
            if(role.Users.Count > 0)
            {
                return new IdentityResult("Cannot delete a Role with active Users!");
            }
            return await RoleManager.DeleteAsync(role);
        }

        #endregion

        #region Courses

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await courseRepo.GetAllAsync();
        }
        public async Task<Course> GetCourseById(int id)
        {
            return await courseRepo.GetAsync(id);
        }
        public async Task<Course> GetCourseById(string id)
        {
            return await courseRepo.GetAsync(id);
        }
        public async Task CreateCourse(Course course)
        {
            await courseRepo.AddAsync(course);
        }
        public async Task EditCourse(Course course)
        {
            await courseRepo.UpdateAsync(course);
        }
        public async Task DeleteCourse(Course course)
        {
            await courseRepo.DeleteAsync(course);
        }

        #endregion

        #region Course Users

        public async Task<IEnumerable<Teacher>> GetTeachersForCourse(string courseId)
        {
            var course = await courseRepo.GetAsync(courseId);
            return course.Teachers.ToList();
        }
        public async Task<IEnumerable<Student>> GetStudentsForCourse(string courseId)
        {
            var course = await courseRepo.GetAsync(courseId);
            return course.Students.ToList();
        }

        public async Task AddUserToCourse(User user, string courseId)
        {

            var course = await courseRepo.GetAsync(courseId);
            if (course == null)
            {

            }

            if (user.Role == "Student")
            {
                var student = await studentRepo.GetAsync(s => s.UserId == user.Id);

                if (student == null)
                {

                }
                course.Students.Add(student);
                student.Courses.Add(course);
            }
            else if (user.Role == "Teacher")
            {
                var teacher = await teacherRepo.GetAsync(t => t.UserId == user.Id);

                if (teacher == null)
                {

                }
                course.Teachers.Add(teacher);
                teacher.Courses.Add(course);
            }
            else { return; }

            await courseRepo.SaveAsync();
        }
        public async Task RemoveUserFromCourse(User user, string courseId)
        {

            var course = await courseRepo.GetAsync(courseId);
            if (course == null)
            {

            }

            if (user.Role == "Student")
            {
                var student = await studentRepo.GetAsync(s => s.UserId == user.Id);

                if (student == null)
                {

                }
                course.Students.Remove(student);
                student.Courses.Remove(course);
            }
            else if(user.Role == "Teacher")
            {
                var teacher = await teacherRepo.GetAsync(t => t.UserId == user.Id);

                if (teacher == null)
                {

                }
                course.Teachers.Remove(teacher);
                teacher.Courses.Remove(course);
            }
            else { return;  }

            await courseRepo.SaveAsync();
        }


        #endregion

        #region Course Assignments

        public async Task<Assignment> GetAssignment(int id)
        {
            var repo = new GenericRepository<Assignment>();
            return await repo.GetAsync(id);
        }
        public async Task<IEnumerable<Assignment>> GetAssignmentsForCourse(string courseId)
        {
            var repo = new GenericRepository<Assignment>();
            var list = await repo.GetAllAsync();
            list = list.Where(a => a.CourseId == courseId);

            return list;
        }

        public async Task CreateAssignmentForCourse(Assignment assignment)
        {
            var date = DateTime.Now.Date;
            assignment.Deadline = date.AddMonths(1).ToString();
            var repo = new GenericRepository<Assignment>();
            await repo.AddAsync(assignment);
            var course = await courseRepo.GetAsync(assignment.CourseId);
            course.Assignments.Add(assignment);
            await courseRepo.SaveAsync();
        }
        public async Task DeleteAssignmentFromCourse(Assignment assignment)
        {
            var repo = new GenericRepository<Assignment>();
            assignment = await repo.GetAsync(assignment.Id);
            var course = await courseRepo.GetAsync(assignment.CourseId);
            course.Assignments.Remove(assignment);
            await repo.DeleteAsync(assignment);
        }
        public async Task EditAssignmentForCourse(Assignment assignment)
        {
            var repo = new GenericRepository<Assignment>();
            await repo.UpdateAsync(assignment);
        }



        #endregion

        // Helpers
        private string CreateUsername(string fName, string lName)
        {
            var date = System.DateTime.Now.Year.ToString();
            return fName.Substring(0, 3).ToLower() + lName.Substring(0, 3).ToLower() + "-" + (char)date[date.Length - 1];
        }
        private string CreateRandomPassword(string username)
        {
            username = username.Substring(0, username.Length - 2);
            var rng = new System.Random();
            var n = rng.Next(username.Length);
            var index = username.Length;
            var password = username.ToCharArray();

            while (index != 0)
            {
                n = rng.Next(index);
                index--;
                var tmp = password[index];
                password[index] = password[n];
                password[n] = tmp;
                var s = new string(password);

                if (index == 0 && new string(password) == username)
                {
                    index = password.Length;
                }
            }
            password[0] = password[0].ToString().ToUpper().ToCharArray()[0];
            var result = new string(password);
            result += "-";

            for (index = rng.Next(1, 9); index > 0; index--)
            {
                result += rng.Next(1, 9);
            }
            return result;
        }
        private async Task<bool> AddToRole(User user, string role)
        {
            switch (role)
            {
                case "Student":
                    var student = await studentRepo.GetAsync(s => s.UserId == user.Id);
                    if (student == null)
                    {
                        student = new Student { UserId = user.Id };
                    }
                    await studentRepo.AddAsync(student);
                    break;
                case "Teacher":
                    var teacher = await teacherRepo.GetAsync(s => s.UserId == user.Id);
                    if (teacher == null)
                    {
                        teacher = new Teacher { UserId = user.Id };
                    }
                    await teacherRepo.AddAsync(teacher);
                    break;
            }
            user.Role = role;
            return true;
        }
        private async Task<bool> RemoveFromeRole(User user)
        {
            switch (user.Role)
            {
                case "Student":
                    var student = await studentRepo.GetAsync(s => s.UserId == user.Id);
                    if (student == null)
                    {
                        return false;
                    }
                    await studentRepo.DeleteAsync(student.Id);
                    break;
                case "Teacher":
                    var teacher = await teacherRepo.GetAsync(s => s.UserId == user.Id);
                    if (teacher == null)
                    {
                        return false;
                    }
                    await teacherRepo.DeleteAsync(teacher.Id);
                    break;
            }
            return true;
        }

        #endregion

    }
}