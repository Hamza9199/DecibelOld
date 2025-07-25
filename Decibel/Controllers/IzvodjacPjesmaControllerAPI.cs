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
    public class IzvodjacPjesmaControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IzvodjacPjesmaControllerAPI(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/IzvodjacPjesmaControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IzvodjacPjesma>>> GetIzvodjacPjesma()
        {
            return await _context.IzvodjacPjesma.ToListAsync();
        }

        // GET: api/IzvodjacPjesmaControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IzvodjacPjesma>> GetIzvodjacPjesma(string id)
        {
            var izvodjacPjesma = await _context.IzvodjacPjesma.FindAsync(id);

            if (izvodjacPjesma == null)
            {
                return NotFound();
            }

            return izvodjacPjesma;
        }

        // PUT: api/IzvodjacPjesmaControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIzvodjacPjesma(string id, IzvodjacPjesma izvodjacPjesma)
        {
            if (id != izvodjacPjesma.izvodjacID)
            {
                return BadRequest();
            }

            _context.Entry(izvodjacPjesma).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IzvodjacPjesmaExists(id))
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

        // POST: api/IzvodjacPjesmaControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IzvodjacPjesma>> PostIzvodjacPjesma(IzvodjacPjesma izvodjacPjesma)
        {
            _context.IzvodjacPjesma.Add(izvodjacPjesma);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (IzvodjacPjesmaExists(izvodjacPjesma.izvodjacID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetIzvodjacPjesma", new { id = izvodjacPjesma.izvodjacID }, izvodjacPjesma);
        }

        // DELETE: api/IzvodjacPjesmaControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIzvodjacPjesma(string id)
        {
            var izvodjacPjesma = await _context.IzvodjacPjesma.FindAsync(id);
            if (izvodjacPjesma == null)
            {
                return NotFound();
            }

            _context.IzvodjacPjesma.Remove(izvodjacPjesma);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IzvodjacPjesmaExists(string id)
        {
            return _context.IzvodjacPjesma.Any(e => e.izvodjacID == id);
        }
    }
}
