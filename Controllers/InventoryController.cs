using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using PaternosterDemo.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PaternosterDemo.Controllers
{
    public class InventoryController : Controller
    {
        private readonly AppDbContext _context;
        public InventoryController(AppDbContext context) => _context = context;

        // GET: Inventory
        public async Task<IActionResult> Index(string? search)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            ViewBag.IsAdmin = HttpContext.Session.GetString("Role") == "Admin";

            var query = _context.Inventories
                                .Include(i => i.Part)
                                .Include(i => i.Cabinet)
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i =>
                    (i.Part != null && i.Part.Name != null && i.Part.Name.Contains(search)) ||
                    (i.Part != null && i.Part.ArticleNumber != null && i.Part.ArticleNumber.Contains(search))
                );
            }

            var list = await query.ToListAsync();

            ViewBag.TotalPerPart = list.GroupBy(i => i.PartId)
                                       .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));

            return View(list);
        }

        // GET: Inventory/Create
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            LoadDropdowns();
            return View();
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inventory inventory)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                // Eerst Inventory toevoegen
                _context.Inventories.Add(inventory);
                await _context.SaveChangesAsync(); // InventoryId wordt nu gegenereerd

                // Daarna Transaction toevoegen
                _context.Transactions.Add(new Transaction
                {
                    InventoryId = inventory.InventoryId,
                    UserId = userId.Value,
                    QuantityChanged = inventory.Quantity,
                    Timestamp = System.DateTime.Now
                });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(inventory);
        }

        // GET: Inventory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            if (id == null) return NotFound();

            var inventory = await _context.Inventories
                                          .Include(i => i.Part)
                                          .Include(i => i.Cabinet)
                                          .FirstOrDefaultAsync(i => i.InventoryId == id);
            if (inventory == null) return NotFound();

            LoadDropdowns();
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Inventory inventory)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            if (id != inventory.InventoryId) return NotFound();

            if (ModelState.IsValid)
            {
                var original = await _context.Inventories.AsNoTracking()
                                    .FirstOrDefaultAsync(i => i.InventoryId == id);
                if (original == null) return NotFound();

                int diff = inventory.Quantity - original.Quantity;

                _context.Update(inventory);

                if (diff != 0)
                {
                    _context.Transactions.Add(new Transaction
                    {
                        InventoryId = inventory.InventoryId,
                        UserId = userId.Value,
                        QuantityChanged = diff,
                        Timestamp = System.DateTime.Now
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(inventory);
        }

        // GET: Inventory/Delete/5 (Admin only)
        public async Task<IActionResult> Delete(int? id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("AccessDenied", "Account");

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
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return RedirectToAction("Login", "Account");
            if (HttpContext.Session.GetString("Role") != "Admin") return RedirectToAction("AccessDenied", "Account");

            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory != null)
            {
                _context.Transactions.Add(new Transaction
                {
                    InventoryId = inventory.InventoryId,
                    UserId = userId.Value,
                    QuantityChanged = -inventory.Quantity,
                    Timestamp = System.DateTime.Now
                });

                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns()
        {
            ViewData["Parts"] = new SelectList(_context.Parts.AsNoTracking().ToList(), "PartId", "Name");
            ViewData["Cabinets"] = new SelectList(_context.Cabinets.AsNoTracking().ToList(), "CabinetId", "CabinetNumber");
        }
    }
}
