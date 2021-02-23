using Prism;
using Prism.Ioc;
using DHwD.ViewModels;
using DHwD.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DHwD.Views.Dialog;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DHwD
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<TeamPageView, TeamPageViewViewModel>();
            containerRegistry.RegisterForNavigation<GameListView, GameListViewModel>();
            containerRegistry.RegisterForNavigation<CreateNewTeam, CreateNewTeamViewModel>();
            containerRegistry.RegisterForNavigation<JoinToTeamPassword, JoinToTeamPasswordViewModel>();
            containerRegistry.RegisterForNavigation<StartPage, StartPageViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<ChatPage, ChatPageViewModel>();
            containerRegistry.RegisterForNavigation<MyTeamPage, MyTeamPageViewModel>();
            containerRegistry.RegisterDialog<LocationDetailsDialog, LocationDetailsDialogViewModel>();   //.RegisterForNavigation<LocationDetailsDialog, LocationDetailsDialogViewModel>();
        }
    }
}
