﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DecorAndHandicraftMerchant.Data;
using DecorAndHandicraftMerchant.Models;
using Microsoft.AspNetCore.Authorization;

namespace DecorAndHandicraftMerchant.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public IActionResult Index()
        {
            if (User.IsInRole("Administrator"))
            {
                var orders = _context.Orders.Include(o => o.Profile).Include(o => o.OrderDetails).OrderByDescending(o => o.OrderDate).ToList();
                return View(orders);
            }
            else
            {
                var orders = _context.Orders.Where(o => o.Profile.Username == User.Identity.Name).OrderByDescending(o => o.OrderDate).ToList();
                var applicationDbContext = _context.Orders.Include(o => o.Profile).Include(o => o.OrderDetails);
                return View(orders);
            }
        }

        public IActionResult OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetails = _context.OrderDetails.Include(od => od.Product).Where(od => od.OrderId == id).OrderBy(od => od.Product.Name).ToList();
            ViewData["OrderId"] = _context.Orders.Find(id).OrderId;
            
            if (orderDetails == null)
            {
                return NotFound();
            }

            return View(orderDetails);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
