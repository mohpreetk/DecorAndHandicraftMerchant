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
//Stripe payment
using Stripe;
//access stripe keys
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;

namespace DecorAndHandicraftMerchant.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        IConfiguration _iconfiguration;

        public ProductsController(ApplicationDbContext context, IConfiguration iconfiguration)
        {
            _context = context;
            _iconfiguration = iconfiguration;
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
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,SubCategoryId")] Models.Product product, IFormFile Photo)
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
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,SubCategoryId")] Models.Product product)
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

        public IActionResult AddToCart(int ProductId, int Quantity)
        {
            var price = _context.Products.Find(ProductId).Price;

            var timeStamp = DateTime.Now;
            //Create Customer Identification
            var CustomerId = GetCustomerId();
            var cart = new Cart
            {
                ProductId = ProductId,
                TimeStamp = timeStamp,
                Quantity = Quantity,
                UnitPrice = price,
                CustomerId = CustomerId
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();

            return RedirectToAction("Cart");
        }

        private string GetCustomerId()
        {
            if (HttpContext.Session.GetString("CustomerId") == null)
            {
                var CustomerId = "";
                if (User.Identity.IsAuthenticated)
                {
                    //name is not actually name, it is email address
                    CustomerId = User.Identity.Name;
                }
                else
                {
                    CustomerId = Guid.NewGuid().ToString();
                }
                HttpContext.Session.SetString("CustomerId", CustomerId);

            }
            return HttpContext.Session.GetString("CustomerId");
        }


        public IActionResult Cart()
        {
            var CustomerId = "";
            if (HttpContext.Session.GetString("CustomerId") == null)
            {
                CustomerId = GetCustomerId();
            }
            else
            {
                CustomerId = HttpContext.Session.GetString("CustomerId");
            }
            Console.WriteLine(HttpContext.Session.GetString("CustomerId"));

            var cartProducts
                 = _context.Carts.Include(c => c.Product).Where(c => c.CustomerId == CustomerId).ToList();
            return View(cartProducts);
        }

        [Authorize]
        public IActionResult Checkout(decimal id)
        {
            ViewBag.Total = String.Format("{0:c}", id);
            if (_context.Profiles
                .FirstOrDefault(m => m.Username == User.Identity.Name) == null)
            {

                return RedirectToAction(nameof(Create), "Profiles");
            }
            else
            {
                ViewData["Profile"] = _context.Profiles.FirstOrDefault(m => m.Username == User.Identity.Name);
                return View();
            }
        }

        public IActionResult RemoveFromCart(int id)
        {
            var item = _context.Carts.Find(id);
            if (item != null)
            {
                _context.Carts.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Checkout([Bind("ProfileId")] Models.Order order)
        {
            order.OrderDate = DateTime.Now;
            order.Total = (from c in _context.Carts
                           where c.CustomerId == HttpContext.Session.GetString("CustomerId")
                           select c.Quantity * c.UnitPrice).Sum();
            HttpContext.Session.SetObject("Order", order);
            return RedirectToAction("Payment");
        }

        [Authorize]
        public IActionResult Payment()
        {
            var order = HttpContext.Session.GetObject<Models.Order>("Order");
            ViewBag.Total = order.Total;

            ViewBag.PublishableKey = _iconfiguration.GetSection("Stripe")["PublishableKey"];

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ProcessPayment()
        {
            var order = HttpContext.Session.GetObject<Models.Order>("Order");

            StripeConfiguration.ApiKey = _iconfiguration.GetSection("Stripe")["SecretKey"];

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                      UnitAmount = (long?)(order.Total * 100),
                      Currency = "cad",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                        Name = "Decor and Handicraft Merchant"
                      }
                    },
                    Quantity = 1
                  }
                },
                Mode = "payment",
                SuccessUrl = "https://" + Request.Host + "/Products/SaveOrder",
                CancelUrl = "https://" + Request.Host + "/Products/Cart"
            };
            var service = new SessionService();
            Session session = service.Create(options);
            return Json(new
            {
                id = session.Id
            });
        }

    }
}

