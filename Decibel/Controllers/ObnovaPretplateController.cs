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
    public class ObnovaPretplateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ObnovaPretplateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ObnovaPretplate
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ObnovaPretplate.Include(o => o.KorisnikPretplata);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ObnovaPretplate/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obnovaPretplate = await _context.ObnovaPretplate
                .Include(o => o.KorisnikPretplata)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (obnovaPretplate == null)
            {
                return NotFound();
            }

            return View(obnovaPretplate);
        }

        // GET: ObnovaPretplate/Create
        public IActionResult Create()
        {
            ViewData["korisnikPretplataID"] = new SelectList(_context.KorisnikPretplata, "ID", "ID");
            return View();
        }

        // POST: ObnovaPretplate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,korisnikPretplataID,datumObnove,iznosObnove,kreiranDatumVrijeme")] ObnovaPretplate obnovaPretplate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(obnovaPretplate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["korisnikPretplataID"] = new SelectList(_context.KorisnikPretplata, "ID", "ID", obnovaPretplate.korisnikPretplataID);
            return View(obnovaPretplate);
        }

        // GET: ObnovaPretplate/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obnovaPretplate = await _context.ObnovaPretplate.FindAsync(id);
            if (obnovaPretplate == null)
            {
                return NotFound();
            }
            ViewData["korisnikPretplataID"] = new SelectList(_context.KorisnikPretplata, "ID", "ID", obnovaPretplate.korisnikPretplataID);
            return View(obnovaPretplate);
        }

        // POST: ObnovaPretplate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,korisnikPretplataID,datumObnove,iznosObnove,kreiranDatumVrijeme")] ObnovaPretplate obnovaPretplate)
        {
            if (id != obnovaPretplate.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(obnovaPretplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObnovaPretplateExists(obnovaPretplate.ID))
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
            ViewData["korisnikPretplataID"] = new SelectList(_context.KorisnikPretplata, "ID", "ID", obnovaPretplate.korisnikPretplataID);
            return View(obnovaPretplate);
        }

        // GET: ObnovaPretplate/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obnovaPretplate = await _context.ObnovaPretplate
                .Include(o => o.KorisnikPretplata)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (obnovaPretplate == null)
            {
                return NotFound();
            }

            return View(obnovaPretplate);
        }

        // POST: ObnovaPretplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var obnovaPretplate = await _context.ObnovaPretplate.FindAsync(id);
            if (obnovaPretplate != null)
            {
                _context.ObnovaPretplate.Remove(obnovaPretplate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObnovaPretplateExists(long id)
        {
            return _context.ObnovaPretplate.Any(e => e.ID == id);
        }
    }
}
