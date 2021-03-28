using DHwD.Models;
using DHwD.Models.REST;
using DHwD.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DHwD.ViewModels.GameInterface
{
    public class ChatPageViewModel : ViewModelBase
    {
        public ChatPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            chat = new ObservableCollection<Chats>();
            _restService = new RestService();
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
            if (parameters.ContainsKey("Game"))
            {
                _game = parameters.GetValue<Games>("Game");
            }
        }

        #region variables
        //private DelegateCommand _command;
        private INavigationService _navigationService;
        private ObservableCollection<Chats> chat;
        private JWTToken jWT;
        private SqliteService _sqliteService;
        private RestService _restService;
        #endregion

        #region  Property
        internal Team _Team { get; private set; }
        internal Games _game { get; private set; }
        #endregion

        internal async Task Startchat()
        {
            await foreach (var item in _restService.GetChat(jWT, _game.Id))
            {
                chat.Add(item);
            }
        }
    }
}
