using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Models.Global;

namespace OpenERP.Data.Mappings.Global
{
    public class CityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.Property(u => u.StateId)
                .IsRequired();

            builder.HasOne(s => s.State)
               .WithMany(c => c.Cities)
               .HasForeignKey(s => s.StateId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}