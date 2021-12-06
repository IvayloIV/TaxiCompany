namespace TaxiCompany.Models.Dto
{
    public class CarByReviewDto
    {
        public long Id { get; set; }
        public string RegistrationPlate { get; set; }
        public string Brand { get; set; }
        public bool TechnicalReview { get; set; }
        public double TotalDistance { get; set; }

        public CarByReviewDto(long id, string registrationPlate, string brand, bool technicalReview, double totalDistance)
        {
            Id = id;
            RegistrationPlate = registrationPlate;
            Brand = brand;
            TechnicalReview = technicalReview;
            TotalDistance = totalDistance;
        }
    }
}
