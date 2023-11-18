using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal class PermissionTypeEntityConfiguration : IEntityTypeConfiguration<PermissionType>
    {
        public void Configure(EntityTypeBuilder<PermissionType> builder)
        {
            builder.ToTable("PermissionType");
            builder.HasKey(pt => pt.Id);

            builder.Property(pt => pt.Name)
                .HasColumnName("Name")
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
