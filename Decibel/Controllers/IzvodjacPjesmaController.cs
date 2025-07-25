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
    public class IzvodjacPjesmaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IzvodjacPjesmaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IzvodjacPjesma
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.IzvodjacPjesma.Include(i => i.Izvodjac).Include(i => i.Pjesma);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: IzvodjacPjesma/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izvodjacPjesma = await _context.IzvodjacPjesma
                .Include(i => i.Izvodjac)
                .Include(i => i.Pjesma)
                .FirstOrDefaultAsync(m => m.izvodjacID == id);
            if (izvodjacPjesma == null)
            {
                return NotFound();
            }

            return View(izvodjacPjesma);
        }

        // GET: IzvodjacPjesma/Create
        public IActionResult Create()
        {
            ViewData["izvodjacID"] = new SelectList(_context.Korisnik, "ID", "ID");
            ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID");
            return View();
        }

        // POST: IzvodjacPjesma/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("izvodjacID,pjesmaID,kreiranDatumVrijeme")] IzvodjacPjesma izvodjacPjesma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(izvodjacPjesma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["izvodjacID"] = new SelectList(_context.Korisnik, "ID", "ID", izvodjacPjesma.izvodjacID);
            ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", izvodjacPjesma.pjesmaID);
            return View(izvodjacPjesma);
        }

        // GET: IzvodjacPjesma/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izvodjacPjesma = await _context.IzvodjacPjesma.FindAsync(id);
            if (izvodjacPjesma == null)
            {
                return NotFound();
            }
            ViewData["izvodjacID"] = new SelectList(_context.Korisnik, "ID", "ID", izvodjacPjesma.izvodjacID);
            ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", izvodjacPjesma.pjesmaID);
            return View(izvodjacPjesma);
        }

        // POST: IzvodjacPjesma/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("izvodjacID,pjesmaID,kreiranDatumVrijeme")] IzvodjacPjesma izvodjacPjesma)
        {
            if (id != izvodjacPjesma.izvodjacID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(izvodjacPjesma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IzvodjacPjesmaExists(izvodjacPjesma.izvodjacID))
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
            ViewData["izvodjacID"] = new SelectList(_context.Korisnik, "ID", "ID", izvodjacPjesma.izvodjacID);
            ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", izvodjacPjesma.pjesmaID);
            return View(izvodjacPjesma);
        }

        // GET: IzvodjacPjesma/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izvodjacPjesma = await _context.IzvodjacPjesma
                .Include(i => i.Izvodjac)
                .Include(i => i.Pjesma)
                .FirstOrDefaultAsync(m => m.izvodjacID == id);
            if (izvodjacPjesma == null)
            {
                return NotFound();
            }

            return View(izvodjacPjesma);
        }

        // POST: IzvodjacPjesma/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var izvodjacPjesma = await _context.IzvodjacPjesma.FindAsync(id);
            if (izvodjacPjesma != null)
            {
                _context.IzvodjacPjesma.Remove(izvodjacPjesma);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IzvodjacPjesmaExists(string id)
        {
            return _context.IzvodjacPjesma.Any(e => e.izvodjacID == id);
        }
    }
}
