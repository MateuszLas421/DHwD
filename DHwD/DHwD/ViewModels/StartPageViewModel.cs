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
        async void ExecuteMapCommand()
        {
            await _navigationService.NavigateAsync("MapPage", useModalNavigation: true, animated: false);
        }
        async void ExecuteChatCommand()
        {
            await _navigationService.NavigateAsync("ChatPage", useModalNavigation: true, animated: false);
        }
        async void ExecuteSettingsCommand()
        {
            await _navigationService.NavigateAsync("SettingsPage", useModalNavigation: true, animated: true);
        }
        async void ExecuteMyTeampageCommand()
        {
            await _navigationService.NavigateAsync("MyTeamPage", useModalNavigation: true, animated: false);
        }
    }
}