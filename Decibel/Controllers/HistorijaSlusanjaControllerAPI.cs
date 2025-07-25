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
    public class HistorijaSlusanjaControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HistorijaSlusanjaControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/HistorijaSlusanjaControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorijaSlusanja>>> GetHistorijaSlusanja()
        {
            return await _context.HistorijaSlusanja.ToListAsync();
        }

        // GET: api/HistorijaSlusanjaControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HistorijaSlusanja>> GetHistorijaSlusanja(long id)
        {
            var historijaSlusanja = await _context.HistorijaSlusanja.FindAsync(id);

            if (historijaSlusanja == null)
            {
                return NotFound();
            }

            return historijaSlusanja;
        }

        // PUT: api/HistorijaSlusanjaControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistorijaSlusanja(long id, HistorijaSlusanja historijaSlusanja)
        {
            if (id != historijaSlusanja.ID)
            {
                return BadRequest();
            }

            _context.Entry(historijaSlusanja).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistorijaSlusanjaExists(id))
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

        // POST: api/HistorijaSlusanjaControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HistorijaSlusanja>> PostHistorijaSlusanja(HistorijaSlusanja historijaSlusanja)
        {
            _context.HistorijaSlusanja.Add(historijaSlusanja);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistorijaSlusanja", new { id = historijaSlusanja.ID }, historijaSlusanja);
        }

        // DELETE: api/HistorijaSlusanjaControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorijaSlusanja(long id)
        {
            var historijaSlusanja = await _context.HistorijaSlusanja.FindAsync(id);
            if (historijaSlusanja == null)
            {
                return NotFound();
            }

            _context.HistorijaSlusanja.Remove(historijaSlusanja);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistorijaSlusanjaExists(long id)
        {
            return _context.HistorijaSlusanja.Any(e => e.ID == id);
        }
    }
}
