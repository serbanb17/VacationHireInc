// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.DataLayer.Configurations
{
    internal class VehicleOrderConfiguration : IEntityTypeConfiguration<VehicleOrder>
    {
        public void Configure(EntityTypeBuilder<VehicleOrder> builder)
        {
            builder.ToTable("VehicleOrder");
            builder.Ignore(vo => vo.OtherCurrencyPrice);
            builder.HasOne(vo => vo.User).WithMany(u => u.VehicleOrders).HasForeignKey(vo => vo.UserId);
            builder.HasOne(vo => vo.Customer).WithMany(c => c.VehicleOrders).HasForeignKey(vo => vo.CustomerId);
        }
    }
}
