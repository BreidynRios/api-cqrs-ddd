using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permission");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.EmployeeId)
                .HasColumnName("EmployeeId");

            builder.Property(p => p.PermissionTypeId)
                .HasColumnName("PermissionTypeId");

            builder.Property(p => p.CreatedBy)
                .HasColumnName("CreatedBy");

            builder.Property(p => p.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("datetime");

            builder.Property(p => p.UpdatedBy)
                .HasColumnName("UpdatedBy")
                .IsRequired(false);

            builder.Property(p => p.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("datetime")
                .IsRequired(false);

            builder.HasOne(p => p.Employee)
                .WithMany(p => p.Permissions)
                .HasForeignKey(p => p.EmployeeId);

            builder.HasOne(p => p.PermissionType)
                .WithMany(p => p.Permissions)
                .HasForeignKey(p => p.PermissionTypeId);
        }
    }
}
