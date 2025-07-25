using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Decibel.Data;
using Decibel.Dto;
using Decibel.Models;
using Decibel.Services;
using Decibel.ViewModels;

namespace Decibel.Controllers
{
        public class KorisnikPjesmaController : Controller
        {
                private readonly ApplicationDbContext _context;

                public KorisnikPjesmaController(ApplicationDbContext context)
                {
                        _context = context;
                }

                [HttpGet]
                public async Task<IActionResult> DodajPjesmuUOmiljene(int id, string returnUrl)
                {
                        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        if (userId == null)
                        {
                                return Unauthorized();
                        }

                        var existingLike = _context.KorisnikPjesma
                                .FirstOrDefault(kp => kp.korisnikID == userId && kp.pjesmaID == id);

                        if (existingLike != null)
                        {
                                return BadRequest("Song is already liked.");
                        }

                        var newLike = new KorisnikPjesma
                        {
                                korisnikID = userId,
                                pjesmaID = id,
                                kreiranDatumVrijeme = DateTime.UtcNow
                        };

                        var pjesma = await _context.Pjesma.Where(p => p.ID == id).FirstOrDefaultAsync();
                        pjesma.brojLajkova++;

                        _context.Update(pjesma);

                        _context.KorisnikPjesma.Add(newLike);
                        _context.SaveChanges();

                        return !string.IsNullOrEmpty(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index", "Pjesma");
                }

                [HttpGet]
                public async Task<IActionResult> ObrisiPjesmuIzOmiljenih(int id, string returnUrl)
                {
                        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        if (userId == null)
                        {
                                return Unauthorized();
                        }

                        var existingLike = _context.KorisnikPjesma
                                .FirstOrDefault(kp => kp.korisnikID == userId && kp.pjesmaID == id);

                        if (existingLike == null)
                        {
                                return BadRequest("Song is not liked.");
                        }

                        var pjesma = await _context.Pjesma.Where(p => p.ID == id).FirstOrDefaultAsync();

                        if (pjesma.brojLajkova > 0)
                        {
                                pjesma.brojLajkova--;

                                _context.Update(pjesma);
                        }

                        _context.KorisnikPjesma.Remove(existingLike);
                        _context.SaveChanges();

                        return !string.IsNullOrEmpty(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index", "Pjesma");
                }


                // GET: KorisnikPjesma
                public async Task<IActionResult> Index()
                {
                        // Dobavlja trenutnog korisnika
                        string? korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        if (korisnikID == null)
                                return null;

                        var pjesme = _context.KorisnikPjesma
                                .Where(kp => kp.korisnikID == korisnikID)
                                .OrderByDescending(kp => kp.kreiranDatumVrijeme)
                                .Select(kp => new PjesmaDto
                                {
                                        Pjesma = kp.Pjesma,
                                        DodanDatumVrijeme = kp.kreiranDatumVrijeme,
                                        lajkovana = true
                                });


                        Console.WriteLine("\n");
                        foreach (var p in pjesme)
                        {
                                Console.WriteLine(p.Pjesma.naziv);
                                Console.WriteLine(p.DodanDatumVrijeme);
                        }
                        Console.WriteLine("\n");

                        var playLista = ViewModelMapServis.MapirajUPjesmeViewModel(pjesme);

                        var lista = new PjesmaListaViewModel
                        {
                                Pjesme = playLista
                        };

                        if (lista == null)
                        {
                                return NotFound();
                        }

                        ViewData["KontekstTip"] = KontekstPustanja.Omiljene;

                        return View(lista);
                }

                // GET: KorisnikPjesma/Details/5
                public async Task<IActionResult> Details(string id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var korisnikPjesma = await _context.KorisnikPjesma
                            .Include(k => k.Korisnik)
                            .Include(k => k.Pjesma)
                            .FirstOrDefaultAsync(m => m.korisnikID == id);
                        if (korisnikPjesma == null)
                        {
                                return NotFound();
                        }

                        return View(korisnikPjesma);
                }


                // GET: KorisnikPjesma/Create
                public IActionResult Create()
                {
                        ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID");
                        ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID");
                        return View();
                }

                // POST: KorisnikPjesma/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("korisnikID,pjesmaID,kreiranDatumVrijeme")] KorisnikPjesma korisnikPjesma)
                {
                        if (ModelState.IsValid)
                        {
                                _context.Add(korisnikPjesma);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                        }
                        ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", korisnikPjesma.korisnikID);
                        ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", korisnikPjesma.pjesmaID);
                        return View(korisnikPjesma);
                }

                // GET: KorisnikPjesma/Edit/5
                public async Task<IActionResult> Edit(string id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var korisnikPjesma = await _context.KorisnikPjesma.FindAsync(id);
                        if (korisnikPjesma == null)
                        {
                                return NotFound();
                        }
                        ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", korisnikPjesma.korisnikID);
                        ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", korisnikPjesma.pjesmaID);
                        return View(korisnikPjesma);
                }

                // POST: KorisnikPjesma/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(string id, [Bind("korisnikID,pjesmaID,kreiranDatumVrijeme")] KorisnikPjesma korisnikPjesma)
                {
                        if (id != korisnikPjesma.korisnikID)
                        {
                                return NotFound();
                        }

                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        _context.Update(korisnikPjesma);
                                        await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                        if (!KorisnikPjesmaExists(korisnikPjesma.korisnikID))
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
                        ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", korisnikPjesma.korisnikID);
                        ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", korisnikPjesma.pjesmaID);
                        return View(korisnikPjesma);
                }

                // GET: KorisnikPjesma/Delete/5
                public async Task<IActionResult> Delete(string id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var korisnikPjesma = await _context.KorisnikPjesma
                            .Include(k => k.Korisnik)
                            .Include(k => k.Pjesma)
                            .FirstOrDefaultAsync(m => m.korisnikID == id);
                        if (korisnikPjesma == null)
                        {
                                return NotFound();
                        }

                        return View(korisnikPjesma);
                }

                // POST: KorisnikPjesma/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(string id)
                {
                        var korisnikPjesma = await _context.KorisnikPjesma.FindAsync(id);
                        if (korisnikPjesma != null)
                        {
                                _context.KorisnikPjesma.Remove(korisnikPjesma);
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }

                private bool KorisnikPjesmaExists(string id)
                {
                        return _context.KorisnikPjesma.Any(e => e.korisnikID == id);
                }
        }
}
