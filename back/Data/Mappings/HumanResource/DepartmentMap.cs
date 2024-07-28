using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Models.HumanResource;

namespace OpenERP.Data.Mappings.HumanResource
{
    public class DepartmentMap : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(40);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.Property(u => u.ManagerId);

            builder.HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.JobHistories)
               .WithOne(jh => jh.Department)
               .HasForeignKey(jh => jh.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}