using DHwD.Models;
using DHwD.Service;
using DHwD.ViewModels.Base;
using Microsoft.AppCenter.Crashes;
using Models.ModelsDB;
using Models.ModelsMobile;
using Models.Request;
using Models.Respone;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;

namespace DHwD.ViewModels.GameInterface
{
    public class StartPageViewModel : GameBaseViewModel
    {
        public StartPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Location Activelocation = new Location();
            url_Data  = new Url_data();
        }

        #region Navigation
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode()==NavigationMode.Back)
            {
                if (parameters.ContainsKey("Team"))
                {
                    _Team = parameters.GetValue<MobileTeam>("Team");
                }
                if (parameters.ContainsKey("JWT"))
                {
                    jWT = parameters.GetValue<JWTToken>("JWT");
                }
                if (parameters.ContainsKey("Location"))
                {
                    Activelocation = parameters.GetValue<Location>("Location");
                }
                StartLocationEventAsync(_Team, Activelocation);
            }
        }

        #endregion

        #region  Property
        public Url_data url_Data { get; set; }

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

        public Location Activelocation { get; set; }

        #endregion
        #region Command
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
        #endregion
        #region Operations
        private async Task StartLocationEventAsync(MobileTeam team, Location activelocation)
        {
            BlockedPlaceRequest blockedPlaceRequest = new BlockedPlaceRequest{ 
                Id_Place = activelocation.Place.Id,
                Id_Team =  team.Id,
                Id_Game = _game.Id
            };
            GetRequest getRequest = new GetRequest(url_Data.Check.ToString());
            getRequest = await PrepareGetRequest.PrepareFirstParametr(getRequest, "Id_Team", team.Id.ToString());

            var result = await BaseREST.GetExecuteAsync(getRequest);
            if (result.Succes == true)
            {
                await BlockedLocation(blockedPlaceRequest);
            }
        }

        private async Task BlockedLocation(BlockedPlaceRequest blockedPlaceRequest)
        {
            try
            {
                var result = await BaseREST.PostExecuteAsync<BlockedPlaceRequest, ActivePlace>(url_Data.BlockedPlace.ToString(), blockedPlaceRequest);
                if (result != null)
                {

                }
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}