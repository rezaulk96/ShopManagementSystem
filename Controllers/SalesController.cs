using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Data;
using ShopManagementSystem.Models;
using ShopManagementSystem.Models.ViewModels;

namespace ShopManagementSystem.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POS Screen
        public IActionResult Create()
        {
            var vm = new SalesCreateVM
            {
                SalesId = _context.Sales.Any() ? _context.Sales.Max(x => x.SalesId) + 1 : 1,
                SalesDate = DateTime.Now,
                UserName = User.Identity?.Name ?? "Admin"
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int page = 1)
        {
            ViewBag.UserName = User.Identity?.Name ?? "Admin";

            int pageSize = 08;

            var salesQuery = _context.Sales
                .OrderByDescending(x => x.SalesDate);

            var totalCount = await salesQuery.CountAsync();

            var salesList = await salesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.SalesList = salesList;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return View(new SalesCreateVM());
        }

        // Product Auto Search
        public async Task<JsonResult> SearchProduct(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new List<object>());

            var products = await _context.ProductLiftings
                .Include(p => p.Product)
                .Where(p => p.Unit > 0 &&
                            p.Product.ProductName.Contains(term))
                .GroupBy(p => new { p.ProductId, p.Product.ProductName })
                .Select(g => new
                {
                    id = g.First().ProductLiftingId,
                    label = g.Key.ProductName,
                    price = g.Max(x => x.MRP), // latest/maximum MRP
                    stock = g.Sum(x => x.Unit) // total stock
                })
                .Where(x => x.stock > 0)
                .Take(10)
                .ToListAsync();

            return Json(products);



            //if (string.IsNullOrWhiteSpace(term))
            //    return Json(new List<object>());

            //var products = await _context.ProductLiftings
            //    .Include(p => p.Product)
            //    .Where(p => p.Product.ProductName.Contains(term))
            //    .Select(p => new
            //    {
            //        id = p.ProductLiftingId,
            //        label = p.Product.ProductName,
            //        price = p.MRP
            //    })
            //    .Take(10)
            //    .ToListAsync();

            //return Json(products);
        }

        // Complete Sale
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesCreateVM model)
        {
            if (model.Items == null || !model.Items.Any())
            {
                ModelState.AddModelError("", "No items added.");
                return View(model);
            }

            var sale = new Sales
            {
                SalesDate = DateTime.Now,
                GrandTotal = model.Items.Sum(x => x.TotalPrice),
                SalesDetails = new List<SalesDetail>()
            };

            // Validate stock and prepare SalesDetails
            foreach (var item in model.Items)
            {
                var productLifting = await _context.ProductLiftings
                    .FirstOrDefaultAsync(p => p.ProductLiftingId == item.ProductLiftingId);

                if (productLifting == null)
                {
                    ModelState.AddModelError("", "Product not found.");
                    return View(model);
                }

                if (productLifting.Unit < item.Quantity)
                {
                    ModelState.AddModelError("", "Not enough stock.");
                    return View(model);
                }

                // BACKUP BEFORE UPDATE
                var history = new ProductLiftingHistory
                {
                    ProductLiftingId = productLifting.ProductLiftingId,
                    ProductId = productLifting.ProductId,
                    PreviousUnit = productLifting.Unit,
                    SoldQuantity = item.Quantity,
                    RemainingUnit = productLifting.Unit - item.Quantity,
                    CostPrice = productLifting.CostPrice,
                    MRP = productLifting.MRP,
                    LiftingDate = productLifting.LiftingDate,
                    SoldDate = DateTime.Now
                };

                _context.ProductLiftingHistories.Add(history);

                // reduce stock
                productLifting.Unit -= item.Quantity;

                sale.SalesDetails.Add(new SalesDetail
                {
                    ProductLiftingId = item.ProductLiftingId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                });
            }

            // Add sale (and save all changes: stock updates + sale + details)
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            TempData["msg"] = "Sale Completed!";
            return RedirectToAction(nameof(Create));
        }
    }
}
