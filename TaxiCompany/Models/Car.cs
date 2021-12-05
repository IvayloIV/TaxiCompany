using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiCompany.Models
{
    [Table("car")]
    public class Car : BaseModel
    {
        private long id;
        private string registrationPlate;
        private string brand;
        private int passengerSeats;
        private bool bigBootSpace;
        private bool technicalReview;
        private ICollection<Order> orders;

        [Key, Column("id")]
        public long Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        [Column("registration_plate")]
        public string RegistrationPlate
        {
            get { return registrationPlate; }
            set { registrationPlate = value; OnPropertyChanged(nameof(RegistrationPlate)); }
        }

        [Column("brand")]
        public string Brand
        {
            get { return brand; }
            set { brand = value; OnPropertyChanged(nameof(Brand)); }
        }

        [Column("passenger_seats")]
        public int PassengerSeats
        {
            get { return passengerSeats; }
            set { passengerSeats = value; OnPropertyChanged(nameof(PassengerSeats)); }
        }

        [Column("big_boot_space")]
        public bool BigBootSpace
        {
            get { return bigBootSpace; }
            set { bigBootSpace = value; OnPropertyChanged(nameof(BigBootSpace)); }
        }

        [Column("technical_review")]
        public bool TechnicalReview
        {
            get { return technicalReview; }
            set { technicalReview = value; OnPropertyChanged(nameof(TechnicalReview)); }
        }

        public ICollection<Order> Orders
        {
            get { return orders; }
            set { orders = value; }
        }
    }
}
