// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.DataLayer.Configurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Surname).IsRequired().HasMaxLength(50);
            builder.HasIndex(c => c.Email).IsUnique();
            builder.Property(c => c.Email).IsRequired();
            builder.HasIndex(c => c.PhoneNumber).IsUnique();
            builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(15);
            builder.Property(c => c.Birthday).IsRequired();
        }
    }
}
