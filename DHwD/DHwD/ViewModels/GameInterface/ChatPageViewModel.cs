using DHwD.Models;
using DHwD.Service;
using DHwD.ViewModels.Base;
using Microsoft.AppCenter.Crashes;
using Models.ModelsDB;
using Models.ModelsMobile;
using Models.Request;
using Models.Respone;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DHwD.ViewModels.GameInterface
{
    public class ChatPageViewModel : GameBaseViewModel, INotifyPropertyChanged
    {
        public ChatPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            chat = new ObservableCollection<Chats>();
            _restService = new RestService();
            url_Data = new Url_data();
            OnSendCommand = new Command(() => TaskAsync());
           // OnSendCommand = new Command(async () => { chat.Insert(0, new Chats() { Text = TextToSend, IsSystem = false }); });          ///Debug chat
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Team"))
            {
                _Team = parameters.GetValue<Team>("Team");
            }
            if (parameters.ContainsKey("JWT"))
            {
                jWT = parameters.GetValue<JWTToken>("JWT");
            }
            if (parameters.ContainsKey("Game"))
            {
                _game = parameters.GetValue<Games>("Game");
            }
            if (parameters.ContainsKey("Chat"))
            {
                Chat = parameters.GetValue<ObservableCollection<Chats>>("Chat");
            }
            if (parameters.ContainsKey("APlace"))
            {
                activePlace = parameters.GetValue<ActivePlace>("APlace");
            }
        }

        #region variables
        //private DelegateCommand _command;
        private INavigationService _navigationService;
        public ObservableCollection<Chats> chat;
        private JWTToken jWT;
        Url_data url_Data;
        private SqliteService _sqliteService;
        private RestService _restService;
        private SolutionRequest message;
        private IPageDialogService _dialogService;
        List<Chats> toaddedchats;
        #endregion

        #region  Property
        public ObservableCollection<Chats> Chat
        {
            get => chat;
            set => SetProperty(ref chat, value);
        }
        public ActivePlace activePlace { get; set; }
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }
        public ICommand MessageAppearingCommand { get; set; }
        public ICommand MessageDisappearingCommand { get; set; }
        public bool ShowScrollTap { get; set; } = false;
        public bool LastMessageVisible { get; set; } = true;
        public int PendingMessageCount { get; set; } = 0;
        public bool PendingMessageCountVisible { get { return PendingMessageCount > 0; } }

        public Queue<Chats> DelayedMessages { get; set; } = new Queue<Chats>();

        internal Team _Team { get; private set; }
        internal Games _game { get; private set; }
        #endregion
         
        void OnMessageAppearing(Chats message)
        {
            var idx = chat.IndexOf(message);
            if (idx <= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    while (DelayedMessages.Count > 0)
                    {
                        chat.Insert(0, DelayedMessages.Dequeue());
                    }
                    ShowScrollTap = false;
                    LastMessageVisible = true;
                    PendingMessageCount = 0;
                });
            }
        }

        void OnMessageDisappearing(Chats message)
        {
            var idx = chat.IndexOf(message);
            if (idx >= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ShowScrollTap = true;
                    LastMessageVisible = false;
                });

            }
        }

        #region Operation

        private async void TaskAsync()
        {
            if (string.IsNullOrEmpty(TextToSend))
            {
                return;
            }

            if (activePlace.Place == null)
            {
                Message message = new Message
                {
                    Text = TextToSend,
                    gameid = _game.Id
                };
                var result = await BaseREST.PostExecuteAsync<Message, BaseRespone>(url_Data.Chat.ToString(), jWT, message);
                if (result.Succes == false) 
                {
                    try
                    {
                        await _dialogService.DisplayAlertAsync("Ups!", "Failed to send message.", "OK");
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
                if (result.Succes == true)
                {
                    await Updatechat();
                    int i = 0;
                    var startTimeSpan = TimeSpan.Zero;
                    var periodTimeSpan = TimeSpan.FromMilliseconds(400);
                    Timer timer;
                    timer = new Timer(async (e) =>
                    {
                        if (toaddedchats.Count > i)
                            AddChat(toaddedchats[i]);
                        i++;
                    }, TimeSpan.FromSeconds(15), startTimeSpan, periodTimeSpan);
                }
            }
            else
            {
                if (activePlace.IsCompleted)   // TODO 
                {
                    if (activePlace.IsCompleted == true)
                        activePlace = null;
                    //return ;
                }

                if (activePlace.TypePlace==2 &&  !String.IsNullOrEmpty(activePlace.QuizStatus) && Int32.Parse(activePlace.QuizStatus) > 0)    // TypePlace==2 Quiz
                {
                    await QuizRequest();
                    GetRequest getRequest = new GetRequest(url_Data.ActivePlace.ToString());
                    getRequest = await PrepareGetRequest.AddOnlyValue(getRequest, activePlace.ID.ToString());
                    activePlace = await BaseREST.GetExecuteAsync<ActivePlace>(jWT, getRequest);
                    await Updatechat();
                    await UpdateMessage();
                    return;
                }
                SolutionRequest message = new SolutionRequest
                {
                    TextSolution = TextToSend,
                    Id_Game = _game.Id,
                    Id_Place = activePlace.Place.Id,
                    Id_Team = _Team.Id,
                    IdMystery = activePlace.Place.Location.MysteryRef
                };
                var result = await BaseREST.PostExecuteAsync<SolutionRequest, BaseRespone>(url_Data.Solutions.ToString(), jWT, message);
                if (result.Succes == false)
                {
                    try
                    {
                        await _dialogService.DisplayAlertAsync("Ups!", "Failed to send message.", "OK");
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
                if (result.Message == "Chat")
                {
                    GetRequest getRequest = new GetRequest(url_Data.Quiz.ToString());
                    getRequest = await PrepareGetRequest.PrepareFirstParametr(getRequest, "Id_place", activePlace.Place.Id.ToString());
                    getRequest = await PrepareGetRequest.PrepareMoreParametr(getRequest, "Id_team", _Team.Id.ToString());
                    await BaseREST.GetExecuteAsync<Quiz>(jWT, getRequest);

                    getRequest = new GetRequest(url_Data.ActivePlace.ToString());
                    getRequest = await PrepareGetRequest.AddOnlyValue(getRequest, activePlace.ID.ToString());
                    activePlace = await BaseREST.GetExecuteAsync<ActivePlace>(jWT ,getRequest);
                    if (activePlace.IsCompleted == true)
                        activePlace = null;
                }    
                await Updatechat();
                int i = 0;
                var startTimeSpan = TimeSpan.Zero;
                var periodTimeSpan = TimeSpan.FromMilliseconds(400);
                Timer timer;
                timer = new Timer(async (e) => 
                {
                    if (toaddedchats.Count > i)
                        AddChat(toaddedchats[i]);
                    i++;
                }, TimeSpan.FromSeconds(15), startTimeSpan, periodTimeSpan);
            }
        }

        private async Task UpdateMessage()          // TO Check
        {
            int i = 0;
            var timer = new Timer(async (e) =>
            {
                if (toaddedchats.Count > i)
                    AddChat(toaddedchats[i]);
                i++;
            }, TimeSpan.FromSeconds(15), TimeSpan.Zero, TimeSpan.FromMilliseconds(400));
        }

        private async Task QuizRequest()
        {
            QuizSolution quizSolution = new QuizSolution
            {
                Id_Place = activePlace.Place.Id,
                TextSolution = TextToSend,
                Id_Team = _Team.Id
            };
            var result = await BaseREST.PostExecuteAsync<QuizSolution ,BaseRespone>(url_Data.Quiz.ToString(), jWT, quizSolution);
        }

        internal async Task Updatechat()
        {
            GetRequest getRequest = new GetRequest(url_Data.ChatUpdate.ToString());
            getRequest = await PrepareGetRequest.PrepareFirstParametr(getRequest, "Game", _game.Id.ToString());
            getRequest = await PrepareGetRequest.PrepareMoreParametr(getRequest, "DateTimeCreate", chat[0].DateTimeCreate.ToString());
            toaddedchats = await BaseREST.GetExecuteAsync<List<Chats>>(jWT, getRequest);

        }
        private void AddChat(Chats item)
        {
            chat.Insert(0, item);
            
        }
        #endregion
    }
}
