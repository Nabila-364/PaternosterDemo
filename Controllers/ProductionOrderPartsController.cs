using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using PaternosterDemo.Models;
using System.Linq;
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
            var parts = await _context.ProductionOrderParts
                                      .Include(p => p.Part)
                                      .Include(p => p.ProductionOrder)
                                      .ToListAsync();
            return View(parts);
        }

        // GET: ProductionOrderParts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var part = await _context.ProductionOrderParts
                                     .Include(p => p.Part)
                                     .Include(p => p.ProductionOrder)
                                     .FirstOrDefaultAsync(p => p.Id == id);

            if (part == null) return NotFound();

            return View(part);
        }

        // GET: ProductionOrderParts/Create
        public IActionResult Create()
        {
            ViewData["Parts"] = new SelectList(_context.Parts, "PartId", "Name");
            ViewData["Orders"] = new SelectList(_context.ProductionOrders, "OrderId", "OrderId");
            return View();
        }

        // POST: ProductionOrderParts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductionOrderPart productionOrderPart)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Parts"] = new SelectList(_context.Parts, "PartId", "Name", productionOrderPart.PartId);
                ViewData["Orders"] = new SelectList(_context.ProductionOrders, "OrderId", "OrderId", productionOrderPart.ProductionOrderOrderId);
                return View(productionOrderPart);
            }

            _context.Add(productionOrderPart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductionOrderParts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var part = await _context.ProductionOrderParts.FindAsync(id);
            if (part == null) return NotFound();

            ViewData["Parts"] = new SelectList(_context.Parts, "PartId", "Name", part.PartId);
            ViewData["Orders"] = new SelectList(_context.ProductionOrders, "OrderId", "OrderId", part.ProductionOrderOrderId);
            return View(part);
        }

        // POST: ProductionOrderParts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductionOrderPart productionOrderPart)
        {
            if (id != productionOrderPart.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["Parts"] = new SelectList(_context.Parts, "PartId", "Name", productionOrderPart.PartId);
                ViewData["Orders"] = new SelectList(_context.ProductionOrders, "OrderId", "OrderId", productionOrderPart.ProductionOrderOrderId);
                return View(productionOrderPart);
            }

            _context.Update(productionOrderPart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductionOrderParts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var part = await _context.ProductionOrderParts
                                     .Include(p => p.Part)
                                     .Include(p => p.ProductionOrder)
                                     .FirstOrDefaultAsync(p => p.Id == id);

            if (part == null) return NotFound();

            return View(part);
        }

        // POST: ProductionOrderParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var part = await _context.ProductionOrderParts.FindAsync(id);
            if (part != null)
            {
                _context.ProductionOrderParts.Remove(part);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
