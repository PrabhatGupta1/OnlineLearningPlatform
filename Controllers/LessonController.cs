using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Areas.Identity.Data;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Utility;

namespace OnlineLearningPlatform.Controllers
{

    public class LessonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LessonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lesson
        [Authorize(Roles = StaticUserRoles.Role_Admin)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lessons.ToListAsync());
        }

        public async Task<IActionResult> CourseLesson(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var courseLessons = _context.Lessons.Where(x => x.CourseID == id).ToList();
            if (courseLessons == null)
            {
                return NotFound();
            }
            ViewBag.CourseID = id;
            return View(courseLessons);
        }

        // GET: Lesson/Details/5
        [Authorize(Roles = StaticUserRoles.Role_Instructor)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(m => m.LessonID == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // GET: Lesson/Create/CourseId
        [Authorize(Roles = StaticUserRoles.Role_Instructor)]
        public IActionResult Create(int Id)
        {
            TempData["CourseId"] = Id;
            return View();
        }

        // POST: Lesson/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = StaticUserRoles.Role_Instructor)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LessonID,CourseID,Title,VideoURL,Duration,OrderIndex,CreatedAt")] Lesson lesson)
        {
            
            if (ModelState.IsValid)
            {
                lesson.CreatedAt = DateTime.Now;
                await _context.AddAsync(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction("CourseLesson", "Lesson", new { id = lesson.CourseID });
                
            }
            return View(lesson);
        }

        // GET: Lesson/Edit/5
        [Authorize(Roles = StaticUserRoles.Role_Instructor)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // POST: Lesson/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = StaticUserRoles.Role_Instructor)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LessonID,CourseID,Title,VideoURL,Duration,OrderIndex,CreatedAt")] Lesson lesson)
        {
            if (id != lesson.LessonID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    lesson.CreatedAt = DateTime.Now;
                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(lesson.LessonID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("CourseLesson", "Lesson", new { Id = lesson.CourseID });
            }
            return View(lesson);
        }

        // GET: Lesson/Delete/5
        [Authorize(Roles = StaticUserRoles.Role_Instructor)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(m => m.LessonID == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // POST: Lesson/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = StaticUserRoles.Role_Instructor)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            var CourseId = 0;
            if (lesson != null)
            {
                CourseId = lesson.CourseID;
                _context.Lessons.Remove(lesson);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("CourseLesson", "Lesson", new { Id = CourseId });
        }

        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.LessonID == id);
        }
    }
}
