using Mapsui;
using Mapsui.Projection;
using Mapsui.Utilities;
using Mapsui.UI.Forms;
using System;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using System.Threading;
using Xamarin.Forms;
using DHwD.Models;

namespace DHwD.ViewModels
{
    public class MapPageViewModel : ViewModelBase, INavigationAware
    {
        private MapView _mapView;
        private Mapsui.Map _map;
        private Plugin.Geolocator.Abstractions.Position _currentLocation;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private Task _initializingTask, _pinstask, _gpsTask;

        public MapPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            Map = new Mapsui.Map();
            var map = new Mapsui.Map
            {
                CRS = "EPSG:3857",
                Transformation = new MinimalTransformation()
            };

            var tileLayer = OpenStreetMap.CreateTileLayer();
            map.Layers.Add(tileLayer);
            map.Widgets.Add(new Mapsui.Widgets.ScaleBar.ScaleBarWidget(map)
            {
                TextAlignment = Mapsui.Widgets.Alignment.Center,
                HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Left,
                VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Bottom
            });
            Map = map;

            _pinstask = GetPinsData();
        }
        #region  Property
        public Team _Team { get; set; }
        public JWTToken jWT { get; set; }
        #endregion
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
            var timer = new Timer(async (e) =>
            {
                await Gps();
            }, null, startTimeSpan, periodTimeSpan);
            _gpsTask = Gps();
        }

        public async Task Gps()
        {
            var request = new Xamarin.Essentials.GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);
            var coords = new Mapsui.UI.Forms.Position(location.Latitude, location.Longitude);
            MyMap.MyLocationLayer.UpdateMyLocation(coords);
            MyMap.MyLocationFollow=true;
        }
     
        public Plugin.Geolocator.Abstractions.Position CurrentLocation
        {
            get => _currentLocation;
            set => SetProperty(ref _currentLocation, value);
        }
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
        public async Task GetPinsData()
        {
            await Task.Run(async () =>
            {
                Pin activepin = new Pin (MyMap)
                {
                    Label = $"fsdfdsfdsf",
                    Scale = 1
                };

                activepin.Callout.Anchor = new Point(0, activepin.Height * activepin.Scale);
                //activepin.Callout.
                MyMap.Pins.Add(activepin);
                activepin.ShowCallout();
            });
        }
    }
}
