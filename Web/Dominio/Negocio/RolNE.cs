using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Comun;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Modelo;
using Repositorio;

namespace Negocio
{
    public class RolNE
    {
        private Bitacora _bitacora = null;
        private CuentaRE _cuentaRE = null;
        private UserManager<IdentityUserMO> _userManager = null;
        private RoleManager<IdentityRole> _roleManager = null;

        public RolNE(IConfiguration configuration, UserManager<IdentityUserMO> userManager, RoleManager<IdentityRole> roleManager)
        {
            _bitacora = _bitacora ?? new Bitacora(configuration);
            _cuentaRE = _cuentaRE ?? new CuentaRE(configuration);
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Object> ListarNombresUsuariosAsync(CancellationToken cancelToken)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (cancelToken == null)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    var users = _userManager.Users.ToList();
                
                    if (users != null && users.Count > 0)
                    {
                        List<UsuarioMO> listaUsuarios = new List<UsuarioMO>();

                        foreach (var item in users)
                        {
                            UsuarioMO usuarioMO = new UsuarioMO();
                            usuarioMO.IdUsuario = item.IdUsuario;
                            usuarioMO.Usuario = item.UserName;
                            listaUsuarios.Add(usuarioMO);
                        }

                        objeto = new
                        {
                            codigo = listaUsuarios != null && listaUsuarios.Count > 0 ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                            mensaje = listaUsuarios != null && listaUsuarios.Count > 0 ? Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_OK : Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_NO_OK,
                            listaUsuarios = listaUsuarios
                        };
                        esCorrecto = listaUsuarios != null && listaUsuarios.Count > 0 ? true : false;
                    }
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_OK : Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_ROL_NE, Constante.METODO_LISTAR_USUARIOS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_ROL_NE, Constante.METODO_LISTAR_USUARIOS_ASYNC, Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarRolesAsync(CancellationToken cancelToken)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (cancelToken == null)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    List<IdentityRole> listaRoles = _roleManager.Roles.ToList();
                    objeto = new
                    {
                        codigo = listaRoles != null && listaRoles.Count > 0 ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = listaRoles != null && listaRoles.Count > 0 ? Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_OK : Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_NO_OK,
                        listaRoles = listaRoles
                    };
                    esCorrecto = listaRoles != null && listaRoles.Count > 0 ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ROLES_ASYNC_OK : Constante.MENSAJE_LISTAR_ROLES_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_ROL_NE, Constante.METODO_LISTAR_ROLES_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_ROL_NE, Constante.METODO_LISTAR_ROLES_ASYNC, Constante.MENSAJE_LISTAR_ROLES_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ListarRolesUsuarioAsync(CancellationToken cancelToken, String usuario)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (usuario == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    IdentityUserMO identityMO = await _userManager.FindByNameAsync(usuario);
                    IList<String> listaRoles = await _userManager.GetRolesAsync(identityMO);
                    objeto = new
                    {
                        codigo = listaRoles.Count > 0 ? Constante.CODIGO_OK : Constante.CODIGO_OMISION,
                        mensaje = listaRoles.Count > 0 ? String.Empty : Constante.MENSAJE_USUARIO_SIN_ROLES,
                        listaRoles = listaRoles
                    };
                    esCorrecto = listaRoles != null && listaRoles.Count > 0 ? true : false;
                }
                
                String mensaje = esCorrecto == true ? Constante.MENSAJE_LISTAR_ROLES_USUARIO_ASYNC_OK : Constante.MENSAJE_LISTAR_ROLES_USUARIO_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_ROL_NE, Constante.METODO_LISTAR_ROLES_USUARIO_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_ROL_NE, Constante.METODO_LISTAR_ROLES_USUARIO_ASYNC, Constante.MENSAJE_LISTAR_ROLES_USUARIO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> AsignarRolAsync(CancellationToken cancelToken, String[] parametros)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                String usuario = parametros[0] == null ? String.Empty : parametros[0].ToString().Trim();
                String rol = parametros[1] == null ? String.Empty : parametros[1].ToString().Trim();
                
                if (usuario == String.Empty || rol == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    IdentityUserMO identityMO = await _userManager.FindByNameAsync(usuario);
                    IdentityResult identityResult = await _userManager.AddToRoleAsync(identityMO, rol);
                    String mensajeError = String.Empty;
                    
                    if (!identityResult.Succeeded)
                    {
                        foreach (IdentityError item in identityResult.Errors)
                        {
                            mensajeError += String.Format("{0}{1}", item.Description, Environment.NewLine);
                        }
                    }

                    objeto = new
                    {
                        codigo = identityResult.Succeeded ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = identityResult.Succeeded ? Constante.MENSAJE_ASIGNAR_ROL_ASYNC_OK : mensajeError
                    };
                    esCorrecto = identityResult.Succeeded ? true : false;
                }
                
                String mensaje = esCorrecto == true ? Constante.MENSAJE_ASIGNAR_ROL_ASYNC_OK : Constante.MENSAJE_ASIGNAR_ROL_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_ROL_NE, Constante.METODO_ASIGNAR_ROL_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_ROL_NE, Constante.METODO_ASIGNAR_ROL_ASYNC, Constante.MENSAJE_ASIGNAR_ROL_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> DesasignarRolAsync(CancellationToken cancelToken, String[] parametros)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                String usuario = parametros[0] == null ? String.Empty : parametros[0].ToString().Trim();
                String rol = parametros[1] == null ? String.Empty : parametros[1].ToString().Trim();
                
                if (usuario == String.Empty || rol == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    IdentityUserMO identityMO = await _userManager.FindByNameAsync(usuario);
                    IdentityResult identityResult = await _userManager.RemoveFromRoleAsync(identityMO, rol);
                    String mensajeError = String.Empty;
                    
                    if (!identityResult.Succeeded)
                    {
                        foreach (IdentityError item in identityResult.Errors)
                        {
                            mensajeError += String.Format("{0}{1}", item.Description, Environment.NewLine);
                        }
                    }

                    objeto = new
                    {
                        codigo = identityResult.Succeeded ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = identityResult.Succeeded ? Constante.MENSAJE_DESASIGNAR_ROL_ASYNC_OK : mensajeError
                    };
                    esCorrecto = identityResult.Succeeded ? true : false;
                }
                
                String mensaje = esCorrecto == true ? Constante.MENSAJE_DESASIGNAR_ROL_ASYNC_OK : Constante.MENSAJE_DESASIGNAR_ROL_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_ROL_NE, Constante.METODO_DESASIGNAR_ROL_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_ROL_NE, Constante.METODO_DESASIGNAR_ROL_ASYNC, Constante.MENSAJE_DESASIGNAR_ROL_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }
    }
}