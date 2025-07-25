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
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace Decibel.Controllers
{
        public class PjesmaPlayListaController : Controller
        {
                private readonly ApplicationDbContext _context;
                private readonly PjesmaPlayListaService _pjesmaPlayListaService;

                public PjesmaPlayListaController(ApplicationDbContext context, PjesmaPlayListaService pjesmaPlayListaService)
                {
                        _context = context;
                        _pjesmaPlayListaService = pjesmaPlayListaService;
                }

                [HttpPost("PromjeniRedoslijed")]
                public async Task<IActionResult> PromjeniRedoslijed([FromBody] PromjeniRedoslijedDto dto)
                {
                        try
                        {
                                await _pjesmaPlayListaService.PromjeniRedoslijed(dto.PjesmaID, dto.PocetnaPjesmaID, dto.PlaylistaID);

                                return Json(new { success = true, message = "Redoslijed Promijenjen Uspješno." });
                        }
                        catch (Exception ex)
                        {
                                return Json(new { success = false, message = ex.Message });
                        }
                }


                [HttpPost("PremjestiNaPocetak")]
                public async Task<IActionResult> PremjestiNaPocetak([FromBody] PremjestiNaPocetakDto dto)
                {
                        try
                        {
                                await _pjesmaPlayListaService.PremjestiNaPocetak(dto.PjesmaID, dto.PlaylistaID);

                                return Json(new { success = true, message = "Pjesma Premještena Na Početak." });
                        }
                        catch (Exception ex)
                        {
                                return Json(new { success = false, message = ex.Message });
                        }
                }

                [HttpGet]
                [Route("[Controller]/[Action]/{playlistaID}/{pjesmaID}")]
                public async Task<IActionResult> GetPjesmaPlayListaViewModel(int playlistaID, int pjesmaID)
                {
                        var korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var redoslijed = await _pjesmaPlayListaService.GetPlaylistOrderAsync(playlistaID);

                        var query = redoslijed
                                .Join(_context.Pjesma
                                                .Include(p => p.Album) 
                                                .Include(p => p.IzvodjacPjesma)
                                                .ThenInclude(ip => ip.Izvodjac), 
                                        r => r.pjesmaID,
                                        p => p.ID,
                                        (r, p) => new PjesmaDto
                                        {
                                                RedoslijedPjesama = r.redoslijedPjesama,
                                                Pjesma = p,
                                                next = r.pokazivacNaSljedecuPjesmuID,
                                                prev = r.pokazivacNaPrethodnuPjesmuID,
                                                DodanDatumVrijeme = p.kreiranDatumVrijeme,
                                                lajkovana = _context.KorisnikPjesma
                                                        .Any(kpRel => kpRel.korisnikID == korisnikID && kpRel.pjesmaID == p.ID)
                                        })
                                .OrderBy(r => r.RedoslijedPjesama)
                                .Where(p => p.Pjesma.ID == pjesmaID)
                                .AsQueryable();

                        var pjesma = ViewModelMapServis.MapirajUPjesmeViewModel(query);

                        return Json(pjesma.FirstOrDefault());
                }



                [HttpGet]
                [Route("[Controller]/[Action]/{ID}")]
                public async Task<IActionResult> PjesmaPlayLista(int ID)
                {
                        var korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var redoslijed = await _pjesmaPlayListaService.GetPlaylistOrderAsync(ID);

                        var query = redoslijed
                                .Join(_context.Pjesma
                                                .Include(p => p.Album) // Include the related Album
                                                .Include(p => p.IzvodjacPjesma) // Include the related IzvodjacPjesma
                                                .ThenInclude(ip => ip.Izvodjac), // Include the related Izvodjac inside IzvodjacPjesma
                                        r => r.pjesmaID,
                                        p => p.ID,
                                        (r, p) => new PjesmaDto
                                        {
                                                RedoslijedPjesama = r.redoslijedPjesama,
                                                Pjesma = p,
                                                next = r.pokazivacNaSljedecuPjesmuID,
                                                prev = r.pokazivacNaPrethodnuPjesmuID,
                                                DodanDatumVrijeme = p.kreiranDatumVrijeme,
                                                lajkovana = _context.KorisnikPjesma
                                                        .Any(kpRel => kpRel.korisnikID == korisnikID && kpRel.pjesmaID == p.ID)
                                        })
                                .OrderBy(r => r.RedoslijedPjesama)
                                .AsQueryable();

                        var playliste = _context.PlayLista.Where(p => p.korisnikID == korisnikID).ToList();

                        var playlisteLista = playliste.Select(playlista => new PlayListaViewModel
                        {
                                ID = playlista.ID,
                                ImageUrl = playlista.putanjaSlika,
                                Title = playlista.naziv,
                                TrackCount = 0
                        }).ToList();

                        var VM = ViewModelMapServis.MapirajUPjesmeViewModel(query);

                        ViewData["PlaylistaID"] = ID;
                        ViewData["Ime"] = await _context.PlayLista.Where(p => p.ID == ID).Select(p => p.naziv).FirstOrDefaultAsync();
                        ViewData["TipListe"] = "PlayLista";

                        ViewData["KontekstTip"] = KontekstPustanja.Playlista;

                        return View("_PjesmaPlayListaPartialView", new PjesmaListaViewModel { Pjesme = VM, ID = ID, PlayListe = playlisteLista});
                }


                [HttpGet]
                [Route("[Controller]/[Action]/{pjesmaID}/{playlistaID}")]
                public async Task<IActionResult> ObrisiPjesmu(int pjesmaID, int playlistaID)
                {
                        var result = _pjesmaPlayListaService.ObrisiPjesmu(pjesmaID, playlistaID);

                        await result;

                        return RedirectToAction("PjesmaPlayLista", new { ID = playlistaID });

                }

                [HttpPost]
                [Route("[Controller]/[Action]")]
                public async Task<ActionResult> DodajPjesmuNaKrajListe(int pjesmaID, int playlistaID)
                {
                        try
                        {
                                var temp = await _context.PlayLista.Where(p => p.ID == playlistaID)
                                        .Select(p => p.naziv).FirstOrDefaultAsync();

                                if (temp == null)
                                        return null;

                                string res = "Pjesma dodana na kraj playliste: " + temp;

                                await _pjesmaPlayListaService.DodajPjesmuNaKrajListe(pjesmaID, playlistaID);
                                return Ok(new { success = true, message = res });
                        }
                        catch (Exception ex)
                        {
                                Console.WriteLine(ex.Message);
                                return BadRequest(new { success = false, message = ex.Message });
                        }
                }


                // GET: PjesmaPlayLista
                public async Task<IActionResult> Index()
                {
                        var applicationDbContext = _context.PjesmaPlaylista.Include(p => p.Pjesma).Include(p => p.PlayLista).Include(p => p.prethodnaPjesma).Include(p => p.sljedecaPjesma);
                        return View(await applicationDbContext.ToListAsync());
                }

                // GET: PjesmaPlayLista/Details/5
                public async Task<IActionResult> Details(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var pjesmaPlayLista = await _context.PjesmaPlaylista
                            .Include(p => p.Pjesma)
                            .Include(p => p.PlayLista)
                            .Include(p => p.prethodnaPjesma)
                            .Include(p => p.sljedecaPjesma)
                            .FirstOrDefaultAsync(m => m.pjesmaID == id);
                        if (pjesmaPlayLista == null)
                        {
                                return NotFound();
                        }

                        return View(pjesmaPlayLista);
                }

                // GET: PjesmaPlayLista/Create
                public IActionResult Create()
                {
                        ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID");
                        ViewData["playlistaID"] = new SelectList(_context.PlayLista, "ID", "ID");
                        ViewData["pokazivacNaPrethodnuPjesmuID"] = new SelectList(_context.Pjesma, "ID", "ID");
                        ViewData["pokazivacNaSljedecuPjesmuID"] = new SelectList(_context.Pjesma, "ID", "ID");
                        return View();
                }


                // POST: PjesmaPlayLista/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("pjesmaID,playlistaID,pokazivacNaSljedecuPjesmuID,pokazivacNaPrethodnuPjesmuID,kreiranDatumVrijeme")] PjesmaPlayLista pjesmaPlayLista)
                {
                        if (ModelState.IsValid)
                        {
                                _context.Add(pjesmaPlayLista);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                        }
                        ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", pjesmaPlayLista.pjesmaID);
                        ViewData["playlistaID"] = new SelectList(_context.PlayLista, "ID", "ID", pjesmaPlayLista.playlistaID);
                        ViewData["pokazivacNaPrethodnuPjesmuID"] = new SelectList(_context.Pjesma, "ID", "ID", pjesmaPlayLista.pokazivacNaPrethodnuPjesmuID);
                        ViewData["pokazivacNaSljedecuPjesmuID"] = new SelectList(_context.Pjesma, "ID", "ID", pjesmaPlayLista.pokazivacNaSljedecuPjesmuID);
                        return View(pjesmaPlayLista);
                }

                // GET: PjesmaPlayLista/Edit/5
                public async Task<IActionResult> Edit(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var pjesmaPlayLista = await _context.PjesmaPlaylista.FindAsync(id);
                        if (pjesmaPlayLista == null)
                        {
                                return NotFound();
                        }
                        ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", pjesmaPlayLista.pjesmaID);
                        ViewData["playlistaID"] = new SelectList(_context.PlayLista, "ID", "ID", pjesmaPlayLista.playlistaID);
                        ViewData["pokazivacNaPrethodnuPjesmuID"] = new SelectList(_context.Pjesma, "ID", "ID", pjesmaPlayLista.pokazivacNaPrethodnuPjesmuID);
                        ViewData["pokazivacNaSljedecuPjesmuID"] = new SelectList(_context.Pjesma, "ID", "ID", pjesmaPlayLista.pokazivacNaSljedecuPjesmuID);
                        return View(pjesmaPlayLista);
                }

                // POST: PjesmaPlayLista/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(long id, [Bind("pjesmaID,playlistaID,pokazivacNaSljedecuPjesmuID,pokazivacNaPrethodnuPjesmuID,kreiranDatumVrijeme")] PjesmaPlayLista pjesmaPlayLista)
                {
                        if (id != pjesmaPlayLista.pjesmaID)
                        {
                                return NotFound();
                        }

                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        _context.Update(pjesmaPlayLista);
                                        await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                        if (!PjesmaPlayListaExists(pjesmaPlayLista.pjesmaID))
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
                        ViewData["pjesmaID"] = new SelectList(_context.Pjesma, "ID", "ID", pjesmaPlayLista.pjesmaID);
                        ViewData["playlistaID"] = new SelectList(_context.PlayLista, "ID", "ID", pjesmaPlayLista.playlistaID);
                        ViewData["pokazivacNaPrethodnuPjesmuID"] = new SelectList(_context.Pjesma, "ID", "ID", pjesmaPlayLista.pokazivacNaPrethodnuPjesmuID);
                        ViewData["pokazivacNaSljedecuPjesmuID"] = new SelectList(_context.Pjesma, "ID", "ID", pjesmaPlayLista.pokazivacNaSljedecuPjesmuID);
                        return View(pjesmaPlayLista);
                }

                // GET: PjesmaPlayLista/Delete/5
                public async Task<IActionResult> Delete(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var pjesmaPlayLista = await _context.PjesmaPlaylista
                            .Include(p => p.Pjesma)
                            .Include(p => p.PlayLista)
                            .Include(p => p.prethodnaPjesma)
                            .Include(p => p.sljedecaPjesma)
                            .FirstOrDefaultAsync(m => m.pjesmaID == id);
                        if (pjesmaPlayLista == null)
                        {
                                return NotFound();
                        }

                        return View(pjesmaPlayLista);
                }

                // POST: PjesmaPlayLista/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(long id)
                {
                        var pjesmaPlayLista = await _context.PjesmaPlaylista.FindAsync(id);
                        if (pjesmaPlayLista != null)
                        {
                                _context.PjesmaPlaylista.Remove(pjesmaPlayLista);
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }

                private bool PjesmaPlayListaExists(long id)
                {
                        return _context.PjesmaPlaylista.Any(e => e.pjesmaID == id);
                }
        }
}
