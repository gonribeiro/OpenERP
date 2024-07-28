using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Enums.HumanResource;
using OpenERP.Models.HumanResource;

namespace OpenERP.Data.Mappings.HumanResource
{
    public class EmployeeMap : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.InactiveDate)
                   .HasColumnType("Date");

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(120);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(u => u.Birthdate)
                   .IsRequired()
                   .HasColumnType("Date");

            builder.Property(r => r.MaritalStatus)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (MaritalStatus)Enum.Parse(typeof(MaritalStatus), v)
                );

            builder.Property(u => u.NationalityId)
                   .IsRequired();

            builder.Property(u => u.PlaceOfBirth)
                   .HasMaxLength(120);

            builder.Property(u => u.BankId);

            builder.Property(u => u.AccountNumber);

            builder.Property(u => u.RoutingNumber);

            builder.Property(u => u.Address)
                   .IsRequired()
                   .HasMaxLength(120);

            builder.Property(u => u.ZipCode)
                   .IsRequired()
                   .HasMaxLength(40);

            builder.Property(u => u.CityId)
                   .IsRequired();

            builder.Property(u => u.SocialSecurityNumber)
                   .IsRequired()
                   .HasMaxLength(40);
            builder.HasIndex(c => c.SocialSecurityNumber)
                   .IsUnique();

            builder.Property(u => u.PassportNumber);
            builder.HasIndex(c => c.PassportNumber)
                   .IsUnique();

            builder.Property(u => u.DriverLicenseNumber);
            builder.HasIndex(c => c.DriverLicenseNumber)
                   .IsUnique();

            builder.HasOne(u => u.Nationality)
                   .WithMany()
                   .HasForeignKey(u => u.NationalityId);

            builder.HasOne(u => u.City)
                   .WithMany()
                   .HasForeignKey(u => u.CityId);

            builder.HasOne(u => u.Bank)
                   .WithMany()
                   .HasForeignKey(u => u.BankId);

            builder.HasMany(u => u.DepartmentEmployee)
                   .WithOne(du => du.Employee)
                   .HasForeignKey(du => du.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.JobHistories)
                   .WithOne(jh => jh.Employee)
                   .HasForeignKey(jh => jh.EmployeeId);

            builder.HasMany(u => u.EducationHistories)
                   .WithOne(du => du.Employee)
                   .HasForeignKey(du => du.EmployeeId);

            builder
                .HasMany(e => e.Vacations)
                .WithOne(v => v.Employee)
                .HasForeignKey(v => v.EmployeeId);

            builder
                .HasMany(e => e.ApprovedVacations)
                .WithOne(v => v.ApprovedBy)
                .HasForeignKey(v => v.ApprovedById);

            /*builder.HasMany(u => u.Contacts)
                   .WithOne(c => c.Employee)
                   .HasForeignKey(c => c.ModelId)
                   .OnDelete(DeleteBehavior.Cascade);*/

            /*builder.HasMany(u => u.FileStorage)
                   .WithOne(fs => fs.Employee)
                   .HasForeignKey(fs => fs.ModelId);*/
        }
    }
}