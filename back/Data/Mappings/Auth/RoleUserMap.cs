using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Models.Auth;

namespace OpenERP.Data.Mappings.Auth
{
    public class RoleUserMap : IEntityTypeConfiguration<RoleUser>
    {
        public void Configure(EntityTypeBuilder<RoleUser> builder)
        {
            builder.HasKey(ru => ru.Id);

            builder.Property(ru => ru.Id)
                .IsRequired();

            builder.Property(ru => ru.RoleId)
                .HasColumnName("RoleId")
                .IsRequired();

            builder.Property(ru => ru.UserId)
                .HasColumnName("UserId")
                .IsRequired();

            builder.HasOne(ru => ru.Role)
                .WithMany()
                .HasForeignKey(ru => ru.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ru => ru.User)
                .WithMany()
                .HasForeignKey(ru => ru.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}