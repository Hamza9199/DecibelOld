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
    public class KorisnikPretplataControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KorisnikPretplataControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/KorisnikPretplataControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KorisnikPretplata>>> GetKorisnikPretplata()
        {
            return await _context.KorisnikPretplata.ToListAsync();
        }

        // GET: api/KorisnikPretplataControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KorisnikPretplata>> GetKorisnikPretplata(long id)
        {
            var korisnikPretplata = await _context.KorisnikPretplata.FindAsync(id);

            if (korisnikPretplata == null)
            {
                return NotFound();
            }

            return korisnikPretplata;
        }

        // PUT: api/KorisnikPretplataControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKorisnikPretplata(long id, KorisnikPretplata korisnikPretplata)
        {
            if (id != korisnikPretplata.ID)
            {
                return BadRequest();
            }

            _context.Entry(korisnikPretplata).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KorisnikPretplataExists(id))
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

        // POST: api/KorisnikPretplataControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<KorisnikPretplata>> PostKorisnikPretplata(KorisnikPretplata korisnikPretplata)
        {
            _context.KorisnikPretplata.Add(korisnikPretplata);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKorisnikPretplata", new { id = korisnikPretplata.ID }, korisnikPretplata);
        }

        // DELETE: api/KorisnikPretplataControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKorisnikPretplata(long id)
        {
            var korisnikPretplata = await _context.KorisnikPretplata.FindAsync(id);
            if (korisnikPretplata == null)
            {
                return NotFound();
            }

            _context.KorisnikPretplata.Remove(korisnikPretplata);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KorisnikPretplataExists(long id)
        {
            return _context.KorisnikPretplata.Any(e => e.ID == id);
        }
    }
}
