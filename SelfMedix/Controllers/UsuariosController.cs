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
    public class UsuariosController : Controller
    {
        private readonly SelfmedixContext _context;
        private string URL = "http://localhost:5183/api/Usuarios";
        private readonly HttpClient _httpClient;

        public UsuariosController()
        {
            _context = new SelfmedixContext();
            _httpClient = new HttpClient();
        }
        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var httpResponse = await _httpClient.GetAsync(URL);
            if (httpResponse.IsSuccessStatusCode)
            {
                var result = httpResponse.Content.ReadAsStringAsync().Result;
                List<Usuario>? modelList = JsonConvert.DeserializeObject<List<Usuario>>(result);
                return View(modelList);

            }
            else return BadRequest(Results.NotFound());
        }

        // GET: Usuarios/Details/5
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
                var cita = JsonConvert.DeserializeObject<Usuario>(result);

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

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombres,Apellidos,FechaNacimiento,FechaCreacion,FechaElimina,Vigente,UrlImg,Correo,Contrasenia")] Usuario usuario)
        {
            // Convertir la cita en un objeto JSON
            var usuarioJson = JsonConvert.SerializeObject(usuario);

            // Crear un objeto HttpContent a partir del JSON
            var content = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

            // Hacer una solicitud POST a la URL de la API
            var response = await _httpClient.PostAsync(URL, content);

            // Verificar si la solicitud fue exitosa
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);

        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombres,Apellidos,FechaNacimiento,FechaCreacion,FechaElimina,Vigente,UrlImg,Correo,Contrasenia")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioJson = JsonConvert.SerializeObject(usuario);
                    var content = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"{URL}/{id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(usuario);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

         
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"{URL}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var usuarioJson = await response.Content.ReadAsStringAsync();
                var usuario1 = JsonConvert.DeserializeObject<Usuario>(usuarioJson);

                if (usuario1 == null)
                {
                    return NotFound();
                }

                return View(usuario1);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Usuarios/Delete/5
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

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
