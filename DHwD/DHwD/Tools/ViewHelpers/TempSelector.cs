using DHwD.Views.Custom;
using Models.ModelsDB;
using Xamarin.Forms;

namespace DHwD.Tools.ViewHelpers
{
    public class TempSelector : DataTemplateSelector
    {
        DataTemplate IncomingMessageVCDT;
        DataTemplate SentMessageVCDT;

        public TempSelector()
        {
            this.IncomingMessageVCDT = new DataTemplate(typeof(IncomingMessageVC));
            this.SentMessageVCDT = new DataTemplate(typeof(SentMessageVC));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var message = item as Chats;
            if (message == null)
                return null;


            return (message.IsSystem == true) ? SentMessageVCDT : IncomingMessageVCDT;
        }
    }
}
