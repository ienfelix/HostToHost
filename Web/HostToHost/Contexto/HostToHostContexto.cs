using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modelo;

namespace HostToHost.Contexto
{
    public class HostToHostContexto : IdentityDbContext<IdentityUserMO> 
    {
        public HostToHostContexto(DbContextOptions<HostToHostContexto> options) : base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        }
    }

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<IdentityUserMO>
    {
        public void Configure(EntityTypeBuilder<IdentityUserMO> builder)
        {
            builder.Property(u => u.IdUsuario).HasColumnType("nchar(6)");
            builder.Property(u => u.IdUsuario).IsRequired();
        }
    }
}