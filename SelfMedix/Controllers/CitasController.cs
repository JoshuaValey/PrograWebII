using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SelfMedix.Models;

namespace SelfMedix.Controllers
{
    public class CitasController : Controller
    {
        private readonly SelfmedixContext _context;
        private string URL = "http://localhost:5183/api/Citas";
        private readonly HttpClient _httpClient;

        public CitasController()
        {
            _context = new SelfmedixContext();
            _httpClient = new HttpClient();
        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
            var httpResponse = await _httpClient.GetAsync(URL);
            if (httpResponse.IsSuccessStatusCode)
            {
                var result = httpResponse.Content.ReadAsStringAsync().Result;
                List<Cita>? modelList = JsonConvert.DeserializeObject<List<Cita>>(result);
                return View(modelList);

            }
            else return BadRequest(Results.NotFound());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var httpResponse = await _httpClient.GetAsync($"{URL}/{id}");

            if (httpResponse.IsSuccessStatusCode)
            {
                var result = await httpResponse.Content.ReadAsStringAsync();
                var cita = JsonConvert.DeserializeObject<Cita>(result);

                if (cita == null)
                {
                    return NotFound();
                }

                return View(cita);
            }
            else
            {
                return BadRequest(Results.NotFound());
            }
        }


        // GET: Citas/Create
        public IActionResult Create()
        {
            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Id");
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id");
            return View();
        }



        //// POST: Citas/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FechaCreacion,FechaCita,IdMedico,IdPaciente,Estado,Descripcion")] Cita cita)
        {
           
                // Convertir la cita en un objeto JSON
                var citaJson = JsonConvert.SerializeObject(cita);

                // Crear un objeto HttpContent a partir del JSON
                var content = new StringContent(citaJson, Encoding.UTF8, "application/json");

                // Hacer una solicitud POST a la URL de la API
                var response = await _httpClient.PostAsync(URL, content);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            

            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Id", cita.IdMedico);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", cita.IdPaciente);
            return View(cita);


            //if (ModelState.IsValid)
            //{
            //    _context.Add(cita);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Id", cita.IdMedico);
            //ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", cita.IdPaciente);
            //return View(cita);
        }



        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cita == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }
            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Id", cita.IdMedico);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", cita.IdPaciente);
            return View(cita);
        }

        // POST: Citas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaCreacion,FechaCita,IdMedico,IdPaciente,Estado,Descripcion")] Cita cita)
        {
            if (id != cita.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var citaJson = JsonConvert.SerializeObject(cita);
                    var content = new StringContent(citaJson, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"{URL}/{Id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(cita);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Id", cita.IdMedico);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", cita.IdPaciente);
            return View(cita);



            #region coment
            //if (id != cita.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(cita);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!CitaExists(cita.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Id", cita.IdMedico);
            //ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", cita.IdPaciente);
            //return View(cita);

            #endregion
        }

        // GET: Citas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cita == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita
                .Include(c => c.IdMedicoNavigation)
                .Include(c => c.IdPacienteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cita == null)
            {
                return Problem("Entity set 'SelfmedixContext.Cita'  is null.");
            }
            var cita = await _context.Cita.FindAsync(id);
            if (cita != null)
            {
                _context.Cita.Remove(cita);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
          return (_context.Cita?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
