using DHwD.Models;
using DHwD.Service;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace DHwD.ViewModels
{
    public class TeamPageViewViewModel: ViewModelBase, INavigationAware
    {
        HttpClient httpClient;
        public TeamPageViewViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            Title = "Error";
            _navigationService = navigationService;
            _dialogService = dialogService;
            _sqliteService = new SqliteService();
            _restService = new RestService();
            ObTeam = new ObservableCollection<Team>();
            Game = new Games();
            SelectedCommand = new DelegateCommand<Team>(Selected, _ => !IsBusy).ObservesProperty(() => IsBusy);
        }
        private async Task Init()
        {
            await Task.Run(async () =>
            {
                jwt = new JWTToken();
                jwt = await _sqliteService.GetToken();
                await foreach (var item in _restService.GetTeams(jwt,Game.Id))
                {
                    ObTeam.Add(item);
                }
            });
        }
        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Games"))
            {
                Game=parameters.GetValue<Games>("Games");
                Title = Game.Name;
            }
            _initializingTask = Init();
        }

        #region variables
        private Games _game;
        private ObservableCollection<Team> _obTeam;
        private Task _initializingTask;
        private JWTToken jwt;
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;
        private SqliteService _sqliteService;
        private RestService _restService;
        private DelegateCommand _btnCreateTeam;
        #endregion

        #region  Property
        public Games Game { get; set; }
        public DelegateCommand<Team> SelectedCommand { get; }
        public ObservableCollection<Team> ObTeam { get => _obTeam; set => SetProperty(ref _obTeam, value); }
        public DelegateCommand BtnCreateTeam =>
        _btnCreateTeam ?? (_btnCreateTeam = new DelegateCommand(CreateTeamCommand));
        #endregion

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        async void CreateTeamCommand()
        {
            var p = new NavigationParameters
            {
                { "Game", Game }
            };
            await _navigationService.NavigateAsync("CreateNewTeam", p);
        }
        private async void Selected(Team teams)
        {
            IsBusy = true;
            var p = new NavigationParameters
            {
                { "Team", teams }
            };

            await _navigationService.NavigateAsync("NavigationPage/MainTabbedPage", useModalNavigation: true, animated: false);
            IsBusy = false;
        }
    }
}
