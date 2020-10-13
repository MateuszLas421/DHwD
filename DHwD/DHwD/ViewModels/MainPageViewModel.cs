using Prism.Commands;
using Prism.Navigation;
using System;
using Xamarin.Forms;
using Plugin.DeviceInfo;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DHwD.Service;
using Prism.Services;
using DHwD.Models;
using System.Diagnostics;

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
            _sqliteService = new SqliteService();
            _restService = new RestService();
            bool a = false;
            Task t = Task.Run(async () =>
            {
                UserRegistration user = await _sqliteService.GetItemAsync();
                if (user != null)
                {
                    JWTToken jwt = new JWTToken();
                    jwt = await _restService.LoginAsync(user);
                    if (jwt != null)
                    {
                        await _sqliteService.SaveToken(jwt);
                        jwt = null;
                        a = true;
                    }
                }
                
            });
            try
            {
                t.Wait(3000);
                if (a)
                {
                   _navigationService.NavigateAsync("NavigationPage/GameListView", useModalNavigation: true, animated: false);                        
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                _dialogService.DisplayAlertAsync("Alert!", "Please enter your nickname", "OK");
            }
        }

        #endregion

        # region variables
        private DelegateCommand _logincommand;
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;
        private Color _nickNameColor;
        private SqliteService _sqliteService;
        private RestService _restService;
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
            JWTToken jWT;
            Hash hash = new Hash();
            Func<string, string> token = r => hash.ComputeHash(r, new SHA256CryptoServiceProvider());
            if (string.IsNullOrEmpty(user.NickName)) { NickNameColor = Color.Red; await _dialogService.DisplayAlertAsync("Alert!", "Please enter your nickname", "OK"); return; }
            user.Token = token(CrossDeviceInfo.Current.Id.ToString());
            jWT = await _restService.LoginAsync(user);
            if (jWT!=null) 
            {
                await _sqliteService.SaveUser(user);
                await _sqliteService.SaveToken(jWT);
                await _navigationService.NavigateAsync("NavigationPage/GameListView", useModalNavigation: true, animated: false);
            }                                                                        
            else 
            {
                bool Register=false;
                Register = await _restService.RegisterNewUserAsync(user);
                if (Register==false) 
                { await _dialogService.DisplayAlertAsync("Alert!", "Ups Something was wrong", "OK"); return; }
                jWT = await _restService.LoginAsync(user);
                if (jWT != null)
                {
                    await _sqliteService.SaveUser(user);
                    await _sqliteService.SaveToken(jWT);
                    await _navigationService.NavigateAsync("NavigationPage/GameListView", useModalNavigation: true, animated: false);
                }
                else
                    await _dialogService.DisplayAlertAsync("Alert!", "Ups Something was wrong", "OK"); return;   
            } 
        }

    }
}
