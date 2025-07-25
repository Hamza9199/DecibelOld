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
    public class KorisnikPlayListaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KorisnikPlayListaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KorisnikPlayLista
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.KorisnikPlayLista.Include(k => k.Korisnik).Include(k => k.PlayLista);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: KorisnikPlayLista/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnikPlayLista = await _context.KorisnikPlayLista
                .Include(k => k.Korisnik)
                .Include(k => k.PlayLista)
                .FirstOrDefaultAsync(m => m.korisnikID == id);
            if (korisnikPlayLista == null)
            {
                return NotFound();
            }

            return View(korisnikPlayLista);
        }

        // GET: KorisnikPlayLista/Create
        public IActionResult Create()
        {
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID");
            ViewData["playlistaID"] = new SelectList(_context.PlayLista, "ID", "ID");
            return View();
        }

        // POST: KorisnikPlayLista/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("korisnikID,playlistaID,kreiranDatumVrijeme")] KorisnikPlayLista korisnikPlayLista)
        {
            if (ModelState.IsValid)
            {
                _context.Add(korisnikPlayLista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", korisnikPlayLista.korisnikID);
            ViewData["playlistaID"] = new SelectList(_context.PlayLista, "ID", "ID", korisnikPlayLista.playlistaID);
            return View(korisnikPlayLista);
        }

        // GET: KorisnikPlayLista/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnikPlayLista = await _context.KorisnikPlayLista.FindAsync(id);
            if (korisnikPlayLista == null)
            {
                return NotFound();
            }
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", korisnikPlayLista.korisnikID);
            ViewData["playlistaID"] = new SelectList(_context.PlayLista, "ID", "ID", korisnikPlayLista.playlistaID);
            return View(korisnikPlayLista);
        }

        // POST: KorisnikPlayLista/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("korisnikID,playlistaID,kreiranDatumVrijeme")] KorisnikPlayLista korisnikPlayLista)
        {
            if (id != korisnikPlayLista.korisnikID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(korisnikPlayLista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisnikPlayListaExists(korisnikPlayLista.korisnikID))
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
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", korisnikPlayLista.korisnikID);
            ViewData["playlistaID"] = new SelectList(_context.PlayLista, "ID", "ID", korisnikPlayLista.playlistaID);
            return View(korisnikPlayLista);
        }

        // GET: KorisnikPlayLista/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnikPlayLista = await _context.KorisnikPlayLista
                .Include(k => k.Korisnik)
                .Include(k => k.PlayLista)
                .FirstOrDefaultAsync(m => m.korisnikID == id);
            if (korisnikPlayLista == null)
            {
                return NotFound();
            }

            return View(korisnikPlayLista);
        }

        // POST: KorisnikPlayLista/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var korisnikPlayLista = await _context.KorisnikPlayLista.FindAsync(id);
            if (korisnikPlayLista != null)
            {
                _context.KorisnikPlayLista.Remove(korisnikPlayLista);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KorisnikPlayListaExists(string id)
        {
            return _context.KorisnikPlayLista.Any(e => e.korisnikID == id);
        }
    }
}
