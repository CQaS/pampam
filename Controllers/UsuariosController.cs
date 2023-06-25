using InmobiliariaVaras.Models;
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
    public class UsuariosController : Controller
    {
        private readonly ILogger<UsuariosController> _logger;

        private readonly RepositorioUsuarios repositorioUsuarios;
        private readonly RepositorioPropietarios repositorioPropietarios;
        private readonly RepositorioInquilinos repositorioInquilinos;
        private readonly RepositorioInmuebles repositorioInmuebles;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public UsuariosController(ILogger<UsuariosController> logger, IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.environment = environment;
            this.configuration = configuration;
            this.repositorioUsuarios = new RepositorioUsuarios(configuration);
            this.repositorioPropietarios = new RepositorioPropietarios(configuration);
            this.repositorioInquilinos = new RepositorioInquilinos(configuration);
            this.repositorioInmuebles = new RepositorioInmuebles(configuration);
            _logger = logger;
        }

        // GET: Usuarios
        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var usuarios = repositorioUsuarios.ObtenerTodos();
            ViewData[nameof(Usuarios)] = usuarios;
            ViewBag.id = TempData["id"];
            if (TempData.ContainsKey("id"))
                ViewBag.id = TempData["id"];
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            return View();
        }

        // GET: Usuarios/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Alta()
        {
            ViewBag.Roles = Usuarios.ObtenerRoles();
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Alta(Usuarios u)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Roles = Usuarios.ObtenerRoles();
                    if(u.id_Us == 0)
                       TempData["Mensaje"] = "Ingrese todos los datos del usuario!";
                       ViewBag.Error= TempData["Mensaje"];
                    return View();
                }
                {
                    var user = repositorioUsuarios.ObtenerPorEmail(u.email);

                    if (user == null )
                    {

                        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: u.contraseña,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                        u.contraseña = hashed;

                        u.rol = User.IsInRole("Administrador") ? u.rol : (int)enRoles.Empleado;
                        var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
                        int res = repositorioUsuarios.Alta(u);

                        if(u.AvatarFile !=null  && res > 0)
                        {
                            u.id_Us = res;
                            string wwwPath = environment.WebRootPath;
                            string path = Path.Combine(wwwPath, "img/avatars");
                            if(!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            string fileName = "avatar_" + u.email + Path.GetExtension(u.AvatarFile.FileName);
                            string pathCompleto = Path.Combine(path, fileName);
                            u.avatar = Path.Combine("/img/avatars", fileName);
                            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                            {
                                u.AvatarFile.CopyTo(stream);
                            }
                            
                            repositorioUsuarios.agregaAvatar(u);
                        }
                        TempData["id"] = u.id_Us;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Mensaje"] = "El Email o Usuario ingresado ya se encuentra registrado en el sistema!";
                        ViewBag.Error = TempData["Mensaje"];
                        ViewBag.Roles = Usuarios.ObtenerRoles();
                        return View();
                    }                      

                }                
                
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                ViewBag.Roles = Usuarios.ObtenerRoles();
               
                return View();
            }
        }


        // GET: Usuario/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Editar(int id)
        {
            ViewData["Title"] = "Editar usuario";
            var u = repositorioUsuarios.ObtenerPorId(id);
            ViewBag.Roles = Usuarios.ObtenerRoles();
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(u);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Editar(int id, Usuarios u)
        {
            var vista = "Editar";
            try
            {
                if (!User.IsInRole("Administrador"))
                {
                    Usuarios usuarioAeditar = repositorioUsuarios.ObtenerPorId(u.id_Us);
                    if (usuarioAeditar.id_Us != id)//si no es admin, solo puede modificarse él mismo
                        return RedirectToAction(nameof(Index), "Home");
                    else
                    {
                        u.rol = usuarioAeditar.rol;

                        if(u.contraseña !=null)
                        {
                            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: u.contraseña,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                            u.contraseña = hashed;
                        }
                        else
                        {
                            u.contraseña = usuarioAeditar.contraseña; 
                        }

                        if(u.AvatarFile !=null)
                        {
                            string wwwPath = environment.WebRootPath;
                            string path = Path.Combine(wwwPath, "img/avatars");
                            if(!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            
                            string fileName = "avatar_" + u.id_Us + Path.GetExtension(u.AvatarFile.FileName);
                            string pathCompleto = Path.Combine(path, fileName);
                            u.avatar = Path.Combine("/img/avatars", fileName);
                            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                            {
                                u.AvatarFile.CopyTo(stream);
                            }

                            repositorioUsuarios.Modifica(u);
                        }
                        else
                        {
                            u.avatar = usuarioAeditar.avatar;
                            repositorioUsuarios.Modifica(u); 
                        }
                        
                        
                        TempData["Mensaje"] = "Datos guardados correctamente"; 
                        if (TempData.ContainsKey("Mensaje"))
                            ViewBag.Mensaje = TempData["Mensaje"];

                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    Usuarios usuarioAeditar = repositorioUsuarios.ObtenerPorId(u.id_Us);
                    if(u.contraseña !=null)
                    {
                        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: u.contraseña,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                        u.contraseña = hashed;
                    }
                    else
                    {
                        u.contraseña = usuarioAeditar.contraseña; 
                    }
                    
                    if(u.AvatarFile !=null)
                    {
                            string wwwPath = environment.WebRootPath;
                            string path = Path.Combine(wwwPath, "img/avatars");
                            if(!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                            string fileName = "avatar_" + u.id_Us + Path.GetExtension(u.AvatarFile.FileName);
                            string pathCompleto = Path.Combine(path, fileName);
                            u.avatar = Path.Combine("/img/avatars", fileName);
                            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                            {
                                u.AvatarFile.CopyTo(stream);
                            }
                            repositorioUsuarios.Modifica(u);
                    }
                    else
                    {
                        u.avatar = usuarioAeditar.avatar;
                        repositorioUsuarios.Modifica(u); 
                    }
                    
                    TempData["Mensaje"] = "Datos guardados correctamente";

                }

                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Error = TempData["Mensaje"];

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                ViewBag.Roles = Usuarios.ObtenerRoles();
                TempData["Mensaje"] = "Verifique nuevamente, ha ocurrido error!";
                return View(vista, u);
            }
        }

        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Borrar(int id)
        {
            repositorioUsuarios.Baja(id);
            return RedirectToAction(nameof(Index));
        }

	    [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login(string returnUrl)
        {
            TempData["returUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as String) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.contraseña,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repositorioUsuarios.ObtenerPorEmail(login.usuario);

                    if(String.IsNullOrEmpty(login.pregunta))
                    {
                        if (e == null || e.contraseña != hashed)
                        {
                            ModelState.AddModelError("", "Datos Incorrectos");
                            TempData["returnUrl"] = returnUrl;
                            return View();
                        }

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, e.email),
                            new Claim("FullName", e.nombre + " " + e.apellido),
                            new Claim(ClaimTypes.Role, e.rolNombre),
                        };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));
                        
                        return RedirectToAction(nameof(Index), "Home");
                    }
                    else
                    {
                        if (e == null || login.pregunta != e.pregunta)
                        {
                            ModelState.AddModelError("", "Los datos ingresados, no son correctos");
                            return View();
                        }
                        else
                        {
                            string hashedNew = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: login.contraseña,
                                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
                            
                            repositorioUsuarios.ModificarPass(e.email, hashedNew); 
                            ModelState.AddModelError("", "Contraseña restaurada");
                            return View(); 
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Usuario/recuperaPass
        public ActionResult RecuperaPass()
        {
            return View();
        }

        // GET: Usuario/Logout
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}