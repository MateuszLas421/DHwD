using DHwD.Service;
using Models.ModelsDB;
using Models.ModelsMobile.Common;
using Models.Request;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace DHwD.ViewModels.Dialogs
{
    public class LocationDetailsDialogViewModel : BindableBase, IDialogAware
    {
        public LocationDetailsDialogViewModel()
        {
            CloseCommand = new DelegateCommand(() => RequestClose(null));
            Place = new Place();
            Place.Location = new Location();
        }

        private string title = "Alert";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public Place Place
        {
            get => place;
            set => SetProperty(ref place, value);
        }

        private Place place;

        public JWTToken jWT { get; set; }

        private string message;

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public DelegateCommand CloseCommand { get; }

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            Console.WriteLine("The Demo Dialog has been closed...");
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("message"))
            {
                Message = parameters.GetValue<string>("message");
            }
            if (parameters.ContainsKey("title"))
            {
                Title = parameters.GetValue<string>("title");
            };
            if (parameters.ContainsKey("JWT"))
            {
                jWT = parameters.GetValue<JWTToken>("JWT");
            };
            if (parameters.ContainsKey("location"))
            {
                Place.Location = parameters.GetValue<Location>("location");
            };
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                Observable.Create<Task>(async o => await GetPlace(Place.Location.ID))
                .SubscribeOn(Scheduler.CurrentThread)
                .Subscribe();
            });
        }

        public async Task GetPlace(int IdLocation)
        {
            Url_data url_Data = new Url_data();
            GetRequest getRequest = new GetRequest(url_Data.PlacebyLocation.ToString());
            getRequest = await PrepareGetRequest.AddOnlyValue(getRequest, IdLocation.ToString());

            Place = await BaseREST.GetExecuteAsync<Place>(jWT, getRequest);
        }
    }
}
