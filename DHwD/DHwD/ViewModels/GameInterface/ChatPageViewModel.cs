using DHwD.Models;
using DHwD.Service;
using DHwD.ViewModels.Base;
using Models.ModelsDB;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DHwD.ViewModels.GameInterface
{
    public class ChatPageViewModel : GameBaseViewModel, INotifyPropertyChanged
    {
        public ChatPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            chat = new ObservableCollection<Chats>();
            _restService = new RestService();

            OnSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(TextToSend))
                {
                    chat.Insert(0, new Chats() { Text = TextToSend, IsSystem = false });
                    SolutionRequest message = new SolutionRequest
                    {
                        TextSolution = TextToSend,
                        gameid = _game.Id,
                        //idMystery = _
                    };

                }

            });
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
           // await Startchat();
        }

        #region variables
        //private DelegateCommand _command;
        private INavigationService _navigationService;
        public ObservableCollection<Chats> chat;
        private JWTToken jWT;
        private SqliteService _sqliteService;
        private RestService _restService;
        private SolutionRequest message;
        #endregion

        #region  Property
        public ObservableCollection<Chats> Chat
        {
            get => chat;
            set => SetProperty(ref chat, value);
        }
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
    }
}
