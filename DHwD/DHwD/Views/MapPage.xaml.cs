using DHwD.ViewModels;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DHwD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public Position CurrentLocation { get; set; }
        public MapPage()
        {
            InitializeComponent();
            var vm = BindingContext as MapPageViewModel;
            vm.MyMap = MyMap;
            this.CurrentLocation = vm.CurrentLocation;
            MyMap.PinClicked += vm.OnClick;
            MyMap.Map = vm.Map;

        }
        protected override void OnAppearing()
        {

        }
    }
}
