using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using PaternosterDemo.Models;
using System.Threading.Tasks;

namespace PaternosterDemo.Controllers
{
    public class ProductionOrderPartsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductionOrderPartsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProductionOrderParts
        public async Task<IActionResult> Index()
        {
            var productionOrderParts = await _context.ProductionOrderParts
                .Include(p => p.Part)
                .Include(p => p.ProductionOrder)
                .ToListAsync();

            return View(productionOrderParts);
        }

        // GET: ProductionOrderParts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var productionOrderPart = await _context.ProductionOrderParts
                .Include(p => p.Part)
                .Include(p => p.ProductionOrder)
                .FirstOrDefaultAsync(m => m.ProductionOrderPartId == id);

            if (productionOrderPart == null) return NotFound();

            return View(productionOrderPart);
        }

        // GET: ProductionOrderParts/Create
        public IActionResult Create()
        {
            ViewData["Parts"] = new SelectList(_context.Parts, "PartId", "Name");
            ViewData["Orders"] = new SelectList(_context.ProductionOrders, "ProductionOrderId", "ProductionOrderId");
            return View();
        }

        // POST: ProductionOrderParts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductionOrderPart productionOrderPart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productionOrderPart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Parts"] = new SelectList(_context.Parts, "PartId", "Name", productionOrderPart.PartId);
            ViewData["Orders"] = new SelectList(_context.ProductionOrders, "ProductionOrderId", "ProductionOrderId", productionOrderPart.ProductionOrderId);
            return View(productionOrderPart);
        }

        // GET: ProductionOrderParts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var productionOrderPart = await _context.ProductionOrderParts.FindAsync(id);
            if (productionOrderPart == null) return NotFound();

            ViewData["Parts"] = new SelectList(_context.Parts, "PartId", "Name", productionOrderPart.PartId);
            ViewData["Orders"] = new SelectList(_context.ProductionOrders, "ProductionOrderId", "ProductionOrderId", productionOrderPart.ProductionOrderId);
            return View(productionOrderPart);
        }

        // POST: ProductionOrderParts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductionOrderPart productionOrderPart)
        {
            if (id != productionOrderPart.ProductionOrderPartId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productionOrderPart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductionOrderPartExists(productionOrderPart.ProductionOrderPartId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Parts"] = new SelectList(_context.Parts, "PartId", "Name", productionOrderPart.PartId);
            ViewData["Orders"] = new SelectList(_context.ProductionOrders, "ProductionOrderId", "ProductionOrderId", productionOrderPart.ProductionOrderId);
            return View(productionOrderPart);
        }

        // GET: ProductionOrderParts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var productionOrderPart = await _context.ProductionOrderParts
                .Include(p => p.Part)
                .Include(p => p.ProductionOrder)
                .FirstOrDefaultAsync(m => m.ProductionOrderPartId == id);

            if (productionOrderPart == null) return NotFound();

            return View(productionOrderPart);
        }

        // POST: ProductionOrderParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productionOrderPart = await _context.ProductionOrderParts.FindAsync(id);
            if (productionOrderPart != null)
            {
                _context.ProductionOrderParts.Remove(productionOrderPart);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductionOrderPartExists(int id)
        {
            return _context.ProductionOrderParts.Any(e => e.ProductionOrderPartId == id);
        }
    }
}
