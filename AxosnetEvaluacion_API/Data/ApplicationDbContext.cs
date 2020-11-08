using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AxosnetEvaluacion_API.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Moneda> Monedas { get; set; }
        public DbSet<Proveedor> Proveedors { get; set; }
        public DbSet<Recibo> Recibos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
