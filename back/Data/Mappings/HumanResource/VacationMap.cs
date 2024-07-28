using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Enums.HumanResource;
using OpenERP.Models.HumanResource;

namespace OpenERP.Data.Mappings.HumanResource
{
    public class VacationMap : IEntityTypeConfiguration<Vacation>
    {
        public void Configure(EntityTypeBuilder<Vacation> builder)
        {
            builder.HasKey(ev => ev.Id);

            builder.Property(u => u.EmployeeId)
                .IsRequired();

            builder.Property(r => r.Type)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (VacationType)Enum.Parse(typeof(VacationType), v)
                );

            builder.Property(ev => ev.StartDate)
                .HasColumnType("Date")
                .IsRequired();

            builder.Property(ev => ev.EndDate)
                .HasColumnType("Date")
                .IsRequired();

            builder.Property(ev => ev.Reason)
                .HasMaxLength(255);

            builder.Property(u => u.ApprovedById);

            builder
                .HasOne(v => v.Employee)
                .WithMany(e => e.Vacations)
                .HasForeignKey(v => v.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(v => v.ApprovedBy)
                .WithMany(e => e.ApprovedVacations)
                .HasForeignKey(v => v.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}