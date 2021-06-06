using DHwD.Models;
using DHwD.Service;
using DHwD.ViewModels.Base;
using Microsoft.AppCenter.Crashes;
using Models.ModelsDB;
using Models.ModelsMobile;
using Models.Request;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace DHwD.ViewModels.GameInterface
{
    public class StartPageViewModel : GameBaseViewModel, INavigationAware
    {
        public StartPageViewModel(INavigationService navigationService, IDialogService dialog)
            : base(navigationService)
        {
            IsLoading = true;
            _navigationService = navigationService;
            _pageDialog = dialog;
            url_Data = new Url_data();
            _restService = new RestService();
            chat = new ObservableCollection<Chats>();
            Location Activelocation = new Location();
            var timer = new Timer((e) =>
            {
                Time = DateTime.Now.ToString("HH:mm");
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
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
            Init = InitializeTask();
            try
            {
                var oauthToken = await Xamarin.Essentials.SecureStorage.GetAsync("first_start");
                if (oauthToken == null)
                {
                    await Xamarin.Essentials.SecureStorage.SetAsync("first_start", "yes_is_a_first_start");
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        _pageDialog.ShowDialog("GameStartAlertDialog");
                    });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
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
        private async Task InitializeTask()
        {
            await Startchat();
            activePlace = await CheckTask(_Team);
        }

        #endregion

        #region  Property
        public DateTime currenttimeforChat;

        private RestService _restService;

        public string time;
        public string Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }

        public Url_data url_Data { get; set; }

        internal Games _game { get; private set; }

        public MobileTeam _team;
        public MobileTeam _Team
        {
            get => _team;
            set => SetProperty(ref _team, value);
        }

        public ActivePlace activePlace { get; set; }

        public JWTToken jWT { get; set; }

        public ObservableCollection<Chats> chat;
        public bool isLoading;

        private INavigationService _navigationService;

        private IDialogService  _pageDialog;

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
                    { "Chat", chat},
                    { "APlace", activePlace }
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
            BlockedPlaceRequest blockedPlaceRequest = new BlockedPlaceRequest
            {
                Id_Place = activelocation.Place.Id,
                Id_Team = team.Id,
                Id_Location = Activelocation.ID,
                Id_Game = _game.Id
            };
            activePlace = await CheckTask(team);
            if (activePlace.Place == null)
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
                activePlace = await BaseREST.PostExecuteAsync<BlockedPlaceRequest, ActivePlace>(url_Data.BlockedPlace.ToString(), jWT, blockedPlaceRequest);
                if (activePlace.Place != null)
                {
                    if (currenttimeforChat != null)
                    {
                        GetRequest getRequest = new GetRequest(url_Data.ChatUpdate.ToString());
                        getRequest = await PrepareGetRequest.PrepareFirstParametr(getRequest, "Game", _game.Id.ToString());
                        getRequest = await PrepareGetRequest.PrepareMoreParametr(getRequest, "DateTimeCreate", currenttimeforChat.ToString());

                        list = await BaseREST.GetExecuteAsync<List<Chats>>(jWT,getRequest);
                        foreach (var item in list)
                        {
                                chat.Insert(0,item);
                        }
                    }
                    else
                    await Startchat();
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