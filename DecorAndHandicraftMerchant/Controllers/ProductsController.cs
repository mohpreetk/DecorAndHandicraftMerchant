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
            //set up iconfiguration for stripe
            _iconfiguration = iconfiguration;
        }

        // GET: Products
        public IActionResult Index(int id)
        {
            //list products with given sub category id
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

            //find product for given sub category id
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
            // order the sub category list by name
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
                // check if a valid photo is given in order to prevent null pointer exception
                if (Photo != null && Photo.Length > 0)
                {
                    // get the name of temporary file
                    var tempFile = Path.GetTempFileName();

                    // get a new GUId
                    var fileName = Guid.NewGuid() + "-" + Photo.FileName;

                    // set a directory for uploaded photos
                    var uploadPath = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\products_added\\" + fileName;
                    // restrict the upload path to 256 characters to prevent upload of photos that can not be stored in OS
                    if (uploadPath.Length < 256)
                    {
                        using var stream = new FileStream(uploadPath, FileMode.Create);
                        await Photo.CopyToAsync(stream);
                        // set photoname in products photo field
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

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,SubCategoryId")] Models.Product product, IFormFile Photo)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // check if a valid photo is given in order to prevent null pointer exception
                    if (Photo != null && Photo.Length > 0)
                    {
                        // get the name of temporary file
                        var tempFile = Path.GetTempFileName();

                        // get a new GUId
                        var fileName = Guid.NewGuid() + "-" + Photo.FileName;

                        // set a directory for uploaded photos
                        var uploadPath = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\products_added\\" + fileName;
                        // restrict the upload path to 256 characters to prevent upload of photos that can not be stored in OS
                        if (uploadPath.Length < 256)
                        {
                            using var stream = new FileStream(uploadPath, FileMode.Create);
                            await Photo.CopyToAsync(stream);
                            // set photoname in products photo field
                            product.Photo = fileName;
                        }
                    }
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

        // check if product exists in database
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        // Products/AddToCart
        public IActionResult AddToCart(int ProductId, int Quantity)
        {
            //find price of product
            var price = _context.Products.Find(ProductId).Price;

            //log the current time
            var timeStamp = DateTime.Now;
            //Create Customer Identification
            var CustomerId = GetCustomerId();
            // create new cart
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

        //create a session customer id for anonymous users
        private string GetCustomerId()
        {
            if (HttpContext.Session.GetString("CustomerId") == null)
            {
                var CustomerId = "";
                // assign email as customer id to registered users
                if (User.Identity.IsAuthenticated)
                {
                    //name is not actually name, it is email address
                    CustomerId = User.Identity.Name;
                }
                // assign guid as customer id to anonymous users
                else
                {
                    CustomerId = Guid.NewGuid().ToString();
                }
                HttpContext.Session.SetString("CustomerId", CustomerId);

            }
            return HttpContext.Session.GetString("CustomerId");
        }

        //Products/Cart
        public IActionResult Cart()
        {
            var CustomerId = "";
            //get new customer id if not yet assigned
            if (HttpContext.Session.GetString("CustomerId") == null)
            {
                CustomerId = GetCustomerId();
            }
            //else get the assigned customer id
            else
            {
                CustomerId = HttpContext.Session.GetString("CustomerId");
            }
            // list products inside the cart for this customer
            var cartProducts
                 = _context.Carts.Include(c => c.Product).Where(c => c.CustomerId == CustomerId).ToList();
            return View(cartProducts);
        }

        [Authorize]
        public IActionResult Checkout(decimal id)
        {
            // format total as amount
            ViewBag.Total = String.Format("{0:c}", id);
            // if user does not have a profile, ask to create one
            if (_context.Profiles
                .FirstOrDefault(m => m.Username == User.Identity.Name) == null)
            {

                return RedirectToAction(nameof(Create), "Profiles");
            }
            //else pass the profile to view
            else
            {
                ViewData["Profile"] = _context.Profiles.FirstOrDefault(m => m.Username == User.Identity.Name);
                return View();
            }
        }

        // Remove the selected item from cart
        public IActionResult RemoveFromCart(int id)
        {
            // find the item and remove if not null
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
            // set order variables
            order.OrderDate = DateTime.Now;
            order.Total = (from c in _context.Carts
                           where c.CustomerId == HttpContext.Session.GetString("CustomerId")
                           select c.Quantity * c.UnitPrice).Sum();
            // add order as a sesion object
            HttpContext.Session.SetObject("Order", order);
            return RedirectToAction("Payment");
        }

        [Authorize]
        public IActionResult Payment()
        {
            // fetch session object and its values
            var order = HttpContext.Session.GetObject<Models.Order>("Order");
            ViewBag.Total = order.Total;

            // provide stripe pk
            ViewBag.PublishableKey = _iconfiguration.GetSection("Stripe")["PublishableKey"];

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ProcessPayment()
        {
            //sent the payment to stripe payment gateway
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
                // redirect to save order on success and cart on cancel
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

        [Authorize]
        public IActionResult SaveOrder()
        {
            // fetch session object and its values
            var order = HttpContext.Session.GetObject<Models.Order>("Order");
            // add order to database
            _context.Orders.Add(order);
            _context.SaveChanges();
            // get the current cart items and add each of them to order detail
            var cartItems = _context.Carts.Where(c => c.CustomerId == User.Identity.Name);

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Total = item.UnitPrice * item.Quantity
                };

                _context.OrderDetails.Add(orderDetail);
            }
            _context.SaveChanges();
            // clear cart
            foreach (var item in cartItems)
            {
                _context.Carts.Remove(item);
            }
            _context.SaveChanges();

            HttpContext.Session.SetInt32("ItemCount", 0);
            return RedirectToAction("OrderDetails", "Orders", new { id = order.OrderId });
        }

    }
}

