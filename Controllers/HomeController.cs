using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWebStagiaire2024.Data;
using TestWebStagiaire2024.Models;
using TestWebStagiaire2024.Models.Entities;

namespace TestWebStagiaire2024.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var items = await dbContext.Items.Where(i => !i.IsDeleted && i.UserId == HttpContext.Session.GetInt32("UserId")).ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await dbContext.Items.FindAsync(id);
            if (item is null)
                return NotFound();
            return Json(item);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
                });
            }

            try
            {
                if (item.Id is null)
                {
                    dbContext.Items.Add(item);
                }
                else
                {
					var existingItem = await dbContext.Items
				.FirstOrDefaultAsync(i => i.Id == item.Id && i.UserId == HttpContext.Session.GetInt32("UserId") && !i.IsDeleted);
                    if (existingItem is not null)
                    {
                        existingItem.Name = item.Name;
                        existingItem.Quantity = item.Quantity;
                        existingItem.Price = item.Price;
                    }
                    else {
                        throw new Exception();
                    }
                }
                var result = await dbContext.SaveChangesAsync();
				return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await dbContext.Items.FindAsync(id);
            if (item is not null)
            {
                item.IsDeleted = true;
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

		[HttpPost]
		public async Task<IActionResult> DeleteAll(int userId)
		{
			var itemsToDelete = await dbContext.Items
											   .Where(i => i.UserId == userId && !i.IsDeleted)
											   .ToListAsync();
			foreach (var item in itemsToDelete)
			{
				item.IsDeleted = true;
			}

			await dbContext.SaveChangesAsync();

			return RedirectToAction("Index", "Home");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
