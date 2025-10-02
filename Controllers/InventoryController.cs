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
            ViewBag.IsAdmin = HttpContext.Session.GetString("Role") == "Admin";

            var inventories = await _context.Inventories
                                           .Include(i => i.Part)
                                           .Include(i => i.Cabinet)
                                           .ToListAsync();
            return View(inventories);
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

            if (ModelState.IsValid)
            {
                try
                {
                    var original = await _context.Inventories.AsNoTracking().FirstAsync(i => i.InventoryId == id);
                    int diff = inventory.Quantity - original.Quantity;

                    _context.Update(inventory);

                    // TRANSACTIE LOGGEN
                    var userId = HttpContext.Session.GetInt32("UserId");
                    if (userId.HasValue && diff != 0)
                    {
                        _context.Transactions.Add(new Transaction
                        {
                            InventoryId = inventory.InventoryId,
                            UserId = userId.Value,
                            QuantityChanged = diff,
                            Timestamp = DateTime.Now
                        });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.InventoryId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(inventory);
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.InventoryId == id);
        }

        private void LoadDropdowns()
        {
            ViewData["Parts"] = new SelectList(_context.Parts.ToList(), "PartId", "Name");
            ViewData["Cabinets"] = new SelectList(_context.Cabinets.ToList(), "CabinetId", "CabinetNumber");
        }
    }
}
