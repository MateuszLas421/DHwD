﻿using DHwD.Repository.Interfaces;
using DHwD.Service;
using DHwD.Tools;
using Models.ModelsMobile.Common;
using Plugin.DeviceInfo;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DHwD.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region constructor 
        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            ILogs logs, IStorage storage)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _logsRepository = logs;
            _storage = storage;
            Title = "Login Page";
        }
        #region Navigation
        public override void Initialize(INavigationParameters parameters)
        {
            Init = Task.Run(async () => await InitializeTask());
        }
        #endregion

        private async Task InitializeTask()   //  To Check! !!!  // TODO!
        {
            user = new UserRegistration();
            NickNameColor = Color.Default;
            _restService = new RestService();
            bool userExist = false;
            Task sqlTask = Task.Run(async () =>
           {
               //UserRegistration user = await _sqliteService.GetItemAsync();
               user.Token = await _storage.ReadData(Constans.JWT);
               if (user != null)
               {
                   JWTToken jwt = new JWTToken();
                   jwt = await _restService.LoginAsync(user); // Update
                   if (jwt != null)
                   {
                       await _storage.SaveData(Constans.JWT, jwt.Token);
                       jwt = null;
                       userExist = true;
                   }
                   else if (jwt == null)
                   {
                       //await _sqliteService.DeleteUser();
                   }
               }
           });
            try
            {
                while (sqlTask.IsCompleted == false)
                {
                    Thread.Sleep(250);
                }
                if (userExist)
                {
                    var p = new NavigationParameters
                    {

                    };
                    await _navigationService.NavigateAsync("NavigationPage/GameListView", p, useModalNavigation: true, animated: false);
                }
            }
            catch (Exception ex)
            {
                _logsRepository.LogError(ex);

                await _dialogService.DisplayAlertAsync("Alert!", "Please enter your nickname", "OK");
            }
        }

        #endregion

        #region variables
        private DelegateCommand _logincommand;
        private ILogs _logsRepository;
        private IStorage _storage;
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;
        private Color _nickNameColor;
        //private SqliteService _sqliteService;
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
            if (string.IsNullOrEmpty(user.NickName))
            {
                NickNameColor = Color.Red;
                await _dialogService.DisplayAlertAsync("Alert!", "Please enter your nickname", "OK");
                return;
            }
            user.Token = token(CrossDeviceInfo.Current.Id.ToString());
            jWT = await _restService.LoginAsync(user);  // Update
            if (jWT != null)
            {
                //await _sqliteService.SaveUser(user);
                await _storage.SaveData(Constans.JWT, jWT.Token);
                var p = new NavigationParameters
                {
                    { "JWT", jWT }
                };
                await _navigationService.NavigateAsync("NavigationPage/GameListView", p, useModalNavigation: true, animated: true);
            }
            else
            {
                bool Register = false;
                Register = await _restService.RegisterNewUserAsync(user);
                if (Register == false) { await _dialogService.DisplayAlertAsync("Alert!", "Ups Something was wrong", "OK"); return; }
                jWT = await _restService.LoginAsync(user);
                if (jWT != null)
                {
                    //await _sqliteService.SaveUser(user);
                    await _storage.SaveData(Constans.JWT, jWT.Token);
                    var p = new NavigationParameters
                    {
                        { "JWT", jWT }
                    };
                    await _navigationService.NavigateAsync("NavigationPage/GameListView", p, useModalNavigation: true, animated: true);
                }
                else
                    await _dialogService.DisplayAlertAsync("Alert!", "Ups Something was wrong", "OK"); return;
            }
        }

    }
}
