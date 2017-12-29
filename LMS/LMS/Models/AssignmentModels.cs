using System.Web.Mvc;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Assignment
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Deadline { get; set; }
        public string Description { get; set; }
        [Required]
        public string CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course {get; set; }

        //public virtual ICollection<File> Files;
    }

    public class AssignmentListVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CourseId { get; set; }
        public string Deadline { get; set; }
    }

    public class SubmittedAssignment
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public Grade Grade { get; set; }
        [Required]
        public string Answer { get; set; }
        public bool Show { get; set; }

        [Required]
        public int AssignmentId { get; set; }
        [ForeignKey("AssignmentId")]
        public virtual Assignment Assignment { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
    public class SubmitAssignmentVM
    {
        [Required]
        public int AssignmentId { get; set; }
        [Required]
        public string Answer { get; set; }
        // add more stuff later
    }
    public class GradeAssignmentVM
    {
        [Required, Display(Name ="SubmittedAssignmentId")]
        public int Id { get; set; }
        [Required]
        public string Answer { get; set; }
        [Required]
        public Grade Grade { get; set; }

        public IEnumerable<SelectListItem> Grades { get; set; }

        public GradeAssignmentVM()
        {
            Grades = new SelectList(Enum.GetValues(typeof(Grade)), "U");
        }
        // add more stuff later
    }

    public enum Grade
    {
        F, E, D, C, B, A, U
    }
}