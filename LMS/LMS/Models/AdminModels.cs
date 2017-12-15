using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class AdminUserListViewModel
    {
        [Key]
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
    }
    public class AdminRoleListViewModel
    {
        [Required, Display(Name = "Id")]
        public string Id { get; set; }
        [Required, Display(Name = "Name")]
        public string Name { get; set; }
    }

    public class AdminCreateUserBindingModel
    {
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, Display(Name = "Role")]
        public string Role { get; set; }
    }
    public class AdminCreateRoleBindingModel
    {
        [Required, Display(Name = "Name")]
        public string Name { get; set; }
    }
}