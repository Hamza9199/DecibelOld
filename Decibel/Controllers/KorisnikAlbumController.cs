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
    public class KorisnikAlbumController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KorisnikAlbumController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KorisnikAlbum
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.KorisnikAlbum.Include(k => k.Album).Include(k => k.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: KorisnikAlbum/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnikAlbum = await _context.KorisnikAlbum
                .Include(k => k.Album)
                .Include(k => k.Korisnik)
                .FirstOrDefaultAsync(m => m.korisnikID == id);
            if (korisnikAlbum == null)
            {
                return NotFound();
            }

            return View(korisnikAlbum);
        }

        // GET: KorisnikAlbum/Create
        public IActionResult Create()
        {
            ViewData["albumID"] = new SelectList(_context.Album, "ID", "ID");
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID");
            return View();
        }

        // POST: KorisnikAlbum/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("korisnikID,albumID,kreiranDatumVrijeme")] KorisnikAlbum korisnikAlbum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(korisnikAlbum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["albumID"] = new SelectList(_context.Album, "ID", "ID", korisnikAlbum.albumID);
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", korisnikAlbum.korisnikID);
            return View(korisnikAlbum);
        }

        // GET: KorisnikAlbum/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnikAlbum = await _context.KorisnikAlbum.FindAsync(id);
            if (korisnikAlbum == null)
            {
                return NotFound();
            }
            ViewData["albumID"] = new SelectList(_context.Album, "ID", "ID", korisnikAlbum.albumID);
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", korisnikAlbum.korisnikID);
            return View(korisnikAlbum);
        }

        // POST: KorisnikAlbum/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("korisnikID,albumID,kreiranDatumVrijeme")] KorisnikAlbum korisnikAlbum)
        {
            if (id != korisnikAlbum.korisnikID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(korisnikAlbum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisnikAlbumExists(korisnikAlbum.korisnikID))
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
            ViewData["albumID"] = new SelectList(_context.Album, "ID", "ID", korisnikAlbum.albumID);
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", korisnikAlbum.korisnikID);
            return View(korisnikAlbum);
        }

        // GET: KorisnikAlbum/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnikAlbum = await _context.KorisnikAlbum
                .Include(k => k.Album)
                .Include(k => k.Korisnik)
                .FirstOrDefaultAsync(m => m.korisnikID == id);
            if (korisnikAlbum == null)
            {
                return NotFound();
            }

            return View(korisnikAlbum);
        }

        // POST: KorisnikAlbum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var korisnikAlbum = await _context.KorisnikAlbum.FindAsync(id);
            if (korisnikAlbum != null)
            {
                _context.KorisnikAlbum.Remove(korisnikAlbum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KorisnikAlbumExists(string id)
        {
            return _context.KorisnikAlbum.Any(e => e.korisnikID == id);
        }
    }
}
