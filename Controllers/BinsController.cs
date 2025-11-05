using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using PaternosterDemo.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PaternosterDemo.Controllers
{
    public class BinsController : Controller
    {
        private readonly AppDbContext _context;

        public BinsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Bins
        public async Task<IActionResult> Index()
        {
            var bins = await _context.Bins
                                     .Include(b => b.Part)
                                     .Include(b => b.Shelf)
                                     .ThenInclude(s => s.Cabinet)
                                     .ToListAsync();
            return View(bins);
        }

        // GET: Bins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var bin = await _context.Bins
                                    .Include(b => b.Part)
                                    .Include(b => b.Shelf)
                                    .ThenInclude(s => s.Cabinet)
                                    .FirstOrDefaultAsync(b => b.BinId == id);

            if (bin == null) return NotFound();
            return View(bin);
        }

        // GET: Bins/Create
        public IActionResult Create()
        {
            ViewData["Parts"] = _context.Parts.ToList();
            ViewData["Shelves"] = _context.Shelves.Include(s => s.Cabinet).ToList();
            return View();
        }

        // POST: Bins/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bin bin)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Parts"] = _context.Parts.ToList();
                ViewData["Shelves"] = _context.Shelves.Include(s => s.Cabinet).ToList();
                return View(bin);
            }

            _context.Add(bin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Bins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var bin = await _context.Bins.FindAsync(id);
            if (bin == null) return NotFound();

            ViewData["Parts"] = _context.Parts.ToList();
            ViewData["Shelves"] = _context.Shelves.Include(s => s.Cabinet).ToList();
            return View(bin);
        }

        // POST: Bins/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Bin bin)
        {
            if (id != bin.BinId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["Parts"] = _context.Parts.ToList();
                ViewData["Shelves"] = _context.Shelves.Include(s => s.Cabinet).ToList();
                return View(bin);
            }

            _context.Update(bin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Bins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var bin = await _context.Bins
                                    .Include(b => b.Part)
                                    .Include(b => b.Shelf)
                                    .ThenInclude(s => s.Cabinet)
                                    .FirstOrDefaultAsync(b => b.BinId == id);

            if (bin == null) return NotFound();
            return View(bin);
        }

        // POST: Bins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bin = await _context.Bins.FindAsync(id);
            if (bin != null)
            {
                _context.Bins.Remove(bin);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
