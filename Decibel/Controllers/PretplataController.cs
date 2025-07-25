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
    public class PretplataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PretplataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pretplata
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pretplata.ToListAsync());
        }

        // GET: Pretplata/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pretplata = await _context.Pretplata
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pretplata == null)
            {
                return NotFound();
            }

            return View(pretplata);
        }

        // GET: Pretplata/Create
        public async Task<IActionResult> Create()
        {
			var pretplate = await _context.Pretplata.ToListAsync();

			if (pretplate == null || !pretplate.Any())
			{
				ViewData["ErrorMessage"] = "Trenutno nema dostupnih pretplata.";
				return View(new List<Pretplata>());  
			}
			return View(pretplate);
		}

        // POST: Pretplata/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,naziv,opis,cijena,dostupno,kreiranDatumVrijeme")] Pretplata pretplata)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pretplata);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pretplata);
        }

		[HttpPost]
		public IActionResult Plati(long id)
		{
			var pretplata = _context.Pretplata.Find(id);
			if (pretplata == null)
			{
				return NotFound();
			}

			//var sessionUrl = _stripeService.CreateCheckoutSession(pretplata);
			return Redirect("/");
		}


		// GET: Pretplata/Edit/5
		public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pretplata = await _context.Pretplata.FindAsync(id);
            if (pretplata == null)
            {
                return NotFound();
            }
            return View(pretplata);
        }

        // POST: Pretplata/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,naziv,opis,cijena,dostupno,kreiranDatumVrijeme")] Pretplata pretplata)
        {
            if (id != pretplata.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pretplata);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PretplataExists(pretplata.ID))
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
            return View(pretplata);
        }

        // GET: Pretplata/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pretplata = await _context.Pretplata
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pretplata == null)
            {
                return NotFound();
            }

            return View(pretplata);
        }

        // POST: Pretplata/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var pretplata = await _context.Pretplata.FindAsync(id);
            if (pretplata != null)
            {
                _context.Pretplata.Remove(pretplata);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PretplataExists(long id)
        {
            return _context.Pretplata.Any(e => e.ID == id);
        }
    }
}
