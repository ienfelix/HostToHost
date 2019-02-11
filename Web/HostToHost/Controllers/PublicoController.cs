using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Comun;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Modelo;
using Negocio;

namespace HostToHost.Controllers
{
    [AllowAnonymous]
    public class PublicoController : Controller
    {
        private CuentaNE _cuentaNE = null;

        public PublicoController(IConfiguration configuration, UserManager<IdentityUserMO> userManager, SignInManager<IdentityUserMO> signInManager)
        {
            _cuentaNE = _cuentaNE ?? new CuentaNE(configuration, userManager, signInManager);
        }

        public IActionResult Loguear()
        {
            return View();
        }

        public IActionResult Denegar()
        {
            return View();
        }

        public IActionResult Modificar()
        {
            return View();
        }

        public IActionResult Recuperar()
        {
            return View();
        }

        public IActionResult Reiniciar()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> IniciarSesionAsync([FromBody] String[] parametros)
        {
            Object objeto = null;
            try
            {
                objeto = await _cuentaNE.IniciarSesionAsync(new CancellationToken(false), parametros);
            }
            catch (Exception e)
            {
                objeto = new
                {
                    codigo = Constante.CODIGO_ERROR,
                    mensaje = e.Message
                };
                return Json(objeto);
            }
            return Json(objeto);
        }

        [HttpPost]
        public async Task<JsonResult> SolicitarCodigoTokenAsync([FromBody] String usuario)
        {
            Object objeto = null;
            try
            {
                objeto = await _cuentaNE.SolicitarCodigoTokenAsync(new CancellationToken(false), usuario);
            }
            catch (Exception e)
            {
                objeto = new
                {
                    codigo = Constante.CODIGO_NO_OK,
                    mensaje = e.Message
                };
                return Json(objeto);
            }
            return Json(objeto);
        }

        [HttpGet]
        public async Task<JsonResult> RecuperarClaveAsync([FromQuery] String usuario, [FromQuery] String correo)
        {
            Object objeto = null;
            try
            {
                objeto = await _cuentaNE.RecuperarClaveAsync(new CancellationToken(false), usuario, correo);
            }
            catch (Exception e)
            {
                objeto = new
                {
                    codigo = Constante.CODIGO_ERROR,
                    mensaje = e.Message
                };
                return Json(objeto);
            }
            return Json(objeto);
        }

        [HttpPost]
        public async Task<JsonResult> ReiniciarClaveAsync([FromBody] String[] parametros)
        {
            Object objeto = null;
            try
            {
                objeto = await _cuentaNE.ReiniciarClaveAsync(new CancellationToken(false), parametros);
            }
            catch (Exception e)
            {
                objeto = new 
                {
                    codigo = Constante.CODIGO_ERROR,
                    mensaje = e.Message
                };
                return Json(objeto);
            }
            return Json(objeto);
        }
    }
}