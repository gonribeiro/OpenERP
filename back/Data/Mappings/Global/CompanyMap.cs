using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Models.Global;

namespace OpenERP.Data.Mappings.Global
{
    public class CompanyMap : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.LegalName)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(c => c.LegalName)
                .IsUnique();

            builder.Property(e => e.TradeName)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(c => c.TradeName)
                .IsUnique();

            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasColumnName("Type");

            builder.Property(u => u.CityId);

            builder.Property(e => e.Address)
                .HasMaxLength(120);

            builder.Property(e => e.ZipCode)
                .HasMaxLength(40);

            builder.Property(e => e.ProductAndServiceDescription);

            builder.HasOne(e => e.City)
                .WithMany(c => c.Companies)
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            /*builder.HasMany(u => u.Contacts)
                .WithOne(c => c.Company)
                .HasForeignKey(c => c.ModelId)
                .OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}