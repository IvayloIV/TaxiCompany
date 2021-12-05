using TaxiCompany.Stores;
using TaxiCompany.ViewModels;
using System;
using System.Windows.Input;

namespace TaxiCompany.Commands
{
    public class NavigateCommand<TViewModel> : ICommand
        where TViewModel : BaseViewModel
    {
        private readonly Func<NavigationStore, TViewModel> viewModel;
        private readonly NavigationStore navigationStore;
        public event EventHandler CanExecuteChanged;

        public NavigateCommand(NavigationStore navigationStore, Func<NavigationStore, TViewModel> viewModel)
        {
            this.navigationStore = navigationStore;
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            navigationStore.CurrentViewModel = viewModel(navigationStore);
        }
    }
}
