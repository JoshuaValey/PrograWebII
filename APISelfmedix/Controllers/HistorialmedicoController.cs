using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISelfmedix.Models;

namespace APISelfmedix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialmedicoController : ControllerBase
    {
        private readonly SelfmedixContext _context;

        public HistorialmedicoController(SelfmedixContext context)
        {
            _context = context;
        }

        // GET: api/Historialmedico
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Historialmedico>>> GetHistorialmedicos()
        {
          if (_context.Historialmedicos == null)
          {
              return NotFound();
          }
            return await _context.Historialmedicos.ToListAsync();
        }

        // GET: api/Historialmedico/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Historialmedico>> GetHistorialmedico(int id)
        {
          if (_context.Historialmedicos == null)
          {
              return NotFound();
          }
            var historialmedico = await _context.Historialmedicos.FindAsync(id);

            if (historialmedico == null)
            {
                return NotFound();
            }

            return historialmedico;
        }

        // PUT: api/Historialmedico/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistorialmedico(int id, Historialmedico historialmedico)
        {
            if (id != historialmedico.Id)
            {
                return BadRequest();
            }

            _context.Entry(historialmedico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistorialmedicoExists(id))
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

        // POST: api/Historialmedico
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Historialmedico>> PostHistorialmedico(Historialmedico historialmedico)
        {
          if (_context.Historialmedicos == null)
          {
              return Problem("Entity set 'SelfmedixContext.Historialmedicos'  is null.");
          }
            _context.Historialmedicos.Add(historialmedico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistorialmedico", new { id = historialmedico.Id }, historialmedico);
        }

        // DELETE: api/Historialmedico/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorialmedico(int id)
        {
            if (_context.Historialmedicos == null)
            {
                return NotFound();
            }
            var historialmedico = await _context.Historialmedicos.FindAsync(id);
            if (historialmedico == null)
            {
                return NotFound();
            }

            _context.Historialmedicos.Remove(historialmedico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistorialmedicoExists(int id)
        {
            return (_context.Historialmedicos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
