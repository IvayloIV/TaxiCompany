using TaxiCompany.Commands;
using TaxiCompany.Dao;
using TaxiCompany.Models.Dto;
using TaxiCompany.Stores;
using Microsoft.WindowsAPICodePack.Dialogs;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace TaxiCompany.ViewModels
{
    public class DriverOrderReportViewModel : BaseViewModel
    {
        public RelayCommand SearchCommand { get; }
        public RelayCommand ExcelExportCommand { get; }
        public ICommand NavigationBackCommand { get; }

        private DriverDao driverDao;
        private ObservableCollection<DriverOrderDto> driversOrderDto;

        private string ordersCount;
        private string totalDistance;

        public DriverOrderReportViewModel(NavigationStore navigationStore)
        {
            SearchCommand = new RelayCommand(Search);
            ExcelExportCommand = new RelayCommand(CreateExcelFile);
            NavigationBackCommand = new NavigateCommand<ReportHomeViewModel>(navigationStore, (n) => new ReportHomeViewModel(n));
            driverDao = new DriverDao();
            Search();
        }

        public ObservableCollection<DriverOrderDto> DriversOrderDto
        {
            get { return driversOrderDto; }
            set { driversOrderDto = value; OnPropertyChanged(nameof(DriversOrderDto)); }
        }

        public string OrdersCount
        {
            get { return ordersCount; }
            set { ordersCount = value; OnPropertyChanged(nameof(OrdersCount)); }
        }

        public string TotalDistance
        {
            get { return totalDistance; }
            set { totalDistance = value; OnPropertyChanged(nameof(TotalDistance)); }
        }

        private void Search()
        {
            DriversOrderDto = new ObservableCollection<DriverOrderDto>(driverDao.GetDriversOrders(ordersCount, totalDistance));
        }

        private void CreateExcelFile()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string pathFile = Path.Combine(dialog.FileName, "DriverOrdersReport.xlsx");
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
                    ExcelWorksheet ew = excelPackage.Workbook.Worksheets.Add("DriverOrdersReport");

                    ew.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ew.Row(1).Style.Font.Size = 12;
                    ew.Row(1).Style.Font.Bold = true;
                    ew.Row(1).Style.Font.Color.SetColor(Color.White);
                    ew.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ew.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(48, 84, 150));
                    ew.Cells["A1"].EntireColumn.Width = 15;
                    ew.Cells["B1:E1"].EntireColumn.Width = 35;

                    ew.Cells["A1"].Value = "ЕГН";
                    ew.Cells["B1"].Value = "Име на шофьора";
                    ew.Cells["C1"].Value = "Професионален опит (години)";
                    ew.Cells["D1"].Value = "Брой поръчки";
                    ew.Cells["E1"].Value = "Общо изминато разстояние (км.)";

                    if (DriversOrderDto.Count > 0)
                    {
                        ew.Cells[$"E2:E{DriversOrderDto.Count + 1}"].Style.Numberformat.Format = "0.00";
                        ExcelIgnoredError eie = ew.IgnoredErrors.Add(ew.Cells[$"A2:A{DriversOrderDto.Count + 1}"]);
                        eie.NumberStoredAsText = true;
                    }

                    for (int i = 2; i <= DriversOrderDto.Count + 1; i++)
                    {
                        DriverOrderDto driver = DriversOrderDto[i - 2];
                        ew.Cells[$"A{i}"].Value = $"{driver.Id}";
                        ew.Cells[$"B{i}"].Value = driver.Name;
                        ew.Cells[$"C{i}"].Value = driver.WorkExperience;
                        ew.Cells[$"D{i}"].Value = driver.OrdersCount;
                        ew.Cells[$"E{i}"].Value = driver.TotalDistance;
                    }

                    excelPackage.Save();
                }
            }
        }
    }
}
