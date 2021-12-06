using TaxiCompany.Commands;
using TaxiCompany.Dao;
using TaxiCompany.Models;
using TaxiCompany.Models.Dto;
using TaxiCompany.Stores;
using Microsoft.WindowsAPICodePack.Dialogs;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace TaxiCompany.ViewModels
{
    public class CarOrderReportViewModel : BaseViewModel
    {
        private const string ALL_BRANDS = "-- Всички --";

        public RelayCommand SearchCommand { get; }
        public RelayCommand ExcelExportCommand { get; }
        public ICommand NavigationBackCommand { get; }

        private CarDao carDao;
        private ObservableCollection<CarOrderDto> carOrdersDto;

        private string registrationPlate;
        private ObservableCollection<string> brands;
        private string selectedBrand;

        public CarOrderReportViewModel(NavigationStore navigationStore)
        {
            SearchCommand = new RelayCommand(Search);
            ExcelExportCommand = new RelayCommand(CreateExcelFile);
            NavigationBackCommand = new NavigateCommand<ReportHomeViewModel>(navigationStore, (n) => new ReportHomeViewModel(n));
            carDao = new CarDao();
            brands = new ObservableCollection<string>(carDao.GetCarBrands());
            brands.Insert(0, ALL_BRANDS);
            SelectedBrand = brands[0];
            Search();
        }

        public ObservableCollection<CarOrderDto> CarOrdersDto
        {
            get { return carOrdersDto; }
            set { carOrdersDto = value; OnPropertyChanged(nameof(CarOrdersDto)); }
        }

        public string RegistrationPlate
        {
            get { return registrationPlate; }
            set { registrationPlate = value; OnPropertyChanged(nameof(RegistrationPlate)); }
        }

        public ObservableCollection<string> Brands
        {
            get { return brands; }
        }

        public string SelectedBrand
        {
            get { return selectedBrand; }
            set { selectedBrand = value; OnPropertyChanged(nameof(SelectedBrand)); }
        }

        private void Search()
        {
            CarOrdersDto = new ObservableCollection<CarOrderDto>(carDao.GetCarsOrders(RegistrationPlate,
                SelectedBrand.Equals(ALL_BRANDS) ? null : SelectedBrand));
        }

        private void CreateExcelFile()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string pathFile = Path.Combine(dialog.FileName, "CarOrdersReport.xlsx");
                if (File.Exists(pathFile))
                {
                    if (MessageBox.Show("Искате ли да презапишите файлът?", "Предупреждение!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                    else
                    {
                        File.Delete(pathFile);
                    }
                }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelPackage = new ExcelPackage(pathFile))
                {
                    ExcelWorksheet ew = excelPackage.Workbook.Worksheets.Add("CarOrdersReport");

                    ew.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ew.Row(1).Style.Font.Size = 12;
                    ew.Row(1).Style.Font.Bold = true;
                    ew.Row(1).Style.Font.Color.SetColor(Color.White);
                    ew.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ew.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(48, 84, 150));
                    ew.Cells["A1"].EntireColumn.Width = 17;
                    ew.Cells["B1:C1"].EntireColumn.Width = 30;
                    ew.Cells["D1"].EntireColumn.Width = 17;
                    ew.Cells["E1"].EntireColumn.Width = 35;

                    ew.Cells["A1"].Value = "Номер на такси";
                    ew.Cells["B1"].Value = "Регистрационен номер";
                    ew.Cells["C1"].Value = "Марка на автомобила";
                    ew.Cells["D1"].Value = "Брой поръчки";
                    ew.Cells["E1"].Value = "Общо изминато разстояние (км.)";

                    if (CarOrdersDto.Count > 0)
                    {
                        ew.Cells[$"E2:E{CarOrdersDto.Count + 1}"].Style.Numberformat.Format = "0.00";
                    }

                    for (int i = 2; i <= CarOrdersDto.Count + 1; i++)
                    {
                        CarOrderDto car = CarOrdersDto[i - 2];
                        ew.Cells[$"A{i}"].Value = car.Id;
                        ew.Cells[$"B{i}"].Value = car.RegistrationPlate;
                        ew.Cells[$"C{i}"].Value = car.Brand;
                        ew.Cells[$"D{i}"].Value = car.OrdersCount;
                        ew.Cells[$"E{i}"].Value = car.TotalDistance;
                    }

                    excelPackage.Save();
                }
            }
        }
    }
}
