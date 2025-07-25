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
    public class PratilacKorisnikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PratilacKorisnikController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PratilacKorisnik
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Pratilac.Include(p => p.Korisnik).Include(p => p.Pratilac);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PratilacKorisnik/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pratilacKorisnik = await _context.Pratilac
                .Include(p => p.Korisnik)
                .Include(p => p.Pratilac)
                .FirstOrDefaultAsync(m => m.korisnikID == id);
            if (pratilacKorisnik == null)
            {
                return NotFound();
            }

            return View(pratilacKorisnik);
        }

        // GET: PratilacKorisnik/Create
        public IActionResult Create()
        {
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID");
            ViewData["pratilacID"] = new SelectList(_context.Korisnik, "ID", "ID");
            return View();
        }

        // POST: PratilacKorisnik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("korisnikID,pratilacID,kreiranDatumVrijeme")] PratilacKorisnik pratilacKorisnik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pratilacKorisnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", pratilacKorisnik.korisnikID);
            ViewData["pratilacID"] = new SelectList(_context.Korisnik, "ID", "ID", pratilacKorisnik.pratilacID);
            return View(pratilacKorisnik);
        }

        // GET: PratilacKorisnik/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pratilacKorisnik = await _context.Pratilac.FindAsync(id);
            if (pratilacKorisnik == null)
            {
                return NotFound();
            }
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", pratilacKorisnik.korisnikID);
            ViewData["pratilacID"] = new SelectList(_context.Korisnik, "ID", "ID", pratilacKorisnik.pratilacID);
            return View(pratilacKorisnik);
        }

        // POST: PratilacKorisnik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("korisnikID,pratilacID,kreiranDatumVrijeme")] PratilacKorisnik pratilacKorisnik)
        {
            if (id != pratilacKorisnik.korisnikID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pratilacKorisnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PratilacKorisnikExists(pratilacKorisnik.korisnikID))
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
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", pratilacKorisnik.korisnikID);
            ViewData["pratilacID"] = new SelectList(_context.Korisnik, "ID", "ID", pratilacKorisnik.pratilacID);
            return View(pratilacKorisnik);
        }

        // GET: PratilacKorisnik/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pratilacKorisnik = await _context.Pratilac
                .Include(p => p.Korisnik)
                .Include(p => p.Pratilac)
                .FirstOrDefaultAsync(m => m.korisnikID == id);
            if (pratilacKorisnik == null)
            {
                return NotFound();
            }

            return View(pratilacKorisnik);
        }

        // POST: PratilacKorisnik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var pratilacKorisnik = await _context.Pratilac.FindAsync(id);
            if (pratilacKorisnik != null)
            {
                _context.Pratilac.Remove(pratilacKorisnik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PratilacKorisnikExists(string id)
        {
            return _context.Pratilac.Any(e => e.korisnikID == id);
        }
    }
}
