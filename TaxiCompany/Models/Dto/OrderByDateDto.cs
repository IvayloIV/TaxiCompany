using System;

namespace TaxiCompany.Models.Dto
{
    public class OrderByDateDto
    {
        public long Id { get; set; }
        public DateTime OrderDate { get; set; }
        public double Distance { get; set; }
        public string RegistrationPlate { get; set; }
        public string Brand { get; set; }
    }
}
