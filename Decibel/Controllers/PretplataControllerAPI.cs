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
    public class PretplataControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PretplataControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PretplataControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pretplata>>> GetPretplata()
        {
            return await _context.Pretplata.ToListAsync();
        }

        // GET: api/PretplataControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pretplata>> GetPretplata(long id)
        {
            var pretplata = await _context.Pretplata.FindAsync(id);

            if (pretplata == null)
            {
                return NotFound();
            }

            return pretplata;
        }

        // PUT: api/PretplataControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPretplata(long id, Pretplata pretplata)
        {
            if (id != pretplata.ID)
            {
                return BadRequest();
            }

            _context.Entry(pretplata).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PretplataExists(id))
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

        // POST: api/PretplataControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pretplata>> PostPretplata(Pretplata pretplata)
        {
            _context.Pretplata.Add(pretplata);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPretplata", new { id = pretplata.ID }, pretplata);
        }

        // DELETE: api/PretplataControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePretplata(long id)
        {
            var pretplata = await _context.Pretplata.FindAsync(id);
            if (pretplata == null)
            {
                return NotFound();
            }

            _context.Pretplata.Remove(pretplata);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PretplataExists(long id)
        {
            return _context.Pretplata.Any(e => e.ID == id);
        }
    }
}
