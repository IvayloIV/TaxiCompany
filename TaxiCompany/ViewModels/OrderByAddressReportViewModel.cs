using TaxiCompany.Commands;
using TaxiCompany.Dao;
using TaxiCompany.Models.Dto;
using TaxiCompany.Stores;
using Microsoft.WindowsAPICodePack.Dialogs;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace TaxiCompany.ViewModels
{
    public class OrderByAddressReportViewModel : BaseViewModel
    {
        public RelayCommand SearchCommand { get; }
        public RelayCommand ExcelExportCommand { get; }
        public ICommand NavigationBackCommand { get; }

        private OrderDao orderDao;
        private ObservableCollection<OrderByAddressDto> ordersByAddressDto;

        private string address;

        public OrderByAddressReportViewModel(NavigationStore navigationStore)
        {
            SearchCommand = new RelayCommand(Search);
            ExcelExportCommand = new RelayCommand(CreateExcelFile);
            NavigationBackCommand = new NavigateCommand<ReportHomeViewModel>(navigationStore, (n) => new ReportHomeViewModel(n));
            orderDao = new OrderDao();
            Address = "Филип";
            Search();
        }

        public ObservableCollection<OrderByAddressDto> OrdersByAddressDto
        {
            get { return ordersByAddressDto; }
            set { ordersByAddressDto = value; OnPropertyChanged(nameof(OrdersByAddressDto)); }
        }

        public string Address
        {
            get { return address; }
            set { address = value; OnPropertyChanged(nameof(Address)); }
        }

        private void Search()
        {
            OrdersByAddressDto = new ObservableCollection<OrderByAddressDto>(orderDao.OrdersByAddress(address));
        }

        private void CreateExcelFile()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string pathFile = Path.Combine(dialog.FileName, "OrdersByAddressReport.xlsx");
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
                    ExcelWorksheet ew = excelPackage.Workbook.Worksheets.Add("OrdersByAddressReport");

                    ew.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ew.Row(1).Style.Font.Size = 12;
                    ew.Row(1).Style.Font.Bold = true;
                    ew.Row(1).Style.Font.Color.SetColor(Color.White);
                    ew.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ew.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(48, 84, 150));
                    ew.Cells["A1:E1"].EntireColumn.Width = 27;

                    ew.Cells["A1"].Value = "Адрес на поръчката";
                    ew.Cells["B1"].Value = "Време";
                    ew.Cells["C1"].Value = "Разстояние(км.)";
                    ew.Cells["D1"].Value = "Име на шофьор";
                    ew.Cells["E1"].Value = "Марка на автомобил";

                    if (ordersByAddressDto.Count > 0)
                    {
                        ew.Cells[$"C2:C{ordersByAddressDto.Count + 1}"].Style.Numberformat.Format = "0.00";
                    }

                    for (int i = 2; i <= ordersByAddressDto.Count + 1; i++)
                    {
                        OrderByAddressDto order = ordersByAddressDto[i - 2];
                        ew.Cells[$"A{i}"].Value = order.Address;
                        ew.Cells[$"B{i}"].Value = $"{order.OrderDate:g}";
                        ew.Cells[$"C{i}"].Value = order.Distance;
                        ew.Cells[$"D{i}"].Value = order.DriverName;
                        ew.Cells[$"E{i}"].Value = order.CarBrand;
                    }

                    excelPackage.Save();
                }
            }
        }
    }
}
