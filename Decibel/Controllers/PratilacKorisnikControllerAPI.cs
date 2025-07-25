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
    public class PratilacKorisnikControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PratilacKorisnikControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PratilacKorisnikControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PratilacKorisnik>>> GetPratilac()
        {
            return await _context.Pratilac.ToListAsync();
        }

        // GET: api/PratilacKorisnikControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PratilacKorisnik>> GetPratilacKorisnik(string id)
        {
            var pratilacKorisnik = await _context.Pratilac.FindAsync(id);

            if (pratilacKorisnik == null)
            {
                return NotFound();
            }

            return pratilacKorisnik;
        }

        // PUT: api/PratilacKorisnikControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPratilacKorisnik(string id, PratilacKorisnik pratilacKorisnik)
        {
            if (id != pratilacKorisnik.korisnikID)
            {
                return BadRequest();
            }

            _context.Entry(pratilacKorisnik).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PratilacKorisnikExists(id))
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

        // POST: api/PratilacKorisnikControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PratilacKorisnik>> PostPratilacKorisnik(PratilacKorisnik pratilacKorisnik)
        {
            _context.Pratilac.Add(pratilacKorisnik);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PratilacKorisnikExists(pratilacKorisnik.korisnikID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPratilacKorisnik", new { id = pratilacKorisnik.korisnikID }, pratilacKorisnik);
        }

        // DELETE: api/PratilacKorisnikControllerAPI/5
        [HttpDelete("{korisnikID}/{pratilacID}")]
        public async Task<IActionResult> DeletePratilacKorisnik(string korisnikID, string pratilacID)
        {
                var pratilacKorisnik = await _context.Pratilac
                        .Where(p => p.korisnikID == korisnikID && p.pratilacID == pratilacID).FirstOrDefaultAsync();

            if (pratilacKorisnik == null)
            {
                return NotFound();
            }

            _context.Pratilac.Remove(pratilacKorisnik);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PratilacKorisnikExists(string id)
        {
            return _context.Pratilac.Any(e => e.korisnikID == id);
        }
    }
}
