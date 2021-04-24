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
using Mapsui.Rendering.Skia;
using Prism.Services.Dialogs;
using DHwD.Views.Dialog;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using DHwD.Tools;
using DHwD.Models.REST;
using DHwD.ViewModels.Base;
using System.Collections.Generic;

namespace DHwD.ViewModels.GameInterface
{
    public class MapPageViewModel : GameBaseViewModel, INavigationAware
    {
        private MapView _mapView;
        private Mapsui.Map _map;
        private Plugin.Geolocator.Abstractions.Position _currentLocation;
        private readonly IDialogService _dialog;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private Task _initializingTask, _pinstask, _gpsTask;
        private RestService _restService;

        #region  Property
        public Team _Team { get; set; }
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
        public Plugin.Geolocator.Abstractions.Position CurrentLocation
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
            CurrentLocation = new Plugin.Geolocator.Abstractions.Position();

            Map.Home = n => n.NavigateTo(new Mapsui.Geometries.Point(1059114.80157058, 5179580.75916194), Map.Resolutions[14]);
        }

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
            var p = new NavigationParameters
                {
                    { "Team", _Team },
                    { "JWT", jWT }
                };

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(3);
            int i;
            var timer = new Timer(async (e) =>
            {
                await Gps();
                for (i = 0; i < MyMap.Pins.Count; i++)
                {
                    if (await Distance(MyMap.Pins[i]) < 100)
                    {
                        await _navigationService.GoBackAsync();
                    }
                }
            }, null, startTimeSpan, periodTimeSpan);

            _gpsTask = Gps();
            _pinstask = GetPinsData(_Team, jWT);
        }

        public async Task Gps()
        {
            var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Best);
            var location = await Xamarin.Essentials.Geolocation.GetLocationAsync(request);
            CurrentLocation.Latitude = location.Latitude;
            CurrentLocation.Longitude = location.Longitude;
            var coords = new Mapsui.UI.Forms.Position(CurrentLocation.Latitude, CurrentLocation.Longitude);
            MyMap.MyLocationLayer.UpdateMyLocation(coords);
            //MyMap.MyLocationFollow = true;                 /// Check map.Home = n => n.NavigateT
        }

        public async Task<double> Distance(Pin activepin)
        {
            var distance =  CalculateCoordinates.DistanceInKmBetweenEarthCoordinates(CurrentLocation.Latitude, CurrentLocation.Longitude,activepin.Position.Latitude,activepin.Position.Longitude);
            return await Task.FromResult<double>(distance);
        }



        public async Task GetPinsData(Team team, JWTToken jWT)
        {
            await Task.Run(async () =>
            {
                List<Location> location = new List<Location>();
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


            });
        }
        //public static IObservable<T> CheckLocation<T>(T value)  // delete
        //{
        //    return Observable.Create<T>(o =>
        //    {
        //        o.OnNext(value);
        //        o.OnCompleted();
        //        return Disposable.Empty;
        //    });
        //}

        public void PinClicked(object sender, PinClickedEventArgs args)
        {
            //var mapView = (MapView)sender;
            args.Pin.Label = "click";
            args.Pin.IsVisible = true;
            var parameters = new DialogParameters
            {
                { "title", "test!" },
                { "message", "test message" }
            };
            _dialog.ShowDialog("LocationDetailsDialog", parameters);
            args.Pin.Callout.CalloutClicked += (s, e) =>
            {
                args.Pin.Label = "You clicked me!";
                e.Handled = true;            
                args.Pin.ShowCallout();
                return;
            };
            args.Pin.ShowCallout();
            return;
        }

    }
}
