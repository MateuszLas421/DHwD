using DHwD.Models;
using DHwD.ViewModels.Base;
using Models.ModelsDB;
using Models.ModelsMobile;
using Prism.Navigation;
using Prism.Services;

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
                _Team = parameters.GetValue<MobileTeam>("Team");
            }
            if (parameters.ContainsKey("JWT"))
            {
                jWT = parameters.GetValue<JWTToken>("JWT");
            }
        }
        #region  Property
        public MobileTeam _Team { get; set; }
        public JWTToken jWT { get; private set; }
        #endregion
    }
}
