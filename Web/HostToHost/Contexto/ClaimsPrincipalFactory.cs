using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Comun;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Modelo;

namespace Contexto
{
    public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityUserMO, IdentityRole>
    {
        public ClaimsPrincipalFactory(UserManager<IdentityUserMO> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUserMO user) {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim(Constante.ID_USUARIO, user.IdUsuario ?? ""));
            return identity;
        }
    }
}