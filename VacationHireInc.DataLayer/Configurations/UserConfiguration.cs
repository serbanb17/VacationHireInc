// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.DataLayer.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.UserName).IsUnique();
            builder.Property(u => u.UserName).IsRequired();
            builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Surname).IsRequired().HasMaxLength(50);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.Password).IsRequired();
            builder.HasIndex(u => u.Privilege);
        }
    }
}
