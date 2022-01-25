using Prism;
using Prism.Ioc;
using DHwD.ViewModels;
using DHwD.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DHwD.Views.Dialog;
using DHwD.ViewModels.GameInterface;
using DHwD.ViewModels.Dialogs;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using DHwD.Service;
using DHwD.Views.Dialogs;
using DHwD.Repository.Interfaces;
using DHwD.Repository;
using DHwD.ViewModels.StartInterface;

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

        protected override void OnStart()
        {
            Url_data data = new Url_data();
            AppCenter.Start("android=" + data.AppCenterAndroid +";"  +
                  "ios="+ data.AppCenteriOS,
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Base Containers
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<TeamPageView, TeamPageViewViewModel>();
            containerRegistry.RegisterForNavigation<GameListView, GameListViewModel>();
            containerRegistry.RegisterForNavigation<CreateNewTeam, CreateNewTeamViewModel>();
            containerRegistry.RegisterForNavigation<JoinToTeamPassword, JoinToTeamPasswordViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginViewModel>();

            //Game Containers
            containerRegistry.RegisterForNavigation<StartPage, StartPageViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<ChatPage, ChatPageViewModel>();
            containerRegistry.RegisterForNavigation<MyTeamPage, MyTeamPageViewModel>();

            //Dialogs Containers
            containerRegistry.RegisterDialog<LocationDetailsDialog, LocationDetailsDialogViewModel>();
            containerRegistry.RegisterDialog<GameStartAlertDialog, GameStartAlertDialogViewModel>();

            // Register
            containerRegistry.Register<ILogs, LogsRepository>();
            containerRegistry.Register<IStorage, Storage>();
        }
    }
}
