using Microsoft.AppCenter.Crashes;
using Models.Respone;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DHwD.Service
{
    public static class BaseREST
    {
        static HttpClient client;

        public static async Task<BaseRespone> PostExecuteAsync<T>( string str, T item)  where T : class
        { 
            using (client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                BaseRespone response = new BaseRespone();
                string json = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                try { response = (BaseRespone)await client.PostAsync(str, content); }                                      //  POST  // 
                catch (Exception ex) { 
                    Debug.WriteLine(ex.Message.ToString());
                    Crashes.TrackError(ex);
                    return await Task.FromResult<BaseRespone>(response); 
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"successfully saved.");
                    return response;
                }
                return await Task.FromResult<BaseRespone>(response);
            }
        }

        //public static async Task<BaseRespone> GetExecuteAsync<T>(string str, T content) where T : HttpContent  // TODO 
        //{
        //    using (client = new HttpClient())
        //    {
        //        BaseRespone response = new BaseRespone();
        //        try { response = (BaseRespone)await client.GetAsync(str); }                                      //  Get  //TODO 
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine(ex.Message.ToString());
        //            Crashes.TrackError(ex);
        //            return await Task.FromResult<BaseRespone>(response);
        //        }

        //        if (response.IsSuccessStatusCode)
        //        {
        //            Debug.WriteLine(@"successfully saved.");
        //            return response;
        //        }
        //        return await Task.FromResult<BaseRespone>(response);
        //    }
        //}
    }
}
