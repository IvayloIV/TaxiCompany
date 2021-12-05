using TaxiCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TaxiCompany.Models.Dto;

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

        public bool IsDriverIdExist(string id)
        {
            return goodsContext.Drivers
                .Any(d => d.Id.Equals(id));
        }

        public List<DriverOrderDto> GetDriversOrders(string ordersCountStr, string totalDistanceStr)
        {
            var query = goodsContext.Drivers
                .Include("Orders")
                .GroupBy(d => new { d.Id, d.Name, d.WorkExperience })
                .Select(d =>
                    new
                    {
                        id = d.Key.Id,
                        name = d.Key.Name,
                        workExperience = d.Key.WorkExperience,
                        ordersCount = d.Sum(o => o.Orders.Count()),
                        totalDistance = d.Sum(o => o.Orders.Count() == 0 ? 0 : o.Orders.Sum(or => or.Distance))
                    });

            if (ordersCountStr != null && ordersCountStr.Length > 0)
            {
                int ordersCount = int.Parse(ordersCountStr);
                query = query.Where(d => d.ordersCount.CompareTo(ordersCount) > 0);
            }

            if (totalDistanceStr != null && totalDistanceStr.Length > 0)
            {
                double totalDistance = double.Parse(totalDistanceStr);
                query = query.Where(d => d.totalDistance.CompareTo(totalDistance) > 0);
            }

            return query
                .OrderByDescending(d => d.totalDistance)
                .ToList()
                .Select(d => new DriverOrderDto(d.id, d.name, d.workExperience, d.ordersCount, d.totalDistance))
                .ToList();
        }
    }
}
