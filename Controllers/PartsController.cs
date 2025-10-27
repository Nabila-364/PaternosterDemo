using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using PaternosterDemo.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PaternosterDemo.Controllers
{
    public class PartsController : Controller
    {
        private readonly AppDbContext _context;
        public PartsController(AppDbContext context) => _context = context;

        // GET: Parts
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");
            ViewBag.IsAdmin = HttpContext.Session.GetString("Role") == "Admin";

            return View(await _context.Parts.ToListAsync());
        }

        // GET: Parts/Create
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");
            return View();
        }

        // POST: Parts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Part part)
        {
            if (!ModelState.IsValid) return View(part);

            _context.Add(part);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Parts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            if (id == null) return NotFound();

            var part = await _context.Parts.FindAsync(id);
            if (part == null) return NotFound();

            return View(part);
        }

        // POST: Parts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Part part)
        {
            if (id != part.PartId) return NotFound();
            if (!ModelState.IsValid) return View(part);

            _context.Update(part);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Parts/Delete/5 (Admin only)
        public async Task<IActionResult> Delete(int? id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("AccessDenied", "Account");

            if (id == null) return NotFound();

            var part = await _context.Parts.FirstOrDefaultAsync(p => p.PartId == id);
            if (part == null) return NotFound();

            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("AccessDenied", "Account");

            var part = await _context.Parts.FindAsync(id);
            if (part != null)
            {
                _context.Parts.Remove(part);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
