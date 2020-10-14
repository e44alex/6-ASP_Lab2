using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StudentSubjectAttendancesController : Controller
    {
        private readonly AppDBContext _context;

        public StudentSubjectAttendancesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: StudentSubjectAttendances
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Attendances.Include(s => s.Student).Include(s => s.Subject);
            return View(await appDBContext.ToListAsync());
        }

        // GET: StudentSubjectAttendances/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentSubjectAttendance = await _context.Attendances
                .Include(s => s.Student)
                .Include(s => s.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentSubjectAttendance == null)
            {
                return NotFound();
            }

            return View(studentSubjectAttendance);
        }

        // GET: StudentSubjectAttendances/Create
        public IActionResult Create()
        {

            ViewData["StudentId"] = new SelectList(_context.Students, "Name", "Name");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Name", "Name");
            return View();
        }

        // POST: StudentSubjectAttendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubjectName,StudentName,CountMissed")] StudentSubjectAttendance studentSubjectAttendance)
        {

            if (ModelState.IsValid)
            {
                studentSubjectAttendance.Id = Guid.NewGuid();
                studentSubjectAttendance.Student = _context.Students.FirstOrDefault(x=> x.Name == studentSubjectAttendance.StudentName);
                studentSubjectAttendance.Subject = _context.Subjects.FirstOrDefault(x=> x.Name == studentSubjectAttendance.SubjectName);
                studentSubjectAttendance.StudentId = studentSubjectAttendance.Student.Id;
                studentSubjectAttendance.SubjectId = studentSubjectAttendance.Subject.Id;
                _context.Add(studentSubjectAttendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", studentSubjectAttendance.StudentId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id", studentSubjectAttendance.SubjectId);
            return View(studentSubjectAttendance);
        }

        // GET: StudentSubjectAttendances/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var appDBContext = _context.Attendances.Include(s => s.Student).Include(s => s.Subject);

            var studentSubjectAttendance = await appDBContext.FirstOrDefaultAsync(x=> x.Id == id);
            if (studentSubjectAttendance == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "Name", "Name", studentSubjectAttendance.Student.Name);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Name", "Name", studentSubjectAttendance.Subject.Name);
            return View(studentSubjectAttendance);
        }

        // POST: StudentSubjectAttendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,SubjectId,StudentId,CountMissed")] StudentSubjectAttendance studentSubjectAttendance)
        {
            if (id != studentSubjectAttendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentSubjectAttendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentSubjectAttendanceExists(studentSubjectAttendance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", studentSubjectAttendance.StudentId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id", studentSubjectAttendance.SubjectId);
            return View(studentSubjectAttendance);
        }

        // GET: StudentSubjectAttendances/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentSubjectAttendance = await _context.Attendances
                .Include(s => s.Student)
                .Include(s => s.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentSubjectAttendance == null)
            {
                return NotFound();
            }

            return View(studentSubjectAttendance);
        }

        // POST: StudentSubjectAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var studentSubjectAttendance = await _context.Attendances.FindAsync(id);
            _context.Attendances.Remove(studentSubjectAttendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet, ActionName("Filter")]
        public async Task<IActionResult> Filter(string text)
        {
            if (text == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var result =
                _context.Attendances
                    .Include(s => s.Student)
                    .Include(s => s.Subject)
                    .Where(x => x.Student.Name.Contains(text) || x.Subject.Name.Contains(text));

            return View("Index", await result.ToListAsync());
        }

        private bool StudentSubjectAttendanceExists(Guid id)
        {
            return _context.Attendances.Any(e => e.Id == id);
        }
    }
}
