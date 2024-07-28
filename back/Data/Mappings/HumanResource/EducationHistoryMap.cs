using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenERP.Models.HumanResource;

namespace OpenERP.Data.Mappings.HumanResource
{
    public class EducationHistoryMap : IEntityTypeConfiguration<EducationHistory>
    {
        public void Configure(EntityTypeBuilder<EducationHistory> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.EmployeeId)
                .IsRequired();

            builder.Property(e => e.InstitutionId)
                .IsRequired();

            builder.Property(e => e.Course)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.EducationLevel)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.StartDate)
                .IsRequired()
                .HasColumnType("Date");

            builder.Property(e => e.EndDate)
                .IsRequired()
                .HasColumnType("Date");

            builder.HasOne(e => e.Employee)
                .WithMany(p => p.EducationHistories)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Institution)
                .WithMany()
                .HasForeignKey(e => e.InstitutionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
