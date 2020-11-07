using DHwD.Models;
using DHwD.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DHwD.ViewModels
{
    public class JoinToTeamPasswordViewModel : ViewModelBase,INavigationAware
    {
        public JoinToTeamPasswordViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            
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
            var p = new NavigationParameters
                {
                    { "Team", _Team },
                    { "JWT", jwt }
                };
            if (_Team.MyTeam == true)
                _navigationService.NavigateAsync("NavigationPage/MainTabbedPage", p, animated: false, useModalNavigation: true);
            if (_Team.StatusPassword == false) //false
                _navigationService.NavigateAsync("NavigationPage/MainTabbedPage", p, animated: false, useModalNavigation: true);
            _initializingTask = Init();
        }
        private async Task Init()   //TODO PASSWORD Check
        {
            await Task.Run(async () =>
            {

            });
        }

        #region variables
        private Task _initializingTask;
        private JWTToken jwt;
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;
        private SqliteService _sqliteService;
        private RestService _restService;
        #endregion
        #region  Property
        public Team _Team { get; set; }
        public JWTToken jWT { get; set; }
        #endregion

    }
}
