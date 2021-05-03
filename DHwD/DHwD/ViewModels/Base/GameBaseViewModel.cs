using Plugin.Geolocator.Abstractions;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DHwD.ViewModels.Base
{
    public class GameBaseViewModel : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        private Position _currentLocation;
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public Position CurrentLocation
        {
            get => _currentLocation;
            set => SetProperty(ref _currentLocation, value);
        }

        public GameBaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
        //public async Task TaskLocation()  // TODO
        //{
        //    var startTimeSpan = TimeSpan.Zero;
        //    var periodTimeSpan = TimeSpan.FromSeconds(3);
        //    var timer = new Timer(async (e) =>
        //    {
        //        await Gps();
        //        for (i = 0; i < MyMap.Pins.Count; i++)
        //        {
        //            if (await Distance(MyMap.Pins[i]) < 100)
        //            {
        //            }
        //        }
        //        if (tick < 5)
        //        {
        //            MyMap.MyLocationFollow = true;
        //            tick++;
        //        }
        //    }, null, startTimeSpan, periodTimeSpan);
        //}
        //public async Task Gps()
        //{
        //    var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Best);
        //    var location = await Xamarin.Essentials.Geolocation.GetLocationAsync(request);
        //    CurrentLocation.Latitude = location.Latitude;
        //    CurrentLocation.Longitude = location.Longitude;
        //    var coords = new Mapsui.UI.Forms.Position(CurrentLocation.Latitude, CurrentLocation.Longitude);
        //}
    }
}
