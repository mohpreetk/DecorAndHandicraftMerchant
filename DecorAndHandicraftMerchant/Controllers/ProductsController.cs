using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DecorAndHandicraftMerchant.Data;
using DecorAndHandicraftMerchant.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace DecorAndHandicraftMerchant.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public IActionResult Index(int id)
        {
            //using queries instead of logical code in views
            var products = _context.Products.Where(p => p.SubCategoryId == id).OrderBy(p => p.Name).ToList();
            ViewBag.subCategory = _context.SubCategories.Find(id).Name.ToString();
            var applicationDbContext = _context.Products.Include(p => p.SubCategory);
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories.OrderBy(sc => sc.Name), "SubCategoryId", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,SubCategoryId")] Product product, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                if (product.Photo != null && Photo.Length > 0)
                {
                    var tempFile = Path.GetTempFileName();

                    var fileName = Guid.NewGuid() + "-" + Photo.FileName;

                    var uploadPath = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\products_added\\" + fileName;
                    if (uploadPath.Length < 260)
                    {
                        using var stream = new FileStream(uploadPath, FileMode.Create);
                        await Photo.CopyToAsync(stream);

                        product.Photo = fileName;
                    }
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = product.SubCategoryId });
            }
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "Name", product.SubCategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "Name", product.SubCategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,SubCategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = product.SubCategoryId });
            }
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "Name", product.SubCategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = product.SubCategoryId });
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        //Temporary method created to demonstrate add to cart option only for logged in users
        [Authorize]
        public async Task<IActionResult> AddToCart()
        {
            var applicationDbContext = _context.Products.Include(p => p.SubCategory);
            return View(await applicationDbContext.OrderBy(c => c.Name).ToListAsync());
        }

    }
}

