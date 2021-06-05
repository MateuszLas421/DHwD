using Mapsui;
using Mapsui.Projection;
using Mapsui.Utilities;
using Mapsui.UI.Forms;
using System;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using System.Threading;
using Xamarin.Forms;
using DHwD.Models;
using DHwD.Service;
using Prism.Services.Dialogs;
using DHwD.Tools;
using DHwD.ViewModels.Base;
using System.Collections.Generic;
using Models.ModelsDB;
using Models.ModelsMobile;
using Microsoft.AppCenter.Crashes;

namespace DHwD.ViewModels.GameInterface
{
    public class MapPageViewModel : GameBaseViewModel, INavigationAware
    {
        private MapView _mapView;
        private Mapsui.Map _map;
        private Xamarin.Essentials.Location _currentLocation;
        private readonly IDialogService _dialog;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private Task _initializingTask, _pinstask, _gpsTask;
        private RestService _restService;
        private List<Location> location;
        CancellationTokenSource cts;

        #region  Property
        public MobileTeam _Team { get; set; }
        public JWTToken jWT { get; set; }
        public MapView MyMap
        {
            get => _mapView;
            set => SetProperty(ref _mapView, value);
        }
        public Mapsui.Map Map
        {
            get => _map;
            set => SetProperty(ref _map, value);
        }
        public Xamarin.Essentials.Location CurrentLocation
        {
            get => _currentLocation;
            set => SetProperty(ref _currentLocation, value);
        }
        #endregion

        public MapPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IDialogService dialog)
            : base(navigationService)
        {
            _dialog = dialog;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _restService = new RestService();
            location = new List<Location>();
            Map = new Mapsui.Map
            {
                CRS = "EPSG:3857",
                Transformation = new MinimalTransformation()
            }; 
            var tileLayer = OpenStreetMap.CreateTileLayer();
            Map.Layers.Add(tileLayer);
            Map.Widgets.Add(new Mapsui.Widgets.ScaleBar.ScaleBarWidget(Map)
            {
                TextAlignment = Mapsui.Widgets.Alignment.Center,
                HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Left,
                VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Bottom
            });

            CurrentLocation = new Xamarin.Essentials.Location();
            //Map.Home = n => n.NavigateTo(new Mapsui.Geometries.Point(1019114.80157058, 5719580.75916194), Map.Resolutions[14]);
        }

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
            //MyMap.MyLocationFollow = true;
            var p = new NavigationParameters
            {
                    { "Team", _Team },
                    { "JWT", jWT }
            };
            Map.Home = n => n.NavigateTo(new Mapsui.Geometries.Point(1059114.80157058, 5179580.75916194), Map.Resolutions[14]);
            var timer = new Timer(async (e) =>
            {
                await GpsAsync();
                var coords = new Mapsui.UI.Forms.Position(CurrentLocation.Latitude, CurrentLocation.Longitude);
                Device.BeginInvokeOnMainThread(() =>
                {
                    MyMap.MyLocationLayer.UpdateMyLocation(coords);
                    //MyMap.MyLocationFollow = true;
                });
                for (int i = 0; i < MyMap.Pins.Count; i++)
                {
                    try
                    {
                        if (Distance(MyMap.Pins[i]) < 20)
                        {
                            var parametr = new NavigationParameters
                                   {
                                      { "Team", _Team },
                                      { "JWT", jWT },
                                      { "Location", location[i] }
                                   };
                            await _navigationService.GoBackAsync(parametr);
                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(4));
            _pinstask = GetPinsData(_Team, jWT);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            OnDisappearing();
        }

        public async Task GpsAsync()
        {
           var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Best, TimeSpan.FromSeconds(30));
            cts = new CancellationTokenSource();
            CurrentLocation = await Xamarin.Essentials.Geolocation.GetLocationAsync(request, cts.Token);              
        }

        public double Distance(Pin activepin)
        {
            var distance =  CalculateCoordinates.DistanceInKmBetweenEarthCoordinates(CurrentLocation.Latitude, CurrentLocation.Longitude,activepin.Position.Latitude,activepin.Position.Longitude);
            return distance;
        }
        protected void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
        }
        
        public async Task GetPinsData(Team team, JWTToken jWT)
        {
            location = await _restService.GetLocationAsync(jWT, team);

            for (int i = 0; i < location.Count; i++)
            {
                Pin activepin = new Pin(MyMap)
                {
                    Label = $"faas",
                    Scale = 1
                };
                activepin.Position = new Position(location[i].Latitude, location[i].Longitude);
                activepin.Callout.Anchor = new Point(0, activepin.Height * activepin.Scale);
                activepin.Label = location[i].Place.Name;

                MyMap.Pins.Add(activepin);
                activepin.ShowCallout();
            }
        }

    public void PinClicked(object sender, PinClickedEventArgs args)
        {  
            args.Handled = true;
            Location postlocation = new Location();

            foreach (var item in location)
            {
                if (item.Latitude == args.Pin.Position.Latitude && item.Longitude == args.Pin.Position.Longitude)
                    postlocation = item;
            }
            args.Pin.IsVisible = true;
            var parameters = new DialogParameters
            {
                { "title", "test!" },
                { "message", "test message" },
                { "JWT", jWT },
                { "location", postlocation }
            };
            _dialog.ShowDialogAsync("LocationDetailsDialog", parameters);
            args.Pin.Callout.CalloutClicked += (s, e) =>
            {
                args.Pin.Label = postlocation.Place.Name;         
                args.Pin.ShowCallout();
                return;
            };
            args.Pin.ShowCallout();
            return;
        }

    }
}
