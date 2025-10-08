using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SKIData.Model;

namespace SKIData.Data
{
    public class SkiResortsController : Controller
    {
        private readonly SkiResortContext _context;

        public SkiResortsController(SkiResortContext context)
        {
            _context = context;
        }

        // GET: SkiResorts
        public async Task<IActionResult> Index()
        {
            return View(await _context.SkiResorts.ToListAsync());
        }

        // GET: SkiResorts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skiResort = await _context.SkiResorts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (skiResort == null)
            {
                return NotFound();
            }

            return View(skiResort);
        }

        // GET: SkiResorts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SkiResorts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,State,Country,Condition,Latitude,Longitude,WebsiteUrl")] SkiResort skiResort)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skiResort);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(skiResort);
        }

        // GET: SkiResorts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skiResort = await _context.SkiResorts.FindAsync(id);
            if (skiResort == null)
            {
                return NotFound();
            }
            return View(skiResort);
        }

        // POST: SkiResorts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,State,Country,Condition,Latitude,Longitude,WebsiteUrl")] SkiResort skiResort)
        {
            if (id != skiResort.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(skiResort);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkiResortExists(skiResort.Id))
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
            return View(skiResort);
        }

        // GET: SkiResorts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skiResort = await _context.SkiResorts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (skiResort == null)
            {
                return NotFound();
            }

            return View(skiResort);
        }

        // POST: SkiResorts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skiResort = await _context.SkiResorts.FindAsync(id);
            if (skiResort != null)
            {
                _context.SkiResorts.Remove(skiResort);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SkiResortExists(int id)
        {
            return _context.SkiResorts.Any(e => e.Id == id);
        }
    }
}
