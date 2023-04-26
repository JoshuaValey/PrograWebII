using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SelfMedix.Models;

namespace SelfMedix.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;


    namespace SelfMedix.Controllers
    {
        public class PacientesController : Controller
        {
            private readonly SelfmedixContext _context;
            private string URL = "http://localhost:5183/api/Pacientes";
            private readonly HttpClient _httpClient;

            public PacientesController()
            {
                _context = new SelfmedixContext();
                _httpClient = new HttpClient();
            }

            // GET: Pacientess
            public async Task<IActionResult> Index()
            {
                var httpResponse = await _httpClient.GetAsync(URL);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var result = httpResponse.Content.ReadAsStringAsync().Result;
                    List<Paciente>? modelList = JsonConvert.DeserializeObject<List<Paciente>>(result);
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
                    var Paciente = JsonConvert.DeserializeObject<Paciente>(result);

                    if (Paciente == null)
                    {
                        return NotFound();
                    }

                    return View(Paciente);
                }
                else
                {
                    return BadRequest(Results.NotFound());
                }
            }


            // GET: Pacientes/Create
            public IActionResult Create()
            {
                ViewData["IdHistorial"] = new SelectList(_context.Pacientes, "Id", "Id");
                ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id");
                return View();
            }



            //// POST: Pacientes/Create
            //// To protect from overposting attacks, enable the specific properties you want to bind to.
            //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(Paciente Paciente)
            {

                // Convertir la Paciente en un objeto JSON
                var PacienteJson = JsonConvert.SerializeObject(Paciente);

                // Crear un objeto HttpContent a partir del JSON
                var content = new StringContent(PacienteJson, Encoding.UTF8, "application/json");

                // Hacer una solicitud POST a la URL de la API
                var response = await _httpClient.PostAsync(URL, content);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }


                //ViewData["IdHistorial"] = new SelectList(_context.Pacientes, "Id", "Id", Paciente.Id);
                //ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", Paciente.IdPaciente);
                return View(Paciente);



            }



            // GET: Pacientes/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null || _context.Pacientes == null)
                {
                    return NotFound();
                }

                var Paciente = await _context.Pacientes.FindAsync(id);
                if (Paciente == null)
                {
                    return NotFound();
                }
                ViewData["IdMedico"] = new SelectList(_context.Pacientes, "Id", "Id", Paciente.Id

                    );


                //ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", Paciente.IdPaciente);
                return View(Paciente);
            }

            // POST: Paciente/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, Paciente Paciente)
            {
                if (id != Paciente.Id)
                {
                    return NotFound();
                }

                //if (ModelState.IsValid)
                //{
                try
                {
                    var PacienteJson = JsonConvert.SerializeObject(Paciente);
                    var content = new StringContent(PacienteJson, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"{URL}/{id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(Paciente);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
                // }

                ViewData["IdHistorial"] = new SelectList(_context.Pacientes, "Id", "Id", Paciente.Id);
                //ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", Paciente.IdPaciente);
                return View(Paciente);




            }

            // GET: Paciente/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var response = await _httpClient.GetAsync($"{URL}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var PacienteJson = await response.Content.ReadAsStringAsync();
                    var Paciente1 = JsonConvert.DeserializeObject<Paciente>(PacienteJson);

                    if (Paciente1 == null)
                    {
                        return NotFound();
                    }

                    return View(Paciente1);
                }
                else
                {
                    return NotFound();
                }

                #region Coment
                //if (id == null || _context.Paciente == null)
                //{
                //    return NotFound();
                //}

                //var Paciente = await _context.Paciente
                //    .Include(c => c.IdMedicoNavigation)
                //    .Include(c => c.IdPacienteNavigation)
                //    .FirstOrDefaultAsync(m => m.Id == id);
                //if (Paciente == null)
                //{
                //    return NotFound();
                //}

                //return View(Paciente);

                #endregion
            }

            // POST: Paciente/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {

                var response = await _httpClient.DeleteAsync($"{URL}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }

                #region coment
                //if (_context.Paciente == null)
                //{
                //    return Problem("Entity set 'SelfmedixContext.Paciente'  is null.");
                //}
                //var Paciente = await _context.Paciente.FindAsync(id);
                //if (Paciente != null)
                //{
                //    _context.Paciente.Remove(Paciente);
                //}

                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                #endregion
            }

            private bool PacienteExists(int id)
            {
                return (_context.Pacientes?.Any(e => e.Id == id)).GetValueOrDefault();
            }
        }
    }

}
