using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebPulse2023.Data;
using WebPulse2023.Models;

namespace WebPulse2023.Controllers
{
    public class PingStatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PingStatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PingStatistics
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PingStatistic.Include(p => p.Website);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PingStatistics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PingStatistic == null)
            {
                return NotFound();
            }

            var pingStatistic = await _context.PingStatistic
                .Include(p => p.Website)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pingStatistic == null)
            {
                return NotFound();
            }

            return View(pingStatistic);
        }

        // GET: PingStatistics/Create
        public IActionResult Create()
        {
            ViewData["WebsiteId"] = new SelectList(_context.Website, "Id", "Id");
            return View();
        }

        // POST: PingStatistics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WebsiteId,Interval,Timestamp,UpCount,DownCount")] PingStatistic pingStatistic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pingStatistic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WebsiteId"] = new SelectList(_context.Website, "Id", "Id", pingStatistic.WebsiteId);
            return View(pingStatistic);
        }

        // GET: PingStatistics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PingStatistic == null)
            {
                return NotFound();
            }

            var pingStatistic = await _context.PingStatistic.FindAsync(id);
            if (pingStatistic == null)
            {
                return NotFound();
            }
            ViewData["WebsiteId"] = new SelectList(_context.Website, "Id", "Id", pingStatistic.WebsiteId);
            return View(pingStatistic);
        }

        // POST: PingStatistics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WebsiteId,Interval,Timestamp,UpCount,DownCount")] PingStatistic pingStatistic)
        {
            if (id != pingStatistic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pingStatistic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PingStatisticExists(pingStatistic.Id))
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
            ViewData["WebsiteId"] = new SelectList(_context.Website, "Id", "Id", pingStatistic.WebsiteId);
            return View(pingStatistic);
        }

        // GET: PingStatistics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PingStatistic == null)
            {
                return NotFound();
            }

            var pingStatistic = await _context.PingStatistic
                .Include(p => p.Website)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pingStatistic == null)
            {
                return NotFound();
            }

            return View(pingStatistic);
        }

        // POST: PingStatistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PingStatistic == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PingStatistic'  is null.");
            }
            var pingStatistic = await _context.PingStatistic.FindAsync(id);
            if (pingStatistic != null)
            {
                _context.PingStatistic.Remove(pingStatistic);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PingStatisticExists(int id)
        {
          return (_context.PingStatistic?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
