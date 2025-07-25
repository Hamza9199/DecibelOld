using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Decibel.Data;
using Decibel.Dto;
using Decibel.Models;
using Decibel.Services;
using Decibel.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Decibel.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class PjesmaControllerAPI : ControllerBase
        {
                private readonly ApplicationDbContext _context;

                public PjesmaControllerAPI(ApplicationDbContext context)
                {
                        _context = context;
                }

                [HttpGet]
                [Route("[action]/{query}")]
                public async Task<ActionResult> Pretraga(string query)
                {
                        string korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        var pjesme = _context.Pjesma
                                .Select(kp => new PjesmaDto
                                {
                                        Pjesma = kp,
                                        DodanDatumVrijeme = kp.kreiranDatumVrijeme,
                                        lajkovana = _context.KorisnikPjesma
                                                .Any(kpRel => kpRel.korisnikID == korisnikID && kpRel.pjesmaID == kp.ID)
                                })
                                .Where(p => p.Pjesma.naziv.Contains(query))
                                .Take(10);

                        var pjesme_ = ViewModelMapServis.MapirajUPjesmeViewModel(pjesme);

                        var korisnici = await _context.Korisnik
                                .Select(kp => new IzvodjacViewModel() 
                                {
                                        ID = kp.ID,
                                        korisnickoIme = kp.korisnickoIme,
                                        putanjaProfilneSlike = kp.putanjaProfilneSlike
                                })
                                .Where(p => p.korisnickoIme.Contains(query))
                                .Take(10)
                                .ToListAsync();

                        var playliste = await _context.PlayLista
                                .Select(kp => new PlayListaViewModel
                                {
                                        ID = kp.ID,
                                        Title = kp.naziv,
                                        TrackCount = 0,
                                        ImageUrl = kp.putanjaSlika
                                })
                                .Where(p => p.Title.Contains(query))
                                .Take(10)
                                .ToListAsync();

                        var album = await _context.Album
                                .Select(kp => new PlayListaViewModel
                                {
                                        ID = kp.ID,
                                        Title = kp.naziv,
                                        TrackCount = 0,
                                        ImageUrl = kp.putanjaSlika
                                })
                                .Where(p => p.Title.Contains(query))
                                .Take(10)
                                .ToListAsync();

                        var pretraga = new PretragaViewModel
                        {
                                pjesme = pjesme_,
                                korisnici = korisnici,
                                playliste = playliste,
                                albumi = album
                        };

                        return new JsonResult(pretraga);
                }

                // GET: api/PjesmaControllerAPI
                [HttpGet]
                public async Task<ActionResult<IEnumerable<Pjesma>>> GetPjesma()
                {
                        return await _context.Pjesma.ToListAsync();
                }

                // GET: api/PjesmaControllerAPI/5
                [HttpGet("{id}")]
                public async Task<ActionResult<Pjesma>> GetPjesma(long id)
                {
                        var pjesma = await _context.Pjesma.FindAsync(id);

                        if (pjesma == null)
                        {
                                return NotFound();
                        }

                        return pjesma;
                }

                [HttpPut("DodajPjesmuUAlbum/{pjesmaID}/{albumID}")]
                public async Task<ActionResult> DodajPjesmuUAlbum(long pjesmaID, long albumID)
                {
                        // Nova pozicija pjesme koja ce se dodati u Album
                        long? novaPozicija = 0;

                        if (pjesmaID == null || albumID == null)
                                return BadRequest();

                        // Trazi zadnju pjesmu u albumu
                        var zadnjaPjesmaUAlbumu = await _context.Pjesma.Where(k => k.albumID == albumID)
                                .OrderByDescending(k => k.redniBrojUAlbumu).FirstOrDefaultAsync();

                        // Ako ne nadje zadnju pjesmu to znaci da je album prazan,
                        // ako je prazan onda ce nova pjesma biti prva u albumu
                        if (zadnjaPjesmaUAlbumu == null)
                        {
                                novaPozicija = 1;
                        }
                        else
                        {
                                // Posto se nova pjesma dodaje na kraj, imat ce redni broj inkrementiran
                                // od zadnje pjesme
                                novaPozicija = zadnjaPjesmaUAlbumu.redniBrojUAlbumu + 1;

                                Console.WriteLine($"\n\nZadnja Pjesma: {zadnjaPjesmaUAlbumu.ID}");
                        }

                        // Trazi pjesmu koja se treba dodati u Album
                        var pjesma = await _context.Pjesma.Where(p => p.ID == pjesmaID).FirstOrDefaultAsync();

                        if (pjesma == null)
                                return BadRequest();

                        Console.WriteLine($"Pjesma: {pjesma.ID}");

                        pjesma.albumID = albumID;
                        pjesma.redniBrojUAlbumu = novaPozicija;

                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                                return BadRequest();
                        }

                        return Ok();
                }


                [HttpGet("GetIzvodjace/{id}")]
                public async Task<ActionResult> GetIzvodjace(long id)
                {
                        var izvodjaci = await _context.KorisnikPjesma
                                .Where(kp => kp.pjesmaID == id)
                                .Join(
                                        _context.Korisnik,
                                        kp => kp.korisnikID,
                                        k => k.ID,
                                        (kp, k) => new { Korisnik = k }
                                )
                                .Join(
                                        _context.Users,
                                        combined => combined.Korisnik.ID,
                                        au => au.Id,
                                        (combined, au) => new
                                        {
                                                UserId = au.Id,
                                                UserName = au.UserName,
                                                Email = au.Email
                                        }
                                )
                                .ToListAsync();

                        if (izvodjaci == null || !izvodjaci.Any())
                        {
                                return NotFound(new { message = "Izvodjaci nisu pronadjeni." });
                        }

                        return new JsonResult(new { izvodjaci });
                }


                [HttpGet("GetZanrove/{id}")]
                public async Task<ActionResult> GetZanrove(long id)
                {
                        var zanrovi = await _context.PjesmaZanr
                                .Where(pz => pz.pjesmaID == id)
                                .Join(
                                        _context.Zanr,
                                        pz => pz.zanrID,
                                        z => z.ID,
                                        (pz, z) => new { Zanr = z }
                                )
                                .Select(result => result.Zanr)
                                .ToListAsync();

                        if (!zanrovi.Any())
                        {
                                return NotFound(new { message = "Zanrovi nisu pronadjeni." });
                        }

                        return new JsonResult(new { zanrovi });
                }

                // PUT: api/PjesmaControllerAPI/5
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPut("{id}")]
                public async Task<IActionResult> PutPjesma(long id, Pjesma pjesma)
                {
                        if (id != pjesma.ID)
                        {
                                return BadRequest();
                        }

                        _context.Entry(pjesma).State = EntityState.Modified;

                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                                if (!PjesmaExists(id))
                                {
                                        return NotFound();
                                }
                                else
                                {
                                        throw;
                                }
                        }

                        return NoContent();
                }

                [HttpPut("PutPjesmaAPI/{id}")]
                public async Task<IActionResult> PutPjesma(long id, PjesmaAPI pjesmaDto)
                {
                        if (id != pjesmaDto.ID)
                        {
                                return BadRequest("The ID in the URL does not match the ID in the request body.");
                        }

                        var existingPjesma = await _context.Pjesma.FindAsync(id);
                        if (existingPjesma == null)
                        {
                                return NotFound("Pjesma not found.");
                        }

                        existingPjesma.albumID = pjesmaDto.albumID;
                        existingPjesma.redniBrojUAlbumu = pjesmaDto.redniBrojUAlbumu;
                        existingPjesma.naziv = pjesmaDto.naziv;
                        existingPjesma.opis = pjesmaDto.opis;
                        existingPjesma.datumObjave = pjesmaDto.datumObjave;
                        existingPjesma.trajanjeSekunde = pjesmaDto.trajanjeSekunde;
                        existingPjesma.javno = pjesmaDto.javno;
                        existingPjesma.odobreno = pjesmaDto.odobreno;
                        existingPjesma.putanjaAudio = pjesmaDto.putanjaAudio;
                        existingPjesma.putanjaSlika = pjesmaDto.putanjaSlika;
                        existingPjesma.putanjaGif = pjesmaDto.putanjaGif;
                        existingPjesma.brojReprodukcija = pjesmaDto.brojReprodukcija;
                        existingPjesma.brojLajkova = pjesmaDto.brojLajkova;
                        existingPjesma.jezikPjesme = pjesmaDto.jezikPjesme;
                        existingPjesma.licenca = pjesmaDto.licenca;
                        existingPjesma.eksplicitniSadrzaj = pjesmaDto.eksplicitniSadrzaj;
                        existingPjesma.tekst = pjesmaDto.tekst;
                        existingPjesma.kreiranDatumVrijeme = pjesmaDto.kreiranDatumVrijeme;

                        _context.Entry(existingPjesma).State = EntityState.Modified;

                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                                if (!PjesmaExists(id))
                                {
                                        return NotFound();
                                }
                                else
                                {
                                        throw;
                                }
                        }

                        return NoContent();
                }


                // POST: api/PjesmaControllerAPI
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
                public async Task<ActionResult<Pjesma>> PostPjesma(Pjesma pjesma)
                {
                        _context.Pjesma.Add(pjesma);
                        await _context.SaveChangesAsync();

                        return CreatedAtAction("GetPjesma", new { id = pjesma.ID }, pjesma);
                }

                [HttpPost("PostPjesmaAPI")]
                public async Task<ActionResult<Pjesma>> PostPjesmaAPI(PjesmaAPI pjesmaDto)
                {
                        var pjesma = new Pjesma
                        {
                                albumID = pjesmaDto.albumID,
                                redniBrojUAlbumu = pjesmaDto.redniBrojUAlbumu,
                                naziv = pjesmaDto.naziv,
                                opis = pjesmaDto.opis,
                                datumObjave = pjesmaDto.datumObjave,
                                trajanjeSekunde = pjesmaDto.trajanjeSekunde,
                                javno = pjesmaDto.javno,
                                odobreno = pjesmaDto.odobreno,
                                putanjaAudio = pjesmaDto.putanjaAudio,
                                putanjaSlika = pjesmaDto.putanjaSlika,
                                putanjaGif = pjesmaDto.putanjaGif,
                                brojReprodukcija = pjesmaDto.brojReprodukcija,
                                brojLajkova = pjesmaDto.brojLajkova,
                                jezikPjesme = pjesmaDto.jezikPjesme,
                                licenca = pjesmaDto.licenca,
                                eksplicitniSadrzaj = pjesmaDto.eksplicitniSadrzaj,
                                tekst = pjesmaDto.tekst,
                                kreiranDatumVrijeme = pjesmaDto.kreiranDatumVrijeme
                        };

                        _context.Pjesma.Add(pjesma);
                        await _context.SaveChangesAsync();

                        return CreatedAtAction("GetPjesma", new { id = pjesma.ID }, pjesma);
                }


                // DELETE: api/PjesmaControllerAPI/5
                [HttpDelete("{id}")]
                public async Task<IActionResult> DeletePjesma(long id)
                {
                        var pjesma = await _context.Pjesma.FindAsync(id);
                        if (pjesma == null)
                        {
                                return NotFound();
                        }

                        _context.Pjesma.Remove(pjesma);
                        await _context.SaveChangesAsync();

                        return NoContent();
                }


                [HttpPost("dozvoli/{id}")]
                public async Task<IActionResult> DozvoliAsync(long id)
                {
                        try
                        {
                                var zahtjev = await _context.Pjesma.FindAsync(id);
                                if (zahtjev == null)
                                {
                                        return NotFound(new { message = "Zahtjev nije pronađen." });
                                }

                                zahtjev.odobreno = true;

                                _context.Pjesma.Update(zahtjev);

                                await _context.SaveChangesAsync();

                                return Ok(new { message = "Zahtjev je uspješno odobren." });
                        }
                        catch (Exception ex)
                        {
                                return StatusCode(500, new { message = "Interna greška servera", error = ex.Message });
                        }
                }


                [HttpPost("odbij/{id}")]
                public async Task<IActionResult> OdbijAsync(long id)
                {
                        try
                        {
                                var zahtjev = await _context.Pjesma.FindAsync(id);
                                if (zahtjev == null)
                                {
                                        return NotFound(new { message = "Zahtjev nije pronađen." });
                                }

                                //zahtjev.odobreno = false;
                                //_context.Pjesma.Remove(zahtjev);


                                await _context.SaveChangesAsync();

                                return Ok(new { message = "Zahtjev je uspješno odbijen.", zahtjev });
                        }
                        catch (Exception ex)
                        {
                                return StatusCode(500, new { message = "Interna greška servera", error = ex.Message });
                        }
                }

		[HttpPost("obrisi/{id}")]
		public async Task<IActionResult> Obrisi(long id)
		{
			try
			{
				var zahtjev = await _context.Pjesma.FindAsync(id);
				if (zahtjev == null)
				{
					return NotFound(new { message = "Zahtjev nije pronađen." });
				}

				//zahtjev.odobreno = false;
				_context.Pjesma.Remove(zahtjev);


				await _context.SaveChangesAsync();

				return Ok(new { message = "Zahtjev je uspješno odbijen.", zahtjev });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Interna greška servera", error = ex.Message });
			}
		}


		private bool PjesmaExists(long id)
                    {
                            return _context.Pjesma.Any(e => e.ID == id);
                    }
            }
}
