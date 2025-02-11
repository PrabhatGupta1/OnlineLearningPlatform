using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace OnlineLearningPlatform.Models
{
    public class Lesson
    {
        [Key]
        public int LessonID { get; set; }

        [ForeignKey("Course")]
        [Required]
        public int CourseID { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? VideoURL { get; set; }

        public int Duration { get; set; }

        [Required]
        public int OrderIndex { get; set; }

        public DateTime CreatedAt { get; set; }
    }



    //CREATE TABLE Lessons(
    //LessonID INT PRIMARY KEY IDENTITY(1,1),
    //CourseID INT NOT NULL,
    //Title NVARCHAR(255) NOT NULL,
    //VideoURL NVARCHAR(255),
    //Duration INT, -- in minutes
    //OrderIndex INT NOT NULL,
    //CreatedAt DATETIME DEFAULT GETDATE(),
    //FOREIGN KEY(CourseID) REFERENCES Courses(CourseID) ON DELETE CASCADE
    //);
}
