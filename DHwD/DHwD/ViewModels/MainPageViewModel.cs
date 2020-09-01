using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Plugin.DeviceInfo;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DHwD.Model;
using DHwD.Service;
using Prism.Services;

namespace DHwD.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region constructor 
        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            Title = "Login Page";
            user = new User();
        }
        #endregion

        # region variables
        private DelegateCommand _logincommand;
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;
        #endregion

        #region  Property
        public User user { get; set; }
        public DelegateCommand LoginCommand =>
            _logincommand ?? (_logincommand = new DelegateCommand(ExecuteLoginCommand));
        #endregion

        void ExecuteLoginCommand() //async Task
        {
            Hash hash = new Hash();
            Func<string, string> token = r =>hash.ComputeHash(r, new SHA256CryptoServiceProvider());
            user.Token = token(CrossDeviceInfo.Current.Id.ToString());
            RestService restService = new RestService(_dialogService);
            Task task= Task.Run(async ()=> await restService.RegisterNewUserAsync(user, true)); 
            Title = user.NickName;
            // _navigationService.NavigateAsync("NavigationPage/TeamPageView", useModalNavigation: true);
        }

    }
}
