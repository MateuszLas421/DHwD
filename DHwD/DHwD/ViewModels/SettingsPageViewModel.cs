using Prism.Navigation;
using Prism.Services;

namespace DHwD.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            _navigationService = navigationService;
        }
        private INavigationService _navigationService;
    }
}
