using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using PaternosterDemo.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PaternosterDemo.Controllers
{
    public class ProductionOrdersController : Controller
    {
        private readonly AppDbContext _context;

        public ProductionOrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProductionOrders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.ProductionOrders
                                       .Include(o => o.ProductionOrderParts)
                                       .ThenInclude(p => p.Part)
                                       .ToListAsync();
            return View(orders);
        }

        // GET: ProductionOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.ProductionOrders
                                      .Include(o => o.ProductionOrderParts)
                                      .ThenInclude(p => p.Part)
                                      .FirstOrDefaultAsync(o => o.ProductionOrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // GET: ProductionOrders/Create
        public IActionResult Create() => View();

        // POST: ProductionOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductionOrder order)
        {
            if (!ModelState.IsValid) return View(order);

            _context.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductionOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.ProductionOrders.FindAsync(id);
            if (order == null) return NotFound();

            return View(order);
        }

        // POST: ProductionOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductionOrder order)
        {
            if (id != order.ProductionOrderId) return NotFound();
            if (!ModelState.IsValid) return View(order);

            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductionOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.ProductionOrders.FindAsync(id);
            if (order == null) return NotFound();

            return View(order);
        }

        // POST: ProductionOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.ProductionOrders.FindAsync(id);
            if (order != null)
            {
                _context.ProductionOrders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
