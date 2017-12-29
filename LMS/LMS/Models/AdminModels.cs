using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class UserDetailsVM
    {
        [Required, Display(Name = "Id")]
        public string Id { get; set; }
        [Required, Display(Name = "Username")]
        public string UserName { get; set; }
        [Required, Display(Name = "Email")]
        public string Email { get; set; }
        [Required, Display(Name = "Role")]
        public string Role { get; set; }
        [Required, Display(Name = "FistName")]
        public string FirstName { get; set; }
        [Required, Display(Name = "LastName")]
        public string LastName { get; set; }
        [Display(Name = "Courses")]
        public List<CourseListVM> Courses { get; set; }


        public static implicit operator UserDetailsVM(User user)
        {
            return new UserDetailsVM
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role
            };
        }
        public static implicit operator User(UserDetailsVM model)
        {
            return new User
            {
                Id = model.Id,
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = model.Role
            };
        }
    }
    public class RoleDetailsVM
    {
        [Required, Display(Name = "Id")]
        public string Id { get; set; }
        [Required, Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Users")]
        public List<UserListVM> Users { get; set; }

        public static implicit operator RoleDetailsVM(IdentityRole role)
        {
            return new RoleDetailsVM
            {
                Id = role.Id,
                Name = role.Name
            };
        }
        public static implicit operator IdentityRole(RoleDetailsVM model)
        {
            return new IdentityRole
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        //
    }
    public class CourseDetailsVM
    {
        [Required, Display(Name = "Id")]
        public string Id { get; set; }
        [Required, Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Teachers")]
        public List<TeacherListVM> Teachers { get; set; }
        [Display(Name = "Students")]
        public List<StudentListVM> Students { get; set; }

        public static implicit operator CourseDetailsVM(Course course)
        {
            List<TeacherListVM> teachers = new List<TeacherListVM>();
            foreach(var teacher in course.Teachers)
            {
                teachers.Add(teacher);
            }

            List<StudentListVM> students = new List<StudentListVM>();
            foreach (var student in course.Students)
            {
                students.Add(student);
            }


            return new CourseDetailsVM
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Teachers = teachers,
                Students = students
            };
        }
        public static implicit operator Course(CourseDetailsVM model)
        {
            return new Course
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };
        }
    }
    public class AssignmentDetailsVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CourseId { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public string Deadline { get; set; }
        //List<File> Files = new List<File>();

        public static implicit operator AssignmentDetailsVM(Assignment assignment)
        {
            return new AssignmentDetailsVM
            {
                Id = assignment.Id,
                Name = assignment.Name,
                Description = assignment.Description,
                CourseId = assignment.CourseId,
                Deadline = assignment.Deadline
            };
        }
        public static implicit operator Assignment(AssignmentDetailsVM model)
        {
            return new Assignment
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                CourseId = model.CourseId,
                Deadline = model.Deadline
            };
        }
    }

    public class RoleListVM
    {
        [Required, Display(Name = "Id")]
        public string Id { get; set; }
        [Required, Display(Name = "Name")]
        public string Name { get; set; }

        public static implicit operator RoleListVM(IdentityRole role)
        {
            return new RoleListVM
            {
                Id = role.Id,
                Name = role.Name
            };
        }
        public static implicit operator IdentityRole(RoleListVM model)
        {
            return new IdentityRole
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }

    public class AdminCreateUserBindingModel
    {
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, Display(Name = "Role")]
        public string Role { get; set; }

        public static implicit operator AdminCreateUserBindingModel(User user)
        {
            return new AdminCreateUserBindingModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role
            };
        }
        public static implicit operator User(AdminCreateUserBindingModel model)
        {
            return new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = model.Role
            };
        }
    }
    public class AdminCreateRoleBindingModel
    {
        [Required, Display(Name = "Name")]
        public string Name { get; set; }
    }

    public class UserCourseVM
    {
        [Required]
        public UserListVM User { get; set; }
        [Required]
        public CourseDetailsVM Course { get; set; }
    }

    public class SeUsertPasswordBindingModel
    {
        public UserListVM User { get; set; }
        public SetPasswordBindingModel Passwords { get; set; }
    }
}