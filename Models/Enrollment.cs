using Humanizer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace OnlineLearningPlatform.Models
{
    public class Enrollment
    {
        [Key]
        [Required]
        public int EnrollmentID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required] 
        public int CourseID { get; set; }
        
        [Required]
        public DateTime EnrolledAt { get; set; }
        
        public CourseStatus? Status { get; set; }


        public enum CourseStatus { 
            Active,
            Completed
        }

    }
}
