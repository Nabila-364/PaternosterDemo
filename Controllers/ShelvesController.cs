using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using PaternosterDemo.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PaternosterDemo.Controllers
{
    public class ShelvesController : Controller
    {
        private readonly AppDbContext _context;
        public ShelvesController(AppDbContext context) => _context = context;

        // GET: Shelves
        public async Task<IActionResult> Index()
        {
            var shelves = await _context.Shelves.Include(s => s.Cabinet).ToListAsync();
            return View(shelves);
        }

        // GET: Shelves/Create
        public IActionResult Create()
        {
            ViewData["CabinetId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cabinets, "CabinetId", "CabinetNumber");
            return View();
        }

        // POST: Shelves/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Shelf shelf)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CabinetId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cabinets, "CabinetId", "CabinetNumber", shelf.CabinetId);
                return View(shelf);
            }

            _context.Add(shelf);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Shelves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var shelf = await _context.Shelves.FindAsync(id);
            if (shelf == null) return NotFound();

            ViewData["CabinetId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cabinets, "CabinetId", "CabinetNumber", shelf.CabinetId);
            return View(shelf);
        }

        // POST: Shelves/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Shelf shelf)
        {
            if (id != shelf.ShelfId) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["CabinetId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Cabinets, "CabinetId", "CabinetNumber", shelf.CabinetId);
                return View(shelf);
            }

            _context.Update(shelf);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Shelves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var shelf = await _context.Shelves.Include(s => s.Cabinet).FirstOrDefaultAsync(s => s.ShelfId == id);
            if (shelf == null) return NotFound();

            return View(shelf);
        }

        // POST: Shelves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shelf = await _context.Shelves.FindAsync(id);
            if (shelf != null)
            {
                _context.Shelves.Remove(shelf);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
