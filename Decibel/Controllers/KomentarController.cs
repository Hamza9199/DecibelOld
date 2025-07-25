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
    public class KomentarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KomentarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Komentar
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Komentar.Include(k => k.Korisnik).Include(k => k.Pjesma);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Komentar/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await _context.Komentar
                .Include(k => k.Korisnik)
                .Include(k => k.Pjesma)
                .FirstOrDefaultAsync(m => m.korisnikID == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        // GET: Komentar/Create
        public IActionResult Create()
        {
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID");
            ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID");
            return View();
        }

        // POST: Komentar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("korisnikID,pjesmaID,tekst,vrijemePjesmeSekunde,kreiranDatumVrijeme")] Komentar komentar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(komentar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", komentar.korisnikID);
            ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", komentar.pjesmaID);
            return View(komentar);
        }

		// GET: Komentar/Edit/ID
		[HttpGet]
		[Route("Komentar/Edit/{ID}")]
		public async Task<IActionResult> Edit(long ID)
		{
			if (ID == 0)
			{
				return NotFound();
			}

			var komentar = await _context.Komentar.FirstOrDefaultAsync(k => k.ID == ID);

			if (komentar == null)
			{
				return NotFound();
			}

			return View(komentar);
		}



		// POST: Komentar/Edit/ID
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(long ID, [Bind("tekst")] Komentar komentar)
		{
			if (ID == 0)
			{
				return NotFound();
			}

			var existingKomentar = await _context.Komentar.AsNoTracking().FirstOrDefaultAsync(k => k.ID == ID);

			if (existingKomentar == null)
			{
				return NotFound();
			}

			komentar.korisnikID = existingKomentar.korisnikID;
			komentar.pjesmaID = existingKomentar.pjesmaID;
			komentar.vrijemePjesmeSekunde = existingKomentar.vrijemePjesmeSekunde;
			komentar.kreiranDatumVrijeme = existingKomentar.kreiranDatumVrijeme;
			
		

			try
			{
				_context.Entry(komentar).Property(k => k.tekst).IsModified = true;
				await _context.SaveChangesAsync();
				return RedirectToAction("Details", "Pjesma", new { id = komentar.pjesmaID });

			}
			catch (DbUpdateConcurrencyException)
			{
				return NotFound();
			}
		}



		// GET: Komentar/Delete/ID
		[HttpGet]
		[Route("Komentar/Delete/{ID}")]
		public async Task<IActionResult> Delete(long ID)
		{
			if (ID == 0)
			{
				return NotFound();
			}

			var komentar = await _context.Komentar
				.Include(k => k.Korisnik)
				.Include(k => k.Pjesma)
				.FirstOrDefaultAsync(k => k.ID == ID);  

			if (komentar == null)
			{
				return NotFound();
			}

			return View(komentar);
		}



		[HttpGet]
		public async Task<IActionResult> KomentariZaPjesmu([FromQuery] long pjesmaID)
		{
			var komentari = await _context.Komentar
				.Where(k => k.pjesmaID.Equals(pjesmaID))
                                .OrderByDescending(k => k.kreiranDatumVrijeme)
				.Select(k => new
				{
					k.ID,
					k.korisnikID,
					k.pjesmaID,
					k.tekst,
					k.kreiranDatumVrijeme,
					k.vrijemePjesmeSekunde,
					korisnickoIme = _context.Korisnik
				.Where(u => u.ID == k.korisnikID)
				.Select(u => u.korisnickoIme)
				.FirstOrDefault() ?? "Nepoznati korisnik"
				})
				.ToListAsync();

			foreach (var komentar in komentari)
			{
				Console.WriteLine(komentar);
			}


			return Json(komentari);
		}


		// POST: Komentar/Delete/ID
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(long ID)
		{
			var komentar = await _context.Komentar
				.FirstOrDefaultAsync(k => k.ID == ID);

			if (komentar != null)
			{
				_context.Komentar.Remove(komentar);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction("Details", "Pjesma", new { id = komentar.pjesmaID });
		}



		private bool KomentarExists(string id)
        {
            return _context.Komentar.Any(e => e.korisnikID == id);
        }
    }
}
