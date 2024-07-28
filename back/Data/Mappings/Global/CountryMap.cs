using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Models.Global;

namespace OpenERP.Data.Mappings.Global
{
    public class CountryMap : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(255);
            builder.HasIndex(c => c.Name)
                   .IsUnique();

            builder.Property(u => u.Nationality)
                   .IsRequired()
                   .HasMaxLength(255);
            builder.HasIndex(c => c.Nationality)
                   .IsUnique();

            builder.HasMany(c => c.States)
                   .WithOne(p => p.Country)
                   .HasForeignKey(p => p.CountryId);
        }
    }
}