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
    public class RolController : Controller
    {
        private CuentaNE _cuentaNE = null;
        private RolNE _rolNE = null;
        private IHttpContextAccessor _httpContextAccessor;

        public RolController(IConfiguration configuration, UserManager<IdentityUserMO> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUserMO> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _cuentaNE = _cuentaNE ?? new CuentaNE(configuration, userManager, signInManager);
            _rolNE = _rolNE ?? new RolNE(configuration, userManager, roleManager);
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

        public IActionResult Asignar()
        {
            return View();
        }

        public IActionResult Desasignar()
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

        [HttpGet]
        public async Task<JsonResult> ListarNombresUsuariosAsync()
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _rolNE.ListarNombresUsuariosAsync(new CancellationToken(false));
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
        public async Task<JsonResult> ListarRolesAsync()
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _rolNE.ListarRolesAsync(new CancellationToken(false));
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
        public async Task<JsonResult> ListarRolesUsuarioAsync([FromQuery] String usuario)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _rolNE.ListarRolesUsuarioAsync(new CancellationToken(false), usuario);
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
        public async Task<Object> AsignarRolAsync([FromBody] String[] parametros)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _rolNE.AsignarRolAsync(new CancellationToken(false), parametros);
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
        public async Task<Object> DesasignarRolAsync([FromBody] String[] parametros)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _rolNE.DesasignarRolAsync(new CancellationToken(false), parametros);
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