using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        [Required, Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "First Last")]
        public string LastName { get; set; }
        [Required, Display(Name = "First Role")]
        public string Role { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class UserListVM
    {
        [Required, Display(Name = "Id")]
        public string Id { get; set; }
        [Required, Display(Name = "Username")]
        public string UserName { get; set; }
        [Required, Display(Name = "Role")]
        public string Role { get; set; }

        public static implicit operator UserListVM(User user)
        {
            return new UserListVM
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.Role
            };
        }
        public static implicit operator User(UserListVM model)
        {
            return new User
            {
                Id = model.Id,
                UserName = model.UserName,
                Role = model.Role
            };
        }
    }

    public class Student
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required, ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public Student()
        {
            Courses = new List<Course>();
        }
    }
    public class StudentListVM
    {
        [Key, Required]
        public int Id { get; set; }
        [Required, Display(Name = "Username")]
        public string UserName { get; set; }

        public static implicit operator StudentListVM(Student student)
        {
            return new StudentListVM
            {
                Id = student.Id,
                UserName = student.User.UserName
            };
        }
        public static implicit operator Student(StudentListVM model)
        {
            return new Student
            {
                Id = model.Id
            };
        }
    }

    public class Teacher
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required, ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public Teacher()
        {
            Courses = new List<Course>();
        }
    }
    public class TeacherListVM
    {
        [Key, Required]
        public int Id { get; set; }
        [Required, Display(Name = "Username")]
        public string UserName { get; set; }

        public static implicit operator TeacherListVM(Teacher teacher)
        {
            return new TeacherListVM
            {
                Id = teacher.Id,
                UserName = teacher.User.UserName
            };
        }
        public static implicit operator Teacher(TeacherListVM model)
        {
            return new Teacher
            {
                Id = model.Id
            };
        }
    }


}