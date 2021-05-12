using Xamarin.Forms;

namespace DHwD.Views
{
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed() => true;   //Turn off 
    }
}
