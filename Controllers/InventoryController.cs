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

            ViewBag.IsAdmin = HttpContext.Session.GetString("Role") == "Admin";

            return View(inventories);
        }

        // GET: Inventory/Create
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(inventory);
            }

            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Inventory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null) return NotFound();

            LoadDropdowns();
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inventory inventory)
        {
            if (id != inventory.InventoryId) return NotFound();
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(inventory);
            }

            var original = await _context.Inventories.AsNoTracking().FirstOrDefaultAsync(i => i.InventoryId == id);
            if (original == null) return NotFound();

            int diff = inventory.Quantity - original.Quantity;

            _context.Update(inventory);

            if (diff != 0)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId.HasValue)
                {
                    _context.Transactions.Add(new Transaction
                    {
                        InventoryId = inventory.InventoryId,
                        UserId = userId.Value,
                        QuantityChanged = diff,
                        Timestamp = DateTime.Now
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Inventory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var inventory = await _context.Inventories
                                          .Include(i => i.Part)
                                          .Include(i => i.Cabinet)
                                          .FirstOrDefaultAsync(i => i.InventoryId == id);
            if (inventory == null) return NotFound();

            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns()
        {
            ViewData["Parts"] = new SelectList(_context.Parts.ToList(), "PartId", "Name");
            ViewData["Cabinets"] = new SelectList(_context.Cabinets.ToList(), "CabinetId", "CabinetNumber");
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.InventoryId == id);
        }
    }
}
