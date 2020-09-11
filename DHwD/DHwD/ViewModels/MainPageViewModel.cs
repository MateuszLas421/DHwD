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
using System.Threading;

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
            user = new UserRegistration();
            NickNameColor = Color.Default;
        }
        #endregion

        # region variables
        private DelegateCommand _logincommand;
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;
        private Color _nickNameColor;
        #endregion

        #region  Property

        public Color NickNameColor
        {
            get => _nickNameColor;
            set => SetProperty(ref _nickNameColor, value);
        }
        public UserRegistration user { get; set; }
        public DelegateCommand LoginCommand =>
            _logincommand ?? (_logincommand = new DelegateCommand(ExecuteLoginCommand));
        #endregion

        async void ExecuteLoginCommand()
        {
            bool CheckUserExists;
            Hash hash = new Hash();
            Func<string, string> token = r => hash.ComputeHash(r, new SHA256CryptoServiceProvider());
            if (string.IsNullOrEmpty(user.NickName)) { NickNameColor = Color.Red; await _dialogService.DisplayAlertAsync("Alert!", "Please enter your nickname", "OK"); return; }
            user.Token = token(CrossDeviceInfo.Current.Id.ToString());
            RestService restService = new RestService(_dialogService);
            CheckUserExists = await restService.CheckUserExistsAsync(user);
            Task responseTask = Task.Run(() =>
            {
                bool a = true; 
                while (a)
                {
                    try { { if (CheckUserExists == true || CheckUserExists == false) { a = false; return; } else Thread.Sleep(300); } }
                    catch (Exception)
                    {
                        Thread.Sleep(300);
                    }
                }
            });
            
            if (CheckUserExists) { return; }                                                                        
            else 
            {
                bool Register;
                Register= await restService.RegisterNewUserAsync(user);
                if (Register)
                    await _navigationService.NavigateAsync("NavigationPage/TeamPageView", useModalNavigation: true);
            } 
        }

    }
}
