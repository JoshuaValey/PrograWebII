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
    public class HistorialmedicoController : Controller
    {
        private readonly SelfmedixContext _context;
        private string URL = "http://localhost:5183/api/Historialmedico";
        private readonly HttpClient _httpClient;

        public HistorialmedicoController()
        {
            _context = new SelfmedixContext();
            _httpClient = new HttpClient();
        }

        // GET: Historialmedicos
        public async Task<IActionResult> Index()
        {
            var httpResponse = await _httpClient.GetAsync(URL);
            if (httpResponse.IsSuccessStatusCode)
            {
                var result = httpResponse.Content.ReadAsStringAsync().Result;
                List<Historialmedico>? modelList = JsonConvert.DeserializeObject<List<Historialmedico>>(result);
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
                var Historialmedico = JsonConvert.DeserializeObject<Historialmedico>(result);

                if (Historialmedico == null)
                {
                    return NotFound();
                }

                return View(Historialmedico);
            }
            else
            {
                return BadRequest(Results.NotFound());
            }
        }


        // GET: Historialmedico/Create
        public IActionResult Create()
        {
            ViewData["IdHistorial"] = new SelectList(_context.Historialmedicos, "Id", "Id");
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id");
            return View();
        }



        //// POST: Historialmedico/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Historialmedico Historialmedico)
        {

            // Convertir la Historialmedico en un objeto JSON
            var HistorialmedicoJson = JsonConvert.SerializeObject(Historialmedico);

            // Crear un objeto HttpContent a partir del JSON
            var content = new StringContent(HistorialmedicoJson, Encoding.UTF8, "application/json");

            // Hacer una solicitud POST a la URL de la API
            var response = await _httpClient.PostAsync(URL, content);

            // Verificar si la solicitud fue exitosa
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }


            ViewData["IdHistorial"] = new SelectList(_context.Historialmedicos, "Id", "Id", Historialmedico.Id);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", Historialmedico.IdPaciente);
            return View(Historialmedico);



        }



        // GET: Historialmedico/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Historialmedicos == null)
            {
                return NotFound();
            }

            var Historialmedico = await _context.Historialmedicos.FindAsync(id);
            if (Historialmedico == null)
            {
                return NotFound();
            }
            ViewData["IdMedico"] = new SelectList(_context.Historialmedicos, "Id", "Id", Historialmedico.Id

                );


            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", Historialmedico.IdPaciente);
            return View(Historialmedico);
        }

        // POST: Historialmedico/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Historialmedico Historialmedico)
        {
            if (id != Historialmedico.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                var HistorialmedicoJson = JsonConvert.SerializeObject(Historialmedico);
                var content = new StringContent(HistorialmedicoJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{URL}/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(Historialmedico);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }
            // }

            ViewData["IdHistorial"] = new SelectList(_context.Historialmedicos, "Id", "Id", Historialmedico.Id);
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", Historialmedico.IdPaciente);
            return View(Historialmedico);




        }

        // GET: Historialmedico/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"{URL}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var HistorialmedicoJson = await response.Content.ReadAsStringAsync();
                var Historialmedico1 = JsonConvert.DeserializeObject<Historialmedico>(HistorialmedicoJson);

                if (Historialmedico1 == null)
                {
                    return NotFound();
                }

                return View(Historialmedico1);
            }
            else
            {
                return NotFound();
            }

            #region Coment
            //if (id == null || _context.Historialmedico == null)
            //{
            //    return NotFound();
            //}

            //var Historialmedico = await _context.Historialmedico
            //    .Include(c => c.IdMedicoNavigation)
            //    .Include(c => c.IdPacienteNavigation)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (Historialmedico == null)
            //{
            //    return NotFound();
            //}

            //return View(Historialmedico);

            #endregion
        }

        // POST: Historialmedico/Delete/5
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
            //if (_context.Historialmedico == null)
            //{
            //    return Problem("Entity set 'SelfmedixContext.Historialmedico'  is null.");
            //}
            //var Historialmedico = await _context.Historialmedico.FindAsync(id);
            //if (Historialmedico != null)
            //{
            //    _context.Historialmedico.Remove(Historialmedico);
            //}

            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            #endregion 
        }

        private bool HistorialmedicoExists(int id)
        {
            return (_context.Historialmedicos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
