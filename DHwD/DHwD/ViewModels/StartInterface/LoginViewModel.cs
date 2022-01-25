using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DHwD.ViewModels.StartInterface
{
    public class LoginViewModel : ViewModelBase, INavigationAware
    {

        #region variables
        private INavigationService _navigationService;
        #endregion
        public LoginViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
