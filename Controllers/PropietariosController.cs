using InmobiliariaVaras.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaVaras.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly ILogger<PropietariosController> _logger;
        private readonly RepositorioPropietarios repositorioPropietarios;
        private readonly RepositorioInmuebles repositorioInmuebles;

       public PropietariosController(ILogger<PropietariosController> logger, IConfiguration config)
        {
            
            this.repositorioPropietarios = new RepositorioPropietarios(config);
            this.repositorioInmuebles = new RepositorioInmuebles(config);
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            var lta = repositorioPropietarios.obtener();
            ViewData[nameof(Propietarios)] = lta;
            return View();

          }

       // GET:
       [Authorize] 
        public IActionResult Alta()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Alta(Propietarios p) // retorna una vista para dar alta a un prop
        {
            try
            {
            repositorioPropietarios.Alta(p);
            return RedirectToAction("Index");
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
        public IActionResult Editar(int id) //retorna la vista editar con un Prop

        {
            Propietarios p = repositorioPropietarios.Buscar(id); 
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Editar(Propietarios p) //guarda los datos del Prop

        {
            try
            {
            repositorioPropietarios.Editar(p);
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(p);
            }
        }

        [Authorize]
        public IActionResult Detalles(int id) //detalles de un Prop
        {
            var lta = repositorioInmuebles.obtenerPorPropietario(id);
            ViewData[nameof(Inmuebles)] = lta;
            Propietarios p = repositorioPropietarios.Buscar(id); 
            return View(p);
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Delete(int id) //elimina un Prop
        {
            repositorioPropietarios.Borrar(id);
            return RedirectToAction("Index");
        }
    
    }
}