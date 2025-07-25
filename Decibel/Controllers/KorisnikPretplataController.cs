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
    public class KorisnikPretplataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KorisnikPretplataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KorisnikPretplata
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.KorisnikPretplata.Include(k => k.Korisnik).Include(k => k.Pretplata);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: KorisnikPretplata/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
			var pretplata = await _context.Pretplata
                
                .ToListAsync();

			ViewData["Pretplata"] = pretplata;


			var korisnikPretplata = await _context.KorisnikPretplata
                .Include(k => k.Korisnik)
                .Include(k => k.Pretplata)
                .FirstOrDefaultAsync(m => m.korisnikID == id);
            if (korisnikPretplata == null)
            {
                return NotFound();
            }

            return View(korisnikPretplata);
        }

		// GET: KorisnikPretplata/Create
		public IActionResult Create()
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			var korisnik = _context.Korisnik.FirstOrDefault(k => k.ID == userId);

			if (korisnik == null)
			{
				return NotFound("Nema prijavljenog korisnika.");
			}
			var pretplate = _context.Pretplata.ToList();
			ViewBag.Pretplate = pretplate;
			ViewBag.KorisnikIme = korisnik.korisnickoIme;
			ViewBag.KorisnikID = korisnik.ID;

			return View();
		}



		// POST: KorisnikPretplata/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("pretplataID")] KorisnikPretplata korisnikPretplata)
		{
			// Dohvati trenutno prijavljenog korisnika
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			var korisnik = await _context.Korisnik.FirstOrDefaultAsync(k => k.ID == userId);
			if (korisnik == null)
			{
				return NotFound("Nema prijavljenog korisnika.");
			}

			korisnikPretplata.korisnikID = korisnik.ID;
			korisnikPretplata.kreiranDatumVrijeme = DateTime.Now;
			korisnikPretplata.datumVrijemeObnove = DateTime.Now.AddMonths(1);
			korisnikPretplata.datumIsteka = DateTime.Now.AddMonths(1);
			korisnikPretplata.PretplataStatus = PretplataStatusEnum.Aktivna;

            try
            {
                _context.Add(korisnikPretplata);
                await _context.SaveChangesAsync();
				return RedirectToAction("Details", new { id = korisnik.ID });
			}
			catch (Exception ex)
            {
                Console.WriteLine($"Greška: {ex.Message}");
            }

			ViewBag.Pretplate = await _context.Pretplata.ToListAsync();
			return View(korisnikPretplata);
		}


		// GET: KorisnikPretplata/Edit/5
		public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnikPretplata = await _context.KorisnikPretplata.FindAsync(id);
            if (korisnikPretplata == null)
            {
                return NotFound();
            }
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", korisnikPretplata.korisnikID);
            ViewData["pretplataID"] = new SelectList(_context.Pretplata, "ID", "ID", korisnikPretplata.pretplataID);
            return View(korisnikPretplata);
        }

        // POST: KorisnikPretplata/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,korisnikID,pretplataID,PretplataStatus,kreiranDatumVrijeme,datumVrijemeObnove,datumIsteka")] KorisnikPretplata korisnikPretplata)
        {
            if (id != korisnikPretplata.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(korisnikPretplata);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisnikPretplataExists(korisnikPretplata.ID))
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
            ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", korisnikPretplata.korisnikID);
            ViewData["pretplataID"] = new SelectList(_context.Pretplata, "ID", "ID", korisnikPretplata.pretplataID);
            return View(korisnikPretplata);
        }

        // GET: KorisnikPretplata/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var korisnikPretplata = await _context.KorisnikPretplata
                .Include(k => k.Korisnik)
                .Include(k => k.Pretplata)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (korisnikPretplata == null)
            {
                return NotFound();
            }

            return View(korisnikPretplata);
        }

        // POST: KorisnikPretplata/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var korisnikPretplata = await _context.KorisnikPretplata.FindAsync(id);
            if (korisnikPretplata != null)
            {
                _context.KorisnikPretplata.Remove(korisnikPretplata);
            }

            await _context.SaveChangesAsync();
			return RedirectToAction("Index", "Home");  
        }

		private bool KorisnikPretplataExists(long id)
        {
            return _context.KorisnikPretplata.Any(e => e.ID == id);
        }
    }
}
