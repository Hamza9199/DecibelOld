using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Decibel.Data;
using Decibel.Dto;
using Decibel.Models;
using Decibel.Services;
using Decibel.ViewModels;
using Microsoft.Identity.Client;
using NuGet.Common;
using System.ComponentModel;
using Microsoft.IdentityModel.Tokens;
using NuGet.ProjectModel;
using static Dropbox.Api.Files.ListRevisionsMode;

namespace Decibel.Controllers
{
        public class PjesmaController : Controller
        {
                private readonly ApplicationDbContext _context;
                private readonly FirebaseService _firebaseService;

                public PjesmaController(ApplicationDbContext context, FirebaseService firebaseService)
                {
                        _context = context;
                        _firebaseService = firebaseService;
                }


                [HttpGet]
                [Route("[Controller]/[Action]")]
                public async Task<IActionResult> Search(string query)
                {
                        string? korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var tip = _context.UserRoles
                                .Where(ur => ur.UserId == korisnikID);

                        bool jeIzvodjac = !tip.Where(k => k.RoleId == "2").IsNullOrEmpty();

                        bool jeAdmin = !tip.Where(k => k.RoleId == "3").IsNullOrEmpty();

                        var pjesme = _context.Pjesma
                                .Include(nu => nu.Korisnik)
                                .Include(p => p.Album);

                        var ret = Enumerable.Empty<Pjesma>().AsQueryable();

                        if (jeIzvodjac)
                        {
                                ret = pjesme.Where(i => i.naziv.Contains(query));
                        }
                        else
                        {
                                ret = pjesme.Where(i => i.naziv.Contains(query));
                        }

                        var res = ret.Select(i => new
                        {
                                i.ID,
                                i.naziv,
                                i.putanjaSlika,
                                albumStatus = i.albumID == null
                                        ? (object)"Pjesma nije dio Albuma."
                                        : new { albumName = i.Album.naziv, albumLink = Url.Action("Details", "Album", new { id = i.albumID }) }

                        })
                                .Take(8)
                                .ToList();

                        return Json(res);
                }





                [HttpGet("[controller]/[action]/{ID}")]
                public async Task<IActionResult> GetPutanjaAudio(long ID)
                {
                        var pjesma = await _context.Pjesma.FindAsync(ID);

                        if (pjesma == null)
                        {
                                return NotFound();
                        }

                        return Json(new { url = pjesma.putanjaAudio });
                }

                [HttpGet]
                [Route("[Controller]/[Action]")]
                public async Task<IActionResult> Player(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var pjesma = await _context.Pjesma
                                .Include(p => p.Album)
                                .FirstOrDefaultAsync(m => m.ID == id);
                        if (pjesma == null)
                        {
                                return NotFound();
                        }

                        return PartialView(pjesma);
                }

                // GET: Pjesma


                public async Task<IActionResult> Index()
                {
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
                                });

                        var pjesmeLista = ViewModelMapServis.MapirajUPjesmeViewModel(pjesmeQuery);

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
                                Pjesme = pjesmeLista,
                                PlayListe = playlisteLista
                        };

                        //return View(await applicationDbContext.ToListAsync());

                        ViewData["KontekstTip"] = KontekstPustanja.Ostalo;

                        return View(lista);
                }


                // GET: Pjesma/Lista
                public async Task<IActionResult> Lista()
                {
                        var songs = await _context.Pjesma
                                                                        .Where(doz => doz.odobreno == false)
                                                                        .ToListAsync();

                        var albumi = await _context.Album
                                                .Where(doz => doz.odobreno == false)
                                                .ToListAsync();


                        ViewData["Songs"] = songs;
                        ViewData["Albums"] = albumi;

                        return View();
                }


                // GET: Pjesma/MojaListaPjesama
                public async Task<IActionResult> MojaListaPjesama()
                {
                        var korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var korisnikovePjesmeIds = await _context.IzvodjacPjesma
                                                   .Where(ip => ip.izvodjacID == korisnikID)
                                                   .Select(ip => ip.pjesmaID)
                                                   .ToListAsync();

                        var songs = await _context.Pjesma
                                                   .Where(p => korisnikovePjesmeIds.Contains(p.ID))
                                                   .ToListAsync();


                        ViewData["Songs"] = songs;

                        return View();
                }

                [HttpGet("[controller]/[action]/{id}")]
                public async Task<IActionResult> GetPjesmaViewModel(long? id)
                {
                        if (id == null)
                        {
                                Console.WriteLine("ID je null");
                                return NotFound();
                        }

                        var korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var pjesmeQuery = _context.Pjesma
                                .Include(p => p.Album)
                                .Include(p => p.Korisnik)
                                .Include(p => p.IzvodjacPjesma)
                                .ThenInclude(ip => ip.Izvodjac)
                                .Select(kp => new PjesmaDto
                                {
                                        Pjesma = kp,
                                        prev = 30,
                                        next = 30,
                                        korisnikIme = kp.Korisnik.korisnickoIme,
                                        DodanDatumVrijeme = kp.kreiranDatumVrijeme,
                                        lajkovana = _context.KorisnikPjesma
                                                .Any(kpRel => kpRel.korisnikID == korisnikID && kpRel.pjesmaID == kp.ID)
                                })
                                .Where(p => p.Pjesma.ID == id);


                        var pjesma = ViewModelMapServis.MapirajUPjesmeViewModel(pjesmeQuery);

                        return Json(pjesma.FirstOrDefault());
                }

                // GET: Pjesma/Details/5
                [HttpGet]
                [Route("[Controller]/[Action]/{id}")]
                public async Task<IActionResult> Details(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        // Optimize the query to filter by the ID before projecting
                        var pjesmaQuery = _context.Pjesma
                            .Where(kp => kp.ID == id) // Filter early to minimize data load
                            .Include(p => p.Album)
                            .Include(p => p.IzvodjacPjesma)
                                .ThenInclude(ip => ip.Izvodjac)
                            .Select(kp => new PjesmaDto
                            {
                                    Pjesma = kp,
                                    DodanDatumVrijeme = kp.kreiranDatumVrijeme,
                                    lajkovana = _context.KorisnikPjesma
                                    .Any(kpRel => kpRel.korisnikID == korisnikID && kpRel.pjesmaID == kp.ID)
                            });

                        var pjesma = await pjesmaQuery.SingleOrDefaultAsync(); // Fetch a single song

                        if (pjesma == null)
                        {
                                return NotFound();
                        }

                        var _pjesma = ViewModelMapServis.MapirajUPjesmeViewModel(pjesma);

                        return View(_pjesma);
                }


                // GET: Pjesma/Create
                public async Task<IActionResult> Create()
                {
                        ViewData["albumID"] = new SelectList(_context.Album, "ID", "ID");

                        var zanrovi = await _context.Zanr
                        .ToListAsync();

                        ViewData["Zanr"] = zanrovi;



                        return View();
                }



                // POST: Pjesma/Create
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Create([Bind("ID,albumID,redniBrojUAlbumu,naziv,opis,datumObjave,trajanjeSekunde,javno,odobreno,licenca,eksplicitniSadrzaj,kreiranDatumVrijeme,AudioFile,ImageFile,GifFile,IzvodjaciIDs,ZanrIDs")] Pjesma pjesma)
                {
                        string? korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        if (!ModelState.IsValid || korisnikID == null)
                        {
                                Console.WriteLine("ModelState nije validan.");
                                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                                {
                                        Console.WriteLine($"Error: {error.ErrorMessage}");
                                }

                                return View(pjesma);
                        }

                        Pjesma p = await PjesmaServis.Kreiraj(korisnikID, pjesma, _firebaseService);

                        _context.Add(p);

                        await _context.SaveChangesAsync();

                        foreach (var izvodjacId in pjesma.IzvodjaciIDs)
                        {
                                var izvodjacPjesma = new IzvodjacPjesma
                                {
                                        izvodjacID = izvodjacId,
                                        pjesmaID = pjesma.ID
                                };

                                _context.Add(izvodjacPjesma);
                        }


                        foreach (var zanrId in pjesma.ZanrIDs)
                        {
                                var pjesmaZanr = new PjesmaZanr
                                {
                                        pjesmaID = p.ID,
                                        zanrID = zanrId
                                };

                                _context.PjesmaZanr.Add(pjesmaZanr);


                        }



                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                }

                // GET: Pjesma/Edit/5
                public async Task<IActionResult> Edit(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var pjesma = await _context.Pjesma.FindAsync(id);
                        if (pjesma == null)
                        {
                                return NotFound();
                        }
                        ViewData["albumID"] = new SelectList(_context.Album, "ID", "ID", pjesma.albumID);

                        var zanrovi = await _context.Zanr
                                                .ToListAsync();

                        ViewData["Zanr"] = zanrovi;

                        return View(pjesma);
                }

                // POST: Pjesma/Edit/5
                // To protect from overposting attacks, enable the specific properties you want to bind to.
                // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(long id, [Bind("ID,albumID,redniBrojUAlbumu,naziv,opis,datumObjave,trajanjeSekunde,javno,odobreno,licenca,eksplicitniSadrzaj,kreiranDatumVrijeme,AudioFile,ImageFile,GifFile,IzvodjaciIDs,ZanrIDs")] Pjesma pjesma)
                {
                        string? korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        if (id != pjesma.ID)
                        {
                                return NotFound();
                        }

                        if (!ModelState.IsValid || korisnikID == null)
                        {
                                Console.WriteLine("ModelState nije validan.");
                                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                                {
                                        Console.WriteLine($"Error: {error.ErrorMessage}");
                                }

                                return View(pjesma);
                        }

                        try
                        {

                                Pjesma p = await PjesmaServis.Kreiraj(korisnikID, pjesma, _firebaseService);

                                _context.Add(p);

                                _context.Update(p);
                                await _context.SaveChangesAsync();

                                var existingIzvodjaci = _context.IzvodjacPjesma.Where(ip => ip.pjesmaID == pjesma.ID);
                                _context.IzvodjacPjesma.RemoveRange(existingIzvodjaci);

                                var existingZanrovi = _context.PjesmaZanr.Where(pz => pz.pjesmaID == pjesma.ID);
                                _context.PjesmaZanr.RemoveRange(existingZanrovi);

                                foreach (var izvodjacId in pjesma.IzvodjaciIDs)
                                {
                                        var izvodjacPjesma = new IzvodjacPjesma
                                        {
                                                izvodjacID = izvodjacId,
                                                pjesmaID = p.ID
                                        };

                                        _context.Add(izvodjacPjesma);
                                }

                                foreach (var zanrId in pjesma.ZanrIDs)
                                {
                                        var pjesmaZanr = new PjesmaZanr
                                        {
                                                pjesmaID = p.ID,
                                                zanrID = zanrId
                                        };

                                        _context.PjesmaZanr.Add(pjesmaZanr);
                                }

                                await _context.SaveChangesAsync();

                                return RedirectToAction(nameof(Index));
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                                if (!PjesmaExists(pjesma.ID))
                                {
                                        return NotFound();
                                }
                                else
                                {
                                        throw;
                                }
                        }
                }


                // GET: Pjesma/Delete/5
                public async Task<IActionResult> Delete(long? id)
                {
                        if (id == null)
                        {
                                return NotFound();
                        }

                        var pjesma = await _context.Pjesma
                            .Include(p => p.Album)
                            .FirstOrDefaultAsync(m => m.ID == id);
                        if (pjesma == null)
                        {
                                return NotFound();
                        }

                        return View(pjesma);
                }

                // POST: Pjesma/Delete/5
                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(long id)
                {
                        var pjesma = await _context.Pjesma.FindAsync(id);
                        if (pjesma != null)
                        {
                                _context.Pjesma.Remove(pjesma);
                        }

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }

                private bool PjesmaExists(long id)
                {
                        return _context.Pjesma.Any(e => e.ID == id);
                }
        }
}
