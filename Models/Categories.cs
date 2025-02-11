using System.ComponentModel.DataAnnotations;

namespace OnlineLearningPlatform.Models
{
    public class Categories
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
