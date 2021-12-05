using System;

namespace TaxiCompany.Models
{
    public class OrderValidation : BaseModel
    {
        private string addressError;
        private string distanceError;
        private string feeError;

        public string AddressError
        {
            get { return addressError; }
            set { addressError = value; OnPropertyChanged(nameof(AddressError)); }
        }

        public string DistanceError
        {
            get { return distanceError; }
            set { distanceError = value; OnPropertyChanged(nameof(DistanceError)); }
        }

        public string FeeError
        {
            get { return feeError; }
            set { feeError = value; OnPropertyChanged(nameof(FeeError)); }
        }

        public bool ValidateOrder(Order order)
        {
            bool hasErrors = false;

            if (order.Address == null || order.Address.Length <= 4)
            {
                AddressError = "Адресът трябва да бъде повече от 4 символа.";
                hasErrors = true;
            }
            else
            {
                AddressError = string.Empty;
            }

            if (order.Distance <= 0)
            {
                DistanceError = "Изминатото разстояние трябва да бъде положително число.";
                hasErrors = true;
            }
            else
            {
                DistanceError = string.Empty;
            }

            if (order.Fee <= 0)
            {
                FeeError = "Таксата трябва да бъде положително число.";
                hasErrors = true;
            }
            else
            {
                FeeError = string.Empty;
            }

            return hasErrors;
        }
    }
}
