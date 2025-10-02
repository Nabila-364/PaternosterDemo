using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using PaternosterDemo.Data;
using PaternosterDemo.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PaternosterDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vul gebruikersnaam en wachtwoord in.";
                return View();
            }

            var exists = await _context.Users.AnyAsync(u => u.Username == username);
            if (exists)
            {
                ViewBag.Error = "Gebruikersnaam bestaat al.";
                return View();
            }

            var user = new User
            {
                Username = username,
                PasswordHash = ComputeHash(password),
                Role = string.IsNullOrWhiteSpace(role) ? "Medewerker" : role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Login));
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vul gebruikersnaam en wachtwoord in.";
                return View();
            }

            var hash = ComputeHash(password);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == hash);

            if (user == null)
            {
                ViewBag.Error = "Ongeldige gebruikersnaam of wachtwoord.";
                return View();
            }

            // Sla session-gegevens op
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("Index", "Inventory");
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        // Helper: SHA256-hash van wachtwoord
        private string ComputeHash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}
