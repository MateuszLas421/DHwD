using DHwD.Interface;
using DHwD.Model;
using Newtonsoft.Json;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DHwD.Service
{
    class RestService: IRestService
    {
        HttpClient client;
        IPageDialogService _dialogService;
        public RestService(IPageDialogService dialogService)
        {
            client = new HttpClient();
            _dialogService = dialogService;
        }
        public async Task RegisterNewUserAsync(User item, bool isNewItem = true)//isNewItem = false
        {
            //Debug.WriteLine("doItem successfully 1.");   ///1
            Url_data url_=null;
            try { url_ = new Url_data(); }
            catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); } 
            Uri uri;
            try { uri = new Uri(string.Format(url_.RegisterUri.ToString(), string.Empty)); /*Debug.WriteLine("doItem successfully 2.");*/ }  ///2 
            catch (Exception ex) { await _dialogService.DisplayAlertAsync("Alert", ex.Message.ToString(), "OK"); return; }
           // Debug.WriteLine("doItem successfully 3.");   ///3

            string json = JsonConvert.SerializeObject(item);                                                 // JSON
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            if (isNewItem)
            {
                try { response = await client.PostAsync(uri, content); }                                      //  POST  //            TODO !!!!! check
                catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); /*await _dialogService.DisplayAlertAsync("Alert", "You have an unknown registration error.", "OK"); */ return; }
            }
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"successfully saved.");
            }
        }
    }
}
