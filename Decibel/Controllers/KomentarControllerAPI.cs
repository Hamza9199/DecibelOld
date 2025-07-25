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
        public class KomentarControllerAPI : ControllerBase
        {
                private readonly ApplicationDbContext _context;

                public KomentarControllerAPI(ApplicationDbContext context)
                {
                        _context = context;
                }

                // GET: api/KomentarControllerAPI
                [HttpGet]
                public async Task<ActionResult<IEnumerable<Komentar>>> GetKomentar()
                {
                        return await _context.Komentar.ToListAsync();
                }
                
                

                // GET: api/KomentarControllerAPI/5
                [HttpGet("{id}")]
                public async Task<ActionResult<Komentar>> GetKomentar(string id)
                {
                        var komentar = await _context.Komentar.FindAsync(id);

                        if (komentar == null)
                        {
                                return NotFound();
                        }

                        return komentar;
                }

                // PUT: api/KomentarControllerAPI/5
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPut("{id}")]
                public async Task<IActionResult> PutKomentar(string id, Komentar komentar)
                {
                        if (id != komentar.korisnikID)
                        {
                                return BadRequest();
                        }

                        _context.Entry(komentar).State = EntityState.Modified;

                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                                if (!KomentarExists(id))
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

                // POST: api/KomentarControllerAPI
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
                public async Task<ActionResult<Komentar>> PostKomentar(Komentar komentar)
                {
                        // Dobavlja trenutnog korisnika
                        string korisnikID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        komentar.korisnikID = korisnikID;

                        _context.Komentar.Add(komentar);
                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateException)
                        {
                                
                        }

                        return CreatedAtAction("GetKomentar", new { id = komentar.korisnikID }, komentar);
                }

                // DELETE: api/KomentarControllerAPI/5
                [HttpDelete("{id}")]
                public async Task<IActionResult> DeleteKomentar(string id)
                {
                        var komentar = await _context.Komentar.FindAsync(id);
                        if (komentar == null)
                        {
                                return NotFound();
                        }

                        _context.Komentar.Remove(komentar);
                        await _context.SaveChangesAsync();

                        return NoContent();
                }

                private bool KomentarExists(string id)
                {
                        return _context.Komentar.Any(e => e.korisnikID == id);
                }
        }
}
