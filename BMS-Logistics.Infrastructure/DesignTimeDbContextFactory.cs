using BMS_Logistics.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BMS_Logistics.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../BMS-Logistics.API");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            optionsBuilder.UseNpgsql(connectionString);

            return new DataContext(optionsBuilder.Options, new DesignTimeAuditableEntityService());

            //// Build configuration to read appsettings.json from the Infrastructure project
            //var basePath = Directory.GetCurrentDirectory();
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(basePath)
            //    .AddJsonFile("appsettings.json", optional: true)
            //    .AddJsonFile($"appsettings.Development.json", optional: true)
            //    .AddEnvironmentVariables();

            //var config = builder.Build();

            //var connectionString = config.GetConnectionString("DefaultConnection")
            //    ?? config["ConnectionStrings:DefaultConnection"];

            //if (string.IsNullOrWhiteSpace(connectionString))
            //    throw new InvalidOperationException("No se ha encontrado la cadena de conexión 'DefaultConnection' en appsettings.");

            //var optionsBuilder = new DbContextOptionsBuilder<BMS_Logistics.Infrastructure.Persistence.Context.DataContext>();
            //optionsBuilder.UseSqlServer(connectionString);

            //// Crear e inyectar la implementación de tiempo de diseño del servicio auditable
            //var auditableService = new DesignTimeAuditableEntityService();

            //return new Persistence.Context.DataContext(optionsBuilder.Options, auditableService);
        }
    }
}
