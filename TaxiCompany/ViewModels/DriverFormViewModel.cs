using TaxiCompany.Commands;
using TaxiCompany.Dao;
using TaxiCompany.Models;
using TaxiCompany.Models.Enums;
using TaxiCompany.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace TaxiCompany.ViewModels
{
    public class DriverFormViewModel : BaseViewModel
    {
        private const string ADD_NEW_DRIVER = "-- Добави нов шофьор --";
        private const string ADD_NEW_ORDER = "-- Добави новa поръчка --";

        private ObservableCollection<Order> orders;
        private CarDao carDao;

        private Driver driver;
        private DriverValidation driverValidation;
        private string successMessage;
        private DriverDao driverDao;
        private OrderDao orderDao;
        private ObservableCollection<string> driverValues;
        private string selectedDriverValue;
        private string labelText;
        
        private Order order;
        private OrderValidation orderValidation;
        private string successMessageOrder;
        private ObservableCollection<string> orderValues;
        private string selectedOrderValue;
        private string labelTextOrder;

        private ObservableCollection<string> carValues;
        private string selectedCarValue;

        private string carVisible;
        private string orderVisible;
        private bool isDriverIdReadOnly;

        public RelayCommand CreateDriverCommand { get; }
        public RelayCommand CreateOrderCommand { get; }
        public ICommand NavigationBackCommand { get; }

        public DriverFormViewModel(NavigationStore navigationStore)
        {
            CreateDriverCommand = new RelayCommand(CreateDriver);
            CreateOrderCommand = new RelayCommand(CreateOrder);
            NavigationBackCommand = new NavigateCommand<FormHomeViewModel>(navigationStore, (n) => new FormHomeViewModel(n));
            driverDao = new DriverDao();
            orderDao = new OrderDao();
            carDao = new CarDao();
            driverValidation = new DriverValidation();
            InitDriver();
            InitOrder();
            InitCar();
            InitOrders();
        }

        private void InitDriver()
        {
            Driver = new Driver();
            Driver.DrivingLicenceValidTo = DateTime.Now;
            UpdateDriverValues(ADD_NEW_DRIVER);
            LabelText = Enum.GetName(typeof(OperationType), OperationType.Създаване);
        }

        private void InitOrders()
        {
            if (Driver.Id != null)
            {
                List<Order> orders = orderDao.FindByDriverId(Driver.Id);
                Orders = new ObservableCollection<Order>(orders);
            }
            else
            {
                Orders = new ObservableCollection<Order>();
            }
        }

        private void InitOrder()
        {
            Order = new Order();
            Order.OrderDate = DateTime.Now;
            OrderValidation = new OrderValidation();
            UpdateOrderValues(ADD_NEW_ORDER);
            LabelTextOrder = Enum.GetName(typeof(OperationType), OperationType.Създаване);
        }

        private void UpdateDriverValues(string defaultValue)
        {
            DriverValues = new ObservableCollection<string>(driverDao.GetAll().Select(a => $"{a.Id} - {a.Name}").ToList());
            DriverValues.Insert(0, ADD_NEW_DRIVER);
            SelectedDriverValue = defaultValue;
        }

        private void UpdateOrderValues(string defaultValue)
        {
            OrderValues = new ObservableCollection<string>();
            if (Driver.Id != null)
            {
                foreach (Order order in orderDao.FindByDriverId(Driver.Id))
                {
                    OrderValues.Add($"{order.Id} - {order.Address} - {order.Distance} км. - {order.Car.RegistrationPlate}");
                }
            }
            OrderValues.Insert(0, ADD_NEW_ORDER);
            SelectedOrderValue = defaultValue;
        }

        private void InitCar()
        {
            if (Driver.Id != null)
            {
                CarValues = new ObservableCollection<string>();

                foreach (Car car in carDao.GetAll())
                {
                    CarValues.Add($"{car.Id} - {car.RegistrationPlate} - {car.Brand}");
                }

                if (CarValues.Count > 0)
                {
                    SelectedCarValue = CarValues[0];
                }
                else
                {
                    SelectedCarValue = null;
                }
            }
        }

        public Driver Driver
        {
            get { return driver; }
            set { driver = value; OnPropertyChanged(nameof(Driver)); }
        }

        public DriverValidation DriverValidation
        {
            get { return driverValidation; }
            set { driverValidation = value; OnPropertyChanged(nameof(DriverValidation)); }
        }

        public string SuccessMessage
        {
            get { return successMessage; }
            set { successMessage = value; OnPropertyChanged(nameof(SuccessMessage)); }
        }

        public ObservableCollection<string> DriverValues
        {
            get { return driverValues; }
            set { driverValues = value; OnPropertyChanged(nameof(DriverValues)); }
        }

        public string LabelText
        {
            get { return labelText; }
            set { labelText = value; OnPropertyChanged(nameof(LabelText)); }
        }

        public ObservableCollection<Order> Orders
        {
            get { return orders; }
            set { orders = value; OnPropertyChanged(nameof(Orders)); }
        }

        public Order Order
        {
            get { return order; }
            set { order = value; OnPropertyChanged(nameof(Order)); }
        }

        public OrderValidation OrderValidation
        {
            get { return orderValidation; }
            set { orderValidation = value; OnPropertyChanged(nameof(OrderValidation)); }
        }

        public string SuccessMessageOrder
        {
            get { return successMessageOrder; }
            set { successMessageOrder = value; OnPropertyChanged(nameof(SuccessMessageOrder)); }
        }

        public ObservableCollection<string> OrderValues
        {
            get { return orderValues; }
            set { orderValues = value; OnPropertyChanged(nameof(OrderValues)); }
        }

        public string SelectedOrderValue
        {
            get { return selectedOrderValue; }
            set
            {
                selectedOrderValue = value;
                OnPropertyChanged(nameof(SelectedOrderValue));
                SwapOrder(selectedOrderValue);
            }
        }

        public string LabelTextOrder
        {
            get { return labelTextOrder; }
            set { labelTextOrder = value; OnPropertyChanged(nameof(LabelTextOrder)); }
        }

        public string SelectedDriverValue
        {
            get { return selectedDriverValue; }
            set
            {
                selectedDriverValue = value;
                OnPropertyChanged(nameof(SelectedDriverValue));
                SwapDriver(selectedDriverValue);
                InitOrder();
                InitCar();
                InitOrders();
            }
        }

        public ObservableCollection<string> CarValues
        {
            get { return carValues; }
            set { carValues = value; OnPropertyChanged(nameof(CarValues)); }
        }

        public string SelectedCarValue
        {
            get { return selectedCarValue; }
            set { selectedCarValue = value; OnPropertyChanged(nameof(SelectedCarValue)); }
        }

        public string CarVisible
        {
            get { return carVisible; }
            set { carVisible = value; OnPropertyChanged(nameof(CarVisible)); }
        }

        public string OrderVisible
        {
            get { return orderVisible; }
            set { orderVisible = value; OnPropertyChanged(nameof(OrderVisible)); }
        }

        public bool IsDriverIdReadOnly
        {
            get { return isDriverIdReadOnly; }
            set { isDriverIdReadOnly = value; OnPropertyChanged(nameof(IsDriverIdReadOnly)); }
        }

        private void SwapDriver(string selectedDriverValue)
        {
            SuccessMessage = string.Empty;
            DriverValidation = new DriverValidation();

            if (!selectedDriverValue.Equals(ADD_NEW_DRIVER))
            {
                string driverId = selectedDriverValue.Split('-')[0].Trim();
                Driver = (Driver) driverDao.FindById(driverId).Clone();
                LabelText = Enum.GetName(typeof(OperationType), OperationType.Редактиране);
                OrderVisible = "Visible";
                IsDriverIdReadOnly = true;
            }
            else
            {
                Driver = new Driver();
                Driver.DrivingLicenceValidTo = DateTime.Now;
                LabelText = Enum.GetName(typeof(OperationType), OperationType.Създаване);
                OrderVisible = "Hidden";
                IsDriverIdReadOnly = false;
            }
        }

        private void SwapOrder(string selectedOrderValue)
        {
            SuccessMessageOrder = string.Empty;
            OrderValidation = new OrderValidation();

            if (selectedOrderValue != null && !selectedOrderValue.Equals(ADD_NEW_ORDER))
            {
                long orderId = long.Parse(selectedOrderValue.Split('-')[0].Trim());
                Order = (Order) orderDao.FindById(orderId).Clone();
                LabelTextOrder = Enum.GetName(typeof(OperationType), OperationType.Редактиране);
                CarVisible = "Hidden";
            }
            else
            {
                Order = new Order();
                Order.OrderDate = DateTime.Now;
                LabelTextOrder = Enum.GetName(typeof(OperationType), OperationType.Създаване);
                CarVisible = "Visible";
            }
        }

        private void CreateDriver()
        {
            if (!DriverValidation.ValidateDriver(Driver, selectedDriverValue.Equals(ADD_NEW_DRIVER) ? driverDao : null))
            {
                if (!selectedDriverValue.Equals(ADD_NEW_DRIVER))
                {
                    string oldDriverId = SelectedDriverValue.Split('-')[0].Trim();
                    driverDao.Update(oldDriverId, Driver);
                    UpdateDriverValues($"{Driver.Id} - {Driver.Name}");
                    SuccessMessage = "Успешно редактирахте шофьора!";
                }
                else
                {
                    driverDao.Save(Driver);
                    InitDriver();
                    SuccessMessage = "Успешно създадохте нов шофьор!";
                }
            }
            else
            {
                SuccessMessage = string.Empty;
            }
        }

        private void CreateOrder()
        {
            if (!OrderValidation.ValidateOrder(Order))
            {
                if (!selectedOrderValue.Equals(ADD_NEW_ORDER))
                {
                    orderDao.Update(Order);
                    UpdateOrderValues($"{order.Id} - {order.Address} - {order.Distance} км. - {order.Car.RegistrationPlate}");
                    SuccessMessageOrder = "Успешно редактирахте поръчката!";
                }
                else
                {
                    if (SelectedCarValue == null)
                    {
                        SuccessMessageOrder = "Нямате налични коли!";
                        return;
                    }

                    Order.DriverId = Driver.Id;
                    Order.CarId = int.Parse(SelectedCarValue.Split('-')[0].Trim());
                    orderDao.Save(Order);
                    InitOrder();
                    SuccessMessageOrder = "Успешно създадохте нова поръчка!";
                }

                InitCar();
                InitOrders();
            }
            else
            {
                SuccessMessageOrder = string.Empty;
            }
        }
    }
}
