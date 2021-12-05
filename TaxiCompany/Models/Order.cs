using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiCompany.Models
{
    [Table("taxi_order")]
    public class Order : BaseModel
    {
        private long id;
        private string address;
        private DateTime orderDate;
        private double distance;
        private double fee;
        private long carId;
        private string driverId;
        private Car car;
        private Driver driver;

        [Key, Column("id")]
        public long Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        [Column("address")]
        public string Address
        {
            get { return address; }
            set { address = value; OnPropertyChanged(nameof(Address)); }
        }

        [Column("order_date")]
        public DateTime OrderDate
        {
            get { return orderDate; }
            set { orderDate = value; OnPropertyChanged(nameof(OrderDate)); }
        }

        [Column("distance")]
        public double Distance
        {
            get { return distance; }
            set { distance = value; OnPropertyChanged(nameof(Distance)); }
        }

        [Column("fee")]
        public double Fee
        {
            get { return fee; }
            set { fee = value; OnPropertyChanged(nameof(Fee)); }
        }

        [Column("car_id")]
        [ForeignKey("Car")]
        public long CarId
        {
            get { return carId; }
            set { carId = value; OnPropertyChanged(nameof(CarId)); }
        }

        [Column("driver_id")]
        [ForeignKey("Driver")]
        public string DriverId
        {
            get { return driverId; }
            set { driverId = value; OnPropertyChanged(nameof(DriverId)); }
        }

        public Car Car
        {
            get { return car; }
            set { car = value; }
        }

        public Driver Driver
        {
            get { return driver; }
            set { driver = value; }
        }
    }
}
