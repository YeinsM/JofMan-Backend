using BMS_Logistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMS_Logistics.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refreshToken");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .IsRequired();

            builder.Property(x => x.UserId)
                     .IsRequired();

            builder.Property(x => x.Token)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.ExpiryDate)
                    .IsRequired();

            builder.Property(x => x.DeviceInfo)
                .HasMaxLength(50);

            builder.Property(x => x.Used);
        }
    }
}
