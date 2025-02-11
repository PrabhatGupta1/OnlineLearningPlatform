namespace OnlineLearningPlatform.Models
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? ProfilePicPath { get; set; }

        public IFormFile? ProfilePicture { get; set; }

        public string? Bio { get; set; }

    }
}
