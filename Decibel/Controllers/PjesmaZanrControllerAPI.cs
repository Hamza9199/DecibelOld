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
    public class PjesmaZanrControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PjesmaZanrControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PjesmaZanrControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PjesmaZanr>>> GetPjesmaZanr()
        {
            return await _context.PjesmaZanr.ToListAsync();
        }

        // GET: api/PjesmaZanrControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PjesmaZanr>> GetPjesmaZanr(long id)
        {
            var pjesmaZanr = await _context.PjesmaZanr.FindAsync(id);

            if (pjesmaZanr == null)
            {
                return NotFound();
            }

            return pjesmaZanr;
        }

        // PUT: api/PjesmaZanrControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPjesmaZanr(long id, PjesmaZanr pjesmaZanr)
        {
            if (id != pjesmaZanr.pjesmaID)
            {
                return BadRequest();
            }

            _context.Entry(pjesmaZanr).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PjesmaZanrExists(id))
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

        // POST: api/PjesmaZanrControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PjesmaZanr>> PostPjesmaZanr(PjesmaZanr pjesmaZanr)
        {
            _context.PjesmaZanr.Add(pjesmaZanr);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PjesmaZanrExists(pjesmaZanr.pjesmaID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPjesmaZanr", new { id = pjesmaZanr.pjesmaID }, pjesmaZanr);
        }

        // DELETE: api/PjesmaZanrControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePjesmaZanr(long id)
        {
            var pjesmaZanr = await _context.PjesmaZanr.FindAsync(id);
            if (pjesmaZanr == null)
            {
                return NotFound();
            }

            _context.PjesmaZanr.Remove(pjesmaZanr);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PjesmaZanrExists(long id)
        {
            return _context.PjesmaZanr.Any(e => e.pjesmaID == id);
        }
    }
}
