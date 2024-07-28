using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Models.Auth;

namespace OpenERP.Data.Mappings.Auth
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.InactiveDate)
                .HasColumnType("Date");

            builder.Property(u => u.Name)
                .HasMaxLength(120);
            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.HasMany(u => u.RoleUser)
                .WithOne(fs => fs.Role)
                .HasForeignKey(fs => fs.RoleId);
        }
    }
}