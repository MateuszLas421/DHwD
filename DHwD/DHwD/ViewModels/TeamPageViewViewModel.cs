﻿using DHwD.Models;
using DHwD.Service;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DHwD.ViewModels
{
    public class TeamPageViewViewModel: ViewModelBase, INavigationAware
    {
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
            ListScrolled = new DelegateCommand(ListScrolledCommand);
            ListviewIsRefreshing = false;
        }
        private async Task Init()
        {
            await Task.Run(async () =>
                                     {
                                         jwt = new JWTToken();
                                         jwt = await _sqliteService.GetToken();
                                         var Member = await _restService.GetMyTeams(jwt, Game.Id);
                                         await foreach (var item in _restService.GetTeams(jwt, Game.Id))
                                         {
                                             if (item.Id == Member.Team.Id)
                                             {
                                                 item.MyTeam = true;
                                                 item.MyteamTEXT = "Attached";
                                                 MyTeamExist = true;
                                                 ObTeam.Add(item);
                                                 continue;
                                             }
                                             item.MyTeam = false;
                                             ObTeam.Add(item);
                                         }
                                     });
        }
        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Games"))
            {
                Game = parameters.GetValue<Games>("Games");
                Title = Game.Name;
            }
            _initializingTask = Init();
            _initializingTask.Wait(1000);
            for(int i = 0;i<ObTeam.Count;i++)
            {
                if(ObTeam[i].MyTeam==true)
                    _navigationService.NavigateAsync("NavigationPage/StartPage", animated: false, useModalNavigation: false);
            }
        }

        #region variables
        private bool _myTeamExist = false;
        private ObservableCollection<Team> _obTeam;
        private Task _initializingTask;
        private JWTToken jwt;
        private INavigationService _navigationService;
        private IPageDialogService _dialogService;
        private SqliteService _sqliteService;
        private RestService _restService;
        private DelegateCommand _btnCreateTeam;
        private bool _isBusy;
        private bool _listviewIsRefreshing;
        #endregion

        #region  Property
        public Games Game { get; set; }
        public DelegateCommand<Team> SelectedCommand { get; }
        public ObservableCollection<Team> ObTeam { get => _obTeam; set => SetProperty(ref _obTeam, value); }
        public DelegateCommand BtnCreateTeam =>
        _btnCreateTeam ?? (_btnCreateTeam = new DelegateCommand(CreateTeamCommand));
        public DelegateCommand ListScrolled { get; }
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        public bool ListviewIsRefreshing
        {
            get => _listviewIsRefreshing;
            set => SetProperty(ref _listviewIsRefreshing, value);
        }
        public bool MyTeamExist { get => _myTeamExist; set => _myTeamExist = value; }
        #endregion

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
            List<TeamMembers> list=new List<TeamMembers>();
            var p = new NavigationParameters
            {
                { "Team", teams },
                { "JWT", jwt }
            };
            if (!teams.MyTeam)  
            {
                if (MyTeamExist == true)
                {
                    await _dialogService.DisplayAlertAsync("Alert", "If you want join other team, you must leave your team.", "OK");
                    IsBusy = false;
                    return;
                }
                await foreach (var item in _restService.GetTeamMembers(jwt, teams.Id))
                {
                    try
                    {
                        list.Add(item);
                    }
                    catch(NullReferenceException ex)
                    {
                        await _dialogService.DisplayAlertAsync("ALERT", ex.Message.ToString(), "OK");
                        IsBusy = false;
                        return;
                    }
                }
                int count;
                try
                {
                    count = list.Count;
                }
                catch (NullReferenceException)
                {
                    await _dialogService.DisplayAlertAsync("ALERT", "You cannot join this team.", "OK");
                    IsBusy = false;
                    return;
                }
                if (count >= 4)
                {
                    await _dialogService.DisplayAlertAsync("ALERT", "You cannot join this team.", "OK");
                    IsBusy = false;
                    return;
                }
                if (teams.OnlyOnePlayer)
                {
                    await _dialogService.DisplayAlertAsync("ALERT", "You cannot join this team.", "OK");
                    IsBusy = false;
                    return;
                }
            }
            await _navigationService.NavigateAsync("JoinToTeamPassword", p, animated: false,useModalNavigation:false);
            IsBusy = false;
        }
        private async void ListScrolledCommand()
        {
            ListviewIsRefreshing = true;
            ObTeam.Clear();
            await Init();
            ListviewIsRefreshing = false;
        }
    }
}

