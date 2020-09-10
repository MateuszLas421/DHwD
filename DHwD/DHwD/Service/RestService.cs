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
        public async Task<bool> CheckUserExistsAsync(UserRegistration item)
        {
            bool isNewItem = false;
            item.NickName = "TestData"; item.Token = "sdadsfs-gsfg-rgdsads64-c5rsd";    //delete
            Url_data url_ = null;
            HttpResponseMessage response = null;
            try { url_ = new Url_data(); }
            catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return isNewItem; }
            Uri uri;
            try { uri = new Uri(string.Format(url_.CheckLogin + item.NickName + "/" + item.Token)); }
            catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return isNewItem; }
           // if (isNewItem==false)
           // {
                try { response = await client.GetAsync(uri.ToString()); }
                catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return isNewItem; }
            //}
            if (response.IsSuccessStatusCode)  /// The user exists
            {
                //string responseContent = await response.Content.ReadAsStringAsync();
                //var t= JsonConvert.DeserializeObject<User>(responseContent);              //////// //////////////////////////////////////////////////////////TODO
                Debug.WriteLine(@" The user exists. ");
                isNewItem=true;
                return isNewItem;

            }
            return isNewItem;
        }
        public async Task RegisterNewUserAsync(UserRegistration item, bool isNewItem = true)//isNewItem = false
        {
            Url_data url_=null;
            HttpResponseMessage response = null;
            string json = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            try { url_ = new Url_data(); }
            catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return; } 
            Uri uri;

            try { uri = new Uri(string.Format(url_.RegisterUri.ToString(), string.Empty)); } 
            catch (Exception ex) { Debug.WriteLine(ex.Message);  return; }
            
            response = null;

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
