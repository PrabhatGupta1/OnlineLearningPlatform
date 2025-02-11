using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OnlineLearningPlatform.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? ProfilePicPath { get; set; }

    public string? Bio { get; set; }

}

