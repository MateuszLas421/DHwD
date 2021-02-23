using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DHwD.ViewModels
{
    public class LocationDetailsDialogViewModel : BindableBase, IDialogAware
    {
        public LocationDetailsDialogViewModel()
        {
            CloseCommand = new DelegateCommand(() => RequestClose(null));
        }


        private string title = "Alert";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

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
            // If not using IAutoInitialize you would need to set the Message property here.
            // Message = parameters.GetValue<string>("message");
        }
    }
}
