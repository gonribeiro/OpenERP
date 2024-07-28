using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Models.HumanResource;

namespace OpenERP.Data.Mappings.HumanResource
{
    public class JobMap : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(j => j.Name)
                .IsRequired()
                .HasMaxLength(120);
            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.Property(j => j.Currency)
                .IsRequired()
                .HasConversion<string>()
                .HasColumnType("varchar(10)")
                .HasColumnName("Currency");

            builder.Property(j => j.MinSalary)
                .HasColumnType("decimal(13,2)");

            builder.Property(j => j.MaxSalary)
                .HasColumnType("decimal(13,2)");

            builder.HasMany(j => j.JobHistories)
                .WithOne(jh => jh.Job)
                .HasForeignKey(jh => jh.JobId);
        }
    }
}