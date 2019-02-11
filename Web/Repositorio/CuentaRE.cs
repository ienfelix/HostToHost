using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Comun;
using Microsoft.Extensions.Configuration;
using Modelo;

namespace Repositorio
{
    public class CuentaRE
    {
        private String _conexionHostToHost = String.Empty;
        private SqlConnection _con = null;
        private SqlCommand _cmd = null;
        private SqlDataReader _reader = null;
        private Bitacora _bitacora = null;

        public CuentaRE(IConfiguration configuration)
        {
            _bitacora = _bitacora ?? new Bitacora(configuration);
            _conexionHostToHost = configuration.GetConnectionString(Constante.CONEXION_HOST_TO_HOST_PRODUCCION) ?? String.Empty;
        }

        public async Task<ObjetoUsuarioMO> ListarUsuariosAsync(CancellationToken cancelToken, Int32 pagina, Int32 filas)
        {
            ObjetoUsuarioMO objetoUsuarioMO = new ObjetoUsuarioMO();
            List<UsuarioMO> listaUsuarios = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_LISTAR_USUARIOS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.PAGINA, System.Data.SqlDbType.Int);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = pagina;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.FILAS, System.Data.SqlDbType.Int);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = filas;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.TOTAL_REGISTROS, System.Data.SqlDbType.Int);
                        par3.Direction = System.Data.ParameterDirection.Output;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);
                        
                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoUsuarioMO.Codigo = Constante.CODIGO_OMISION;
                            objetoUsuarioMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaUsuarios = new List<UsuarioMO>();
                            
                            while (await _reader.ReadAsync(cancelToken))
                            {
                                UsuarioMO usuarioMO = new UsuarioMO();
                                usuarioMO.IdUsuario = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                usuarioMO.Usuario = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                usuarioMO.ApePaterno = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                usuarioMO.ApeMaterno = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                usuarioMO.Nombres = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                usuarioMO.Correo = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                usuarioMO.Celular = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                usuarioMO.Estado = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                usuarioMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._9);
                                usuarioMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                usuarioMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaUsuarios.Add(usuarioMO);
                            }

                            objetoUsuarioMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoUsuarioMO.Codigo = _reader != null ? objetoUsuarioMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoUsuarioMO.Mensaje = _reader != null ? objetoUsuarioMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoUsuarioMO.ListaUsuarios = listaUsuarios;
                        objetoUsuarioMO.TotalRegistros = par3.Value == null ? 0 : Convert.ToInt32(par3.Value);
                        String mensaje = _reader != null ? Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_OK : Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_LISTAR_USUARIOS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_LISTAR_USUARIOS_ASYNC, Constante.MENSAJE_LISTAR_USUARIOS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoUsuarioMO;
        }

        public async Task<ObjetoUsuarioMO> BuscarUsuariosAsync(CancellationToken cancelToken, String usuario, String apePaterno, String nombres)
        {
            ObjetoUsuarioMO objetoUsuarioMO = new ObjetoUsuarioMO();
            List<UsuarioMO> listaUsuarios = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_BUSCAR_USUARIOS, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.USUARIO, System.Data.SqlDbType.NVarChar, Constante._20);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = usuario;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.APE_PATERNO, System.Data.SqlDbType.NVarChar, Constante._50);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = apePaterno;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.NOMBRES, System.Data.SqlDbType.NVarChar, Constante._50);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = nombres;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);
                        
                        if (_reader != null && !_reader.HasRows)
                        {
                            objetoUsuarioMO.Codigo = Constante.CODIGO_OMISION;
                            objetoUsuarioMO.Mensaje = Constante.MENSAJE_SIN_RESULTADOS;
                        }
                        else if (_reader != null && _reader.HasRows)
                        {
                            listaUsuarios = new List<UsuarioMO>();
                            
                            while (await _reader.ReadAsync(cancelToken))
                            {
                                UsuarioMO usuarioMO = new UsuarioMO();
                                usuarioMO.IdUsuario = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                usuarioMO.Usuario = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                usuarioMO.ApePaterno = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                usuarioMO.ApeMaterno = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                usuarioMO.Nombres = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                usuarioMO.Correo = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                usuarioMO.Celular = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                usuarioMO.Estado = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                                usuarioMO.UsuarioCreacion = await _reader.IsDBNullAsync(Constante._8, cancelToken) ? String.Empty : _reader.GetString(Constante._8);
                                DateTime? fechaCreacion = await _reader.IsDBNullAsync(Constante._9, cancelToken) ? (DateTime?)null : _reader.GetDateTime(Constante._9);
                                usuarioMO.FechaCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortDateString();
                                usuarioMO.HoraCreacion = fechaCreacion == null ? String.Empty : fechaCreacion.Value.ToShortTimeString();
                                listaUsuarios.Add(usuarioMO);
                            }

                            objetoUsuarioMO.Codigo = Constante.CODIGO_OK;
                        }

                        _reader.Close();
                        _con.Close();
                        objetoUsuarioMO.Codigo = _reader != null ? objetoUsuarioMO.Codigo : Constante.CODIGO_NO_OK;
                        objetoUsuarioMO.Mensaje = _reader != null ? objetoUsuarioMO.Mensaje : Constante.MENSAJE_RECARGAR_PAGINA;
                        objetoUsuarioMO.ListaUsuarios = listaUsuarios;
                        String mensaje = _reader != null ? Constante.MENSAJE_BUSCAR_USUARIOS_ASYNC_OK : Constante.MENSAJE_BUSCAR_USUARIOS_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_BUSCAR_USUARIOS_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_BUSCAR_USUARIOS_ASYNC, Constante.MENSAJE_BUSCAR_USUARIOS_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoUsuarioMO;
        }

        public async Task<ObjetoUsuarioMO> CrearUsuarioAsync(CancellationToken cancelToken, UsuarioMO usuarioMO)
        {
            ObjetoUsuarioMO objetoUsuarioMO = new ObjetoUsuarioMO();
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_CREAR_USUARIO, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.APE_PATERNO, System.Data.SqlDbType.NVarChar, Constante._50);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = usuarioMO.ApePaterno;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.APE_MATERNO, System.Data.SqlDbType.NVarChar, Constante._50);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = usuarioMO.ApeMaterno;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.NOMBRES, System.Data.SqlDbType.NVarChar, Constante._50);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = usuarioMO.Nombres;

                        SqlParameter par7 = _cmd.Parameters.Add(Constante.CORREO, System.Data.SqlDbType.NVarChar, Constante._50);
                        par7.Direction = System.Data.ParameterDirection.Input;
                        par7.Value = usuarioMO.Correo;

                        SqlParameter par8 = _cmd.Parameters.Add(Constante.CELULAR, System.Data.SqlDbType.NVarChar, Constante._15);
                        par8.Direction = System.Data.ParameterDirection.Input;
                        par8.Value = usuarioMO.Celular;

                        SqlParameter par16 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par16.Direction = System.Data.ParameterDirection.Output;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleRow, cancelToken);
                        
                        if (_reader != null && _reader.HasRows)
                        {
                            if (await _reader.ReadAsync(cancelToken))
                            {
                                objetoUsuarioMO.Codigo = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? 0 : _reader.GetInt32(Constante._0);
                                objetoUsuarioMO.Mensaje = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                            }
                        }

                        _reader.Close();
                        _con.Close();
                        objetoUsuarioMO.IdUsuario = par16.Value == null ? String.Empty : par16.Value.ToString();
                        String mensaje = _reader != null ? Constante.MENSAJE_CREAR_USUARIO_ASYNC_OK : Constante.MENSAJE_CREAR_USUARIO_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_CREAR_USUARIO_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_CREAR_USUARIO_ASYNC, Constante.MENSAJE_CREAR_USUARIO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoUsuarioMO;
        }

        public async Task<ObjetoUsuarioMO> EditarUsuarioAsync(CancellationToken cancelToken, UsuarioMO usuarioMO)
        {
            ObjetoUsuarioMO objetoUsuarioMO = new ObjetoUsuarioMO();
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_EDITAR_USUARIO, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = usuarioMO.IdUsuario;
                        
                        SqlParameter par2 = _cmd.Parameters.Add(Constante.APE_PATERNO, System.Data.SqlDbType.NVarChar, Constante._50);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = usuarioMO.ApePaterno;

                        SqlParameter par3 = _cmd.Parameters.Add(Constante.APE_MATERNO, System.Data.SqlDbType.NVarChar, Constante._50);
                        par3.Direction = System.Data.ParameterDirection.Input;
                        par3.Value = usuarioMO.ApeMaterno;

                        SqlParameter par4 = _cmd.Parameters.Add(Constante.NOMBRES, System.Data.SqlDbType.NVarChar, Constante._50);
                        par4.Direction = System.Data.ParameterDirection.Input;
                        par4.Value = usuarioMO.Nombres;

                        SqlParameter par7 = _cmd.Parameters.Add(Constante.CORREO, System.Data.SqlDbType.NVarChar, Constante._50);
                        par7.Direction = System.Data.ParameterDirection.Input;
                        par7.Value = usuarioMO.Correo;

                        SqlParameter par8 = _cmd.Parameters.Add(Constante.CELULAR, System.Data.SqlDbType.NVarChar, Constante._15);
                        par8.Direction = System.Data.ParameterDirection.Input;
                        par8.Value = usuarioMO.Celular;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleRow, cancelToken);
                        
                        if (_reader != null && _reader.HasRows)
                        {
                            if (await _reader.ReadAsync(cancelToken))
                            {
                                objetoUsuarioMO.Codigo = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? 0 : _reader.GetInt32(Constante._0);
                                objetoUsuarioMO.Mensaje = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                            }
                        }

                        _reader.Close();
                        _con.Close();
                        String mensaje = _reader != null ? Constante.MENSAJE_EDITAR_USUARIO_ASYNC_OK : Constante.MENSAJE_EDITAR_USUARIO_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_EDITAR_USUARIO_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_EDITAR_USUARIO_ASYNC, Constante.MENSAJE_EDITAR_USUARIO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoUsuarioMO;
        }

        public async Task<ObjetoUsuarioMO> InactivarUsuarioAsync(CancellationToken cancelToken, String idUsuario)
        {
            ObjetoUsuarioMO objetoUsuarioMO = new ObjetoUsuarioMO();
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_INACTIVAR_USUARIO, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleRow, cancelToken);
                        
                        if (_reader != null && _reader.HasRows)
                        {
                            if (await _reader.ReadAsync(cancelToken))
                            {
                                objetoUsuarioMO.Codigo = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? 0 : _reader.GetInt32(Constante._0);
                                objetoUsuarioMO.Mensaje = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                            }
                        }

                        _reader.Close();
                        _con.Close();
                        String mensaje = _reader != null ? Constante.MENSAJE_INACTIVAR_USUARIO_ASYNC_OK : Constante.MENSAJE_INACTIVAR_USUARIO_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_INACTIVAR_USUARIO_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_INACTIVAR_USUARIO_ASYNC, Constante.MENSAJE_INACTIVAR_USUARIO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoUsuarioMO;
        }

        public async Task<ObjetoUsuarioMO> EliminarUsuarioAsync(CancellationToken cancelToken, String idUsuario)
        {
            ObjetoUsuarioMO objetoUsuarioMO = new ObjetoUsuarioMO();
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_ELIMINAR_USUARIO, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NVarChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleRow, cancelToken);
                        
                        if (_reader != null && _reader.HasRows)
                        {
                            if (await _reader.ReadAsync(cancelToken))
                            {
                                objetoUsuarioMO.Codigo = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? 0 : _reader.GetInt32(Constante._0);
                                objetoUsuarioMO.Mensaje = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                            }
                        }

                        _reader.Close();
                        _con.Close();
                        String mensaje = _reader != null ? Constante.MENSAJE_ELIMINAR_USUARIO_ASYNC_OK : Constante.MENSAJE_ELIMINAR_USUARIO_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_ELIMINAR_USUARIO_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_ELIMINAR_USUARIO_ASYNC, Constante.MENSAJE_ELIMINAR_USUARIO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoUsuarioMO;
        }

        public async Task<UsuarioMO> ConsultarUsuariosAsync(CancellationToken cancelToken, String usuario)
        {
            UsuarioMO usuarioMO = null;
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_CONSULTAR_USUARIO, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.USUARIO, System.Data.SqlDbType.NVarChar, Constante._20);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = usuario;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult, cancelToken);
                        
                        if (_reader != null && _reader.HasRows)
                        {
                            if (await _reader.ReadAsync(cancelToken))
                            {
                                usuarioMO = new UsuarioMO();
                                usuarioMO.IdUsuario = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? String.Empty : _reader.GetString(Constante._0);
                                usuarioMO.Usuario = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                                usuarioMO.ApePaterno = await _reader.IsDBNullAsync(Constante._2, cancelToken) ? String.Empty : _reader.GetString(Constante._2);
                                usuarioMO.ApeMaterno = await _reader.IsDBNullAsync(Constante._3, cancelToken) ? String.Empty : _reader.GetString(Constante._3);
                                usuarioMO.Nombres = await _reader.IsDBNullAsync(Constante._4, cancelToken) ? String.Empty : _reader.GetString(Constante._4);
                                usuarioMO.Correo = await _reader.IsDBNullAsync(Constante._5, cancelToken) ? String.Empty : _reader.GetString(Constante._5);
                                usuarioMO.Celular = await _reader.IsDBNullAsync(Constante._6, cancelToken) ? String.Empty : _reader.GetString(Constante._6);
                                usuarioMO.FechaCreacion = await _reader.IsDBNullAsync(Constante._7, cancelToken) ? String.Empty : _reader.GetString(Constante._7);
                            }
                        }

                        _reader.Close();
                        _con.Close();
                        String mensaje = usuarioMO != null ? Constante.MENSAJE_CONSULTAR_USUARIO_ASYNC_OK : Constante.MENSAJE_CONSULTAR_USUARIO_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_CONSULTAR_USUARIO_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_CONSULTAR_USUARIO_ASYNC, Constante.MENSAJE_CONSULTAR_USUARIO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con != null && _con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return usuarioMO;
        }

        public async Task<ObjetoUsuarioMO> SolicitarCodigoTokenAsync(CancellationToken cancelToken, String idUsuario)
        {
            ObjetoUsuarioMO objetoUsuarioMO = new ObjetoUsuarioMO();
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_SOLICITAR_CODIGO_TOKEN, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleRow, cancelToken);

                        if (_reader != null && _reader.HasRows)
                        {
                            if (await _reader.ReadAsync(cancelToken))
                            {
                                objetoUsuarioMO.Codigo = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? 0 : _reader.GetInt32(Constante._0);
                                objetoUsuarioMO.Mensaje = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                            }
                        }

                        _reader.Close();
                        _con.Close();
                        String mensaje = _reader != null ? Constante.MENSAJE_SOLICITAR_CODIGO_TOKEN_ASYNC_OK : Constante.MENSAJE_SOLICITAR_CODIGO_TOKEN_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_SOLICITAR_CODIGO_TOKEN_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_SOLICITAR_CODIGO_TOKEN_ASYNC, Constante.MENSAJE_SOLICITAR_CODIGO_TOKEN_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoUsuarioMO;
        }

        public async Task<ObjetoUsuarioMO> ValidarCodigoTokenAsync(CancellationToken cancelToken, String idUsuario, String token)
        {
            ObjetoUsuarioMO objetoUsuarioMO = new ObjetoUsuarioMO();
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_VALIDAR_CODIGO_TOKEN, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.TOKEN, System.Data.SqlDbType.NVarChar, Constante._255);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = token;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleRow, cancelToken);

                        if (_reader != null && _reader.HasRows)
                        {
                            if (await _reader.ReadAsync(cancelToken))
                            {
                                objetoUsuarioMO.Codigo = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? 0 : _reader.GetInt32(Constante._0);
                                objetoUsuarioMO.Mensaje = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                            }
                        }

                        _reader.Close();
                        _con.Close();
                        String mensaje = _reader != null ? Constante.MENSAJE_VALIDAR_CODIGO_TOKEN_ASYNC_OK : Constante.MENSAJE_VALIDAR_CODIGO_TOKEN_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_VALIDAR_CODIGO_TOKEN_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_VALIDAR_CODIGO_TOKEN_ASYNC, Constante.MENSAJE_VALIDAR_CODIGO_TOKEN_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoUsuarioMO;
        }

        public async Task<ObjetoUsuarioMO> RecuperarClaveAsync(CancellationToken cancelToken, String idUsuario, String enlace)
        {
            ObjetoUsuarioMO objetoUsuarioMO = new ObjetoUsuarioMO();
            try
            {
                using (_con = new SqlConnection(_conexionHostToHost))
                {
                    using (_cmd = new SqlCommand(Constante.SPW_HTH_RECUPERAR_CLAVE, _con))
                    {
                        _cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter par1 = _cmd.Parameters.Add(Constante.ID_USUARIO, System.Data.SqlDbType.NChar, Constante._6);
                        par1.Direction = System.Data.ParameterDirection.Input;
                        par1.Value = idUsuario;

                        SqlParameter par2 = _cmd.Parameters.Add(Constante.ENLACE, System.Data.SqlDbType.NVarChar, Constante._2000);
                        par2.Direction = System.Data.ParameterDirection.Input;
                        par2.Value = enlace;

                        await _con.OpenAsync(cancelToken);
                        _reader = await _cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleRow, cancelToken);

                        if (_reader != null && _reader.HasRows)
                        {
                            if (await _reader.ReadAsync(cancelToken))
                            {
                                objetoUsuarioMO.Codigo = await _reader.IsDBNullAsync(Constante._0, cancelToken) ? 0 : _reader.GetInt32(Constante._0);
                                objetoUsuarioMO.Mensaje = await _reader.IsDBNullAsync(Constante._1, cancelToken) ? String.Empty : _reader.GetString(Constante._1);
                            }
                        }

                        _reader.Close();
                        _con.Close();
                        String mensaje = _reader != null ? Constante.MENSAJE_RECUPERAR_CLAVE_ASYNC_OK : Constante.MENSAJE_RECUPERAR_CLAVE_ASYNC_NO_OK;
                        await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_RECUPERAR_CLAVE_ASYNC, mensaje);
                    }
                }
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_REPOSITORIO, Constante.CLASE_CUENTA_RE, Constante.METODO_RECUPERAR_CLAVE_ASYNC, Constante.MENSAJE_RECUPERAR_CLAVE_ASYNC_NO_OK, e.Message);
                throw e;
            }
            finally
            {
                if (_con.State != System.Data.ConnectionState.Closed)
                {
                    _con.Close();
                }
            }
            return objetoUsuarioMO;
        }
    }
}
