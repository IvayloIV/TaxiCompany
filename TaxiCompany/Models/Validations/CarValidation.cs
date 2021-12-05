using System;
using TaxiCompany.Dao;

namespace TaxiCompany.Models
{
    public class CarValidation : BaseModel
    {
        private string registrationPlateError;
        private string brandError;
        private string passengerSeatsError;

        public string RegistrationPlateError
        {
            get { return registrationPlateError; }
            set { registrationPlateError = value; OnPropertyChanged(nameof(RegistrationPlateError)); }
        }

        public string BrandError
        {
            get { return brandError; }
            set { brandError = value; OnPropertyChanged(nameof(BrandError)); }
        }

        public string PassengerSeatsError
        {
            get { return passengerSeatsError; }
            set { passengerSeatsError = value; OnPropertyChanged(nameof(PassengerSeatsError)); }
        }

        public bool ValidateCar(Car car, CarDao carDao)
        {
            bool hasErrors = false;

            if (car.RegistrationPlate == null || car.RegistrationPlate.Length <= 3)
            {
                RegistrationPlateError = "Регистрационният номер трябва да бъде повече от 3 символа.";
                hasErrors = true;
            }
            else if (carDao.IsRegistrationPlateExist(car.RegistrationPlate))
            {
                RegistrationPlateError = "Регистрационният номер вече съществува.";
                hasErrors = true;
            }
            else
            {
                RegistrationPlateError = string.Empty;
            }

            if (car.Brand == null || car.Brand.Length <= 1)
            {
                BrandError = "Брандът трябва да съдържа поне 2 символа.";
                hasErrors = true;
            }
            else
            {
                BrandError = string.Empty;
            }

            if (car.PassengerSeats <= 2 || car.PassengerSeats >= 11)
            {
                PassengerSeatsError = "Местата за пътници трябва да бъде число между 2 и 11.";
                hasErrors = true;
            }
            else
            {
                PassengerSeatsError = string.Empty;
            }

            return hasErrors;
        }
    }
}
