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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DHwD.ViewModels.GameInterface
{
    public class StartPageViewModel : GameBaseViewModel
    {
        public StartPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            IsLoading = true;
            _navigationService = navigationService;
            Location Activelocation = new Location();
            url_Data  = new Url_data();
            _restService = new RestService();
            chat = new ObservableCollection<Chats>();
        }

        #region Navigation
        public override async void Initialize(INavigationParameters parameters)
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
            await Startchat();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
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
                    await StartLocationEventAsync(_Team, Activelocation);
                }
            }
        }

        #endregion

        #region  Property
        public DateTime currenttimeforChat;

        private RestService _restService;

        public Url_data url_Data { get; set; }

        internal Games _game { get; private set; }

        public MobileTeam _Team { get; set; }

        public JWTToken jWT { get; set; }

        public ObservableCollection<Chats> chat;
        public bool isLoading;

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

        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }
        public ObservableCollection<Chats> Chat
        {
            get => chat;
            set => SetProperty(ref chat, value);
        }

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
                    { "Game", _game},
                    { "Chat", chat}
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        /// <param name="activelocation"></param>
        /// <returns></returns>
        private async Task StartLocationEventAsync(MobileTeam team, Location activelocation)
        {
            BlockedPlaceRequest blockedPlaceRequest = new BlockedPlaceRequest{ 
                Id_Place = activelocation.Place.Id,
                Id_Team =  team.Id,
                Id_Location = Activelocation.ID,
                Id_Game = _game.Id
            };
            
            if (await CheckTask(team)!=null)
            {
                await BlockedLocation(blockedPlaceRequest);
            }
            else
            {
                
            }
        }

        private async Task BlockedLocation(BlockedPlaceRequest blockedPlaceRequest)
        {
            try
            {
                List<Chats> list = new List<Chats>();
                var result = await BaseREST.PostExecuteAsync<BlockedPlaceRequest, ActivePlace>(url_Data.BlockedPlace.ToString(), jWT, blockedPlaceRequest);
                if (result != null)
                {
                    if (currenttimeforChat != null)
                    {
                        GetRequest getRequest = new GetRequest(url_Data.ChatUpdate.ToString());
                        getRequest = await PrepareGetRequest.PrepareFirstParametr(getRequest, "Game", _game.Id.ToString());
                        getRequest = await PrepareGetRequest.PrepareMoreParametr(getRequest, "DateTimeCreate", currenttimeforChat.ToString());

                        list = await BaseREST.GetExecuteAsync<List<Chats>>(jWT,getRequest);
                    }
                    else
                    {
                        await Startchat();
                    }
                    if (list.Count!=0 && list != null)
                    {
                        foreach (var item in list)
                        {
                            chat.Add(item);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        internal async Task Startchat()
        {
            await foreach (var item in  _restService.GetChat(jWT, _game.Id))
            {
                if(item.Text!=null)
                    chat.Add(item);
            }
        }
        private async Task<ActivePlace> CheckTask(Team  team)
        {
            GetRequest getRequest = new GetRequest(url_Data.Check.ToString());
            getRequest = await PrepareGetRequest.PrepareFirstParametr(getRequest, "Id_Team", team.Id.ToString());

            var result = await BaseREST.GetExecuteAsync<ActivePlace>(jWT, getRequest);
            return await Task.FromResult(result); 
        }

        #endregion
    }
}