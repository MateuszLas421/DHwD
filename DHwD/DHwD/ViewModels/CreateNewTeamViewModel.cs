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
    public class CreateNewTeamViewModel : ViewModelBase
    {
        public CreateNewTeamViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            Game = new Games();
        }
        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Games"))
            {
                Game = parameters.GetValue<Games>("Games");
            }
            ChboX = false;
        }
        #region variables
        private bool _chboX;
        private Task _initializingTask;
        private JWTToken jwt;
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;
        private SqliteService _sqliteService;
        private RestService _restService;
        #endregion

        #region  Property
        public bool ChboX
        {
            get => _chboX;
            set => SetProperty(ref _chboX, value);
        }
        public Games Game { get; set; }
        #endregion
    }
}
