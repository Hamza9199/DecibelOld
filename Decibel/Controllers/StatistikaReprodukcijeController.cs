using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Decibel.Data;
using Decibel.Models;
using Decibel.ViewModels;

namespace Decibel.Controllers
{
        public class StatistikaReprodukcijeController : Controller
        {
                private readonly ApplicationDbContext _context;

                public StatistikaReprodukcijeController(ApplicationDbContext context)
                {
                        _context = context;
                }

                // GET: StatistikaReprodukcije
                public async Task<IActionResult> Index()
                {

                        string korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var izvodaci = await _context.UserRoles
                                .Where(ur => ur.RoleId == "3" && ur.UserId == korisnikID).FirstOrDefaultAsync();

                        if (izvodaci == null)
                        {
                                return Unauthorized();
                        }

                        /*
                        const songData = [
                            { song: "Song A", streams: 5000 },
                            { song: "Song B", streams: 4500 },
                            { song: "Song C", streams: 4000 },
                            { song: "Song D", streams: 3500 },
                            { song: "Song E", streams: 3000 },
                            ];
                        */

                        var pjesme = await _context.StatistikaReprodukcije
                                .Join(
                                        _context.Pjesma,
                                        sr => sr.pjesmaID,
                                        p => p.ID,
                                        (sr, p) => new StatsViewModel()
                                        {
                                                Pjesma = p,
                                                Stat = sr
                                        })
                                .OrderByDescending(p => p.Stat.brojReprodukcija)
                                .Take(10)
                                .ToListAsync();

                        var lajkovi = await _context.Pjesma
                                .OrderByDescending(p => p.brojLajkova)
                                .Take(10)
                                .ToListAsync();

                        var pratioci = await _context.Korisnik
                                .OrderByDescending(p => p.brojPratilaca)
                                .Take(10)
                                .ToListAsync();

                        ViewData["Reprodukcija"] = pjesme;
                        ViewData["Lajkove"] = lajkovi;
                        ViewData["Pratioci"] = pratioci;

                        return View();
                }

                // GET: StatistikaReprodukcije/Details/5
                public async Task<IActionResult> Details(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var statistikaReprodukcije = await _context.StatistikaReprodukcije
                            .FirstOrDefaultAsync(m => m.ID == id);
                        if (statistikaReprodukcije == null)
                        {
                                return NotFound();
                        }

                        return View(statistikaReprodukcije);
                }

                // GET: StatistikaReprodukcije/Create
                public IActionResult Create()
                {
                        return View();
                }

                // POST: StatistikaReprodukcije/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("ID,korisnikID,pjesmaID,brojReprodukcija,ukupnoTrajanje,zadnjePustanje")] StatistikaReprodukcije statistikaReprodukcije)
                {
                        if (ModelState.IsValid)
                        {
                                _context.Add(statistikaReprodukcije);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                        }
                        return View(statistikaReprodukcije);
                }

                // GET: StatistikaReprodukcije/Edit/5
                public async Task<IActionResult> Edit(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var statistikaReprodukcije = await _context.StatistikaReprodukcije.FindAsync(id);
                        if (statistikaReprodukcije == null)
                        {
                                return NotFound();
                        }
                        return View(statistikaReprodukcije);
                }

                // POST: StatistikaReprodukcije/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(long id, [Bind("ID,korisnikID,pjesmaID,brojReprodukcija,ukupnoTrajanje,zadnjePustanje")] StatistikaReprodukcije statistikaReprodukcije)
                {
                        if (id != statistikaReprodukcije.ID)
                        {
                                return NotFound();
                        }

                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        _context.Update(statistikaReprodukcije);
                                        await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                        if (!StatistikaReprodukcijeExists(statistikaReprodukcije.ID))
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
                        return View(statistikaReprodukcije);
                }

                // GET: StatistikaReprodukcije/Delete/5
                public async Task<IActionResult> Delete(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var statistikaReprodukcije = await _context.StatistikaReprodukcije
                            .FirstOrDefaultAsync(m => m.ID == id);
                        if (statistikaReprodukcije == null)
                        {
                                return NotFound();
                        }

                        return View(statistikaReprodukcije);
                }

                // POST: StatistikaReprodukcije/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(long id)
                {
                        var statistikaReprodukcije = await _context.StatistikaReprodukcije.FindAsync(id);
                        if (statistikaReprodukcije != null)
                        {
                                _context.StatistikaReprodukcije.Remove(statistikaReprodukcije);
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }

                private bool StatistikaReprodukcijeExists(long id)
                {
                        return _context.StatistikaReprodukcije.Any(e => e.ID == id);
                }
        }
}
