using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Areas.Identity.Data;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> AppUsers { get; set; }
    public DbSet<Categories> Categories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        builder.ApplyConfiguration(new CategoriesEntityConfiguration());
        builder.ApplyConfiguration(new CourseEntityConfiguration());
        builder.ApplyConfiguration(new LessonEntityConfiguration());
        builder.ApplyConfiguration(new EnrollmentEntityConfiguration());
    }

//public DbSet<OnlineLearningPlatform.Models.ApplicationUserViewModel> ApplicationUserViewModel { get; set; } = default!;
}



internal class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.FullName).HasMaxLength(255);
        builder.Property(x => x.CreatedAt);
        builder.Property(x=> x.ProfilePicPath).HasMaxLength(255);
        builder.Property(x => x.Bio).HasMaxLength(500);
    }
}

internal class CategoriesEntityConfiguration : IEntityTypeConfiguration<Categories>
{
    public void Configure(EntityTypeBuilder<Categories> builder)
    {
        builder.Property(x => x.CategoryID);
        builder.Property(x => x.CategoryName).HasMaxLength(255);
        builder.Property(x => x.Description).HasMaxLength(500);
    }
}

internal class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property(x => x.CategoryId);
        builder.Property(x => x.Title).HasMaxLength(255);
        builder.Property(x => x.Description).HasMaxLength(255);
        builder.Property(x => x.InstructorId);
        builder.Property(x => x.Price);
        builder.Property(x => x.CategoryId);
        builder.Property(x => x.Language);
        builder.Property(x => x.Level);
        builder.Property(x => x.ThumbnailPath).HasMaxLength(255);
        builder.Property(x => x.CreatedAt);
    }
}

internal class LessonEntityConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.Property(x => x.LessonID);
        builder.Property(x => x.CourseID);
        builder.Property(x => x.Title).HasMaxLength(255);
        builder.Property(x => x.VideoURL).HasMaxLength(255);
        builder.Property(x => x.Duration);
        builder.Property(x => x.OrderIndex);
        builder.Property(x => x.CreatedAt);
    }
}

internal class EnrollmentEntityConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.Property(x => x.EnrollmentID);
        builder.Property(x => x.UserID).HasMaxLength(255);
        builder.Property(x => x.CourseID);
        builder.Property(x => x.EnrolledAt);
        builder.Property(x => x.Status);
    }
}