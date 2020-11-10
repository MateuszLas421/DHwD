using Mapsui;
using Mapsui.Projection;
using Mapsui.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DHwD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {

        public MapPage()
        {
            InitializeComponent();
            var map = new Map
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
            mapView.Map = map;
        }
    }
}
