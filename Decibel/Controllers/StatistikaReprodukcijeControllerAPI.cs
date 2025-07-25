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
    public class StatistikaReprodukcijeControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StatistikaReprodukcijeControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StatistikaReprodukcijeControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatistikaReprodukcije>>> GetStatistikaReprodukcije()
        {
            return await _context.StatistikaReprodukcije.ToListAsync();
        }

        // GET: api/StatistikaReprodukcijeControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatistikaReprodukcije>> GetStatistikaReprodukcije(long id)
        {
            var statistikaReprodukcije = await _context.StatistikaReprodukcije.FindAsync(id);

            if (statistikaReprodukcije == null)
            {
                return NotFound();
            }

            return statistikaReprodukcije;
        }

        // PUT: api/StatistikaReprodukcijeControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatistikaReprodukcije(long id, StatistikaReprodukcije statistikaReprodukcije)
        {
            if (id != statistikaReprodukcije.ID)
            {
                return BadRequest();
            }

            _context.Entry(statistikaReprodukcije).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatistikaReprodukcijeExists(id))
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

        // POST: api/StatistikaReprodukcijeControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StatistikaReprodukcije>> PostStatistikaReprodukcije(StatistikaReprodukcije statistikaReprodukcije)
        {
            _context.StatistikaReprodukcije.Add(statistikaReprodukcije);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatistikaReprodukcije", new { id = statistikaReprodukcije.ID }, statistikaReprodukcije);
        }

        // DELETE: api/StatistikaReprodukcijeControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatistikaReprodukcije(long id)
        {
            var statistikaReprodukcije = await _context.StatistikaReprodukcije.FindAsync(id);
            if (statistikaReprodukcije == null)
            {
                return NotFound();
            }

            _context.StatistikaReprodukcije.Remove(statistikaReprodukcije);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatistikaReprodukcijeExists(long id)
        {
            return _context.StatistikaReprodukcije.Any(e => e.ID == id);
        }
    }
}
