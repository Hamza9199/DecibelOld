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
    public class ObnovaPretplateControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ObnovaPretplateControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ObnovaPretplateControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObnovaPretplate>>> GetObnovaPretplate()
        {
            return await _context.ObnovaPretplate.ToListAsync();
        }

        // GET: api/ObnovaPretplateControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ObnovaPretplate>> GetObnovaPretplate(long id)
        {
            var obnovaPretplate = await _context.ObnovaPretplate.FindAsync(id);

            if (obnovaPretplate == null)
            {
                return NotFound();
            }

            return obnovaPretplate;
        }

        // PUT: api/ObnovaPretplateControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObnovaPretplate(long id, ObnovaPretplate obnovaPretplate)
        {
            if (id != obnovaPretplate.ID)
            {
                return BadRequest();
            }

            _context.Entry(obnovaPretplate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObnovaPretplateExists(id))
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

        // POST: api/ObnovaPretplateControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ObnovaPretplate>> PostObnovaPretplate(ObnovaPretplate obnovaPretplate)
        {
            _context.ObnovaPretplate.Add(obnovaPretplate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetObnovaPretplate", new { id = obnovaPretplate.ID }, obnovaPretplate);
        }

        // DELETE: api/ObnovaPretplateControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObnovaPretplate(long id)
        {
            var obnovaPretplate = await _context.ObnovaPretplate.FindAsync(id);
            if (obnovaPretplate == null)
            {
                return NotFound();
            }

            _context.ObnovaPretplate.Remove(obnovaPretplate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ObnovaPretplateExists(long id)
        {
            return _context.ObnovaPretplate.Any(e => e.ID == id);
        }
    }
}
