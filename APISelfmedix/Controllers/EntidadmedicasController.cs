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
    public class EntidadmedicasController : ControllerBase
    {
        private readonly SelfmedixContext _context;

        public EntidadmedicasController(SelfmedixContext context)
        {
            _context = context;
        }

        // GET: api/Entidadmedicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entidadmedica>>> GetEntidadmedicas()
        {
          if (_context.Entidadmedicas == null)
          {
              return NotFound();
          }
            return await _context.Entidadmedicas.ToListAsync();
        }

        // GET: api/Entidadmedicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Entidadmedica>> GetEntidadmedica(int id)
        {
          if (_context.Entidadmedicas == null)
          {
              return NotFound();
          }
            var entidadmedica = await _context.Entidadmedicas.FindAsync(id);

            if (entidadmedica == null)
            {
                return NotFound();
            }

            return entidadmedica;
        }

        // PUT: api/Entidadmedicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntidadmedica(int id, Entidadmedica entidadmedica)
        {
            if (id != entidadmedica.Id)
            {
                return BadRequest();
            }

            _context.Entry(entidadmedica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntidadmedicaExists(id))
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

        // POST: api/Entidadmedicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Entidadmedica>> PostEntidadmedica(Entidadmedica entidadmedica)
        {
          if (_context.Entidadmedicas == null)
          {
              return Problem("Entity set 'SelfmedixContext.Entidadmedicas'  is null.");
          }
            _context.Entidadmedicas.Add(entidadmedica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntidadmedica", new { id = entidadmedica.Id }, entidadmedica);
        }

        // DELETE: api/Entidadmedicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntidadmedica(int id)
        {
            if (_context.Entidadmedicas == null)
            {
                return NotFound();
            }
            var entidadmedica = await _context.Entidadmedicas.FindAsync(id);
            if (entidadmedica == null)
            {
                return NotFound();
            }

            _context.Entidadmedicas.Remove(entidadmedica);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntidadmedicaExists(int id)
        {
            return (_context.Entidadmedicas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
