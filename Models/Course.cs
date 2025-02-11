using OnlineLearningPlatform.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineLearningPlatform.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        [ForeignKey("AspNetUsers")] 
        public string InstructorId { get; set; }

        [Required]
        public double Price { get; set; }

        [ForeignKey("Categories")]
        public int CategoryId { get; set; }

        [Required]
        public Languages Language { get; set; }

        public Levels Level { get; set; }

        public string? ThumbnailPath { get; set; }

        public DateTime CreatedAt { get; set; }

        public enum Languages {
            English,
            Hindi,
            Urdu
        }
        public enum Levels
        {
            Beginner,
            Intermediate,
            Advanced
        }


    }
}
