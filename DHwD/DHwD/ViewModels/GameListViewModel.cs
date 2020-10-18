using DHwD.Models;
using DHwD.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DHwD.ViewModels
{
    public class GameListViewModel : ViewModelBase
    {
        #region constructor 
        public GameListViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _sqliteService = new SqliteService();
            _restService = new RestService();
            _initializingTask = Init();
            ObGames = new ObservableCollection<Games>();
            SelectedCommand = new DelegateCommand<Games>(Selected, _ => !IsBusy).ObservesProperty(() => IsBusy);
        }
        #endregion
        private async Task Init()
        {
            await Task.Run(async () =>
             {
                 jwt = new JWTToken();
                 jwt = await _sqliteService.GetToken();
                 await foreach (var item in _restService.GetGames(jwt))
                 {
                     ObGames.Add(item);
                 }
             });
        }

        #region variables
        private DelegateCommand _command;
        private ObservableCollection<Games> _obGames;
        private readonly Task _initializingTask;
        private JWTToken jwt;
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;
        private SqliteService _sqliteService;
        private RestService _restService;
        #endregion

        #region  Property
        public DelegateCommand<Games> SelectedCommand { get; }
        public ObservableCollection<Games> ObGames { get => _obGames; set => SetProperty(ref _obGames, value); }
        #endregion

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        private async void Selected(Games games)
        {
            IsBusy = true;
            var p = new NavigationParameters
            {
                { "Games", games }
            };

            await _navigationService.NavigateAsync("TeamPageView", p);
            IsBusy = false;
        }
    }
}
