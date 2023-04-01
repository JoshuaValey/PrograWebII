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
    public class EntidadmedicasController : Controller
    {
        private readonly SelfmedixContext _context;
        private string URL = "http://localhost:5183/api/Entidadmedicas";
        private readonly HttpClient _httpClient;

        public EntidadmedicasController()
        {
            _context = new SelfmedixContext();
            _httpClient = new HttpClient();
        }

        // GET: Entidadmedicas
        public async Task<IActionResult> Index()
        {
            var httpResponse = await _httpClient.GetAsync(URL);
            if (httpResponse.IsSuccessStatusCode)
            {
                var result = httpResponse.Content.ReadAsStringAsync().Result;
                List<Entidadmedica>? modelList = JsonConvert.DeserializeObject<List<Entidadmedica>>(result);
                return View(modelList);

            }
            else return BadRequest(Results.NotFound());
        }

        // GET: Entidadmedicas/Details/5
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
                var EM = JsonConvert.DeserializeObject<Entidadmedica>(result);

                if (EM == null)
                {
                    return NotFound();
                }

                return View(EM);
            }
            else
            {
                return BadRequest(Results.NotFound());
            }
        }

        // GET: Entidadmedicas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Entidadmedicas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Direccion")] Entidadmedica entidadmedica)
        {

            // Convertir la cita en un objeto JSON
            var entidadJson = JsonConvert.SerializeObject(entidadmedica);

            // Crear un objeto HttpContent a partir del JSON
            var content = new StringContent(entidadJson, Encoding.UTF8, "application/json");

            // Hacer una solicitud POST a la URL de la API
            var response = await _httpClient.PostAsync(URL, content);

            // Verificar si la solicitud fue exitosa
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }


  
            return View(entidadmedica);
        }

        // GET: Entidadmedicas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Entidadmedicas == null)
            {
                return NotFound();
            }

            var entidadmedica = await _context.Entidadmedicas.FindAsync(id);
            if (entidadmedica == null)
            {
                return NotFound();
            }
            return View(entidadmedica);
        }

        // POST: Entidadmedicas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Direccion")] Entidadmedica entidadmedica)
        {
            if (id != entidadmedica.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var entidadJson = JsonConvert.SerializeObject(entidadmedica);
                    var content = new StringContent(entidadJson, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"{URL}/{id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(entidadmedica);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

          
            return View(entidadmedica);
        }

        // GET: Entidadmedicas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"{URL}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var entidadJson = await response.Content.ReadAsStringAsync();
                var entidad1 = JsonConvert.DeserializeObject<Entidadmedica>(entidadJson);

                if (entidad1 == null)
                {
                    return NotFound();
                }

                return View(entidad1);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Entidadmedicas/Delete/5
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

        private bool EntidadmedicaExists(int id)
        {
          return (_context.Entidadmedicas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
