using BMS_Logistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BMS_Logistics.Infrastructure.Persistence.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.RoleId).HasMaxLength(50);
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.SurName).HasMaxLength(50);
            builder.Property(x => x.UserName).HasMaxLength(20);
            builder.Property(x => x.Password).HasMaxLength(100);
            builder.Property(x => x.Email).HasMaxLength(50);
            builder.Property(x => x.ExpirationDate);
            builder.Property(x => x.StatusId);
            builder.Property(x => x.Attempts);
        }
    }
}
