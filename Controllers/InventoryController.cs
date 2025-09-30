using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using PaternosterDemo.Models;

namespace PaternosterDemo.Controllers
{
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;

        public InventoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            var inventories = await _context.Inventories
                                           .Include(i => i.Part)
                                           .Include(i => i.Cabinet)
                                           .ToListAsync();
            return View(inventories);
        }

        // GET: Inventory/Create
        public IActionResult Create()
        {
            var parts = _context.Parts.ToList();
            var cabinets = _context.Cabinets.ToList();

            // Debug output
            Console.WriteLine($"Parts count: {parts.Count}, Cabinets count: {cabinets.Count}");

            ViewData["Parts"] = new SelectList(parts, "PartId", "Name");
            ViewData["Cabinets"] = new SelectList(cabinets, "CabinetId", "CabinetNumber");

            return View();
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Inventories.Add(inventory);
                await _context.SaveChangesAsync();

                // Debug: bevestiging
                Console.WriteLine($"Inventory toegevoegd: PartId={inventory.PartId}, CabinetId={inventory.CabinetId}, Quantity={inventory.Quantity}");

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Log ModelState errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState fout: {error.ErrorMessage}");
                }
            }

            // Herlaad dropdowns
            ViewData["Parts"] = new SelectList(_context.Parts.ToList(), "PartId", "Name");
            ViewData["Cabinets"] = new SelectList(_context.Cabinets.ToList(), "CabinetId", "CabinetNumber");
            return View(inventory);
        }
    }
}
