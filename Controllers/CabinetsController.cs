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

        public async Task<IActionResult> Index()
        {
            var cabinets = await _context.Cabinets.Include(c => c.Inventories).ToListAsync();
            return View(cabinets);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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
    }
}
