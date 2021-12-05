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
    public class OrderByDateReportViewModel : BaseViewModel
    {
        public RelayCommand SearchCommand { get; }
        public RelayCommand ExcelExportCommand { get; }
        public ICommand NavigationBackCommand { get; }

        private OrderDao orderDao;
        private ObservableCollection<OrderByDateDto> ordersByDateDto;

        private string carRegistrationPlate;
        private DateTime orderDate;

        public OrderByDateReportViewModel(NavigationStore navigationStore)
        {
            SearchCommand = new RelayCommand(Search);
            ExcelExportCommand = new RelayCommand(CreateExcelFile);
            NavigationBackCommand = new NavigateCommand<ReportHomeViewModel>(navigationStore, (n) => new ReportHomeViewModel(n));
            orderDao = new OrderDao();
            OrderDate = DateTime.Now;
            Search();
        }

        public ObservableCollection<OrderByDateDto> OrdersByDateDto
        {
            get { return ordersByDateDto; }
            set { ordersByDateDto = value; OnPropertyChanged(nameof(OrdersByDateDto)); }
        }

        public string CarRegistrationPlate
        {
            get { return carRegistrationPlate; }
            set { carRegistrationPlate = value; OnPropertyChanged(nameof(CarRegistrationPlate)); }
        }

        public DateTime OrderDate
        {
            get { return orderDate; }
            set { orderDate = value; OnPropertyChanged(nameof(OrderDate)); }
        }

        private void Search()
        {
            OrdersByDateDto = new ObservableCollection<OrderByDateDto>(orderDao.OrdersByDateAndRegistrationPlate(orderDate, carRegistrationPlate));
        }

        private void CreateExcelFile()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string pathFile = Path.Combine(dialog.FileName, "OrderByDateReport.xlsx");
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
                    ExcelWorksheet ew = excelPackage.Workbook.Worksheets.Add("OrderByDateReport");

                    ew.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ew.Row(1).Style.Font.Size = 12;
                    ew.Row(1).Style.Font.Bold = true;
                    ew.Row(1).Style.Font.Color.SetColor(Color.White);
                    ew.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ew.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(48, 84, 150));
                    ew.Cells["A1"].EntireColumn.Width = 9;
                    ew.Cells["B1:E1"].EntireColumn.Width = 27;

                    ew.Cells["A1"].Value = "Номер";
                    ew.Cells["B1"].Value = "Време на поръчката";
                    ew.Cells["C1"].Value = "Изминато разстояние(км.)";
                    ew.Cells["D1"].Value = "Регистрационен номер";
                    ew.Cells["E1"].Value = "Марка на автомобила";

                    if (ordersByDateDto.Count > 0)
                    {
                        ew.Cells[$"C2:C{ordersByDateDto.Count + 1}"].Style.Numberformat.Format = "0.00";
                    }

                    for (int i = 2; i <= ordersByDateDto.Count + 1; i++)
                    {
                        OrderByDateDto order = ordersByDateDto[i - 2];
                        ew.Cells[$"A{i}"].Value = order.Id;
                        ew.Cells[$"B{i}"].Value = $"{order.OrderDate:g}";
                        ew.Cells[$"C{i}"].Value = order.Distance;
                        ew.Cells[$"D{i}"].Value = order.RegistrationPlate;
                        ew.Cells[$"E{i}"].Value = order.Brand;
                    }

                    excelPackage.Save();
                }
            }
        }
    }
}
