using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Controllers
{
    public class ProductLiftingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductLiftingController(ApplicationDbContext context)
        {
            _context = context;
        }
        //Index
        public async Task<IActionResult> Index()
        {
            var data = await _context.ProductLiftings
                .Include(p => p.Product)
                .ToListAsync();
            return View(data);
        }

        //Create Get
        public IActionResult Create()
        {
            ViewBag.ProductList = new SelectList(_context.Products, "ProductId", "ProductName");
            return View();
        }

        //Create Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductLifting model)
        {
            ViewBag.ProductList = new SelectList(_context.Products, "ProductId", "ProductName", model.ProductId);
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            _context.ProductLiftings.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Edit Get
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _context.ProductLiftings.FindAsync(id);
            if (data == null)
                return NotFound();

            ViewBag.ProductList = new SelectList(_context.Products, "ProductId", "ProductName", data.ProductId);
            return View(data);
        }
        //Edit Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductLifting model)
        {
            if (id != model.ProductLiftingId)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ProductList = new SelectList(_context.Products, "ProductId", "ProductName", model.ProductId);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFonfirmed(int id)
        {
            var data = await _context.ProductLiftings.FindAsync(id);
            if (data == null)
                return NotFound();

            _context.ProductLiftings.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
