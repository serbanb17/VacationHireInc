// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.DataLayer.Configurations
{
    internal class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicle");
            builder.HasIndex(v => v.LicencePlate).IsUnique();
            builder.Property(v => v.LicencePlate).IsRequired().HasMaxLength(15);
        }
    }
}
