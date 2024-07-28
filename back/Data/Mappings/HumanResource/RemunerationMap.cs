using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpenERP.Models.HumanResource;

namespace OpenERP.Data.Mappings.HumanResource
{
    public class RemunerationMap : IEntityTypeConfiguration<Remuneration>
    {
        public void Configure(EntityTypeBuilder<Remuneration> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.EmployeeId)
                .IsRequired();

            builder.Property(r => r.Currency)
                .IsRequired()
                .HasConversion<string>()
                .HasColumnType("varchar(10)")
                .HasColumnName("Currency");

            builder.Property(r => r.BaseSalary)
                .IsRequired()
                .HasColumnType("decimal(13, 2)");

            builder.Property(r => r.Bonus)
                .HasColumnType("decimal(13, 2)");

            builder.Property(r => r.Commission)
                .HasColumnType("decimal(13, 2)");

            builder.Property(r => r.OtherAllowances)
                .HasColumnType("decimal(13, 2)");

            builder.Property(r => r.OtherAllowancesDescription)
                .HasMaxLength(255);

            builder.Property(r => r.StartDate)
                .IsRequired()
                .HasColumnType("Date");

            builder.Property(r => r.EndDate)
                .HasColumnType("Date");

            builder.HasOne(r => r.Employee)
                .WithMany(e => e.Remunerations)
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}