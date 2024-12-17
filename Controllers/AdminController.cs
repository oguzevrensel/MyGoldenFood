using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyGoldenFood.ApplicationDbContext;
using System.Security.Claims;

namespace MyGoldenFood.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // Giriş Sayfası
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Giriş Doğrulama
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "Admin") // Admin rolü ekleniyor
                };

                var claimsIdentity = new ClaimsIdentity(claims, "AdminCookie");
                await HttpContext.SignInAsync("AdminCookie", new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Dashboard");
            }

            ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre!";
            return View();
        }

        // Dashboard Sayfası
        [Authorize(AuthenticationSchemes = "AdminCookie")]
        public IActionResult Dashboard()
        {
            return View();
        }

        // Oturum Kapatma
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminCookie");
            return RedirectToAction("Index");
        }
    }
}
