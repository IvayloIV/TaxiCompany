using TaxiCompany.Commands;
using TaxiCompany.Stores;
using System.Windows.Input;

namespace TaxiCompany.ViewModels
{
    public class FormHomeViewModel : BaseViewModel
    {
        public ICommand NavigationCarsCommand { get; }
        public ICommand NavigationDriversCommand { get; }
        public ICommand NavigationBackCommand { get; }

        public FormHomeViewModel(NavigationStore navigationStore)
        {
            NavigationCarsCommand = new NavigateCommand<CarFormViewModel>(navigationStore, (n) => new CarFormViewModel(n));
            NavigationDriversCommand = new NavigateCommand<DriverFormViewModel>(navigationStore, (n) => new DriverFormViewModel(n));
            NavigationBackCommand = new NavigateCommand<HomeViewModel>(navigationStore, (n) => new HomeViewModel(n));
        }
    }
}
