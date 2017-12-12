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
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}