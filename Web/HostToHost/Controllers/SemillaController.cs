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
using static Comun.Constante;

namespace HostToHost.Controllers
{
    [AllowAnonymous]
    public class SemillaController : Controller
    {
        private readonly UserManager<IdentityUserMO> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private CuentaNE _cuentaNE = null;

        public SemillaController(UserManager<IdentityUserMO> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<IdentityUserMO> signInManager) {
            _userManager = userManager;
            _roleManager = roleManager;
            _cuentaNE = _cuentaNE ?? new CuentaNE(configuration, userManager, signInManager);
        }

        [HttpPost]
        public async Task<IActionResult> InicializarRoles()
        {
            // Get the list of the roles from enum.
            Role[] roles = (Role[])Enum.GetValues(typeof(Role));

            foreach (Role item in roles)
            {
                // Create an identity role object out of the enum value.
                IdentityRole identityRole = new IdentityRole {
                    Id = item.getRoleId(),
                    Name = item.getRoleName()
                };

                // Create the role if it doesn't already exist.
                if (!await _roleManager.RoleExistsAsync(roleName: identityRole.Name))
                {
                    IdentityResult result = await _roleManager.CreateAsync(identityRole);

                    // Return 500 if it fails
                    if (!result.Succeeded) {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
            }

            // All good, Ok.
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> InicializarUsuario()
        {
            UsuarioMO usuarioMO = new UsuarioMO();
            usuarioMO.ApePaterno = Constante.POR_DEFECTO_APE_PATERNO;
            usuarioMO.ApeMaterno = Constante.POR_DEFECTO_APE_MATERNO;
            usuarioMO.Nombres = Constante.POR_DEFECTO_NOMBRES;
            usuarioMO.Usuario = Constante.POR_DEFECTO_USUARIO;
            usuarioMO.Correo = Constante.POR_DEFECTO_CORREO;
            usuarioMO.Celular = Constante.POR_DEFECTO_CELULAR;
            await _cuentaNE.CrearUsuarioDesconectadoAsync(new CancellationToken(false), usuarioMO);
            
            // Define the default user.
            IdentityUserMO identityMO = new IdentityUserMO
            {
                IdUsuario = Constante.POR_DEFECTO_ID_USUARIO,
                UserName = Constante.POR_DEFECTO_USUARIO,
                NormalizedUserName = Constante.POR_DEFECTO_USUARIO.ToUpper(),
                Email = Constante.POR_DEFECTO_CORREO,
                NormalizedEmail = Constante.POR_DEFECTO_CORREO.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = Constante.POR_DEFECTO_CELULAR,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = Constante._20
            };

            // Get the list of the roles from enum.
            Role[] roles = (Role[])Enum.GetValues(typeof(Role));

            // Create the default user if it doesn't already exist.
            if (await _userManager.FindByNameAsync(identityMO.UserName) == null) {
                // Do not check for credentials of any kind yet.
                IdentityResult result = await _userManager.CreateAsync(identityMO, password: Constante.POR_DEFECTO_CLAVE);

                // Return 500 if it fails
                if (!result.Succeeded) {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                // Asign all roles to the default user.
                result = await _userManager.AddToRolesAsync(identityMO, roles.Where(r => r.getRoleName() != Constante.ROL_ADMINISTRADOR).Select(r => r.getRoleName()));

                // Return 500 if it fails
                if (!result.Succeeded) {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            // All good, Ok.
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> InicializarAdministrador()
        {
            UsuarioMO usuarioMO = new UsuarioMO();
            usuarioMO.ApePaterno = Constante.POR_DEFECTO_APE_PATERNO_ADMINISTRADOR;
            usuarioMO.ApeMaterno = Constante.POR_DEFECTO_APE_MATERNO_ADMINISTRADOR;
            usuarioMO.Nombres = Constante.POR_DEFECTO_NOMBRES_ADMINISTRADOR;
            usuarioMO.Usuario = Constante.POR_DEFECTO_USUARIO_ADMINISTRADOR;
            usuarioMO.Correo = Constante.POR_DEFECTO_CORREO_ADMINISTRADOR;
            usuarioMO.Celular = Constante.POR_DEFECTO_CELULAR_ADMINISTRADOR;
            await _cuentaNE.CrearUsuarioDesconectadoAsync(new CancellationToken(false), usuarioMO);

            // Define the default user.
            IdentityUserMO identityMO = new IdentityUserMO {
                IdUsuario = Constante.POR_DEFECTO_ID_USUARIO_ADMINISTRADOR,
                UserName = Constante.POR_DEFECTO_USUARIO_ADMINISTRADOR,
                NormalizedUserName = Constante.POR_DEFECTO_USUARIO_ADMINISTRADOR.ToUpper(),
                Email = Constante.POR_DEFECTO_CORREO_ADMINISTRADOR,
                NormalizedEmail = Constante.POR_DEFECTO_CORREO_ADMINISTRADOR.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = Constante.POR_DEFECTO_CELULAR_ADMINISTRADOR,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = Constante._20
            };

            // Get the list of the roles from enum.
            Role[] roles = (Role[])Enum.GetValues(typeof(Role));

            // Create the default user if it doesn't already exist.
            if (await _userManager.FindByNameAsync(identityMO.UserName) == null) {
                // Do not check for credentials of any kind yet.
                IdentityResult result = await _userManager.CreateAsync(identityMO, password: Constante.POR_DEFECTO_CLAVE);

                // Return 500 if it fails
                if (!result.Succeeded) {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                // Asign all roles to the default user.
                result = await _userManager.AddToRolesAsync(identityMO, roles.Select(r => r.getRoleName()));

                // Return 500 if it fails
                if (!result.Succeeded) {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            // All good, Ok.
            return Ok();
        }
    }

    public static class Inicializador
    {
        public static String getRoleId(this Role role) {
            String idRol = String.Empty;

            switch (role) {
                case Role.Administrador:
                idRol = Constante.ID_ROL_ADMINISTRADOR;
                break;
                case Role.Coordinador:
                idRol = Constante.ID_ROL_COORDINADOR;
                break;
                case Role.Tesorero:
                idRol = Constante.ID_ROL_TESORERO;
                break;
                case Role.Aprobador:
                idRol = Constante.ID_ROL_APROBADOR;
                break;
                case Role.Supervisor:
                idRol = Constante.ID_ROL_SUPERVISOR;
                break;
            }

            return idRol;
        }

        public static String getRoleName(this Role role) {
            return role.ToString();
        }
    }
}