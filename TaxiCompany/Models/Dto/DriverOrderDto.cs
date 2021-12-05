namespace TaxiCompany.Models.Dto
{
    public class DriverOrderDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int WorkExperience { get; set; }
        public int OrdersCount { get; set; }
        public double TotalDistance { get; set; }

        public DriverOrderDto(string id, string name, int workExperience, int ordersCount, double totalDistance)
        {
            Id = id;
            Name = name;
            WorkExperience = workExperience;
            OrdersCount = ordersCount;
            TotalDistance = totalDistance;
        }
    }
}
