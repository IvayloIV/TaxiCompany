using TaxiCompany.Models;
using System.Data.Entity;

namespace TaxiCompany.Dao
{
    public interface ITaxiCompanyContext
    {
        DbSet<Car> Cars { get; set; }
        DbSet<Driver> Drivers { get; set; }
        DbSet<Order> Orders { get; set; }
        Database Database { get; }

        void Dispose();

        int SaveChanges();
    }
}
