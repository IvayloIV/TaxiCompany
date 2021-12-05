using TaxiCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxiCompany.Dao
{
    public class OrderDao
    {
        private readonly ITaxiCompanyContext goodsContext;

        public OrderDao()
        {
            goodsContext = TaxiCompanyContextSingleton.GetContext();
        }

        public void Save(Order order)
        {
            goodsContext.Orders.Add(order);
            goodsContext.SaveChanges();
        }

        public void Update(Order order)
        {
            Order currentOrder = FindByDriverIdAndCarId(order.DriverId, order.CarId);
            currentOrder.Address = order.Address;
            currentOrder.OrderDate = order.OrderDate;
            currentOrder.Distance = order.Distance;
            currentOrder.Fee = order.Fee;
            goodsContext.SaveChanges();
        }

        public Order FindByDriverIdAndCarId(string driverId, long carId)
        {
            return goodsContext.Orders
                .Where(d => d.DriverId.Equals(driverId) && d.CarId.Equals(carId))
                .FirstOrDefault();
        }

        public Order FindByDriverIdAndCarRegistrationPlate(string driverId, string registrationPlate)
        {
            return goodsContext.Orders
                .Include("Car")
                .Where(d => d.DriverId.Equals(driverId) && d.Car.RegistrationPlate.Equals(registrationPlate))
                .FirstOrDefault();
        }

        public List<Order> FindByDriverId(string driverId)
        {
            return goodsContext.Orders
                .Include("Car")
                .Where(d => d.DriverId.Equals(driverId))
                .ToList();
        }

        public List<Order> GetAll()
        {
            return goodsContext.Orders.ToList();
        }
    }
}
