using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using PaternosterDemo.Models;

namespace PaternosterDemo.Controllers
{
    public class CabinetsController : Controller
    {
        private readonly AppDbContext _context;

        public CabinetsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Cabinets
        public async Task<IActionResult> Index()
        {
            var cabinets = await _context.Cabinets.ToListAsync();
            return View(cabinets);
        }

        // GET: Cabinets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cabinets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cabinet cabinet)
        {
            if (ModelState.IsValid)
            {
                _context.Cabinets.Add(cabinet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cabinet);
        }

        // GET: Cabinets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
                    if (!_context.Cabinets.Any(c => c.CabinetId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cabinet);
        }

        // GET: Cabinets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cabinet = await _context.Cabinets
                                        .FirstOrDefaultAsync(c => c.CabinetId == id);
            if (cabinet == null) return NotFound();

            return View(cabinet);
        }

        // POST: Cabinets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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
