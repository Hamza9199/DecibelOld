using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Decibel.Data;
using Decibel.Models;

namespace Decibel.Controllers
{
    public class PjesmaZanrController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PjesmaZanrController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PjesmaZanr
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PjesmaZanr.Include(p => p.Pjesma).Include(p => p.Zanr);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PjesmaZanr/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pjesmaZanr = await _context.PjesmaZanr
                .Include(p => p.Pjesma)
                .Include(p => p.Zanr)
                .FirstOrDefaultAsync(m => m.pjesmaID == id);
            if (pjesmaZanr == null)
            {
                return NotFound();
            }

            return View(pjesmaZanr);
        }

        // GET: PjesmaZanr/Create
        public IActionResult Create()
        {
            ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID");
            ViewData["zanrID"] = new SelectList(_context.Zanr, "ID", "ID");
            return View();
        }

        // POST: PjesmaZanr/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("pjesmaID,zanrID,kreiranDatumVrijeme")] PjesmaZanr pjesmaZanr)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pjesmaZanr);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", pjesmaZanr.pjesmaID);
            ViewData["zanrID"] = new SelectList(_context.Zanr, "ID", "ID", pjesmaZanr.zanrID);
            return View(pjesmaZanr);
        }

        // GET: PjesmaZanr/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pjesmaZanr = await _context.PjesmaZanr.FindAsync(id);
            if (pjesmaZanr == null)
            {
                return NotFound();
            }
            ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", pjesmaZanr.pjesmaID);
            ViewData["zanrID"] = new SelectList(_context.Zanr, "ID", "ID", pjesmaZanr.zanrID);
            return View(pjesmaZanr);
        }

        // POST: PjesmaZanr/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("pjesmaID,zanrID,kreiranDatumVrijeme")] PjesmaZanr pjesmaZanr)
        {
            if (id != pjesmaZanr.pjesmaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pjesmaZanr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PjesmaZanrExists(pjesmaZanr.pjesmaID))
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
            ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", pjesmaZanr.pjesmaID);
            ViewData["zanrID"] = new SelectList(_context.Zanr, "ID", "ID", pjesmaZanr.zanrID);
            return View(pjesmaZanr);
        }

        // GET: PjesmaZanr/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pjesmaZanr = await _context.PjesmaZanr
                .Include(p => p.Pjesma)
                .Include(p => p.Zanr)
                .FirstOrDefaultAsync(m => m.pjesmaID == id);
            if (pjesmaZanr == null)
            {
                return NotFound();
            }

            return View(pjesmaZanr);
        }

        // POST: PjesmaZanr/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var pjesmaZanr = await _context.PjesmaZanr.FindAsync(id);
            if (pjesmaZanr != null)
            {
                _context.PjesmaZanr.Remove(pjesmaZanr);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PjesmaZanrExists(long id)
        {
            return _context.PjesmaZanr.Any(e => e.pjesmaID == id);
        }
    }
}
