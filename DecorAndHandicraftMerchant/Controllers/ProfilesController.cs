using System;
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
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // no index because each user will have only one profile

        // GET: Profiles/Details/5
        public async Task<IActionResult> Details(string username)
        {
            if (username == null)
            {
                return NotFound();
            }

            // show the profile for logged in user
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(m => m.Username == username);
            if (profile == null)
            {
                return RedirectToAction(nameof(Create));
            }

            return View(profile);
        }

        // GET: Profiles/Create
        public IActionResult Create()
        {
            //check if the profile already exists, edit if does and create if it does not
            var usernameVerification = _context.Profiles.Where(m => m.Username == User.Identity.Name).ToList();
            if (usernameVerification.Count == 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Edit), new { username = User.Identity.Name });
            }
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProfileId,Username, FirstName,LastName,PhoneNumber,AddressLine1,AddressLine2,City,Province,Country,PostalCode")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                _context.Profiles.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { username = profile.Username });
            }

            return View(profile);
        }

        // GET: Profiles/Edit/5
        public async Task<IActionResult> Edit(string username)
        {
            if (username == null)
            {
                return NotFound();
            }
            // get user's profile
            var profile = await _context.Profiles.FirstOrDefaultAsync(m => m.Username == username);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string username, [Bind("ProfileId,Username, FirstName,LastName,PhoneNumber,AddressLine1,AddressLine2,City,Province,Country,PostalCode")] Profile profile)
        {
            if (username != profile.Username)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.ProfileId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), profile.Username);
            }
            return View(profile);
        }

        // GET: Profiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .FirstOrDefaultAsync(m => m.ProfileId == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //check if profile exists in database
        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.ProfileId == id);
        }
    }
}
