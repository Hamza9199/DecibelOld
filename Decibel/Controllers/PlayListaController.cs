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
using Microsoft.AspNetCore.Identity;

namespace Decibel.Controllers
{
        public class PlayListaController : Controller
        {
                private readonly ApplicationDbContext _context;
                private readonly FirebaseService _firebaseService;

                public PlayListaController(ApplicationDbContext context, FirebaseService firebaseService)
                {
                        _context = context;
                        _firebaseService = firebaseService;
                }

                // GET: PlayLista
                public async Task<IActionResult> Index()
                {
                        var korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var mojePlayliste = await _context.PlayLista
                                .Where(p => p.korisnikID == korisnikID)
                                .Select(playlista => new PlayListaViewModel
                                {
                                        ID = playlista.ID,
                                        ImageUrl = playlista.putanjaSlika,
                                        Title = playlista.naziv,
                                        TrackCount = 0 
                                })
                                .ToListAsync() ?? new List<PlayListaViewModel>();


                        var omiljenePlayliste = await _context.KorisnikPlayLista
                                .Where(kpl => kpl.korisnikID == korisnikID)
                                .Select(kpl => kpl.PlayLista)
                                .Where(playlista => playlista.korisnikID != korisnikID)
                                .Select(playlista => new PlayListaViewModel
                                {
                                        ID = playlista.ID,
                                        ImageUrl = playlista.putanjaSlika,
                                        Title = playlista.naziv,
                                        TrackCount = 0
                                })
                                .ToListAsync() ?? new List<PlayListaViewModel>();

                        ViewData["MojePlayliste"] = mojePlayliste;
                        ViewData["OmiljenePlayliste"] = omiljenePlayliste;

                        return View();
                }


                // GET: PlayLista/Details/5
                public async Task<IActionResult> Details(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var pjesmeQuery = _context.Pjesma
                                        .Include(p => p.Album)
                                        .Include(p => p.IzvodjacPjesma)
                                        .ThenInclude(ip => ip.Izvodjac)
                                        .Select(kp => new PjesmaDto
                                        {
                                                Pjesma = kp,
                                                DodanDatumVrijeme = kp.kreiranDatumVrijeme,
                                                lajkovana = _context.KorisnikPjesma
                                                        .Any(kpRel => kpRel.korisnikID == korisnikID && kpRel.pjesmaID == kp.ID)
                                        })
                                        .Where(p => p.Pjesma.albumID == id);


                        var album = ViewModelMapServis.MapirajUPjesmeViewModel(pjesmeQuery);

                        var lista = new PjesmaListaViewModel
                        {
                                Pjesme = album
                        };

                        if (lista == null)
                        {
                                return NotFound();
                        }

                        return View(lista);
                }

                // GET: PlayLista/Create
                public IActionResult Create()
                {
                        // ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID");
                        return View();
                }

                // POST: PlayLista/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("ID,naziv,opis,kreiranDatumVrijeme,javno,brojLajkova,ImageFile,GifFile")] PlayLista playLista)
                {
                        var korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        playLista.korisnikID = korisnikID;

                        if (!ModelState.IsValid)
                        {
                                Console.WriteLine("ModelState nije validan.");
                                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                                {
                                        Console.WriteLine($"Error: {error.ErrorMessage}");
                                }

                                return null;
                        }

                        if (playLista.ImageFile != null)
                        {
                                Console.WriteLine("IMAGE FILE NIJE NULL!");

                                try
                                {
                                        playLista.putanjaSlika = await _firebaseService.UploadFileAsync(
                                                playLista.ImageFile.OpenReadStream(),
                                                playLista.ImageFile.FileName,
                                                playLista.ImageFile.ContentType
                                        );
                                }
                                catch (Exception ex)
                                {
                                        Console.WriteLine($"Error tokom uploada: {ex.Message}");
                                        Console.WriteLine(ex.StackTrace);
                                        return View(playLista);
                                }
                        }

                        if (playLista.GifFile != null)
                        {
                                try
                                {
                                        playLista.putanjaGif = await _firebaseService.UploadFileAsync(
                                                playLista.GifFile.OpenReadStream(),
                                                playLista.GifFile.FileName,
                                                playLista.GifFile.ContentType
                                        );
                                }
                                catch (Exception ex)
                                {
                                        Console.WriteLine($"Error tokom uploada: {ex.Message}");
                                        Console.WriteLine(ex.StackTrace);
                                        return View(playLista);
                                }

                        }

                        _context.Add(playLista);

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                }

                // GET: PlayLista/Edit/5
                public async Task<IActionResult> Edit(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var playLista = await _context.PlayLista.FindAsync(id);
                        if (playLista == null)
                        {
                                return NotFound();
                        }
                        ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", playLista.korisnikID);
                        return View(playLista);
                }

                // POST: PlayLista/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(long id, [Bind("ID,korisnikID,naziv,opis,kreiranDatumVrijeme,javno,brojLajkova,putanjaSlika,putanjaGif")] PlayLista playLista)
                {
                        if (id != playLista.ID)
                        {
                                return NotFound();
                        }

                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        _context.Update(playLista);
                                        await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                        if (!PlayListaExists(playLista.ID))
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
                        ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", playLista.korisnikID);
                        return View(playLista);
                }

                // GET: PlayLista/Delete/5
                public async Task<IActionResult> Delete(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var playLista = await _context.PlayLista
                            .Include(p => p.Korisnik)
                            .FirstOrDefaultAsync(m => m.ID == id);
                        if (playLista == null)
                        {
                                return NotFound();
                        }

                        return View(playLista);
                }

                // POST: PlayLista/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(long id)
                {
                        var playLista = await _context.PlayLista.FindAsync(id);
                        if (playLista != null)
                        {
                                _context.PlayLista.Remove(playLista);
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }

                private bool PlayListaExists(long id)
                {
                        return _context.PlayLista.Any(e => e.ID == id);
                }
        }
}
