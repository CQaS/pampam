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
    public class InquilinosController : Controller
    {
        private readonly ILogger<InquilinosController> _logger;
        private readonly RepositorioInquilinos repositorioInquilinos;

       public InquilinosController(ILogger<InquilinosController> logger, IConfiguration config)
        {
            
            this.repositorioInquilinos = new RepositorioInquilinos(config);
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            var lta = repositorioInquilinos.obtener();
            ViewData[nameof(Inquilinos)] = lta;
            return View();

        }

        //search_Instantanea_AJAX_JSON
        [HttpPost]
        public JsonResult Search(string Name)
        {
            var lta = repositorioInquilinos.search(Name);
            return Json(lta);
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
        public IActionResult Alta(Inquilinos i) // retorna una vista para dar alta a un inquilino
        {
            try
            {
            repositorioInquilinos.Alta(i);
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
        public IActionResult Editar(int id) //retorna la vista editar con un Inquilino

        {
            Inquilinos i = repositorioInquilinos.Buscar(id); 
            return View(i);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Editar(Inquilinos i) //guarda los datos del Inquilino

        {
            try
            {
            repositorioInquilinos.Editar(i);
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
        public IActionResult Detalles(int id) //detalles de un Inquilino
        {
            Inquilinos i = repositorioInquilinos.Buscar(id); 
            return View(i);
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult Delete(int id) //elimina un Inquilino
        {
            repositorioInquilinos.Borrar(id);
            return RedirectToAction("Index");
        }


       
    }
}