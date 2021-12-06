using TaxiCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiCompany.Models.Dto;

namespace TaxiCompany.Dao
{
    public class CarDao
    {
        private readonly ITaxiCompanyContext goodsContext;

        public CarDao()
        {
            goodsContext = TaxiCompanyContextSingleton.GetContext();
        }

        public void Save(Car car)
        {
            goodsContext.Cars.Add(car);
            goodsContext.SaveChanges();
        }

        public void Update(Car car)
        {
            Car currentCar = FindById(car.Id);
            currentCar.RegistrationPlate = car.RegistrationPlate;
            currentCar.Brand = car.Brand;
            currentCar.PassengerSeats = car.PassengerSeats;
            currentCar.BigBootSpace = car.BigBootSpace;
            currentCar.TechnicalReview = car.TechnicalReview;
            goodsContext.SaveChanges();
        }

        public Car FindById(long id)
        {
            return goodsContext.Cars.Find(id);
        }

        public List<Car> GetAll()
        {
            return goodsContext.Cars.ToList();
        }

        public bool IsRegistrationPlateExist(string registrationPlate)
        {
            return goodsContext.Cars
                .Any(c => c.RegistrationPlate.Equals(registrationPlate));
        }

        public List<string> GetCarBrands()
        {
            return goodsContext.Cars
                .GroupBy(c => c.Brand)
                .Select(c => c.Key)
                .OrderBy(b => b)
                .ToList();
        }

        public List<CarOrderDto> GetCarsOrders(string registrationPlate, string brand)
        {
            var query = goodsContext.Cars
                .Include("Orders")
                .GroupBy(c => new { c.Id, c.RegistrationPlate, c.Brand })
                .Select(c =>
                    new
                    {
                        id = c.Key.Id,
                        registrationPlate = c.Key.RegistrationPlate,
                        brand = c.Key.Brand,
                        ordersCount = c.Sum(o => o.Orders.Count()),
                        totalDistance = c.Sum(o => o.Orders.Count() == 0 ? 0 : o.Orders.Sum(or => or.Distance))
                    });

            if (registrationPlate != null && registrationPlate.Length > 0)
            {
                query = query.Where(c => c.registrationPlate.Contains(registrationPlate));
            }

            if (brand != null && brand.Length > 0)
            {
                query = query.Where(c => c.brand.Equals(brand));
            }

            return query
                .OrderByDescending(c => c.ordersCount)
                .ToList()
                .Select(c => new CarOrderDto(c.id, c.registrationPlate, c.brand, c.ordersCount, c.totalDistance))
                .ToList();
        }

        public List<CarByReviewDto> GetCarsByReview(bool technicalReview)
        {
            return goodsContext.Cars
                .Include("Orders")
                .GroupBy(c => new { c.Id, c.RegistrationPlate, c.Brand, c.TechnicalReview })
                .Select(c =>
                    new
                    {
                        id = c.Key.Id,
                        registrationPlate = c.Key.RegistrationPlate,
                        brand = c.Key.Brand,
                        technicalReview = c.Key.TechnicalReview,
                        totalDistance = c.Sum(o => o.Orders.Count() == 0 ? 0 : o.Orders.Sum(or => or.Distance))
                    })
                .Where(c => c.technicalReview == technicalReview)
                .OrderByDescending(c => c.totalDistance)
                .ToList()
                .Select(c => new CarByReviewDto(c.id, c.registrationPlate, c.brand, c.technicalReview, c.totalDistance))
                .ToList();
        }
    }
}
