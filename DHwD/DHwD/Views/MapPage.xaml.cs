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
        }
        protected override void OnAppearing()
        {
            var vm = BindingContext as MapPageViewModel;
            vm.MyMap = MyMap;
            this.CurrentLocation = vm.CurrentLocation;
            MyMap.Map = vm.Map;
        }
    }
}
