using DHwD.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DHwD.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        public StartPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            _navigationService = navigationService;
        }
        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Team"))
            {
                _Team = parameters.GetValue<Team>("Team");
            }
            if (parameters.ContainsKey("JWT"))
            {
                jWT = parameters.GetValue<JWTToken>("JWT");
            }
        }

        #region  Property
        public Team _Team { get; set; }
        public JWTToken jWT { get; set; }

        private INavigationService _navigationService;

        private DelegateCommand _map, _chat, _settings, _teampage;

        public DelegateCommand ChatCommand =>
            _chat ?? (_chat = new DelegateCommand(ExecuteChatCommand));

        public DelegateCommand SettingsCommand =>
            _settings ?? (_settings = new DelegateCommand(ExecuteSettingsCommand));

        public DelegateCommand MyTeampageCommand =>
            _teampage ?? (_teampage = new DelegateCommand(ExecuteMyTeampageCommand));

        public DelegateCommand MapCommand =>
            _map ?? (_map = new DelegateCommand(ExecuteMapCommand));

        #endregion
        async void ExecuteMapCommand()
        {
            var p = new NavigationParameters
                {
                    { "Team", _Team },
                    { "JWT", jWT }
                };
            await _navigationService.NavigateAsync("MapPage", p, useModalNavigation: true, animated: false);
        }
        async void ExecuteChatCommand()
        {
            var p = new NavigationParameters
                {
                    { "Team", _Team },
                    { "JWT", jWT }
                };
            await _navigationService.NavigateAsync("ChatPage", p, useModalNavigation: true, animated: false);
        }
        async void ExecuteSettingsCommand()
        {
                        var p = new NavigationParameters
                {
                    { "Team", _Team },
                    { "JWT", jWT }
                };
            await _navigationService.NavigateAsync("SettingsPage", p, useModalNavigation: true, animated: true);
        }
        async void ExecuteMyTeampageCommand()
        {
            var p = new NavigationParameters
                {
                    { "Team", _Team },
                    { "JWT", jWT }
                };
            await _navigationService.NavigateAsync("MyTeamPage", p, useModalNavigation: true, animated: false);
        }
    }
}