using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiCompany.Models
{
    [Table("driver")]
    public class Driver : BaseModel
    {
        private string id;
        private string name;
        private string address;
        private DateTime drivingLicenceValidTo;
        private int workExperience;
        private string nationality;
        private ICollection<Order> orders;

        [Key, Column("id")]
        public string Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        [Column("name")]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        [Column("address")]
        public string Address
        {
            get { return address; }
            set { address = value; OnPropertyChanged(nameof(Address)); }
        }

        [Column("driving_licence_valid_to")]
        public DateTime DrivingLicenceValidTo
        {
            get { return drivingLicenceValidTo; }
            set { drivingLicenceValidTo = value; OnPropertyChanged(nameof(DrivingLicenceValidTo)); }
        }

        [Column("work_experience")]
        public int WorkExperience
        {
            get { return workExperience; }
            set { workExperience = value; OnPropertyChanged(nameof(WorkExperience)); }
        }

        [Column("nationality")]
        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; OnPropertyChanged(nameof(Nationality)); }
        }

        public ICollection<Order> Orders
        {
            get { return orders; }
            set { orders = value; }
        }
    }
}
