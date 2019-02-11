using System;
using System.Threading;
using System.Threading.Tasks;
using Comun;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Modelo;
using Repositorio;

namespace Negocio
{
    public class CuentaNE
    {
        private Bitacora _bitacora = null;
        private CuentaRE _cuentaRE = null;
        private UserManager<IdentityUserMO> _userManager = null;
        private SignInManager<IdentityUserMO> _signInManager = null;

        public CuentaNE(IConfiguration configuration, UserManager<IdentityUserMO> userManager, SignInManager<IdentityUserMO> signInManager)
        {
            _bitacora = _bitacora ?? new Bitacora(configuration);
            _cuentaRE = _cuentaRE ?? new CuentaRE(configuration);
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Object> ListarUsuariosAsync(CancellationToken cancelToken, Int32 pagina, Int32 filas)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (pagina == 0 || filas == 0)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    ObjetoUsuarioMO objetoUsuarioMO = await _cuentaRE.ListarUsuariosAsync(cancelToken, pagina, filas);
                    objeto = new
                    {
                        codigo = objetoUsuarioMO.Codigo,
                        mensaje = objetoUsuarioMO.Mensaje,
                        listaUsuarios = objetoUsuarioMO.ListaUsuarios,
                        totalRegistros = objetoUsuarioMO.TotalRegistros
                    };
                    esCorrecto = objetoUsuarioMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }
                
                String mensaje = esCorrecto ? Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_OK : Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_LISTAR_USUARIOS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_LISTAR_USUARIOS_ASYNC, Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> BuscarUsuariosAsync(CancellationToken cancelToken, String usuario, String apePaterno, String nombres)
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
                    usuario = usuario ?? String.Empty;
                    apePaterno = apePaterno ?? String.Empty;
                    nombres = nombres ?? String.Empty;
                    ObjetoUsuarioMO objetoUsuarioMO = await _cuentaRE.BuscarUsuariosAsync(cancelToken, usuario, apePaterno, nombres);
                    objeto = new
                    {
                        codigo = objetoUsuarioMO.Codigo,
                        mensaje = objetoUsuarioMO.Mensaje,
                        listaUsuarios = objetoUsuarioMO.ListaUsuarios,
                        totalRegistros = objetoUsuarioMO.TotalRegistros
                    };
                    esCorrecto = objetoUsuarioMO.Codigo != Constante.CODIGO_NO_OK ? true : false;
                }
                
                String mensaje = esCorrecto ? Constante.MENSAJE_BUSCAR_USUARIOS_ASYNC_OK : Constante.MENSAJE_BUSCAR_USUARIOS_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_BUSCAR_USUARIOS_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_BUSCAR_USUARIOS_ASYNC, Constante.MENSAJE_BUSCAR_USUARIOS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> CrearUsuarioDesconectadoAsync(CancellationToken cancelToken, UsuarioMO usuarioMO)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (usuarioMO == null)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    ObjetoUsuarioMO objetoUsuarioMO = await _cuentaRE.CrearUsuarioAsync(cancelToken, usuarioMO);
                    objeto = new
                    {
                        codigo = objetoUsuarioMO.Codigo,
                        mensaje = objetoUsuarioMO.Mensaje
                    };
                    esCorrecto = objetoUsuarioMO.Codigo == Constante.CODIGO_OK ? true : false;
                }
                
                String mensaje = esCorrecto == true ? Constante.MENSAJE_CREAR_USUARIO_ASYNC_OK : Constante.MENSAJE_CREAR_USUARIO_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_CREAR_USUARIO_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_CREAR_USUARIO_ASYNC, Constante.MENSAJE_CREAR_USUARIO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> CrearUsuarioAsync(CancellationToken cancelToken, UsuarioMO usuarioMO)
        {
            Object objeto = null;
            Boolean esParametrosConforme = false, esInexistenteConforme = false, esCreadoConforme = false, esRegistradoConforme = false;
            try
            {
                String mensaje = String.Empty;

                if (usuarioMO != null)
                {
                    esParametrosConforme = true;
                    IdentityUserMO usuarioPorNombre = await _userManager.FindByNameAsync(usuarioMO.Usuario);
                    IdentityUserMO usuarioPorCorreo = await _userManager.FindByEmailAsync(usuarioMO.Correo);

                    if (usuarioPorNombre == null && usuarioPorCorreo == null)
                    {
                        esInexistenteConforme = true;
                        ObjetoUsuarioMO objetoUsuarioMO = await _cuentaRE.CrearUsuarioAsync(cancelToken, usuarioMO);

                        if (objetoUsuarioMO != null && objetoUsuarioMO.Codigo == Constante.CODIGO_OK)
                        {
                            esCreadoConforme = true;
                            mensaje = objetoUsuarioMO.Mensaje;
                            IdentityUserMO identityMO = MapearModeloUsuarioHaciaModeloIdentity(usuarioMO);
                            identityMO.IdUsuario = objetoUsuarioMO.IdUsuario;
                            IdentityResult identityResult = await _userManager.CreateAsync(identityMO, usuarioMO.Clave);
                            esRegistradoConforme = identityResult.Succeeded;
                                
                            if (!identityResult.Succeeded)
                            {
                                await _cuentaRE.EliminarUsuarioAsync(cancelToken, objetoUsuarioMO.IdUsuario);
                                
                                foreach (IdentityError item in identityResult.Errors)
                                {
                                    mensaje += String.Format("{0}{1}", item.Description, Environment.NewLine);
                                }
                            }
                        }
                    }
                }

                if (!esParametrosConforme)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_USUARIO_CLAVE_TOKEN_NO_INGRESADAS
                    };
                }
                else if (!esInexistenteConforme)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_USUARIO_DUPLICADO,
                        mensaje = Constante.MENSAJE_USUARIO_DUPLICADO
                    };
                }
                else if (!esCreadoConforme)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_RECARGAR_PAGINA
                    };
                }
                else if (!esRegistradoConforme)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = mensaje
                    };
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_OK,
                        mensaje = mensaje
                    };
                }
                
                mensaje = esRegistradoConforme == true ? Constante.MENSAJE_CREAR_USUARIO_ASYNC_OK : Constante.MENSAJE_CREAR_USUARIO_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_CREAR_USUARIO_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_CREAR_USUARIO_ASYNC, Constante.MENSAJE_CREAR_USUARIO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> EditarUsuarioAsync(CancellationToken cancelToken, UsuarioMO usuarioMO)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (usuarioMO == null)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    ObjetoUsuarioMO objetoUsuarioMO = await _cuentaRE.EditarUsuarioAsync(cancelToken, usuarioMO);
                    objeto = new
                    {
                        codigo = objetoUsuarioMO.Codigo,
                        mensaje = objetoUsuarioMO.Mensaje
                    };
                    esCorrecto = objetoUsuarioMO.Codigo == Constante.CODIGO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_EDITAR_USUARIO_ASYNC_OK : Constante.MENSAJE_EDITAR_USUARIO_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_EDITAR_USUARIO_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_EDITAR_USUARIO_ASYNC, Constante.MENSAJE_EDITAR_USUARIO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> InactivarUsuarioAsync(CancellationToken cancelToken, String idUsuario)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                if (idUsuario == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    ObjetoUsuarioMO objetoUsuarioMO = await _cuentaRE.InactivarUsuarioAsync(cancelToken, idUsuario);
                    objeto = new
                    {
                        codigo = objetoUsuarioMO.Codigo,
                        mensaje = objetoUsuarioMO.Mensaje
                    };
                    esCorrecto = objetoUsuarioMO.Codigo == Constante.CODIGO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_INACTIVAR_USUARIO_ASYNC_OK : Constante.MENSAJE_INACTIVAR_USUARIO_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_INACTIVAR_USUARIO_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_INACTIVAR_USUARIO_ASYNC, Constante.MENSAJE_INACTIVAR_USUARIO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        private IdentityUserMO MapearModeloUsuarioHaciaModeloIdentity(UsuarioMO usuarioMO)
        {
            IdentityUserMO identityMO = new IdentityUserMO();
            try
            {
                identityMO.IdUsuario = String.Empty;
                identityMO.UserName = usuarioMO.Usuario;
                identityMO.NormalizedUserName = usuarioMO.Usuario.ToUpper();
                identityMO.Email = usuarioMO.Correo;
                identityMO.NormalizedEmail = usuarioMO.Correo.ToUpper();
                identityMO.EmailConfirmed = true;
                identityMO.PhoneNumber = usuarioMO.Celular;
                identityMO.PhoneNumberConfirmed = true;
                identityMO.TwoFactorEnabled = false;
                identityMO.LockoutEnabled = false;
                identityMO.AccessFailedCount = 20;
            }
            catch (Exception e)
            {
                throw e;
            }
            return identityMO;
        }

        public async Task<Object> IniciarSesionAsync(CancellationToken cancelToken, String[] parametros)
        {
            Object objeto = null;
            Boolean esParametrosConforme = false, esUsuarioConforme = false, esTokenConforme = false, esLogueadoConforme = false;
            try
            {
                String usuario = parametros[0] == null ? String.Empty : parametros[0].ToString().Trim();
                String clave = parametros[1] == null ? String.Empty : parametros[1].ToString().Trim();
                String token = parametros[2] == null ? String.Empty : parametros[2].ToString().Trim();
                String mensaje = String.Empty;
                
                if (usuario != String.Empty && clave != String.Empty && token != String.Empty)
                {
                    esParametrosConforme = true;
                    UsuarioMO usuarioMO = await _cuentaRE.ConsultarUsuariosAsync(new CancellationToken(false), usuario);

                    if (usuarioMO != null)
                    {
                        esUsuarioConforme = true;
                        ObjetoUsuarioMO objetoUsuarioMO = await _cuentaRE.ValidarCodigoTokenAsync(cancelToken, usuarioMO.IdUsuario, token);

                        if (objetoUsuarioMO != null && objetoUsuarioMO.Codigo == Constante.CODIGO_OK)
                        {
                            esTokenConforme = true;
                            SignInResult signInResult = await _signInManager.PasswordSignInAsync(usuario, clave, Constante.IS_PERSISTENT_FALSE, Constante.IS_LOCKOUT_ON_FAILURE_FALSE);
                            mensaje = objetoUsuarioMO.Mensaje;

                            if (signInResult.Succeeded == false)
                            {
                                if (signInResult.IsLockedOut)
                                {
                                    mensaje = Constante.MENSAJE_USUARIO_BLOQUEADO;
                                }
                                else if (signInResult.IsNotAllowed)
                                {
                                    mensaje = Constante.MENSAJE_USUARIO_NO_PERMITIDO;
                                }
                                else if (signInResult.RequiresTwoFactor)
                                {
                                    mensaje = Constante.MENSAJE_DOBLE_AUTENTICACION;
                                }
                                else
                                {
                                    mensaje = Constante.MENSAJE_USUARIO_CLAVE_INCORRECTAS;
                                }
                            }
                            else
                            {
                                esLogueadoConforme = true;
                            }
                        }
                    }
                }

                if (!esParametrosConforme)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_USUARIO_CLAVE_TOKEN_NO_INGRESADAS
                    };
                }
                else if (!esUsuarioConforme)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_USUARIO_CLAVE_INCORRECTAS
                    };
                }
                else if (!esTokenConforme)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = mensaje
                    };
                }
                else if (!esLogueadoConforme)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = mensaje
                    };
                }
                else
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_OK,
                        url = Constante.URL_PRIVADO_INICIO
                    };
                }

                mensaje = esLogueadoConforme == true ? Constante.MENSAJE_INICIAR_SESION_ASYNC_OK : Constante.MENSAJE_INICIAR_SESION_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_INICIAR_SESION_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_INICIAR_SESION_ASYNC, Constante.MENSAJE_INICIAR_SESION_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> CerrarSesionAsync(CancellationToken cancelToken)
        {
            Object objeto = null;
            try
            {
                await _signInManager.SignOutAsync();
                objeto = new
                {
                    codigo = Constante.CODIGO_OK,
                    url = Constante.URL_PUBLICO_LOGUEAR
                };

                String mensaje = Constante.MENSAJE_CERRAR_SESION_ASYNC_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_CERRAR_SESION_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_CERRAR_SESION_ASYNC, Constante.MENSAJE_CERRAR_SESION_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ConsultarNombreUsuarioAsync(CancellationToken cancelToken, String usuario)
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
                    UsuarioMO usuarioMO = await _cuentaRE.ConsultarUsuariosAsync(new CancellationToken(false), usuario);
                    objeto = new 
                    {
                        codigo = usuarioMO != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        nombres = usuarioMO != null ? usuarioMO.Nombres : Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                    esCorrecto = usuarioMO != null ? true : false;
                }
                
                String mensaje = esCorrecto == true ? Constante.MENSAJE_CONSULTAR_NOMBRE_USUARIO_ASYNC_OK : Constante.MENSAJE_CONSULTAR_NOMBRE_USUARIO_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_CONSULTAR_NOMBRE_USAURIO_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_CONSULTAR_NOMBRE_USAURIO_ASYNC, Constante.MENSAJE_CONSULTAR_NOMBRE_USUARIO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> SolicitarCodigoTokenAsync(CancellationToken cancelToken, String usuario)
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
                    UsuarioMO usuarioMO = await _cuentaRE.ConsultarUsuariosAsync(cancelToken, usuario);

                    if (usuarioMO == null)
                    {
                        objeto = new
                        {
                            codigo = Constante.CODIGO_NO_OK,
                            mensaje = Constante.MENSAJE_USUARIO_CLAVE_INCORRECTAS
                        };
                    }
                    else
                    {
                        ObjetoUsuarioMO objetoUsuarioMO = await _cuentaRE.SolicitarCodigoTokenAsync(cancelToken, usuarioMO.IdUsuario);
                        objeto = new
                        {
                            codigo = objetoUsuarioMO.Codigo,
                            mensaje = objetoUsuarioMO.Mensaje
                        };
                        esCorrecto = objetoUsuarioMO.Codigo == Constante.CODIGO_OK ? true : false;
                    }
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_SOLICITAR_CODIGO_TOKEN_ASYNC_OK : Constante.MENSAJE_SOLICITAR_CODIGO_TOKEN_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_SOLICITAR_CODIGO_TOKEN_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_SOLICITAR_CODIGO_TOKEN_ASYNC, Constante.MENSAJE_SOLICITAR_CODIGO_TOKEN_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ModificarClaveAsync(CancellationToken cancelToken, String[] parametros, String usuario)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                String claveActual = parametros[0] == null ? String.Empty : parametros[0].ToString().Trim();
                String claveNueva = parametros[1] == null ? String.Empty : parametros[1].ToString().Trim();
                
                if (usuario == String.Empty || claveActual == String.Empty || claveNueva == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    IdentityUserMO identityUserMO = await _userManager.FindByNameAsync(usuario);
                    IdentityResult identityResult = await _userManager.ChangePasswordAsync(identityUserMO, claveActual, claveNueva);
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
                        mensaje = identityResult.Succeeded ? Constante.MENSAJE_VOLVER_INICIAR_SESION : mensajeError
                    };
                    esCorrecto = identityResult.Succeeded ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_MODIFICAR_CLAVE_ASYNC_OK : Constante.MENSAJE_MODIFICAR_CLAVE_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_MODIFICAR_CLAVE_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_MODIFICAR_CLAVE_ASYNC, Constante.MENSAJE_MODIFICAR_CLAVE_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ConsultarUsuarioAsync(CancellationToken cancelToken, String usuario)
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
                    UsuarioMO usuarioMO = await _cuentaRE.ConsultarUsuariosAsync(cancelToken, usuario);
                    objeto = new
                    {
                        codigo = usuarioMO != null ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = usuarioMO != null ? String.Empty : Constante.MENSAJE_CONSULTAR_USUARIO_ASYNC_NO_OK,
                        usuario = usuarioMO
                    };
                    esCorrecto = usuarioMO != null ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_CONSULTAR_USUARIO_ASYNC_OK : Constante.MENSAJE_CONSULTAR_USUARIO_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_CONSULTAR_USUARIO_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_CONSULTAR_USUARIO_ASYNC, Constante.MENSAJE_CONSULTAR_USUARIO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> RecuperarClaveAsync(CancellationToken cancelToken, String usuario, String correo)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                usuario = usuario ?? String.Empty;
                correo = correo ?? String.Empty;

                if (usuario == String.Empty && correo == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    IdentityUserMO identityUserMO = usuario != String.Empty ? await _userManager.FindByNameAsync(usuario) : await _userManager.FindByEmailAsync(correo);
                    String token = await _userManager.GeneratePasswordResetTokenAsync(identityUserMO);
                    var callbackUrl = String.Format(Constante.URL_RECUPERAR_CLAVE, identityUserMO.UserName, token);
                    ObjetoUsuarioMO objetoUsuarioMO = await _cuentaRE.RecuperarClaveAsync(cancelToken, identityUserMO.IdUsuario, callbackUrl);
                    objeto = new
                    {
                        codigo = objetoUsuarioMO.Codigo,
                        mensaje = objetoUsuarioMO.Mensaje
                    };
                    esCorrecto = objetoUsuarioMO.Codigo == Constante.CODIGO_OK ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_RECUPERAR_CLAVE_ASYNC_OK : Constante.MENSAJE_RECUPERAR_CLAVE_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_RECUPERAR_CLAVE_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_RECUPERAR_CLAVE_ASYNC, Constante.MENSAJE_RECUPERAR_CLAVE_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }

        public async Task<Object> ReiniciarClaveAsync(CancellationToken cancelToken, String[] parametros)
        {
            Object objeto = null;
            Boolean esCorrecto = false;
            try
            {
                String usuario = parametros[0] == null ? String.Empty : parametros[0].ToString().Trim();
                String clave = parametros[1] == null ? String.Empty : parametros[1].ToString().Trim();
                String token = parametros[2] == null ? String.Empty : parametros[2].ToString().Trim();

                if (usuario == String.Empty || clave == String.Empty || token == String.Empty)
                {
                    objeto = new
                    {
                        codigo = Constante.CODIGO_NO_OK,
                        mensaje = Constante.MENSAJE_PARAMETROS_NO_PRESENTES
                    };
                }
                else
                {
                    IdentityUserMO identityUserMO = await _userManager.FindByNameAsync(usuario);
                    IdentityResult identityResult = await _userManager.ResetPasswordAsync(identityUserMO, token, clave);
                    esCorrecto = identityResult.Succeeded;
                    String mensajeError = String.Empty;

                    if (!identityResult.Succeeded)
                    {
                        foreach (var item in identityResult.Errors)
                        {
                            mensajeError += String.Format("{0}{1}", item.Description, Environment.NewLine);
                        }
                    }
                    
                    objeto = new
                    {
                        codigo = identityResult.Succeeded ? Constante.CODIGO_OK : Constante.CODIGO_NO_OK,
                        mensaje = identityResult.Succeeded ? Constante.MENSAJE_VOLVER_INICIAR_SESION : mensajeError,
                        url = Constante.URL_PUBLICO_LOGUEAR
                    };
                    esCorrecto = identityResult.Succeeded ? true : false;
                }

                String mensaje = esCorrecto == true ? Constante.MENSAJE_REINICIAR_CLAVE_ASYNC_OK : Constante.MENSAJE_REINICIAR_CLAVE_ASYNC_NO_OK;
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_REINICIAR_CLAVE_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_NEGOCIO, Constante.CLASE_CUENTA_NE, Constante.METODO_REINICIAR_CLAVE_ASYNC, Constante.MENSAJE_REINICIAR_CLAVE_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return objeto;
        }
    }
}
