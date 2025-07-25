using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Decibel.Data;
using Decibel.Models;
using Decibel.Services;

namespace Decibel.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class PjesmaPlayListaControllerAPI : ControllerBase
        {
                private readonly ApplicationDbContext _context;
                private readonly PjesmaPlayListaService _pjesmaPlayListaService;

                public PjesmaPlayListaControllerAPI(ApplicationDbContext context, PjesmaPlayListaService pjesmaPlayListaService)
                {
                        _context = context;
                        _pjesmaPlayListaService = pjesmaPlayListaService;
                }

                [HttpDelete("ObrisiPjesmu/{pjesmaID}/{playlistaID}")]
                public async Task<ActionResult> ObrisiPjesmu(int pjesmaID, int playlistaID)
                {
                        if (pjesmaID == null || playlistaID == null)
                                return BadRequest();

                        await _pjesmaPlayListaService.ObrisiPjesmu(pjesmaID, playlistaID);

                        return Ok(new { success = true });
                }


                [HttpPost("GetRedoslijedPjesama/{playlistaID}")]
                public async Task<ActionResult> GetRedoslijedPjesama(int playlistaID)
                {
                        var lista = await _pjesmaPlayListaService.GetPlaylistOrderAsync(playlistaID);

                        return new JsonResult(lista);
                }

                // GET: api/PjesmaPlayListaControllerAPI
                [HttpGet]
                public async Task<ActionResult<IEnumerable<PjesmaPlayLista>>> GetPjesmaPlaylista()
                {
                        return await _context.PjesmaPlaylista.ToListAsync();
                }

                // GET: api/PjesmaPlayListaControllerAPI/5
                [HttpGet("{id}")]
                public async Task<ActionResult<PjesmaPlayLista>> GetPjesmaPlayLista(long id)
                {
                        var pjesmaPlayLista = await _context.PjesmaPlaylista.FindAsync(id);

                        if (pjesmaPlayLista == null)
                        {
                                return NotFound();
                        }

                        return pjesmaPlayLista;
                }

                // PUT: api/PjesmaPlayListaControllerAPI/5
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPut("{id}")]
                public async Task<IActionResult> PutPjesmaPlayLista(long id, PjesmaPlayLista pjesmaPlayLista)
                {
                        if (id != pjesmaPlayLista.pjesmaID)
                        {
                                return BadRequest();
                        }

                        _context.Entry(pjesmaPlayLista).State = EntityState.Modified;

                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                                if (!PjesmaPlayListaExists(id))
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

                // POST: api/PjesmaPlayListaControllerAPI
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
                public async Task<ActionResult<PjesmaPlayLista>> PostPjesmaPlayLista(PjesmaPlayLista pjesmaPlayLista)
                {
                        _context.PjesmaPlaylista.Add(pjesmaPlayLista);
                        try
                        {
                                await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateException)
                        {
                                if (PjesmaPlayListaExists(pjesmaPlayLista.pjesmaID))
                                {
                                        return Conflict();
                                }
                                else
                                {
                                        throw;
                                }
                        }

                        return CreatedAtAction("GetPjesmaPlayLista", new { id = pjesmaPlayLista.pjesmaID }, pjesmaPlayLista);
                }

                // DELETE: api/PjesmaPlayListaControllerAPI/5
                [HttpDelete("{id}")]
                public async Task<IActionResult> DeletePjesmaPlayLista(long id)
                {
                        var pjesmaPlayLista = await _context.PjesmaPlaylista.FindAsync(id);
                        if (pjesmaPlayLista == null)
                        {
                                return NotFound();
                        }

                        _context.PjesmaPlaylista.Remove(pjesmaPlayLista);
                        await _context.SaveChangesAsync();

                        return NoContent();
                }

                private bool PjesmaPlayListaExists(long id)
                {
                        return _context.PjesmaPlaylista.Any(e => e.pjesmaID == id);
                }
        }
}
