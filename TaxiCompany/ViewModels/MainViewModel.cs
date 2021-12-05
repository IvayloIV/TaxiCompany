using TaxiCompany.ViewModels;

namespace TaxiCompany.Stores
{
    public class MainViewModel : BaseViewModel
    {
        private NavigationStore navigationStore;

        public BaseViewModel CurrentViewModel
        {
            get { return navigationStore.CurrentViewModel; }
        }

        public MainViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
