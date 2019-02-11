using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Comun;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Modelo;
using Negocio;

namespace HostToHost.Controllers
{
    [Authorize]
    public class PrivadoController : Controller
    {
        private CuentaNE _cuentaNE = null;
        private IHttpContextAccessor _httpContextAccessor;

        public PrivadoController(IConfiguration configuration, UserManager<IdentityUserMO> userManager, SignInManager<IdentityUserMO> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _cuentaNE = _cuentaNE ?? new CuentaNE(configuration, userManager, signInManager);
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Inicio()
        {
            return View();
        }

        public IActionResult Modificar()
        {
            return View();
        }

        public IActionResult Actualizar()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> CerrarSesionAsync()
        {
            Object objeto = null;
            try
            {
                Boolean IsAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (IsAuthenticated)
                {
                    objeto = await _cuentaNE.CerrarSesionAsync(new CancellationToken(false));
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_AUTENTICADO,
                        mensaje = Constante.MENSAJE_NO_AUTENTICADO,
                        url = Constante.URL_PUBLICO_LOGUEAR
                    };
                }
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

        [HttpGet]
        public async Task<JsonResult> ConsultarNombreUsuarioAsync()
        {
            Object objeto = null;
            try
            {
                Boolean IsAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (IsAuthenticated)
                {
                    String usuario = _httpContextAccessor.HttpContext.User.Identity.Name;
                    objeto = await _cuentaNE.ConsultarNombreUsuarioAsync(new CancellationToken(false), usuario);
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_AUTENTICADO,
                        mensaje = Constante.MENSAJE_NO_AUTENTICADO,
                        url = Constante.URL_PUBLICO_LOGUEAR
                    };
                }
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
        public async Task<JsonResult> ModificarClaveAsync([FromBody] String[] parametros)
        {
            Object objeto = null;
            try
            {
                Boolean IsAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (IsAuthenticated)
                {
                    String usuario = _httpContextAccessor.HttpContext.User.Identity.Name;
                    objeto = await _cuentaNE.ModificarClaveAsync(new CancellationToken(false), parametros, usuario);
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_AUTENTICADO,
                        mensaje = Constante.MENSAJE_NO_AUTENTICADO,
                        url = Constante.URL_PUBLICO_LOGUEAR
                    };
                }
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

        [HttpGet]
        public async Task<JsonResult> ConsultarUsuarioAsync()
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String usuario = _httpContextAccessor.HttpContext.User.Identity.Name;
                    objeto = await _cuentaNE.ConsultarUsuarioAsync(new CancellationToken(false), usuario);
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_AUTENTICADO,
                        mensaje = Constante.MENSAJE_NO_AUTENTICADO,
                        url = Constante.URL_PUBLICO_LOGUEAR
                    };
                }
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
        public async Task<JsonResult> ActualizarDatosAsync([FromBody] UsuarioMO usuarioMO)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _cuentaNE.EditarUsuarioAsync(new CancellationToken(false), usuarioMO);
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_AUTENTICADO,
                        mensaje = Constante.MENSAJE_NO_AUTENTICADO,
                        url = Constante.URL_PUBLICO_LOGUEAR
                    };
                }
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