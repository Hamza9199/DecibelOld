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
    public class KorisnikAlbumControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KorisnikAlbumControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/KorisnikAlbumControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KorisnikAlbum>>> GetKorisnikAlbum()
        {
            return await _context.KorisnikAlbum.ToListAsync();
        }

        // GET: api/KorisnikAlbumControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KorisnikAlbum>> GetKorisnikAlbum(string id)
        {
            var korisnikAlbum = await _context.KorisnikAlbum.FindAsync(id);

            if (korisnikAlbum == null)
            {
                return NotFound();
            }

            return korisnikAlbum;
        }

        // PUT: api/KorisnikAlbumControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKorisnikAlbum(string id, KorisnikAlbum korisnikAlbum)
        {
            if (id != korisnikAlbum.korisnikID)
            {
                return BadRequest();
            }

            _context.Entry(korisnikAlbum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KorisnikAlbumExists(id))
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

        // POST: api/KorisnikAlbumControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<KorisnikAlbum>> PostKorisnikAlbum(KorisnikAlbum korisnikAlbum)
        {
            _context.KorisnikAlbum.Add(korisnikAlbum);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (KorisnikAlbumExists(korisnikAlbum.korisnikID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetKorisnikAlbum", new { id = korisnikAlbum.korisnikID }, korisnikAlbum);
        }

        // DELETE: api/KorisnikAlbumControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKorisnikAlbum(string id)
        {
            var korisnikAlbum = await _context.KorisnikAlbum.FindAsync(id);
            if (korisnikAlbum == null)
            {
                return NotFound();
            }

            _context.KorisnikAlbum.Remove(korisnikAlbum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KorisnikAlbumExists(string id)
        {
            return _context.KorisnikAlbum.Any(e => e.korisnikID == id);
        }
    }
}
