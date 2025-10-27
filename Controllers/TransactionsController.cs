using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using System.Threading.Tasks;
using System.Linq;

namespace PaternosterDemo.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Inventory)
                    .ThenInclude(i => i.Part!)
                .Include(t => t.Inventory)
                    .ThenInclude(i => i.Cabinet!)
                .Include(t => t.User!)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();

            return View(transactions);
        }
    }
}

