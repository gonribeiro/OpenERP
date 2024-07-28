using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Models.Global;

namespace OpenERP.Data.Mappings.Global
{
    public class StateMap : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.Property(u => u.CountryId)
                .IsRequired();

            builder.HasOne(s => s.Country)
               .WithMany(c => c.States)
               .HasForeignKey(s => s.CountryId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Cities)
                .WithOne(p => p.State)
                .HasForeignKey(p => p.StateId);
        }
    }
}