using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public class KorisnikPjesmaControllerAPI : ControllerBase
        {
                private readonly ApplicationDbContext _context;

                public KorisnikPjesmaControllerAPI(ApplicationDbContext context)
                {
                        _context = context;
                }

                // GET: api/KorisnikPjesmaControllerAPI
                [HttpGet]
                public async Task<ActionResult<IEnumerable<KorisnikPjesma>>> GetKorisnikPjesma()
                {
                        return await _context.KorisnikPjesma.ToListAsync();
                }

                // GET: api/KorisnikPjesmaControllerAPI/5
                [HttpGet("{id}")]
                public async Task<ActionResult<KorisnikPjesma>> GetKorisnikPjesma(string id)
                {
                        var korisnikPjesma = await _context.KorisnikPjesma.FindAsync(id);

                        if (korisnikPjesma == null)
                        {
                                return NotFound();
                        }

                        return korisnikPjesma;
                }

                // PUT: api/KorisnikPjesmaControllerAPI/5
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPut("{id}")]
                public async Task<IActionResult> PutKorisnikPjesma(string id, KorisnikPjesma korisnikPjesma)
                {
                        if (id != korisnikPjesma.korisnikID)
                        {
                                return BadRequest();
                        }

                        _context.Entry(korisnikPjesma).State = EntityState.Modified;

                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                                if (!KorisnikPjesmaExists(id))
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

                // POST: api/KorisnikPjesmaControllerAPI
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
                public async Task<ActionResult<KorisnikPjesma>> PostKorisnikPjesma(long pjesmaID)
                {
                        // Dobavlja trenutnog korisnika
                        string? korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        if (korisnikID == null)
                                return null;

                        var korisnikPjesma = new KorisnikPjesma
                        {
                                korisnikID = korisnikID,
                                pjesmaID = pjesmaID
                        };

                        _context.KorisnikPjesma.Add(korisnikPjesma);
                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateException)
                        {
                                if (KorisnikPjesmaExists(korisnikPjesma.korisnikID))
                                {
                                        return Conflict();
                                }
                                else
                                {
                                        throw;
                                }
                        }

                        return CreatedAtAction("GetKorisnikPjesma", new { id = korisnikPjesma.korisnikID }, korisnikPjesma);
                }

                // DELETE: api/KorisnikPjesmaControllerAPI/5
                [HttpDelete("{korisnikID}/{pjesmaID}")]
                public async Task<IActionResult> DeleteKorisnikPjesma(string korisnikID, int pjesmaID)
                {
                        var korisnikPjesma = await _context.KorisnikPjesma
                                .Where(kp => kp.korisnikID == korisnikID && kp.pjesmaID == pjesmaID)
                                .FirstOrDefaultAsync();

                        if (korisnikPjesma == null)
                        {
                                return NotFound();
                        }

                        _context.KorisnikPjesma.Remove(korisnikPjesma);
                        await _context.SaveChangesAsync();

                        return NoContent();
                }


                private bool KorisnikPjesmaExists(string id)
                {
                        return _context.KorisnikPjesma.Any(e => e.korisnikID == id);
                }
        }
}
