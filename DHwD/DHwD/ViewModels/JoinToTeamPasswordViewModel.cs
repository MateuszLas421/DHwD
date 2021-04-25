using DHwD.Models;
using DHwD.Service;
using Models.ModelsDB;
using Models.ModelsMobile;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace DHwD.ViewModels
{
    public class JoinToTeamPasswordViewModel : ViewModelBase, INavigationAware
    {
        public JoinToTeamPasswordViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _sqliteService = new SqliteService();
            _restService = new RestService();
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
            var p = new NavigationParameters
                {
                    { "Team", _Team },
                    { "JWT", jWT },
                    { "Game", _game }
                };
            if (_Team.MyTeam == true)
                _navigationService.NavigateAsync("NavigationPage/StartPage", p, animated: false, useModalNavigation: true);
            if (_Team.StatusPassword == false && _Team.MyTeam == false)
            {
                _initializingTask = Init();

                //_initializingTask.RunSynchronously();
                Thread.Sleep(1500);
                //if (_initializingTask.Result == true) //
                _navigationService.NavigateAsync("NavigationPage/StartPage", p, animated: false, useModalNavigation: true);
            }
        }
        private async Task<bool> Init()
        {
                bool result = await _restService.JoinToTeam(jWT, _Team);
                if (!result)
                {
                    await _dialogService.DisplayAlertAsync("ALERT", "You cannot join this team.", "OK");
                    return false;
                }
                return true;
        }

        #region variables
        private Task<bool> _initializingTask;
        private DelegateCommand _logincommand;
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;
        private SqliteService _sqliteService;
        private RestService _restService;
        private string _TeamPassword;
        #endregion

        #region  Property
        internal Games _game { get; private set; }
        public MobileTeam _Team { get; set; }
        public string TeamPassword
        {
            get => _TeamPassword;
            set => SetProperty(ref _TeamPassword, value);
        }
        public JWTToken jWT { get; set; }
        public DelegateCommand LoginCommand =>
            _logincommand ?? (_logincommand = new DelegateCommand(ExecuteLoginCommand));
        #endregion
        async void ExecuteLoginCommand()
        {
            if (string.IsNullOrEmpty(TeamPassword))
            {
                await _dialogService.DisplayAlertAsync("Alert", "You did not enter your password!", "OK");
                return;
            }    
            var p = new NavigationParameters
                {
                    { "Team", _Team },
                    { "JWT", jWT },
                    { "Game", _game }
                };
            Hash hash = new Hash();
            Func<string, string> pass = r => hash.ComputeHash(r, new SHA256CryptoServiceProvider());
            TeamPassword = pass(TeamPassword);
            bool result = await _restService.CheckTeamPass(jWT, _Team.Id, TeamPassword);
            if (result == false)
            {
                await _dialogService.DisplayAlertAsync("Alert", "You did enter incorrect password!", "OK");
                return;
            }
            bool join = await _restService.JoinToTeam(jWT, _Team);
            if (!join)
            {
                await _dialogService.DisplayAlertAsync("ALERT", "You cannot join this team.", "OK");
                return;
            }
            await _navigationService.NavigateAsync("NavigationPage/StartPage", p, animated: false, useModalNavigation: true);
        }
    }
}
