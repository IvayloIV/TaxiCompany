using TaxiCompany.Commands;
using TaxiCompany.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaxiCompany.ViewModels
{
    public class ReportHomeViewModel : BaseViewModel
    {
        public ICommand NavigationOrderByDateCommand { get; }
        public ICommand NavigationDriverOrdersCommand { get; }
        public ICommand NavigationOrderByAddressCommand { get; }
        public ICommand NavigationCarOrdersCommand { get; }
        public ICommand NavigationCarsByReviewCommand { get; }
        public ICommand NavigationBackCommand { get; }

        public ReportHomeViewModel(NavigationStore navigationStore)
        {
            NavigationOrderByDateCommand = new NavigateCommand<OrderByDateReportViewModel>(navigationStore, (n) => new OrderByDateReportViewModel(n));
            NavigationDriverOrdersCommand = new NavigateCommand<DriverOrderReportViewModel>(navigationStore, (n) => new DriverOrderReportViewModel(n));
            NavigationOrderByAddressCommand = new NavigateCommand<OrderByAddressReportViewModel>(navigationStore, (n) => new OrderByAddressReportViewModel(n));
            NavigationCarOrdersCommand = new NavigateCommand<CarOrderReportViewModel>(navigationStore, (n) => new CarOrderReportViewModel(n));
            NavigationCarsByReviewCommand = new NavigateCommand<CarByReviewReportViewModel>(navigationStore, (n) => new CarByReviewReportViewModel(n));
            NavigationBackCommand = new NavigateCommand<HomeViewModel>(navigationStore, (n) => new HomeViewModel(n));
        }
    }
}
