using TaxiCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TaxiCompany.Models.Dto;

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
            Order currentOrder = FindById(order.Id);
            currentOrder.Address = order.Address;
            currentOrder.OrderDate = order.OrderDate;
            currentOrder.Distance = order.Distance;
            currentOrder.Fee = order.Fee;
            goodsContext.SaveChanges();
        }

        public Order FindById(long orderId)
        {
            return goodsContext.Orders
                .Where(o => o.Id.Equals(orderId))
                .FirstOrDefault();
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

        public List<OrderByDateDto> OrdersByDateAndRegistrationPlate(DateTime orderDate, string registrationPlate)
        {
            IQueryable<Order> query = goodsContext.Orders
                .Include("Car")
                .Where(d => d.OrderDate.CompareTo(orderDate) <= 0);

            if (registrationPlate != null && registrationPlate.Length > 0)
            {
                query = query.Where(o => o.Car.RegistrationPlate.Equals(registrationPlate));
            }

            return query.OrderByDescending(o => o.OrderDate)
                .ToList()
                .Select(o =>
                {
                    OrderByDateDto obdd = new OrderByDateDto();
                    obdd.Id = o.Id;
                    obdd.OrderDate = o.OrderDate;
                    obdd.Distance = o.Distance;
                    obdd.RegistrationPlate = o.Car.RegistrationPlate;
                    obdd.Brand = o.Car.Brand;
                    return obdd;
                })
                .ToList();
        }

        public List<OrderByAddressDto> OrdersByAddress(string address)
        {
            IQueryable<Order> query = goodsContext.Orders
                .Include("Car")
                .Include("Driver");

            if (address != null && address.Length > 0)
            {
                query = query.Where(o => o.Address.Contains(address));
            }

            return query.OrderByDescending(o => o.OrderDate)
                .ToList()
                .Select(o =>
                {
                    OrderByAddressDto oad = new OrderByAddressDto();
                    oad.Address = o.Address;
                    oad.OrderDate = o.OrderDate;
                    oad.Distance = o.Distance;
                    oad.DriverName = o.Driver.Name;
                    oad.CarBrand = o.Car.Brand;
                    return oad;
                })
                .ToList();
        }
    }
}
