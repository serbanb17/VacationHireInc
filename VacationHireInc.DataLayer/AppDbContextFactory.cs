// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Text;
using VacationHireInc.DataLayer.Initializers;

namespace VacationHireInc.DataLayer
{
    internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            if(args.Length != 2)
                throw new Exception($"Creating new {nameof(AppDbContext)} requires exactly two parameters in the following order: database connection string, password salt string");
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(args[0]);
            Initializer.SetPasswordSalt(Encoding.Unicode.GetBytes(args[1]));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
