using DHwD.ViewModels.GameInterface;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DHwD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPageViewModel VM;
        public Xamarin.Essentials.Location CurrentLocation { get; set; }
        public MapPage()
        {
            InitializeComponent();
            VM = BindingContext as MapPageViewModel;
            VM.MyMap = MyMap;
            this.CurrentLocation = VM.CurrentLocation;
            MyMap.PinClicked += VM.PinClicked;
            MyMap.Map = VM.Map;

        }
        protected override void OnAppearing()
        {

        }

        /* private void MyMap_PinClicked(object sender, Mapsui.UI.Forms.PinClickedEventArgs e)
        {
            VM.PinClicked(sender, e);
            e.Handled = true;
        }*/
    }
}
