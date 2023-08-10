using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebPulse2023.Data;
using WebPulse2023.Models;
using WebPulse2023.ViewModels;

namespace WebPulse2023.Controllers
{
    public class WebsitesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WebsitesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Websites
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if user not logged in
            }

            var userWebsites = _context.Website.Where(w => w.UserId == currentUser.Id).ToList();
            return View(userWebsites);
        }

        // GET: Websites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Website == null)
            {
                return NotFound();
            }

            var website = await _context.Website
                .FirstOrDefaultAsync(m => m.Id == id);
            if (website == null)
            {
                return NotFound();
            }

            var webPingUpAmount = await _context.WebPing.Where(p => p.Website == website && p.isUp == true)
        .CountAsync();

            var webPingDownAmount = await _context.WebPing
                .Where(p => p.Website == website && !p.isUp == false)
                .CountAsync();

            var pingStatisticUpAmount = await _context.PingStatistic
                .Where(ps => ps.Website == website)
                .SumAsync(ps => ps.UpCount);

            var pingStatisticDownAmount = await _context.PingStatistic
                .Where(ps => ps.Website == website)
                .SumAsync(ps => ps.DownCount);

            var totalUpAmount = webPingUpAmount + pingStatisticUpAmount;
            var totalDownAmount = webPingDownAmount + pingStatisticDownAmount;

            var viewModel = new WebsiteViewModel
            {
                Id = website.Id,
                Url = website.Url,
                Name = website.Name,
                Active = website.Active,
                CreatedAt = website.CreatedAt,
                UpAmount = totalUpAmount,
                DownAmount = totalDownAmount
            };

            return View(viewModel);
        }

        // GET: Websites/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Websites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Url,Name,Active")] Website website)
        {
            // Set user-related information
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                website.UserId = currentUser.Id;

                // Manually remove UserId from ModelState
                ModelState.Remove("UserId");

                if (ModelState.IsValid)
                {
                    _context.Add(website);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            // Handle the case where the current user couldn't be found or authenticated.

            return View(website);
        }

        // GET: Websites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Website == null)
            {
                return NotFound();
            }

            var website = await _context.Website.FindAsync(id);
            if (website == null)
            {
                return NotFound();
            }

            var viewModel = new WebsiteEditViewModel
            {
                Id = website.Id,
                Url = website.Url,
                Name = website.Name,
                Active = website.Active
            };

            return View(viewModel);
        }


        // POST: Websites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WebsiteEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var website = await _context.Website.FindAsync(id);
                if (website == null)
                {
                    return NotFound();
                }

                website.Url = viewModel.Url;
                website.Name = viewModel.Name;
                website.Active = viewModel.Active;

                try
                {
                    _context.Update(website);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WebsiteExists(website.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }


        // GET: Websites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Website == null)
            {
                return NotFound();
            }

            var website = await _context.Website
                .FirstOrDefaultAsync(m => m.Id == id);
            if (website == null)
            {
                return NotFound();
            }

            return View(website);
        }

        // POST: Websites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Website == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Website'  is null.");
            }
            var website = await _context.Website.FindAsync(id);
            if (website != null)
            {
                var webPingsToDelete = _context.WebPing.Where(wp => wp.WebsiteId == id);
                _context.WebPing.RemoveRange(webPingsToDelete);
                _context.Website.Remove(website);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WebsiteExists(int id)
        {
          return (_context.Website?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
