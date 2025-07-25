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
        public class AlbumController : Controller
        {
                private readonly ApplicationDbContext _context;
                private readonly FirebaseService _firebaseService;

                public AlbumController(ApplicationDbContext context, FirebaseService firebaseService)
                {
                        _context = context;
                        _firebaseService = firebaseService;
                }

                // GET: Album
                public async Task<IActionResult> Index()
                {
                        var albumi = await _context.Album
								.Where(a => a.odobreno == true)
								.Select(album => new PlayListaViewModel
                                {
                                        ID = album.ID,
                                        ImageUrl = album.putanjaSlika,
                                        Title = album.naziv,
                                        TrackCount = 0,
                                        Izvodjaci = album.Pjesma
                                                .SelectMany(pjesma => pjesma.IzvodjacPjesma)
                                                .Select(ip => ip.Izvodjac)
                                                .Distinct()
                                                .Select(izvodjac => new IzvodjacViewModel
                                                {
                                                        ID = izvodjac.ID,
                                                        korisnickoIme = izvodjac.korisnickoIme,
                                                        putanjaProfilneSlike = izvodjac.putanjaProfilneSlike
                                                })
                                                .ToList()
                                })
                                .ToListAsync();

                        ViewData["Albumi"] = albumi;

                        return View();
                }


                // GET: Album/Details/5
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

                        var playliste = _context.PlayLista.Where(p => p.korisnikID == korisnikID).ToList();

                        var playlisteLista = playliste.Select(playlista => new PlayListaViewModel
                        {
                                ID = playlista.ID,
                                ImageUrl = playlista.putanjaSlika,
                                Title = playlista.naziv,
                                TrackCount = 0
                        }).ToList();

                        var lista = new PjesmaListaViewModel
                        {
                                Pjesme = album,
                                PlayListe = playlisteLista,
                                ID = id
                        };

                        if (lista == null)
                        {
                                return NotFound();
                        }

                        ViewData["KontekstTip"] = KontekstPustanja.Album;

                        return View(lista);
                }

                // GET: Album/Create
                // GET: Album/Create
                public IActionResult Create()
                {
                        ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID");
                        return View();
                }

                // POST: Album/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("ID,korisnikID,naziv,opis,kreiranDatumVrijeme,javno,putanjaSlika,putanjaGif,PjesmeIDs,ImageFile,GifFile,")] Album album)
                {
                        string? korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        if (!ModelState.IsValid || korisnikID == null)
                        {
                                Console.WriteLine("ModelState nije validan.");
                                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                                {
                                        Console.WriteLine($"Error: {error.ErrorMessage}");
                                }

                                return View(album);
                        }

                        Album a = await PjesmaServis.KreirajAlbum(korisnikID, album, _firebaseService);

                        a.korisnikID = korisnikID;

                        _context.Add(a);

                        await _context.SaveChangesAsync();

                        foreach (var pjesmaID in a.PjesmeIDs)
                        {
                                var pjesma = await _context.Pjesma
                                        .Where(p => p.ID == pjesmaID).FirstOrDefaultAsync();

                                pjesma.albumID = a.ID;

                                _context.Update(pjesma);
                        }

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                }

                // GET: Album/MojaListaAlbuma
                public async Task<IActionResult> MojaListaAlbuma()
                {
                        var korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var songs = await _context.Album
                                                   .Where(p => p.korisnikID == korisnikID)
                                                   .ToListAsync();

                        ViewData["Songs"] = songs;

                        return View();
                }


                // GET: Album/Edit/5
                public async Task<IActionResult> Edit(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var album = await _context.Album.FindAsync(id);
                        if (album == null)
                        {
                                return NotFound();
                        }
                        ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", album.korisnikID);
                        return View(album);
                }

                // POST: Album/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(long id, [Bind("ID,korisnikID,naziv,opis,kreiranDatumVrijeme,odobreno,javno,brojLajkova,putanjaSlika,putanjaGif")] Album album)
                {
                        if (id != album.ID)
                        {
                                return NotFound();
                        }

                        if (ModelState.IsValid)
                        {
                                try
                                {
                                        _context.Update(album);
                                        await _context.SaveChangesAsync();
                                }
                                catch (DbUpdateConcurrencyException)
                                {
                                        if (!AlbumExists(album.ID))
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
                        ViewData["korisnikID"] = new SelectList(_context.Korisnik, "ID", "ID", album.korisnikID);
                        return View(album);
                }

                // GET: Album/Delete/5
                public async Task<IActionResult> Delete(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var album = await _context.Album
                            .Include(a => a.Korisnik)
                            .FirstOrDefaultAsync(m => m.ID == id);
                        if (album == null)
                        {
                                return NotFound();
                        }

                        return View(album);
                }

                // POST: Album/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(long id)
                {
                        var album = await _context.Album.FindAsync(id);
                        if (album != null)
                        {
                                _context.Album.Remove(album);
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }

                private bool AlbumExists(long id)
                {
                        return _context.Album.Any(e => e.ID == id);
                }
        }
}
