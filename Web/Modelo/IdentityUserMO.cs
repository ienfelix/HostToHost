using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Modelo
{
    public class IdentityUserMO : IdentityUser
    {
        [Required]
        [StringLength(6)]
        public String IdUsuario { get; set; }
    }
}
