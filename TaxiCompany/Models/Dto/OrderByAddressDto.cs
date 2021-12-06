using System;

namespace TaxiCompany.Models.Dto
{
    public class OrderByAddressDto
    {
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
        public double Distance { get; set; }
        public string DriverName { get; set; }
        public string CarBrand { get; set; }
    }
}
