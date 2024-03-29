﻿using System;
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
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubCategories
        public IActionResult Index(int id)
        {
            // list sub categories with clicked category id
            var subCategories = _context.SubCategories.Where(sc => sc.CategoryId == id).OrderBy(sc => sc.Name).ToList();
            //passing on the category name to SubCategory Page
            ViewBag.category = _context.Categories.Find(id).Name.ToString();
            var applicationDbContext = _context.SubCategories.Include(s => s.Category);
            return View(subCategories);
        }

        // GET: SubCategories/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.SubCategoryId == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // GET: SubCategories/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories.OrderBy(c => c.Name), "CategoryId", "Name");
            return View();
        }

        // POST: SubCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("SubCategoryId,Name,CategoryId")] SubCategory subCategory, IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                // check if a valid photo is given in order to prevent null pointer exception
                if (Photo != null && Photo.Length > 0)
                {
                    var tempFile = Path.GetTempFileName();

                    var fileName = Guid.NewGuid() + "-" + Photo.FileName;

                    var uploadPath = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\sub-categories_added\\" + fileName;

                    // restrict the upload path to 256 characters to prevent upload of photos that can not be stored in OS
                    if (uploadPath.Length < 260)
                    {
                        using var stream = new FileStream(uploadPath, FileMode.Create);
                        await Photo.CopyToAsync(stream);

                        subCategory.Photo = fileName;
                    }
                }
                _context.Add(subCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = subCategory.CategoryId });
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategories/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories.FindAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", subCategory.CategoryId);
            return View(subCategory);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("SubCategoryId,Name,CategoryId")] SubCategory subCategory, IFormFile Photo)
        {
            if (id != subCategory.SubCategoryId)
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
                        var tempFile = Path.GetTempFileName();

                        var fileName = Guid.NewGuid() + "-" + Photo.FileName;

                        var uploadPath = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\sub-categories_added\\" + fileName;

                        // restrict the upload path to 256 characters to prevent upload of photos that can not be stored in OS
                        if (uploadPath.Length < 260)
                        {
                            using var stream = new FileStream(uploadPath, FileMode.Create);
                            await Photo.CopyToAsync(stream);

                            subCategory.Photo = fileName;
                        }
                    }
                    _context.Update(subCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoryExists(subCategory.SubCategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = subCategory.CategoryId });
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.SubCategoryId == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await _context.SubCategories.FindAsync(id);
            _context.SubCategories.Remove(subCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = subCategory.CategoryId });
        }

        // check if sub category exists in database
        private bool SubCategoryExists(int id)
        {
            return _context.SubCategories.Any(e => e.SubCategoryId == id);
        }


    }
}
