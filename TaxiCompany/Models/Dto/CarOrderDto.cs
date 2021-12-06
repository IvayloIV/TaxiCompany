namespace TaxiCompany.Models.Dto
{
    public class CarOrderDto
    {
        public long Id { get; set; }
        public string RegistrationPlate { get; set; }
        public string Brand { get; set; }
        public int OrdersCount { get; set; }
        public double TotalDistance { get; set; }

        public CarOrderDto(long id, string registrationPlate, string brand, int ordersCount, double totalDistance)
        {
            Id = id;
            RegistrationPlate = registrationPlate;
            Brand = brand;
            OrdersCount = ordersCount;
            TotalDistance = totalDistance;
        }
    }
}
