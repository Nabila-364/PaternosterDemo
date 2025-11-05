using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using PaternosterDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaternosterDemo.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionOrdersApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductionOrdersApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductionOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionOrder>>> GetProductionOrders()
        {
            return await _context.ProductionOrders
                                 .Include(p => p.ProductionOrderParts)
                                 .ThenInclude(pp => pp.Part)
                                 .ToListAsync();
        }

        // GET: api/ProductionOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionOrder>> GetProductionOrder(int id)
        {
            var order = await _context.ProductionOrders
                                      .Include(p => p.ProductionOrderParts)
                                      .ThenInclude(pp => pp.Part)
                                      .FirstOrDefaultAsync(p => p.ProductionOrderId == id);

            if (order == null)
                return NotFound();

            return order;
        }

        // POST: api/ProductionOrders
        [HttpPost]
        public async Task<ActionResult<ProductionOrder>> CreateProductionOrder(ProductionOrder order)
        {
            _context.ProductionOrders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductionOrder), new { id = order.ProductionOrderId }, order);
        }

        // PUT: api/ProductionOrders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductionOrder(int id, ProductionOrder order)
        {
            if (id != order.ProductionOrderId)
                return BadRequest();

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.ProductionOrders.AnyAsync(p => p.ProductionOrderId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/ProductionOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductionOrder(int id)
        {
            var order = await _context.ProductionOrders.FindAsync(id);
            if (order == null)
                return NotFound();

            _context.ProductionOrders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
