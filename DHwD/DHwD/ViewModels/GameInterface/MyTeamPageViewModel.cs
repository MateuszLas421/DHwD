﻿using DHwD.Models;
using DHwD.Models.REST;
using DHwD.ViewModels.Base;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DHwD.ViewModels.GameInterface
{
    public class MyTeamPageViewModel : GameBaseViewModel
    {
        public MyTeamPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            _navigationService = navigationService;
        }
        private INavigationService _navigationService;
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
        }
        #region  Property
        public Team _Team { get; set; }
        public JWTToken jWT { get; private set; }
        #endregion
    }
}
