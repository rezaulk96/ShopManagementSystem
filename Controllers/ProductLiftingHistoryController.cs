using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;

namespace ShopManagementSystem.Controllers
{
    public class ProductLiftingHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductLiftingHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get ProductLiftingHistory
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;

            var query = _context.ProductLiftingHistories
                .Include(p => p.Product)
                .Include(p => p.ProductLifting)
                .OrderByDescending(p => p.SoldDate);

            var totalItems = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(data);
        }
    }
}
