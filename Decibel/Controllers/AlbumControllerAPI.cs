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
    public class AlbumControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AlbumControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AlbumControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbum()
        {
            return await _context.Album.ToListAsync();
        }

        // GET: api/AlbumControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Album>> GetAlbum(long id)
        {
            var album = await _context.Album.FindAsync(id);

            if (album == null)
            {
                return NotFound();
            }

            return album;
        }

        // PUT: api/AlbumControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlbum(long id, Album album)
        {
            if (id != album.ID)
            {
                return BadRequest();
            }

            _context.Entry(album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
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

        [HttpPut("AlbumAPI/{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> PutAlbum(long id, AlbumAPI albumDto)
        {
                if (id != albumDto.ID)
                {
                        return BadRequest("The ID in the URL does not match the ID in the request body.");
                }

                var existingAlbum = await _context.Album.FindAsync(id);
                if (existingAlbum == null)
                {
                        return NotFound("Album not found.");
                }

                // Update the existing album with values from the DTO
                existingAlbum.naziv = albumDto.naziv;
                existingAlbum.opis = albumDto.opis;
                existingAlbum.kreiranDatumVrijeme = albumDto.kreiranDatumVrijeme;
                existingAlbum.odobreno = albumDto.odobreno;
                existingAlbum.javno = albumDto.javno;
                existingAlbum.brojLajkova = albumDto.brojLajkova;
                existingAlbum.putanjaSlika = albumDto.putanjaSlika;
                existingAlbum.putanjaGif = albumDto.putanjaGif;

                _context.Entry(existingAlbum).State = EntityState.Modified;

                try
                {
                        await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                        if (!AlbumExists(id))
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


                // POST: api/AlbumControllerAPI
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
        public async Task<ActionResult<Album>> PostAlbum(Album album)
        {
            _context.Album.Add(album);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlbum", new { id = album.ID }, album);
        }

        [HttpPost("AlbumAPI")]
        [Consumes("application/json")]
        public async Task<ActionResult<Album>> PostAlbum(AlbumAPI albumDto)
        {
                var album = new Album
                {
                        korisnikID = albumDto.korisnikID,
                        naziv = albumDto.naziv,
                        opis = albumDto.opis,
                        kreiranDatumVrijeme = albumDto.kreiranDatumVrijeme,
                        odobreno = albumDto.odobreno,
                        javno = albumDto.javno,
                        brojLajkova = albumDto.brojLajkova,
                        putanjaSlika = albumDto.putanjaSlika,
                        putanjaGif = albumDto.putanjaGif
                };

                _context.Album.Add(album);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAlbum", new { id = album.ID }, album);
        }


                // DELETE: api/AlbumControllerAPI/5
                [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(long id)
        {
            var album = await _context.Album.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            _context.Album.Remove(album);
            await _context.SaveChangesAsync();

            return NoContent();
        }

		[HttpPost("dozvoli/{id}")]
		public async Task<IActionResult> DozvoliAsync(long id)
		{
			try
			{
				var zahtjev = await _context.Album.FindAsync(id);
				if (zahtjev == null)
				{
					return NotFound(new { message = "Zahtjev nije pronađen." });
				}

				zahtjev.odobreno = true;

				_context.Album.Update(zahtjev);


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
				var zahtjev = await _context.Album.FindAsync(id);
				if (zahtjev == null)
				{
					return NotFound(new { message = "Zahtjev nije pronađen." });
				}

				//zahtjev.odobreno = false;
				//_context.Album.Remove(zahtjev);

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
				var zahtjev = await _context.Album.FindAsync(id);
				if (zahtjev == null)
				{
					return NotFound(new { message = "Zahtjev nije pronađen." });
				}

				//zahtjev.odobreno = false;
				_context.Album.Remove(zahtjev);


				await _context.SaveChangesAsync();

				return Ok(new { message = "Zahtjev je uspješno odbijen.", zahtjev });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "Interna greška servera", error = ex.Message });
			}
		}

		private bool AlbumExists(long id)
        {
            return _context.Album.Any(e => e.ID == id);
        }
    }
}
