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
    public class PagosController : Controller
    {
        private readonly ILogger<PagosController> _logger;

        private readonly RepositorioPagos repositorioPagos;
        private readonly RepositorioContratos repositorioContratos;

        public PagosController(ILogger<PagosController> logger, IConfiguration config)
        {
            this.repositorioPagos = new RepositorioPagos(config);
            this.repositorioContratos = new RepositorioContratos(config);
            _logger = logger;
        }

        // GET: 
        [Authorize]
        public IActionResult Index()
        {
            var lta = repositorioPagos.obtener();
            ViewData[nameof(Pagos)] = lta;
            return View();
        }

        public ActionResult PorContrato(int id)
        {
            var res = repositorioContratos.Buscar(id);
            ViewData[nameof(Contratos)] = res;
            var lista = repositorioPagos.BuscarPorContrato(id);            
            ViewData[nameof(Pagos)] = lista;
            return View();
        }

        // GET: 
        [Authorize]
        public IActionResult Alta()
        {
            var lta = repositorioContratos.obtener();
            ViewData[nameof(Contratos)] = lta;
            return View();
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Alta(Pagos i)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    repositorioPagos.Alta(i);
                    return RedirectToAction("Index");
                }
                else
                {
                    var lta = repositorioContratos.obtener();
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
            Pagos i = repositorioPagos.BuscarPorPagos(id);
            return View(i);
        }


        // 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Editar(Pagos i)
        {
            try
            {
            repositorioPagos.Editar(i);

            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(i);
            }
        }

        //
        [Authorize(Policy = "Administrador")]
        public IActionResult Delete(int id)
        {
            repositorioPagos.Borrar(id);
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

        // GET
        [Authorize]
        public IActionResult Pagar(int id)
        {
            var max = repositorioPagos.numSigPago(id);
            ViewData["max"] = max;
            var res = repositorioContratos.Buscar(id);
            ViewData[nameof(Contratos)] = res;
            Pagos i = repositorioPagos.Buscar(id);
            return View(i);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Pagar(Pagos i)
        {
            try
            {
            repositorioPagos.Alta(i);
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(i);
            }
        }
    }
}