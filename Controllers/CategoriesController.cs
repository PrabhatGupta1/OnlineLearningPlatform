using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Areas.Identity.Data;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Utility;

namespace OnlineLearningPlatform.Controllers
{
    [Authorize(Roles = StaticUserRoles.Role_Admin)]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
           return View(_context.Categories.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Categories catagory)
        {
            if (ModelState.IsValid) 
            {
                _context.Categories.Add(catagory);
                _context.SaveChanges();
                TempData["Create_Success"] = "Category Created Successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            Categories category = await _context.Categories.FindAsync(Id);
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int Id, Categories category)
        {
            if (Id == category.CategoryID)
            {
                if (ModelState.IsValid)
                {
                    _context.Categories.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["Update_Success"] = "Category Updated Successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            Categories category = await _context.Categories.FindAsync(Id);
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            Categories category = await _context.Categories.FindAsync(Id);
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            Categories category = await _context.Categories.FindAsync(Id);
            if (category != null) { 
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Delete_Success"] = "Category Deleted Successfully.";
                return RedirectToAction("Index");
            }
            return View(category);
        }


    }
}
