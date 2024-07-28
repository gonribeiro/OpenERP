using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Models.HumanResource;

namespace OpenERP.Data.Mappings.HumanResource
{
    public class JobHistoryMap : IEntityTypeConfiguration<JobHistory>
    {
        public void Configure(EntityTypeBuilder<JobHistory> builder)
        {
            builder.HasKey(jh => jh.Id);

            builder.Property(jh => jh.EmployeeId)
                .IsRequired();

            builder.Property(jh => jh.JobId)
                .IsRequired();

            builder.Property(jh => jh.DepartmentId)
                .IsRequired();

            builder.Property(jh => jh.StartDate)
                .IsRequired()
                .HasColumnType("Date");

            builder.Property(jh => jh.EndDate)
                .HasColumnType("Date");

            builder.HasOne(jh => jh.Job)
                .WithMany(j => j.JobHistories)
                .HasForeignKey(jh => jh.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(jh => jh.Department)
                .WithMany(d => d.JobHistories)
                .HasForeignKey(jh => jh.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(jh => jh.Employee)
                .WithMany(e => e.JobHistories)
                .HasForeignKey(jh => jh.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}