using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class DashboardVM
    {
        public List<SubmittedAssignment> UngradedAssignments { get; set; }
        public List<SubmittedAssignment> GradedAssignments { get; set; }
        public List<Assignment> AssignmentsToSubmit { get; set; }

        public DashboardVM()
        {
            UngradedAssignments = new List<SubmittedAssignment>();
            GradedAssignments = new List<SubmittedAssignment>();
            AssignmentsToSubmit = new List<Assignment>();
        }
    }
}