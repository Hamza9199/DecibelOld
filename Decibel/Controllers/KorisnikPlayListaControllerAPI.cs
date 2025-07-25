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
    public class KorisnikPlayListaControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KorisnikPlayListaControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/KorisnikPlayListaControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KorisnikPlayLista>>> GetKorisnikPlayLista()
        {
            return await _context.KorisnikPlayLista.ToListAsync();
        }

        // GET: api/KorisnikPlayListaControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KorisnikPlayLista>> GetKorisnikPlayLista(string id)
        {
            var korisnikPlayLista = await _context.KorisnikPlayLista.FindAsync(id);

            if (korisnikPlayLista == null)
            {
                return NotFound();
            }

            return korisnikPlayLista;
        }

        // PUT: api/KorisnikPlayListaControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKorisnikPlayLista(string id, KorisnikPlayLista korisnikPlayLista)
        {
            if (id != korisnikPlayLista.korisnikID)
            {
                return BadRequest();
            }

            _context.Entry(korisnikPlayLista).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KorisnikPlayListaExists(id))
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

        // POST: api/KorisnikPlayListaControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<KorisnikPlayLista>> PostKorisnikPlayLista(KorisnikPlayLista korisnikPlayLista)
        {
            _context.KorisnikPlayLista.Add(korisnikPlayLista);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (KorisnikPlayListaExists(korisnikPlayLista.korisnikID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetKorisnikPlayLista", new { id = korisnikPlayLista.korisnikID }, korisnikPlayLista);
        }

        // DELETE: api/KorisnikPlayListaControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKorisnikPlayLista(string id)
        {
            var korisnikPlayLista = await _context.KorisnikPlayLista.FindAsync(id);
            if (korisnikPlayLista == null)
            {
                return NotFound();
            }

            _context.KorisnikPlayLista.Remove(korisnikPlayLista);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KorisnikPlayListaExists(string id)
        {
            return _context.KorisnikPlayLista.Any(e => e.korisnikID == id);
        }
    }
}
