using DHwD.ViewModels.GameInterface;
using System;
using Xamarin.Forms;

namespace DHwD.Views.Custom
{
    public partial class InputBar : ContentView
    {
        public InputBar()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.SetBinding(HeightRequestProperty, new Binding("Height", BindingMode.OneWay, null, null, null, chatTextInput));
            }
        }
        public void Handle_Completed(object sender, EventArgs e)
        {
            chatTextInput.Focus();
            chatTextInput.Text = "";
        }

        public void UnFocusEntry()
        {
            chatTextInput?.Unfocus();
        }
    }
}