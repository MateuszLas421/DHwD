using DHwD.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DHwD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeamPageView : ContentPage
    {
        TeamPageViewViewModel vm;
        public TeamPageView()
        {
            InitializeComponent();
            vm = BindingContext as TeamPageViewViewModel;
        }

        private void Create_Clicked(object sender, System.EventArgs e)
        {
            vm.CreateTeamCommand(sender,e);
        }
    }
}
