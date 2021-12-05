using TaxiCompany.Models;
using System.Data.Entity;

namespace TaxiCompany.Dao
{
    public class TaxiCompanyContext : DbContext, ITaxiCompanyContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public TaxiCompanyContext() : base(nameOrConnectionString: "TaxiCompanyDbConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("taxi_company");
            base.OnModelCreating(modelBuilder);
        }
    }
}
