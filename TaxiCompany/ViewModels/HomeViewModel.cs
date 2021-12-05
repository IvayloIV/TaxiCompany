using TaxiCompany.Commands;
using TaxiCompany.Dao;
using TaxiCompany.Stores;
using System.Windows;
using System.Windows.Input;

namespace TaxiCompany.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand NavigationFormInputCommand { get; }
        public ICommand NavigationReportsCommand { get; }
        public ICommand NavigationExitCommand { get; }

        public HomeViewModel(NavigationStore navigationStore)
        {
            NavigationFormInputCommand = new NavigateCommand<FormHomeViewModel>(navigationStore, (n) => new FormHomeViewModel(n));
            NavigationReportsCommand = new NavigateCommand<ReportHomeViewModel>(navigationStore, (n) => new ReportHomeViewModel(n));
            NavigationExitCommand = new RelayCommand(CloseMainWindow);
        }

        private void CloseMainWindow()
        {
            TaxiCompanyContextSingleton.DestroyContext();
            Application.Current.MainWindow.Close();
        }
    }
}
