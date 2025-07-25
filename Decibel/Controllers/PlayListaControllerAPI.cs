using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Decibel.Data;
using Decibel.Models;

namespace Decibel.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class PlayListaControllerAPI : ControllerBase
        {
                private readonly ApplicationDbContext _context;

                public PlayListaControllerAPI(ApplicationDbContext context)
                {
                        _context = context;
                }

                // GET: api/PlayListaControllerAPI
                [HttpGet]
                public async Task<ActionResult<IEnumerable<PlayLista>>> GetPlayLista()
                {
                        return await _context.PlayLista.ToListAsync();
                }

                // GET: api/PlayListaControllerAPI/5
                [HttpGet("{id}")]
                public async Task<ActionResult<PlayLista>> GetPlayLista(long id)
                {
                        var playLista = await _context.PlayLista.FindAsync(id);

                        if (playLista == null)
                        {
                                return NotFound();
                        }

                        return playLista;
                }

                // PUT: api/PlayListaControllerAPI/5
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPut("{id}")]
                public async Task<IActionResult> PutPlayLista(long id, PlayLista playLista)
                {
                        if (id != playLista.ID)
                        {
                                return BadRequest();
                        }

                        _context.Entry(playLista).State = EntityState.Modified;

                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                                if (!PlayListaExists(id))
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

                [HttpPut("PlayListaAPI/{id}")]
                [Consumes("application/json")]
                public async Task<IActionResult> PutPlayLista(long id, PlayListaAPI playListaDto)
                {
                        if (id != playListaDto.ID)
                        {
                                return BadRequest("The ID in the URL does not match the ID in the request body.");
                        }

                        var existingPlayLista = await _context.PlayLista.FindAsync(id);
                        if (existingPlayLista == null)
                        {
                                return NotFound("PlayLista not found.");
                        }

                        existingPlayLista.naziv = playListaDto.naziv;
                        existingPlayLista.opis = playListaDto.opis;
                        existingPlayLista.kreiranDatumVrijeme = playListaDto.kreiranDatumVrijeme;
                        existingPlayLista.javno = playListaDto.javno;
                        existingPlayLista.brojLajkova = playListaDto.brojLajkova;
                        existingPlayLista.putanjaSlika = playListaDto.putanjaSlika;
                        existingPlayLista.putanjaGif = playListaDto.putanjaGif;

                        _context.Entry(existingPlayLista).State = EntityState.Modified;

                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                                if (!PlayListaExists(id))
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


                // POST: api/PlayListaControllerAPI
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
                public async Task<ActionResult<PlayLista>> PostPlayLista(PlayLista playLista)
                {
                        _context.PlayLista.Add(playLista);
                        await _context.SaveChangesAsync();

                        return CreatedAtAction("GetPlayLista", new { id = playLista.ID }, playLista);
                }

                [HttpPost("PlayListaAPI")]
                [Consumes("application/json")]
                public async Task<ActionResult<PlayLista>> PostPlayLista(PlayListaAPI playListaDto)
                {
                        var playLista = new PlayLista
                        {
                                korisnikID = playListaDto.korisnikID,
                                naziv = playListaDto.naziv,
                                opis = playListaDto.opis,
                                kreiranDatumVrijeme = playListaDto.kreiranDatumVrijeme,
                                javno = playListaDto.javno,
                                brojLajkova = playListaDto.brojLajkova,
                                putanjaSlika = playListaDto.putanjaSlika,
                                putanjaGif = playListaDto.putanjaGif
                        };

                        _context.PlayLista.Add(playLista);
                        await _context.SaveChangesAsync();

                        return CreatedAtAction("GetPlayLista", new { id = playLista.ID }, playLista);
                }


                // DELETE: api/PlayListaControllerAPI/5
                [HttpDelete("{id}")]
                public async Task<IActionResult> DeletePlayLista(long id)
                {
                        var playLista = await _context.PlayLista.FindAsync(id);
                        if (playLista == null)
                        {
                                return NotFound();
                        }

                        _context.PlayLista.Remove(playLista);
                        await _context.SaveChangesAsync();

                        return NoContent();
                }

		[HttpPost("obrisi/{id}")]
		public async Task<IActionResult> Obrisi(long id)
		{
			try
			{
				var zahtjev = await _context.PlayLista.FindAsync(id);
				if (zahtjev == null)
				{
					return NotFound(new { message = "Zahtjev nije pronađen." });
				}

				//zahtjev.odobreno = false;
				_context.PlayLista.Remove(zahtjev);


				await _context.SaveChangesAsync();

				return Ok(new { message = "Zahtjev je uspješno odbijen.", zahtjev });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Interna greška servera", error = ex.Message });
			}
		}

		private bool PlayListaExists(long id)
                {
                        return _context.PlayLista.Any(e => e.ID == id);
                }
        }
}
