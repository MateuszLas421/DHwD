using DHwD.Models;
using Microsoft.AppCenter.Crashes;
using Models.Request;
using Models.Respone;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DHwD.Service
{
    public static class BaseREST
    {
        static HttpClient client;

        public static async Task<TOut> PostExecuteNoAuthAsync<T,TOut>( string str, T item)  where T : class    // TO Check
        { 
            using (client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                HttpResponseMessage response = new HttpResponseMessage();
                string json = JsonConvert.SerializeObject(item);
                string responseBody;
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    response = await client.PostAsync(str, content);   //  POST  // 
                    responseBody = await response.Content.ReadAsStringAsync();
                }                                     
                catch (Exception ex)
                { 
                    Debug.WriteLine(ex.Message.ToString());
                    Crashes.TrackError(ex);
                    return await Task.FromResult<TOut>(JsonConvert.DeserializeObject<TOut>(null)); 
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"successfully saved.");
                    return await Task.FromResult<TOut>(JsonConvert.DeserializeObject<TOut>(responseBody));
                }
                return await Task.FromResult<TOut>(JsonConvert.DeserializeObject<TOut>(null));
            }
        }

        public static async Task<TOut> PostExecuteAsync<T,TOut>(string str, JWTToken jWT, T item) where T : class
        {
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })      //  POST  // 
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var serialized = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
#if DEBUG
                var test = JsonConvert.SerializeObject(item);
#endif
                using (HttpResponseMessage response = await client.PostAsync(str, serialized))
                {
                    try
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            Debug.WriteLine(@"successfully saved.");
                            return await Task.FromResult(JsonConvert.DeserializeObject<TOut>(responseBody));
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message.ToString());
                        Crashes.TrackError(ex);
                    }
                }
            }
            return await Task.FromResult(JsonConvert.DeserializeObject<TOut>(null));
        }

        ///// <summary>
        ///// Rest Get Method
        ///// </summary>
        ///// <param name="getRequest"></param>
        ///// <returns>BaseRespone</returns>
        //public static async Task<BaseRespone> GetExecuteAsync(GetRequest getRequest) 
        //{
        //    using (client = new HttpClient())
        //    {
        //        BaseRespone response = new BaseRespone();
        //        try { response = (BaseRespone)await client.GetAsync(getRequest.strURL); }                 //  Get  
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

        /// <summary>
        /// Rest Get Method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getRequest"></param>
        /// <returns></returns>
        public static async Task<T> GetExecuteAsync<T>(JWTToken jWT, GetRequest getRequest) where T : class
        {
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                HttpResponseMessage response;
                string responseContent;
                try
                {
                    response =  await client.GetAsync(getRequest.strURL);                       //  Get  
                    responseContent = await response.Content.ReadAsStringAsync();
                }                
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message.ToString());
                    Crashes.TrackError(ex);
                    return await Task.FromResult(JsonConvert.DeserializeObject<T>(null));
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"successfully saved.");
                    return await Task.FromResult<T>(JsonConvert.DeserializeObject<T>(responseContent));
                }
                return await Task.FromResult<T>(JsonConvert.DeserializeObject<T>(responseContent));
            }
        }
    }
}
