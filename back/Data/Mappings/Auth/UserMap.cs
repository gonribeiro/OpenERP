using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Models.Auth;

namespace OpenERP.Data.Mappings.Auth
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.InactiveDate)
                .HasColumnType("Date");

            builder.Property(u => u.Username)
                .HasMaxLength(120);
            builder.HasIndex(c => c.Username)
                .IsUnique();

            builder.Property(u => u.Password)
                .HasMaxLength(255);

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnType("DateTime");

            builder.Property(u => u.LastPasswordUpdatedAt)
                .HasColumnType("DateTime");

            builder.HasMany(u => u.RoleUser)
                .WithOne(fs => fs.User)
                .HasForeignKey(fs => fs.UserId);
        }
    }
}