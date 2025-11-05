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
    public class ProductionOrderPartsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductionOrderPartsApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductionOrderParts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionOrderPart>>> GetParts()
        {
            return await _context.ProductionOrderParts
                                 .Include(pp => pp.Part)
                                 .Include(pp => pp.ProductionOrder)
                                 .ToListAsync();
        }

        // GET: api/ProductionOrderParts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionOrderPart>> GetPart(int id)
        {
            var part = await _context.ProductionOrderParts
                                     .Include(pp => pp.Part)
                                     .Include(pp => pp.ProductionOrder)
                                     .FirstOrDefaultAsync(pp => pp.ProductionOrderId == id);

            if (part == null)
                return NotFound();

            return part;
        }

        // POST: api/ProductionOrderParts
        [HttpPost]
        public async Task<ActionResult<ProductionOrderPart>> CreatePart(ProductionOrderPart part)
        {
            _context.ProductionOrderParts.Add(part);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPart), new { id = part.ProductionOrderId }, part);
        }

        // PUT: api/ProductionOrderParts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePart(int id, ProductionOrderPart part)
        {
            if (id != part.ProductionOrderId)
                return BadRequest();

            _context.Entry(part).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.ProductionOrderParts.AnyAsync(p => p.ProductionOrderId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/ProductionOrderParts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePart(int id)
        {
            var part = await _context.ProductionOrderParts.FindAsync(id);
            if (part == null)
                return NotFound();

            _context.ProductionOrderParts.Remove(part);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
