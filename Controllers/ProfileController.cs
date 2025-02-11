using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Areas.Identity.Data;
using OnlineLearningPlatform.Models;


namespace OnlineLearningPlatform.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ApplicationUserViewModel()
            {
                Id = user.Id,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt,
                ProfilePicPath = user.ProfilePicPath,
                Bio = user.Bio
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ApplicationUserViewModel()
            {
                Id = user.Id,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt,
                ProfilePicPath = user.ProfilePicPath,
                Bio = user.Bio
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(ApplicationUserViewModel? model)
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                if(model.ProfilePicture != null) 
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "Images/ProfilePictures/");
                    string fileName = Guid.NewGuid().ToString() + "_" + model.ProfilePicture.FileName;
                    string filePath = Path.Combine(folder, fileName);
                    model.ProfilePicture.CopyTo(new FileStream(filePath, FileMode.Create));

                    user.ProfilePicPath = fileName;
                }
                user.FullName = model.FullName;
                user.Bio = model.Bio;
                
                await _userManager.UpdateAsync(user);
                return RedirectToAction("MyProfile", "Profile");
            }
            return View(model);
        }
    }
}
