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
    public class CarByReviewReportViewModel : BaseViewModel
    {
        public RelayCommand SearchCommand { get; }
        public RelayCommand ExcelExportCommand { get; }
        public ICommand NavigationBackCommand { get; }

        private CarDao carDao;
        private ObservableCollection<CarByReviewDto> carsByReviewDto;

        private bool technicalReview;

        public CarByReviewReportViewModel(NavigationStore navigationStore)
        {
            SearchCommand = new RelayCommand(Search);
            ExcelExportCommand = new RelayCommand(CreateExcelFile);
            NavigationBackCommand = new NavigateCommand<ReportHomeViewModel>(navigationStore, (n) => new ReportHomeViewModel(n));
            carDao = new CarDao();
            Search();
        }

        public ObservableCollection<CarByReviewDto> CarsByReviewDto
        {
            get { return carsByReviewDto; }
            set { carsByReviewDto = value; OnPropertyChanged(nameof(CarsByReviewDto)); }
        }

        public bool TechnicalReview
        {
            get { return technicalReview; }
            set { technicalReview = value; OnPropertyChanged(nameof(TechnicalReview)); }
        }

        private void Search()
        {
            CarsByReviewDto = new ObservableCollection<CarByReviewDto>(carDao.GetCarsByReview(TechnicalReview));
        }

        private void CreateExcelFile()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string pathFile = Path.Combine(dialog.FileName, "CarsByReviewReport.xlsx");
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
                    ExcelWorksheet ew = excelPackage.Workbook.Worksheets.Add("CarsByReviewReport");

                    ew.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ew.Row(1).Style.Font.Size = 12;
                    ew.Row(1).Style.Font.Bold = true;
                    ew.Row(1).Style.Font.Color.SetColor(Color.White);
                    ew.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ew.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(48, 84, 150));
                    ew.Cells["A1"].EntireColumn.Width = 17;
                    ew.Cells["B1:C1"].EntireColumn.Width = 30;
                    ew.Cells["D1"].EntireColumn.Width = 24;
                    ew.Cells["E1"].EntireColumn.Width = 35;

                    ew.Cells["A1"].Value = "Номер на такси";
                    ew.Cells["B1"].Value = "Регистрационен номер";
                    ew.Cells["C1"].Value = "Марка на автомобила";
                    ew.Cells["D1"].Value = "Технически преглед";
                    ew.Cells["E1"].Value = "Общо изминато разстояние (км.)";

                    if (CarsByReviewDto.Count > 0)
                    {
                        ew.Cells[$"E2:E{CarsByReviewDto.Count + 1}"].Style.Numberformat.Format = "0.00";
                    }

                    for (int i = 2; i <= CarsByReviewDto.Count + 1; i++)
                    {
                        CarByReviewDto car = CarsByReviewDto[i - 2];
                        ew.Cells[$"A{i}"].Value = car.Id;
                        ew.Cells[$"B{i}"].Value = car.RegistrationPlate;
                        ew.Cells[$"C{i}"].Value = car.Brand;
                        ew.Cells[$"D{i}"].Value = car.TechnicalReview ? "Да" : "Не";
                        ew.Cells[$"E{i}"].Value = car.TotalDistance;
                    }

                    excelPackage.Save();
                }
            }
        }
    }
}
