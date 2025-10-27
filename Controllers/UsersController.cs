using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaternosterDemo.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PaternosterDemo.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                // Laat een nette melding zien i.p.v. crashen
                TempData["ErrorMessage"] = "U bent niet gemachtigd om deze pagina te bekijken.";
                return RedirectToAction("Index", "Home");
            }

            var users = await _context.Users.ToListAsync();
            return View(users);
        }
    }
}
