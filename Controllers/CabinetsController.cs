using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using PaternosterDemo.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PaternosterDemo.Controllers
{
    public class CabinetsController : Controller
    {
        private readonly AppDbContext _context;
        public CabinetsController(AppDbContext context) => _context = context;

        // GET: Cabinets
        public async Task<IActionResult> Index()
        {
            ViewBag.IsAdmin = HttpContext.Session.GetString("Role") == "Admin";
            return View(await _context.Cabinets.ToListAsync());
        }

        // GET: Cabinets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cabinet = await _context.Cabinets.FirstOrDefaultAsync(c => c.CabinetId == id);
            if (cabinet == null) return NotFound();
            return View(cabinet);
        }

        // GET: Cabinets/Create (Admin only)
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("AccessDenied", "Account");
            return View();
        }

        // POST: Cabinets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cabinet cabinet)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid)
            {
                _context.Cabinets.Add(cabinet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cabinet);
        }

        // GET: Cabinets/Edit/5 (Admin only)
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("AccessDenied", "Account");

            if (id == null) return NotFound();
            var cabinet = await _context.Cabinets.FindAsync(id);
            if (cabinet == null) return NotFound();
            return View(cabinet);
        }

        // POST: Cabinets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cabinet cabinet)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("AccessDenied", "Account");

            if (id != cabinet.CabinetId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cabinet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Cabinets.Any(c => c.CabinetId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cabinet);
        }

        // GET: Cabinets/Delete/5 (Admin only)
        public async Task<IActionResult> Delete(int? id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("AccessDenied", "Account");

            if (id == null) return NotFound();
            var cabinet = await _context.Cabinets.FirstOrDefaultAsync(c => c.CabinetId == id);
            if (cabinet == null) return NotFound();
            return View(cabinet);
        }

        // POST: Cabinets/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("AccessDenied", "Account");

            var cabinet = await _context.Cabinets.FindAsync(id);
            if (cabinet != null)
            {
                _context.Cabinets.Remove(cabinet);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
