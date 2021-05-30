using DHwD.ViewModels.GameInterface;
using System.Windows.Input;
using Xamarin.Forms;

namespace DHwD.Views
{
    public partial class ChatPage : ContentPage
    {
        public ICommand ScrollListCommand { get; set; }

        private ChatPageViewModel vm;
        public ChatPage()
        {
            InitializeComponent();
            vm = BindingContext as ChatPageViewModel;
        }
        public void ScrollTap(object sender, System.EventArgs e)
        {
            lock (new object())
            {
                if (BindingContext != null)
                {
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
