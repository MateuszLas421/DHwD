using DHwD.Models;
using DHwD.ViewModels.Base;
using Models.ModelsDB;
using Models.ModelsMobile;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace DHwD.ViewModels.GameInterface
{
    public class StartPageViewModel : GameBaseViewModel
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
                _Team = parameters.GetValue<MobileTeam>("Team");
            }
            if (parameters.ContainsKey("JWT"))
            {
                jWT = parameters.GetValue<JWTToken>("JWT");
            }
            if (parameters.ContainsKey("Game"))
            {
                _game = parameters.GetValue<Games>("Game");
            }
        }

        #region  Property

        internal Games _game { get; private set; }

        public MobileTeam _Team { get; set; }

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
                    { "JWT", jWT },
                    { "Game", _game}
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