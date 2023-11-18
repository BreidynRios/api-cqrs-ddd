using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .HasColumnName("Name")
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Surname)
                .HasColumnName("Surname")
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.DocumentNumber)
                .HasColumnName("DocumentNumber")
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Address)
                .HasColumnName("Address")
                .HasMaxLength(200)
                .IsRequired(false)
                .IsUnicode(false);

            builder.Property(e => e.CreatedBy)
                .HasColumnName("CreatedBy");

            builder.Property(e => e.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("datetime");

            builder.Property(e => e.UpdatedBy)
                .HasColumnName("UpdatedBy")
                .IsRequired(false);

            builder.Property(e => e.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("datetime")
                .IsRequired(false);
        }
    }
}
