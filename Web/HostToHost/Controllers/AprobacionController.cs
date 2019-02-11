using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Comun;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Negocio;

namespace HostToHost.Controllers
{
    [Authorize(Roles = Constante.ROL_APROBADOR)]
    public class AprobacionController : Controller
    {
        private AprobacionNE _aprobacionNE = null;
        private FiltroNE _filtroNE = null;
        private IHttpContextAccessor _httpContextAccessor;

        public AprobacionController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _aprobacionNE = _aprobacionNE ?? new AprobacionNE(configuration);
            _filtroNE = _filtroNE ?? new FiltroNE(configuration);
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult ListarOrdenesBancariasPorAprobar()
        {
            return View();
        }

        public IActionResult ListarOrdenesBancariasAnuladas()
        {
            return View();
        }

        public IActionResult ListarOrdenesBancariasAprobadas()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaPorAprobar()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaAprobada()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaAnulada()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaDetallePorAprobar()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaDetalleAprobada()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaDetalleAnulada()
        {
            return View();
        }

        public IActionResult AprobarOrdenBancariaLiberada()
        {
            return View();
        }

        public IActionResult AprobarOrdenBancariaDetalleLiberada()
        {
            return View();
        }

        public IActionResult AnularOrdenBancariaLiberada()
        {
            return View();
        }

        public IActionResult AnularOrdenBancariaDetalleLiberada()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> ListarOrdenesBancariasPorAprobarAsync([FromQuery] Int32 pagina, [FromQuery] Int32 filas, [FromQuery] String idSociedad, [FromQuery] String idBanco, [FromQuery] String idTipoOrden, [FromQuery] String idEstadoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _aprobacionNE.ListarOrdenesBancariasPorAprobarAsync(new CancellationToken(false), idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden, idEstadoOrden);
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
        public async Task<JsonResult> ListarOrdenesBancariasDetallePorAprobarAsync([FromQuery] String idSociedad, [FromQuery] String idSap, [FromQuery] String anio, [FromQuery] String momentoOrden, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _aprobacionNE.ListarOrdenesBancariasDetallePorAprobarAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden);
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
        public async Task<JsonResult> BuscarOrdenesBancariasPorAprobarAsync([FromQuery] String idSap, [FromQuery] String fechaInicio, [FromQuery] String fechaFin)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _aprobacionNE.BuscarOrdenesBancariasPorAprobarAsync(new CancellationToken(false), idUsuario, idSap, fechaInicio, fechaFin);
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
        public async Task<IActionResult> ObtenerVistaParcialOrdenBancariaPorAprobarAsync([FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _aprobacionNE.ObtenerVistaParcialOrdenBancariaPorAprobarAsync(new CancellationToken(false), this, idTipoOrden);
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
        public async Task<IActionResult> ObtenerVistaParcialOrdenBancariaDetallePorAprobarAsync([FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _aprobacionNE.ObtenerVistaParcialOrdenBancariaDetallePorAprobarAsync(new CancellationToken(false), this, idTipoOrden);
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
        public async Task<JsonResult> ListarOrdenesBancariasAprobadasAsync([FromQuery] Int32 pagina, [FromQuery] Int32 filas, [FromQuery] String idSociedad, [FromQuery] String idBanco, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _aprobacionNE.ListarOrdenesBancariasAprobadasAsync(new CancellationToken(false), idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden);
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_AUTENTICADO,
                        mensaje = Constante.MENSAJE_NO_AUTENTICADO
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
        public async Task<JsonResult> ListarOrdenesBancariasDetalleAprobadasAsync([FromQuery] String idSociedad, [FromQuery] String idSap, [FromQuery] String anio, [FromQuery] String momentoOrden, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _aprobacionNE.ListarOrdenesBancariasDetalleAprobadasAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden);
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
        public async Task<JsonResult> BuscarOrdenesBancariasAprobadasAsync([FromQuery] String idSap, [FromQuery] String fechaInicio, [FromQuery] String fechaFin)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _aprobacionNE.BuscarOrdenesBancariasAprobadasAsync(new CancellationToken(false), idUsuario, idSap, fechaInicio, fechaFin);
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
        public async Task<IActionResult> ObtenerVistaParcialOrdenBancariaAprobadaAsync([FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _aprobacionNE.ObtenerVistaParcialOrdenBancariaAprobadaAsync(new CancellationToken(false), this, idTipoOrden);
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
        public async Task<IActionResult> ObtenerVistaParcialOrdenBancariaDetalleAprobadaAsync([FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _aprobacionNE.ObtenerVistaParcialOrdenBancariaDetalleAprobadaAsync(new CancellationToken(false), this, idTipoOrden);
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
        public async Task<JsonResult> ConsultarEstadoOrdenBancariaAsync([FromQuery] String idSociedad, [FromQuery] String idSap, [FromQuery] String anio, [FromQuery] String momentoOrden, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _aprobacionNE.ConsultarEstadoOrdenBancariaAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden);
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_AUTENTICADO,
                        mensaje = Constante.MENSAJE_NO_AUTENTICADO
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
        public async Task<JsonResult> AprobarOrdenBancariaLiberadaAsync([FromBody] String[] parametros)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _aprobacionNE.AprobarOrdenBancariaLiberadaAsync(new CancellationToken(false), parametros, idUsuario);
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_AUTENTICADO,
                        mensaje = Constante.MENSAJE_NO_AUTENTICADO
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
        public async Task<JsonResult> AnularOrdenBancariaLiberadaAsync([FromBody] String[] parametros)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    String usuario = _httpContextAccessor.HttpContext.User.Identity.Name;
                    objeto = await _aprobacionNE.AnularOrdenBancariaLiberadaAsync(new CancellationToken(false), parametros, idUsuario, usuario);
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_AUTENTICADO,
                        mensaje = Constante.MENSAJE_NO_AUTENTICADO
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
        public async Task<JsonResult> ConsultarEstadoMasivoOrdenesBancariasAsync([FromQuery] String cadena)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _aprobacionNE.ConsultarEstadoMasivoOrdenesBancariasAsync(new CancellationToken(false), cadena);
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_AUTENTICADO,
                        mensaje = Constante.MENSAJE_NO_AUTENTICADO
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
        public async Task<JsonResult> AprobarMasivoOrdenesBancariasAsync([FromBody] String[] parametros)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _aprobacionNE.AprobarMasivoOrdenesBancariasAsync(new CancellationToken(false), parametros, idUsuario);
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_AUTENTICADO,
                        mensaje = Constante.MENSAJE_NO_AUTENTICADO
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
        public async Task<JsonResult> ListarFlujoAprobacionAsync([FromQuery] String idSociedad, [FromQuery] String idSap, [FromQuery] String anio, [FromQuery] String momentoOrden, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _aprobacionNE.ListarFlujoAprobacionAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden);
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_AUTENTICADO,
                        mensaje = Constante.MENSAJE_NO_AUTENTICADO
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
        public async Task<JsonResult> ListarOrdenesBancariasAnuladasAsync([FromQuery] Int32 pagina, [FromQuery] Int32 filas, [FromQuery] String idSociedad, [FromQuery] String idBanco, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _aprobacionNE.ListarOrdenesBancariasAnuladasAsync(new CancellationToken(false), idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden);
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
        public async Task<JsonResult> ListarOrdenesBancariasDetalleAnuladasAsync([FromQuery] String idSociedad, [FromQuery] String idSap, [FromQuery] String anio, [FromQuery] String momentoOrden, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _aprobacionNE.ListarOrdenesBancariasDetalleAnuladasAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden);
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
        public async Task<JsonResult> BuscarOrdenesBancariasAnuladasAsync([FromQuery] String idSap, [FromQuery] String fechaInicio, [FromQuery] String fechaFin)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _aprobacionNE.BuscarOrdenesBancariasAnuladasAsync(new CancellationToken(false), idUsuario, idSap, fechaInicio, fechaFin);
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
        public async Task<IActionResult> ObtenerVistaParcialOrdenBancariaAnuladaAsync([FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _aprobacionNE.ObtenerVistaParcialOrdenBancariaAnuladaAsync(new CancellationToken(false), this, idTipoOrden);
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
        public async Task<IActionResult> ObtenerVistaParcialOrdenBancariaDetalleAnuladaAsync([FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _aprobacionNE.ObtenerVistaParcialOrdenBancariaDetalleAnuladaAsync(new CancellationToken(false), this, idTipoOrden);
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
        public async Task<IActionResult> ListarFiltrosOrdenesBancariasPorAprobarAsync()
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _filtroNE.ListarFiltrosOrdenesBancariasPorAprobarAsync(new CancellationToken(false), idUsuario);
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
        public async Task<IActionResult> ListarFiltrosOrdenesBancariasAprobadasAsync()
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _filtroNE.ListarFiltrosOrdenesBancariasAprobadasAsync(new CancellationToken(false), idUsuario);
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
        public async Task<IActionResult> ListarFiltrosOrdenesBancariasAnuladasAsync()
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _filtroNE.ListarFiltrosOrdenesBancariasAnuladasAsync(new CancellationToken(false), idUsuario);
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