using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DHwD.ViewModels
{
    public class ChatPageViewModel : ViewModelBase
    {
        public ChatPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            _navigationService = navigationService;
        }
        private INavigationService _navigationService;
    }
}
