using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PaternosterDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userRole = HttpContext.Session.GetString("Role") ?? "Gast";

            var dashboardData = new
            {
                PartCount = await _context.Parts.CountAsync(),
                InventoryCount = await _context.Inventories.SumAsync(i => (int?)i.Quantity) ?? 0,
                CabinetCount = await _context.Cabinets.CountAsync(),
                UserCount = userRole == "Admin" ? await _context.Users.CountAsync() : (int?)null,
                Role = userRole
            };

            return View(dashboardData);
        }
    }
}
