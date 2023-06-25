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
    public class ContratosController : Controller
    {
        private readonly ILogger<ContratosController> _logger;

        private readonly RepositorioContratos repositorioContratos;
        private readonly RepositorioPropietarios repositorioPropietarios;
        private readonly RepositorioInquilinos repositorioInquilinos;
        private readonly RepositorioInmuebles repositorioInmuebles;

        public ContratosController(ILogger<ContratosController> logger, IConfiguration config)
        {
            this.repositorioContratos = new RepositorioContratos(config);
            this.repositorioPropietarios = new RepositorioPropietarios(config);
            this.repositorioInquilinos = new RepositorioInquilinos(config);
            this.repositorioInmuebles = new RepositorioInmuebles(config);
            _logger = logger;
        }

        // GET:
        [Authorize]
        public IActionResult Index()
        {
            var lta = repositorioContratos.obtener();
            ViewData[nameof(Contratos)] = lta;
            ViewBag.multa = TempData["multa"];
            return View();
        }

        // GET:
        [Authorize]
        public IActionResult Alta()
        {
            var lta = repositorioInmuebles.obtenerDisponibles();
            ViewData[nameof(Inmuebles)] = lta;
            var lta2 = repositorioInquilinos.obtener();
            ViewData[nameof(Inquilinos)] = lta2;
            return View();
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Alta(Contratos i)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repositorioContratos.Alta(i);
                    return RedirectToAction("Index");
                }
                else
                {
                    var lta = repositorioPropietarios.obtener();
                    ViewData[nameof(Propietarios)] = lta;
                    return View(i);
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
            Contratos i = repositorioContratos.Buscar(id);
            var lta = repositorioInmuebles.obtener();
            ViewData[nameof(Inmuebles)] = lta;
            var lta2 = repositorioInquilinos.obtener();
            ViewData[nameof(Inquilinos)] = lta2;
            return View(i);
        }


        // 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Editar(Contratos i)
        {
            try
            {
                repositorioContratos.Editar(i);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(i);
            }
        }

        [Authorize]
        public IActionResult Detalles(int id)
        {
            Contratos i = repositorioContratos.Buscar(id);
            var lta = repositorioInmuebles.Buscar(i.inm_Id);
            ViewData[nameof(Inmuebles)] = lta;
            var lta2 = repositorioInquilinos.Buscar(i.inq_Id);
            ViewData[nameof(Inquilinos)] = lta2;
            return View(i);
        }

        // 
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete(int id)
        {
            var multa = repositorioContratos.Borrar(id);
            TempData["multa"] = multa;
            return RedirectToAction("Index");
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
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
    }
}