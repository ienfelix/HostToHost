using System;
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
    [Authorize(Roles = Constante.ROL_TESORERO)]
    public class TesoreriaController : Controller
    {
        private TesoreriaNE _tesoreriaNE = null;
        private FiltroNE _filtroNE = null;
        private IHttpContextAccessor _httpContextAccessor;

        public TesoreriaController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _tesoreriaNE = _tesoreriaNE ?? new TesoreriaNE(configuration);
            _filtroNE = _filtroNE ?? new FiltroNE(configuration);
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult ListarOrdenesBancariasLiberadas()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaLiberada()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaDetalleLiberada()
        {
            return View();
        }

        public IActionResult DeshacerOrdenBancariaLiberada()
        {
            return View();
        }

        public IActionResult DeshacerOrdenBancariaDetalleLiberada()
        {
            return View();
        }

        public IActionResult ListarOrdenesBancariasDeshechas()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaDeshecha()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaDetalleDeshecha()
        {
            return View();
        }

        public IActionResult ListarOrdenesBancariasAprobadas()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaAprobada()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaDetalleAprobada()
        {
            return View();
        }

        public IActionResult ListarOrdenesBancariasDiarias()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaDiaria()
        {
            return View();
        }

        public IActionResult VerOrdenBancariaDetalleDiaria()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> ListarOrdenesBancariasLiberadasAsync([FromQuery] Int32 pagina, [FromQuery] Int32 filas, [FromQuery] String idSociedad, [FromQuery] String idBanco, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _tesoreriaNE.ListarOrdenesBancariasLiberadasAsync(new CancellationToken(false), idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden);
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
        public async Task<JsonResult> ListarOrdenesBancariasDetalleLiberadasAsync([FromQuery] String idSociedad, [FromQuery] String idSap, [FromQuery] String anio, [FromQuery] String momentoOrden, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _tesoreriaNE.ListarOrdenesBancariasDetalleLiberadasAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden);
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
        public async Task<JsonResult> BuscarOrdenesBancariasLiberadasAsync([FromQuery] String idSap, [FromQuery] String fechaInicio, [FromQuery] String fechaFin)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _tesoreriaNE.BuscarOrdenesBancariasLiberadasAsync(new CancellationToken(false), idUsuario, idSap, fechaInicio, fechaFin);
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
        public async Task<IActionResult> ObtenerVistaParcialOrdenBancariaLiberadaAsync([FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _tesoreriaNE.ObtenerVistaParcialOrdenBancariaLiberadaAsync(new CancellationToken(false), this, idTipoOrden);
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
        public async Task<IActionResult> ObtenerVistaParcialOrdenBancariaDetalleLiberadaAsync([FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _tesoreriaNE.ObtenerVistaParcialOrdenBancariaDetalleLiberadaAsync(new CancellationToken(false), this, idTipoOrden);
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
        public async Task<JsonResult> DeshacerOrdenBancariaLiberadaAsync([FromBody] String[] parametros)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    String usuario = _httpContextAccessor.HttpContext.User.Identity.Name;
                    objeto = await _tesoreriaNE.DeshacerOrdenBancariaLiberadaAsync(new CancellationToken(false), parametros, idUsuario, usuario);
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
        public async Task<JsonResult> ListarOrdenesBancariasDeshechasAsync([FromQuery] Int32 pagina, [FromQuery] Int32 filas, [FromQuery] String idSociedad, [FromQuery] String idBanco, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _tesoreriaNE.ListarOrdenesBancariasDeshechasAsync(new CancellationToken(false), idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden);
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
        public async Task<JsonResult> ListarOrdenesBancariasDetalleDeshechasAsync([FromQuery] String idSociedad, [FromQuery] String idSap, [FromQuery] String anio, [FromQuery] String momentoOrden, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _tesoreriaNE.ListarOrdenesBancariasDetalleDeshechasAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden);
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
        public async Task<JsonResult> BuscarOrdenesBancariasDeshechasAsync([FromQuery] String idSap, [FromQuery] String fechaInicio, [FromQuery] String fechaFin)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _tesoreriaNE.BuscarOrdenesBancariasDeshechasAsync(new CancellationToken(false), idUsuario, idSap, fechaInicio, fechaFin);
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
        public async Task<IActionResult> ObtenerVistaParcialOrdenBancariaDeshechaAsync([FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _tesoreriaNE.ObtenerVistaParcialOrdenBancariaDeshechaAsync(new CancellationToken(false), this, idTipoOrden);
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
        public async Task<IActionResult> ObtenerVistaParcialOrdenBancariaDetalleDeshechaAsync([FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _tesoreriaNE.ObtenerVistaParcialOrdenBancariaDetalleDeshechaAsync(new CancellationToken(false), this, idTipoOrden);
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
                    objeto = await _tesoreriaNE.ListarOrdenesBancariasAprobadasAsync(new CancellationToken(false), idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden);
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
                    objeto = await _tesoreriaNE.ListarOrdenesBancariasDetalleAprobadasAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden);
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
                    objeto = await _tesoreriaNE.BuscarOrdenesBancariasAprobadasAsync(new CancellationToken(false), idUsuario, idSap, fechaInicio, fechaFin);
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
                    objeto = await _tesoreriaNE.ObtenerVistaParcialOrdenBancariaAprobadaAsync(new CancellationToken(false), this, idTipoOrden);
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
                    objeto = await _tesoreriaNE.ObtenerVistaParcialOrdenBancariaDetalleAprobadaAsync(new CancellationToken(false), this, idTipoOrden);
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
        public async Task<JsonResult> ListarOrdenesBancariasDiariasAsync([FromQuery] Int32 pagina, [FromQuery] Int32 filas, [FromQuery] String idSociedad, [FromQuery] String idBanco, [FromQuery] String idTipoOrden, [FromQuery] String idEstadoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _tesoreriaNE.ListarOrdenesBancariasDiariasAsync(new CancellationToken(false), idUsuario, pagina, filas, idSociedad, idBanco, idTipoOrden, idEstadoOrden);
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
        public async Task<JsonResult> ListarOrdenesBancariasDetalleDiariasAsync([FromQuery] String idSociedad, [FromQuery] String idSap, [FromQuery] String anio, [FromQuery] String momentoOrden, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _tesoreriaNE.ListarOrdenesBancariasDetalleDiariasAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden);
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
        public async Task<JsonResult> BuscarOrdenesBancariasDiariasAsync([FromQuery] String idSap)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _tesoreriaNE.BuscarOrdenesBancariasDiariasAsync(new CancellationToken(false), idUsuario, idSap);
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
        public async Task<IActionResult> ObtenerVistaParcialOrdenBancariaDiariaAsync([FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _tesoreriaNE.ObtenerVistaParcialOrdenBancariaDiariaAsync(new CancellationToken(false), this, idTipoOrden);
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
        public async Task<IActionResult> ObtenerVistaParcialOrdenBancariaDetalleDiariaAsync([FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _tesoreriaNE.ObtenerVistaParcialOrdenBancariaDetalleDiariaAsync(new CancellationToken(false), this, idTipoOrden);
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
        public async Task<JsonResult> ListarFlujoAprobacionAsync([FromQuery] String idSociedad, [FromQuery] String idSap, [FromQuery] String anio, [FromQuery] String momentoOrden, [FromQuery] String idTipoOrden)
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    objeto = await _tesoreriaNE.ListarFlujoAprobacionAsync(new CancellationToken(false), idSociedad, idSap, anio, momentoOrden, idTipoOrden);
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
        public async Task<IActionResult> ListarFiltrosOrdenesBancariasLiberadasAsync()
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _filtroNE.ListarFiltrosOrdenesBancariasLiberadasAsync(new CancellationToken(false), idUsuario);
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
        public async Task<IActionResult> ListarFiltrosOrdenesBancariasDesechasAsync()
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _filtroNE.ListarFiltrosOrdenesBancariasDesechasAsync(new CancellationToken(false), idUsuario);
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
                    objeto = await _filtroNE.ListarFiltrosOrdenesBancariasAprobadasTesoreriaAsync(new CancellationToken(false), idUsuario);
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
        public async Task<IActionResult> ListarFiltrosOrdenesBancariasDiariasAsync()
        {
            Object objeto = null;
            try
            {
                Boolean isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

                if (isAuthenticated)
                {
                    String idUsuario = _httpContextAccessor.HttpContext.User.FindFirst(Constante.ID_USUARIO).Value;
                    objeto = await _filtroNE.ListarFiltrosOrdenesBancariasDiariasAsync(new CancellationToken(false), idUsuario);
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