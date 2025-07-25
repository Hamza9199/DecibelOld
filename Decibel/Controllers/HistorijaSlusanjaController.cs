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
        public class HistorijaSlusanjaController : Controller
        {
                private readonly ApplicationDbContext _context;

                public HistorijaSlusanjaController(ApplicationDbContext context)
                {
                        _context = context;
                }

                // GET: HistorijaSlusanja
                public async Task<IActionResult> Index()
                {
                        string korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var historijaQuery = _context.HistorijaSlusanja
                            .Where(hs => hs.korisnikID == korisnikID)
                            .Join(
                                _context.Pjesma,
                                hs => hs.pjesmaID,
                                p => p.ID,
                                (hs, p) => new
                                {
                                        Pjesma = p,
                                        Historija = hs,
                                }
                            );

                        var groupedData = historijaQuery
                            .AsEnumerable() 
                            .GroupBy(dto => dto.Pjesma.ID)
                            .Select(g => g
                                .OrderByDescending(dto => dto.Historija.kreiranDatumVrijeme)
                                .First()
                            );

                        var enrichedData = groupedData.Select(dto => new PjesmaDto
                        {
                                Pjesma = _context.Pjesma
                                .Include(p => p.Album)
                                .Include(p => p.IzvodjacPjesma)
                                    .ThenInclude(ip => ip.Izvodjac)
                                .First(p => p.ID == dto.Pjesma.ID),
                                DodanDatumVrijeme = dto.Historija.kreiranDatumVrijeme,
                                context = dto.Historija.kontekstPustanja,
                                contextURL = dto.Historija.kontekstPustanjaURL,
                                lajkovana = _context.KorisnikPjesma
                                .Any(kp => kp.korisnikID == korisnikID && kp.pjesmaID == dto.Pjesma.ID)
                        });

                        var query = enrichedData
                            .OrderByDescending(dto => dto.DodanDatumVrijeme)
                            .AsQueryable();

                        var playliste = await _context.PlayLista
                            .Where(p => p.korisnikID == korisnikID)
                            .Select(playlista => new PlayListaViewModel
                            {
                                    ID = playlista.ID,
                                    ImageUrl = playlista.putanjaSlika,
                                    Title = playlista.naziv,
                                    TrackCount = 0
                            })
                            .ToListAsync();

                        var VM = ViewModelMapServis.MapirajUPjesmeViewModel(query);

                        ViewData["TipListe"] = "Historija";

                        ViewData["KontekstTip"] = KontekstPustanja.Historija;

                        return View(new PjesmaListaViewModel { Pjesme = VM, PlayListe = playliste });
                }

                [HttpPost]
                [Route("[controller]/[action]")]
                public async Task<IActionResult> PushSesija([FromBody] RecordPlayModel model)
                {
                        if (model == null || model.PjesmaID <= 0)
                        {
                                return BadRequest(ModelState);
                        }

                        string korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        if (string.IsNullOrEmpty(korisnikID))
                        {
                                return Unauthorized();
                        }

                        var newRecord = new HistorijaSlusanja
                        {
                                korisnikID = korisnikID,
                                pjesmaID = model.PjesmaID,
                                playlistaID = model.PlaylistaID,
                                trajanje = model.Trajanje,
                                kontekstPustanjaURL = model.KontekstPustanjaURL,
                                kontekstPustanja = (KontekstPustanja)Enum.Parse(typeof(KontekstPustanja), model.KontekstPustanja, true),
                                offline = model.Offline
                        };
                        
                        _context.HistorijaSlusanja.Add(newRecord);

                        var pjesma = await _context.Pjesma.Where(p => p.ID == model.PjesmaID).FirstOrDefaultAsync();

                        pjesma.brojReprodukcija++;

                        _context.Update(pjesma);

                        await _context.SaveChangesAsync();

                        return Ok(new { message = "Historija pustanja (Sesija) sacuvana uspjesno." });
                }

                public class RecordPlayModel
                {
                        public long PjesmaID { get; set; }
                        public long? PlaylistaID { get; set; }
                        public int Trajanje { get; set; }
                        public string KontekstPustanjaURL { get; set; }
                        public string KontekstPustanja { get; set; }
                        public bool Offline { get; set; }
                }


                // GET: HistorijaSlusanja/Details/5
                public async Task<IActionResult> Details(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var historijaSlusanja = await _context.HistorijaSlusanja
                            .FirstOrDefaultAsync(m => m.ID == id);
                        if (historijaSlusanja == null)
                        {
                                return NotFound();
                        }

                        return View(historijaSlusanja);
                }

                // GET: HistorijaSlusanja/Create
                public IActionResult Create()
                {
                        return View();
                }

                // POST: HistorijaSlusanja/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("ID,korisnikID,pjesmaID,playlistaID,kreiranDatumVrijeme,trajanje,kontekstPustanja,offline")] HistorijaSlusanja historijaSlusanja)
                {
                        if (ModelState.IsValid)
                        {
                                _context.Add(historijaSlusanja);
                                await _context.SaveChangesAsync();
                                return RedirectToAction(nameof(Index));
                        }
                        return View(historijaSlusanja);
                }

                // GET: HistorijaSlusanja/Edit/5
                public async Task<IActionResult> Edit(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var historijaSlusanja = await _context.HistorijaSlusanja.FindAsync(id);
                        if (historijaSlusanja == null)
                        {
                                return NotFound();
                        }
                        return View(historijaSlusanja);
                }

                // POST: HistorijaSlusanja/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(long id, [Bind("ID,korisnikID,pjesmaID,playlistaID,kreiranDatumVrijeme,trajanje,kontekstPustanja,offline")] HistorijaSlusanja historijaSlusanja)
                {
                        if (id != historijaSlusanja.ID)
                        {
                                return NotFound();
                        }

                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        _context.Update(historijaSlusanja);
                                        await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                        if (!HistorijaSlusanjaExists(historijaSlusanja.ID))
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
                        return View(historijaSlusanja);
                }

                // GET: HistorijaSlusanja/Delete/5
                public async Task<IActionResult> Delete(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var historijaSlusanja = await _context.HistorijaSlusanja
                            .FirstOrDefaultAsync(m => m.ID == id);
                        if (historijaSlusanja == null)
                        {
                                return NotFound();
                        }

                        return View(historijaSlusanja);
                }

                // POST: HistorijaSlusanja/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(long id)
                {
                        var historijaSlusanja = await _context.HistorijaSlusanja.FindAsync(id);
                        if (historijaSlusanja != null)
                        {
                                _context.HistorijaSlusanja.Remove(historijaSlusanja);
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }

                private bool HistorijaSlusanjaExists(long id)
                {
                        return _context.HistorijaSlusanja.Any(e => e.ID == id);
                }
        }
}
