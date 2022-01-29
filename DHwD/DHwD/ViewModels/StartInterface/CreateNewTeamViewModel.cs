using DHwD.Repository.Interfaces;
using DHwD.Service;
using DHwD.Tools;
using Models.ModelsDB;
using Models.ModelsMobile.Common;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace DHwD.ViewModels
{
    public class CreateNewTeamViewModel : ViewModelBase, INavigationAware
    {
        public CreateNewTeamViewModel(INavigationService navigationService,
            IStorage storage, IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _storage = storage;
            _restService = new RestService();
            Team = new Team();
            Game = new Games();
        }
        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Game"))
                Game = parameters.GetValue<Games>("Game");
            ChboX = false;
        }
        #region variables
        private Team _team;
        private bool _chboX;
        private Games _game;
        private Task _initializingTask;
        private JWTToken jwt;
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;
        private IStorage _storage;
        private RestService _restService;
        private DelegateCommand _btnCreateTeam;
        #endregion

        #region  Property
        public bool ChboX
        {
            get => _chboX;
            set => SetProperty(ref _chboX, value);
        }
        public Games Game
        {
            get => _game;
            set => SetProperty(ref _game, value);
        }
        public Team Team
        {
            get => _team;
            set => SetProperty(ref _team, value);
        }
        public DelegateCommand BtnCreateTeam =>
            _btnCreateTeam ?? (_btnCreateTeam = new DelegateCommand(CreateTeamCommand));
        #endregion

        async void CreateTeamCommand()
        {
            jwt = new JWTToken { Token = await _storage.ReadData(Constans.JWT) };
            Team.StatusPassword = ChboX;
            Team.Games = new Games();
            Team.Games.Id = Game.Id;
            if (Team.StatusPassword == false)
            {
                Team.Password = null;
            }
            else
            {
                Hash hash = new Hash();
                Func<string, string> token = r => hash.ComputeHash(r, new SHA256CryptoServiceProvider());
                Team.Password = token(Team.Password);
            }
            bool result = await _restService.CreateNewTeam(jwt, Team);
            Thread.Sleep(100);
            if (result == false)
            {
                await _dialogService.DisplayAlertAsync("Error", "The application can't create a team.", "OK");
            }
            Team.Name = null;
            Team.Password = null;
            Team.Description = null;
            await _navigationService.GoBackAsync();
        }
    }
}
