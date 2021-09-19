using DHwD.Models;
using DHwD.Repository.Interfaces;
using DHwD.Service;
using DHwD.Tools;
using Models.ModelsDB;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DHwD.ViewModels
{
    public class GameListViewModel : ViewModelBase
    {
        #region constructor 
        public GameListViewModel(INavigationService navigationService,
            IStorage storage, IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _storage = storage;
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
                 jwt = new JWTToken { Token = await _storage.ReadData(Constans.JWT) };
                 await foreach (var item in _restService.GetGames(jwt))
                 {
                     ObGames.Add(item);
                 }
             });
        }

        #region variables
        //private DelegateCommand _command;
        private ObservableCollection<Games> _obGames;
        private readonly Task _initializingTask;
        private JWTToken jwt;
        private IStorage _storage;
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;
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
                { "Game", games }
            };
            await _navigationService.NavigateAsync("TeamPageView", p);
            IsBusy = false;
        }
    }
}
