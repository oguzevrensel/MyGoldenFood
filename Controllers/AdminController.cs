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
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            // Kullanıcı doğrulama
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Kullanıcı oturumu için Claims oluşturma
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("IsAdmin", "true") // Ek bir claim
                };

                var claimsIdentity = new ClaimsIdentity(claims, "AdminCookie");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // Oturum kalıcı olacak (tarayıcı kapansa bile)
                };

                // Authentication işlemini yap
                await HttpContext.SignInAsync("AdminCookie", new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Dashboard");
            }

            // Hatalı giriş
            ViewBag.ErrorMessage = "Invalid username or password!";
            return View();
        }



        [Authorize(AuthenticationSchemes = "AdminCookie")]
        [HttpGet]     
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminCookie");
            return RedirectToAction("Index");
        }

    }
}
