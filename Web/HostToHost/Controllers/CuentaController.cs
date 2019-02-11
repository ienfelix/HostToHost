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
    [Authorize(Roles = Constante.ROL_ADMINISTRADOR + "," + Constante.ROL_COORDINADOR)]
    public class CuentaController : Controller
    {
        private CuentaNE _cuentaNE = null;
        private IHttpContextAccessor _httpContextAccessor;

        public CuentaController(IConfiguration configuration, UserManager<IdentityUserMO> userManager, SignInManager<IdentityUserMO> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _cuentaNE = _cuentaNE ?? new CuentaNE(configuration, userManager, signInManager);
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Listar()
        {
            return View();
        }

        public IActionResult Crear()
        {
            return View();
        }

        public IActionResult Editar()
        {
            return View();
        }

        public IActionResult Inactivar()
        {
            return View();
        }

        public IActionResult Ver()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> ListarUsuariosAsync([FromQuery] Int32 pagina, [FromQuery] Int32 filas)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _cuentaNE.ListarUsuariosAsync(new CancellationToken(false), pagina, filas);
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
        public async Task<JsonResult> BuscarUsuariosAsync([FromQuery] String usuario, [FromQuery] String apePaterno, [FromQuery] String nombres)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _cuentaNE.BuscarUsuariosAsync(new CancellationToken(false), usuario, apePaterno, nombres);
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
        public async Task<JsonResult> CrearUsuarioAsync([FromBody] UsuarioMO usuarioMO)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _cuentaNE.CrearUsuarioAsync(new CancellationToken(false), usuarioMO);
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
        public async Task<JsonResult> EditarUsuarioAsync([FromBody] UsuarioMO usuarioMO)
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

        [HttpPut]
        public async Task<JsonResult> InactivarUsuarioAsync([FromBody] String idUsuario)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _cuentaNE.InactivarUsuarioAsync(new CancellationToken(false), idUsuario);
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