using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;

namespace Comun
{
    public class Util
    {
        private Bitacora _bitacora = null;
        private String _servidor = String.Empty, _usuario = String.Empty, _clave = String.Empty, _carpetaEncriptado = String.Empty;
        private Int32 _puerto = 0;

        public Util(IConfiguration configuration)
        {
            _bitacora = _bitacora ?? new Bitacora(configuration);
            _servidor = configuration[Constante.SERVIDOR_SFTP_IP] ?? String.Empty;
            _puerto = configuration[Constante.SERVIDOR_SFTP_PUERTO] == String.Empty ? 0 : Convert.ToInt32(configuration[Constante.SERVIDOR_SFTP_PUERTO]);
            _usuario = configuration[Constante.SERVIDOR_SFTP_USUARIO] ?? String.Empty;
            _clave = configuration[Constante.SERVIDOR_SFTP_CLAVE] ?? String.Empty;
            _carpetaEncriptado = configuration[Constante.CARPETA_ENCRIPTADO] ?? String.Empty;
        }

        public async Task<Boolean> EncriptarYEnviarArchivoDesarrolloAsync(CancellationToken cancelToken, String nombreArchivo, String rutaArchivo)
        {
            Boolean esConforme = false, esEncriptado = false, esEnviado = false;
            try
            {
                if (nombreArchivo != String.Empty)
                {
                    String rutaDestino = String.Format("{0}{1}{2}", _carpetaEncriptado, nombreArchivo, Constante.EXTENSION_PGP);
                    Process process = new Process();
                    process.StartInfo.FileName = Constante.PGP_EXE;
                    String arguments = String.Format("{0} {1} {2} {3} {4} {5}", Constante.PGP_ENCRYPT, rutaArchivo, Constante.SCOTIABANK_DEV_KEY_ID, Constante.PGP_OUTPUT, _carpetaEncriptado, Constante.PGP_OVERWRITE);
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    esEncriptado = process.WaitForExit(Timeout.Infinite);

                    if (File.Exists(rutaDestino))
                    {
                        esEncriptado = true;
                        File.Delete(rutaArchivo);
                        rutaArchivo = String.Format("{0}{1}{2}", _carpetaEncriptado, nombreArchivo, Constante.EXTENSION_PGP);
                        nombreArchivo = nombreArchivo.Replace(Constante.EXTENSION_TXT, Constante.EXTENSION_PGP);
                        PasswordAuthenticationMethod authentication = new PasswordAuthenticationMethod(_usuario, _clave);
                        ConnectionInfo connection = new ConnectionInfo(_servidor, _puerto, _usuario, authentication);
                        
                        using (SftpClient sftpClient = new SftpClient(connection))
                        {
                            sftpClient.Connect();
                            Boolean isConnected = sftpClient.IsConnected;
                            
                            if (isConnected)
                            {
                                using (FileStream fileStream = File.OpenRead(rutaArchivo))
                                {
                                    String rutaRemota = String.Format("{0}{1}", Constante.CARPETA_IN, nombreArchivo);
                                    sftpClient.UploadFile(fileStream, rutaRemota);
                                    esEnviado = sftpClient.Exists(rutaRemota);
                                    esConforme = esEnviado == true ? true : false;
                                }
                            }
                            
                            sftpClient.Disconnect();
                            sftpClient.Dispose();
                        }
                    }
                }

                String mensajeEncriptado = esEncriptado ? Constante.MENSAJE_ENCRIPTAR_ARCHIVO_ASYNC_OK : Constante.MENSAJE_ENCRIPTAR_ARCHIVO_ASYNC_NO_OK;
                String mensajeEnviadp = esEnviado ? Constante.MENSAJE_ENVIAR_ARCHIVO_HACIA_BANCO_ASYNC_OK : Constante.MENSAJE_ENVIAR_ARCHIVO_HACIA_BANCO_ASYNC_NO_OK;
                String mensaje = esConforme ? String.Format("{0} | {1}", mensajeEncriptado, mensajeEncriptado) : String.Format("{0} | {1}", mensajeEncriptado, mensajeEncriptado);
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_COMUN, Constante.CLASE_UTIL, Constante.METODO_ENCRIPTAR_ARCHIVO_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_COMUN, Constante.CLASE_UTIL, Constante.METODO_ENCRIPTAR_ARCHIVO_ASYNC, Constante.MENSAJE_ENCRIPTAR_ARCHIVO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return esConforme;
        }

        public async Task<Boolean> EncriptarEnviarArchivoAsync(CancellationToken cancelToken, String nombreArchivo, String rutaArchivo)
        {
            Boolean esConforme = false, esEncriptado = false, esEnviado = false;
            String arguments = String.Empty, error = String.Empty, message = String.Empty;
            try
            {
                if (nombreArchivo != String.Empty)
                {
                    String rutaDestino = String.Format("{0}{1}{2}", _carpetaEncriptado, nombreArchivo, Constante.EXTENSION_PGP);
                    String carpetaEncriptado = _carpetaEncriptado.Substring(Constante._0, _carpetaEncriptado.LastIndexOf(Constante.DELIMITADOR_BACKSLASH));
                    String homeDirectory = string.Format("\"{0}\"", Constante.PGP_DIRECTORY);
                    arguments = String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", Constante.PGP_VERBOSE, Constante.PGP_HOME_DIRECTORY, homeDirectory, Constante.PGP_ENCRYPT, rutaArchivo, Constante.SCOTIABANK_DEV_KEY_ID, Constante.PGP_OUTPUT, carpetaEncriptado, Constante.PGP_OVERWRITE);
                    Process process = new Process();
                    process.StartInfo.FileName = Constante.PGP_EXE;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();
                    message = process.StandardOutput.ReadToEnd();
                    error = process.StandardError.ReadToEnd();
                    process.WaitForExit(Timeout.Infinite);
                    
                    if (File.Exists(rutaDestino))
                    {
                        esEncriptado = true;
                        File.Delete(rutaArchivo);
                        rutaArchivo = String.Format("{0}{1}{2}", _carpetaEncriptado, nombreArchivo, Constante.EXTENSION_PGP);
                        nombreArchivo = nombreArchivo.Replace(Constante.EXTENSION_TXT, Constante.EXTENSION_PGP);

                        PasswordAuthenticationMethod authentication = new PasswordAuthenticationMethod(_usuario, _clave);
                        ConnectionInfo connection = new ConnectionInfo(_servidor, _puerto, _usuario, authentication);
                        using (SftpClient sftpClient = new SftpClient(connection))
                        {
                            sftpClient.Connect();
                            Boolean isConnected = sftpClient.IsConnected;
                            
                            if (isConnected)
                            {
                                using (FileStream fileStream = File.OpenRead(rutaArchivo))
                                {
                                    String rutaRemota = String.Format("{0}{1}", Constante.CARPETA_IN, nombreArchivo);
                                    sftpClient.UploadFile(fileStream, rutaRemota);
                                    esEnviado = sftpClient.Exists(rutaRemota);
                                    esConforme = esEnviado == true ? true : false;
                                }
                            }
                            
                            sftpClient.Disconnect();
                            sftpClient.Dispose();
                        }
                    }
                }

                String mensajeEncriptado = esEncriptado ? Constante.MENSAJE_ENCRIPTAR_ARCHIVO_ASYNC_OK : Constante.MENSAJE_ENCRIPTAR_ARCHIVO_ASYNC_NO_OK;
                String mensajeEnviado = esEnviado ? Constante.MENSAJE_ENVIAR_ARCHIVO_HACIA_BANCO_ASYNC_OK : Constante.MENSAJE_ENVIAR_ARCHIVO_HACIA_BANCO_ASYNC_NO_OK;
                String mensaje = esConforme ? String.Format("{0} {1}", Constante.MENSAJE_ENCRIPTAR_ARCHIVO_ASYNC_OK, Constante.MENSAJE_ENVIAR_ARCHIVO_HACIA_BANCO_ASYNC_OK) : String.Format("{0} | {1} | {2} | {3} | {4}", mensajeEncriptado, mensajeEncriptado, arguments, message, error);
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_NOTIFICACION, Constante.PROYECTO_COMUN, Constante.CLASE_UTIL, Constante.METODO_ENCRIPTAR_ENVIAR_ARCHIVO_ASYNC, mensaje);
            }
            catch (Exception e)
            {
                await _bitacora.RegistrarEventoAsync(cancelToken, Constante.BITACORA_ERROR, Constante.PROYECTO_COMUN, Constante.CLASE_UTIL, Constante.METODO_ENCRIPTAR_ENVIAR_ARCHIVO_ASYNC, Constante.MENSAJE_ENCRIPTAR_ARCHIVO_ASYNC_NO_OK, e.Message);
                throw e;
            }
            return esConforme;
        }
    }
}