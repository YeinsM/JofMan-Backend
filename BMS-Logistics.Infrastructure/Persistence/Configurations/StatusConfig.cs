using BMS_Logistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMS_Logistics.Infrastructure.Persistence.Configurations
{
    public class StatusConfig : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("statuses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Code).HasMaxLength(50);
            builder.Property(x => x.Table).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(50).IsRequired();
        }
    }
}
