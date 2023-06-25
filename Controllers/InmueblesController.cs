using InmobiliariaVaras.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Claims;

namespace InmobiliariaVaras.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly ILogger<InmueblesController> _logger;
        private readonly RepositorioInmuebles repositorioInmuebles;
        private readonly RepositorioPropietarios repositorioPropietarios;
        private readonly RepositorioContratos repositorioContratos;
        private readonly IWebHostEnvironment environment;


        public InmueblesController(ILogger<InmueblesController> logger, IWebHostEnvironment environment, IConfiguration config)
        {
            this.environment = environment;
            this.repositorioInmuebles = new RepositorioInmuebles(config);
            this.repositorioPropietarios = new RepositorioPropietarios(config);
            this.repositorioContratos = new RepositorioContratos(config);
            _logger = logger;
        }

        // GET:
        [Authorize]
        public IActionResult Index()
        {
            var lta = repositorioInmuebles.obtener();
            ViewData[nameof(Inmuebles)] = lta;
            ViewData["listaInmu"] = "Inmuebles Actuales";
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult BuscarPorFecha(BuscarPorFecha busqueda)
        {

            var lista = repositorioInmuebles.BuscarInmueblesDisponibles(busqueda);

            ViewData[nameof(Inmuebles)] = lista;
            ViewData["listaPorFecha"] = "Periodo de busqueda: " + busqueda.FechaInicio.ToShortDateString() + " - " + busqueda.FechaFin.ToShortDateString();

            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];


            return View(nameof(Index));

        }

        [Authorize]
        public IActionResult Alta()
        {
            var lta = repositorioPropietarios.obtener();
            ViewData[nameof(Propietarios)] = lta;
            return View();
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Alta(Inmuebles b)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (b.imagenFile != null)
                    {
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "img/Inmueble_CodProp_" + b.prop_Id);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = "Inmueble_" + DateTime.Now.ToString("dd_MM_yyyy") + DateTime.Now.ToString("hh_mm_ss") + Path.GetExtension(b.imagenFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        b.imagen = Path.Combine("/img/Inmueble_CodProp_" + b.prop_Id, fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            b.imagenFile.CopyTo(stream);
                        }

                        repositorioInmuebles.Alta(b);

                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "No se pudo cargar con éxito Alta de Inmueble, por favor, intente nuevamente!";
                    var lta = repositorioPropietarios.obtener();
                    ViewData[nameof(Propietarios)] = lta;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return RedirectToAction("Index");
            }
        }

        // GET
        [Authorize]
        public IActionResult Editar(int id)

        {
            var lta = repositorioPropietarios.obtener();
            ViewData[nameof(Propietarios)] = lta;
            Inmuebles b = repositorioInmuebles.Buscar(id);
            return View(b);
        }


        // 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Editar(Inmuebles b)
        {
            try
            {
                var inmuebleActual = repositorioInmuebles.Buscar(b.id_Inm);

                if (b.imagenFile != null)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "img/Inmueble_CodProp_" + inmuebleActual.prop_Id);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "Inmueble_" + DateTime.Now.ToString("dd_MM_yyyy") + DateTime.Now.ToString("hh_mm_ss") + Path.GetExtension(b.imagenFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    b.imagen = Path.Combine("/img/Inmueble_CodProp_" + inmuebleActual.prop_Id, fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        b.imagenFile.CopyTo(stream);
                    }
                }
                else
                {
                    b.imagen = inmuebleActual.imagen;
                }

                repositorioInmuebles.Editar(b);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(b);
            }
        }

        [Authorize]
        public IActionResult Detalles(int id)
        {
            Inmuebles b = repositorioInmuebles.Buscar(id);
            return View(b);
        }


        // 
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete(int id)
        {
            repositorioInmuebles.Borrar(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Disponible(int id)
        {
            var ok = repositorioInmuebles.Disponible(id);
            return Json(ok);
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id_Inm, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public IActionResult VerContratos(int id)
        {
            var lta = repositorioContratos.VerContratosDelInmueble(id);
            ViewData[nameof(Contratos)] = lta;
            return View();
        }

    }
}
