#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tweet.Data;
using tweet.Models;

namespace tweet.Controllers
{
    public class TweetsController : Controller
    {
        private readonly TweetDbContext _context;

        public TweetsController(TweetDbContext context)
        {
            _context = context;
        }

        // GET: Tweets
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tweets.ToListAsync());
        }

        // GET: Tweets/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tweet = await _context.Tweets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tweet == null)
            {
                return NotFound();
            }

            return View(tweet);
        }

        // GET: Tweets/Create
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tweets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Tweet tweet)
        {
            if (User.Identity.IsAuthenticated)
            {
                // ModelState.Clear();
                tweet.UserName = User.Claims.FirstOrDefault(x => x.Type.ToLower() == "name")?.Value;
                ModelState.MarkFieldValid("UserName");
            }
            else
            {
                tweet.UserName = "Unknown";
            }

            if (ModelState.IsValid)
            {
                _context.Add(tweet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tweet);
        }

        // GET: Tweets/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tweet = await _context.Tweets.FindAsync(id);
            if (tweet == null)
            {
                return NotFound();
            }
            return View(tweet);
        }

        // POST: Tweets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Tweet tweet)
        {
            if (User.Identity.IsAuthenticated)
            {
                tweet.UserName = User.Claims.FirstOrDefault(x => x.Type.ToLower() == "name")?.Value;
                // ModelState.Clear();
                ModelState.MarkFieldValid("UserName");
            }
            else
            {
                tweet.UserName = "Unknown";
            }

            if (id != tweet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tweet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TweetExists(tweet.Id))
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
            if (tweet.UserName != User.Claims.FirstOrDefault(x => x.Type.ToLower() == "name")?.Value)
            {
                return View("NotValidUser");
            }
            return View(tweet);
        }

        // GET: Tweets/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tweet = await _context.Tweets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tweet == null)
            {
                return NotFound();
            }
            if (tweet.UserName != User.Claims.FirstOrDefault(x => x.Type.ToLower() == "name")?.Value)
            {
                return View("NotValidUser");
            }

            return View(tweet);
        }

        // POST: Tweets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tweet = await _context.Tweets.FindAsync(id);
            _context.Tweets.Remove(tweet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TweetExists(int id)
        {
            return _context.Tweets.Any(e => e.Id == id);
        }
    }
}
