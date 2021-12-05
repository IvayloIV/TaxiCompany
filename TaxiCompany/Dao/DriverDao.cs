using TaxiCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxiCompany.Dao
{
    public class DriverDao
    {
        private readonly ITaxiCompanyContext goodsContext;

        public DriverDao()
        {
            goodsContext = TaxiCompanyContextSingleton.GetContext();
        }

        public void Save(Driver driver)
        {
            goodsContext.Drivers.Add(driver);
            goodsContext.SaveChanges();
        }

        public void Update(Driver driver)
        {
            Driver currentDriver = FindById(driver.Id);
            currentDriver.Name = driver.Name;
            currentDriver.Address = driver.Address;
            currentDriver.DrivingLicenceValidTo = driver.DrivingLicenceValidTo;
            currentDriver.WorkExperience = driver.WorkExperience;
            currentDriver.Nationality = driver.Nationality;
            goodsContext.SaveChanges();
        }

        public Driver FindById(string id)
        {
            return goodsContext.Drivers.Find(id);
        }

        public List<Driver> GetAll()
        {
            return goodsContext.Drivers.ToList();
        }
    }
}
