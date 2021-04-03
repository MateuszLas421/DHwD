using DHwD.ViewModels.GameInterface;
using System.Windows.Input;
using Xamarin.Forms;

namespace DHwD.Views
{
    public partial class ChatPage : ContentPage
    {
        public ICommand ScrollListCommand { get; set; }
        public ChatPage()
        {
            InitializeComponent();
            //this.BindingContext = BindingContext as ChatPageViewModel; 
        }
        public void ScrollTap(object sender, System.EventArgs e)
        {
            lock (new object())
            {
                if (BindingContext != null)
                {
                    var vm = BindingContext as ChatPageViewModel;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        while (vm.DelayedMessages.Count > 0)
                        {
                            vm.chat.Insert(0, vm.DelayedMessages.Dequeue());
                        }
                        vm.ShowScrollTap = false;
                        vm.LastMessageVisible = true;
                        vm.PendingMessageCount = 0;
                        ChatList?.ScrollToFirst();
                    });


                }

            }
        }
        public void OnListTapped(object sender, ItemTappedEventArgs e)
        {
            chatInput.UnFocusEntry();
        }
    }
}
