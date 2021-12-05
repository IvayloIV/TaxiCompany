using TaxiCompany.ViewModels;
using System;

namespace TaxiCompany.Stores
{
    public class NavigationStore
    {
        public event Action CurrentViewModelChanged;

        private BaseViewModel currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get { return currentViewModel; }
            set { currentViewModel = value; OnCurrentViewModelChanged(); }
        }

        private void OnCurrentViewModelChanged()
        {
            if (CurrentViewModelChanged != null)
            {
                CurrentViewModelChanged.Invoke();
            }
        }
    }
}
