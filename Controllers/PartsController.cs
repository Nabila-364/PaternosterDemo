using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using PaternosterDemo.Models;

namespace PaternosterDemo.Controllers
{
    public class PartsController : Controller
    {
        private readonly AppDbContext _context;

        public PartsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Parts
        public async Task<IActionResult> Index()
        {
            var parts = await _context.Parts
                                      .Include(p => p.Inventories)
                                      .ToListAsync();
            return View(parts);
        }

        // GET: Parts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Part part)
        {
            if (ModelState.IsValid)
            {
                _context.Parts.Add(part);          // voeg toe
                await _context.SaveChangesAsync(); // sla op
                return RedirectToAction(nameof(Index));
            }
            return View(part);
        }
    }
}
