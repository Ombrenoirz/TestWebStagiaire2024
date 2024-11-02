using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TestWebStagiaire2024.Data;
using TestWebStagiaire2024.Models;
using TestWebStagiaire2024.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace TestWebStagiaire2024.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IPasswordHasher<User> passwordHasher;

        public AuthController(AppDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.passwordHasher = new PasswordHasher<User>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Username == viewModel.Username);
            if (user is null || passwordHasher.VerifyHashedPassword(user, user.PasswordHash, viewModel.Password) == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("Username", "Nom d'utilisateur ou mot de passe invalide");
                ModelState.AddModelError("Password", "Nom d'utilisateur ou mot de passe invalide");
                return View(viewModel);
            }

            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var existingUser = await dbContext.Users.SingleOrDefaultAsync(u => u.Username == viewModel.Username);
            if (existingUser is not null)
            {
                ModelState.AddModelError("Username", "Nom d'utilisateur existant");
                return View(viewModel);
            }

            var user = new User
            {
                Username = viewModel.Username,
                PasswordHash = passwordHasher.HashPassword(new User(), viewModel.Password)
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Login", "Auth");
        }
    }
}
