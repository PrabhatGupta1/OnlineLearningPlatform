using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineLearningPlatform.Areas.Identity.Data;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Utility;
using System;

namespace OnlineLearningPlatform.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public CourseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.CourseType = "Explore Courses";
            return View(_context.Courses.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> MyLearnings(string id)
        {
            List<Course> EnrolledCourses = new List<Course>();
            var enrollments = _context.Enrollments.Where(x => x.UserID.Equals(id)).ToList();
            if (enrollments != null)
            {
                foreach (var item in enrollments)
                {
                    var enrolledCourse = await _context.Courses.FindAsync(item.CourseID);
                    if (enrolledCourse != null)
                    {
                        EnrolledCourses.Add(enrolledCourse);
                    }
                }
            }
            ViewBag.CourseType = "My Learnings";
            return View("Index", EnrolledCourses);
        }

        public void BindCategoriesWithModel()
        {
            CourseViewModel.CategoryList = new List<SelectListItem>();
            var categories = _context.Categories.ToList();

            foreach (var category in categories)
            {
                if (category != null)
                {
                    var categoryItem = new SelectListItem()
                    {
                        Text = category.CategoryName,
                        Value = category.CategoryID.ToString(),
                    };

                    CourseViewModel.CategoryList.Add(categoryItem);
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRoles.Role_Instructor + ", " + StaticUserRoles.Role_Admin)]
        public IActionResult Create()
        {
            BindCategoriesWithModel();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRoles.Role_Instructor + ", " + StaticUserRoles.Role_Admin)]
        public async Task<IActionResult> Create(CourseViewModel course)
        {
            Course newCourse = new Course();
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (course.Thumbnail != null)
                {
                    var folder = Path.Combine(_environment.WebRootPath, "Images/CourseThumbnails");
                    var fileName = Guid.NewGuid().ToString() + "_" + course.Thumbnail.FileName;
                    var filePath = Path.Combine(folder, fileName);
                    course.Thumbnail.CopyTo(new FileStream(filePath, FileMode.Create));

                    newCourse.ThumbnailPath = fileName;

                }

                newCourse.Title = course.Title;
                newCourse.Description = course.Description;
                newCourse.InstructorId = user.Id;
                newCourse.Price = course.Price;
                newCourse.CategoryId = course.CategoryId;
                newCourse.Language = (Course.Languages)course.Language;
                newCourse.Level = (Course.Levels)course.Level;
                newCourse.CreatedAt = DateTime.Now;

                await _context.Courses.AddAsync(newCourse);
                await _context.SaveChangesAsync();
                TempData["Create_Success"] = "Course Created Successfully";
                return RedirectToAction("Index");

            }
            return View();
        }

        public bool IsStudentEnrolled(string userId, int courseId)
        {
            var student = _context.Enrollments.FirstOrDefault(x => x.UserID.Equals(userId) && x.CourseID == courseId);
            if (student != null)
            {
                return true;
            }
            return false;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var course = await _context.Courses.FindAsync(Id);

            if (course != null)
            {
                ApplicationUser instructor = await _context.AppUsers.FindAsync(course.InstructorId);
                if (instructor != null)
                {
                    course.InstructorId = instructor.FullName;
                }
                if (user != null)
                {
                    ViewBag.isEnrolled = IsStudentEnrolled(user.Id, course.CourseId);
                }
            }

            return View(course);
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRoles.Role_Instructor + ", " + StaticUserRoles.Role_Admin)]
        public async Task<IActionResult> Edit(int Id)
        {
            var course = await _context.Courses.FindAsync(Id);
            if (course != null)
            {
                CourseViewModel newCourse = new CourseViewModel()
                {
                    Title = course.Title,
                    Description = course.Description,
                    Price = course.Price,
                    InstructorId = course.InstructorId,
                    CategoryId = course.CategoryId,
                    Language = (CourseViewModel.Languages)course.Language,
                    Level = (CourseViewModel.Levels)course.Level,
                    ThumbnailPath = course.ThumbnailPath,
                    CreatedAt = course.CreatedAt,
                };
                BindCategoriesWithModel();
                return View(newCourse);
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRoles.Role_Instructor + ", " + StaticUserRoles.Role_Admin)]
        public async Task<IActionResult> Edit(int Id, CourseViewModel course)
        {
            var courseToUpdate = await _context.Courses.FindAsync(Id);

            if (courseToUpdate != null)
            {
                if (course.Thumbnail != null)
                {
                    var folder = Path.Combine(_environment.WebRootPath, "Images/CourseThumbnails");
                    var fileName = Guid.NewGuid().ToString() + "_" + course.Thumbnail.FileName;
                    var filePath = Path.Combine(folder, fileName);
                    course.Thumbnail.CopyTo(new FileStream(filePath, FileMode.Create));

                    courseToUpdate.ThumbnailPath = fileName;

                }
                courseToUpdate.Title = course.Title;
                courseToUpdate.Description = course.Description;
                courseToUpdate.Price = course.Price;
                courseToUpdate.CategoryId = course.CategoryId;
                courseToUpdate.Language = (Course.Languages)course.Language;
                courseToUpdate.Level = (Course.Levels)course.Level;
                courseToUpdate.CreatedAt = DateTime.Now;

                _context.Courses.Update(courseToUpdate);
                await _context.SaveChangesAsync();
                TempData["Update_Success"] = "Course Updated Successfully";
                return RedirectToAction("Index");
            }

            return View(course);
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRoles.Role_Instructor + ", " + StaticUserRoles.Role_Admin)]
        public async Task<IActionResult> Delete(int Id)
        {
            var course = await _context.Courses.FindAsync(Id);

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = StaticUserRoles.Role_Instructor + ", " + StaticUserRoles.Role_Admin)]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var course = await _context.Courses.FindAsync(Id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                TempData["Delete_Success"] = "Course Deleted Successfully";
                return RedirectToAction("Index");
            }
            return View(course);
        }


    }
}
