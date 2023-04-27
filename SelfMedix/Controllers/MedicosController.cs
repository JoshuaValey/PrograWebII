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
    public class MedicosController : Controller
    {
        private readonly SelfmedixContext _context;
        private string URL = "http://localhost:5183/api/Medicos";
        private readonly HttpClient _httpClient;

        public MedicosController()
        {
            _context = new SelfmedixContext();
            _httpClient = new HttpClient();
        }

        // GET: Medicos
        public async Task<IActionResult> Index()
        {
            var httpResponse = await _httpClient.GetAsync(URL);
            if (httpResponse.IsSuccessStatusCode)
            {
                var result = httpResponse.Content.ReadAsStringAsync().Result;
                List<Medico>? modelList = JsonConvert.DeserializeObject<List<Medico>>(result);
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
                var Medico = JsonConvert.DeserializeObject<Medico>(result);

                if (Medico == null)
                {
                    return NotFound();
                }

                return View(Medico);
            }
            else
            {
                return BadRequest(Results.NotFound());
            }
        }


        // GET: Medicos/Create
        public IActionResult Create()
        {
            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Id");
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id");
            return View();
        }



        //// POST: Medicos/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FechaCreacion,FechaMedico,IdMedico,IdPaciente,Estado,Descripcion")] Medico Medico)
        {

            // Convertir la Medico en un objeto JSON
            var MedicoJson = JsonConvert.SerializeObject(Medico);

            // Crear un objeto HttpContent a partir del JSON
            var content = new StringContent(MedicoJson, Encoding.UTF8, "application/json");

            // Hacer una solicitud POST a la URL de la API
            var response = await _httpClient.PostAsync(URL, content);

            // Verificar si la solicitud fue exitosa
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }


            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Id", Medico.Id);
            //ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", Medico.IdPaciente);
            return View(Medico);



        }



        // GET: Medicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Medicos == null)
            {
                return NotFound();
            }

            var Medico = await _context.Medicos.FindAsync(id);
            if (Medico == null)
            {
                return NotFound();
            }
            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Id", Medico.Id);
            //ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", Medico.IdPaciente);
            return View(Medico);
        }

        // POST: Medicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaCreacion,FechaMedico,IdMedico,IdPaciente,Estado,Descripcion")] Medico Medico)
        {
            if (id != Medico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var MedicoJson = JsonConvert.SerializeObject(Medico);
                    var content = new StringContent(MedicoJson, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"{URL}/{id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(Medico);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            ViewData["IdMedico"] = new SelectList(_context.Medicos, "Id", "Id", Medico.Id);
            //ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "Id", "Id", Medico.IdPaciente);
            return View(Medico);




        }

        // GET: Medicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"{URL}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var MedicoJson = await response.Content.ReadAsStringAsync();
                var Medico1 = JsonConvert.DeserializeObject<Medico>(MedicoJson);

                if (Medico1 == null)
                {
                    return NotFound();
                }

                return View(Medico1);
            }
            else
            {
                return NotFound();
            }

        }

        // POST: Medicos/Delete/5
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

            
        }

        private bool MedicoExists(int id)
        {
            return (_context.Medicos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
