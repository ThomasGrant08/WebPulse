using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPulse2023.Data;
using WebPulse2023.Models;
using WebPulse2023.Services;

namespace WebPulse2023.Controllers
{
    public class WebPingController : Controller
    {
        private readonly IWebPingService _webPingService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WebPingController(IWebPingService webPingService, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _webPingService = webPingService;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var webPings = _context.WebPing.Include(w => w.Website).ToList();
            return View(webPings);
        }

        public IActionResult SiteTracker()
        {
            var userID = _userManager.GetUserId(HttpContext.User);
            var websites = _context.Website.Where(w => w.UserId == userID).OrderBy(w => w.Name).ToList();
            ViewBag.UserID = userID;
            return View(websites);
        }

        [HttpGet]
        [Route("api/WebPings")]
        public IActionResult GetWebPings(string userId)
        {
            var webPings = _context.WebPing
                    .Include(ping => ping.Website)
                    .Where(ping => ping.Website.UserId == userId)
                    .GroupBy(ping => ping.Website)
                    .Select(group => group.OrderByDescending(ping => ping.Timestamp).FirstOrDefault())
                    .ToList();


            var pingData = webPings.Select(ping => new
            {
                WebsiteName = ping.Website.Name,
                Timestamp = ping.Timestamp,
                IsUp = ping.isUp,
                ResponseTime = ping.ResponseTime,
                StatusCode = ping.StatusCode
            });

            return Ok(pingData);
        }
    }
}
