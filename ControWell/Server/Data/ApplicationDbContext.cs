using ControWell.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ControWell.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }


        public DbSet<Tanque> Tanques { get; set; }
        public DbSet<Cinta> Cintas { get; set; }
        public DbSet<Termometro> Termometros { get; set; }
        public DbSet<Pozo> Pozos { get; set; }
        public DbSet<VariableProceso> VariableProcesos { get; set; }
        public DbSet<Alarma> Alarmas { get; set; }
        public DbSet<Aforo> AforoTKs { get; set; }       
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<NivelesDeLaCinta> NivelesDeLaCintas { get; set; }
        public DbSet<CalibracionTermometro> CalibracionTermometros { get; set; }
        public DbSet<Guia> Guias { get; set; }
        public DbSet<Sello> Sellos { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Operario> Operarios { get; set; }
        public DbSet<Conductor> Conductors { get; set; }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<EcopetrolMaterial> EcopetrolMaterials { get; set; }
        public DbSet<OfertaDiaria> OfertaDiarias { get; set; }
        
    }
}
