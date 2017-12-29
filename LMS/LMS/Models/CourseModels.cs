using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class Course
    {
        [Key, Display(Name = "Id")]
        public string Id { get; set; }
        [Required, Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        //public virtual ICollection<Lecture> Lectures { get; set; }


        public Course()
        {
            Teachers = new List<Teacher>();
            Students = new List<Student>();
            Assignments = new List<Assignment>();
        }
    }

    public class CourseListVM
    {
        [Required, Display(Name = "Id")]
        public string Id { get; set; }
        [Required, Display(Name = "Name")]
        public string Name { get; set; }

        public static implicit operator CourseListVM(Course course)
        {
            return new CourseListVM
            {
                Id = course.Id,
                Name = course.Name
            };
        }
        public static implicit operator Course(CourseListVM model)
        {
            return new Course
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }

    //public class Grade
    //{
    //    [Key, Required]
    //    public int Id { get; set; }
    //    [Required]
    //    public string Date { get; set; }
    //    [Required]
    //    public int CourseID { get; set; }
    //    [Required, ForeignKey("CourseID")]
    //    public virtual Course Course { get; set; }
    //    [Required]
    //    public string TeacherId { get; set; }
    //    [Required, ForeignKey("TeacherId")]
    //    public virtual User Teacher { get; set; }
    //    [Required]
    //    public string StudentId { get; set; }
    //    [Required, ForeignKey("StudentId")]
    //    public virtual User Student { get; set; }
    //}
}