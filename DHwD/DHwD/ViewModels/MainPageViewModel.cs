using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DHwD.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Main Page";
        }
        private DelegateCommand _logincommand;
        private INavigationService _navigationService;

        public DelegateCommand LoginCommand =>
            _logincommand ?? (_logincommand = new DelegateCommand(ExecuteLoginCommand));

        void ExecuteLoginCommand()
        {
            _navigationService.NavigateAsync("NavigationPage/TeamPageView", useModalNavigation: true);
        }
    }
}
