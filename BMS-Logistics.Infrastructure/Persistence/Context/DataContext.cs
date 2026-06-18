using BMS_Logistics.API.Interfaces;
using BMS_Logistics.Domain.Entities;
using BMS_Logistics.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BMS_Logistics.Infrastructure.Persistence.Context
{
    public class DataContext : DbContext
    {
        private readonly IAuditableEntityService _auditableService;

        public DataContext(DbContextOptions<DataContext> options, IAuditableEntityService auditableEntityService) : base(options) 
        {
            _auditableService = auditableEntityService;
        }

        // Security Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Configurations of models

            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new StatusConfig());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfig());

            #endregion
        }

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }

        private void ApplyAuditInfo()
        {
            var auditableEntities = ChangeTracker
                .Entries<SharedProperty>()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            _auditableService.ApplyAuditInfo(auditableEntities.Select(e => e.Entity));
        }
    }
}
