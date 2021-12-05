using TaxiCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
