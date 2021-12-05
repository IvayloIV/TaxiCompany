using TaxiCompany.Commands;
using TaxiCompany.Dao;
using TaxiCompany.Models;
using TaxiCompany.Models.Enums;
using TaxiCompany.Stores;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace TaxiCompany.ViewModels
{
    public class CarFormViewModel : BaseViewModel
    {
        private const string ADD_NEW_CAR = "-- Добави ново такси --";

        private Car car;
        private CarValidation carValidation;
        private string successMessage;
        private CarDao carDao;
        private ObservableCollection<string> carValues;
        private string selectedCarValue;
        private string labelText;

        public RelayCommand CreateCommand { get; }
        public ICommand NavigationBackCommand { get; }

        public CarFormViewModel(NavigationStore navigationStore)
        {
            CreateCommand = new RelayCommand(Create);
            NavigationBackCommand = new NavigateCommand<FormHomeViewModel>(navigationStore, (n) => new FormHomeViewModel(n));
            carDao = new CarDao();
            carValidation = new CarValidation();
            InitCar();
        }

        private void InitCar()
        {
            Car = new Car();
            UpdateCarValues(ADD_NEW_CAR);
            LabelText = Enum.GetName(typeof(OperationType), OperationType.Създаване);
        }

        private void UpdateCarValues(string defaultValue)
        {
            CarValues = new ObservableCollection<string>(carDao.GetAll().Select(a => $"{a.Id} - {a.RegistrationPlate} - {a.Brand}").ToList());
            CarValues.Insert(0, ADD_NEW_CAR);
            SelectedCarValue = defaultValue;
        }

        public Car Car
        {
            get { return car; }
            set { car = value; OnPropertyChanged(nameof(Car)); }
        }

        public CarValidation CarValidation
        {
            get { return carValidation; }
            set { carValidation = value; OnPropertyChanged(nameof(CarValidation)); }
        }

        public string SuccessMessage
        {
            get { return successMessage; }
            set { successMessage = value; OnPropertyChanged(nameof(SuccessMessage)); }
        }

        public ObservableCollection<string> CarValues
        {
            get { return carValues; }
            set { carValues = value; OnPropertyChanged(nameof(CarValues)); }
        }

        public string LabelText
        {
            get { return labelText; }
            set { labelText = value; OnPropertyChanged(nameof(LabelText)); }
        }

        public string SelectedCarValue
        {
            get { return selectedCarValue; }
            set
            {
                selectedCarValue = value;
                OnPropertyChanged(nameof(SelectedCarValue));
                SwapCar(selectedCarValue);
            }
        }

        private void SwapCar(string selectedCarValue)
        {
            SuccessMessage = string.Empty;
            CarValidation = new CarValidation();

            if (!selectedCarValue.Equals(ADD_NEW_CAR))
            {
                string carId = selectedCarValue.Split('-')[0].Trim();
                Car = carDao.FindById(long.Parse(carId));
                LabelText = Enum.GetName(typeof(OperationType), OperationType.Редактиране);
            }
            else
            {
                Car = new Car();
                LabelText = Enum.GetName(typeof(OperationType), OperationType.Създаване);
            }
        }

        private void Create()
        {
            if (!CarValidation.ValidateCar(Car, carDao))
            {
                if (!selectedCarValue.Equals(ADD_NEW_CAR))
                {
                    carDao.Update(Car);
                    UpdateCarValues($"{Car.Id} - {Car.RegistrationPlate} - {Car.Brand}");
                    SuccessMessage = "Успешно редактирахте таксито!";
                }
                else
                {
                    carDao.Save(Car);
                    InitCar();
                    SelectedCarValue = ADD_NEW_CAR;
                    SuccessMessage = "Успешно създадохте ново такси!";
                }
            }
            else
            {
                SuccessMessage = string.Empty;
            }
        }
    }
}
