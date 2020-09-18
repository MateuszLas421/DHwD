using DHwD.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace DHwD.ViewModels
{
    public class TeamPageViewViewModel : BindableBase
    {
        ObservableCollection<Team> Teams;
        HttpClient httpClient;
        public TeamPageViewViewModel()
        {
           
        }
    }
}
