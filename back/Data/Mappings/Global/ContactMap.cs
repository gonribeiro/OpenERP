using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenERP.Models.Global;
using OpenERP.Enums.Global;
using OpenERP.Enums.HumanResource;

namespace OpenERP.Data.Mappings.Global
{
    public class ContactMap : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ModelType)
                .IsRequired();

            builder.Property(c => c.ModelId)
                .IsRequired();

            builder.Property(c => c.Type)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (ContactType)Enum.Parse(typeof(ContactType), v)
                );

            builder.Property(c => c.Information)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(c => c.ContactName)
                .HasMaxLength(120);

            builder.Property(c => c.ContactRelationType)
                .HasConversion(
                    v => v.ToString(),
                    v => (ContactRelationType)Enum.Parse(typeof(ContactRelationType), v)
                );

            /*builder.HasOne(c => c.Employee)
                .WithMany()
                .HasForeignKey(c => c.ModelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Company)
                .WithMany()
                .HasForeignKey(c => c.ModelId)
                .OnDelete(DeleteBehavior.Restrict);*/
        }
    }
}